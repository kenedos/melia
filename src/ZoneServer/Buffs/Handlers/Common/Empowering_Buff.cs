using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Empowering Buff, which increases the target's maximum SP.
	/// </summary>
	/// <remarks>
	/// NumArg1: The skill level.
	/// </remarks>
	[BuffHandler(BuffId.Empowering_Buff)]
	public class Empowering_Buff : BuffHandler
	{
		private const float MspBonusPerLevel = 0.1f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = buff.Caster;
			var skillLevel = buff.NumArg1;

			// Display a custom text effect showing the buff's level.
			Send.ZC_NORMAL.PlayTextEffect(buff.Target, caster, "EMPOWERING_LEVEL_TEXT", skillLevel);

			// This buff is only for player characters.
			if (buff.Target is not Character target)
				return;

			// Calculate the Base Max SP by subtracting the current bonus value from the final value.
			var maxSp = target.Properties.GetFloat(PropertyName.MSP);
			var maxSpBonus = target.Properties.GetFloat(PropertyName.MSP_BM);
			var baseMaxSp = maxSp - maxSpBonus;

			// Calculate and apply the MSP bonus.
			var mspBonus = baseMaxSp * skillLevel * MspBonusPerLevel;
			AddPropertyModifier(buff, target, PropertyName.MSP_BM, mspBonus);
		}

		public override void OnEnd(Buff buff)
		{
			// This buff is only for player characters.
			if (buff.Target is not Character)
				return;

			RemovePropertyModifier(buff, buff.Target, PropertyName.MSP_BM);
		}
	}
}
