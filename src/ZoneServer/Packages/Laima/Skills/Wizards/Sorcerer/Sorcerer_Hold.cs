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
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer skill Hold.
	/// Orders summoned creatures to hold position and attack nearby enemies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_Hold)]
	public class Sorcerer_HoldOverride : IGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_wiz_hold_shot", "voice_wiz_m_hold_shot");
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

			// Check if caster has any sorcerer summons
			if (caster is Character character)
			{
				var summons = SorcererSummonCommands.GetSorcererSummons(caster);
				if (summons.Count == 0)
				{
					caster.ServerMessage(Localization.Get("No summons available to command."));
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

			skill.Run(this.HandleSkill(caster, skill, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPos)
		{
			// Wait for skill animation
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			// Order summons to hold at the target location
			SorcererSummonCommands.OrderHold(skill, caster, targetPos);

			// Play effect at target location to show hold position
			Send.ZC_NORMAL.SkillProjectile(caster, targetPos, "F_ground_hold", 1f, "None", 0, 0, TimeSpan.Zero);
		}
	}
}
