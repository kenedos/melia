using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers.Wizards.Necromancer
{
	[Package("laima")]
	[BuffHandler(BuffId.Disinter_Soldier_Buff)]
	public class Necromancer_Disinter_Soldier_BuffOverride : BuffHandler
	{
		private const string MaxHpVarName = "Melia.Disinter.MaxHpBonus";
		private const string PhysicalDefenseVarName = "Melia.Disinter.PhysicalDefenseBonus";
		private const string MagicDefenseVarName = "Melia.Disinter.MagicDefenseBonus";
		private const float Bonus = 0.10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var maxHpBonus = target.Properties.GetFloat(PropertyName.MHP) * Bonus;
			var physicalDefenseBonus = target.Properties.GetFloat(PropertyName.DEF) * Bonus;
			var magicDefenseBonus = target.Properties.GetFloat(PropertyName.MDEF) * Bonus;

			buff.Vars.SetFloat(MaxHpVarName, maxHpBonus);
			buff.Vars.SetFloat(PhysicalDefenseVarName, physicalDefenseBonus);
			buff.Vars.SetFloat(MagicDefenseVarName, magicDefenseBonus);

			target.Properties.Modify(PropertyName.MHP_BM, maxHpBonus);
			target.Properties.Modify(PropertyName.DEF_BM, physicalDefenseBonus);
			target.Properties.Modify(PropertyName.MDEF_BM, magicDefenseBonus);
			target.Properties.Invalidate(PropertyName.MHP, PropertyName.DEF, PropertyName.MDEF);

			var maxHp = target.Properties.GetFloat(PropertyName.MHP);

			Send.ZC_UPDATE_MHP(target, (int)maxHp);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (buff.Vars.TryGetFloat(MaxHpVarName, out var maxHpBonus))
				target.Properties.Modify(PropertyName.MHP_BM, -maxHpBonus);

			if (buff.Vars.TryGetFloat(PhysicalDefenseVarName, out var physicalDefenseBonus))
				target.Properties.Modify(PropertyName.DEF_BM, -physicalDefenseBonus);

			if (buff.Vars.TryGetFloat(MagicDefenseVarName, out var magicDefenseBonus))
				target.Properties.Modify(PropertyName.MDEF_BM, -magicDefenseBonus);

			target.Properties.Invalidate(PropertyName.MHP, PropertyName.DEF, PropertyName.MDEF);

			var maxHp = target.Properties.GetFloat(PropertyName.MHP);
			var currentHp = target.Properties.GetFloat(PropertyName.HP);

			if (currentHp > maxHp)
				target.Properties.SetFloat(PropertyName.HP, maxHp);

			Send.ZC_UPDATE_MHP(target, (int)maxHp);
		}
	}
}
