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
	/// Handler override for the Dievdirbys skill Carve Austras Koks.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Dievdirbys_CarveAustrasKoks)]
	public class Dievdirbys_CarveAustrasKoksOverride : IMeleeGroundSkillHandler
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

			// Create Austras Koks summon using MonsterSkillCreateMob for proper summon management
			// Parameters: skill, caster, className, position, angle, name, aiName, lvFix, lifeTime, simpleAi, monProp
			// Lifetime: 15 + (skill.Level * 2) seconds
			var position = originPos.GetRelative(farPos, 17);

			// Create the Romuva pad for Austras Koks effects
			MonsterSkillCreatePad(caster, skill, position, 0, PadName.Cleric_Romuva);

			var austrasKoks = MonsterSkillCreateMob(skill, caster, "pcskill_wood_AustrasKoks2", position, 0, "", "", 0, 15 + skill.Level * 2, "", "!SCR_SUMMON_AUSTRASKOKS#1");

			if (austrasKoks == null)
			{
				caster.ServerMessage(Localization.Get("Failed to summon Austras Koks."));
				return;
			}

			// Apply property overrides to make Austras Koks take only 1 damage per hit
			// HPCount reduces all incoming damage to exactly 1 and sets max HP to the specified value
			var austrasKoksMaxHp = 20 + (3 * skill.Level);
			var propertyOverrides = new PropertyOverrides();
			propertyOverrides.Add(PropertyName.HPCount, austrasKoksMaxHp);
			austrasKoks.ApplyOverrides(propertyOverrides);
			austrasKoks.Properties.InvalidateAll();
			austrasKoks.HealToFull();

			// Note: MonsterSkillCreateMob already handles AddMonster, DelayEnterWorld, EnterDelayedActor
		}
	}
}
