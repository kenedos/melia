using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Miko
{
	/// <summary>
	/// Handler for the Miko skill Gohei, two swings that strip 1 to 3 buffs
	/// off the enemies and 1 to 5 removable debuffs off the allies ahead,
	/// leaving them with Mental Breakdown and Mental Recovery.
	/// </summary>
	/// <remarks>
	/// [Arts] Gohei: O-Gohei strips the enemies within 100 instead.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Miko_Gohei)]
	public class Miko_GoheiOverride : IGroundSkillHandler
	{
		private const float Length = 50f;
		private const float Width = 30f;
		private const float OGoheiRange = 100f;
		private const int MaxAllies = 5;
		private static readonly (int Time, int AniTime)[] HitTimings = [(50, 250), (200, 400)];
		private static readonly TimeSpan StripTime = TimeSpan.FromMilliseconds(330);
		private static readonly TimeSpan MentalDuration = TimeSpan.FromSeconds(10);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var area = new Square(caster.Position, caster.Direction, Length, Width);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area).LimitBySDR(caster, skill).ToList();

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Swing(skill, caster, area, targets));
		}

		/// <summary>
		/// Strikes the targets once per swing, then strips the enemies and
		/// allies in reach.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="area"></param>
		/// <param name="targets"></param>
		/// <returns></returns>
		private async Task Swing(Skill skill, ICombatEntity caster, Square area, List<ICombatEntity> targets)
		{
			var elapsed = 0;

			foreach (var timing in HitTimings)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(timing.Time - elapsed));
				elapsed = timing.Time;

				if (caster.IsDead)
					return;

				var aniTime = TimeSpan.FromMilliseconds(timing.AniTime - timing.Time);
				var hits = new List<SkillHitInfo>();

				foreach (var enemy in targets.Where(t => !t.IsDead))
				{
					var skillHitResult = SCR_SkillHit(caster, enemy, skill);
					enemy.TakeDamage(skillHitResult.Damage, caster);

					hits.Add(new SkillHitInfo(caster, enemy, skill, skillHitResult, aniTime, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}

			await skill.Wait(StripTime - TimeSpan.FromMilliseconds(elapsed));

			if (caster.IsDead)
				return;

			var enemies = caster.IsAbilityActive(AbilityId.Miko9)
				? caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, OGoheiRange)
				: caster.Map.GetAttackableEnemiesIn(caster, area);

			foreach (var enemy in enemies)
				this.Strip(skill, caster, enemy, BuffType.Buff, 3, BuffId.MentalCollapse_Debuff);

			foreach (var ally in PartySkillHelper.GetAlliesInRange(caster, caster.Position, Length).Where(a => area.IsInside(a.Position)).Take(MaxAllies))
				this.Strip(skill, caster, ally, BuffType.Debuff, 5, BuffId.MentalRecovery_Buff);
		}

		/// <summary>
		/// Strips 1 to the given number of removable buffs of the type off
		/// the entity and gives it the result buff for each one removed.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="entity"></param>
		/// <param name="type"></param>
		/// <param name="maxRemoved"></param>
		/// <param name="resultBuffId"></param>
		private void Strip(Skill skill, ICombatEntity caster, ICombatEntity entity, BuffType type, int maxRemoved, BuffId resultBuffId)
		{
			if (!entity.Components.TryGet<BuffComponent>(out var buffComponent))
				return;

			var removable = buffComponent.GetList().Where(a => a.Data.Type == type && a.Data.Removable).Take(GameRandom.Get().Next(1, maxRemoved + 1)).ToList();
			if (removable.Count == 0)
				return;

			foreach (var buff in removable)
				buffComponent.Remove(buff.Id);

			entity.StartBuff(resultBuffId, skill.Level, removable.Count, MentalDuration, caster, skill.Id);
		}
	}
}
