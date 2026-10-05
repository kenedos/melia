using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Onmyoji
{
	[Package("laima")]
	[SkillHandler(SkillId.Onmyoji_Toyou)]
	public class Onmyoji_ToyouOverride : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int TotalHits = 6;
		private const int MaximumTargets = 15;
		private const int HitIntervalMilliseconds = 500;
		private const int DurationMilliseconds = 3000;
		private const int HoldDurationSeconds = 8;
		private const float HoldChance = 0.10f;
		private const float AreaRadius = 120f;

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

			var pad = new Pad(PadName.Toyou_Pad, caster, skill, new Circle(targetPosition, AreaRadius));
			pad.Position = targetPosition;
			pad.Direction = caster.Direction;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(DurationMilliseconds);
			caster.Map.AddPad(pad);

			skill.Run(this.RunToyou(skill, caster, pad, targetPosition));
			caster.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(caster);
		}

		private async Task RunToyou(Skill skill, ICombatEntity caster, Pad pad, Position center)
		{
			try
			{
				for (var hitIndex = 0; hitIndex < TotalHits; hitIndex++)
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
			var debrisEnabled = caster.IsAbilityActive(AbilityId.Onmyoji15);
			var knockdownEnabled = caster.IsAbilityActive(AbilityId.Onmyoji14);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, new Circle(center, AreaRadius))
				.Where(target => target != null && !target.IsDead)
				.Where(target => debrisEnabled || target.MoveType != MoveType.Flying)
				.OrderBy(target => target.Position.Get2DDistance(center))
				.Take(MaximumTargets)
				.ToList();
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);
				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.FromMilliseconds(HitIntervalMilliseconds), TimeSpan.Zero);

				if (knockdownEnabled && target.IsKnockdownable())
				{
					// Força 0 impede o deslocamento para trás; VPow (vertical) faz ele levantar e cair no chão
					skillHit.KnockBackInfo = new KnockBackInfo(center, target, KnockBackType.KnockDown, 0, 10);
					skillHit.KnockBackInfo.Speed = 1;
					skillHit.KnockBackInfo.VPow = 1;
					skillHit.HitInfo.KnockBackType = KnockBackType.KnockDown;
					target.ApplyKnockback(caster, skill, skillHit);
				}

				if (RandomProvider.Get().NextDouble() < HoldChance)
					target.StartBuff(BuffId.Hold, skill.Level, 0f, TimeSpan.FromSeconds(HoldDurationSeconds), caster, skill.Id);

				hits.Add(skillHit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
