using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Appraiser
{
	/// <summary>
	/// Handler for the Triplet Lense buff, which attaches the caster's
	/// lenses to nearby enemies, bosses first.
	/// </summary>
	/// <remarks>
	/// A normal enemy takes one lens for 5 seconds, a boss takes every
	/// remaining lens for 2 seconds each.
	/// </remarks>
	[Package("laima-skills")]
	[BuffHandler(BuffId.TripletLens_Buff)]
	public class Appraiser_TripletLens_BuffOverride : BuffHandler
	{
		private const float LensRange = 100f;
		private static readonly TimeSpan NormalLensDuration = TimeSpan.FromSeconds(5);
		private static readonly TimeSpan BossLensDurationPerLens = TimeSpan.FromSeconds(2);

		public override void WhileActive(Buff buff)
		{
			var caster = buff.Target;
			if (caster.IsDead)
				return;

			var target = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, LensRange)
				.Where(e => !e.IsDead && !e.IsBuffActive(BuffId.TripletLens_Debuff))
				.OrderByDescending(e => e.Rank == MonsterRank.Boss)
				.FirstOrDefault();

			if (target == null)
				return;

			var isBoss = target.Rank == MonsterRank.Boss;
			var lenses = isBoss ? buff.OverbuffCounter : 1;
			var duration = isBoss ? BossLensDurationPerLens * lenses : NormalLensDuration;

			target.StartBuff(BuffId.TripletLens_Debuff, buff.NumArg1, 0, duration, caster, buff.SkillId);

			for (var i = 0; i < lenses; i++)
				buff.DecreaseOverbuff();

			if (buff.OverbuffCounter <= 0)
			{
				caster.StopBuff(BuffId.TripletLens_Buff);
				return;
			}

			buff.NotifyUpdate();
		}
	}
}
