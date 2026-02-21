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
using Melia.Zone.Skills.Handlers;
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

namespace Melia.Zone.Skills.HandlersOverrides.Clerics.Dievdirbys
{
	/// <summary>
	/// Handler override for the Dievdirbys skill Carve Vakarine.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Dievdirbys_CarveVakarine)]
	public class Dievdirbys_CarveVakarineOverride : IMeleeGroundSkillHandler
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
			var vakarine = MonsterSkillCreateMob(skill, caster, "pcskill_wood_bakarine2", spawnPosition, 0, "", "", 0, 15 + skill.Level * 2, "", "!SCR_SUMMON_VAKARINE#1");

			if (vakarine == null)
			{
				caster.ServerMessage(Localization.Get("Failed to summon Vakarine."));
				return;
			}

			// Apply property overrides to make Vakarine take only 1 damage per hit
			// HPCount reduces all incoming damage to exactly 1 and sets max HP to the specified value
			var vakarineMaxHp = 20 + (3 * skill.Level);
			var propertyOverrides = new PropertyOverrides();
			propertyOverrides.Add(PropertyName.HPCount, vakarineMaxHp);
			vakarine.ApplyOverrides(propertyOverrides);
			vakarine.Properties.InvalidateAll();
			vakarine.HealToFull();

			// Start the periodic buff application task
			skill.Run(this.ApplyVakarineBuffs(skill, caster, vakarine));

			// Note: MonsterSkillCreateMob already handles AddMonster, DelayEnterWorld, EnterDelayedActor
		}

		private async Task ApplyVakarineBuffs(Skill skill, ICombatEntity caster, ICombatEntity vakarine)
		{
			const float BuffRange = 150f;
			const int BuffDurationMs = 20000;
			const int UpdateIntervalMs = 3000;

			// Continue buffing while vakarine is alive
			while (vakarine != null && !vakarine.IsDead)
			{
				// Find all allies in range of the Vakarine
				var allies = caster.Map.GetActorsInRange<ICombatEntity>(vakarine.Position, BuffRange, target =>
				{
					// Only buff allies of the caster
					if (target == null || target.IsDead)
						return false;

					// Check if target is an ally (not hostile)
					return caster.IsAlly(target);
				});

				// Apply or refresh Scud buff to all allies in range
				foreach (var ally in allies)
				{
					ally.StartBuff(BuffId.Scud, skill.Level, 0, TimeSpan.FromMilliseconds(BuffDurationMs), caster);
				}

				// Wait for the next update
				await skill.Wait(TimeSpan.FromMilliseconds(UpdateIntervalMs));
			}
		}
	}
}
