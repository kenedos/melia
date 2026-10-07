using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Common
{
	/// <summary>
	/// Handler for the Common skill Remove, the slot that ends a
	/// transformation from the quickslot bar.
	/// </summary>
	/// <remarks>
	/// The client asks for the transformation's buff to be removed in the
	/// same press, which is what ends the form.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Common_StateClear)]
	public class Common_StateClearOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			skill.IncreaseOverheat();

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, 0, null);
		}
	}
}
