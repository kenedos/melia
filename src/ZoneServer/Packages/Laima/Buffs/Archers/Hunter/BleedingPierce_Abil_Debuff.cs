using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers.Archers.Hunter
{
	/// <summary>
	/// Handler for the BleedingPierce Ability Debuff (with Hunter25 ability).
	/// Reduces target's movement speed by 25% and healing received by 50%.
	/// Healing reduction is handled in SCR_CalculateHeal.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.BleedingPierce_Abil_Debuff)]
	public class BleedingPierce_Abil_DebuffOverride : BuffHandler
	{
		private const float MoveSpeedReduction = 0.25f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			var mspdReduction = target.Properties.GetFloat(PropertyName.MSPD) * MoveSpeedReduction;
			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -mspdReduction);
			Send.ZC_MOVE_SPEED(target);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
			Send.ZC_MOVE_SPEED(buff.Target);
		}
	}
}
