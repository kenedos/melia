using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using System.Linq;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Packages.Laima.Buffs.Archers.PiedPiper
{
	[Package("laima")]
	[BuffHandler(BuffId.LiedDerWeltbaum_Buff)]
	public class LiedDerWeltbaum_BuffOverride : BuffHandler, IBuffOnAttackHitInfoCreatedHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int AggroUpdateIntervalMilliseconds = 1000;
		private const int AggroAmount = 100000;
		private const int MaximumProvokedMonsters = 5;
		private const float AggroRange = 160f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(AggroUpdateIntervalMilliseconds);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead || buff.Target.Map == null)
				return;

			var area = new Melia.Zone.Skills.SplashAreas.Circle(
				buff.Target.Position,
				AggroRange);

			var targets = buff.Target.Map
				.GetAttackableEnemiesIn(buff.Target, area)
				.OfType<Mob>()
				.Where(mob => !mob.IsDead)
				.Distinct()
				.OrderBy(mob =>
					buff.Target.Position.Get2DDistance(mob.Position))
				.Take(MaximumProvokedMonsters)
				.ToList();

			foreach (var mob in targets)
			{
				Melia.Zone.World.Actors.ICombatEntityExtensions.InsertHate(
					mob,
					buff.Target,
					AggroAmount);
			}
		}

		public override void OnEnd(Buff buff)
		{
		}

		public void OnAttackHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (skillHitInfo == null || skillHitInfo.Attacker != buff.Target)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var damageBonus = this.GetDamageBonus(skillLevel);

			skillHitInfo.HitInfo.Damage *= 1f + damageBonus;
		}

		private float GetDamageBonus(int skillLevel)
		{
			return (5f + skillLevel * 1.5f) / 100f;
		}
	}
}
