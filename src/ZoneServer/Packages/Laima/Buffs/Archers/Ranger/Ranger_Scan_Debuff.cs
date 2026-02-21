using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Archers.Ranger
{
	/// <summary>
	/// Handler for the Scan debuff, which removes the target's critical 
	/// resistance based on the caster's accuracy.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Ranger_Scan_Debuff)]
	public class Ranger_Scan_DebuffOverride : BuffHandler
	{
		/// <summary>
		/// Applies the debuff when the buff starts.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var level = buff.NumArg1;
			var critResistReduce = buff.NumArg2;

			AddPropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM, -critResistReduce);
		}

		/// <summary>
		/// Removes the debuff when the buff ends.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM);
		}
	}
}
