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
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using Melia.Zone.World.Items;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.World;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Geometry;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Characters.Components;

namespace Melia.Zone.Skills.HandlersOverrides.Clerics.Dievdirbys
{
	/// <summary>
	/// Handler override for the Dievdirbys skill Carve.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Dievdirbys_Carve)]
	public class Dievdirbys_CarveOverride : IMeleeGroundSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.Zero;

		// List of Dievdirbys statue class names
		private static readonly HashSet<string> DievdirbysStatues = new HashSet<string>
		{
			"pcskill_wood_ausrine2",
			"pcskill_wood_owl2",
			"pcskill_wood_zemina2",
			"pcskill_wood_AustrasKoks2",
			"pcskill_wood_bakarine2",
			"pcskill_wood_laima2",
		};

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			// Define splash area for the skill
			// Length 55, width 20 based on skill's melee range
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 20);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = TimeSpan.Zero;
			var skillTargets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var hits = new List<SkillHitInfo>();

			foreach (var target in skillTargets.LimitBySDR(caster, skill))
			{
				// Carve has 5 multi-hits as defined in skills_overrides.txt
				var modifier = SkillModifier.MultiHit(5);

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);

			// Check for Dievdirbys statues in the skill area
			var statue = this.FindStatueInArea(caster, splashArea);
			if (statue != null)
			{
				// Play VFX on the statue
				this.PlayStatueHitEffect(caster, statue);

				// Apply statue-specific effect
				this.ApplyStatueEffect(caster, statue, skill);
			}

			// Dievdirbys18 ability: Add Wood Drop
			// This ability has a chance to drop wood resources when hitting enemies with Carve
			if (caster.IsAbilityActive(AbilityId.Dievdirbys18))
			{
				// AddDropWood: Drop 1 wood resource with drop rate based on ability level
				// Each level of the ability increases drop rate by 10%
				AddDropWood(caster, 1, caster.GetAbilityLevel(AbilityId.Dievdirbys18) * 10);
			}
		}

		/// <summary>
		/// Checks if the given monster is a Dievdirbys statue.
		/// </summary>
		private bool IsDievdirbysStatue(Mob monster)
		{
			if (monster == null)
				return false;

			return DievdirbysStatues.Contains(monster.ClassName);
		}

		/// <summary>
		/// Finds the first Dievdirbys statue in the specified area.
		/// </summary>
		private Mob FindStatueInArea(ICombatEntity caster, IShapeF area)
		{
			// Get all monsters in the area (statues are monsters with Our_Forces faction)
			var monsters = caster.Map.GetActorsIn<Mob>(area);

			// Find the first statue
			foreach (var monster in monsters)
			{
				if (this.IsDievdirbysStatue(monster))
					return monster;
			}

			return null;
		}

		/// <summary>
		/// Plays a visual effect on the statue when hit by Carve.
		/// </summary>
		private void PlayStatueHitEffect(ICombatEntity caster, Mob statue)
		{
			if (caster is Character character)
			{
				// Play a blue light effect on the statue
				Send.ZC_NORMAL.PlayEffect(character.Connection, statue, animationName: "F_hit001_light_yellow2", heightOffset: EffectLocation.Bottom);
				Send.ZC_NORMAL.PlayEffect(character.Connection, statue, animationName: "F_ground079_light_yellow", heightOffset: EffectLocation.Bottom);
			}
		}

		/// <summary>
		/// Applies the statue-specific effect based on the statue type.
		/// </summary>
		private void ApplyStatueEffect(ICombatEntity caster, Mob statue, Skill skill)
		{
			var statueClassName = statue.ClassName;

			switch (statueClassName)
			{
				case "pcskill_wood_ausrine2":
					this.ApplyAusrineEffect(caster, statue, skill);
					break;

				case "pcskill_wood_owl2":
					this.ApplyOwlEffect(caster, statue, skill);
					break;

				case "pcskill_wood_zemina2":
					this.ApplyZeminaEffect(caster, statue, skill);
					break;

				case "pcskill_wood_AustrasKoks2":
					this.ApplyAustrasKoksEffect(caster, statue, skill);
					break;

				case "pcskill_wood_bakarine2":
					this.ApplyVakarineEffect(caster, statue, skill);
					break;

				case "pcskill_wood_laima2":
					this.ApplyLaimaEffect(caster, statue, skill);
					break;

				default:
					break;
			}
		}

		/// <summary>
		/// Applies the Ausrine statue effect: buffs all allies in wider range when carved.
		/// Range: 75 + 20 * Carve Skill Level
		/// </summary>
		private void ApplyAusrineEffect(ICombatEntity caster, Mob statue, Skill skill)
		{
			// Calculate buff range based on Carve skill level: 75 + 20 * SkillLv
			var buffRange = 75f + (20f * skill.Level);
			var buffArea = new CircleF(statue.Position, buffRange);
			var alliesInRange = caster.Map.GetActorsIn<Character>(buffArea);

			var buffId = BuffId.Ausirine_Buff;
			var duration = TimeSpan.FromSeconds(15); // 15 second duration

			foreach (var ally in alliesInRange)
			{
				// Check if the ally
				if (caster.IsAlly(ally))
				{
					ally.StartBuff(buffId, skill.Level, 0f, duration, caster);
				}
			}

			// Also buff the caster
			if (caster is Character casterChar)
			{
				casterChar.StartBuff(buffId, skill.Level, 0f, duration, caster);
			}

			// Play visual effect at the statue
			if (caster is Character character)
			{
				Send.ZC_NORMAL.PlayEffect(character.Connection, statue, animationName: "F_cleric_ausirine_shot_light", heightOffset: EffectLocation.Bottom, scale: 5f);
			}
		}

		/// <summary>
		/// Applies the Owl statue effect: triggers the owl to attack nearby enemies using Mon_skill_Owl_E_Skill_2.
		/// </summary>
		private void ApplyOwlEffect(ICombatEntity caster, Mob statue, Skill skill)
		{
			// The owl attacks enemies within range using Mon_skill_Owl_E_Skill_2
			const float detectionRange = 60f;

			// Find enemy targets in range of the owl
			var targets = statue.Map.GetActorsInRange<ICombatEntity>(statue.Position, detectionRange, target =>
			{
				// Only attack enemies of the caster
				if (target == null || target.IsDead)
					return false;

				// Check if target is an enemy
				if (caster is Character character)
					return target.IsEnemy(character);
				else
					return target.IsEnemy(caster);
			});

			// If no targets found, do nothing
			if (targets.Count == 0)
				return;

			// Get the owl's skill component
			if (!statue.Components.TryGet<BaseSkillComponent>(out var skillComponent))
				return;

			// Get the Mon_skill_Owl_E_Skill_2 skill from the owl
			if (!skillComponent.TryGet(SkillId.Mon_skill_Owl_E_Skill_2, out var owlSkill))
				return;

			// Get the closest target
			var closestTarget = targets.OrderBy(t => t.Position.Get2DDistance(statue.Position)).FirstOrDefault();

			if (closestTarget == null)
				return;

			// Get the skill handler and execute it
			if (ZoneServer.Instance.SkillHandlers.TryGetHandler<ITargetSkillHandler>(SkillId.Mon_skill_Owl_E_Skill_2, out var handler))
			{
				// Mark skill as being used
				skillComponent.UseSkill(SkillId.Mon_skill_Owl_E_Skill_2);

				// Execute the skill handler (owl attacks with poison effect)
				handler.Handle(owlSkill, statue, closestTarget);
			}
		}

		/// <summary>
		/// Applies the Zemina statue effect: recovers SP for all players in Zemina's range.
		/// Formula: Base SP (50 * Carve Level) * Zemina Modifier (1 + 5% per Zemina level) * MNA Modifier (1 + 0.5% per MNA point)
		/// </summary>
		private void ApplyZeminaEffect(ICombatEntity caster, Mob statue, Skill skill)
		{
			// Play visual effects on the statue for SP recovery
			if (caster is Character character)
			{
				Send.ZC_NORMAL.PlayEffect(character.Connection, statue, animationName: "F_cleric_zemina_shot_light", heightOffset: EffectLocation.Bottom, scale: 5f);
			}

			// Find all allied characters in range around the statue
			const float zeminaRange = 100f;
			var spRecoveryArea = new CircleF(statue.Position, zeminaRange);
			var alliesInRange = caster.Map.GetActorsIn<Character>(spRecoveryArea);

			// Get the Zemina skill level from the caster's skills
			var zeminaLevel = 1;
			if (caster.Components.TryGet<SkillComponent>(out var skillComponent))
			{
				if (skillComponent.TryGet(SkillId.Dievdirbys_CarveZemina, out var zeminaSkill))
				{
					zeminaLevel = zeminaSkill.Level;
				}
			}

			// Calculate SP recovery
			// Base SP = 20 * Carve Attack Level
			var baseSp = 20f * skill.Level;

			// Zemina Modifier = +5% per Zemina statue level
			var zeminaModifier = 1f + (zeminaLevel * 0.05f);

			// MNA (SPR) Modifier = +0.5% per MNA point
			var casterMna = caster.Properties.GetFloat(PropertyName.MNA);
			var mnaModifier = 1f + (casterMna * 0.005f);

			// Calculate final SP recovery amount
			var finalSpRecovery = baseSp * zeminaModifier * mnaModifier;

			// Recover SP for all allied players in range
			foreach (var ally in alliesInRange)
			{
				// Check if ally
				if (caster.IsAlly(ally))
				{
					var spToRecover = Math.Min(finalSpRecovery, ally.MaxSp - ally.Sp);
					ally.Heal(0, spToRecover);
				}
			}

			// Also recover SP for the caster if they're in range
			if (caster is Character casterChar)
			{
				var spRecoveryAreaCheck = new CircleF(statue.Position, zeminaRange);
				if (spRecoveryAreaCheck.IsInside(casterChar.Position))
				{
					var spToRecover = Math.Min(finalSpRecovery, casterChar.MaxSp - casterChar.Sp);
					casterChar.Heal(0, spToRecover);
				}
			}
		}

		/// <summary>
		/// Applies the Austras Koks statue effect: damages and silences enemies in range.
		/// Damage is based on Carve skill damage, and number of targets is based on CarveAustraKoks skill level.
		/// </summary>
		private void ApplyAustrasKoksEffect(ICombatEntity caster, Mob statue, Skill skill)
		{
			// Play visual effects on the statue for the attack
			if (caster is Character character)
			{
				Send.ZC_NORMAL.PlayEffect(character.Connection, statue, animationName: "F_ground131_dark_red", heightOffset: EffectLocation.Bottom, scale: 2f);
			}

			// Limit to maximum 3 targets
			var maxTargets = 3;

			// Find enemies in 100f range around the statue (matching Romuva pad range)
			const float austrasKoksRange = 100f;
			var damageArea = new CircleF(statue.Position, austrasKoksRange);
			var enemiesInRange = caster.Map.GetAttackableEnemiesIn(caster, damageArea);

			// Limit targets by maxTargets
			var limitedEnemies = enemiesInRange.Take(maxTargets);

			var hits = new List<SkillHitInfo>();
			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = TimeSpan.Zero;

			foreach (var target in limitedEnemies)
			{
				// Use Carve skill damage calculation (multi-hit 5x)
				var modifier = SkillModifier.MultiHit(5);

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);

				// Apply 3-second silence debuff
				var silenceDuration = TimeSpan.FromSeconds(3);
				target.StartBuff(BuffId.Common_Silence, 1, 0f, silenceDuration, caster);
			}

			// Send hit info if any targets were hit
			if (hits.Count > 0)
			{
				Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}

		/// <summary>
		/// Applies the Vakarine statue effect: knockbacks nearby enemies without dealing damage.
		/// Knockback velocity: 70 + 10 * Carve Skill Level
		/// </summary>
		private void ApplyVakarineEffect(ICombatEntity caster, Mob statue, Skill skill)
		{
			// Play visual effects on the statue for the knockback
			if (caster is Character character)
			{
				Send.ZC_NORMAL.PlayEffect(character.Connection, statue, animationName: "F_statue_vakarine_light_1", heightOffset: EffectLocation.Bottom, scale: 5f);
			}

			const float vakarineRange = 150f;
			var knockbackArea = new CircleF(statue.Position, vakarineRange);
			var enemiesInRange = caster.Map.GetAttackableEnemiesIn(caster, knockbackArea);

			var hits = new List<SkillHitInfo>();
			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = TimeSpan.Zero;

			// Calculate knockback velocity based on Carve skill level: 70 + 10 * SkillLv
			var knockbackVelocity = 70 + (10 * skill.Level);

			foreach (var target in enemiesInRange)
			{
				// Create a hit result with 0 damage (knockback only)
				var skillHitResult = new SkillHitResult();
				skillHitResult.Result = HitResultType.Hit;
				skillHitResult.Damage = 0;

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				// Apply knockback if target is knockbackable
				if (target.IsKnockdownable())
				{
					// Knockback away from the statue
					skillHit.KnockBackInfo = new KnockBackInfo(statue.Position, skillHit.Target, HitType.Normal, knockbackVelocity, 10);
					skillHit.HitInfo.Type = HitType.KnockBack;
					target.ApplyKnockback(caster, skill, skillHit);
				}

				hits.Add(skillHit);
			}

			// Send hit info if any targets were hit
			if (hits.Count > 0)
			{
				Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}

		/// <summary>
		/// Drops wood items near the caster based on probability.
		/// </summary>
		private void AddDropWood(ICombatEntity caster, int itemCount, int probability = 100)
		{
			if (probability > 100)
				probability = 100;

			var rnd = RandomProvider.Get();
			for (var i = 0; i < itemCount; i++)
			{
				var randomItem = rnd.Next(0, 100);
				if (randomItem <= probability)
				{
					var itemName = "wood_06";

					if (!ZoneServer.Instance.Data.ItemDb.TryFind(itemName, out var item))
						continue;

					var direction = new Direction(rnd.Next(0, 360));
					var dropRadius = ZoneServer.Instance.Conf.World.DropRadius;
					var distance = rnd.Next(dropRadius / 2, dropRadius + 1);
					new Item(item.Id).Drop(caster.Map, caster.Position, direction, distance, 0, caster.Layer);
				}
			}
		}

		/// <summary>
		/// Applies the Laima statue effect: reduces cooldown of all skills currently on cooldown.
		/// Reduction: (5% + 1% * Carve Skill Level) of max cooldown (absolute reduction)
		/// </summary>
		private void ApplyLaimaEffect(ICombatEntity caster, Mob statue, Skill skill)
		{
			// Play visual effects on the statue
			if (caster is Character character)
			{
				Send.ZC_NORMAL.PlayEffect(character.Connection, statue, animationName: "F_cleric_zemina_shot_light", heightOffset: EffectLocation.Bottom, scale: 5f);
			}

			// Calculate reduction percentage: 5% + 1% * Carve Skill Level
			var reductionPercent = 0.05f + (0.01f * skill.Level);

			// Find all allied characters in range around the statue
			const float laimaRange = 150f;
			var effectArea = new CircleF(statue.Position, laimaRange);
			var alliesInRange = caster.Map.GetActorsIn<Character>(effectArea);

			foreach (var ally in alliesInRange)
			{
				// Check if ally
				if (!caster.IsAlly(ally))
					continue;

				// Get the ally's skill component
				if (!ally.Components.TryGet<SkillComponent>(out var skillComponent))
					continue;

				// Reduce cooldown of all skills currently on cooldown
				var allSkills = skillComponent.GetList();
				foreach (var allySkill in allSkills)
				{
					if (allySkill.IsOnCooldown)
					{
						// Calculate reduction based on max cooldown (absolute reduction)
						var reduction = TimeSpan.FromMilliseconds(allySkill.Data.CooldownTime.TotalMilliseconds * reductionPercent);

						// Reduce the cooldown
						allySkill.ReduceCooldown(reduction);
					}
				}
			}

			// Also apply to the caster if in range
			if (caster is Character casterChar && effectArea.IsInside(casterChar.Position))
			{
				if (casterChar.Components.TryGet<SkillComponent>(out var casterSkillComponent))
				{
					var allSkills = casterSkillComponent.GetList();
					foreach (var casterSkill in allSkills)
					{
						if (casterSkill.IsOnCooldown)
						{
							// Calculate reduction based on max cooldown (absolute reduction)
							var reduction = TimeSpan.FromMilliseconds(casterSkill.Data.CooldownTime.TotalMilliseconds * reductionPercent);

							// Reduce the cooldown
							casterSkill.ReduceCooldown(reduction);
						}
					}
				}
			}
		}
	}
}
