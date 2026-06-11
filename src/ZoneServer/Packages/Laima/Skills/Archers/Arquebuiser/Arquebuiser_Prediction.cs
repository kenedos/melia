using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Yggdrasil.Extensions;

namespace Melia.Zone.Skills.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Arquebuiser skill Prediction.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Arquebusier_Prediction)]
	public class Arquebusier_Prediction : IGroundSkillHandler, IDynamicCasted
	{
		private const float SplashRadius = 50;
		private const int TotalHits = 10;

		/// <summary>
		/// Called when the user starts casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Called when the user stops casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles skill, applying a debuff to the target
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			caster.SetAttackState(true);

			// [Arts] Prediction: Perspective Distortion
			// Cooldown changes to 10 seconds and the caster can select an area to cast the skill
			// Decreasing the AoE Defense Ratio as much AoE Attack Ratio the caster has
			if (caster.IsAbilityActive(AbilityId.Arquebusier23))
			{
				var cooldown = TimeSpan.FromSeconds(10);
				skill.IncreaseOverheat(cooldown);

				var splashArea = new Circle(farPos, SplashRadius);
				this.ApplyDebuffInArea(skill, caster, splashArea);

				Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
				
			} else
			{
				skill.IncreaseOverheat();

				var duration = TimeSpan.FromMinutes(30);
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				var accuracyReinforceRateBonus = 1f + SCR_Get_AbilityReinforceRate(skill);

				caster.StartBuff(BuffId.Prediction_Buff, skill.Level, accuracyReinforceRateBonus, duration, caster);
			}

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, null);
		}

		/// <summary>
		/// Executes the debuff on the desired area and reach targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		private void  ApplyDebuffInArea(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			for (var i = 0; i < TotalHits; ++i)
			{
				var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
				var targetPos = splashArea.OriginPos;
				
				if (targets.Count != 0)
				{
					var target = targets.Random();
					if (!caster.CanDamage(target))
						continue;

					target.StartBuff(BuffId.Prediction_Debuff, 0f, 0f, TimeSpan.FromSeconds(3), caster);
				}
			}
		}
	}
}
