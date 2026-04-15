using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer skill Obey.
	/// Allows the sorcerer to directly control their summoned creature.
	/// </summary>
	/// <remarks>
	/// When Obey is activated:
	/// - The summon receives Sorcerer_Obey_Status_Buff
	/// - The caster receives Sorcerer_Obey_PC_DEF_Buff
	/// - The player can directly control the summon's movement and attacks
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_Obey)]
	public class Sorcerer_ObeyOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			// Check if caster has any sorcerer summons
			if (caster is Character character)
			{
				var summons = SorcererSummonCommands.GetSorcererSummons(caster);
				if (summons.Count == 0)
				{
					caster.ServerMessage(Localization.Get("No summons available to control."));
					return;
				}

				// Check if summon is in a valid position (not in OBB)
				var mainSummon = summons.FirstOrDefault();
				if (mainSummon != null && mainSummon.IsInOBB())
				{
					caster.ServerMessage(Localization.Get("Cannot control summon in current position."));
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

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			// Wait for skill animation
			await skill.Wait(TimeSpan.FromMilliseconds(700));

			// Start the control mode
			SorcererSummonCommands.StartObeyControl(caster);
		}
	}
}
