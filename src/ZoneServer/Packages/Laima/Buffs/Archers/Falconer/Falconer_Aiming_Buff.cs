using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handle for the Aiming, Aiming.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Aiming_Buff)]
	public class Falconer_Aiming_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			Send.ZC_NORMAL.EnableHitRadiusPreview(buff.Target, true, buff.Caster);
			Send.ZC_ALTER_HIT_RADIUS(buff.Caster, buff.Target, 11.05f, 0f);
		}

		public override void OnEnd(Buff buff)
		{
			Send.ZC_NORMAL.EnableHitRadiusPreview(buff.Target, false, buff.Caster);
			Send.ZC_ALTER_HIT_RADIUS(buff.Caster, buff.Target, 0, 0f);
		}
	}
}
