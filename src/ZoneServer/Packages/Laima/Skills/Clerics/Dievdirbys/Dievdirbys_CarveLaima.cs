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

namespace Melia.Zone.Skills.HandlersOverrides.Clerics.Dievdirbys
{
	/// <summary>
	/// Handler override for the Dievdirbys skill Carve Laima.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Dievdirbys_CarveLaima)]
	public class Dievdirbys_CarveLaimaOverride : IMeleeGroundSkillHandler
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

			var spawnPosition = originPos.GetRelative(caster.Direction, 17);

			// Create the Laima statue monster
			// Select the appropriate monster name based on ability
			var laima = MonsterSkillCreateMob(skill, caster, "pcskill_wood_laima2", spawnPosition, 0, "", "", 0, 15 + skill.Level * 2, "", "!SCR_SUMMON_LAIMA#1");

			if (laima == null)
			{
				caster.ServerMessage(Localization.Get("Failed to summon Laima."));
				return;
			}

			// Apply property overrides to make Laima take only 1 damage per hit
			// HPCount reduces all incoming damage to exactly 1 and sets max HP to the specified value
			var laimaMaxHp = 20 + (3 * skill.Level);
			var propertyOverrides = new PropertyOverrides();
			propertyOverrides.Add(PropertyName.HPCount, laimaMaxHp);
			laima.ApplyOverrides(propertyOverrides);
			laima.Properties.InvalidateAll();
			laima.HealToFull();

			// Start the periodic buff application task
			skill.Run(this.ApplyLaimaBuffs(skill, caster, laima));
		}

		private async Task ApplyLaimaBuffs(Skill skill, ICombatEntity caster, ICombatEntity laima)
		{
			const float BuffRange = 100f;
			const int BuffDurationMs = 20000;
			const int UpdateIntervalMs = 3000;

			var abilityActive = caster.IsAbilityActive(AbilityId.Dievdirbys32);

			// Continue buffing while laima is alive
			while (laima != null && !laima.IsDead)
			{
				if (abilityActive)
				{
					// Ability ON: Apply movement speed debuff to enemies
					var enemies = caster.Map.GetActorsInRange<ICombatEntity>(laima.Position, BuffRange, target =>
					{
						if (target == null || target.IsDead)
							return false;
						return caster.IsEnemy(target);
					});

					foreach (var enemy in enemies)
					{
						enemy.StartBuff(BuffId.CarveLaima_MSPD_Debuff, skill.Level, 0, TimeSpan.FromMilliseconds(BuffDurationMs), caster);
					}
				}
				else
				{
					// Ability OFF: Apply cooldown reduction buff to allies
					var allies = caster.Map.GetActorsInRange<ICombatEntity>(laima.Position, BuffRange, target =>
					{
						if (target == null || target.IsDead)
							return false;
						return caster.IsAlly(target);
					});

					foreach (var ally in allies)
					{
						ally.StartBuff(BuffId.CarveLaima_Buff, skill.Level, 0, TimeSpan.FromMilliseconds(BuffDurationMs), caster);
					}
				}

				// Wait for the next update
				await skill.Wait(TimeSpan.FromMilliseconds(UpdateIntervalMs));
			}
		}
	}
}
