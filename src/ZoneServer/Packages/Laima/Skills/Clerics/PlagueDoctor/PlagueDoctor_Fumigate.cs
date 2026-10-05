using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.PlagueDoctor
{
	/// <summary>
	/// Fumigate.
	/// Removes removable debuffs from nearby allies or periodically damages nearby enemies when Fumigate: Perfusion is active.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.PlagueDoctor_Fumigate)]
	public class PlagueDoctor_Fumigate : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MaximumRemovedDebuffs = 3;
		private const int MaximumRemovableDebuffLevel = 5;
		private const int MaximumPerfusionTargets = 10;
		private const int PerfusionPulseCount = 10;
		private const int MaximumSkillLevel = 10;
		private const int MaximumEnhanceLevel = 100;
		private const float FumigateRadius = 100f;
		private const float PerfusionFactorAtLevelOne = 112f;
		private const float PerfusionFactorAtLevelTen = 162f;
		private static readonly TimeSpan CastDelay = TimeSpan.FromMilliseconds(800);
		private static readonly TimeSpan PerfusionPulseInterval = TimeSpan.FromSeconds(1);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (caster is not Character character || caster.IsDead)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			if (character.IsAbilityActive(AbilityId.PlagueDoctor29))
				skill.Run(this.ExecutePerfusion(skill, character));
			else
				skill.Run(this.ExecuteFumigate(skill, character));
		}

		private async Task ExecuteFumigate(Skill skill, Character caster)
		{
			await skill.Wait(CastDelay);

			if (caster.IsDead || caster.Map == null)
			{
				caster.SetAttackState(false);
				return;
			}

			var purificationPosition = caster.Position;
			var allies = this.GetAlliesInRange(caster, purificationPosition);

			foreach (var ally in allies)
				this.ApplyOriginalFumigateEffects(skill, caster, ally);

			if (caster.IsAbilityActive(AbilityId.PlagueDoctor6))
				await this.MaintainPurificationArea(skill, caster, purificationPosition);

			caster.SetAttackState(false);
		}

		private async Task ExecutePerfusion(Skill skill, Character caster)
		{
			await skill.Wait(CastDelay);

			for (var pulse = 0; pulse < PerfusionPulseCount; pulse++)
			{
				if (caster.IsDead || caster.Map == null)
					break;

				this.ExecutePerfusionPulse(skill, caster);

				if (pulse < PerfusionPulseCount - 1)
					await skill.Wait(PerfusionPulseInterval);
			}

			caster.SetAttackState(false);
		}

		private void ExecutePerfusionPulse(Skill skill, Character caster)
		{
			var area = new Circle(caster.Position, FumigateRadius);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area).Where(target => target != null && !target.IsDead).OrderBy(target => caster.Position.Get2DDistance(target.Position)).Take(MaximumPerfusionTargets).ToList();
			var perfusionMultiplier = this.GetPerfusionFactor(skill.Level) / 100f;
			perfusionMultiplier *= this.GetPerfusionEnhanceMultiplier(caster);

			foreach (var target in targets)
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				skillHitResult.Damage *= perfusionMultiplier;

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
				skillHit.HitEffect = HitEffect.Impact;
				skillHit.ApplyDamage();

				Send.ZC_HIT_INFO(caster, target, skillHit.HitInfo);

				this.RemoveOneBeneficialBuff(target);
			}
		}

		private int RemoveDebuffs(Character target)
		{
			var buffComponent = target.Components.Get<BuffComponent>();

			if (buffComponent == null)
				return 0;

			var removableDebuffs = buffComponent.GetList().Where(this.CanRemoveDebuff).Take(MaximumRemovedDebuffs).ToList();

			foreach (var debuff in removableDebuffs)
				buffComponent.Remove(debuff.Id);

			return removableDebuffs.Count;
		}

		private List<Character> GetAlliesInRange(Character caster, Position position)
		{
			return caster.Map.GetCharacters(character => character != null && !character.IsDead && character.Layer == caster.Layer && !caster.IsEnemy(character) && position.Get2DDistance(character.Position) <= FumigateRadius).ToList();
		}

		private void ApplyOriginalFumigateEffects(Skill skill, Character caster, Character ally)
		{
			var removedDebuffs = this.RemoveDebuffs(ally);

			if (removedDebuffs > 0 && caster.IsAbilityActive(AbilityId.PlagueDoctor22))
			{
				var healingPercentage = removedDebuffs * 0.10f;
				var healingAmount = ally.MaxHp * healingPercentage;
				ally.ModifyHp(healingAmount);
			}

			if (caster.IsAbilityActive(AbilityId.PlagueDoctor5))
				ally.StartBuff(BuffId.Fumigate_Buff_ImmuneAbil, skill.Level, 0f, TimeSpan.FromSeconds(10), caster, skill.Id);
		}

		private async Task MaintainPurificationArea(Skill skill, Character caster, Position purificationPosition)
		{
			var purificationLevel = Math.Clamp(caster.Abilities.GetLevel(AbilityId.PlagueDoctor6), 1, 3);

			for (var second = 0; second < 10; second++)
			{
				if (caster.IsDead || caster.Map == null)
					break;

				var allies = this.GetAlliesInRange(caster, purificationPosition);

				foreach (var ally in allies)
					ally.StartBuff(BuffId.Fumigate_Buff_ResAbil, purificationLevel, 0f, TimeSpan.FromMilliseconds(1500), caster, skill.Id);

				if (second < 9)
					await skill.Wait(TimeSpan.FromSeconds(1));
			}
		}

		private bool CanRemoveDebuff(Buff buff)
		{
			if (buff.Data.Type != BuffType.Debuff)
				return false;

			if (!buff.Data.RemoveBySkill)
				return false;

			if (buff.Data.Level > MaximumRemovableDebuffLevel)
				return false;

			if (buff.Data.Tags.HasAny(BuffTag.Stun, BuffTag.Hold, BuffTag.Immobilize, BuffTag.Slow))
				return false;

			return true;
		}

		private void RemoveOneBeneficialBuff(ICombatEntity target)
		{
			var buffComponent = target.Components.Get<BuffComponent>();

			if (buffComponent == null)
				return;

			var beneficialBuff = buffComponent.GetList().FirstOrDefault(buff => buff.Data.Type == BuffType.Buff && buff.Data.RemoveBySkill);

			if (beneficialBuff != null)
				buffComponent.Remove(beneficialBuff.Id);
		}

		private float GetPerfusionFactor(int skillLevel)
		{
			skillLevel = Math.Clamp(skillLevel, 1, MaximumSkillLevel);
			var progress = (skillLevel - 1) / (float)(MaximumSkillLevel - 1);
			return PerfusionFactorAtLevelOne + (PerfusionFactorAtLevelTen - PerfusionFactorAtLevelOne) * progress;
		}

		private float GetPerfusionEnhanceMultiplier(Character character)
		{
			var enhanceLevel = Math.Clamp(character.Abilities.GetLevel(AbilityId.PlagueDoctor30), 0, MaximumEnhanceLevel);

			if (enhanceLevel <= 0)
				return 1f;

			var bonusPercent = enhanceLevel * 0.5f;

			if (enhanceLevel >= MaximumEnhanceLevel)
				bonusPercent += 10f;

			return 1f + bonusPercent / 100f;
		}
	}
}
