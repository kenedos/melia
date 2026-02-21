using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Swordsman
{
	/// <summary>
	/// Handler for PainBarrier_Buff, which provides knockback and
	/// knockdown immunity. Also handles the Doppelsoeldner3 ability
	/// trade-off of movement speed for damage resistance.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.PainBarrier_Buff)]
	public class PainBarrier_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		private const float MspdBonusPerLevel = 0.05f;
		private const float DrBonusPerLevel = -0.1f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			if (!target.TryGetActiveAbilityLevel(AbilityId.Doppelsoeldner3, out var abilityLevel) || abilityLevel <= 0)
				return;

			var mspdBonus = abilityLevel * MspdBonusPerLevel;
			var drBonus = abilityLevel * DrBonusPerLevel;

			target.Properties.Modify(PropertyName.MSPD_BM, mspdBonus);
			target.Properties.Modify(PropertyName.DR_BM, drBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;
			if (!target.TryGetActiveAbilityLevel(AbilityId.Doppelsoeldner3, out var abilityLevel) || abilityLevel <= 0)
				return;

			var mspdBonus = abilityLevel * MspdBonusPerLevel;
			var drBonus = abilityLevel * DrBonusPerLevel;

			target.Properties.Modify(PropertyName.MSPD_BM, -mspdBonus);
			target.Properties.Modify(PropertyName.DR_BM, -drBonus);
		}

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return KnockResult.Prevent;
		}

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return KnockResult.Prevent;
		}
	}
}
