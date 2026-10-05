using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.NakMuay
{
	/// <summary>

	/// Handler for Ram Muay buff.

	/// Updates the character's basic attack while Ram Muay stance is active.

	/// </summary>

	[Package("laima")]
	[BuffHandler(BuffId.RamMuay_Buff)]
	public class NakMuay_RamMuay_BuffOverride : BuffHandler, IBuffCombatAttackAfterCalcHandler
	{
		private const int ProcChance = 15;
		private const float SkillRange = 100f;
		private const int BleedingDurationSeconds = 10;
		private const int BleedingTickCount = 10;
		private const int StunDurationSeconds = 3;
		private static readonly TimeSpan InitialHitDelay = TimeSpan.FromMilliseconds(400);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(120);
		private static readonly SkillId[] ProcSkillIds =
		{
			SkillId.NakMuay_TeKha,
			SkillId.NakMuay_SokChiang,
			SkillId.NakMuay_TeTrong,
			SkillId.NakMuay_KhaoLoi,
		};

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start)
				return;

			if (buff.Target is Character character)
				NakMuayAttackHelper.UpdateMainAttack(character);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Character character)
				NakMuayAttackHelper.UpdateMainAttack(character);
		}

		public void OnAttackAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (attacker is not Character character || target == null || target.IsDead || !skill.IsNormalAttack)
				return;

			if (character.GetDistance(target) > SkillRange || RandomProvider.Get().Next(100) >= ProcChance)
				return;

			var availableSkills = new List<Skill>();

			foreach (var skillId in ProcSkillIds)
			{
				if (character.TryGetSkill(skillId, out var procSkill) && procSkill.Level > 0)
					availableSkills.Add(procSkill);
			}

			if (availableSkills.Count == 0)
				return;

			var selectedSkill = availableSkills[RandomProvider.Get().Next(availableSkills.Count)];
			var spCost = this.GetBasicSpCost(selectedSkill.Id) * 0.5f;

			if (!character.TrySpendSp(spCost))
				return;

			character.TurnTowards(target);
			Send.ZC_NORMAL.UpdateSkillEffect(character, target.Handle, character.Position, character.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(character, selectedSkill, target);
			selectedSkill.Run(this.HandleProc(character, target, selectedSkill));
		}

		private async Task HandleProc(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			await skill.Wait(InitialHitDelay);

			var multiHitCount = this.GetMultiHitCount(skill.Id);
			var totalDamage = 0f;
			var hitCount = 0;

			for (var hitIndex = 0; hitIndex < multiHitCount; hitIndex++)
			{
				if (target.IsDead || caster.GetDistance(target) > SkillRange)
					break;

				var procHitResult = SCR_SkillHit(caster, target, skill);

				NakMuayMuayThaiHelper.Apply(caster, procHitResult);
				this.ApplyEnhanceAttribute(caster, skill.Id, procHitResult);

				totalDamage += procHitResult.Damage;
				hitCount++;

				target.TakeDamage(procHitResult.Damage, caster);

				var procHit = new SkillHitInfo(caster, target, skill, procHitResult, TimeSpan.FromMilliseconds(20), TimeSpan.Zero);

				Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, procHit);

				if (hitIndex < multiHitCount - 1)
					await skill.Wait(DelayBetweenHits);
			}

			if (target.IsDead || hitCount == 0)
				return;

			switch (skill.Id)
			{
				case SkillId.NakMuay_SokChiang:
				{
					if (totalDamage > 0)
					{
						var bleedingDamagePerTick = totalDamage / BleedingTickCount;

						target.StartBuff(
							BuffId.HeavyBleeding,
							skill.Level,
							bleedingDamagePerTick,
							TimeSpan.FromSeconds(BleedingDurationSeconds),
							caster);
					}

					break;
				}

				case SkillId.NakMuay_TeKha:
				{
					target.StartBuff(
						BuffId.Stun,
						skill.Level,
						0,
						TimeSpan.FromSeconds(StunDurationSeconds),
						caster);

					break;
				}
			}
		}

		private float GetBasicSpCost(SkillId skillId)
		{
			return skillId switch
			{
				SkillId.NakMuay_TeKha => 45f,
				SkillId.NakMuay_SokChiang => 45f,
				SkillId.NakMuay_TeTrong => 46f,
				SkillId.NakMuay_KhaoLoi => 48f,
				_ => 0f,
			};
		}

		private int GetMultiHitCount(SkillId skillId)
		{
			return skillId switch
			{
				SkillId.NakMuay_TeKha => 4,
				SkillId.NakMuay_SokChiang => 4,
				SkillId.NakMuay_TeTrong => 2,
				SkillId.NakMuay_KhaoLoi => 2,
				_ => 1,
			};
		}

		private void ApplyEnhanceAttribute(ICombatEntity caster, SkillId skillId, SkillHitResult skillHitResult)
		{
			if (caster is not Character character)
				return;

			AbilityId abilityId;

			switch (skillId)
			{
				case SkillId.NakMuay_TeKha: abilityId = AbilityId.NakMuay1; break;
				case SkillId.NakMuay_SokChiang: abilityId = AbilityId.NakMuay2; break;
				case SkillId.NakMuay_TeTrong: abilityId = AbilityId.NakMuay3; break;
				case SkillId.NakMuay_KhaoLoi: abilityId = AbilityId.NakMuay4; break;
				default: return;
			}

			if (!character.Abilities.TryGet(abilityId, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
