using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scout
{
	/// <summary>
	/// Handle for the Double Attack Buff, which increases the target's
	/// crit and physical attack rate.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DoubleAttack_Buff)]
	public class DoubleAttack_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var level = buff.NumArg1;
			var bonus = 0.1f + 0.02f * level;

			var byAbility = 1f;
			if (buff.Caster is ICombatEntity casterEntity && casterEntity.TryGetSkill(buff.SkillId, out var skill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				byAbility += SCR_Get_AbilityReinforceRate(skill);
			}

			bonus *= byAbility;

			AddPropertyModifier(buff, buff.Target, PropertyName.CRTHR_RATE_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTHR_RATE_BM);
		}
	}
}
