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
	[Package("laima")]
	[BuffHandler(BuffId.Finestra_Buff)]
	public class Finestra_BuffOverride : BuffHandler
	{
		// Critical rate bonuses
		private const float FlatCrtBonusBase = 15f;
		private const float FlatCrtBonusPerLevel = 1.5f;
		private const float CrtRateBonusBase = 0.10f;
		private const float CrtRateBonusPerLevel = 0.01f;

		// Block rate bonuses
		private const float FlatBlkBonusBase = 25f;
		private const float FlatBlkBonusPerLevel = 5f;
		private const float BlkRateBonusBase = 0.20f;
		private const float BlkRateBonusPerLevel = 0.02f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// 1 second update time
			buff.SetUpdateTime(1000);

			var target = buff.Target;
			var skillLevel = buff.NumArg1;

			// Calculate critical bonuses
			var flatCrtBonus = FlatCrtBonusBase + (FlatCrtBonusPerLevel * skillLevel);
			var crtRateBonus = CrtRateBonusBase + (CrtRateBonusPerLevel * skillLevel);

			// Hoplite32: Finestra Enhance - increases crit bonuses by 0.5% per level
			if (target.TryGetActiveAbilityLevel(AbilityId.Hoplite32, out var abilityLevel))
			{
				var abilityMultiplier = 1f + (abilityLevel * 0.005f);
				flatCrtBonus *= abilityMultiplier;
				crtRateBonus *= abilityMultiplier;
			}

			// Calculate block bonuses
			var flatBlkBonus = FlatBlkBonusBase + (FlatBlkBonusPerLevel * skillLevel);
			var blkRateBonus = BlkRateBonusBase + (BlkRateBonusPerLevel * skillLevel);

			// Apply critical bonuses
			AddPropertyModifier(buff, target, PropertyName.CRTHR_BM, flatCrtBonus);
			AddPropertyModifier(buff, target, PropertyName.CRTHR_RATE_BM, crtRateBonus);

			// Apply block bonuses
			AddPropertyModifier(buff, target, PropertyName.BLK_BM, flatBlkBonus);
			AddPropertyModifier(buff, target, PropertyName.BLK_RATE_BM, blkRateBonus);

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
			RemovePropertyModifier(buff, target, PropertyName.CRTHR_BM);
			RemovePropertyModifier(buff, target, PropertyName.CRTHR_RATE_BM);

			// Remove block bonuses
			RemovePropertyModifier(buff, target, PropertyName.BLK_BM);
			RemovePropertyModifier(buff, target, PropertyName.BLK_RATE_BM);

			// Restore normal attack animation
			Send.ZC_NORMAL.SkillChangeAnimation(target, SkillId.Normal_Attack);
			Send.ZC_NORMAL.SkillChangeAnimation(target, SkillId.Normal_Attack_TH);
		}
	}
}
