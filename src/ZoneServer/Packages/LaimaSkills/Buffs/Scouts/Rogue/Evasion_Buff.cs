using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Scouts.Rogue
{
	/// <summary>
	/// Handler for the Evasion buff. Increases evasion rate.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Evasion_Buff)]
	public class Evasion_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var dr = buff.Target.Properties.GetFloat(PropertyName.DR);
			var bonus = dr * (GetCaptionRatio(buff, 1) / 100f);

			AddPropertyModifier(buff, buff.Target, PropertyName.DR_BM, bonus);
			buff.Target.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
			buff.Target.InvalidateProperties();
		}
	}
}
