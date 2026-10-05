using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Onmyoji
{
	[Package("laima")]
	[SkillHandler(SkillId.Onmyoji_GreenwoodShikigami)]
	public class Onmyoji_GreenwoodShikigamiOverride : IGroundSkillHandler, IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int TotalHits = 10;
		private const int MaximumTargets = 10;
		private const int HitIntervalMilliseconds = 900;
		private const float AreaRadius = 150f;
		private const int DebuffDurationSeconds = 20;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPosition))
				targetPosition = farPos;

			if (!caster.InSkillUseRange(skill, targetPosition))
			{
				caster.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.TurnTowards(targetPosition);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, targetPosition);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPosition);

			var pad = new Pad(PadName.GreenwoodShikigami_Pad, caster, skill, new Circle(targetPosition, AreaRadius));
			pad.Position = targetPosition;
			pad.Direction = caster.Direction;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(TotalHits * HitIntervalMilliseconds);
			caster.Map.AddPad(pad);

			this.Attack(skill, caster, targetPosition);
			skill.Run(this.RunGreenwoodShikigami(skill, caster, pad, targetPosition));
			caster.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(caster);
		}

		private async Task RunGreenwoodShikigami(Skill skill, ICombatEntity caster, Pad pad, Position center)
		{
			try
			{
				for (var hitIndex = 1; hitIndex < TotalHits; hitIndex++)
				{
					await skill.Wait(TimeSpan.FromMilliseconds(HitIntervalMilliseconds));

					if (caster.IsDead || caster.Map == null)
						break;

					this.Attack(skill, caster, center);
				}
			}
			finally
			{
				if (pad?.Map != null)
					pad.Destroy();
			}
		}

		private void Attack(Skill skill, ICombatEntity caster, Position center)
		{
			var area = new Circle(center, AreaRadius);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => target.Position.Get2DDistance(center))
				.Take(MaximumTargets)
				.ToList();
			var hits = new List<SkillHitInfo>();
			var pullEnabled = caster.IsAbilityActive(AbilityId.Onmyoji16);

			foreach (var target in targets)
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);
				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);

				if (pullEnabled && target.IsKnockdownable())
				{
					var pullDirection = target.Position.GetDirection(center);
					var pullFromPosition = target.Position.GetRelative(pullDirection.Backwards, 75);
					skillHit.KnockBackInfo = new KnockBackInfo(pullFromPosition, target, KnockBackType.KnockBack, 100, 10);
					skillHit.KnockBackInfo.Speed = 1;
					skillHit.KnockBackInfo.VPow = 1;
					skillHit.HitInfo.KnockBackType = KnockBackType.KnockBack;
					target.ApplyKnockback(caster, skill, skillHit);
				}

				target.StartBuff(BuffId.GreenwoodShikigami_Debuff, skill.Level, 0f, TimeSpan.FromSeconds(DebuffDurationSeconds), caster, skill.Id);
				hits.Add(skillHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
