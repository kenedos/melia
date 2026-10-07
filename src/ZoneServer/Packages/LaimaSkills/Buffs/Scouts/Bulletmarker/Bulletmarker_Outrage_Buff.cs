using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for Outrage, whose stacks the Bullet Marker's skills spend
	/// for their special effects, and which raises basic attack speed by
	/// the skill's ratio.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Outrage_Buff)]
	public class Bulletmarker_Outrage_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			UpdatePropertyModifier(buff, buff.Target, PropertyName.NormalASPD_BM, GetCaptionRatio(buff, 1));
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.NormalASPD_BM);
		}
	}
}
