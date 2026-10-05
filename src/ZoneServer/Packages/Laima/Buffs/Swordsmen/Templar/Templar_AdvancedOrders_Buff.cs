using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Templar
{
	[Package("laima")]
	[BuffHandler(BuffId.AdvancedOrders_Buff)]
	public class Templar_AdvancedOrders_BuffOverride : BuffHandler
	{
		private const int UpdateInterval = 1000;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int MaximumSpCost = 18;
		private const int MinimumSpCost = 8;
		private const float AuraRange = 120f;
		private const float MinimumDamageReduction = 0.064f;
		private const float MaximumDamageReduction = 0.12f;
		private const float MaximumAllowedDamageReduction = 0.80f;
		private const float EnhanceBonusPerLevel = 0.005f;
		private const float MaximumEnhanceLevelBonus = 0.10f;
		private const int MaximumEnhanceLevel = 100;
		private static readonly TimeSpan AllyBuffDuration = TimeSpan.FromMilliseconds(1500);

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target == null)
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var moveSpeedBonus = (skillLevel + 1) / 2;

			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, moveSpeedBonus);

			if (!ReferenceEquals(buff.Target, buff.Caster))
				return;

			buff.SetUpdateTime(UpdateInterval);
			this.UpdateAura(buff, true);
		}

		public override void WhileActive(Buff buff)
		{
			if (!ReferenceEquals(buff.Target, buff.Caster))
				return;

			this.UpdateAura(buff, true);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target != null)
				RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);

			if (buff.Target is Character character && ReferenceEquals(buff.Target, buff.Caster))
				this.RemoveDistributedBuffs(character);
		}

		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.AdvancedOrders_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.AdvancedOrders_Buff, out var buff))
				return;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var damageReductionPerLevel = (MaximumDamageReduction - MinimumDamageReduction) / (MaximumSkillLevel - MinimumSkillLevel);
			var damageReduction = MinimumDamageReduction + (skillLevel - MinimumSkillLevel) * damageReductionPerLevel;

			if (buff.Caster is Character caster && caster.Abilities.TryGet(AbilityId.Templar14, out var ability) && ability.Active)
			{
				var abilityLevel = Math.Clamp(ability.Level, 0, MaximumEnhanceLevel);
				var enhanceBonus = abilityLevel * EnhanceBonusPerLevel;

				if (abilityLevel >= MaximumEnhanceLevel)
					enhanceBonus += MaximumEnhanceLevelBonus;

				damageReduction *= 1f + enhanceBonus;
			}

			damageReduction = Math.Min(MaximumAllowedDamageReduction, damageReduction);
			modifier.DamageMultiplier *= 1f - damageReduction;
		}

		private void UpdateAura(Buff buff, bool consumeSp)
		{
			if (buff.Target is not Character character || character.IsDead)
			{
				buff.Target?.StopBuff(BuffId.AdvancedOrders_Buff);
				return;
			}

			if (character.Map == null)
			{
				character.StopBuff(BuffId.AdvancedOrders_Buff);
				return;
			}

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);

			if (consumeSp)
			{
				var spCost = this.GetSpCost(skillLevel);

				if (!character.TrySpendSp(spCost))
				{
					character.StopBuff(BuffId.AdvancedOrders_Buff);
					return;
				}
			}

			var partyMembers = character.Map
				.GetPartyMembersInRange(character, AuraRange, true)
				.Where(member => member != null && !member.IsDead && member != character)
				.Distinct()
				.ToList();

			foreach (var member in partyMembers)
			{
				if (member.TryGetBuff(BuffId.AdvancedOrders_Buff, out var existingBuff) && ReferenceEquals(existingBuff.Caster, member))
					continue;

				member.StartBuff(BuffId.AdvancedOrders_Buff, skillLevel, 0f, AllyBuffDuration, character, buff.SkillId);
			}
		}

		private int GetSpCost(int skillLevel)
		{
			var levelProgress = skillLevel - MinimumSkillLevel;
			var totalDifference = MaximumSpCost - MinimumSpCost;
			var reduction = (int)Math.Floor(levelProgress * totalDifference / (float)(MaximumSkillLevel - MinimumSkillLevel));
			return Math.Max(MinimumSpCost, MaximumSpCost - reduction);
		}

		private void RemoveDistributedBuffs(Character caster)
		{
			if (caster.Map == null)
				return;

			var partyMembers = caster.Map
				.GetPartyMembersInRange(caster, AuraRange, true)
				.Where(member => member != null && member != caster)
				.Distinct()
				.ToList();

			foreach (var member in partyMembers)
			{
				if (!member.TryGetBuff(BuffId.AdvancedOrders_Buff, out var distributedBuff))
					continue;

				if (!ReferenceEquals(distributedBuff.Caster, caster))
					continue;

				member.StopBuff(BuffId.AdvancedOrders_Buff);
			}
		}
	}
}
