using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsmen.Hoplite
{
	/// <summary>
	/// Handle override for the Finestra Buff, which increases critical rate
	/// and block rate.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Finestra_Buff)]
	public class Finestra_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// 1 second update time
			buff.SetUpdateTime(1000);

			var target = buff.Target;

			// Apply critical bonuses
			AddPairedPropertyModifier(buff, target, PropertyName.CRTHR_BM, PropertyName.CRTHR_RATE_BM, GetCaptionRatio(buff, 1));

			// Apply block bonuses
			AddPairedPropertyModifier(buff, target, PropertyName.BLK_BM, PropertyName.BLK_RATE_BM, GetCaptionRatio(buff, 2));

			// Change attack animation
			Send.ZC_NORMAL.SkillChangeAnimation(target, SkillId.Normal_Attack, "SKL_FINESTRA_ATK");
			Send.ZC_NORMAL.SkillChangeAnimation(target, SkillId.Normal_Attack_TH, "SKL_FINESTRA_ATK");
		}

		public override void WhileActive(Buff buff)
		{
			// Check if the character is still wielding a spear or two-handed spear
			if (buff.Target is Character character)
			{
				var weapon = character.Inventory.GetItem(EquipSlot.RightHand);
				if (weapon == null || (weapon.Data.EquipType1 != EquipType.Spear && weapon.Data.EquipType1 != EquipType.THSpear))
				{
					// Remove buff if weapon is no longer a spear
					buff.Target.StopBuff(BuffId.Finestra_Buff);
				}
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Remove critical bonuses
			RemovePairedPropertyModifier(buff, target, PropertyName.CRTHR_BM, PropertyName.CRTHR_RATE_BM);

			// Remove block bonuses
			RemovePairedPropertyModifier(buff, target, PropertyName.BLK_BM, PropertyName.BLK_RATE_BM);

			// Restore normal attack animation
			Send.ZC_NORMAL.SkillChangeAnimation(target, SkillId.Normal_Attack);
			Send.ZC_NORMAL.SkillChangeAnimation(target, SkillId.Normal_Attack_TH);
		}
	}
}
