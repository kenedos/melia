using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Fixed Movement Speed, Increase movement speed..
	/// </summary>
	[BuffHandler(BuffId.MoveSpeedFix)]
	public class MoveSpeedFix : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			target.Properties.SetFloat(PropertyName.FIXMSPD_BM, buff.NumArg1);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			target.Properties.SetFloat(PropertyName.FIXMSPD_BM, 0);
		}
	}
}
