using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Fever Time buff.
	/// Increases final damage based on Jolly Roger skill level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.FeverTime)]
	public class FeverTimeOverride : BuffHandler
	{
		private const float BaseDamageBonus = 1.0f;
		private const float DamageBonusPerLevel = 0.1f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.FeverTime)]
		public void OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.FeverTime, out var buff))
				return;

			if (buff.Caster is ICombatEntity caster && caster.TryGetSkill(buff.SkillId, out var buffSkill))
			{
				var level = buffSkill.Level;
				var damageBonus = BaseDamageBonus + (level * DamageBonusPerLevel);

				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				damageBonus *= 1f + SCR_Get_AbilityReinforceRate(buffSkill);

				skillHitResult.Damage += (int)(skillHitResult.Damage * damageBonus);
			}
		}
	}
}
