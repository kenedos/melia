using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Clerics.Zealot
{
	/// <summary>
	/// Handler for the Emphatic Trust debuff, which adds a Holy strike from
	/// the Zealot to the first 10 attacks the target takes.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.EmphasisTrust_Debuff)]
	public class Zealot_EmphasisTrust_DebuffOverride : BuffHandler
	{
		private const int MaxStrikes = 10;
		private const string StrikesVar = "Melia.Zealot.EmphasisTrustStrikes";

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.EmphasisTrust_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Id == SkillId.Zealot_EmphasisTrust || skillHitResult.Damage <= 0)
				return;

			if (!target.TryGetBuff(BuffId.EmphasisTrust_Debuff, out var buff) || buff.Caster is not ICombatEntity zealot)
				return;

			if (!zealot.TryGetSkill(SkillId.Zealot_EmphasisTrust, out var trustSkill))
				return;

			var strikeResult = SCR_SkillHit(zealot, target, trustSkill);
			skillHitResult.AddExtraLine(strikeResult.Damage, SkillId.Zealot_EmphasisTrust);

			var strikes = buff.Vars.GetInt(StrikesVar) + 1;
			buff.Vars.SetInt(StrikesVar, strikes);

			if (strikes >= MaxStrikes)
				target.StopBuff(BuffId.EmphasisTrust_Debuff);
		}
	}
}
