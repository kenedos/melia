using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Rodelero
{
	/// <summary>
	/// Handler for ShieldBash_Debuff (Armor Break).
	/// Reduces the target's physical defense by 10% + 1% per skill level (max 40%).
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ShieldBash_Debuff)]
	public class ShieldBash_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var reductionPercent = Math.Min((10f + 1.0f * buff.NumArg1) / 100f, 0.40f);
			var currentDef = buff.Target.Properties.GetFloat(PropertyName.DEF);
			var reduction = currentDef * reductionPercent;
			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, -reduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
		}
	}
}
