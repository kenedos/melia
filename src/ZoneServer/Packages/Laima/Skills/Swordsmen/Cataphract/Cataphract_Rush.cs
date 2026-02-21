using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Cataphract
{
	/// <summary>
	/// Handler for the Cataphract skill Rush.
	/// Channeled mounted charge that repeatedly damages enemies while moving.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cataphract_Rush)]
	public class Cataphract_RushOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		/// <summary>
		/// Called when the skill begins channeling.
		/// </summary>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StartBuff(BuffId.Warrior_RushMove_Buff, 1f, 0f, TimeSpan.Zero, caster);
			caster.StartBuff(BuffId.Warrior_EnableMovingShot_Buff, 2, 0, TimeSpan.Zero, caster);
			caster.PlaySound("voice_atk_long_war_f", "voice_war_atk_long_cast");

			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Called when the skill channeling ends.
		/// </summary>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.RemoveBuff(BuffId.Warrior_RushMove_Buff);
			caster.StopBuff(BuffId.Warrior_EnableMovingShot_Buff);
			caster.StopSound("voice_atk_long_war_f", "voice_war_atk_long_cast");
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles the Rush skill execution.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Attack(skill, caster));
		}

		/// <summary>
		/// Executes the channeled rush attack, dealing damage in waves.
		/// </summary>
		private async Task Attack(Skill skill, ICombatEntity caster)
		{
			var hits = new List<SkillHitInfo>();
			var hitDelay = TimeSpan.FromMilliseconds(30);
			var damageDelay = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			var totalHits = 11;
			var attackWidth = 55f;
			var delayBetweenRepeats = TimeSpan.FromMilliseconds(250);
			var spCostPerHit = 4 + skill.Level;

			for (var i = 0; i < totalHits; ++i)
			{
				await skill.Wait(hitDelay);

				if (!caster.TrySpendSp(spCostPerHit))
				{
					caster.ServerMessage(Localization.Get("Not enough SP."));
					Send.ZC_SKILL_CAST_CANCEL(caster);
					return;
				}

				var splashParam = skill.GetSplashParameters(caster, caster.Position, caster.Position, length: 0, width: attackWidth, angle: 0);
				var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
				var targetList = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

				foreach (var target in targetList.LimitBySDR(caster, skill))
				{
					var modifier = SkillModifier.Default;
					modifier.BlockPenetrationMultiplier += 1.0f + 0.1f * skill.Level;

					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
					skillHit.HitEffect = HitEffect.Impact;

					hits.Add(skillHit);
				}

				Send.ZC_SKILL_HIT_INFO(caster, hits);

				if (i < totalHits - 1)
					await skill.Wait(delayBetweenRepeats);

				hits.Clear();

				if (!caster.IsCasting())
					break;
			}

			Send.ZC_SKILL_DISABLE(caster);
		}
	}
}
