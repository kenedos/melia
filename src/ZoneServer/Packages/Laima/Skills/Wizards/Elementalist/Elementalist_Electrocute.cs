using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Elementalist
{
	/// <summary>
	/// Handler override for the Elementalist skill Electrocute.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Elementalist_Electrocute)]
	public class Elementalist_ElectrocuteOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			caster.PlaySound("voice_wiz_electrocute_cast", "voice_wiz_m_electrocute_cast");
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			var designatedTarget = targets.FirstOrDefault();
			caster.TurnTowards(designatedTarget);
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			var targetList = new List<ICombatEntity>();
			if (designatedTarget != null)
			{
				targetList.Add(designatedTarget);
				var nearbyTargets = designatedTarget.Map.GetAttackableEntitiesInRangeAroundEntity(caster, designatedTarget, 150, skill.Level + 1)
					.Where(t => t != designatedTarget);
				targetList.AddRange(nearbyTargets);
			}
			skill.Run(this.TargetChainDamage(caster, skill, targetList, "I_laser005_blue#Dummy_effect_electrocute", 4, 0.1));
		}

		/// <summary>
		/// Executes chain damage logic for a skill with specified parameters.
		/// </summary>
		private async Task TargetChainDamage(
			ICombatEntity caster,
			Skill skill,
			List<ICombatEntity> targetList,
			string effect,
			double effectScale,
			double chainDuration)
		{
			if (caster == null || caster.IsDead)
				return;

			if (targetList.Count == 0)
				return;

			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			var maxBounces = 20;
			var bounceRange = 150f;
			var chainTargets = new List<ICombatEntity>();
			var hitKeyActorList = new List<(int, int)>();
			var alreadyHit = new HashSet<ICombatEntity>();

			var currentTarget = targetList[0];
			ICombatEntity previousTarget = null;

			for (var bounce = 0; bounce < maxBounces; bounce++)
			{
				if (currentTarget == null || currentTarget.IsDead)
					break;

				if (!caster.IsEnemy(currentTarget))
					continue;

				var key = caster.GenerateSyncKey();
				hitKeyActorList.Add((currentTarget.Handle, key));
				chainTargets.Add(currentTarget);
				alreadyHit.Add(currentTarget);

				var nearbyTargets = currentTarget.Map.GetAttackableEnemiesInPosition(caster, currentTarget.Position, bounceRange)
					.Where(t => t != currentTarget && !t.IsDead);

				var nextTarget = nearbyTargets
					.Where(t => !alreadyHit.Contains(t))
					.OrderBy(t => t.Position.Get2DDistance(currentTarget.Position))
					.FirstOrDefault();

				if (nextTarget == null)
				{
					nextTarget = nearbyTargets
						.Where(t => t != previousTarget)
						.OrderBy(t => t.Position.Get2DDistance(currentTarget.Position))
						.FirstOrDefault();
				}

				if (nextTarget == null)
				{
					nextTarget = nearbyTargets.FirstOrDefault();
				}

				previousTarget = currentTarget;
				currentTarget = nextTarget;

				if (nextTarget == null)
					break;
			}

			caster.PlayChainEffect(effect, (float)effectScale, (float)chainDuration, hitKeyActorList.ToArray());

			var debuffedTargets = new HashSet<ICombatEntity>();
			foreach (var target in chainTargets)
			{
				if (caster.IsDead || target.IsDead)
					continue;

				var modifier = SkillModifier.Default;
				if (target.IsBuffActiveByKeyword(BuffTag.Freeze))
					modifier.DamageMultiplier = 2f;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);
				var hit = new HitInfo(caster, target, skill, skillHitResult);
				Send.ZC_HIT_INFO(caster, target, hit);

				if (caster.IsEnemy(target) && !debuffedTargets.Contains(target))
				{
					target.StartBuff(BuffId.ElectricShock, 5, 0, TimeSpan.FromSeconds(4), caster);
					debuffedTargets.Add(target);
				}

				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}
}
