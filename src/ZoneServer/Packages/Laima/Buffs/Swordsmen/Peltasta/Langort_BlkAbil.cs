using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsman.Peltasta
{
	/// <summary>
	/// Contains code related to the Langort Block
	/// </summary>
	/// <remarks>
	/// This is completely identical to Momentary Block,
	/// but without the counter effect.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Langort_BlkAbil)]
	public class Langort_BlkAbilOverride : BuffHandler
	{
		private const float BlkBonus = 200f;
		private const float BlkBonusPerLevel = 30f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var baseValue = BlkBonus;
			var byLevel = BlkBonusPerLevel * buff.NumArg1;

			var value = baseValue + byLevel;

			if (buff.Caster is ICombatEntity casterEntity && casterEntity.TryGetSkill(buff.SkillId, out var skill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				value *= 1f + SCR_Get_AbilityReinforceRate(skill);
			}
			value += 1f;

			AddPropertyModifier(buff, buff.Target, PropertyName.BLK_BM, value);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BM);
		}
	}
}
