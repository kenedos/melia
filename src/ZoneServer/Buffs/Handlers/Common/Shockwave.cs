using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Shockwave debuff, which reduces the target's movement speed.
	/// </summary>
	/// <remarks>
	/// NumArg1: The skill level, used to calculate the speed penalty.
	/// </remarks>
	[BuffHandler(BuffId.Shockwave)]
	public class Shockwave : BuffHandler
	{
		private const int SpeedPenaltyPerLevel = 10;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = buff.NumArg1;
			var speedPenalty = skillLevel * SpeedPenaltyPerLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.SPD_BM, -speedPenalty);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.SPD_BM);
		}
	}
}
