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

namespace Melia.Zone.Pads.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for the Rubric pads, which strike and slow the enemies in
	/// front of the Exorcist while they keep reading.
	/// </summary>
	/// <remarks>
	/// Devils and enemies near a Grand Cross take 3 hits per strike.
	/// </remarks>
	[Package("laima-skills")]
	[PadHandler(PadName.Exorcist_Rubric, PadName.Exorcist_Rubric_abil)]
	public class Exorcist_RubricOverride : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		private const float Length = 100f;
		private const float Width = 40f;
		private const int StrikeInterval = 450;
		private const int SpeedReadingStrikeInterval = 225;
		private const int DevilHitCount = 3;
		private const float CorruptionDamageBonus = 0.25f;
		private static readonly TimeSpan SlowDuration = TimeSpan.FromSeconds(1);
		private static readonly TimeSpan CorruptionDuration = TimeSpan.FromSeconds(10);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetUpdateInterval(pad.Name == PadName.Exorcist_Rubric_abil ? SpeedReadingStrikeInterval : StrikeInterval);
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

			if (caster.IsDead || !caster.IsCasting(skill))
			{
				pad.Destroy();
				return;
			}

			pad.Direction = caster.Direction;
			pad.SetRectangleRange(caster.Direction, Width, Length);

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);
			var nearGrandCross = caster.Map.GetPadsAt(caster.Position, Length).Any(a => a.Name == PadName.Exorcist_Koinonia && a.Creator == caster);
			var hasCorruption = caster.TryGetActiveAbilityLevel(AbilityId.Exorcist20, out var corruptionLevel);
			var hits = new List<SkillHitInfo>();

			foreach (var target in pad.Trigger.GetAttackableEntities(caster).Take(maxTargets))
			{
				var modifier = nearGrandCross || target.Race == RaceType.Velnias ? SkillModifier.MultiHit(DevilHitCount) : new SkillModifier();

				if (hasCorruption)
				{
					modifier.AttackAttribute = AttributeType.Dark;
					modifier.DamageMultiplier += CorruptionDamageBonus;
				}

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));

				target.StartBuff(BuffId.Rubric_DeBuff, skill.Level, 0, SlowDuration, caster, skill.Id);

				if (hasCorruption && target.GetOverbuffCount(BuffId.Rubric_Hidden_Debuff) < corruptionLevel)
					target.StartBuff(BuffId.Rubric_Hidden_Debuff, skill.Level, 0, CorruptionDuration, caster, skill.Id);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
