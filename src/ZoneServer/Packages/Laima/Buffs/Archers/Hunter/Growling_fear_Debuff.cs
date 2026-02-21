using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Hunter
{
	/// <summary>
	/// Handler for the Growling fear debuff, applied by Hunter_Growling.
	/// Shows fear emoticon and reduces target's movement speed by 50%.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Growling_fear_Debuff)]
	public class Growling_fear_DebuffOverride : BuffHandler
	{
		private const float MoveSpeedReductionRate = 0.5f;

		/// <summary>
		/// Called when the buff is activated on the target.
		/// Shows fear emoticon and reduces movement speed.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			Send.ZC_SHOW_EMOTICON(target, "I_emo_fear", buff.Duration);

			var currentMspd = target.Properties.GetFloat(PropertyName.MSPD);
			var reduction = currentMspd * MoveSpeedReductionRate;

			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -reduction);
		}

		/// <summary>
		/// Called when the buff ends.
		/// Removes the movement speed penalty.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
