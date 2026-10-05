using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Druid;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Packages.Laima.Pads.Clerics.Druid
{
	[Package("laima")]
	[PadHandler("GroundAura_GrowingGrass_Green_01")]
	public class Druid_Chortasmata_Pad : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float AreaRange = 100f;
		private const int MaximumEnemyTargets = 9;
		private const int UpdateIntervalMilliseconds = 1000;
		private const int RashDurationMilliseconds = 20000;
		private const int FloralScentRefreshMilliseconds = 2000;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator as Character;
			var skillLevel = Math.Clamp(pad.Skill.Level, 1, 10);
			var durationSeconds = 10f + (skillLevel - 1) * (6f / 9f);

			if (caster != null)
				durationSeconds += Druid_ChortasmataDurationAbility.GetDurationBonus(caster);

			pad.SetRange(AreaRange);
			pad.SetUpdateInterval(UpdateIntervalMilliseconds);
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(durationSeconds);
			pad.Trigger.MaxUseCount = int.MaxValue;
			pad.Trigger.MaxActorCount = int.MaxValue;
			Send.ZC_NORMAL.PadUpdate(pad, true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator as Character;

			if (caster == null || caster.IsDead || caster.Map == null || pad.IsDead)
				return;

			this.DamageEnemies(pad, caster);
			this.ApplyFloralScent(pad, caster);
		}

		private void DamageEnemies(Pad pad, Character caster)
		{
			var area = new Melia.Zone.Skills.SplashAreas.Circle(pad.Position, AreaRange);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => pad.Position.Get2DDistance(target.Position))
				.Take(MaximumEnemyTargets)
				.ToList();

			var hits = new List<SkillHitInfo>();
			var enhanceMultiplier = Druid_ChortasmataEnhanceAbility.GetDamageMultiplier(caster);

			foreach (var target in targets)
			{
				var modifier = SkillModifier.Default;
				modifier.DamageMultiplier *= enhanceMultiplier;

				var result = SCR_SkillHit(caster, target, pad.Skill, modifier);

				if (result.Result != HitResultType.Dodge && result.Damage > 0)
				{
					target.TakeDamage(result.Damage, caster);
					target.StartBuff(BuffId.Chortasmata_Debuff, pad.Skill.Level, 0, TimeSpan.FromMilliseconds(RashDurationMilliseconds), caster, pad.Skill.Id);
				}

				hits.Add(new SkillHitInfo(caster, target, pad.Skill, result, HitAnimationTime, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		private void ApplyFloralScent(Pad pad, Character caster)
		{
			var party = caster.Connection?.Party;
			var allies = caster.Map.GetActorsInRange<Character>(pad.Position, AreaRange, target =>
			{
				if (target == null || target.IsDead)
					return false;

				if (target == caster)
					return true;

				return party != null && target.Connection?.Party == party;
			});

			var skillLevel = Math.Clamp(pad.Skill.Level, 1, 10);
			var healingFactor = 41f + (skillLevel - 1) * (69f / 9f);

			foreach (var ally in allies)
				ally.StartBuff(BuffId.Chortasmata_Buff, healingFactor, 0, TimeSpan.FromMilliseconds(FloralScentRefreshMilliseconds), caster, pad.Skill.Id);
		}
	}
}
