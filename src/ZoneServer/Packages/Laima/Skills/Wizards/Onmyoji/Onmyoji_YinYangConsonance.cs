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
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Onmyoji
{
	[Package("laima")]
	[SkillHandler(SkillId.Onmyoji_YinYangConsonance)]
	public class Onmyoji_YinYangConsonanceOverride : IGroundSkillHandler, IMeleeGroundSkillHandler, IDynamicCasted, ICancelSkillHandler
	{
		private const int TotalDamageCycles = 30;
		private const int CycleMilliseconds = 100;
		private const int MaximumTargets = 15;
		private const float AttackRadius = 120f;
		private const string TemporaryPainBarrierKey = "Melia.Onmyoji.YinYangConsonance.TemporaryPainBarrier";
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(CycleMilliseconds);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (caster.IsBuffActive(BuffId.PainBarrier_Buff))
			{
				skill.Vars.SetBool(TemporaryPainBarrierKey, false);
				return;
			}

			var buff = caster.StartBuff(BuffId.PainBarrier_Buff, skill.Level, 0f, TimeSpan.FromMilliseconds(3200), caster, skill.Id);
			skill.Vars.SetBool(TemporaryPainBarrierKey, buff != null);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			this.StopChanneling(skill, caster);
		}

		public void Handle(Skill skill, ICombatEntity caster)
		{
			this.StopChanneling(skill, caster);
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
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			if (!character.TrySpendSp(skill))
			{
				character.SetAttackState(false);
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}

			skill.IncreaseOverheat();
			character.SetAttackState(true);
			character.TurnTowards(farPos);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, farPos);
			skill.Run(this.ExecuteChannel(skill, character, farPos));
		}

		private async Task ExecuteChannel(Skill skill, Character caster, Position center)
		{
			try
			{
				for (var cycle = 0; cycle < TotalDamageCycles; cycle++)
				{
					await skill.Wait(TimeSpan.FromMilliseconds(CycleMilliseconds));

					if (caster.IsDead || caster.Map == null || !caster.IsCasting())
						break;

					this.ExecuteDamageCycle(skill, caster, center, cycle);
				}
			}
			finally
			{
				this.StopChanneling(skill, caster);
			}
		}

		private void ExecuteDamageCycle(Skill skill, Character caster, Position center, int cycle)
		{
			var attribute = this.GetCycleAttribute(cycle);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, new Circle(center, AttackRadius))
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => center.Get2DDistance(target.Position))
				.Take(MaximumTargets)
				.ToList();
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				var modifier = SkillModifier.Default;
				modifier.AttackAttribute = attribute;

				var result = SCR_SkillHit(caster, target, skill, modifier);
				if (result.Result != HitResultType.Dodge && result.Damage > 0f)
					target.TakeDamage(result.Damage, caster);

				var hit = new SkillHitInfo(caster, target, skill, result, HitAnimationTime, TimeSpan.Zero);
				hit.HitInfo.Type = this.GetHitType(attribute);
				hits.Add(hit);
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		private AttributeType GetCycleAttribute(int cycle)
		{
			return cycle % 2 == 0 ? AttributeType.Holy : AttributeType.Dark;
		}

		private HitType GetHitType(AttributeType attribute)
		{
			return attribute == AttributeType.Holy ? HitType.Holy : HitType.Dark;
		}

		private void StopChanneling(Skill skill, ICombatEntity caster)
		{
			if (skill.Vars.GetBool(TemporaryPainBarrierKey))
			{
				caster.StopBuff(BuffId.PainBarrier_Buff);
				skill.Vars.SetBool(TemporaryPainBarrierKey, false);
			}

			if (caster is Character character)
			{
				character.SetAttackState(false);
				Send.ZC_SKILL_DISABLE(character);
			}
		}
	}
}
