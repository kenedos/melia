using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Priest
{
	[Package("laima")]
	[BuffHandler(BuffId.SpellShop_IncreaseMagicDEF_Buff)]
	public class SpellShop_IncreaseMagicDEF_BuffOverride : BuffHandler
	{
		private const int ShopSkillLevel = 5;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = buff.Caster as ICombatEntity;
			var mna = caster?.Properties.GetFloat(PropertyName.MNA) ?? 0f;
			var magicDefenseBonus = MathF.Floor(240f + (ShopSkillLevel - 1) * 80f + (ShopSkillLevel / 3f) * MathF.Pow(mna, 0.9f));

			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, magicDefenseBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
		}
	}
}
