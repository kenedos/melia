using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handle for the Barrier, Barrier is active..
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Barrier_Buff)]
	public class Barrier_BuffOverride : BuffHandler
	{
		private const string MDEFModPropName = PropertyName.MDEF_BM;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			// Calculate the defense value: 40% + 4% per skill level
			var defRate = 0.40f + (0.04f * buff.NumArg1);

			// Get current MDEF and calculate flat bonus (monsters don't have MDEF_RATE_BM)
			var currentMdef = target.Properties.GetFloat(PropertyName.MDEF);
			var mdefBonus = currentMdef * defRate;

			// Apply the defense modifier as flat value
			AddPropertyModifier(buff, target, MDEFModPropName, mdefBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Remove the defense modifier
			RemovePropertyModifier(buff, target, MDEFModPropName);
		}
	}
}
