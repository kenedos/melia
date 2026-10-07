using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Clerics.Zealot
{
	/// <summary>
	/// Handler for Immolation on the Zealot, which burns away 1% of their
	/// HP every 0.5 seconds without killing them and gives Zealot attacks
	/// a 20% minimum critical chance.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Immolation_Buff)]
	public class Zealot_Immolation_BuffOverride : BuffHandler
	{
		private const int BurnInterval = 500;
		private const float HpLossRate = 0.01f;
		private const float MinCritChance = 20f;
		private const float FireAttackPerLevel = 100f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			AddPropertyModifier(buff, target, PropertyName.ResFire_BM, GetCaptionRatio(buff, 1));

			if (target.TryGetActiveAbilityLevel(AbilityId.Zealot1, out var fireAttackLevel))
				AddPropertyModifier(buff, target, PropertyName.Fire_Atk_BM, fireAttackLevel * FireAttackPerLevel);

			buff.SetUpdateTime(BurnInterval);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character || character.IsDead)
				return;

			var hpLoss = Math.Min(character.MaxHp * HpLossRate, character.Hp - 1);
			if (hpLoss > 0)
				character.ModifyHp(-hpLoss);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.ResFire_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.Fire_Atk_BM);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Immolation_Buff)]
		public void OnAttackBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!attacker.IsBuffActive(BuffId.Immolation_Buff) || !skill.Data.ClassName.StartsWith("Zealot_"))
				return;

			modifier.MinCritChance = Math.Max(modifier.MinCritChance, MinCritChance);
		}
	}
}
