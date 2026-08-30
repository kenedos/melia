using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Necromancer Bane debuff, which raises the damage the
	/// caster's own summons deal to the target.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.NecromancerPoison_Debuff)]
	public class Necromancer_NecromancerPoison_DebuffOverride : BuffHandler
	{
		private const float SummonDamageBonus = 0.20f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc_Defense, BuffId.NecromancerPoison_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.NecromancerPoison_Debuff, out var buff))
				return;

			if (attacker is not Summon summon || summon.Owner != buff.Caster)
				return;

			skillHitResult.Damage *= 1f + SummonDamageBonus;
		}
	}
}
