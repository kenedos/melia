using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Scouts.Appraiser
{
	/// <summary>
	/// Increases all damage received by the target by 10%.
	/// NumArg1 contains the percentage increase.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Devaluation_Damage_Debuff)]
	public class Devaluation_Damage_DebuffOverride :
		BuffHandler,
		IBuffCombatDefenseAfterCalcHandler
	{
		private const float DefaultDamageIncreasePercent = 10f;

		public override void OnActivate(
			Buff buff,
			ActivationType activationType
		)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnDefenseAfterCalc(
			Buff buff,
			ICombatEntity attacker,
			ICombatEntity target,
			Skill skill,
			SkillModifier modifier,
			SkillHitResult skillHitResult
		)
		{
			var damageIncreasePercent =
				buff.NumArg1 > 0
					? buff.NumArg1
					: DefaultDamageIncreasePercent;

			skillHitResult.Damage *=
				1f + damageIncreasePercent / 100f;
		}
	}
}
