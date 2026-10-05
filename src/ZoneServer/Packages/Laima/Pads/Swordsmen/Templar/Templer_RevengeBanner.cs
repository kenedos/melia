using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Pads.Handlers.Swordsmen.Templar
{
	[Package("laima")]
	[PadHandler(PadName.Templer_RevengeBanner)]
	public class Templer_RevengeBannerPadOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const int DamageInterval = 1000;
		private const int MaximumTargets = 10;
		private const float AreaMultiplier = 4f;
		private const float EnhancePerLevel = 0.005f;
		private const float MaximumLevelBonus = 0.10f;
		private const int MaximumEnhanceLevel = 100;
		private const float MinimumPullDistance = 5f;

		private static readonly TimeSpan PadDuration = TimeSpan.FromSeconds(30);
		private static readonly TimeSpan PveControlDuration = TimeSpan.FromSeconds(3);
		private static readonly TimeSpan PvpControlDuration = TimeSpan.FromSeconds(1);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;
			var areaRadius = Math.Max(1f, skill.Data.SplashRange * AreaMultiplier);

			pad.SetRange(areaRadius);
			pad.SetUpdateInterval(DamageInterval);
			pad.Trigger.MaxActorCount = MaximumTargets;
			pad.Trigger.LifeTime = PadDuration;

			Send.ZC_NORMAL.PadUpdate(pad, true);

			if (creator == null || creator.IsDead || creator.Map == null || skill == null)
				return;

			this.AttackTargets(pad, creator, skill, applyInitialControl: true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (creator == null || creator.IsDead || creator.Map == null || skill == null)
				return;

			this.AttackTargets(pad, creator, skill, applyInitialControl: false);
		}

		private void AttackTargets(Pad pad, ICombatEntity creator, Skill skill, bool applyInitialControl)
		{
			var areaRadius = Math.Max(1f, skill.Data.SplashRange * AreaMultiplier);
			var damageMultiplier = this.GetEnhanceMultiplier(creator);

			var targets = creator.Map
				.GetAttackableEnemiesInPosition(creator, pad.Position, areaRadius)
				.Where(target => target != null && !target.IsDead && creator.CanAttack(target))
				.OrderBy(target => pad.Position.Get2DDistance(target.Position))
				.Take(MaximumTargets)
				.ToList();

			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var modifier = SkillModifier.Default;
				modifier.DamageMultiplier *= damageMultiplier;

				var result = SCR_SkillHit(creator, target, skill, modifier);

				if (result.Result == HitResultType.Dodge)
					continue;

				if (result.Damage > 0)
					target.TakeDamage(result.Damage, creator);

				var skillHit = new SkillHitInfo(creator, target, skill, result, TimeSpan.Zero, TimeSpan.Zero);
				skillHit.HitEffect = HitEffect.Impact;

				if (applyInitialControl && result.Damage > 0)
					this.ApplyInitialControl(pad, creator, target, skill, skillHit);

				hits.Add(skillHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(creator, hits);
		}

		private void ApplyInitialControl(Pad pad, ICombatEntity creator, ICombatEntity target, Skill skill, SkillHitInfo skillHit)
		{
			var distanceToCenter = target.Position.Get2DDistance(pad.Position);

			if (distanceToCenter <= MinimumPullDistance)
				return;

			var pullPower = Math.Max(1, (int)(distanceToCenter - MinimumPullDistance));
			var pullDirection = target.Position.GetDirection(pad.Position);
			var controlDuration = target is Character ? PvpControlDuration : PveControlDuration;

			skillHit.KnockBackInfo = new KnockBackInfo(target, KnockBackType.KnockBack, pullPower, 0, pullDirection);
			skillHit.HitInfo.KnockBackType = KnockBackType.KnockBack;

			target.ApplyKnockback(creator, skill, skillHit);
			target.AddState(StateType.KnockedBack, controlDuration);
		}

		private float GetEnhanceMultiplier(ICombatEntity creator)
		{
			if (creator is not Character character)
				return 1f;

			if (!character.Abilities.TryGet(AbilityId.Templar16, out var ability) || !ability.Active)
				return 1f;

			var abilityLevel = Math.Clamp(ability.Level, 0, MaximumEnhanceLevel);
			var enhanceRate = abilityLevel * EnhancePerLevel;

			if (abilityLevel >= MaximumEnhanceLevel)
				enhanceRate += MaximumLevelBonus;

			return 1f + enhanceRate;
		}
	}
}
