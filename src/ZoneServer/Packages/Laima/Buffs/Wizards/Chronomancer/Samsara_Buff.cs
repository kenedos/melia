using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	[Package("laima")]
	[BuffHandler(BuffId.Samsara_Buff)]
	public class Samsara_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			Send.ZC_NORMAL.PlayTextEffect(buff.Target, buff.Caster, "SHOW_BUFF_TEXT", (float)buff.Id, null, "Item");
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.StartBuff(BuffId.SamsaraAfter_Buff, System.TimeSpan.Zero, buff.Caster);
		}
	}
}
