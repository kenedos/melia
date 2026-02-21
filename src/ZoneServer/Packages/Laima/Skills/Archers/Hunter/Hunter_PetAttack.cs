using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.World.Maps;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors.CombatEntities.Components;
using System.Collections.Generic;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Handlers.Hunter
{
	/// <summary>
	/// Handler for the Hunter skill Pet Attack.
	/// Active: Commands companion to dash and attack multiple targets.
	/// Passive: Gives companion a chance to auto-cast Coursing or Retrieve effects on attacks.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hunter_PetAttack)]
	public class Hunter_PetAttackOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		/// <summary>
		/// Passive handler - called when skill is learned or becomes active.
		/// The passive effect is handled via the companion attack hook.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster)
		{
			// Passive effect is handled via OnCompanionAttackAfterCalc hook
		}

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TryGetActiveCompanion(out var companion))
			{
				if (caster is Character character)
					character.SystemMessage("CompanionIsNotActive");
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(Activate(caster, skill, targetPos, companion));
		}

		private static async Task Activate(ICombatEntity caster, Skill skill, Position targetPos, World.Actors.Monsters.Companion companion)
		{
			var area = 50f;

			await skill.Wait(TimeSpan.FromMilliseconds(400));

			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, area);
			var targetList = targets.Take(3 + skill.Level);

			if (targets.Count > 0)
				if (companion.Components.TryGet<AiComponent>(out var ai))
					ai.Script.QueueEventAlert(new HateResetAlert());

			var damageDelay = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			var firstTarget = true;
			var hits = new List<SkillHitInfo>();
			foreach (var target in targetList)
			{
				if (!companion.IsDead && companion.CanSee(target))
				{
					companion.SetAttackState(true);

					// Dashes to first target (closest to targetting area)
					if (firstTarget)
					{
						companion.InsertHate(target, 500);
						firstTarget = false;

						var syncKey = companion.GenerateSyncKey();
						Send.ZC_NORMAL.CollisionAndBack(companion, target, syncKey, "ATK", 0.3f, 5f, 0.3f, 1f, 20f, false);
						Send.ZC_NORMAL.AttachEffect(companion, "I_dash037", 1.2f, EffectLocation.Middle);
						Send.ZC_NORMAL.DetachEffect(companion, "I_dash037");

						var newPos = target.Position.GetRelative(target.GetDirection(companion), 20f);
						companion.Position = newPos;
						companion.Direction = companion.GetDirection(target);
					}
					else
						companion.InsertHate(target, 300);


					var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(2));
					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
					skillHit.HitEffect = HitEffect.Impact;

					hits.Add(skillHit);
				}
			}


			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		/// <summary>
		/// Attempts to activate Hunter_PetAttack from companion AI.
		/// </summary>
		/// <param name="master">The companion's owner/master</param>
		/// <param name="companion">The companion entity</param>
		/// <param name="target">The target to attack</param>
		public static void TryActivate(ICombatEntity master, World.Actors.Monsters.Companion companion, ICombatEntity target)
		{
			if (!master.TryGetSkill(SkillId.Hunter_PetAttack, out var skill))
				return;

			if (skill.IsOnCooldown || target.IsDead)
				return;

			if (companion.IsDead)
				return;

			if (!master.InSkillUseRange(skill, target))
				return;

			if (!master.TrySpendSp(skill))
				return;

			skill.IncreaseOverheat();

			var targetPos = target.Position;
			Send.ZC_SKILL_READY(master, skill, 1, companion.Position, targetPos);
			Send.ZC_NORMAL.UpdateSkillEffect(master, target.Handle, companion.Position, companion.Position.GetDirection(targetPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(master, skill, targetPos, ForceId.GetNew(), null);

			skill.Run(Activate(master, skill, targetPos, companion));
		}
	}
}
