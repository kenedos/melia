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
	/// Toggle skill: hawk circles an area, revealing hidden enemies and
	/// boosting allies' AoE Attack Ratio while draining SP per second.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_Circling)]
	public class Falconer_CirclingOverride : IGroundSkillHandler
	{
		private const string CirclingPadVariable = "Falconer.Circling.PadHandle";
		private const float BaseSpPerSecond = 18f;
		private const float SpPerSecondPerLevel = 1.5f;
		private const float MinSpPerSecond = 1f;

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

			// Toggle off: cancel circling (no SP check — caster must always be able to disable)
			if (skill.Vars.GetBool("Melia.Skill.Toggled"))
			{
				skill.Vars.SetBool("Melia.Skill.Toggled", false);
				if (caster is Character toggleOffChar)
					Send.ZC_NORMAL.SkillToggle(toggleOffChar, SkillId.None);

				Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

				this.StripAllyBuffs(caster);
				this.CleanupCirclingPad(caster);
				FalconerHawkHelper.StopCircling(caster, hawk);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			if (caster.IsAbilityActive(AbilityId.Falconer11))
			{
				var extraSp = skill.Properties.GetFloat(PropertyName.SpendSP);
				if (!caster.TrySpendSp(extraSp))
				{
					caster.ServerMessage(Localization.Get("Not enough SP."));
					return;
				}
			}

			// Toggle on: activate circling
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			skill.Vars.SetBool("Melia.Skill.Toggled", true);
			if (caster is Character toggleOnChar)
				Send.ZC_NORMAL.SkillToggle(toggleOnChar, skill.Id);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleCirclingActive(skill, caster, hawk));
		}

		private async Task HandleCirclingActive(Skill skill, ICombatEntity caster, World.Actors.Monsters.Companion hawk)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1200));

			if (hawk.IsLandedOnShoulder)
				hawk.TakeOff();
			else if (hawk.IsOnRoost)
				hawk.LeaveRoost();

			await skill.Wait(TimeSpan.FromMilliseconds(500));

			var pad = SkillCreatePad(hawk, skill, hawk.Position, 0f, PadName.Falconer_Circling);
			if (pad == null)
			{
				skill.Vars.SetBool("Melia.Skill.Toggled", false);
				if (caster is Character toggleFailChar)
					Send.ZC_NORMAL.SkillToggle(toggleFailChar, SkillId.None);
				FalconerHawkHelper.StopCircling(caster, hawk);
				return;
			}

			pad.FollowsTarget(hawk);
			pad.Trigger.LifeTime = TimeSpan.FromMinutes(5);

			caster.SetTempVar(CirclingPadVariable, pad.Handle);
			hawk.Vars.Set("Hawk.Circling.PadId", pad.Handle);

			pad.ChangeGroundEffect("F_archer_circling_ground", 1.7f);

			FalconerHawkHelper.ExecuteCircling(caster, hawk, skill, hawk.Position);

			var spPerSecond = Math.Max(MinSpPerSecond, BaseSpPerSecond - SpPerSecondPerLevel * skill.Level);
			if (caster.IsAbilityActive(AbilityId.Falconer11))
				spPerSecond *= 2f;

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

				if (!caster.TrySpendSp(spPerSecond))
					break;
			}

			skill.Vars.SetBool("Melia.Skill.Toggled", false);
			if (caster is Character character)
				Send.ZC_NORMAL.SkillToggle(character, SkillId.None);

			this.StripAllyBuffs(caster);
			this.CleanupCirclingPad(caster);
			FalconerHawkHelper.StopCircling(caster, hawk);
		}

		private void StripAllyBuffs(ICombatEntity caster)
		{
			caster.StopBuff(BuffId.CirclingIncreaseSR_Buff);

			if (caster is not Character character || character.Connection.Party == null)
				return;

			var members = caster.Map.GetPartyMembersInRange(character, 0f, true);
			foreach (var member in members)
			{
				if (member == caster)
					continue;

				member.StopBuff(BuffId.CirclingIncreaseSR_Buff);
			}
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
