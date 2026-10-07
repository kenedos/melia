using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for Black Death Steam, which poisons the target every second
	/// and may infect a nearby enemy with it.
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill level
	/// NumArg2: Snapshotted damage per tick
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.PlagueVapours_Debuff)]
	public class PlagueDoctor_PlagueVapours_DebuffOverride : DamageOverTimeBuffHandler
	{
		private const int InfectChancePerLevel = 5;
		private const float InfectRange = 80f;

		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Poison;
		}

		protected override void OnDamageTick(Buff buff)
		{
			var target = buff.Target;

			if (target.IsDead || buff.Caster is not ICombatEntity caster)
				return;

			if (GameRandom.Get().Next(100) >= buff.NumArg1 * InfectChancePerLevel)
				return;

			var victim = target.Map.GetAttackableEnemiesInPosition(caster, target.Position, InfectRange)
				.FirstOrDefault(a => a != target && !a.IsBuffActive(BuffId.PlagueVapours_Debuff));

			if (victim == null || !PlagueDoctorSkillHelper.SpreadBuff(buff, victim))
				return;

			if (target.TryGetBuff(BuffId.PlagueVapours_Crtdr_Debuff, out var crtdrDebuff))
				PlagueDoctorSkillHelper.SpreadBuff(crtdrDebuff, victim);
		}
	}

	/// <summary>
	/// Handler for Black Death Steam's critical resistance debuff.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.PlagueVapours_Crtdr_Debuff)]
	public class PlagueDoctor_PlagueVapours_Crtdr_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			AddPropertyModifier(buff, buff.Target, PropertyName.CRTDR_RATE_BM, -GetCaptionRatio(buff, 1) / 100f);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_RATE_BM);
		}
	}
}
