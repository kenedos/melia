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
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Corsair skill Keel Hauling.
	/// Multi-hit forward attack that pulls enemies towards the caster.
	/// Also pulls enemies hooked with Iron Hook.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Corsair_Keelhauling)]
	public class Corsair_KeelhaulingOverride : IMeleeGroundSkillHandler
	{
		private const float SplashLength = 80f;
		private const float SplashWidth = 20f;
		private const int BaseKnockdownVelocity = 300;
		private const int MinKnockdownVelocity = 50;
		private const float MaxPullDistance = 300f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);

			var position = GetRelativePosition(PosType.Self, caster, distance: 20, height: 20);
			Send.ZC_GROUND_EFFECT(caster, position, "F_scout_behead001", 0.60000002f, 0f, 0f, 0f);
			position = GetRelativePosition(PosType.Self, caster, distance: 30, height: 20);
			Send.ZC_GROUND_EFFECT(caster, position, "F_archer_broadhead_cast_blooding", 1.5f, 0f, 0f, 180f);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40f, width: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var aoeTargets = caster.Map.GetAttackableEnemiesIn(caster, splashArea).ToList();
			var hookedTargets = this.GetHookedTargets(caster);

			var knockedDownTargets = new HashSet<ICombatEntity>();
			var processedTargets = new HashSet<ICombatEntity>();
			var hits = new List<SkillHitInfo>();

			foreach (var target in aoeTargets.LimitBySDR(caster, skill))
			{
				processedTargets.Add(target);
				this.ProcessTarget(caster, skill, target, knockedDownTargets, hits);
			}

			foreach (var target in hookedTargets)
			{
				if (processedTargets.Contains(target))
					continue;

				processedTargets.Add(target);
				this.ProcessTarget(caster, skill, target, knockedDownTargets, hits);
			}

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), hits);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos, hookedTargets, knockedDownTargets));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos, List<ICombatEntity> hookedTargets, HashSet<ICombatEntity> knockedDownTargets)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40f, width: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var aoeTargets = caster.Map.GetAttackableEnemiesIn(caster, splashArea).ToList();

			await skill.Wait(TimeSpan.FromMilliseconds(150));

			var processedTargets = new HashSet<ICombatEntity>();
			var hits = new List<SkillHitInfo>();

			foreach (var target in aoeTargets.LimitBySDR(caster, skill))
			{
				processedTargets.Add(target);
				this.ProcessTarget(caster, skill, target, knockedDownTargets, hits);
			}

			foreach (var target in hookedTargets)
			{
				if (processedTargets.Contains(target))
					continue;

				processedTargets.Add(target);
				this.ProcessTarget(caster, skill, target, knockedDownTargets, hits);
			}

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), hits);
		}

		private const int BleedingDurationSeconds = 8;
		private const int BleedingTickCount = 8;

		private void ProcessTarget(ICombatEntity caster, Skill skill, ICombatEntity target, HashSet<ICombatEntity> knockedDownTargets, List<SkillHitInfo> hits)
		{
			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
			skillHit.HitEffect = HitEffect.Impact;

			if (skillHitResult.Damage > 0 && !knockedDownTargets.Contains(target))
			{
				knockedDownTargets.Add(target);

				if (target.IsKnockdownable())
				{
					var velocity = this.CalculatePullVelocity(caster, target);
					skillHit.KnockBackInfo = new KnockBackInfo(caster, target, HitType.KnockDown, velocity, 30, KnockDirection.TowardsCaster);
					skillHit.HitInfo.Type = HitType.KnockDown;
					target.ApplyKnockdown(caster, skill, skillHit);
				}

				// Approx. 100% Skill factor bleed damage, as
				// skillHitResult.Damage is the damage of a single hit
				var bleedDamagePerTick = (skillHitResult.Damage * 2f) / BleedingTickCount;
				target.StartBuff(BuffId.HeavyBleeding, skill.Level, bleedDamagePerTick, TimeSpan.FromSeconds(BleedingDurationSeconds), caster);
			}

			hits.Add(skillHit);
		}

		private List<ICombatEntity> GetHookedTargets(ICombatEntity caster)
		{
			if (!caster.TryGetBuff(BuffId.IronHook, out var buff))
				return new List<ICombatEntity>();

			if (!buff.Vars.TryGet<List<ICombatEntity>>("Melia.IronHook.Targets", out var targets))
				return new List<ICombatEntity>();

			return targets.Where(t => t != null && !t.IsDead).ToList();
		}

		private int CalculatePullVelocity(ICombatEntity caster, ICombatEntity target)
		{
			var distance = (float)caster.Position.Get2DDistance(target.Position);
			var distanceRatio = Math.Clamp(distance / MaxPullDistance, 0f, 1f);
			var velocity = (int)(MinKnockdownVelocity + (BaseKnockdownVelocity - MinKnockdownVelocity) * distanceRatio);
			return velocity;
		}
	}
}
