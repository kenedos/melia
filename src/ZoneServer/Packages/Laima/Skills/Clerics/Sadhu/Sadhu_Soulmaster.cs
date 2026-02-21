using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the Cleric skill Spirit Expert (Soul Master).
	/// This skill is no longer in the skill tree (replaced by Out of Body).
	/// Kept as a no-op to prevent errors if triggered.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sadhu_Soulmaster)]
	public class Sadhu_SoulmasterOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
		}
	}
}
