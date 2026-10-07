using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Wizards.Onmyoji
{
	/// <summary>
	/// Handler for Toyou's shaking ground, which strikes up to the skill's
	/// ratio of enemies on it every 0.5 seconds; knocked down enemies take
	/// half damage and may be held in place.
	/// </summary>
	/// <remarks>
	/// Flying enemies are only struck with Toyou: Debris, 20% of the time
	/// per ability level.
	/// </remarks>
	[Package("laima-skills")]
	[PadHandler(PadName.Toyou_Pad)]
	public class Onmyoji_ToyouOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float Range = 150f;
		private const int UpdateInterval = 500;
		private const float DownedDamageRate = 0.5f;
		private const int HoldChance = 10;
		private const int DebrisChancePerLevel = 20;
		private static readonly TimeSpan LifeTime = TimeSpan.FromMilliseconds(3300);
		private static readonly TimeSpan HoldDuration = TimeSpan.FromSeconds(3);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.SetUpdateInterval(UpdateInterval);
			pad.Trigger.LifeTime = LifeTime;
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

			if (caster.IsDead)
				return;

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);
			caster.TryGetActiveAbilityLevel(AbilityId.Onmyoji15, out var debrisLevel);

			var targets = pad.Trigger.GetAttackableEntities(caster)
				.Where(target => target.MoveType != MoveType.Flying || GameRandom.Get().Next(100) < debrisLevel * DebrisChancePerLevel)
				.Take(maxTargets);

			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var isDown = target.IsKnockedDown();

				var modifier = new SkillModifier();
				if (isDown)
					modifier.FinalDamageMultiplier *= DownedDamageRate;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));

				if (isDown && !target.IsDead && GameRandom.Get().Next(100) < HoldChance)
					target.StartBuff(BuffId.Hold, skill.Level, 0, HoldDuration, caster, skill.Id);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
