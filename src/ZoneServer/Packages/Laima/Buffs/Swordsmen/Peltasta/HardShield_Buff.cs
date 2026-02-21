using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsman.Peltasta
{
	/// <summary>
	/// Handle for the Hard Shield Buff, which increases defense
	/// based on the defense power of the equipped shield
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.HardShield_Buff)]
	public class HardShield_BuffOverride : BuffHandler
	{
		private const float DefMultiplierPerLevel = 0.2f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, this.GetDefBonus(buff));
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
		}

		private float GetDefBonus(Buff buff)
		{
			var shieldDef = 0f;
			var byAbility = 1f;

			if (buff.Caster.Components.TryGet<InventoryComponent>(out var inv))
			{
				var lhItem = inv.GetItem(EquipSlot.LeftHand);
				if (lhItem.Data.EquipType1 == EquipType.Shield)
					shieldDef = lhItem.Data.Def;
			}

			if (buff.Target.TryGetActiveAbilityLevel(AbilityId.Peltasta37, out var abilityLevel))
				byAbility += abilityLevel * 0.005f;

			return shieldDef * DefMultiplierPerLevel * buff.NumArg1 * byAbility;
		}
	}
}
