using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills.Combat;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	[Package("laima")]
	[BuffHandler(BuffId.DoubleBullet_Toggle_Buff)]
	public class SchwarzerReiter_DoubleBullet_Toggle_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Caster is Character character)
				SchwarzerReiterAttackHelper.UpdateMainAttack(character);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Caster is Character character)
				SchwarzerReiterAttackHelper.UpdateMainAttack(character);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.DoubleBullet_Toggle_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.DoubleBullet_Toggle_Buff, out var buff))
				return;

			modifier.BonusDamage += 75 * buff.NumArg1;
		}
	}
}
