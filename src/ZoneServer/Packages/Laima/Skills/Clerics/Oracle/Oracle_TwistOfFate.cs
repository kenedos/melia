using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handler for the Oracle skill Twist Of Fate.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Oracle_TwistOfFate)]
	public class Oracle_TwistOfFateOverride : IGroundSkillHandler
	{
		private const float CenterDistance = 35f;
		private const float Radius = 70f;
		private const int MinDamageRate = 40;
		private const int MaxDamageRate = 50;
		private const int HealSeconds = 5;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			var centerPos = caster.Position.GetRelative(caster.Direction, CenterDistance);
			var maxTargets = OracleSkillHelper.GetTargetCount(skill);

			// The skill cannot kill, so an enemy already down to its last
			// point of health has nothing left for it to take. Already
			// sentenced enemies are taken last, closest first within each
			// group as the selection returns them
			var skillTargets = SkillSelectEnemiesInCircle(caster, centerPos, Radius)
				.Where(a => a.Properties.GetFloat(PropertyName.HP) > 1)
				.OrderBy(a => a.IsBuffActive(BuffId.DeathVerdict_Buff))
				.Take(maxTargets)
				.ToList();

			await skill.Wait(TimeSpan.FromMilliseconds(600));

			foreach (var skillTarget in skillTargets)
			{
				if (skillTarget.IsDead)
					continue;

				var damage = this.GetDamage(skillTarget);
				if (damage <= 0)
					continue;

				// The health is only borrowed, so a target that cannot take
				// the debuff back does not lose any in the first place
				var healPerTick = damage / (HealSeconds * 2);
				var buff = skillTarget.StartBuff(BuffId.TwistOfFate_Debuff, skill.Level, healPerTick, TimeSpan.FromSeconds(HealSeconds), caster, skill.Id);
				if (buff == null)
					continue;

				// The share taken is fixed, so the skill has no use for the
				// rolls the pipeline would otherwise make
				var modifier = SkillModifier.Default;
				modifier.ForcedHit = true;
				modifier.Unblockable = true;
				modifier.Uncrittable = true;

				var skillHitResult = SCR_SkillHit(caster, skillTarget, skill, modifier);
				skillHitResult.Damage = damage;

				skillTarget.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, skillTarget, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
				Send.ZC_SKILL_HIT_INFO(caster, skillHit);
			}
		}

		/// <summary>
		/// Returns how much health the skill takes from the target, always
		/// leaving it with at least one point.
		/// </summary>
		/// <param name="target"></param>
		/// <returns></returns>
		private float GetDamage(ICombatEntity target)
		{
			var hp = target.Properties.GetFloat(PropertyName.HP);
			var damageRate = GameRandom.Get().Next(MinDamageRate, MaxDamageRate + 1);

			if (target is Mob mob && mob.Rank == MonsterRank.Boss)
				damageRate /= 2;

			var damage = (float)Math.Floor(hp * damageRate / 100f);

			return Math.Min(damage, hp - 1);
		}
	}
}
