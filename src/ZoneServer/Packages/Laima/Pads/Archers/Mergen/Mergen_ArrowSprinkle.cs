using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.Packages.Laima.Skills.Archers.Mergen;

namespace Melia.Zone.Packages.Laima.Pads.Archers.Mergen
{
	[Package("laima")]
	[PadHandler(PadName.Mergen_ArrowRain)]
	public class Mergen_ArrowSprinklePadOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int MaximumEnhanceLevel = 100;
		private const int MaximumTargetsPerTick = 10;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.Trigger.UpdateInterval = TimeSpan.FromMilliseconds(300);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var skill = pad.Skill;

			if (caster == null || caster.IsDead || skill == null)
			{
				pad.Destroy();
				return;
			}

			var targets = caster.Map
				.GetAttackableEnemiesIn(caster, pad.Area)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => pad.Position.Get2DDistance(target.Position))
				.Take(MaximumTargetsPerTick)
				.ToList();

			var damageMultiplier = this.GetEnhanceMultiplier(caster);
			damageMultiplier *= MergenZenithHelper.GetDamageMultiplier(caster);

			foreach (var target in targets)
			{
				var modifier = SkillModifier.Default;
				modifier.DamageMultiplier *= damageMultiplier;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

				if (skillHitResult.Result == HitResultType.Dodge)
					continue;

				target.TakeDamage(skillHitResult.Damage, caster);

				var hitInfo = new HitInfo(
					caster,
					target,
					skill,
					skillHitResult.Damage,
					skillHitResult.Result
				);

				Send.ZC_HIT_INFO(caster, target, hitInfo);
			}
		}

		private float GetEnhanceMultiplier(ICombatEntity caster)
		{
			if (caster is not Character character || !character.TryGetAbility(AbilityId.Mergen6, out var ability))
				return 1f;

			var level = Math.Clamp(ability.Level, 0, MaximumEnhanceLevel);
			var bonusPercent = level * 0.5f;

			if (level >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + (bonusPercent / 100f);
		}
	}
}
