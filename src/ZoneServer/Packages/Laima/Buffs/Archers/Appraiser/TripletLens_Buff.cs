using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Packages.Laima.Buffs.Scouts.Appraiser
{
	/// <summary>
	/// Stores the remaining Triplet Lense charges and attaches lenses
	/// to enemies attacked by the buff owner.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.TripletLens_Buff)]
	public class TripletLens_BuffOverride : BuffHandler, IBuffOnAttackHitInfoCreatedHandler
	{
		private const int SecondsPerLens = 5;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnAttackHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			var attacker = skillHitInfo.Attacker;
			var target = skillHitInfo.Target;

			if (attacker == null || target == null)
				return;

			if (attacker != buff.Target)
				return;

			if (attacker.IsDead || target.IsDead)
				return;

			if (skillHitInfo.Skill == null)
				return;

			if (skillHitInfo.Skill.Id == SkillId.Appraiser_TripletLens)
				return;

			if (target.IsBuffActive(BuffId.TripletLens_Debuff))
				return;

			var availableLenses = buff.OverbuffCounter;

			if (availableLenses <= 0)
				return;

			var consumedLenses =
				target is Mob mob && mob.Rank == MonsterRank.Boss
					? availableLenses
					: 1;

			var duration = TimeSpan.FromSeconds(
				consumedLenses * SecondsPerLens
			);

			var skillLevel = Math.Max(1, (int)buff.NumArg1);

			var appliedDebuff = target.StartBuff(
				BuffId.TripletLens_Debuff,
				skillLevel,
				consumedLenses,
				duration,
				attacker,
				SkillId.Appraiser_TripletLens
			);

			if (appliedDebuff == null)
				return;

			for (var lens = 0; lens < consumedLenses; lens++)
				buff.DecreaseOverbuff();

			if (buff.OverbuffCounter <= 0)
			{
				buff.Target.StopBuff(BuffId.TripletLens_Buff);
				return;
			}

			buff.NotifyUpdate();
		}
	}
}
