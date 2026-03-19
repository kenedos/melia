using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Card
{
	/// <summary>
	/// Handler for the Frostbite_DeBuff from the Froster Lord card.
	/// Slows the target's movement speed, equivalent to Lethargy level 5.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Frostbite_DeBuff)]
	public class Frostbite_DeBuffOverride : BuffHandler
	{
		private const float MovementReduction = 20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -MovementReduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
