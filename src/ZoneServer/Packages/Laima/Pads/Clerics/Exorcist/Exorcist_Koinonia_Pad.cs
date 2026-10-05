using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Exorcist;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Pads.Clerics.Exorcist
{
	[Package("laima")]
	[PadHandler("Exorcist_Koinonia")]
	public class Exorcist_Koinonia_Pad : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float DamageRange = 60f;
		private const int MaximumTargets = 10;
		private const int UpdateIntervalMilliseconds = 1000;
		private const int DurationMilliseconds = 5000;
		private const int LifetimeGraceMilliseconds = 250;
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			pad.SetRange(DamageRange);
			pad.SetUpdateInterval(UpdateIntervalMilliseconds);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(DurationMilliseconds + LifetimeGraceMilliseconds);
			pad.Trigger.MaxActorCount = MaximumTargets;

			Send.ZC_NORMAL.PadUpdate(pad, true);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			if (args.Creator is not Character caster || caster.IsDead || caster.Map == null || pad.Skill == null)
				return;

			var area = new Melia.Zone.Skills.SplashAreas.Circle(pad.Position, DamageRange);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area)
					.Where(target => target != null && !target.IsDead)
					.OrderBy(target => pad.Position.Get2DDistance(target.Position))
					.Take(MaximumTargets)
					.ToList();

			if (targets.Count == 0)
				return;

			var skill = pad.Skill;
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var modifier = SkillModifier.Default;
				modifier.DamageMultiplier *= Exorcist_GrandCrossEnhanceAbility.GetDamageMultiplier(caster);

				var result = SCR_SkillHit(caster, target, skill, modifier);

				if (result.Result != HitResultType.Dodge && result.Damage > 0)
					target.TakeDamage(result.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, result, HitAnimationTime, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
