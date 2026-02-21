using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Psychokino
{
	/// <summary>
	/// Handler for HeavyGravity_Debuff, which significantly reduces
	/// movement speed while in the Heavy Gravity zone.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.HeavyGravity_Debuff)]
	public class HeavyGravity_DebuffOverride : BuffHandler
	{
		private const float MspdDebuffRate = 0.9f;

		/// <summary>
		/// Starts buff, heavily reducing movement speed.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = buff.Caster;

			var reduceMspd = target.Properties.GetFloat(PropertyName.MSPD) * MspdDebuffRate;

			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -reduceMspd);

			// Save original MoveType and force to Ground
			target.SetTempVar("HEAVYGRAVITY_MOVETYPE", target.MoveType.ToString());
			target.MoveType = MoveType.Normal;
		}

		/// <summary>
		/// Ends the buff, restoring movement speed.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);

			// Restore original MoveType
			if (Enum.TryParse<MoveType>(target.GetTempVarStr("HEAVYGRAVITY_MOVETYPE"), out var moveType))
				target.MoveType = moveType;
		}
	}
}
