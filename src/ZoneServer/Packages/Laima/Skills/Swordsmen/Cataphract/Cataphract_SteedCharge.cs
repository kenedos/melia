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
	/// Handler for the Cataphract skill Steed Charge.
	/// Mounted charge attack that deals damage in a line.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cataphract_SteedCharge)]
	public class Cataphract_SteedChargeOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles the Steed Charge skill execution.
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
		/// Executes the charge attack.
		/// </summary>
		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 220, width: 35, angle: 40f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var damageDelay = TimeSpan.FromMilliseconds(200);
			var skillHitDelay = TimeSpan.Zero;

			var hits = new List<SkillHitInfo>();
			var targetList = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			foreach (var target in targetList.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(2);
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				if (skillHitResult.Damage > 0 && target.IsKnockdownable())
				{
					skillHit.KnockBackInfo = new KnockBackInfo(caster.Position, target, HitType.KnockDown, 200, 20);
					skillHit.HitInfo.Type = HitType.KnockDown;
					target.ApplyKnockdown(caster, skill, skillHit);
				}

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
