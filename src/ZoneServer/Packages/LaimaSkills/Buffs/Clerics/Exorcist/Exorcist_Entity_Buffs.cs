using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for [Arts] Entity: Search on the Exorcist, which raises
	/// movement speed by 10.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Entity_Pad_Buff)]
	public class Exorcist_Entity_Pad_BuffOverride : BuffHandler
	{
		private const float MoveSpeedBonus = 10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			UpdatePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, MoveSpeedBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}

	/// <summary>
	/// Handler for an enemy revealed by [Arts] Entity: Search, which takes
	/// 20% more damage.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Entity_Pad_Debuff)]
	public class Exorcist_Entity_Pad_DebuffOverride : BuffHandler
	{
		private const float DamageTakenBonus = 0.20f;

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Entity_Pad_Debuff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target.IsBuffActive(BuffId.Entity_Pad_Debuff))
				modifier.FinalDamageMultiplier += DamageTakenBonus;
		}
	}
}
