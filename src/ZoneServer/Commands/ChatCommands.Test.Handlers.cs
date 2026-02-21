using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Util;
using Yggdrasil.Util.Commands;

namespace Melia.Zone.Commands
{
	public partial class ChatCommands : CommandManager<ChatCommand, ChatCommandFunc>
	{
		/// <summary>
		/// Starts a time action visible only to a specific target.
		/// </summary>
		private CommandResult HandleTimeActionOnlyTarget(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 3)
			{
				sender.ServerMessage("Usage: /timeactiontarget <player_name> <animation_name> <seconds> [message]");
				sender.ServerMessage("Example: /timeactiontarget OtherPlayer Repair 10 Repairing your item...");
				return CommandResult.InvalidArgument;
			}

			var targetName = args.Get(0);
			var targetPlayer = ZoneServer.Instance.World.GetCharacterByTeamName(targetName);
			if (targetPlayer == null)
			{
				sender.ServerMessage($"Player '{targetName}' not found.");
				return CommandResult.Fail;
			}

			var animationName = args.Get(1);

			if (!float.TryParse(args.Get(2), NumberStyles.Float, CultureInfo.InvariantCulture, out var durationSec))
			{
				sender.ServerMessage("Invalid duration in seconds.");
				return CommandResult.InvalidArgument;
			}

			var displayText = args.Count > 3 ? string.Join(" ", args.GetAll().Skip(3)) : "Performing Action...";

			// The 'sender' of the command is the one performing the action.
			Send.ZC_NORMAL.TimeActionOnlyTarget(sender.Connection, sender, displayText, animationName, TimeSpan.FromSeconds(durationSec));

