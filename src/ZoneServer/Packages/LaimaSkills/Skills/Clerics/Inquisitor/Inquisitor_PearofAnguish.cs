using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Inquisitor
{
	/// <summary>
	/// Handler for the Inquisitor skill Pear of Anguish, which scatters 5
	/// pears on the ground that burst on the first enemy to come near, and
	/// fly at enemies casting magic nearby.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Inquisitor_PearofAnguish)]
	public class Inquisitor_PearofAnguishOverride : IGroundSkillHandler
	{
		private const string PearsVar = "Melia.Inquisitor.Pears";
		private const int PearCount = 5;
		private const int Spread = 25;
		private const float TriggerRange = 20f;
		private const float MagicDetectRange = 150f;
		private const float PearLifeTime = 30f;
		private static readonly TimeSpan InstallDelay = TimeSpan.FromMilliseconds(600);
		private static readonly TimeSpan CheckInterval = TimeSpan.FromMilliseconds(200);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
				targetPos = farPos;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, ForceId.GetNew(), null);

			skill.Run(this.Install(skill, caster, targetPos));
		}

		/// <summary>
		/// Scatters the pears around the target position, removing the
		/// oldest ones beyond the installation limit.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targetPos"></param>
		/// <returns></returns>
		private async Task Install(Skill skill, ICombatEntity caster, Position targetPos)
		{
			await skill.Wait(InstallDelay);

			if (caster.IsDead)
				return;

			var pears = this.GetPears(skill);
			var watching = this.GetLivePears(skill, caster).Count > 0;

			for (var i = 0; i < PearCount; i++)
			{
				var position = caster.Map.Ground.GetLastValidPosition(targetPos, targetPos.GetRandomInRange2D(Spread));

				var pear = MonsterSkillCreateMob(skill, caster, "pcskill_pear_of_anquish", position, 0, "", "", 0, PearLifeTime, "MON_DUMMY", "");
				if (pear == null)
					continue;

				pear.MonsterType = RelationType.Friendly;
				pear.Faction = FactionType.Law;
				pear.SetHittable(false);
				pear.StartBuff(BuffId.Invincible);

				lock (pears)
					pears.Add(pear);
			}

			var maxPears = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio2);
			foreach (var oldPear in this.GetLivePears(skill, caster).SkipLast(maxPears))
				this.Remove(skill, caster, oldPear);

			if (!watching)
				skill.RunFree(this.Watch(skill, caster));
		}

		/// <summary>
		/// Bursts each pear on the first enemy that comes near it, sending it
		/// first at any enemy casting magic within range, until no pears are
		/// left.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Watch(Skill skill, ICombatEntity caster)
		{
			while (true)
			{
				await skill.Wait(CheckInterval, false);

				var pears = this.GetLivePears(skill, caster);
				if (pears.Count == 0 || caster.IsDead)
					return;

				foreach (var pear in pears)
				{
					var victim = caster.Map.GetAttackableEnemiesInPosition(caster, pear.Position, TriggerRange).FirstOrDefault();
					if (victim != null)
					{
						this.Burst(skill, caster, pear);
						continue;
					}

					var spellcaster = caster.Map.GetAttackableEnemiesInPosition(caster, pear.Position, MagicDetectRange).FirstOrDefault(IsCastingMagic);
					if (spellcaster != null)
						pear.Components.Get<MovementComponent>()?.MoveTo(spellcaster.Position);
				}
			}
		}

		/// <summary>
		/// Bursts the pear, striking the enemies around it.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="pear"></param>
		private void Burst(Skill skill, ICombatEntity caster, Mob pear)
		{
			var hits = new List<SkillHitInfo>();

			foreach (var target in caster.Map.GetAttackableEnemiesInPosition(caster, pear.Position, TriggerRange).LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(caster, hits);

			this.Remove(skill, caster, pear);
		}

		/// <summary>
		/// Takes the pear off the map and out of the skill's list.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="pear"></param>
		private void Remove(Skill skill, ICombatEntity caster, Mob pear)
		{
			var pears = this.GetPears(skill);
			lock (pears)
				pears.Remove(pear);

			caster.Map.RemoveMonster(pear);
		}

		/// <summary>
		/// Returns the skill's list of pears, creating it on first use.
		/// </summary>
		/// <param name="skill"></param>
		/// <returns></returns>
		private List<Mob> GetPears(Skill skill)
		{
			lock (skill.Vars)
			{
				if (!skill.Vars.TryGet<List<Mob>>(PearsVar, out var pears))
				{
					pears = new List<Mob>();
					skill.Vars.Set(PearsVar, pears);
				}

				return pears;
			}
		}

		/// <summary>
		/// Returns a copy of the caster's pears that are still standing,
		/// oldest first, dropping the ones that are gone from the list.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private List<Mob> GetLivePears(Skill skill, ICombatEntity caster)
		{
			var pears = this.GetPears(skill);

			lock (pears)
			{
				pears.RemoveAll(pear => pear.Map != caster.Map || !caster.Map.TryGetMonster(pear.Handle, out _));
				return pears.ToList();
			}
		}

		/// <summary>
		/// Returns true if the entity is using a magic skill.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		private static bool IsCastingMagic(ICombatEntity entity)
		{
			var skillId = entity.GetCurrentSkill();
			if (skillId == SkillId.None || skillId == SkillId.Normal_Attack)
				return false;

			return ZoneServer.Instance.Data.SkillDb.TryFind(skillId, out var skillData) && skillData.AttackType == SkillAttackType.Magic;
		}
	}
}
