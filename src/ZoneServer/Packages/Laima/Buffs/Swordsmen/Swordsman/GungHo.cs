using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Newtonsoft.Json.Linq;

namespace Melia.Zone.Buffs.HandlersOverrides.Swordsmen.Swordsman
{
	/// <summary>
	/// Handle for the Gung Ho Buff, which increases the target's attack.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.GungHo)]
	public class GungHoOverride : BuffHandler
	{
		private const float AtkRateBonusPerLevel = 0.03f;
		private const float AbilityBonus = 0.005f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = this.GetAtkRateBonus(buff);

			var byAbility = 0f;
			if (buff.Target.TryGetActiveAbilityLevel(AbilityId.Swordman13, out var abilityLevel))
				byAbility = abilityLevel * AbilityBonus;

			bonus *= 1f + byAbility;

			AddPropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM);
		}

		private float GetAtkRateBonus(Buff buff)
		{
			var skillLevel = buff.NumArg1;
			return skillLevel * AtkRateBonusPerLevel;
		}
	}
}
