using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Monk
{
	/// <summary>
	/// Handler for the Monk skill Energy Blast.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Monk_EnergyBlast)]
	public class Monk_EnergyBlastOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_cleric_sunrayshand_shot", "voice_cleric_m_sunrayshand_shot");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_cleric_sunrayshand_shot", "voice_cleric_m_sunrayshand_shot");
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets?.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, originPos, farPos));
		}

		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var delayBetweenHits = TimeSpan.FromMilliseconds(100);
			var totalHits = 60;

			var goldenBellBonus = 0f;
			if (caster.TryGetBuff(BuffId.Golden_Bell_Shield_Buff, out var gbsBuff))
			{
				var skillLevel = gbsBuff.NumArg1;
				var totalSpConsumed = gbsBuff.Vars.GetFloat("TotalSpConsumed");
				goldenBellBonus = totalSpConsumed * (1.5f + 0.15f * skillLevel);
				caster.StopBuff(BuffId.Golden_Bell_Shield_Buff);
			}

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 280, width: 40, angle: 0f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			await skill.Wait(TimeSpan.FromMilliseconds(200));

			for (var i = 0; i < totalHits; i++)
			{
				if (caster.IsDead)
					break;

				var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

				var isLastHit = i == totalHits - 1;

				var hits = new List<SkillHitInfo>();

				foreach (var target in targets.Take(15))
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill);
					skillHitResult.Damage += goldenBellBonus;
					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, delayBetweenHits, TimeSpan.Zero);

					if (isLastHit && skillHitResult.Damage > 0 && target.IsKnockdownable() && !caster.IsAbilityActive(AbilityId.Monk8))
					{
						skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, skillHit.Target, HitType.KnockDown, 80, 30);
						skillHit.HitInfo.Type = HitType.KnockDown;
						target.ApplyKnockdown(caster, skill, skillHit);
					}

					hits.Add(skillHit);
				}

				Send.ZC_SKILL_HIT_INFO(caster, hits);

				await skill.Wait(delayBetweenHits);
			}

			Send.ZC_SKILL_DISABLE(caster);
			Send.ZC_NORMAL.SkillCancel(caster, SkillId.Monk_EnergyBlast);
			Send.ZC_NORMAL.SkillCancelCancel(caster, SkillId.Monk_EnergyBlast);
		}
	}
}
