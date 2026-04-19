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
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill Hanging Shot.
	/// The falconer grabs onto the hawk and is lifted into the air,
	/// gaining increased evasion, movement speed, and the ability to
	/// attack while moving. Also removes certain debuffs.
	/// </summary>
	/// <remarks>
	/// Packet sequence from official:
	/// 1. ZC_ENABLE_CONTROL / ZC_LOCK_KEY
	/// 2. ZC_SKILL_ADD / ZC_PC_ATKSTATE / ZC_SKILL_READY / ZC_UPDATE_SP
	/// 3. ZC_NORMAL UpdateSkillEffect
	/// 4. SYNC_START -> ZC_BUFF_ADD + SetMainAttackSkill -> SYNC_END -> SYNC_EXEC_BY_SKILL_TIME
	/// 5. Empty SYNC_START -> SYNC_END -> SYNC_EXEC_BY_SKILL_TIME
	/// 6. ZC_MELEE_GROUND
	/// 7. ZC_NORMAL CollisionAndBack
	/// 8. SYNC_START -> ControlObject + FlyWithObject + MOVE_ANIM -> SYNC_END -> SYNC_EXEC
	/// </remarks>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_HangingShot)]
	public class Falconer_HangingShotOverride : IGroundSkillHandler
	{
		private const int BuffDurationSeconds = 30;
		private const string HangingShotAttachNode = "Dummy_hawk";
		private const int HangingShotAttachOffset = -56;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!FalconerHawkHelper.TryGetHawk(caster, out var hawk))
			{
				if (caster is Character character)
					character.SystemMessage("CompanionIsNotActive");
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (FalconerHawkHelper.IsHawkBusy(hawk))
			{
				caster.ServerMessage(Localization.Get("Hawk is busy."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (caster is not Character chr)
				return;

			var buffTimeSpan = TimeSpan.FromSeconds(BuffDurationSeconds);

			// ===== Packet sequence matching official =====

			// 1. Disable control during pickup (ZC_ENABLE_CONTROL + ZC_LOCK_KEY)
			chr.ToggleControl("HANGINGSHOT", false);

			// 2. Standard skill packets (handled by framework)
			// ZC_SKILL_ADD, ZC_PC_ATKSTATE already sent by framework

			var targetHandle = target?.Handle ?? 0;

			// ZC_SKILL_READY
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);

			// ZC_UPDATE_SP handled by TrySpendSp

			// 3. ZC_NORMAL UpdateSkillEffect
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);

			// 4. Buff + SetMainAttackSkill in sync block with SYNC_EXEC_BY_SKILL_TIME
			var syncKey1 = hawk.GenerateSyncKey();
			Send.ZC_SYNC_START(caster, syncKey1, 1);
			caster.StartBuff(BuffId.HangingShot, skill.Level, 0, buffTimeSpan, hawk, skill.Id);
			// Note: SetMainAttackSkill is sent by buff handler OnActivate
			Send.ZC_SYNC_END(caster, syncKey1, 1);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, syncKey1);

			// 5. Empty sync block with SYNC_EXEC_BY_SKILL_TIME
			var syncKey2 = hawk.GenerateSyncKey();
			Send.ZC_SYNC_START(caster, syncKey2, 1);
			Send.ZC_SYNC_END(caster, syncKey2, 1);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, syncKey2);

			// 6. ZC_MELEE_GROUND
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			// 7-8. Run the async portion for attachment
			skill.Run(this.HandleSkillAsync(caster, skill, hawk, chr, buffTimeSpan));
		}

		private async Task HandleSkillAsync(ICombatEntity caster, Skill skill, Companion hawk, Character character, TimeSpan buffTimeSpan)
		{
			// Wait for skill animation to finish before attachment
			// (official shows ~1s gap between MELEE_GROUND and attachment)
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			// Lock hawk
			hawk.Vars.Set("Hawk.UsingSkill", true);
			hawk.Vars.Set("Hawk.SkillFunction", "HangingShot");

			// Take off from shoulder if landed
			if (hawk.IsLandedOnShoulder)
				hawk.TakeOff();

			// 7. ZC_NORMAL CollisionAndBack (outside sync block)
			Send.ZC_NORMAL.CollisionAndBack(hawk, caster, 0, "HANGINGSHOT", 1f, 7f, 1f, 0.7f, 20f, false);

			// 8. Attachment sequence in sync block with SYNC_EXEC
			var syncKey3 = hawk.GenerateSyncKey();
			Send.ZC_SYNC_START(caster, syncKey3, 1);
			Send.ZC_NORMAL.ControlObject(caster, hawk, ControlLookType.None, false, false, null, true);
			Send.ZC_NORMAL.FlyWithObject(caster, hawk, HangingShotAttachNode, HangingShotAttachOffset);
			Send.ZC_MOVE_ANIM(hawk, FixedAnimation.ASTD, 0);
			Send.ZC_SYNC_END(caster, syncKey3, 1);
			Send.ZC_SYNC_EXEC(caster, syncKey3);

			// Setup server-side attachment tracking
			if (!character.Components.TryGet<AttachmentComponent>(out var attachment))
			{
				attachment = new AttachmentComponent(character);
				character.Components.Add(attachment);
			}

			attachment.AttachTo(
				hawk,
				HangingShotAttachNode,
				Math.Abs(HangingShotAttachOffset),
				AttachmentType.FlyWith,
				isController: true,
				syncDirection: false,
				sendPackets: false  // Already sent manually
			);

			if (character.Components.TryGet<MovementComponent>(out var movement))
				movement.SetAttachmentMovementMode(true);

			// Re-enable control after pickup
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			character.ToggleControl("HANGINGSHOT", true);

			// Remove ground-based debuffs
			this.RemoveGroundDebuffs(caster);

			// Falconer23: [Arts] Hanging Shot: Dodge
			if (caster.IsAbilityActive(AbilityId.Falconer23))
				caster.StartBuff(BuffId.HangingShot_Falconer23_Buff, skill.Level, 0, buffTimeSpan, caster, skill.Id);

			// Monitor buff state
			var endTime = DateTime.Now.Add(buffTimeSpan);
			while (DateTime.Now < endTime && !caster.IsDead && !hawk.IsDead)
			{
				if (!caster.IsBuffActive(BuffId.HangingShot))
					break;

				if (!hawk.IsActivated)
				{
					caster.StopBuff(BuffId.HangingShot);
					break;
				}

				await skill.Wait(TimeSpan.FromMilliseconds(1000));
			}

			// Cleanup
			await this.CleanupHangingShot(skill, character, hawk, attachment, movement);
		}

		private async Task CleanupHangingShot(Skill skill, Character character, Companion hawk, AttachmentComponent attachment, MovementComponent movement)
		{
			hawk.Vars.Set("Hawk.UsingSkill", false);

			if (attachment != null && attachment.IsAttached)
				attachment.Detach(sendPackets: false);

			movement?.SetAttachmentMovementMode(false);

			Send.ZC_NORMAL.ControlObject(character, null, ControlLookType.SameDirection, true, true, "None", true);
			Send.ZC_NORMAL.FlyWithObject(character, null);
			Send.ZC_MOVE_ANIM(hawk, FixedAnimation.EMPTY, 0);

			await FalconerHawkHelper.HawkFlyAway(skill, character, hawk);
		}

		private void RemoveGroundDebuffs(ICombatEntity caster)
		{
			var debuffsToRemove = new[] { BuffId.Slow_Debuff, BuffId.Freeze };

			foreach (var debuffId in debuffsToRemove)
			{
				if (caster.IsBuffActive(debuffId))
					caster.StopBuff(debuffId);
			}
		}
	}
}
