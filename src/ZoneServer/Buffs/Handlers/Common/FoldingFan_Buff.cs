using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the FoldingFan_Buff, which applies a visual spinning effect to the target.
	/// </summary>
	[BuffHandler(BuffId.FoldingFan_Buff)]
	public class FoldingFan_Buff : BuffHandler
	{
		private const int SpinCount = -1;
		private const float RotationPerSecond = 0.15f;
		private const float VelocityChangeTerm = 3.5f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.SpinObject(0, SpinCount, RotationPerSecond, VelocityChangeTerm);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.SpinObject();
		}
	}
}
