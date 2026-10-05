using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Priest
{
	[Package("laima")]
	[BuffHandler(BuffId.SpellShop_Aspersion_Buff)]
	public class SpellShop_Aspersion_BuffOverride : BuffHandler
	{
		private const float BaseDefenseMultiplier = 0.35f;
		private const float DefenseMultiplierPerLevel = 0.05f;
		private const int ShopSkillLevel = 5;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var defenseMultiplier = BaseDefenseMultiplier + DefenseMultiplierPerLevel * ShopSkillLevel;
			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM, defenseMultiplier);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM);
		}
	}
}