			sender.ServerMessage($"Sent targeted time action to '{targetName}'.");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Plays a state-based animation on a targeted NPC.
		/// </summary>
		private CommandResult HandleNpcStateAnim(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (!int.TryParse(args.Get(0), out var handle))
			{
				sender.ServerMessage("Actor handle must be an integer.");
				return CommandResult.InvalidArgument;
			}

			var targetedActor = target.Map.GetMonster(handle);
			if (targetedActor == null)
			{
				sender.ServerMessage("You must target an NPC or monster to use this command.");
				return CommandResult.Fail;
			}

			if (args.Count != 2)
			{
				sender.ServerMessage("Usage: /npcstateanim <handle> <state_id> <animation_name>");
				sender.ServerMessage("Example: /npcstateanim 1 Atk_Slow");
				return CommandResult.InvalidArgument;
			}

			if (!int.TryParse(args.Get(1), out var stateId))
			{
				sender.ServerMessage("State ID must be an integer.");
				return CommandResult.InvalidArgument;
			}

			var animName = args.Get(2);

			Send.ZC_NORMAL.NpcStateAnimation(targetedActor, stateId, animName);
			sender.ServerMessage($"Sent NPC state animation command to '{((IMonsterAppearance)targetedActor).Name}'.");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Creates a wind area effect for testing purposes.
		/// </summary>
		private CommandResult HandleWindArea(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count != 6)
			{
				sender.ServerMessage("Usage: /wind <power> <direction> <width> <height> <push_distance> <duration>");
				sender.ServerMessage("Example: /wind 100 0 100 300 150 5");
				return CommandResult.InvalidArgument;
			}

			if (!float.TryParse(args.Get(0), NumberStyles.Float, CultureInfo.InvariantCulture, out var power) ||
				!float.TryParse(args.Get(1), NumberStyles.Float, CultureInfo.InvariantCulture, out var dir) ||
				!float.TryParse(args.Get(2), NumberStyles.Float, CultureInfo.InvariantCulture, out var width) ||
				!float.TryParse(args.Get(3), NumberStyles.Float, CultureInfo.InvariantCulture, out var height) ||
				!float.TryParse(args.Get(4), NumberStyles.Float, CultureInfo.InvariantCulture, out var dist) ||
				!float.TryParse(args.Get(5), NumberStyles.Float, CultureInfo.InvariantCulture, out var time))
			{
				sender.ServerMessage("All arguments must be valid numbers.");
				return CommandResult.InvalidArgument;
			}

			// The command will create the wind area originating from the target character
			Send.ZC_NORMAL.WindArea(target, power, dir, width, height, dist, time);

			sender.ServerMessage($"Wind area created at {target.TeamName}'s location.");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Throws a UI effect from the sender to a target.
		/// </summary>
		private CommandResult HandleThrowUIToActor(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1 || !int.TryParse(args.Get(0), out var targetHandle))
			{
				sender.ServerMessage("Usage: /throwui <target_handle>");
				return CommandResult.InvalidArgument;
			}
			var targetActor = sender.Map.GetCombatEntity(targetHandle);
			if (targetActor == null)
			{
				sender.ServerMessage($"Target with handle {targetHandle} not found.");
				return CommandResult.Fail;
			}

			// Using some default values for a quick test
			var offset = new Position(0, 50, 0); // Above the target
			var flyTime = 1.5f;
			var delayTime = 0.5f;
			var gravity = -20f;
			var easing = 0.5f;
			var endEffect = "F_hit009_ice";
			var endEffectScale = 2.0f;

			Send.ZC_NORMAL.ThrowUIToActor(sender, targetActor, offset, flyTime, delayTime, gravity, easing, endEffect, endEffectScale);
			sender.ServerMessage($"Sent ThrowUIToActor command from you to handle {targetHandle}.");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Fixes the billboard rotation of the target character.
		/// </summary>
		private CommandResult HandleFixBillboard(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1 || !float.TryParse(args.Get(0), NumberStyles.Float, CultureInfo.InvariantCulture, out var angle))
			{
				sender.ServerMessage("Usage: /fixbillboard <angle_in_degrees> [1=on/0=off]");
				return CommandResult.InvalidArgument;
			}

			var enabled = true;
			if (args.Count > 1 && args.Get(1) == "0")
			{
				enabled = false;
			}

			Send.ZC_NORMAL.SetFixRotateBillboard(target, angle, enabled);
			sender.ServerMessage($"Set fixed billboard rotation for {target.TeamName} to {angle} degrees (Enabled: {enabled}).");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Attaches a visual item model to the target character.
		/// </summary>
		private CommandResult HandleAttachItem(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 2)
			{
				sender.ServerMessage("Usage: /attachitem <item_id> <slot_name> [1=attach/0=detach]");
				return CommandResult.InvalidArgument;
			}

			if (!int.TryParse(args.Get(0), out var itemId) || !ZoneServer.Instance.Data.ItemDb.TryFind(itemId, out var itemData))
			{
				sender.ServerMessage("Invalid or non-existent item ID.");
				return CommandResult.Fail;
			}

			if (!Enum.TryParse<EquipSlot>(args.Get(1), true, out var slot))
			{
				sender.ServerMessage("Invalid slot name (e.g., RH, LH, Head).");
				return CommandResult.Fail;
			}

			var attach = true;
			if (args.Count > 2 && args.Get(2) == "0")
			{
				attach = false;
			}

			var xacName = $"warrior_f_sword_gladius";

			Send.ZC_NORMAL.AttachItemToMonster(target, xacName, slot, itemData.Id, itemData.ClassName, attach);
			sender.ServerMessage($"{(attach ? "Attached" : "Detached")} item {itemData.Name} on {target.TeamName}'s {slot} slot.");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Plays an effect at the character's position and saves its handle.
		/// </summary>
		private CommandResult HandleTestEffect(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
			{
				sender.ServerMessage("Usage: /testeffect <effect_name> [duration=5]");
				return CommandResult.InvalidArgument;
			}

			var effectName = args.Get(0);
			var duration = 5.0f;
			if (args.Count > 1 && !float.TryParse(args.Get(1), out duration))
			{
				sender.ServerMessage("Invalid duration.");
				return CommandResult.InvalidArgument;
			}

			var effectHandle = ZoneServer.Instance.World.CreateEffectHandle();
			Send.ZC_NORMAL.PlayEffectAtPosition(sender, effectName, sender.Position, 1.0f, effectHandle, duration);
			sender.Variables.Temp.SetInt("LastEffectHandle", effectHandle);
			sender.ServerMessage($"Played effect '{effectName}' with handle {effectHandle}. Use /endeffect to remove it.");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Ends a previously created effect.
		/// </summary>
		private CommandResult HandleEndEffect(Character sender, Character target, string message, string commandName, Arguments args)
		{
			int effectHandle;
			if (args.Count > 0)
			{
				if (!int.TryParse(args.Get(0), out effectHandle))
				{
					sender.ServerMessage("Invalid effect handle.");
					return CommandResult.InvalidArgument;
				}
			}
			else if (!sender.Variables.Temp.TryGetInt("LastEffectHandle", out effectHandle))
			{
				sender.ServerMessage("No effect handle saved. Use /testeffect first, or specify a handle.");
				return CommandResult.Fail;
			}

			Send.ZC_NORMAL.EndEffect(sender, effectHandle);
			sender.ServerMessage($"Sent command to end effect with handle {effectHandle}.");
			sender.Variables.Temp.Remove("LastEffectHandle");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Attaches one actor to a node on another actor.
		/// </summary>
		private CommandResult HandleAttachToNode(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 3)
			{
				sender.ServerMessage("Usage: /attachtonode <handle_to_attach> <target_handle> <node_name>");
				sender.ServerMessage("Example: /attachtonode 12345 54321 Bip01-Head");
				return CommandResult.InvalidArgument;
			}
			if (!int.TryParse(args.Get(0), out var attachHandle) || !int.TryParse(args.Get(1), out var targetHandle))
			{
				sender.ServerMessage("Handles must be integers.");
				return CommandResult.InvalidArgument;
			}

			var actorToAttach = sender.Map.GetCombatEntity(attachHandle);
			var targetActor = sender.Map.GetCombatEntity(targetHandle);
			var nodeName = args.Get(2);

			if (actorToAttach == null || targetActor == null)
			{
				sender.ServerMessage("One or both actors not found on this map.");
				return CommandResult.Fail;
			}

			Send.ZC_NORMAL.AttachToNode(actorToAttach, targetActor, nodeName);
			sender.ServerMessage($"Sent attach command for handle {attachHandle} to {targetHandle} on node '{nodeName}'.");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Tests the old PlayEffect packet (0x15).
		/// </summary>
		/// <remarks>Tested with newer client, either packet structure is wrong or doesn't work.</remarks>
		private CommandResult HandlePlayEffect15(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
			{
				sender.ServerMessage("Usage: /playeffect15 <effect_name>");
				return CommandResult.InvalidArgument;
			}

			var effectName = args.Get(0);
			Send.ZC_NORMAL.PlayEffect_15(target, effectName, 1.0f, EffectLocation.Bottom);
			sender.ServerMessage($"Sent old PlayEffect (0x15) for '{effectName}' on target.");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Tests the PlayForceReflect packet.
		/// </summary>
		private CommandResult HandlePlayForceReflect(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count != 3)
			{
				sender.ServerMessage("Usage: /reflect <original_caster_handle> <new_target_handle> <skill_id>");
				return CommandResult.InvalidArgument;
			}

			if (!int.TryParse(args.Get(0), out var origCasterHandle) ||
				!int.TryParse(args.Get(1), out var newTargetHandle) ||
				!int.TryParse(args.Get(2), out var skillIdInt))
			{
				sender.ServerMessage("Handles and Skill ID must be integers.");
				return CommandResult.InvalidArgument;
			}

			var origCaster = sender.Map.GetCombatEntity(origCasterHandle);
			var newTarget = sender.Map.GetCombatEntity(newTargetHandle);

			if (origCaster == null || newTarget == null)
			{
				sender.ServerMessage("Original caster or new target not found on this map.");
				return CommandResult.Fail;
			}

			Send.ZC_NORMAL.PlayForceReflect(target, origCaster, newTarget, (SkillId)skillIdInt, 12345);
			sender.ServerMessage($"Sent reflect packet. Reflector: {target.Name}, Original Caster: Handle {origCasterHandle}, New Target: Handle {newTargetHandle}.");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Tests the old PlayForceEffect (0x16) packet.
		/// </summary>
		private CommandResult HandlePlayForceEffect16(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 2)
			{
				sender.ServerMessage("Usage: /playforce16 <target_handle> <effect_name>");
				return CommandResult.InvalidArgument;
			}

			if (!int.TryParse(args.Get(0), out var targetHandle))
			{
				sender.ServerMessage("Target handle must be an integer.");
				return CommandResult.InvalidArgument;
			}

			var targetActor = sender.Map.GetCombatEntity(targetHandle);
			if (targetActor == null)
			{
				sender.ServerMessage($"Target with handle {targetHandle} not found.");
				return CommandResult.Fail;
			}

			var effectName = args.Get(1);
			var forceId = ForceId.GetNew();

			Send.ZC_NORMAL.PlayForceEffect_16(forceId, sender, sender, targetActor, effectName, 1.0f, "None", "F_hit020", 1.0f, "None", "None", 500f);
			sender.ServerMessage($"Sent old PlayForceEffect (0x16) to target handle {targetHandle}.");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Tests the AddEffect packet.
		/// </summary>
		private CommandResult HandleAddEffect(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
			{
				sender.ServerMessage("Usage: /addeffect <effect_name> [scale]");
				return CommandResult.InvalidArgument;
			}

			var effectName = args.Get(0);
			var scale = 1.0f;
			float.TryParse(args.Get(1), NumberStyles.Float, CultureInfo.InvariantCulture, out scale);

			Send.ZC_NORMAL.AddEffect(target, effectName, scale);
			sender.ServerMessage($"Added effect '{effectName}' to {target.TeamName}.");
			return CommandResult.Okay;
		}

		/// <summary>
		/// Tests the RemoveEffectByName packet.
		/// </summary>
		private CommandResult HandleRemoveEffectByName(Character sender, Character target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
			{
				sender.ServerMessage("Usage: /removeeffectbyname <effect_name>");
				return CommandResult.InvalidArgument;
			}

			var effectName = args.Get(0);

			Send.ZC_NORMAL.RemoveEffectByName(target, effectName);
			sender.ServerMessage($"Sent remove command for effect '{effectName}' from {target.TeamName}.");
			return CommandResult.Okay;
		}
	}
}
