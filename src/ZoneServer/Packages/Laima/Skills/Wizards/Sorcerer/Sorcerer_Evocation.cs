using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using System.Collections.Generic;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer skill Evocation.
	/// Summons one persistent spirit based on the equipped sub-card.
	/// Using Evocation again replaces the current spirit.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_Evocation)]
	public class Sorcerer_EvocationOverride : IGroundSkillHandler, IDynamicCasted
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			var etc = character.Etc.Properties;
			var subCardName = etc.GetString(PropertyName.Sorcerer_bosscardName2, "None");
			var subCardGuid = etc.GetString(PropertyName.Sorcerer_bosscardGUID2, "None");

			if (subCardName == "None" || subCardGuid == "None")
			{
				caster.ServerMessage(Localization.Get("No sub-card equipped."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!long.TryParse(subCardGuid, out var cardId))
			{
				caster.ServerMessage(Localization.Get("Invalid sub-card data."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			var card = character.Inventory.GetItem(cardId);
			if (card == null)
			{
				etc.SetString(PropertyName.Sorcerer_bosscardName2, "None");
				etc.SetString(PropertyName.Sorcerer_bosscardGUID2, "None");
				etc.SetFloat(PropertyName.Sorcerer_bosscard2, 0);
				Send.ZC_OBJECT_PROPERTY(character, PropertyName.Sorcerer_bosscardName2, PropertyName.Sorcerer_bosscard2);
				caster.ServerMessage(Localization.Get("Sub-card no longer available."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			var mainSummons = character.Summons.GetSummons(s => s.Vars.TryGetInt("SORCERER_SUMMONING", out var value) && value == 1);
			if (mainSummons.Count == 0)
			{
				caster.ServerMessage(Localization.Get("Main summon must be active first."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (character.Map.Data.Type == MapType.City)
			{
				caster.ServerMessage(Localization.Get("Cannot use in this area."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(targetPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(character, skill, targetPos, subCardName, card));
		}

		private async Task HandleSkill(Character character, Skill skill, Position targetPos, string monsterClassName, Item card)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(950));

			if (!ZoneServer.Instance.Data.MonsterDb.TryFind(monsterClassName, out var monsterData))
			{
				character.ServerMessage(Localization.Get("Invalid monster data."));
				return;
			}

			var existingEvocations = character.Summons.GetSummons(s => s.Vars.TryGetInt("EVOCATION_MON", out var value) && value == 1);
			foreach (var existingEvocation in existingEvocations)
				this.RemoveEvocation(character, existingEvocation);

			var summon = new Summon(character, monsterData.Id, RelationType.Friendly);
			summon.Position = targetPos;
			summon.Direction = character.Direction;
			summon.Map = character.Map;
			summon.OwnerHandle = character.Handle;
			summon.Faction = FactionType.Law;
			summon.Level = character.Level;
			summon.Vars.SetInt("EVOCATION_MON", 1);

			if (card.Data.EquipExpGroup == EquipExpGroup.Legend_Card)
				summon.Vars.SetInt("LEGEND_CARD", 1);

			var scale = 0.7f + skill.Level / 15f * 0.3f;
			summon.Properties.SetFloat(PropertyName.Scale, scale);
			summon.Properties.SetFloat(PropertyName.WlkMSPD, 160f);
			summon.Properties.SetFloat(PropertyName.RunMSPD, 160f);
			summon.SetState(true, canMove: false, hasAi: false);

			character.Summons.AddSummon(summon);
			summon.Died += this.Evocation_Died;
			summon.AddEffect(ColorEffect.FromRgba(0.8f, 0.6f, 1.0f, 1.0f));

			await this.EvocationAttackSequence(summon, character, skill);
		}

		private async Task EvocationAttackSequence(Summon summon, Character owner, Skill evocationSkill)
		{
			await evocationSkill.Wait(TimeSpan.FromMilliseconds(500));

			if (summon.IsDead || summon.Map == null)
				return;

			var targets = summon.Map.GetAttackableEnemiesInPosition(summon, summon.Position, 150f).Where(enemy => summon.CanAttack(enemy)).OrderBy(enemy => summon.Position.Get2DDistance(enemy.Position)).ToList();
			var target = targets.FirstOrDefault();

			if (target == null)
			{
				this.RemoveEvocation(owner, summon);
				return;
			}

			var specialSkill = this.GetRandomSpecialSkill(summon);
			if (specialSkill == null)
			{
				this.RemoveEvocation(owner, summon);
				return;
			}

			var originalPATKBonus = summon.Properties.GetFloat(PropertyName.PATK_BM);
			var originalMATKBonus = summon.Properties.GetFloat(PropertyName.MATK_BM);

			this.ApplySpecialAttackScaling(summon, owner, evocationSkill, originalPATKBonus, originalMATKBonus);

			try
			{
				if (!this.ExecuteSpecialSkill(summon, target, specialSkill))
				{
					this.RemoveEvocation(owner, summon);
					return;
				}

				await this.WaitForSpecialSkillCompletion(summon, specialSkill, evocationSkill);
			}
			finally
			{
				if (!summon.IsDead)
				{
					summon.Properties.SetFloat(PropertyName.PATK_BM, originalPATKBonus);
					summon.Properties.SetFloat(PropertyName.MATK_BM, originalMATKBonus);
				}
			}

			if (summon.IsDead || summon.Map == null)
				return;

			await this.ExplodeEvocation(summon, owner, evocationSkill);

			if (!summon.IsDead && summon.Map != null)
				this.RemoveEvocation(owner, summon);
		}

		private void ApplySpecialAttackScaling(Summon summon, Character owner, Skill evocationSkill, float originalPATKBonus, float originalMATKBonus)
		{
			var skillDamageFactor = 1f + evocationSkill.Level * 0.10f;
			var enhanceLevel = owner.IsAbilityActive(AbilityId.Sorcerer12) ? owner.GetAbilityLevel(AbilityId.Sorcerer12) : 0;
			var enhanceFactor = 1f + enhanceLevel * 0.005f;
			var totalFactor = skillDamageFactor * enhanceFactor;
			var currentPATK = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MINPATK));
			var currentMATK = Math.Max(0f, summon.Properties.GetFloat(PropertyName.MATK));
			var scaledPATK = currentPATK * totalFactor;
			var scaledMATK = currentMATK * totalFactor;

			summon.Properties.SetFloat(PropertyName.PATK_BM, originalPATKBonus + Math.Max(0f, scaledPATK - currentPATK));
			summon.Properties.SetFloat(PropertyName.MATK_BM, originalMATKBonus + Math.Max(0f, scaledMATK - currentMATK));
		}

		private Skill GetRandomSpecialSkill(Summon summon)
		{
			var skillComponent = summon.Components.Get<BaseSkillComponent>();
			if (skillComponent == null || summon.Data.Skills.Count <= 1)
				return null;

			var specialSkillIds = summon.Data.Skills
				.Skip(1)
				.Select(entry => entry.SkillId)
				.Where(this.HasSupportedHandler)
				.OrderBy(_ => Random.Shared.Next())
				.ToArray();

			if (specialSkillIds.Length == 0)
				return null;

			var selectedSkillId = specialSkillIds[0];

			if (skillComponent.TryGet(selectedSkillId, out var existingSkill))
				return existingSkill;

			var specialSkill = new Skill(summon, selectedSkillId, 1);
			skillComponent.AddSilent(specialSkill);
			return specialSkill;
		}

		private bool HasSupportedHandler(SkillId skillId)
		{
			return ZoneServer.Instance.SkillHandlers.TryGetHandler<ITargetSkillHandler>(skillId, out _) || ZoneServer.Instance.SkillHandlers.TryGetHandler<IGroundSkillHandler>(skillId, out _);
		}

		private bool ExecuteSpecialSkill(Summon summon, ICombatEntity target, Skill specialSkill)
		{
			summon.TurnTowards(target);

			if (ZoneServer.Instance.SkillHandlers.TryGetHandler<ITargetSkillHandler>(specialSkill.Id, out var targetHandler))
			{
				targetHandler.Handle(specialSkill, summon, target);
				return true;
			}

			if (ZoneServer.Instance.SkillHandlers.TryGetHandler<IGroundSkillHandler>(specialSkill.Id, out var groundHandler))
			{
				groundHandler.Handle(specialSkill, summon, summon.Position, target.Position, target);
				return true;
			}

			return false;
		}

		private async Task WaitForSpecialSkillCompletion(Summon summon, Skill specialSkill, Skill evocationSkill)
		{
			const int maximumWaitMilliseconds = 20000;
			const int updateIntervalMilliseconds = 100;

			var minimumDuration = specialSkill.Properties.ShootTime;
			if (minimumDuration < TimeSpan.FromMilliseconds(500))
				minimumDuration = TimeSpan.FromMilliseconds(500);

			var startTime = DateTime.UtcNow;
			var maximumEndTime = startTime.AddMilliseconds(maximumWaitMilliseconds);

			await evocationSkill.Wait(TimeSpan.FromMilliseconds(updateIntervalMilliseconds));

			while (!summon.IsDead && summon.Map != null && DateTime.UtcNow < maximumEndTime)
			{
				var animationFinished = DateTime.UtcNow - startTime >= minimumDuration;
				var handlerFinished = !specialSkill.IsRunning;

				if (animationFinished && handlerFinished)
					break;

				await evocationSkill.Wait(TimeSpan.FromMilliseconds(updateIntervalMilliseconds));
			}

			await evocationSkill.Wait(TimeSpan.FromMilliseconds(200));
		}

		private async Task ExplodeEvocation(Summon summon, Character owner, Skill skill)
		{
			const float explosionRange = 160f;

			Send.ZC_NORMAL.PlayEffect(summon, "I_explosion012_dark", 1.5f);

			var intelligence = Math.Max(0f, owner.Properties.GetFloat(PropertyName.INT));
			var spirit = Math.Max(0f, owner.Properties.GetFloat(PropertyName.MNA));
			var statDamage = intelligence * 2f + spirit * 2f;
			var skillDamageFactor = 1f + skill.Level * 0.50f;
			var enhanceLevel = owner.IsAbilityActive(AbilityId.Sorcerer12) ? owner.GetAbilityLevel(AbilityId.Sorcerer12) : 0;
			var enhanceFactor = 1f + enhanceLevel * 0.005f;
			var enemies = summon.Map.GetAttackableEnemiesInPosition(summon, summon.Position, explosionRange).Where(enemy => summon.CanAttack(enemy)).ToList();
			var hits = new List<SkillHitInfo>();

			foreach (var enemy in enemies)
			{
				var hitResult = SkillUseFunctions.SCR_SkillHit(summon, enemy, skill);
				hitResult.Damage = Math.Max(1f, (hitResult.Damage + statDamage) * skillDamageFactor * enhanceFactor);
				enemy.TakeDamage(hitResult.Damage, summon);
				hits.Add(new SkillHitInfo(summon, enemy, skill, hitResult, TimeSpan.Zero, TimeSpan.Zero));
			}

			if (hits.Count > 0)
				Send.ZC_SKILL_HIT_INFO(summon, hits);

			await skill.Wait(TimeSpan.FromMilliseconds(300));
		}

		private void Evocation_Died(Mob mob, ICombatEntity killer)
		{
			if (mob is not Summon summon)
				return;

			var map = summon.Map;
			summon.Died -= this.Evocation_Died;
			map?.RemoveMonster(summon);
		}

		private void RemoveEvocation(Character owner, Summon summon)
		{
			var map = summon.Map;
			summon.Died -= this.Evocation_Died;

			if (!summon.IsDead)
				summon.Kill(owner);
			else
				owner.Summons.RemoveSummon(summon);

			map?.RemoveMonster(summon);
		}
	}
}
