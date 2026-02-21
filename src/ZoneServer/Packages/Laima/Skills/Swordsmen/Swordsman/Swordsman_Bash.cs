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
using Melia.Zone.Skills.Handlers;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.HandlersOverrides.Swordsmen.Swordsman
{
	/// <summary>
	/// Handler for the Swordsman skill Bash.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Swordman_Bash)]
	public class Swordman_BashOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var hits = new List<SkillHitInfo>();

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 40, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			var hitDelay = 80;
			var damageDelay = 280;
			skill.Run(SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits, this.ApplyStun));
		}

		/// <summary>
		/// Applies stun with 30% chance
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="skillHitResult"></param>
		/// <returns></returns>
		private SkillHitResult ApplyStun(Skill skill, ICombatEntity caster, ICombatEntity target, SkillHitResult skillHitResult)
		{
			var stunChance = 3;

			if (skillHitResult.Damage > 0)
			{
				if (RandomProvider.Get().Next(10) <= stunChance)
					target.StartBuff(BuffId.Stun, skill.Level, 0, TimeSpan.FromSeconds(3), caster);
			}

			return skillHitResult;
		}
	}
}
