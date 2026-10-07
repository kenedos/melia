using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Maps;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers.Wizards.Onmyoji
{
	/// <summary>
	/// Handler for Greenwood Shikigami's tree, which strikes and pulls in up
	/// to the skill's ratio of enemies 5 times as it grows, twice each, and
	/// slows the enemies leaving it or still around it when it finishes.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.GreenwoodShikigami_Pad)]
	public class Onmyoji_GreenwoodShikigamiOverride : ICreatePadHandler, IDestroyPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const float Range = 100f;
		private const int StrikeCount = 5;
		private const int HitsPerStrike = 2;
		private const int StrikeInterval = 1200;
		private const int PullVelocity = 100;
		private static readonly TimeSpan GrowTime = TimeSpan.FromMilliseconds(5500);
		private static readonly TimeSpan LifeTime = TimeSpan.FromMilliseconds(7000);
		private static readonly TimeSpan SlowDuration = TimeSpan.FromSeconds(5);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(Range);
			pad.SetUpdateInterval(StrikeInterval);
			pad.Trigger.LifeTime = LifeTime;
			pad.Trigger.MaxUseCount = StrikeCount;

			var tree = PadCreateMonster(pad, "pcskill_greenwood", pad.Position, 0f, (float)GrowTime.TotalMilliseconds, "None", 1f, immediate: true);
			if (tree == null)
				return;

			foreach (var character in pad.Map.GetCharacters(c => c.Position.InRange2D(tree.Position, Map.VisibleRange)))
				character.LookAround();

			Send.ZC_PLAY_ANI(tree, "Born", true);
			Send.ZC_GROUND_EFFECT(tree, tree.Position, "F_wizard_attractpillar_shot_levitation", 0.6f, (float)GrowTime.TotalSeconds);
			Send.ZC_GROUND_EFFECT(tree, tree.Position, "F_wizard_attractpillar_shot_spread_in", 0.7f, (float)GrowTime.TotalSeconds - 1.5f);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, false);

			foreach (var target in pad.Trigger.GetAttackableEntities(caster))
				target.StartBuff(BuffId.GreenwoodShikigami_Debuff, pad.Skill.Level, 0, SlowDuration, caster, pad.Skill.Id);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;

			if (args.Creator.IsEnemy(args.Initiator))
				args.Initiator.StartBuff(BuffId.GreenwoodShikigami_Debuff, pad.Skill.Level, 0, SlowDuration, args.Creator, pad.Skill.Id);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var skill = pad.Skill;

			if (caster.IsDead)
				return;

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);
			var hits = new List<SkillHitInfo>();

			foreach (var target in pad.Trigger.GetAttackableEntities(caster).Take(maxTargets))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill, SkillModifier.MultiHit(HitsPerStrike));
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);

				if (skillHitResult.Damage > 0 && target.IsKnockdownable())
				{
					var pullFrom = target.Position.GetRelative(pad.Position.GetDirection(target.Position), Range);
					skillHit.KnockBackInfo = new KnockBackInfo(pullFrom, target, KnockBackType.KnockBack, PullVelocity, 10);
					skillHit.HitInfo.KnockBackType = KnockBackType.KnockBack;
					target.ApplyKnockback(caster, skill, skillHit);
				}

				hits.Add(skillHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);

			pad.Trigger.IncreaseUseCount();
		}
	}

	/// <summary>
	/// Handler for Greenwood Shikigami's growth effect.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.GreenwoodShikigami_Pad_Effect)]
	public class Onmyoji_GreenwoodShikigami_EffectOverride : ICreatePadHandler, IDestroyPadHandler
	{
		private static readonly TimeSpan LifeTime = TimeSpan.FromMilliseconds(5000);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.Trigger.LifeTime = LifeTime;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}
	}
}
