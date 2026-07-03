using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Enchanter
{
	[Package("laima")]
	[BuffHandler(BuffId.Agility_Buff)]
	public class Enchanter_Agility_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is Character character)
			{
				var moveSpeedBonus = this.GetMoveSpeedBonus(buff, character);
				var staminaReductionRate = this.GetStaminaReductionRate(buff, character);

				buff.NumArg2 = moveSpeedBonus;
				buff.NumArg3 = staminaReductionRate;

				character.Properties.Modify(PropertyName.MSPD_BM, moveSpeedBonus);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.MSPD,
					PropertyName.MSPD_BM);
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Character character)
			{
				var moveSpeedBonus = buff.NumArg2;

				character.Properties.Modify(PropertyName.MSPD_BM, -moveSpeedBonus);

				Send.ZC_OBJECT_PROPERTY(
					character,
					PropertyName.MSPD,
					PropertyName.MSPD_BM);
			}
		}

		private float GetMoveSpeedBonus(Buff buff, Character character)
		{
			var skillLevel = buff.NumArg1;

			var moveSpeedBonus = skillLevel switch
			{
				1 => 1,
				2 => 1,
				3 => 2,
				4 => 2,
				5 => 3,
				6 => 3,
				7 => 4,
				8 => 4,
				9 => 5,
				10 => 5,
				_ => 5,
			};

			if (character.Abilities.IsActive(AbilityId.Enchanter8))
				moveSpeedBonus += 5;

			return moveSpeedBonus;
		}

		private float GetStaminaReductionRate(Buff buff, Character character)
		{
			var skillLevel = buff.NumArg1;

			// Base skill:
			// Lv1 = 6%, Lv10 = 15%
			var reductionRate = 0.05f + (0.01f * skillLevel);

			// Ability: Agility: Enhance
			if (character.Abilities.TryGet(AbilityId.Enchanter10, out var ability) && ability.Active)
			{
				var enhanceRate = ability.Level * 0.005f;

				if (ability.Level >= 100)
					enhanceRate += 0.10f;

				reductionRate *= 1f + enhanceRate;
			}

			return reductionRate;
		}
	}
}
