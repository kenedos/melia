using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Exorcist;
using Melia.Zone.Pads;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Pads.Clerics.Exorcist
{
	[Package("laima")]
	[PadHandler("Exorcist_AquaBenedicta")]
	public class Exorcist_AquaBenedicta_Pad : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float DamageRange = 100f;
		private const int MaximumTargets = 10;
		private const int UpdateIntervalMilliseconds = 500;
		private const int BaseDurationMilliseconds = 7000;
		private const int LastDropDurationMilliseconds = 2500;
		private const int LifetimeGraceMilliseconds = 250;
		private const string ElapsedTimeVariable = "Exorcist.AquaBenedicta.Elapsed";
		private const string HiddenVariable = "Exorcist.AquaBenedicta.Hidden";
		private static readonly TimeSpan HitAnimationTime = TimeSpan.Zero;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			if (args.Creator is not Character caster)
				return;

			var lastDrop = Exorcist_AquaBenedictaLastDropAbility.IsActive(caster);
			var damageDuration = BaseDurationMilliseconds + (lastDrop ? LastDropDurationMilliseconds : 0);

			pad.SetRange(DamageRange);
			pad.SetUpdateInterval(UpdateIntervalMilliseconds);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(damageDuration + LifetimeGraceMilliseconds);
			pad.Trigger.MaxActorCount = MaximumTargets;
			pad.Variables.SetInt(ElapsedTimeVariable, 0);
			pad.Variables.SetInt(HiddenVariable, 0);

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

			var elapsedTime = pad.Variables.GetInt(ElapsedTimeVariable) + UpdateIntervalMilliseconds;
			pad.Variables.SetInt(ElapsedTimeVariable, elapsedTime);

			var lastDrop = Exorcist_AquaBenedictaLastDropAbility.IsActive(caster);

			if (lastDrop && elapsedTime >= BaseDurationMilliseconds && pad.Variables.GetInt(HiddenVariable) == 0)
			{
				pad.Variables.SetInt(HiddenVariable, 1);
				Send.ZC_NORMAL.PadUpdate(pad, false);
			}

			this.ApplyDamage(pad, caster);
		}

		private void ApplyDamage(Pad pad, Character caster)
		{
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
				modifier.DamageMultiplier *= Exorcist_AquaBenedictaEnhanceAbility.GetDamageMultiplier(caster);

				var result = SCR_SkillHit(caster, target, skill, modifier);

				if (result.Result != HitResultType.Dodge && result.Damage > 0)
				{
					target.TakeDamage(result.Damage, caster);
					target.StartBuff(BuffId.AquaBenedicta_DeBuff, skill.Level, 0, TimeSpan.FromMilliseconds(750), caster, skill.Id);
				}

				hits.Add(new SkillHitInfo(caster, target, skill, result, HitAnimationTime, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
