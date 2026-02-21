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
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handler for the Paladin skill Sanctuary.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Paladin_Sanctuary)]
	public class Paladin_SanctuaryOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			var pad = SkillCreatePad(caster, skill, caster.Position, 0f, PadName.Paladin_Sanctuary_Pad);
			skill.Vars.Set("Skill.Pad", pad);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			// TODO: No Implementation SKL_CANCEL_CANCEL
			var pad = skill.Vars.Get<Pad>("Skill.Pad");
			pad.Destroy();
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			// Check if caster is wielding a shield
			if (caster is Character character)
			{
				var shield = character.Inventory.GetItem(EquipSlot.LeftHand);
				if (shield == null || shield.Data.EquipType1 != EquipType.Shield)
				{
					caster.ServerMessage(Localization.Get("Skill requires a shield."));
					Send.ZC_SKILL_DISABLE(caster);
					return;
				}
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			// var targetPos = originPos.GetRelative(farPos);
			// var pad = SkillCreatePad(caster, skill, targetPos, 0f, PadName.Paladin_Sanctuary_Pad);
			// TODO: Implement Additional SP consumption
		}
	}
}
