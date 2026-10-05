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
	/// Handler for the Sorcerer skill Attack Ground.
	/// Orders summoned creatures to attack at a specific location.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sorcerer_AttackGround)]
	public class Sorcerer_AttackGroundOverride : IGroundSkillHandler, IDynamicCasted
	{
		/// <summary>
		/// Default attack range for the summons when executing attack ground.
		/// </summary>
		private const float DefaultAttackRange = 100f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.PlaySound("voice_wiz_attackground_cast", "voice_wiz_m_attackground_cast");
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
			await skill.Wait(TimeSpan.FromMilliseconds(700));

			// Calculate attack range based on skill level
			var attackRange = DefaultAttackRange;

			// Order summons to attack at the target location
			SorcererSummonCommands.OrderAttackGround(skill, caster, targetPos, attackRange);

			// Play effect at target location to show command area
			Send.ZC_NORMAL.SkillProjectile(caster, targetPos, "F_ground_target", 1f, "None", 0, 0, TimeSpan.Zero);
		}
	}
}
