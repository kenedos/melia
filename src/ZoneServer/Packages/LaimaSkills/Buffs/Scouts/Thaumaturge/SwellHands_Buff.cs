using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Thaumaturge
{
	[Package("laima-skills")]
	[BuffHandler(BuffId.SwellHands_Buff)]
	public class SwellHands_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			var percentBonus = GetCaptionRatio(buff, 1) / 100f;
			var flatBonus = GetCaptionRatio(buff, 2);

			AddPropertyModifier(buff, target, PropertyName.PATK_RATE_BM, percentBonus);
			AddPropertyModifier(buff, target, PropertyName.PATK_BM, flatBonus);

			if (target.Handle != buff.Caster?.Handle)
			{
				Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_BUFF_TEXT", (float)buff.Id, null, "Item");
			}
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_BM);
		}
	}
}
