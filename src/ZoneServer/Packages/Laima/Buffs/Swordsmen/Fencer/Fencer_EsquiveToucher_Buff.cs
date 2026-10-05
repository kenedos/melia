using Melia.Shared.Data.Database;
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
	/// Nullifies direct damage while Esquive Toucher is active.
	/// True Damage is intentionally not prevented.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.EsquiveToucher_Buff)]
	public class EsquiveToucher_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.EsquiveToucher_Buff)]
		public static float OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.EsquiveToucher_Buff, out _))
				return 0f;

			if (skill == null || skill.Data.ClassType == SkillClassType.TrueDamage)
				return 0f;

			if (skillHitResult.Damage <= 0f)
				return 0f;

			skillHitResult.Damage = 0f;
			skillHitResult.Effect = HitEffect.SAFETY;
			skillHitResult.Result = HitResultType.Dodge;

			return 0f;
		}
	}
}
