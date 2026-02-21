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
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World;

namespace Melia.Zone.Skills.HandlersOverrides.Clerics.Dievdirbys
{
	/// <summary>
	/// Handler override for the Dievdirbys skill Carve Owl.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Dievdirbys_CarveOwl)]
	public class Dievdirbys_CarveOwlOverride : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
			Send.ZC_SKILL_RANGE_SQUARE(caster, originPos, farPos, 21, false);

			skill.Run(this.HandleSkill(skill, caster, originPos, farPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1750));

			// Create Owl summon using MonsterSkillCreateMob for proper summon management
			// Owl provides vision and attacks enemies within range
			// Lifetime: 15 + (skill.Level * 2) seconds
			var position = originPos.GetRelative(farPos, distance: 17f);

			// Create the Owl statue monster - no AI needed since we handle attacks in OwlAttackLoop
			var owl = MonsterSkillCreateMob(skill, caster, "pcskill_wood_owl2", position, 0, "", "None", 0, 15 + skill.Level * 2, "None", "Faction#Trap#!SCR_SUMMON_OWL#1");

			if (owl == null)
			{
				caster.ServerMessage(Localization.Get("Failed to summon Owl."));
				return;
			}

			// Apply property overrides to make Owl take only 1 damage per hit
			// HPCount reduces all incoming damage to exactly 1 and sets max HP to the specified value
			var owlMaxHp = 20 + (3 * skill.Level);

			// Calculate owl's attack based on caster's MATK and skill factor
			var casterMATK = (caster.Properties.GetFloat(PropertyName.MINMATK) + caster.Properties.GetFloat(PropertyName.MAXMATK)) / 2f;
			var skillFactor = skill.Properties.GetFloat(PropertyName.SkillFactor);

			// Apply Dievdirbys12 ability enhancement - 0.05% damage increase per level
			var abilityMultiplier = 1f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Dievdirbys12, out var abilityLevel))
				abilityMultiplier += abilityLevel * 0.0005f;

			var owlAttack = casterMATK * (skillFactor / 100f) * abilityMultiplier;

			var propertyOverrides = new PropertyOverrides();
			propertyOverrides.Add(PropertyName.HPCount, owlMaxHp);
			propertyOverrides.Add(PropertyName.MINMATK, owlAttack);
			propertyOverrides.Add(PropertyName.MAXMATK, owlAttack);
			owl.ApplyOverrides(propertyOverrides);
			owl.Properties.InvalidateAll();
			owl.HealToFull();

			// Attach visual effect for Owl
			owl.AttachEffect("F_cleric_owl_loop");

			// Give the owl its monster skills
			var wideMiasmaLevel = 1;
			if (owl.Components.TryGet<BaseSkillComponent>(out var owlSkillComponent))
			{
				// Add the two owl attack skills
				var owlSkill1 = new Skill(owl, SkillId.Mon_skill_Owl_E_Skill_1, skill.Level);
				var owlSkill2 = new Skill(owl, SkillId.Mon_skill_Owl_E_Skill_2, skill.Level);
				var owlSkill3 = new Skill(owl, SkillId.Wugushi_WideMiasma, wideMiasmaLevel);

				owlSkillComponent.AddSilent(owlSkill1);
				owlSkillComponent.AddSilent(owlSkill2);
				owlSkillComponent.AddSilent(owlSkill3);
			}

			// Start background task to make the owl attack nearby enemies
			skill.Run(this.OwlAttackLoop(skill, caster, owl, position));

			// Note: MonsterSkillCreateMob already handles AddMonster, DelayEnterWorld, EnterDelayedActor
		}

		private async Task OwlAttackLoop(Skill skill, ICombatEntity caster, ICombatEntity owl, Position owlPosition)
		{
			const float detectionRange = 60f;
			const int checkInterval = 2800; // Time between attacks

			// Get the owl's skill component
			if (!owl.Components.TryGet<BaseSkillComponent>(out var skillComponent))
			{
				caster.ServerMessage(Localization.Get("Owl skill component not found."));
				return;
			}

			// Track which skill to use next (alternate between the two)
			var useSkill1 = true;

			while (owl != null && !owl.IsDead)
			{
				// Check if owl is still alive
				if (owl == null || owl.IsDead)
					break;

				// Find enemy targets in range
				var targets = owl.Map.GetActorsInRange<ICombatEntity>(owlPosition, detectionRange, target =>
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

				// If targets found, execute one of the owl's monster skills
				if (targets.Count > 0)
				{
					// Alternate between the two skills
					var skillToUse = useSkill1 ? SkillId.Mon_skill_Owl_E_Skill_1 : SkillId.Mon_skill_Owl_E_Skill_2;

					// Get the skill from the owl's skill component
					if (skillComponent.TryGet(skillToUse, out var owlSkill))
					{
						// Get the closest target
						var closestTarget = targets.OrderBy(t => t.Position.Get2DDistance(owlPosition)).FirstOrDefault();

						if (closestTarget != null)
						{
							// Get the skill handler and execute it
							// Owl skills are ITargetSkillHandler type
							if (ZoneServer.Instance.SkillHandlers.TryGetHandler<ITargetSkillHandler>(skillToUse, out var handler))
							{
								// Mark skill as being used
								skillComponent.UseSkill(skillToUse);

								// Execute the skill handler
								handler.Handle(owlSkill, owl, closestTarget);
							}
						}
					}

					// Toggle for next attack
					useSkill1 = !useSkill1;
				}

				// Wait for next check interval
				await skill.Wait(TimeSpan.FromMilliseconds(checkInterval));
			}
		}
	}

	/// <summary>
	/// Attack Skill handlers the owl uses
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Mon_skill_Owl_E_Skill_1)]
	public class Mon_skill_Owl_E_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var hits = new List<SkillHitInfo>();
			SkillHitCircle(caster, skill, target.Position, 80f, hits);

			if (caster.IsOwnerAbilityActive(AbilityId.Dievdirbys4))
			{
				SkillResultTargetBuff(caster, skill, BuffId.Mon_Fire_buff, 1, 0f, 5000f, 1, 100, -1, hits);
			}
		}
	}

	/// <summary>
	/// Attack Skill handlers the owl uses, this one poisons!
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Mon_skill_Owl_E_Skill_2)]
	public class Mon_skill_Owl_E_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target?.Handle ?? 0, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(300));

			var hits = new List<SkillHitInfo>();
			SkillHitCircle(caster, skill, target.Position, 80f, hits);

			// Apply wide miasma poison to all enemies hit (8 second duration)
			if (caster.Components.TryGet<BaseSkillComponent>(out var skillComponent) &&
				skillComponent.TryGet(SkillId.Wugushi_WideMiasma, out var miasmaSkill))
			{
				SkillResultTargetBuff(caster, miasmaSkill, BuffId.WideMiasma_Debuff, 1, 0f, 8000f, 1, 100, -1, hits);
			}
		}
	}
}
