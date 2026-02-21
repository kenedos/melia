using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsman.Peltasta
{
	/// <summary>
	/// Handle for the High Guard Buff, which increases the target's block rate
	/// and critical defense, but prevents evasion.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.HighGuard_Buff)]
	public class HighGuard_BuffOverride : BuffHandler
	{
		private const float FlatBlkBonusBase = 20f;
		private const float FlatBlkBonusPerLevel = 8f;
		private const float BlkRateBonusBase = 0.20f;
		private const float BlkRateBonusPerLevel = 0.02f;
		private const float CrtDrRateBonusBase = 0.30f;
		private const float CrtDrRateBonusPerLevel = 0.02f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var skillLevel = buff.NumArg1;

			var flatBlkBonus = FlatBlkBonusBase + (FlatBlkBonusPerLevel * skillLevel);
			var blkBonus = BlkRateBonusBase + (BlkRateBonusPerLevel * skillLevel);
			var crtResistBonus = CrtDrRateBonusBase + (CrtDrRateBonusPerLevel * skillLevel);

			if (target.TryGetActiveAbilityLevel(AbilityId.Peltasta36, out var abilityLevel))
			{
				flatBlkBonus *= 1f + (abilityLevel * 0.005f);
				blkBonus *= 1f + (abilityLevel * 0.005f);
				crtResistBonus *= 1f + (abilityLevel * 0.005f);
			}

			AddPropertyModifier(buff, target, PropertyName.BLK_BM, flatBlkBonus);
			AddPropertyModifier(buff, target, PropertyName.BLK_RATE_BM, blkBonus);
			AddPropertyModifier(buff, target, PropertyName.CRTDR_RATE_BM, crtResistBonus);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.BLK_BM);
			RemovePropertyModifier(buff, target, PropertyName.BLK_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.CRTDR_RATE_BM);
		}
	}
}
