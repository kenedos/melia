using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Shared.Data.Database;

namespace Melia.Zone.Buffs.Handlers.Wizard
{
	/// <summary>
	/// Handler for the Flame Ground debuff.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.FlameGround_Debuff)]
	public class FlameGround_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.Fire_Def_BM, -buff.NumArg2 * 50);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.Fire_Def_BM);
		}
	}
}
