using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Divine Stigma, Receive continuous damage, Ignores some of your Defense when hit.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DivineStigma_Damage_Debuff)]
	public class DivineStigma_Damage_DebuffOverride : BuffHandler
	{

	}
}
