using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.OutLaw
{
	/// <summary>
	/// Handler for the Mangle Buff, which grants forced evade
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Mangle_Buff)]
	public class Mangle_BuffOverride : BuffHandler
	{
		/// <summary>
		/// Starts buff, modifying the target's movement speed.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// +100% Evasion
			AddPropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM, 1.0f);
		}

		/// <summary>
		/// Ends the buff, resetting the movement speed.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM);
		}
	}
}
