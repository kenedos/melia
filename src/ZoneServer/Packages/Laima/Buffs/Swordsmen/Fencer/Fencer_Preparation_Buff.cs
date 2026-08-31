using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Preparation buff, which raises the target's block
	/// while the skill is being held and arms the follow-up strike.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Preparation_Buff)]
	public class Fencer_Preparation_BuffOverride : BuffHandler
	{
		private static readonly TimeSpan FollowUpDuration = TimeSpan.FromSeconds(10);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.BLK_BM, GetCaptionRatio(buff, 2));
			buff.Target.InvalidateProperties();
		}

		public override void WhileActive(Buff buff)
		{
			Fencer_RapierGuard.EndWithoutRapier(buff);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BM);
			buff.Target.InvalidateProperties();
		}

		[CombatCalcModifier(CombatCalcPhase.OnDodge, BuffId.Preparation_Buff)]
		public void OnDefenseDodge(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Preparation_Buff, out var buff))
				return;

			ArmFollowUp(buff);
		}

		[CombatCalcModifier(CombatCalcPhase.OnBlock, BuffId.Preparation_Buff)]
		public void OnDefenseBlock(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Preparation_Buff, out var buff))
				return;

			ArmFollowUp(buff);
		}

		/// <summary>
		/// Grants the follow-up buff that empowers the target's next rapier
		/// attack, carrying the level Preparation was used at.
		/// </summary>
		/// <param name="buff"></param>
		private static void ArmFollowUp(Buff buff)
		{
			buff.Target.StartBuff(BuffId.Preparation_Buff_End, buff.NumArg1, 0f, FollowUpDuration, buff.Caster, buff.SkillId);
		}
	}
}
