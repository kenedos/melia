using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill Aiming.
	/// Commands the hawk to mark enemies in an area, making them easier to hit
	/// by increasing their effective hit radius for AoE attacks.
	/// </summary>
	/// <remarks>
	/// - Creates a pad at the hawk's position that follows the hawk
	/// - Pad applies Aiming_Buff to enemies every 1 second
	/// - Aiming_Buff increases target HitRadiusBonus (client: ZC_ALTER_HIT_RADIUS)
	/// - Hawk receives Aiming_Hawk_Buff for the skill's duration
	/// - Duration: 10000 + (skill.Level * 5000) ms
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_Aiming)]
	public class Falconer_AimingOverride : IGroundSkillHandler
	{
		private const int BaseDurationMs = 10000;
		private const int DurationPerLevelMs = 5000;
		private const string AimingPadVariable = "Hawk.Aiming.PadId";

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!FalconerHawkHelper.TryGetHawk(caster, out var hawk))
			{
				if (caster is Character character)
					character.SystemMessage("CompanionIsNotActive");
				Send.ZC_SKILL_DISABLE(caster);
				return;
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

			skill.Run(this.HandleSkill(skill, caster, hawk));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, World.Actors.Monsters.Companion hawk)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(300));

			// Clean up any existing Aiming pad
			var oldPadId = hawk.Vars.Get<int>(AimingPadVariable, 0);
			if (oldPadId != 0 && caster.Map.TryGetPad(oldPadId, out var oldPad))
				oldPad?.Destroy();

			// Take off from shoulder if landed
			if (hawk.IsLandedOnShoulder)
				hawk.TakeOff();
			else if (hawk.IsOnRoost)
				hawk.LeaveRoost();

			// Calculate duration
			var durationMs = BaseDurationMs + (skill.Level * DurationPerLevelMs);
			var duration = TimeSpan.FromMilliseconds(durationMs);

			// Apply hawk buff
			hawk.StartBuff(BuffId.Aiming_Hawk_Buff, skill.Level, 0f, duration, caster, skill.Id);

			// Create pad at hawk's position, following the hawk
			var pad = SkillCreatePad(caster, skill, hawk.Position, 0f, PadName.Falconer_Aiming);
			if (pad == null)
				return;

			pad.FollowsTarget(hawk);
			pad.Trigger.LifeTime = duration;

			hawk.Vars.Set(AimingPadVariable, pad.Handle);

			// Set hawk aiming state
			FalconerHawkHelper.ExecuteAiming(caster, hawk, skill, hawk.Position);
		}
	}
}
