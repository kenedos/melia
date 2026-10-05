using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Necromancer_CreateShoggoth)]
	public class Necromancer_CreateShoggothOverride : IGroundSkillHandler
	{
		private const int CorpsePartsCost = 30;
		private const string ShoggothClassName = "pcskill_shogogoth";
		private const string ShoggothIdentifier = "NECROMANCER_SHOGGOTH";
		private const string SummonAi = "PC_Summon_Necromancer";

		private static readonly TimeSpan SummonDuration = TimeSpan.FromMinutes(30);

		private static readonly string[] CardProperties =
		{
			PropertyName.Necro_bosscard1,
			PropertyName.Necro_bosscard2,
			PropertyName.Necro_bosscard3,
			PropertyName.Necro_bosscard4
		};

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			var cards = GetEquippedCardMonsters(character);

			if (cards.Count == 0)
			{
				character.ServerMessage(Localization.Get("Equip at least one valid card in the Necronomicon."));
				return;
			}

			var corpseParts = (int)character.Etc.Properties.GetFloat(PropertyName.Necro_DeadPartsCnt);

			if (corpseParts < CorpsePartsCost)
			{
				character.ServerMessage(Localization.Get("Not enough corpse parts."));
				return;
			}

			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(ShoggothClassName, out _))
			{
				character.ServerMessage(Localization.Get("Shoggoth monster data was not found."));
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			character.SetEtcProperty(PropertyName.Necro_DeadPartsCnt, corpseParts - CorpsePartsCost);

			Send.ZC_PC_PROP_UPDATE(
				character,
				PropertyTable.GetId("PCEtc", PropertyName.Necro_DeadPartsCnt),
				1
			);

			character.AddonMessage(AddonMessage.UPDATE_NECRONOMICON_UI);

			skill.IncreaseOverheat();
			character.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(character, skill, 1, originPos, farPos);

			Send.ZC_NORMAL.UpdateSkillEffect(
				character,
				targetHandle,
				originPos,
				originPos.GetDirection(farPos),
				Position.Zero
			);

			Send.ZC_SKILL_MELEE_GROUND(
				character,
				skill,
				farPos,
				ForceId.GetNew(),
				null
			);

			skill.Run(this.HandleSkill(character, skill, originPos, farPos, cards));
		}

		private async Task HandleSkill(Character character, Skill skill, Position originPos, Position farPos, List<Mob> cards)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1500));

			if (character.IsDead || character.Map == null)
				return;

			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(ShoggothClassName, out var monsterData))
				return;

			RemoveExistingShoggoth(character);

			var summon = new Summon(character, monsterData.Id, RelationType.Friendly);

			summon.OwnerHandle = character.Handle;
			summon.AssociatedHandle = character.Handle;
			summon.Faction = character.Faction;
			summon.Tendency = TendencyType.Aggressive;
			summon.FromGround = true;
			summon.Level = character.Level;

			summon.Properties.SetFloat(PropertyName.Lv, character.Level);
			summon.Properties.SetFloat(PropertyName.WlkMSPD, 120f);
			summon.Properties.SetFloat(PropertyName.RunMSPD, 120f);

			summon.Vars.SetInt(ShoggothIdentifier, 1);
			summon.Vars.SetInt("Melia.Summon.SkillLevel", skill.Level);
			summon.Vars.SetInt("Melia.Summon.Skill", (int)skill.Id);
			summon.Vars.Set("Melia.Summoner.Owner", character);

			ApplyCardStats(summon, character, cards, skill.Level);

			summon.Properties.InvalidateAll();

			summon.Properties.SetFloat(
				PropertyName.HP,
				summon.Properties.GetFloat(PropertyName.MHP)
			);

			summon.Properties.SetFloat(
				PropertyName.SP,
				summon.Properties.GetFloat(PropertyName.MSP)
			);

			summon.Components.Add(new LifeTimeComponent(summon, SummonDuration));

			character.Summons.AddSummon(summon);

			summon.SetState(true, canMove: true, hasAi: false);
			summon.Components.Add(new AiComponent(summon, SummonAi, character));

			var spawnPosition = originPos.GetRelative(farPos, distance: 80f);

			if (character.Map.Ground.TryGetNearestValidPosition(spawnPosition, out var validPosition))
			{
				summon.Position = validPosition;
				Send.ZC_SET_POS(summon);
			}

			var properties = new[]
			{
				PropertyName.HP,
				PropertyName.MHP,
				PropertyName.MINPATK,
				PropertyName.MAXPATK,
				PropertyName.MINMATK,
				PropertyName.MAXMATK,
				PropertyName.DEF,
				PropertyName.MDEF,
				PropertyName.STR,
				PropertyName.CON,
				PropertyName.INT,
				PropertyName.DEX,
				PropertyName.MNA
			};

			Send.ZC_OBJECT_PROPERTY(
				character.Connection,
				summon.Handle,
				summon.Properties.GetSelect(properties)
			);

			character.AddonMessage(AddonMessage.UPDATE_NECRONOMICON_UI);

			Console.WriteLine(
				$"[SHOGGOTH] cards={cards.Count} " +
				$"skillLevel={skill.Level} " +
				$"HP={summon.Properties.GetFloat(PropertyName.MHP)} " +
				$"PATK={summon.Properties.GetFloat(PropertyName.MINPATK)} " +
				$"MATK={summon.Properties.GetFloat(PropertyName.MINMATK)} " +
				$"DEF={summon.Properties.GetFloat(PropertyName.DEF)} " +
				$"MDEF={summon.Properties.GetFloat(PropertyName.MDEF)} " +
				$"STR={summon.Properties.GetFloat(PropertyName.STR)} " +
				$"CON={summon.Properties.GetFloat(PropertyName.CON)} " +
				$"INT={summon.Properties.GetFloat(PropertyName.INT)} " +
				$"DEX={summon.Properties.GetFloat(PropertyName.DEX)} " +
				$"SPR={summon.Properties.GetFloat(PropertyName.MNA)}"
			);
		}

		private static List<Mob> GetEquippedCardMonsters(Character character)
		{
			var monsters = new List<Mob>();

			foreach (var property in CardProperties)
			{
				var cardId = (int)character.Etc.Properties.GetFloat(property);

				if (cardId <= 0)
					continue;

				var card = character.Inventory.FindItem(
					item => item.Id == cardId && item.Data.Group == ItemGroup.Card
				);

				if (card == null)
					continue;

				var monsterId = (int)card.Data.Script.NumArg1;

				if (monsterId <= 0)
					continue;

				if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterId, out var monsterData))
					continue;

				monsters.Add(new Mob(monsterData.Id, RelationType.Enemy));
			}

			return monsters;
		}

		private static void ApplyCardStats(Summon summon, Character character, List<Mob> cards, int skillLevel)
		{
			var totalHp = 0f;
			var totalPhysicalAttack = 0f;
			var totalMagicAttack = 0f;
			var totalDefense = 0f;
			var totalMagicDefense = 0f;
			var totalStrength = 0f;
			var totalConstitution = 0f;
			var totalIntelligence = 0f;
			var totalDexterity = 0f;
			var totalSpirit = 0f;

			foreach (var cardMonster in cards)
			{
				totalHp += Math.Max(0f, cardMonster.Properties.GetFloat(PropertyName.MHP));
				totalPhysicalAttack += Math.Max(0f, cardMonster.Properties.GetFloat(PropertyName.MINPATK));
				totalMagicAttack += Math.Max(0f, cardMonster.Properties.GetFloat(PropertyName.MINMATK));
				totalDefense += Math.Max(0f, cardMonster.Properties.GetFloat(PropertyName.DEF));
				totalMagicDefense += Math.Max(0f, cardMonster.Properties.GetFloat(PropertyName.MDEF));
				totalStrength += Math.Max(0f, cardMonster.Properties.GetFloat(PropertyName.STR));
				totalConstitution += Math.Max(0f, cardMonster.Properties.GetFloat(PropertyName.CON));
				totalIntelligence += Math.Max(0f, cardMonster.Properties.GetFloat(PropertyName.INT));
				totalDexterity += Math.Max(0f, cardMonster.Properties.GetFloat(PropertyName.DEX));
				totalSpirit += Math.Max(0f, cardMonster.Properties.GetFloat(PropertyName.MNA));
			}

			var skillMultiplier = 1f + Math.Max(1, skillLevel) * 0.10f;

			var enhanceLevel = Math.Clamp(character.GetAbilityLevel(AbilityId.Necromancer5), 0, 100);
			var enhanceBonus = enhanceLevel * 0.005f + (enhanceLevel >= 100 ? 0.10f : 0f);
			var enhanceMultiplier = 1f + enhanceBonus;

			var enlargementLevel = character.IsAbilityActive(AbilityId.Necromancer8) ? Math.Clamp(character.GetAbilityLevel(AbilityId.Necromancer8), 0, 20) : 0;
			var enlargementMultiplier = 1f + enlargementLevel * 0.01f;

			var finalMultiplier = skillMultiplier * enhanceMultiplier * enlargementMultiplier;

			summon.Properties.SetFloat(PropertyName.STR, totalStrength * finalMultiplier);
			summon.Properties.SetFloat(PropertyName.CON, totalConstitution * finalMultiplier);
			summon.Properties.SetFloat(PropertyName.INT, totalIntelligence * finalMultiplier);
			summon.Properties.SetFloat(PropertyName.DEX, totalDexterity * finalMultiplier);
			summon.Properties.SetFloat(PropertyName.MNA, totalSpirit * finalMultiplier);

			summon.Properties.InvalidateAll();

			ApplyFinalStat(summon, PropertyName.MHP, PropertyName.MHP_BM, totalHp * finalMultiplier);
			ApplyFinalStat(summon, PropertyName.MINPATK, PropertyName.PATK_BM, totalPhysicalAttack * finalMultiplier);
			ApplyFinalStat(summon, PropertyName.MINMATK, PropertyName.MATK_BM, totalMagicAttack * finalMultiplier);
			ApplyFinalStat(summon, PropertyName.DEF, PropertyName.DEF_BM, totalDefense * finalMultiplier);
			ApplyFinalStat(summon, PropertyName.MDEF, PropertyName.MDEF_BM, totalMagicDefense * finalMultiplier);

			Console.WriteLine($"[SHOGGOTH] cards={cards.Count} skillLevel={skillLevel} enhanceLevel={enhanceLevel} enlargementLevel={enlargementLevel} finalMultiplier={finalMultiplier:F3}");
		}

		private static void ApplyFinalStat(Summon summon, string finalProperty, string bonusProperty, float desiredValue)
		{
			var currentBonus = summon.Properties.GetFloat(bonusProperty);
			var currentValue = summon.Properties.GetFloat(finalProperty);
			var baseValue = currentValue - currentBonus;

			summon.Properties.SetFloat(
				bonusProperty,
				currentBonus + Math.Max(0f, desiredValue) - baseValue
			);
		}

		private static void RemoveExistingShoggoth(Character character)
		{
			var summons = character.Summons.GetSummons(
				summon => summon.Vars.TryGetInt(ShoggothIdentifier, out var value) && value == 1
			);

			foreach (var summon in summons)
				summon.Kill(character);
		}
	}
}
