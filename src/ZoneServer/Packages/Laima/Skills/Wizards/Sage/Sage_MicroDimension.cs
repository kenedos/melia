using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Logging;
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Packages.Laima.Abilities.Wizards.Sage;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.Sage
{
	[Package("laima")]
	[SkillHandler(SkillId.Sage_MicroDimension)]
	public class Sage_MicroDimensionOverride : IGroundSkillHandler
	{
		private const float AreaRadius = 30f;
		private const float AfterEffectRadius = 60f;
		private const int AfterEffectDelayMilliseconds = 1500;
		private static readonly HashSet<int> DuplicatePadHandles = new();

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			var targetPosition = GetTargetPosition(skill, farPos);

			if (!character.InSkillUseRange(skill, targetPosition))
				return;

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			character.SetAttackState(true);
			character.TurnTowards(targetPosition);

			SageRuptureHelper.AddCompletedCast(character);

			var origin = character.Position;

			Send.ZC_SKILL_READY(character, skill, origin, targetPosition);
			Send.ZC_NORMAL.UpdateSkillEffect(character, 0, origin, origin.GetDirection(targetPosition), Position.Zero);

			var hits = new List<SkillHitInfo>();
			var initialTargets = AttackArea(character, skill, targetPosition, hits);

			// Temporarily disabled until recursive pad duplication is fully resolved.
			//if (character.TryGetActiveAbility(AbilityId.Sage8, out _))
			//	DuplicateInstallation(character, targetPosition);

			if (character.TryGetActiveAbility(AbilityId.Sage11, out _) && initialTargets.Count > 0)
				skill.Run(HandleAfterEffect(character, skill, initialTargets));

			if (Versions.Protocol > 500)
				Send.ZC_SKILL_MELEE_GROUND(character, skill, targetPosition, hits);
			else
			{
				Send.ZC_SKILL_MELEE_GROUND(character, skill, targetPosition);
				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(character, hits);
			}

			skill.IncreaseOverheat();
			character.SetAttackState(false);
		}

		private static Position GetTargetPosition(Skill skill, Position farPos)
		{
			if (skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPosition))
				return targetPosition;

			return farPos;
		}

		private static List<ICombatEntity> AttackArea(Character character, Skill skill, Position center, List<SkillHitInfo> hits)
		{
			var hitTargets = new List<ICombatEntity>();

			if (character.Map == null)
				return hitTargets;

			var targets = character.Map.GetAttackableEnemiesIn(character, new CircleF(center, AreaRadius)).ToList();
			var damageMultiplier = Sage_MicroDimensionEnhanceAbility.GetDamageMultiplier(character);

			foreach (var target in targets)
			{
				if (target.IsDead || target.Map != character.Map)
					continue;

				var result = SCR_SkillHit(character, target, skill, SkillModifier.Default);
				result.Damage *= damageMultiplier;
				target.TakeDamage(result.Damage, character);

				hits.Add(new SkillHitInfo(character, target, skill, result, TimeSpan.Zero, TimeSpan.Zero));
				hitTargets.Add(target);
			}

			return hitTargets;
		}

		private static async Task HandleAfterEffect(Character character, Skill skill, List<ICombatEntity> markedTargets)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(AfterEffectDelayMilliseconds));

			if (character.IsDead || character.Map == null)
				return;

			var damagedTargets = new HashSet<ICombatEntity>();
			var hits = new List<SkillHitInfo>();
			var damageMultiplier = Sage_MicroDimensionEnhanceAbility.GetDamageMultiplier(character);

			foreach (var markedTarget in markedTargets)
			{
				if (markedTarget == null || markedTarget.IsDead || markedTarget.Map != character.Map)
					continue;

				var explosionCenter = markedTarget.Position;
				var targets = character.Map.GetAttackableEnemiesIn(character, new CircleF(explosionCenter, AfterEffectRadius)).ToList();

				foreach (var target in targets)
				{
					if (target.IsDead || target.Map != character.Map)
						continue;

					if (!damagedTargets.Add(target))
						continue;

					var result = SCR_SkillHit(character, target, skill, SkillModifier.Default);
					result.Damage *= damageMultiplier;
					target.TakeDamage(result.Damage, character);

					hits.Add(new SkillHitInfo(character, target, skill, result, TimeSpan.Zero, TimeSpan.Zero));
				}
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(character, hits);
		}

		private static void DuplicateInstallation(Character character, Position center)
		{
			if (character.Map == null)
				return;

			DuplicatePadHandles.RemoveWhere(handle =>
				!character.Map.TryGetPad(handle, out var duplicatePad) ||
				duplicatePad == null ||
				duplicatePad.IsDead);

			var originals = character.Map.GetPadsAt(center, AreaRadius)
				.Where(IsDuplicableInstallation)
				.OrderBy(pad => pad.Position.Get2DDistance(center))
				.ToList();

			if (originals.Count == 0)
			{
				Log.Debug($"[Sage Micro Dimension] Duplicate: no compatible installation found. Sage={character.Name}");
				return;
			}

			foreach (var original in originals)
			{
				if (original.Skill == null)
					continue;

				if (original.Creator is not ICombatEntity originalCreator)
				{
					Log.Debug($"[Sage Micro Dimension] Duplicate: invalid creator. Pad={original.Name}");
					continue;
				}

				var duplicate = SkillCreatePad(originalCreator, original.Skill, original.Position, original.Direction.DegreeAngle, original.Name, false);

				if (duplicate == null)
				{
					Log.Debug($"[Sage Micro Dimension] Duplicate: failed to create pad. Pad={original.Name}, Skill={original.Skill.Id}");
					continue;
				}

				DuplicatePadHandles.Add(duplicate.Handle);

				duplicate.NumArg1 = original.NumArg1;
				duplicate.NumArg2 = original.NumArg2;
				duplicate.NumArg3 = original.NumArg3;
				duplicate.Trigger.LifeTime = original.Trigger.LifeTime;
				duplicate.Trigger.UpdateInterval = original.Trigger.UpdateInterval;
				duplicate.Trigger.MaxActorCount = original.Trigger.MaxActorCount;
				duplicate.Trigger.MaxUseCount = original.Trigger.MaxUseCount;
				duplicate.Activate();

				Log.Debug($"[Sage Micro Dimension] Duplicate SUCCESS: Sage={character.Name}, Pad={original.Name}, OriginalHandle={original.Handle}, DuplicateHandle={duplicate.Handle}, Creator={originalCreator.Handle}, Skill={original.Skill.Id}, NumArg1={original.NumArg1}, NumArg2={original.NumArg2}, NumArg3={original.NumArg3}, LifeTime={original.Trigger.LifeTime}, UpdateInterval={original.Trigger.UpdateInterval}");
			}
		}

		private static bool IsDuplicableInstallation(Pad pad)
		{
			if (pad == null || pad.Creator == null || pad.Skill == null)
				return false;

			if (DuplicatePadHandles.Contains(pad.Handle))
				return false;

			if (pad.Skill.Id == SkillId.Sage_MicroDimension ||
				pad.Skill.Id == SkillId.Sage_UltimateDimension ||
				pad.Skill.Id == SkillId.Sage_DimensionCompression ||
				pad.Skill.Id == SkillId.Sage_HoleOfDarkness)
				return false;

			return true;
		}
	}
}
