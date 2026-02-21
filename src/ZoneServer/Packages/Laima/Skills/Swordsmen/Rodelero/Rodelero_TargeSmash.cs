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
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Rodelero
{
	/// <summary>
	/// Handler for the Rodelero skill Targe Smash.
	/// Multi-hit shield bash attack that deals AoE damage.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Rodelero_TargeSmash)]
	public class Rodelero_TargeSmashOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles the Targe Smash skill execution.
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
		/// Executes the multi-hit AoE attack.
		/// </summary>
		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 20, width: 50, angle: 15f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var damageDelay = TimeSpan.FromMilliseconds(400);
			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(TimeSpan.FromMilliseconds(250));

			var hits = new List<SkillHitInfo>();
			var targetList = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var knockBackOrigin = caster.Position.GetRelative(caster.Direction, 20);

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
						skillHit.KnockBackInfo = new KnockBackInfo(knockBackOrigin, target, HitType.KnockBack, 60, 10);
						skillHit.HitInfo.Type = HitType.KnockBack;
					}

					if (RandomProvider.Get().Next(100) < 30)
						target.StartBuff(BuffId.Stun, 1, 0, TimeSpan.FromMilliseconds(3000), caster);
				}

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
