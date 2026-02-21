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
using Melia.Zone.Abilities.Handlers.Swordsmen.Rodelero;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Rodelero
{
	/// <summary>
	/// Handler for the Rodelero skill Slithering.
	/// Channeled evasive maneuver that grants block bonuses and causes
	/// enemy attacks to miss. Final hit knocks down and slows enemies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Rodelero_Slithering)]
	public class Rodelero_SlitheringOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		/// <summary>
		/// Called when the skill begins channeling.
		/// </summary>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.RemoveBuff(BuffId.Slithering_Buff);
			caster.StartBuff(BuffId.Slithering_Buff, skill.Level, 0f, TimeSpan.Zero, caster);
			caster.PlaySound("voice_archer_camouflage_shot", "voice_archer_m_camouflage_shot");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// Called when the skill channeling ends.
		/// </summary>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.RemoveBuff(BuffId.Slithering_Buff);
			caster.StopSound("voice_archer_camouflage_shot", "voice_archer_m_camouflage_shot");
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles the Slithering skill execution.
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

			skill.Run(this.Attack(skill, caster, originPos, farPos));
		}

		/// <summary>
		/// Executes the multi-hit attack.
		/// </summary>
		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var damageDelay = TimeSpan.FromMilliseconds(300);
			var skillHitDelay = TimeSpan.Zero;
			await skill.Wait(TimeSpan.FromMilliseconds(150));

			var hits = new List<SkillHitInfo>();
			var targetList = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			foreach (var target in targetList.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.Default;
				modifier.BonusPAtk = Rodelero31.GetBonusPAtk(caster);
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				if (skillHitResult.Damage > 0)
				{
					if (target.IsKnockdownable())
					{
						skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, HitType.KnockDown, 120, 90);
						skillHit.HitInfo.Type = HitType.KnockDown;
						target.ApplyKnockdown(caster, skill, skillHit);
					}

					target.StartBuff(BuffId.Common_Slow, 1, 0f, TimeSpan.FromMilliseconds(5000), caster);
				}

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
