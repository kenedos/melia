using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Archers.Sapper
{
	/// <summary>
	/// Handler for the SpringTrap_Debuff, which immobilizes the target
	/// and increases damage taken.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.SpringTrap_Debuff)]
	public class SpringTrap_DebuffOverride : BuffHandler
	{
		private const float BaseDamageIncrease = 0.10f;
		private const float DamageIncreasePerLevel = 0.01f;

		/// <summary>
		/// Starts the debuff, immobilizing the target.
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
