using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
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

		private static readonly Dictionary<string, int> HangingShotAttachOffsets = new()
		{
			{ "pet_hawk", -56 },
			{ "Toucan", -40 },
			{ "barn_owl", -40 },
		};

		private static int GetAttachOffset(Companion hawk)
		{
			if (hawk?.CompanionData != null && HangingShotAttachOffsets.TryGetValue(hawk.CompanionData.ClassName, out var baseOffset))
				return baseOffset;

			return 0;
		}

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

			if (caster is not Character chr)
				return;

			var buffTimeSpan = TimeSpan.FromSeconds(BuffDurationSeconds);

			// ===== Packet sequence matching official =====

			// 1. Disable control during pickup (ZC_ENABLE_CONTROL + ZC_LOCK_KEY)
			chr.ToggleControl("HANGINGSHOT", false);

			// If the hawk is perched, lift off
			if (hawk.IsLandedOnShoulder)
				hawk.TakeOff();
			else if (hawk.IsOnRoost)
				hawk.LeaveRoost();

			var targetHandle = target?.Handle ?? 0;

			// ZC_SKILL_READY
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);

			// 3. ZC_NORMAL UpdateSkillEffect
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);

			// 4. Buff + SetMainAttackSkill in sync block with SYNC_EXEC_BY_SKILL_TIME
			var syncKey1 = hawk.GenerateSyncKey();
			Send.ZC_SYNC_START(caster, syncKey1, 1);
			caster.StartBuff(BuffId.HangingShot, skill.Level, 0, buffTimeSpan, hawk, skill.Id);

			// Change normal attack to hanging shot variant
			Send.ZC_NORMAL.SetMainAttackSkill(caster as Character, SkillId.Bow_Hanging_Attack);

			Send.ZC_SYNC_END(caster, syncKey1, 1);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, syncKey1);

			// 5. Empty sync block with SYNC_EXEC_BY_SKILL_TIME
			var syncKey2 = hawk.GenerateSyncKey();
			Send.ZC_SYNC_START(caster, syncKey2, 1);
			Send.ZC_SYNC_END(caster, syncKey2, 1);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, syncKey2);

			// 6. ZC_MELEE_GROUND
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			// 7-8. Hawk-side attachment + flight: queued so it serialises
			// behind any other hawk skill already in flight, and holds the
			// queue for the full buff duration so no other hawk skill
			// interrupts the ride.
			FalconerHawkQueue.Enqueue(hawk, new HawkSkillRequest(
				skill, caster,
				ctx => Execute(ctx, chr, buffTimeSpan),
				SkipMovementLock: true));
		}

		private static async Task Execute(HawkSkillContext ctx, Character character, TimeSpan buffTimeSpan)
		{
			var caster = ctx.Caster;
			var hawk = ctx.Hawk;
			var skill = ctx.Skill;

			// Wait for skill animation to finish before attachment
			// (official shows ~1s gap between MELEE_GROUND and attachment)
			await ctx.Delay(1000);

			// 8. Attachment sequence in sync block with SYNC_EXEC
			var syncKey3 = hawk.GenerateSyncKey();
			Send.ZC_SYNC_START(caster, syncKey3, 1);
			Send.ZC_NORMAL.CollisionAndBack(hawk, caster, syncKey3, "HANGINGSHOT", 1f, 7f, 1f, 0.7f, 20f, false);

			await ctx.Delay(1000);

			hawk.Position = caster.Position;
			Send.ZC_NORMAL.ControlObject(caster, hawk, ControlLookType.None, false, false, null, true);
			Send.ZC_ATTACH_TO_OBJ(caster, hawk, "Dummy_hawk", null, 0.9f);
			caster.TurnTowards(Direction.North);
			await ctx.Delay(500);
			Send.ZC_DETACH_TO_OBJ(caster, hawk);
			var attachOffset = GetAttachOffset(hawk);
			Send.ZC_NORMAL.FlyWithObject(caster, hawk, HangingShotAttachNode, attachOffset);
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
				Math.Abs(attachOffset),
				AttachmentType.FlyWith,
				isController: true,
				syncDirection: false,
				sendPackets: false  // Already sent manually
			);

			character.Components.TryGet<MovementComponent>(out var movement);
			if (movement != null)
			{
				movement.SetAttachmentMovementMode(true);
				movement.NotifyFlying(true, 80f);
			}

			// Re-enable control after pickup
			await ctx.Delay(1000);
			character.ToggleControl("HANGINGSHOT", true);

			// Remove ground-based debuffs
			RemoveGroundDebuffs(caster);

			// Falconer23: [Arts] Hanging Shot: Dodge
			if (caster.IsAbilityActive(AbilityId.Falconer23))
				caster.StartBuff(BuffId.HangingShot_Falconer23_Buff, skill.Level, 0, buffTimeSpan, caster, skill.Id);

			try
			{
				// Monitor buff state — hold the queue for the full ride
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

					await ctx.Delay(1000);
				}
			}
			finally
			{
				await Cleanup(character, hawk, attachment, movement);
			}
		}

		private static async Task Cleanup(Character character, Companion hawk, AttachmentComponent attachment, MovementComponent movement)
		{
			if (attachment != null && attachment.IsAttached)
				attachment.Detach(sendPackets: false);

			var flyHeight = movement?.FlyHeight ?? 0f;

			if (movement != null)
			{
				movement.SetAttachmentMovementMode(false);
				movement.NotifyFlying(false, 0f);
			}

			Send.ZC_NORMAL.ControlObject(character, null, ControlLookType.SameDirection, true, true, "None", true);
			Send.ZC_NORMAL.FlyWithObject(character, null);
			Send.ZC_FLY(character, 0, 5);
			character.AttachToObject(null, "None", "None", attachSec: 0.5f);
			Send.ZC_MOVE_ANIM(hawk, FixedAnimation.ASTD, 0);

			// Sync hawk position to character so it doesn't appear stuck at
			// a stale server-side position after being detached.
			var groundY = character.Position.Y - flyHeight;
			var resumePos = new Position(character.Position.X, groundY + FalconerHawkHelper.DefaultHawkHeight, character.Position.Z);
			hawk.SetPosition(resumePos);

			await Task.CompletedTask;
		}

		private static void RemoveGroundDebuffs(ICombatEntity caster)
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
