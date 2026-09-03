using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Foretell buff, which gives friendly targets inside
	/// the Foretell pad area a chance to avoid any incoming hit.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Foretell_Buff)]
	public class Oracle_Foretell_BuffOverride : BuffHandler
	{
		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.Foretell_Buff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Foretell_Buff, out var buff))
				return;

			var missChance = GetCaptionRatio(buff, 1);

			if (GameRandom.Get().Next(100) >= missChance)
				return;

			skillHitResult.Damage = 0;
			skillHitResult.Effect = HitEffect.SAFETY;
			skillHitResult.Result = HitResultType.Miss;
		}
	}
}
