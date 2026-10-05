using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.Buffs;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Priest
{
	[Package("laima")]
	[BuffHandler(BuffId.SpellShop_Blessing_Buff)]
	public class SpellShop_Blessing_BuffOverride : BuffHandler
	{
		private const float BaseAttackMultiplier = 0.15f;
		private const float AttackMultiplierPerLevel = 0.015f;
		private const int ShopSkillLevel = 5;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var attackMultiplier = BaseAttackMultiplier + AttackMultiplierPerLevel * ShopSkillLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM, attackMultiplier);
			AddPropertyModifier(buff, buff.Target, PropertyName.MATK_RATE_BM, attackMultiplier);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MATK_RATE_BM);
		}
	}
}
