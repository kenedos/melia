using System;
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
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill Circling.
	/// Toggle skill that commands the hawk to circle an area, revealing hidden
	/// enemies and reducing their size rating for increased multi-hit damage.
	/// </summary>
	/// <remarks>
	/// - Applies Circling_Buff to the hawk (visual effect)
	/// - Creates a pad that follows the hawk
	/// - Pad applies CirclingIncreaseSR_Buff to allies who enter
	/// - Falconer11 ability increases the duration
	/// - Casting again cancels the skill
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_Circling)]
	public class Falconer_CirclingOverride : IGroundSkillHandler
	{
		private const int BaseBuffDurationMs = 10000;
		private const string CirclingPadVariable = "Falconer.Circling.PadHandle";

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!FalconerHawkHelper.TryGetHawk(caster, out var hawk))
			{
				if (caster is Character character)
					character.SystemMessage("CompanionIsNotActive");
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			var targetHandle = target?.Handle ?? 0;

			// Toggle off: cancel circling
			if (skill.Vars.GetBool("Melia.Skill.Toggled"))
			{
				skill.Vars.SetBool("Melia.Skill.Toggled", false);
				if (caster is Character toggleOffChar)
					Send.ZC_NORMAL.SkillToggle(toggleOffChar, SkillId.None);

				Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

				caster.StopBuff(BuffId.CirclingIncreaseSR_Buff);
				hawk.StopBuff(BuffId.UC_Detected_Debuff);
				this.CleanupCirclingPad(caster);
				FalconerHawkHelper.StopCircling(caster, hawk);
				return;
			}

			// Toggle on: activate circling
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			skill.Vars.SetBool("Melia.Skill.Toggled", true);
			if (caster is Character toggleOnChar)
				Send.ZC_NORMAL.SkillToggle(toggleOnChar, skill.Id);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			// Calculate buff duration - Falconer11 increases duration
			var buffDurationMs = BaseBuffDurationMs;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Falconer11, out var abilLevel))
				buffDurationMs += abilLevel * 1000;

			var buffDuration = TimeSpan.FromMilliseconds(buffDurationMs);

			// Self-buff on player
			caster.StartBuff(BuffId.CirclingIncreaseSR_Buff, skill.Level, 0f, buffDuration, caster);

			// UC_Detected_Debuff on hawk (visual state)
			hawk.StartBuff(BuffId.UC_Detected_Debuff, skill.Level, 0f, buffDuration, caster);

			skill.Run(this.HandleCirclingActive(skill, caster, hawk, buffDuration));
		}

		private async Task HandleCirclingActive(Skill skill, ICombatEntity caster, World.Actors.Monsters.Companion hawk, TimeSpan duration)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(200));

			// Take off if perched
			if (hawk.IsLandedOnShoulder)
				hawk.TakeOff();
			else if (hawk.IsOnRoost)
				hawk.LeaveRoost();

			await skill.Wait(TimeSpan.FromMilliseconds(500));

			// Create circling pad at hawk's position with hawk as creator
			// so the client renders the pad at the hawk, not the player
			var pad = SkillCreatePad(hawk, skill, hawk.Position, 0f, PadName.Falconer_Circling);
			if (pad == null)
				return;

			pad.FollowsTarget(hawk);
			pad.Trigger.LifeTime = duration;

			caster.SetTempVar(CirclingPadVariable, pad.Handle);
			hawk.Vars.Set("Hawk.Circling.PadId", pad.Handle);

			pad.ChangeGroundEffect("F_archer_circling_ground", 1.7f);

			FalconerHawkHelper.ExecuteCircling(caster, hawk, skill, hawk.Position);

			// Monitor circling state
			var endTime = DateTime.Now.Add(duration);

			while (DateTime.Now < endTime)
			{
				if (!skill.Vars.GetBool("Melia.Skill.Toggled"))
					break;

				if (caster.IsDead || hawk.IsDead)
					break;

				if (pad.IsDead)
					break;

				await skill.Wait(TimeSpan.FromMilliseconds(1000));
			}

			// Auto-cleanup when duration expires
			skill.Vars.SetBool("Melia.Skill.Toggled", false);
			if (caster is Character character)
				Send.ZC_NORMAL.SkillToggle(character, SkillId.None);

			caster.StopBuff(BuffId.CirclingIncreaseSR_Buff);
			hawk.StopBuff(BuffId.UC_Detected_Debuff);
			this.CleanupCirclingPad(caster);
			FalconerHawkHelper.StopCircling(caster, hawk);
		}

		private void CleanupCirclingPad(ICombatEntity caster)
		{
			var padHandle = (int)caster.GetTempVar(CirclingPadVariable);
			if (padHandle != 0 && caster.Map.TryGetPad(padHandle, out var pad))
				pad?.Destroy();
			caster.RemoveTempVar(CirclingPadVariable);
		}
	}
}
