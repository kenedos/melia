using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Scouts.Rogue
{
	/// <summary>
	/// Handler for the KnifeThrowing_Debuff (Bull's-eye).
	/// Increases damage taken from critical attacks.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: None
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.KnifeThrowing_Debuff)]
	public class KnifeThrowing_DebuffOverride : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Result != HitResultType.Crit)
				return;

			var skillLevel = (int)buff.NumArg1;
			var bonusDamage = 0.20f + (skillLevel * 0.02f);

			skillHitResult.Damage *= (1 + bonusDamage);
		}
	}
}
