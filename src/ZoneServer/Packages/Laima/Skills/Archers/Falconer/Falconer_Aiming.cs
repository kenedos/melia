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
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill Aiming.
	/// Toggle skill: hawk marks enemies in an area, increasing their effective
	/// hit radius for AoE attacks. Drains a fixed amount of SP per second while active.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_Aiming)]
	public class Falconer_AimingOverride : IGroundSkillHandler
	{
		private const string AimingPadVariable = "Falconer.Aiming.PadHandle";
		private const float SpPerSecond = 8f;

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

			if (skill.Vars.GetBool("Melia.Skill.Toggled"))
			{
				skill.Vars.SetBool("Melia.Skill.Toggled", false);
				if (caster is Character toggleOffChar)
					Send.ZC_NORMAL.SkillToggle(toggleOffChar, SkillId.None);

				Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

				caster.StopBuff(BuffId.Aiming_Hawk_Buff);
				this.CleanupAimingPad(caster);
				FalconerHawkHelper.StopAiming(caster, hawk);
				return;
			}

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

			skill.Run(this.HandleAimingActive(skill, caster, hawk));
		}

		private async Task HandleAimingActive(Skill skill, ICombatEntity caster, World.Actors.Monsters.Companion hawk)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(300));

			if (hawk.IsLandedOnShoulder)
				hawk.TakeOff();
			else if (hawk.IsOnRoost)
				hawk.LeaveRoost();

			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var pad = SkillCreatePad(caster, skill, hawk.Position, 0f, PadName.Falconer_Aiming);
			if (pad == null)
			{
				skill.Vars.SetBool("Melia.Skill.Toggled", false);
				if (caster is Character toggleFailChar)
					Send.ZC_NORMAL.SkillToggle(toggleFailChar, SkillId.None);
				FalconerHawkHelper.StopAiming(caster, hawk);
				return;
			}

			pad.FollowsTarget(hawk);
			pad.Trigger.LifeTime = TimeSpan.FromMinutes(5);

			caster.SetTempVar(AimingPadVariable, pad.Handle);
			hawk.Vars.Set("Hawk.Aiming.PadId", pad.Handle);

			caster.StartBuff(BuffId.Aiming_Hawk_Buff, skill.Level, 0f, TimeSpan.FromMinutes(5), caster, skill.Id);

			FalconerHawkHelper.ExecuteAiming(caster, hawk, skill, hawk.Position);

			const int MaxTicks = 300;
			for (var tick = 0; tick < MaxTicks; tick++)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(1000));

				if (!skill.Vars.GetBool("Melia.Skill.Toggled"))
					break;

				if (caster.IsDead || hawk.IsDead)
					break;

				if (pad.IsDead)
					break;

				if (!caster.TrySpendSp(SpPerSecond))
					break;
			}

			skill.Vars.SetBool("Melia.Skill.Toggled", false);
			if (caster is Character character)
				Send.ZC_NORMAL.SkillToggle(character, SkillId.None);

			caster.StopBuff(BuffId.Aiming_Hawk_Buff);
			this.CleanupAimingPad(caster);
			FalconerHawkHelper.StopAiming(caster, hawk);
		}

		private void CleanupAimingPad(ICombatEntity caster)
		{
			var padHandle = (int)caster.GetTempVar(AimingPadVariable);
			if (padHandle != 0 && caster.Map.TryGetPad(padHandle, out var pad))
				pad?.Destroy();
			caster.RemoveTempVar(AimingPadVariable);
		}
	}
}
