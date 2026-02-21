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
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.OutLaw
{
	/// <summary>
	/// Handler for the Outlaw skill Rampage.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.OutLaw_Rampage)]
	public class OutLaw_RampageOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 150, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.Attack(skill, caster, splashArea));
		}

		/// <summary>
		/// Executes the actual attack after a delay.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			var damageDelay = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			var delayBetweenHits = new[]
			{
				TimeSpan.FromMilliseconds(50),
				TimeSpan.FromMilliseconds(100),
				TimeSpan.FromMilliseconds(500),
				TimeSpan.FromMilliseconds(50),
				TimeSpan.FromMilliseconds(700),
				TimeSpan.FromMilliseconds(100),
				TimeSpan.FromMilliseconds(50),
				TimeSpan.FromMilliseconds(50)
			};

			// first hit hits instantly
			var hits = new List<SkillHitInfo>();

			var stunChance = skill.Level * 2f;

			var iceVariant = caster.IsAbilityActive(AbilityId.Outlaw20);

			// Caster avoids all attacks during the skill's animation
			caster.StartBuff(BuffId.Skill_MomentaryEvasion_Buff, skill.Level, 0, TimeSpan.FromMilliseconds(1700), caster);

			// Outlaw17 gives a buff that prevents removable debuffs
			if (caster.IsAbilityActive(AbilityId.Outlaw17))
				caster.StartBuff(BuffId.Rampage_Buff, skill.Level, 0, TimeSpan.FromMilliseconds(1700), caster);

			// Outlaw18 gives a buff that adds 10% bonus crit chance per debuff currently active
			if (caster.IsAbilityActive(AbilityId.Outlaw18))
			{
				int debuffCount = 0;
				foreach (var buff in caster.Components.Get<BuffComponent>().GetList())
				{
					if (buff.Data.Type == BuffType.Debuff)
					{
						debuffCount++;
					}
				}

				if (debuffCount > 0)
					caster.StartBuff(BuffId.Rampage_Outlaw18_Buff, debuffCount, 0, TimeSpan.FromMilliseconds(1700), caster);
			}

			for (var i = 0; i < 9; i++)
			{
				var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

				foreach (var target in targets.LimitBySDR(caster, skill))
				{
					var modifier = SkillModifier.Default;

					if (iceVariant)
						modifier.AttackAttribute = AttributeType.Ice;

					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
					target.TakeDamage(skillHitResult.Damage, caster);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
					skillHit.HitEffect = HitEffect.Impact;
					hits.Add(skillHit);

					if (RandomProvider.Get().Next(100) < stunChance)
					{
						target.StartBuff(BuffId.Stun, skill.Level, 0, TimeSpan.FromSeconds(3), caster);
					}
				}

				Send.ZC_SKILL_HIT_INFO(caster, hits);

				hits.Clear();

				if (i < 8)
					await skill.Wait(delayBetweenHits[i]);
			}

			caster.StartBuff(BuffId.Rampage_After_Buff, skill.Level, 0, TimeSpan.FromSeconds(10), caster);
		}
	}
}
