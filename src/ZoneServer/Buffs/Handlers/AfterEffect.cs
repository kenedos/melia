using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Resurrection After Effects.
	/// The after effect of being revived, which decreases combat stats.
	/// </summary>
	[BuffHandler(BuffId.AfterEffect)]
	public class AfterEffect : BuffHandler
	{
		private const float AttackPenalty = -0.5f;
		private const float DefensePenalty = -0.5f;
		private const float MagicAttackPenalty = -0.5f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			// Apply penalties using the helper methods.
			AddPropertyModifier(buff, target, PropertyName.ATK_BM, AttackPenalty);
			AddPropertyModifier(buff, target, PropertyName.DEF_RATE_BM, DefensePenalty);
			AddPropertyModifier(buff, target, PropertyName.MATK_RATE_BM, MagicAttackPenalty);

			if (target is Character character)
				character.InvalidateProperties(PropertyName.ATK_BM, PropertyName.DEF_RATE_BM, PropertyName.MATK_RATE_BM);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Revert penalties using the helper methods.
			RemovePropertyModifier(buff, target, PropertyName.ATK_BM);
			RemovePropertyModifier(buff, target, PropertyName.DEF_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MATK_RATE_BM);

			if (target is Character character)
				character.InvalidateProperties(PropertyName.ATK_BM, PropertyName.DEF_RATE_BM, PropertyName.MATK_RATE_BM);
		}
	}
}
