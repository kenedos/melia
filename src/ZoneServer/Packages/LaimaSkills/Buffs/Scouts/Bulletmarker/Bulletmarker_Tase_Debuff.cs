using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Bulletmarker
{
	/// <summary>
	/// Handler for Tase, which adds a Lightning shock to the next 10 hits
	/// the target takes.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: Damage of each shock
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Tase_Debuff)]
	public class Bulletmarker_Tase_DebuffOverride : BuffHandler
	{
		private const int MaxShocks = 10;
		private const string ShocksVar = "Melia.Bulletmarker.TaseShocks";

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Tase_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skill.Id == SkillId.Bulletmarker_NapalmBullet || skillHitResult.Damage <= 0)
				return;

			if (!target.TryGetBuff(BuffId.Tase_Debuff, out var buff))
				return;

			skillHitResult.AddExtraLine(buff.NumArg2, SkillId.Bulletmarker_Tase);

			var shocks = buff.Vars.GetInt(ShocksVar) + 1;
			buff.Vars.SetInt(ShocksVar, shocks);

			if (shocks >= MaxShocks)
				target.StopBuff(BuffId.Tase_Debuff);
		}
	}
}
