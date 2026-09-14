using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsman.Peltasta
{
	/// <summary>
	/// Handle for the Hard Shield Buff, which increases defense
	/// based on the defense power of the equipped shield
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.HardShield_Buff)]
	public class HardShield_BuffOverride : BuffHandler
	{
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

			if (buff.Caster.Components.TryGet<InventoryComponent>(out var inv))
			{
				var lhItem = inv.GetItem(EquipSlot.LeftHand);
				if (lhItem.Data.EquipType1 == EquipType.Shield)
					shieldDef = lhItem.Data.Def;
			}

			return shieldDef * (GetCaptionRatio(buff, 1) / 100f);
		}
	}
}
