using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Wizards.Onmyoji
{
	/// <summary>
	/// Handler for Yin Yang Harmony's circles, which strike up to the skill's
	/// ratio of enemies in them 10 times every second, alternating
	/// Psychokinesis, Non and Earth, for as long as the Onmyoji keeps
	/// channeling.
	/// </summary>
	/// <remarks>
	/// The Heaven and Earth circle hits twice as often and shrinks by 2%
	/// every 0.1 seconds.
	/// </remarks>
	[Package("laima-skills")]
	[PadHandler(PadName.YinYangConsonance_Pad, PadName.YinYangConsonance_Hidden_Pad)]
	public class Onmyoji_YinYangConsonanceOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float Range = 120f;
		private const int UpdateInterval = 1000;
		private const int HitsPerStrike = 10;
		private const int HeavenAndEarthHitsPerStrike = 20;
		private const float LineInterval = 0.1f;
		private const double HeavenAndEarthShrinkPerTenth = 0.98;
		private static readonly AttributeType[] Attributes = [AttributeType.Soul, AttributeType.Melee, AttributeType.Earth];

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.SetUpdateInterval(UpdateInterval);

			this.Strike(pad, args.Creator);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;

			if (caster.IsDead || !caster.IsCasting(pad.Skill))
			{
				pad.Destroy();
				return;
			}

			this.Strike(pad, caster);
		}

		/// <summary>
		/// Strikes the enemies in the circle, each with one hit that the
		/// client splits into the strike's number of lines.
		/// </summary>
		/// <param name="pad"></param>
		/// <param name="caster"></param>
		private void Strike(Pad pad, ICombatEntity caster)
		{
			var skill = pad.Skill;
			var elapsed = pad.Trigger.LifeTime - pad.Trigger.RemainingLifeTime;
			var strike = (int)(elapsed.TotalMilliseconds / UpdateInterval);
			var isHeavenAndEarth = pad.Name == PadName.YinYangConsonance_Hidden_Pad;

			if (isHeavenAndEarth)
				pad.SetRange((float)(Range * Math.Pow(HeavenAndEarthShrinkPerTenth, elapsed.TotalMilliseconds / 100)));

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);

			foreach (var target in pad.Map.GetAttackableEnemiesIn(caster, pad.Area).Take(maxTargets))
			{
				var modifier = SkillModifier.MultiHit(isHeavenAndEarth ? HeavenAndEarthHitsPerStrike : HitsPerStrike);
				modifier.AttackAttribute = Attributes[strike % Attributes.Length];

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var hitInfo = new HitInfo(caster, target, skill, skillHitResult);
				hitInfo.MultiHitInterval = LineInterval;

				Send.ZC_HIT_INFO(caster, target, hitInfo);
			}
		}
	}
}
