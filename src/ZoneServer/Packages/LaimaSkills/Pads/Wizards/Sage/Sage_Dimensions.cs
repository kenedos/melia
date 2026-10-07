using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Wizards.Sage;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.HandlersOverride.Wizards.Sage
{
	/// <summary>
	/// Handler for Micro Dimension's distortion effect.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Sage_MicroDimension)]
	public class Sage_MicroDimensionPadOverride : ICreatePadHandler, IDestroyPadHandler
	{
		private static readonly TimeSpan LifeTime = TimeSpan.FromMilliseconds(300);

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

	/// <summary>
	/// Handler for Ultimate Dimension's distortion, which strikes up to the
	/// skill's ratio of enemies in it every 0.3 seconds.
	/// </summary>
	/// <remarks>
	/// With Ultimate Dimension: After Effects, enemies entering it are slowed
	/// and struck again every 0.5 seconds for a second per ability level.
	/// </remarks>
	[Package("laima-skills")]
	[PadHandler(PadName.Sage_UltimateDimension)]
	public class Sage_UltimateDimensionPadOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const float Range = 50f;
		private const int UpdateInterval = 300;
		private static readonly TimeSpan LifeTime = TimeSpan.FromMilliseconds(1500);

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

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var target = args.Initiator;

			if (!caster.IsEnemy(target) || !caster.TryGetActiveAbilityLevel(AbilityId.Sage12, out var level))
				return;

			var duration = TimeSpan.FromSeconds(level);
			target.StartBuff(BuffId.UltimateDimension_Debuff, pad.Skill.Level, 0, duration, caster, pad.Skill.Id);
			target.StartBuff(BuffId.UltimateDimension_Damage_Debuff, pad.Skill.Level, 0, duration, caster, pad.Skill.Id);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var skill = pad.Skill;

			if (caster.IsDead)
				return;

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio2);
			var hits = new List<SkillHitInfo>();

			foreach (var target in pad.Trigger.GetAttackableEntities(caster).Take(maxTargets))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}

	/// <summary>
	/// Handler for [Arts] Dimension Compression: Gravity Sphere, which drifts
	/// forward for 5 seconds pulling the enemies around it in, and strikes up
	/// to 3 enemies it reaches once each.
	/// </summary>
	[Package("laima-skills")]
	[PadHandler(PadName.Sage_DimensionCompression_Abil)]
	public class Sage_DimensionCompression_AbilOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		private const int UpdateInterval = 500;
		private const int MaxStrikes = 3;
		private const float PullRange = 100f;
		private const int PullVelocity = 80;
		private static readonly TimeSpan LifeTime = TimeSpan.FromSeconds(5);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetUpdateInterval(UpdateInterval);
			pad.Trigger.LifeTime = LifeTime;
			pad.Trigger.MaxUseCount = MaxStrikes;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			Send.ZC_NORMAL.PadUpdate(args.Trigger, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var target = args.Initiator;
			var skill = pad.Skill;

			if (caster.IsDead || target.IsDead || !caster.IsEnemy(target))
				return;

			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));

			if (!target.IsDead && caster.TryGetActiveAbilityLevel(AbilityId.Sage18, out var aftermathLevel) && GameRandom.Get().Next(100) < aftermathLevel * Sage_DimensionCompressionOverride.StunChancePerLevel)
				target.StartBuff(BuffId.Stun, 1, 0, Sage_DimensionCompressionOverride.StunDuration, caster, skill.Id);

			pad.Trigger.IncreaseUseCount();
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var caster = args.Creator;
			var skill = pad.Skill;

			if (caster.IsDead)
				return;

			foreach (var enemy in caster.Map.GetAttackableEnemiesInPosition(caster, pad.Position, PullRange))
			{
				if (!enemy.IsKnockdownable())
					continue;

				var pullFrom = enemy.Position.GetRelative(pad.Position.GetDirection(enemy.Position), PullRange);
				var pullHit = new SkillHitInfo(caster, enemy, skill, new SkillHitResult(), TimeSpan.Zero, TimeSpan.Zero);
				pullHit.KnockBackInfo = new KnockBackInfo(pullFrom, enemy, KnockBackType.KnockBack, PullVelocity, 10);
				pullHit.HitInfo.KnockBackType = KnockBackType.KnockBack;
				enemy.ApplyKnockback(caster, skill, pullHit);

				Send.ZC_SKILL_HIT_INFO(caster, pullHit);
			}
		}
	}
}
