using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler override for the Decomposition Debuff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Decomposition_Debuff)]
	public class Decomposition_DebuffOverride : BuffHandler
	{
	}
}
