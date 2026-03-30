using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;

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

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var bonus = this.GetAtkRateBonus(buff);

			if (buff.Caster is ICombatEntity casterEntity && casterEntity.TryGetSkill(buff.SkillId, out var skill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				bonus *= 1f + SCR_Get_AbilityReinforceRate(skill);
			}

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
