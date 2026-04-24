using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for FirstStrike_Buff (Pre-Emptive Strike toggle).
	/// When the owner attacks an enemy, the hawk gains hate toward
	/// that target so the AI's CheckFirstStrike can pick it up and
	/// use the full skill rotation with proper GCD. Continuously
	/// drains SP while active and reduces the owner's outgoing
	/// skill damage.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.FirstStrike_Buff)]
	public class Falconer_FirstStrike_BuffOverride : BuffHandler
	{
		private const int SpDrainPerSecond = 8;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(1000);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			if (!character.TrySpendSp(SpDrainPerSecond))
			{
				character.StopBuff(BuffId.FirstStrike_Buff);
				return;
			}
		}

		/// <summary>
		/// After attack calculations are complete, transfer hate to
		/// the hawk so it targets the same enemy independently.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.AfterCalc_Attack, BuffId.FirstStrike_Buff)]
		public static float OnAttackAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.FirstStrike_Buff, out var buff))
				return 0;

			if (skill.Id == SkillId.Falconer_BlisteringThrash
				|| skill.Id == SkillId.Falconer_Pheasant
				|| skill.Id == SkillId.Falconer_Tomahawk)
				return 0;

			if (!FalconerHawkHelper.TryGetHawk(attacker, out var hawk))
				return 0;

			if (hawk.Components.TryGet<AiComponent>(out var ai))
				ai.Script.AddHate(target, 150);

			return 0;
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeBonuses, BuffId.FirstStrike_Buff)]
		public void OnAttackBeforeBonuses(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.TryGetBuff(BuffId.FirstStrike_Buff, out var buff))
				return;

			var skillLevel = (int)buff.NumArg1;
			var percent = 50 + skillLevel * 5;
			if (percent > 100)
				percent = 100;

			modifier.DamageMultiplier *= percent / 100f;
		}
	}
}
