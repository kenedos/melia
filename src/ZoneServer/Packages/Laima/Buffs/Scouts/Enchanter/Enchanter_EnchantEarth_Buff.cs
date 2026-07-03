using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Enchanter
{
	[Package("laima")]
	[BuffHandler(BuffId.EnchantEarth_Buff)]
	public class Enchanter_EnchantEarth_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is Character character)
			{
				var bonus = this.GetBlockPenetrationBonus(buff, character);

				buff.NumArg2 = bonus;

				character.Properties.Modify(PropertyName.BLK_BREAK_BM, bonus);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.BLK_BREAK,
					PropertyName.BLK_BREAK_BM);
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Character character)
			{
				var bonus = buff.NumArg2;

				character.Properties.Modify(PropertyName.BLK_BREAK_BM, -bonus);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.BLK_BREAK,
					PropertyName.BLK_BREAK_BM);
			}
		}

		private float GetBlockPenetrationBonus(Buff buff, Character character)
		{
			var skillLevel = buff.NumArg1;

			// Base skill formula:
			// Lv1 = 6%, Lv10 = 15%
			var bonusRate = 0.05f + (0.01f * skillLevel);

			// Ability: Enchant Earth: Enhance
			// Lv1 = +0.5%
			// Lv100 = +50% + 10% bonus = +60%
			if (character.Abilities.TryGet(AbilityId.Enchanter12, out var ability) && ability.Active)
			{
				var enhanceRate = ability.Level * 0.005f;

				if (ability.Level >= 100)
					enhanceRate += 0.10f;

				bonusRate *= 1f + enhanceRate;
			}

			var currentBlockPenetration = character.Properties.GetFloat(PropertyName.BLK_BREAK);

			return currentBlockPenetration * bonusRate;
		}
	}
}
