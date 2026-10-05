using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Necromancer_RaiseDead)]
	public class Necromancer_RaiseDeadOverride : IGroundSkillHandler
	{
		private const int CorpsePartsCost = 5;
		private const int MaximumSummons = 5;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const float MinimumHpInheritanceRate = 0.50f;
		private const float MaximumHpInheritanceRate = 1f;
		private const float MinimumDefenseInheritanceRate = 0.25f;
		private const float MaximumDefenseInheritanceRate = 0.50f;
		private const float BaseOffensiveInheritanceRate = 0.05f;
		private const float OffensiveInheritanceRatePerLevel = 0.015f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			var summonCount = character.Summons.GetSummons(s => !s.IsDead && s.Id == MonsterId.SkeletonSoldier).Count;

			if (summonCount >= MaximumSummons)
			{
				character.ServerMessage(Localization.Get("You can summon a maximum of 5 Skeleton Soldiers."));
				return;
			}

			var corpseParts = (int)character.Etc.Properties.GetFloat(PropertyName.Necro_DeadPartsCnt);

			if (corpseParts < CorpsePartsCost)
			{
				caster.ServerMessage(Localization.Get("Not enough corpse parts."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			character.SetEtcProperty(PropertyName.Necro_DeadPartsCnt, corpseParts - CorpsePartsCost);
			Send.ZC_OBJECT_PROPERTY(character, character.Etc, PropertyName.Necro_DeadPartsCnt);

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var summon = new Summon(character, MonsterId.SkeletonSoldier, RelationType.Friendly);
			character.Summons.AddSummon(summon);

			summon.Name = "!@#${Auto_1}_of_{Auto_2}$*$Auto_1$*$" + caster.Name + "$*$Auto_2$*$@dicID_^*$ETC_20150317_000235$*^#@!";
			summon.OwnerHandle = caster.Handle;
			summon.Faction = FactionType.Law;
			summon.Tendency = TendencyType.Aggressive;
			summon.FromGround = true;

			summon.Properties.SetFloat(PropertyName.Level, caster.Level);
			summon.Properties.SetFloat(PropertyName.Lv, caster.Level);
			summon.Properties.SetFloat(PropertyName.FIXMSPD_BM, 140f);

			CalculateSummonStats(summon, character, skill);

			summon.Properties.InvalidateAll();
			summon.Properties.SetFloat(PropertyName.HP, summon.Properties.GetFloat(PropertyName.MHP));
			summon.Properties.SetFloat(PropertyName.SP, summon.Properties.GetFloat(PropertyName.MSP));

			summon.Components.Add(new LifeTimeComponent(summon, TimeSpan.FromMinutes(30)));
			summon.SetState(true, canMove: true, hasAi: false);
			summon.Components.Add(new AiComponent(summon, "PC_Summon_Necromancer", character));

			skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SYNC_START(caster, skillHandle, 1);
			summon.StartBuff(BuffId.Ability_buff_PC_Summon, TimeSpan.Zero, summon);
			Send.ZC_SYNC_END(caster, skillHandle, 0);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, skill.Data.DefaultHitDelay);
		}

		private static void CalculateSummonStats(Summon summon, Character character, Skill skill)
		{
			var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
			var hpInheritanceRate = GetLinearRate(skillLevel, MinimumHpInheritanceRate, MaximumHpInheritanceRate);
			var defenseInheritanceRate = GetLinearRate(skillLevel, MinimumDefenseInheritanceRate, MaximumDefenseInheritanceRate);
			var offensiveInheritanceRate = BaseOffensiveInheritanceRate + skillLevel * OffensiveInheritanceRatePerLevel;

			var intelligence = Math.Max(0f, character.Properties.GetFloat(PropertyName.INT));
			var constitution = Math.Max(0f, character.Properties.GetFloat(PropertyName.CON));
			var spirit = Math.Max(0f, character.Properties.GetFloat(PropertyName.MNA));
			var characterMaximumHp = Math.Max(0f, character.Properties.GetFloat(PropertyName.MHP));
			var characterDefense = Math.Max(0f, character.Properties.GetFloat(PropertyName.DEF));
			var characterMagicDefense = Math.Max(0f, character.Properties.GetFloat(PropertyName.MDEF));
			var characterCriticalResistance = Math.Max(0f, character.Properties.GetFloat(PropertyName.CRTDR));
			var characterMinimumMagicAttack = Math.Max(0f, character.Properties.GetFloat(PropertyName.MINMATK));
			var characterMaximumMagicAttack = Math.Max(characterMinimumMagicAttack, character.Properties.GetFloat(PropertyName.MAXMATK));
			var characterAccuracy = Math.Max(0f, character.Properties.GetFloat(PropertyName.HR));

			var oldHpBonus = skillLevel + intelligence + constitution + spirit;
			var oldAttackBonus = skillLevel + intelligence + spirit;
			var oldDefenseBonus = skillLevel + constitution + intelligence + spirit;

			var averageMagicAttack = (characterMinimumMagicAttack + characterMaximumMagicAttack) / 2f;
			var hpBonus = oldHpBonus + characterMaximumHp * hpInheritanceRate;
			var attackBonus = oldAttackBonus + averageMagicAttack * offensiveInheritanceRate;
			var physicalDefenseBonus = oldDefenseBonus + characterDefense * defenseInheritanceRate;
			var magicDefenseBonus = oldDefenseBonus + characterMagicDefense * defenseInheritanceRate;
			var criticalResistanceBonus = characterCriticalResistance * offensiveInheritanceRate;
			var accuracyBonus = characterAccuracy * offensiveInheritanceRate;

			// Aplicação da Enhance (Necromancer7: +0.5% por nível + 10% bônus no lvl 100)
			if (character.TryGetActiveAbilityLevel(AbilityId.Necromancer7, out var enhanceLevel) && enhanceLevel > 0)
			{
				var enhanceBonusRate = 1f + (enhanceLevel * 0.005f) + (enhanceLevel >= 100 ? 0.10f : 0f);

				hpBonus *= enhanceBonusRate;
				attackBonus *= enhanceBonusRate;
				physicalDefenseBonus *= enhanceBonusRate;
				magicDefenseBonus *= enhanceBonusRate;
				criticalResistanceBonus *= enhanceBonusRate;
				accuracyBonus *= enhanceBonusRate;
			}

			summon.Properties.SetFloat(PropertyName.MHP_BM, summon.Properties.GetFloat(PropertyName.MHP_BM) + hpBonus);
			summon.Properties.SetFloat(PropertyName.PATK_BM, summon.Properties.GetFloat(PropertyName.PATK_BM) + attackBonus);
			summon.Properties.SetFloat(PropertyName.MATK_BM, summon.Properties.GetFloat(PropertyName.MATK_BM) + attackBonus);
			summon.Properties.SetFloat(PropertyName.DEF_BM, summon.Properties.GetFloat(PropertyName.DEF_BM) + physicalDefenseBonus);
			summon.Properties.SetFloat(PropertyName.MDEF_BM, summon.Properties.GetFloat(PropertyName.MDEF_BM) + magicDefenseBonus);
			summon.Properties.SetFloat(PropertyName.CRTDR_BM, summon.Properties.GetFloat(PropertyName.CRTDR_BM) + criticalResistanceBonus);
			summon.Properties.SetFloat(PropertyName.HR_BM, summon.Properties.GetFloat(PropertyName.HR_BM) + accuracyBonus);
		}

		private static float GetLinearRate(int skillLevel, float minimumRate, float maximumRate)
		{
			var progress = (skillLevel - MinimumSkillLevel) / (float)(MaximumSkillLevel - MinimumSkillLevel);
			return minimumRate + (maximumRate - minimumRate) * progress;
		}
	}
}
