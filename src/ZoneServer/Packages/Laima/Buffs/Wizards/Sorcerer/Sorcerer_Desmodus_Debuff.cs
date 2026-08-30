using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Blood Sucking debuff, which raises the damage the
	/// caster's own summons deal to the target.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Desmodus_Debuff)]
	public class Sorcerer_Desmodus_DebuffOverride : BuffHandler
	{
		private const float SummonDamageBonusPerLevel = 0.24f;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc_Defense, BuffId.Desmodus_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Desmodus_Debuff, out var buff))
				return;

			if (attacker is not Summon summon || summon.Owner != buff.Caster)
				return;

			skillHitResult.Damage *= 1f + SummonDamageBonusPerLevel * buff.NumArg1;
		}
	}
}
