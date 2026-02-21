using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
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
		private const float AbilityBonus = 0.005f;
		private const float BlkBonus = 200f;
		private const float BlkBonusPerLevel = 30f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var baseValue = BlkBonus;
			var byLevel = BlkBonusPerLevel * buff.NumArg1;

			var value = baseValue + byLevel;

			var byAbility = 1f;
			if (buff.Target.TryGetActiveAbilityLevel(AbilityId.Peltasta27, out var abilityLevel))
				byAbility += abilityLevel * AbilityBonus;

			value *= byAbility;
			value += 1f;

			AddPropertyModifier(buff, buff.Target, PropertyName.BLK_BM, value);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BM);
		}
	}
}
