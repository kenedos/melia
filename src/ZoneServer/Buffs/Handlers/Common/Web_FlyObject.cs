using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Common
{
	/// <summary>
	/// Handler for Web_FlyObject (Snatching), which slows down enemies
	/// by half their movement speed and forces them to ground movement type.
	/// </summary>
	[BuffHandler(BuffId.Web_FlyObject)]
	public class Web_FlyObject : BuffHandler
	{
		private const float MspdDebuffRate = 0.5f;

		/// <summary>
		/// Starts buff, reducing movement speed by half and forcing ground movement.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			// Reduce movement speed by half
			var reduceMspd = target.Properties.GetFloat(PropertyName.MSPD) * MspdDebuffRate;
			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -reduceMspd);

			// Save original MoveType and force to Ground
			target.SetTempVar("WEBFLY_MOVETYPE", target.MoveType.ToString());
			target.MoveType = MoveType.Normal;
		}

		/// <summary>
		/// Ends the buff, restoring movement speed and original movement type.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Restore movement speed
			RemovePropertyModifier(buff, target, PropertyName.MSPD_BM);

			// Restore original MoveType
			if (Enum.TryParse<MoveType>(target.GetTempVarStr("WEBFLY_MOVETYPE"), out var moveType))
				target.MoveType = moveType;
		}
	}
}
