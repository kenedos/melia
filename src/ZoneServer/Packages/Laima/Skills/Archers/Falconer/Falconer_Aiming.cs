using System;
using System.Collections.Generic;
using System.Linq;
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
	/// and increasing damage taken. Marked enemies are revealed even if hidden.
	/// </summary>
	/// <remarks>
	/// - Pad is created at the HAWK's position, not target position
	/// - Pad follows hawk as it moves
	/// - Duration: (skill.Level * 5000) + 10000 ms
	/// - Falconer21 (Hawk Eye) changes buff from Aiming_Buff to Aiming_Hawk_Buff
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_Aiming)]
	public class Falconer_AimingOverride : IGroundSkillHandler
	{
		private const float AimingRadius = 100f;
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

			// Take off from shoulder if landed
			if (hawk.IsLandedOnShoulder)
			{
				hawk.TakeOff();
				await skill.Wait(TimeSpan.FromMilliseconds(1000));
			}

			// Create Aiming pad at HAWK's position (matches Lua: local x, y, z = Get3DPos(hawk))
			var hawkPos = hawk.Position;
			var pad = SkillCreatePad(caster, skill, hawkPos, 0f, PadName.Falconer_Aiming, range: AimingRadius);
			if (pad == null)
				return;

			// Store pad ID on hawk for tracking (matches Lua: SetExProp(hawk, 'AIMING_PAD_ID', padID))
			var padId = pad.Handle;
			hawk.Vars.Set(AimingPadVariable, padId);

			// Set hawk aiming state
			hawk.Vars.Set("Hawk.Aiming.Active", true);
			hawk.Vars.Set("Hawk.Aiming.Position", hawkPos);

			// Calculate duration: (skill.Level * 5000) + 10000
			var durationMs = BaseDurationMs + (skill.Level * DurationPerLevelMs);
			var endTime = DateTime.Now.AddMilliseconds(durationMs);

			// Monitor aiming state
			while (DateTime.Now < endTime)
			{
				// Check if caster or hawk died
				if (caster.IsDead || hawk.IsDead)
					break;

				// Check if pad was replaced (another Aiming cast)
				var currentPadId = hawk.Vars.Get<int>(AimingPadVariable, 0);
				if (currentPadId != padId)
				{
					// Pad was replaced, destroy this one
					if (caster.Map.TryGetPad(pad.Handle, out var existingPad))
						existingPad?.Destroy();
					return;
				}

				// Check if pad is still alive
				if (pad.IsDead)
					break;

				await skill.Wait(TimeSpan.FromMilliseconds(1000));
			}

			// Cleanup
			hawk.Vars.Set("Hawk.Aiming.Active", false);
		}
	}
}
