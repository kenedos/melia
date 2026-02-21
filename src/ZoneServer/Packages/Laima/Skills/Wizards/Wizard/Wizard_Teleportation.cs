using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;

namespace Melia.Zone.Skills.Handlers.Wizards.Wizard
{
	/// <summary>
	/// Handler for the Wizard skill Teleportation.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Wizard_Teleportation)]
	public class Wizard_TeleportationOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (caster is Character character)
			{
				if (character.IsWearingArmorOfType(ArmorMaterialType.Iron))
				{
					caster.ServerMessage(Localization.Get("Can't use while wearing [Plate] armor."));
					Send.ZC_SKILL_DISABLE(caster);
					return;
				}
			}

			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles skill, teleporting caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="target"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			if (caster is Character character)
			{
				if (character.IsWearingArmorOfType(ArmorMaterialType.Iron))
				{
					caster.ServerMessage(Localization.Get("Can't use while wearing [Plate] armor."));
					Send.ZC_SKILL_DISABLE(caster);
					return;
				}
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			// Increase max overheat if "Teleportation: Return" is active,
			// which allows gives the user a brief window in which they
			// can teleport back to the position they teleported from.
			var overheatMaxCount = 1;
			if (caster.IsAbilityActive(AbilityId.Wizard30))
				overheatMaxCount = 2;

			skill.IncreaseOverheat(overheatMaxCount);
			caster.SetAttackState(true);

			caster.StartBuff(BuffId.Teleportation_Buff, 0, 0, TimeSpan.FromSeconds(1), caster);
			caster.StartBuff(BuffId.Skill_NoDamage_Buff, 0, 0, TimeSpan.FromSeconds(1), caster);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var teleportationDistance = 80 + 24 * skill.Level;
			teleportationDistance = Math.Min(248, teleportationDistance);

			Position targetPos;

			targetPos = caster.Position.GetRelative(caster.Direction, teleportationDistance);

			var height = caster.Map.Ground.GetHeightAt(targetPos);
			targetPos.Y = height;

			if (!caster.Map.Ground.IsValidPosition(targetPos))
			{
				var newPos = caster.Map.Ground.GetLastValidPosition(caster.Position, targetPos);
				if (caster.Map.Ground.TryGetHeightAt(newPos, out height))
				{
					targetPos = newPos;
					targetPos.Y = height;
				}
				else
					targetPos = caster.Position;
			}

			caster.Position = targetPos;
			Send.ZC_SET_POS(caster, targetPos);
		}
	}
}
