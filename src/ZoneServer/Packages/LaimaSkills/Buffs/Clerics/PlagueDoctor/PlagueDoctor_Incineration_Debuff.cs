using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for Incineration, which burns the target every second and,
	/// with Incineration: Infect, spreads to nearby enemies when it kills.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: Snapshotted damage per tick
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Incineration_Debuff)]
	public class PlagueDoctor_Incineration_DebuffOverride : DamageOverTimeBuffHandler
	{
		private const float InfectRange = 80f;

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Fire;
		}

		protected override void OnDamageTick(Buff buff)
		{
			var target = buff.Target;

			if (!target.IsDead || buff.Caster is not ICombatEntity caster)
				return;

			if (!caster.TryGetActiveAbilityLevel(AbilityId.PlagueDoctor13, out var infectLevel))
				return;

			var victims = target.Map.GetAttackableEnemiesInPosition(caster, target.Position, InfectRange)
				.Where(a => a != target && !a.IsBuffActive(BuffId.Incineration_Debuff))
				.Take(infectLevel);

			foreach (var victim in victims)
				PlagueDoctorSkillHelper.SpreadBuff(buff, victim);
		}
	}
}
