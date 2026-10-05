using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Archers.Sapper
{
	/// <summary>
	/// Handler for the LegHoldTrap_Debuff, which locks the target's
	/// movement for the duration.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.LegHoldTrap_Debuff)]
	public class LegHoldTrap_DebuffOverride : BuffHandler
	{
		/// <summary>
		/// Starts the debuff, locking the target's movement.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="activationType"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Held);
		}

		/// <summary>
		/// Ends the debuff, removing the held state.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Held);
		}
	}
}
