using System;
using System.Collections.Generic;
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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Archers.QuarrelShooter
{
	/// <summary>
	/// Handler for the QuarrelShooter skill Kneelingshot.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.QuarrelShooter_Kneelingshot)]
	public class QuarrelShooterKneelingshot : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);
			caster.StartBuff(BuffId.Archer_Kneelingshot, 1f, 0f, TimeSpan.Zero, caster);
			SkillResetCooldown(skill, caster);
		}
	}
}
