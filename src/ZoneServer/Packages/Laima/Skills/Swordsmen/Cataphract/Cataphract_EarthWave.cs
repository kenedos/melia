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
using Yggdrasil.Logging;

namespace Melia.Zone.Skills.Handlers.Cataphract
{
	/// <summary>
	/// Handler for the Cataphract skill Earth Wave.
	/// Mounted AoE attack that creates a shockwave on the ground.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cataphract_EarthWave)]
	public class Cataphract_EarthWaveOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		/// <summary>
		/// Called when the skill begins casting.
		/// </summary>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_archer_cloaking_shot", "voice_war_atk_long_cast");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Called when the skill cast ends.
		/// </summary>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopSound("voice_archer_cloaking_shot", "voice_war_atk_long_cast");
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles the Earth Wave skill execution.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, caster.Position, caster.Position));
		}

		/// <summary>
		/// Executes the multi-hit AoE attack.
		/// </summary>
		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 25, width: 70, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var damageDelay = TimeSpan.FromMilliseconds(500);
			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(TimeSpan.FromMilliseconds(350));

			var hits = new List<SkillHitInfo>();
			var targetList = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			foreach (var target in targetList.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(3);
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				if (skillHitResult.Damage > 0 && target.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, HitType.KnockDown, 150, 89);
					skillHit.HitInfo.Type = HitType.KnockDown;
					target.ApplyKnockdown(caster, skill, skillHit);
				}

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
