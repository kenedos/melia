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
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;

namespace Melia.Zone.Skills.Handlers.Barbarian
{
	/// <summary>
	/// Handler for the Barbarian skill Savagery.
	/// </summary>
	[Package("laima")]
	[SkillHandler(HandlerPriority.Low, SkillId.Barbarian_Savagery)]
	public class Barbarian_SavageryOverride : ISelfSkillHandler
	{
		private readonly static TimeSpan BuffDuration = TimeSpan.FromSeconds(50);

		/// <summary>
		/// Handles skill, applying the buff to the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="dir"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var criticalBonus = 0;

			if (caster.TryGetActiveAbilityLevel(AbilityId.Barbarian5, out var barbarian5Level))
				criticalBonus = 5 * barbarian5Level;

			caster.StartBuff(BuffId.Savagery_Buff, skill.Level, criticalBonus, BuffDuration, caster);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);
		}
	}
}
