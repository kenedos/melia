using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Fade Buff, which lowers the target's threat levels
	/// and increases magic defense.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Fade_Buff)]
	public class Fade_BuffOverride : BuffHandler
	{
		private const float BaseMagicDefenseRate = 0.10f;
		private const float MagicDefenseRatePerLevel = 0.04f;

		/// <summary>
		/// Starts buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = buff.NumArg1;
			var rate = BaseMagicDefenseRate + MagicDefenseRatePerLevel * skillLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_RATE_BM, rate);
		}

		/// <summary>
		/// Ends the buff
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_RATE_BM);
		}
	}
}
