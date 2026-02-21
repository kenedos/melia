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
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Rodelero
{
	/// <summary>
	/// Handler for the Rodelero skill Montano.
	/// Upward sword strike that launches enemies into the air.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Rodelero_Montano)]
	public class Rodelero_MontanoOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles the Montano skill execution.
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
		/// Executes the attack with launch effect.
		/// </summary>
		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 0, angle: 170f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var damageDelay = TimeSpan.FromMilliseconds(400);
			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(TimeSpan.FromMilliseconds(200));

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
