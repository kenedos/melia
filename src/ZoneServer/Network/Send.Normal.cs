using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection.Emit;
using System.Security.Cryptography;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Const.Web;
using Melia.Shared.Network;
using Melia.Shared.Network.Helpers;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Util;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.Network.Helpers;
using Melia.Zone.Skills;
using Melia.Zone.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.World.Groups;
using Melia.Zone.World.Items;
using Melia.Zone.World.Maps;
using Melia.Zone.World.Storages;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;

namespace Melia.Zone.Network
{
	public static partial class Send
	{
		public static partial class ZC_NORMAL
		{
			/// <summary>
			/// Starts a time action, displaying a progress bar and
			/// potentially putting the character in an animation.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="timeAction"></param>
			public static void TimeActionStart(Character character, TimeAction timeAction)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.TimeActionStart);

				packet.PutInt(character.Handle);
				packet.PutLpString(timeAction.DisplayText);
				packet.PutLpString(timeAction.AnimationName);
				packet.PutFloat((float)timeAction.Duration.TotalSeconds);
				packet.PutByte(1);
				if (Versions.Protocol > 500)
					packet.PutLpString(timeAction.ButtonText);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Stops a time action, hiding the progress bar and reverting
			/// to the default animation.
			/// </summary>
			/// <param name="character"></param>
			public static void TimeActionEnd(Character character, TimeActionResult result)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.TimeActionEnd);

				packet.PutInt(character.Handle);
				packet.PutByte((byte)result);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Starts a time action that is only visible to a specific target player.
			/// Used for interactions like item repairs at a Squire's camp.
			/// </summary>
			/// <param name="target">The actor performing the action (e.g., the Squire).</param>
			/// <param name="target">The character who will see the progress bar.</param>
			/// <param name="message">The text to display on the progress bar.</param>
			/// <param name="animationName">The animation the caster should play.</param>
			/// <param name="duration">The duration of the action.</param>
			/// <remarks>
			/// Corresponds to script_list.xml ClassID 2194: ShowingTimeActionOnlyTarget(seller, target, msg, animID, second)
			/// </remarks>
			public static void TimeActionOnlyTarget(IZoneConnection conn, Character target, string message, string animationName, TimeSpan duration)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.TimeActionOnlyTarget);

				packet.PutInt(target.Handle);
				packet.PutLpString(message);
				packet.PutLpString(animationName);
				packet.PutFloat((float)duration.TotalSeconds);

				conn.Send(packet);
			}

			/// <summary>
			/// Plays a specific animation for an NPC based on its state.
			/// </summary>
			/// <param name="npc">The NPC actor.</param>
			/// <param name="state">The state to associate the animation with.</param>
			/// <param name="animationName">The animation to play.</param>
			/// <remarks>
			/// Corresponds to script_list.xml ClassID 1926: AddNPCStateAnim(self, state, 'AnimName')
			/// </remarks>
			public static void NpcStateAnimation(IActor npc, int state, string animationName)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.NpcStateAnimation);

				packet.PutInt(npc.Handle);
				packet.PutInt(state);
				packet.AddStringId(animationName);

				npc.Map.Broadcast(packet, npc);
			}

			/// <summary>
			/// Creates a wind effect area that pushes actors.
			/// </summary>
			/// <param name="actor">The actor creating the wind area.</param>
			/// <param name="windPower">The strength of the wind.</param>
			/// <param name="direction">The direction of the wind in degrees.</param>
			/// <param name="width">The width of the affected area.</param>
			/// <param name="height">The length/height of the affected area.</param>
			/// <param name="moveHeight">The distance actors are pushed.</param>
			/// <param name="totalTime">The duration of the effect.</param>
			/// <remarks>
			/// Corresponds to script_list.xml ClassID 817: WindArea(self, windPower, dir, width, height, moveHeight, totalTime)
			/// </remarks>
			public static void WindArea(IActor actor, float windPower, float direction, float width, float height, float moveHeight, float totalTime)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.WindArea);

				packet.PutInt(actor.Handle);
				packet.PutFloat(windPower);
				packet.PutFloat(direction);
				packet.PutFloat(width);
				packet.PutFloat(height);
				packet.PutFloat(moveHeight);
				packet.PutFloat(totalTime);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Plays an animation of an effect getting thrown from the
			/// entity to the position, where a second effect is played
			/// for the impact.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="position"></param>
			/// <param name="startingEffect"></param>
			/// <param name="startingEffectScale"></param>
			/// <param name="endingEffect"></param>
			/// <param name="endingEffectScale"></param>
			/// <param name="range"></param>
			/// <param name="flightTime"></param>
			/// <param name="delay"></param>
			/// <param name="gravity"></param>
			/// <param name="speed"></param>
			/// <param name="dcDelay"></param>
			/// <param name="dcEffectScale"></param>
			/// <param name="dcEffect"></param>
			public static void SkillProjectile(IActor actor, Position position,
				string startingEffect, float startingEffectScale,
				string endingEffect, float endingEffectScale,
				float range, TimeSpan flightTime, TimeSpan delay = default, float gravity = 0, float speed = 1,
				TimeSpan dcDelay = default, float dcEffectScale = 0, string dcEffect = "None")
			{
				if (delay == default)
					delay = TimeSpan.Zero;
				if (dcDelay == default)
					dcDelay = TimeSpan.Zero;

				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_MissileThrow);

				packet.PutInt(actor.Handle);
				packet.AddStringId(startingEffect);
				packet.PutFloat(startingEffectScale);
				packet.AddStringId(endingEffect);
				packet.PutFloat(endingEffectScale);
				packet.PutPosition(position);
				packet.PutFloat(range);
				packet.PutFloat((float)flightTime.TotalSeconds);
				packet.PutFloat((float)delay.TotalSeconds);
				packet.PutFloat(gravity);
				packet.PutFloat(speed);
				packet.PutFloat((float)dcDelay.TotalSeconds);
				packet.PutFloat(dcEffectScale);
				packet.PutLpString(dcEffect);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Plays an animation of an effect falling from the
			/// sky to the position, where a second effect is played
			/// for the impact.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="skillClassName"></param>
			/// <param name="fallingEffectName"></param>
			/// <param name="fallingEffectSize"></param>
			/// <param name="groundEffectName"></param>
			/// <param name="groundEffectSize"></param>
			/// <param name="position"></param>
			/// <exception cref="ArgumentException"></exception>
			public static void SkillFallingProjectile(IActor actor, string skillClassName, string fallingEffectName, float fallingEffectSize,
				string groundEffectName, float groundEffectSize, Position position, float range, TimeSpan startDelay, float flyTime, float fallHeight, float easing, float startEasing)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillFallingProjectile);

				packet.PutInt(actor.Handle);
				packet.PutLpString(skillClassName);
				packet.AddStringId(fallingEffectName);
				packet.PutFloat(fallingEffectSize);
				packet.AddStringId(groundEffectName);
				packet.PutFloat(groundEffectSize);
				packet.PutPosition(position);
				packet.PutFloat(range);
				packet.PutFloat((float)startDelay.TotalSeconds);
				packet.PutFloat(flyTime);
				packet.PutFloat(fallHeight);
				packet.PutFloat(easing);
				packet.PutFloat(startEasing);

				actor.Map.Broadcast(packet, actor);
			}

			public static void Skill_08(IActor actor, string effectId, float effectSize, Position position, float v3, float v4, float v5, float v6, float v7)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillMonsterToss);

				packet.PutInt(actor.Handle);
				packet.AddStringId(effectId);
				packet.PutFloat(effectSize);
				packet.PutPosition(position);
				packet.PutFloat(v3);
				packet.PutFloat(v4);
				packet.PutFloat(v5);
				packet.PutFloat(v6);
				packet.PutFloat(v7);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Throws an actor to a specific destination using parabolic/gravity-based physics.
			/// Plays an effect and scale upon landing.
			/// </summary>
			/// <param name="actor">The actor being thrown (self).</param>
			/// <param name="endEffect">The effect name/ID to play upon landing.</param>
			/// <param name="endEffectScale">The visual scale of the landing effect.</param>
			/// <param name="targetPos">The X, Y, Z destination coordinates.</param>
			/// <param name="travelTime">Duration (seconds) of the flight/toss.</param>
			/// <param name="delayTime">Initial delay (seconds) before the movement starts.</param>
			/// <param name="gravity">Gravity force applied (affects the height of the arc).</param>
			/// <param name="horzEasing">Horizontal easing for the movement (smoothness).</param>
			/// <param name="effectMoveDelay">Delay before the 'moving' effect starts.</param>
			public static void ThrowActor(
				IActor actor,
				string endEffect,
				float endEffectScale,
				Position targetPos,
				float travelTime,
				float delayTime,
				float gravity,
				float horzEasing,
				float effectMoveDelay)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillMonsterToss);

				packet.PutInt(actor.Handle);
				packet.AddStringId(endEffect);    // endEffect
				packet.PutFloat(endEffectScale);  // endEftScale
				packet.PutPosition(targetPos);    // x, y, z

				packet.PutFloat(travelTime);      // time
				packet.PutFloat(delayTime);       // delayTime
				packet.PutFloat(gravity);         // gravity
				packet.PutFloat(horzEasing);      // horzEasing
				packet.PutFloat(effectMoveDelay); // eftMoveDelay

				// Broadcast to all, excluding the actor itself if needed
				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Plays an animation of an item getting thrown from the
			/// entity to the position, where a second effect is played
			/// for the impact.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="xacHeadName"></param>
			/// <param name="equipSlot"></param>
			/// <param name="position"></param>
			/// <param name="endEftName"></param>
			/// <param name="endScale"></param>
			/// <param name="flyTime"></param>
			/// <param name="gravity"></param>
			/// <param name="delayTime"></param>
			/// <param name="easing"></param>
			/// <param name="startRotateBillboard"></param>
			/// <param name="endRotateBillboard"></param>
			/// <param name="itemScale"></param>
			/// <param name="itemAppearOnFloorDuration"></param>
			public static void SkillItemToss(IActor actor, string xacHeadName, string equipSlot, Position position, string endEftName,
				float endScale, float flyTime, float delayTime, float gravity, float easing,
				float startRotateBillboard, float endRotateBillboard, float itemScale = 0, float itemAppearOnFloorDuration = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillItemToss);
				packet.PutInt(actor.Handle);
				packet.PutLpString(xacHeadName);
				packet.PutLpString(equipSlot);
				packet.PutPosition(position);
				packet.AddStringId(endEftName);
				packet.PutFloat(endScale);
				packet.PutFloat(flyTime);
				packet.PutFloat(delayTime);
				packet.PutFloat(gravity);
				packet.PutFloat(easing);
				packet.PutFloat(startRotateBillboard);
				packet.PutFloat(endRotateBillboard);
				packet.PutFloat(itemScale);
				packet.PutFloat(itemAppearOnFloorDuration);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Throws a client-side scripted object/effect from a caster to an actor.
			/// </summary>
			/// <param name="caster">The actor throwing the UI object.</param>
			/// <param name="target">The actor being targeted.</param>
			/// <param name="offset">The position offset from the target's origin.</param>
			/// <param name="flyTime">The duration of the flight.</param>
			/// <param name="delayTime">The delay before the flight starts.</param>
			/// <param name="gravity">The gravity affecting the projectile.</param>
			/// <param name="easing">The easing value for the movement.</param>
			/// <param name="endEffect">The effect to play on impact.</param>
			/// <param name="endEffectScale">The scale of the impact effect.</param>
			/// <remarks>
			/// Corresponds to script_list.xml ClassID 1011: ThrowUIToActor(self, target, x, y, z, updateScp, endScp, flyTime, delayTime, gravity, horEasing, endEffect, endEftScale)
			/// </remarks>
			public static void ThrowUIToActor(IActor caster, IActor target, Position offset, float flyTime, float delayTime, float gravity, float easing, string endEffect, float endEffectScale)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ThrowUIToActor);

				packet.PutInt(caster.Handle);
				packet.PutInt(target.Handle);
				packet.PutPosition(offset);
				packet.AddStringId("None"); // updateScp - for a generic function, "None" is safe.
				packet.AddStringId("None"); // endScp - for a generic function, "None" is safe.
				packet.PutFloat(flyTime);
				packet.PutFloat(delayTime);
				packet.PutFloat(gravity);
				packet.PutFloat(easing);
				packet.AddStringId(endEffect ?? "None");
				packet.PutFloat(endEffectScale);

				caster.Map.Broadcast(packet);
			}

			/// <summary>
			/// Fixes the rotation of an actor's billboard (2D sprite representation) to a specific angle.
			/// </summary>
			/// <param name="actor">The actor whose billboard to fix.</param>
			/// <param name="angle">The angle in radians to fix the billboard at.</param>
			/// <param name="enabled">True to fix the rotation, false to release it.</param>
			/// <remarks>Corresponds to script_list.xml ClassID 820: SetFixRotateBillboard(actor, rotateBillboardAngle)</remarks>
			public static void SetFixRotateBillboard(IActor actor, float angle, bool enabled = true)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetFixRotateBillboard);

				packet.PutInt(actor.Handle);
				packet.PutFloat(angle);
				packet.PutByte(enabled);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Attaches an item's visual model to a node on an actor.
			/// </summary>
			/// <param name="actor">The actor to attach the item model to.</param>
			/// <param name="xacHeadName">The XAC name, usually derived from the character's model.</param>
			/// <param name="handType">The slot/node to attach to (e.g., RH for right hand).</param>
			/// <param name="itemId">The ID of the item to display.</param>
			/// <param name="itemName">The class name of the item.</param>
			/// <param name="attach">True to attach the item, false to detach it.</param>
			/// <param name="angle">An optional rotation angle.</param>
			/// <param name="midOffset">An optional vertical offset.</param>
			/// <remarks>Corresponds to script_list.xml ClassID 628: ItemAttachToMonsterNode(self, xacHeadName, handType, itemID, itemName, isAttach, angle, midOffset)</remarks>
			public static void AttachItemToMonster(IActor actor, string xacHeadName, EquipSlot handType, int itemId, string itemName, bool attach, float angle = 0, float midOffset = 1)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AttachItemToMonster);

				packet.PutInt(actor.Handle);
				packet.PutLpString(xacHeadName);
				packet.PutInt((int)handType);
				packet.PutInt(itemId);
				packet.PutLpString(itemName);
				packet.PutByte(attach);
				packet.PutFloat(angle);
				packet.PutFloat(midOffset);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Update's character's look and broadcasts it.
			/// Used with hair costumes, pocket wigs, skintone changes
			/// in beauty shop.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="equipList"></param>
			/// <param name="pcInfoPartsNode2"></param>
			/// <param name="i1"></param>
			public static void UpdateCharacterLook(IActor actor, Dictionary<EquipSlot, int> equipList, int pcInfoPartsNode2 = 0, int i1 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.UpdateCharacterLook);

				packet.PutInt(actor.Handle);
				packet.PutByte(0);
				packet.PutShort(1);
				packet.PutInt(equipList.Count);

				foreach (var equip in equipList)
				{
					packet.PutInt(equip.Value);
					packet.PutInt((int)equip.Key);
				}
				if (pcInfoPartsNode2 != 0)
				{
					packet.PutByte(1);
					packet.PutInt(pcInfoPartsNode2);
				}
				else
				{
					packet.PutByte(0);
				}
				packet.PutInt(i1);

				actor.Map.Broadcast(packet);
			}

			public static void UpdateCharacterLook(IActor actor, int itemId, EquipSlot slot, int pcInfoPartsNode2 = 0, int i1 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.UpdateCharacterLook);

				packet.PutInt(actor.Handle);
				packet.PutByte(0);
				packet.PutShort(1);
				packet.PutInt(1);

				packet.PutInt(itemId);
				packet.PutInt((int)slot);
				if (pcInfoPartsNode2 != 0)
				{
					packet.PutByte(1);
					packet.PutInt(pcInfoPartsNode2);
				}
				else
				{
					packet.PutByte(0);
				}
				packet.PutInt(i1);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Sends character look update to a specific connection.
			/// Used for pocket wigs, hair costumes, etc.
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="actor"></param>
			/// <param name="itemId"></param>
			/// <param name="slot"></param>
			/// <param name="pcInfoPartsNode2"></param>
			/// <param name="i1"></param>
			public static void UpdateCharacterLook(IZoneConnection conn, IActor actor, int itemId, EquipSlot slot, int pcInfoPartsNode2 = 0, int i1 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.UpdateCharacterLook);

				packet.PutInt(actor.Handle);
				packet.PutByte(0);
				packet.PutShort(1);
				packet.PutInt(1);

				packet.PutInt(itemId);
				packet.PutInt((int)slot);
				if (pcInfoPartsNode2 != 0)
				{
					packet.PutByte(1);
					packet.PutInt(pcInfoPartsNode2);
				}
				else
				{
					packet.PutByte(0);
				}
				packet.PutInt(i1);

				conn.Send(packet);
			}

			/// <summary>
			/// Show's companion food in player's hand.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="slot"></param>
			/// <param name="animationId"></param>
			/// <param name="isVisible"></param>
			public static void PlayEquipItem(IActor actor, EquipSlot slot, string animationId, bool isVisible)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayEquipItem);

				packet.PutInt(actor.Handle);
				packet.PutByte((byte)slot);
				packet.PutLpString(animationId);
				packet.PutByte(isVisible);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Play an effect/animation at a position
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="effectHandle"></param>
			/// <param name="animationId">Refer to packetstrings db</param>
			/// <param name="position">Effect's position</param>
			/// <param name="scale">Effect's scale</param>
			/// <param name="duration">Effect's maximum duration, some animations end before this</param>
			public static void PlayEffectAtPosition(IZoneConnection conn, string effectName, Position position, float scale, int effectHandle, float duration)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.StartEffect);

				packet.AddStringId(effectName);
				packet.PutPosition(position);
				packet.PutFloat(scale);
				packet.PutInt(effectHandle);
				packet.PutFloat(duration);

				conn.Send(packet);
			}

			/// <summary>
			/// Play an effect/animation at a position
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="effectName">Refer to packetstrings db</param>
			/// <param name="position">Effect's position</param>
			/// <param name="scale">Effect's scale</param>
			/// <param name="effectHandle"></param>
			/// <param name="duration">Effect's maximum duration in ms, some animations end before this</param>
			public static void PlayEffectAtPosition(IActor actor, string effectName, Position position, float scale, int effectHandle, float duration)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.StartEffect);

				packet.AddStringId(effectName);
				packet.PutPosition(position);
				packet.PutFloat(scale);
				packet.PutInt(effectHandle);
				// Client receives it in seconds
				duration = duration / 1000;
				packet.PutFloat(duration);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Ends a specific effect instance played at a location.
			/// </summary>
			/// <param name="actor">The actor responsible for the effect.</param>
			/// <param name="effectHandle">The handle of the effect to end, received from PlayEffectAtPosition.</param>
			/// <remarks>
			/// Corresponds to script_list.xml ClassID 1152: EndEffect(self, effectID)
			/// </remarks>
			public static void EndEffect(IActor actor, int effectHandle)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.EndEffect);

				packet.PutInt(actor.Handle);
				packet.PutInt(effectHandle);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Attaches an actor to a specific node on another actor's model.
			/// </summary>
			/// <param name="actorToAttach">The actor to be attached.</param>
			/// <param name="target">The actor to attach to.</param>
			/// <param name="nodeName">The name of the node on the target's model (e.g., 'Bip01-Head').</param>
			/// <remarks>
			/// Corresponds to script_list.xml ClassID 681: AttachToNode(self, target, nodeName)
			/// </remarks>
			public static void AttachToNode(IActor actorToAttach, IActor target, string nodeName)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AttachToNode);

				packet.PutInt(actorToAttach.Handle);
				packet.PutInt(target.Handle);
				packet.PutLpString(nodeName);

				actorToAttach.Map.Broadcast(packet);
			}

			/// <summary>
			/// Attaches effect to actor on client.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="animationName"></param>
			/// <param name="scale"></param>
			/// <param name="heightOffset">0 - Top, 1 - Mid, 2 - Bot</param>
			/// <param name="effectRelativeX">Offsets the effect's X position relative to the actor</param>
			/// <param name="effectRelativeY">Offsets the effect's Y position relative to the actor</param>
			/// <param name="effectRelativeZ">Offsets the effect's Z position relative to the actor</param>
			public static void AttachEffect(IActor actor, string animationName, float scale, EffectLocation heightOffset = EffectLocation.Unknown, float effectRelativeX = 0, float effectRelativeY = 0, float effectRelativeZ = 0, byte b1 = 0, byte b2 = 0, byte b3 = 0, byte b4 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AttachEffect);

				packet.PutInt(actor.Handle);
				packet.AddStringId(animationName);
				packet.PutFloat(scale);
				packet.PutInt((int)heightOffset);
				packet.PutFloat(effectRelativeX);
				packet.PutFloat(effectRelativeY);
				packet.PutFloat(effectRelativeZ);
				packet.PutByte(b1);
				packet.PutByte(b2);
				packet.PutByte(b3);
				packet.PutByte(b4);
				packet.PutEmptyBin(12);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Attaches effect to actor on client.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="effectName"></param>
			/// <param name="scale"></param>
			/// <param name="heightOffset">0 - Top, 1 - Mid, 2 - Bot</param>
			/// <param name="effectRelativeX">Offsets the effect's X position relative to the actor</param>
			/// <param name="effectRelativeY">Offsets the effect's Y position relative to the actor</param>
			/// <param name="effectRelativeZ">Offsets the effect's Z position relative to the actor</param>
			public static void AttachEffect(IZoneConnection conn, IActor actor, string effectName, float scale, EffectLocation heightOffset = EffectLocation.Unknown, float effectRelativeX = 0, float effectRelativeY = 0, float effectRelativeZ = 0, byte b1 = 0, byte b2 = 0, byte b3 = 0, byte b4 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AttachEffect);

				packet.PutInt(actor.Handle);
				packet.AddStringId(effectName);
				packet.PutFloat(scale);
				packet.PutInt((int)heightOffset);
				packet.PutFloat(effectRelativeX);
				packet.PutFloat(effectRelativeY);
				packet.PutFloat(effectRelativeZ);
				packet.PutByte(b1);
				packet.PutByte(b2);
				packet.PutByte(b3);
				packet.PutByte(b4);

				conn.Send(packet);
			}

			/// <summary>
			/// Removes all effects from actor.
			/// </summary>
			/// <param name="actor"></param>
			public static void ClearEffects(IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ClearEffects);

				packet.PutInt(actor.Handle);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Skill effect
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="packetString"></param>
			/// <param name="time"></param>
			/// <param name="str1"></param>
			/// <param name="str2"></param>
			public static void PlayEffectNode(IActor actor, string packetString, float time, string str1, string str2)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayEffectNode);

				packet.PutInt(actor.Handle);
				packet.AddStringId(packetString);
				packet.PutFloat(time);
				packet.PutLpString(str1);
				packet.PutLpString(str2);
				packet.PutInt(0);
				packet.PutInt(4);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Plays an animation effect.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="b1"></param>
			/// <param name="heightOffset"></param>
			/// <param name="b2"></param>
			/// <param name="scale"></param>
			/// <param name="animationName"></param>
			public static void PlayEffect(IZoneConnection conn, IActor actor, byte b1 = 1,
				EffectLocation heightOffset = EffectLocation.Bottom, byte b2 = 1, float scale = 1,
				string animationName = "F_pc_class_change", float f1 = 0, int associatedHandle = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayEffect);

				packet.PutInt(actor.Handle);
				packet.PutByte(b1); // Prev: 0
				packet.PutInt((int)heightOffset);
				packet.PutByte(b2); // Prev: 0
				packet.PutFloat(scale); // Effect size Prev: 6
				packet.AddStringId(animationName);
				packet.PutFloat(f1);
				packet.PutInt(associatedHandle);

				conn.Send(packet);
			}

			/// <summary>
			/// Plays an animation effect.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="b1"></param>
			/// <param name="heightOffset"></param>
			/// <param name="b2"></param>
			/// <param name="scale"></param>
			/// <param name="animationName"></param>
			public static void PlayEffect(IActor actor, byte b1 = 1, EffectLocation heightOffset = EffectLocation.Bottom, byte b2 = 1, float scale = 1, string animationName = "F_pc_class_change", float f1 = 0, int associatedHandle = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayEffect);

				packet.PutInt(actor.Handle);
				packet.PutByte(b1);
				packet.PutInt((int)heightOffset);
				packet.PutByte(b2);
				packet.PutFloat(scale);
				packet.AddStringId(animationName);
				packet.PutFloat(f1);
				packet.PutInt(associatedHandle);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Plays given effect on actor.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="effectName"></param>
			/// <param name="scale"></param>
			public static void PlayEffect(IActor actor, string effectName, float scale = 1)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayEffect);

				packet.PutInt(actor.Handle);
				packet.PutByte(1);
				packet.PutInt(2);
				packet.PutByte(0);
				packet.PutFloat(scale);
				packet.AddStringId(effectName);
				packet.PutInt(0);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Controls a skill's visual effects.
			/// </summary>
			/// <param name="forceId"></param>
			/// <param name="caster"></param>
			/// <param name="source"></param>
			/// <param name="target"></param>
			/// <param name="effect"></param>
			/// <param name="scale"></param>
			/// <param name="soundEffect"></param>
			/// <param name="endEffect"></param>
			/// <param name="endEffectScale"></param>
			/// <param name="endSoundEffect"></param>
			/// <param name="effectSpeed"></param>
			/// <param name="speed"></param>
			/// <exception cref="ArgumentException">
			/// Thrown if any of the packet strings are not found.
			/// </exception>
			public static void PlayForceEffect(int forceId, IActor caster, IActor source, IActor target,
				string effect, float scale, string soundEffect, string endEffect, float endEffectScale, string endSoundEffect, string effectSpeed, float speed)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayForceEffect);

				packet.PutInt(forceId);

				packet.PutInt(caster.Handle);
				packet.PutInt(source.Handle);
				packet.PutInt(target.Handle);

				packet.AddStringId(effect);
				packet.PutFloat(scale);
				packet.AddStringId(soundEffect);
				packet.AddStringId(endEffect);
				packet.PutFloat(endEffectScale);
				packet.AddStringId(endSoundEffect);
				packet.AddStringId(effectSpeed);

				packet.PutFloat(speed);
				packet.PutFloat(0);
				packet.PutFloat(0);
				packet.PutFloat(0);
				packet.PutInt(0);
				packet.PutFloat(5);
				packet.PutFloat(5);
				packet.PutFloat(2);
				packet.PutInt(0);

				source.Map.Broadcast(packet, target);
			}

			/// <summary>
			/// Skill splash effects
			/// </summary>
			/// <param name="caster"></param>
			/// <param name="target1"></param>
			/// <param name="target2"></param>
			/// <param name="forceId"></param>
			/// <param name="effect"></param>
			/// <param name="scale"></param>
			/// <param name="soundEffect"></param>
			/// <param name="endEffect"></param>
			/// <param name="endEffectScale"></param>
			/// <param name="endSoundEffect"></param>
			/// <param name="effectSpeed"></param>
			/// <param name="speed"></param>
			/// <param name="easing"></param>
			/// <param name="collisionRange"></param>
			/// <param name="creationDistance"></param>
			/// <param name="radiusSpeed"></param>
			/// <exception cref="ArgumentException">
			/// Thrown if any of the packet strings are not found.
			/// </exception>
			public static void PlayForceEffect(IActor caster, IActor target1, IActor target2,
			int forceId, string effect, float scale, string soundEffect, string endEffect,
			float endEffectScale, string endSoundEffect, string effectSpeed, float speed,
			float easing = 0, float collisionRange = 0, float creationDistance = 0, float radiusSpeed = 0)
				=> PlayForceEffect(caster, target1, target2, forceId, effect, scale, soundEffect,
					endEffect, endEffectScale, endSoundEffect, effectSpeed, speed, easing,
					0, 0, 0, collisionRange, creationDistance, radiusSpeed);

			/// <summary>
			/// Skill splash effects
			/// </summary>
			/// <param name="caster"></param>
			/// <param name="target1"></param>
			/// <param name="target2"></param>
			/// <param name="effect"></param>
			/// <param name="scale"></param>
			/// <param name="soundEffect"></param>
			/// <param name="endEffect"></param>
			/// <param name="endEffectScale"></param>
			/// <param name="endSoundEffect"></param>
			/// <param name="effectSpeed"></param>
			/// <param name="speed"></param>
			/// <param name="easing"></param>
			/// <param name="collisionRange"></param>
			/// <param name="creationDistance"></param>
			/// <param name="radiusSpeed"></param>
			/// <exception cref="ArgumentException">
			/// Thrown if any of the packet strings are not found.
			/// </exception>
			public static void PlayForceEffect(IActor caster, IActor target1, IActor target2,
			int forceId, string effect, float scale, string soundEffect, string endEffect, float endEffectScale,
			string endSoundEffect, string effectSpeed, float speed, float easing = 0, float gravity = 0,
			float angle = 0, int hitIndex = 0, float collisionRange = 0, float creationDistance = 0, float radiusSpeed = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayForceEffect);

				packet.PutInt(forceId);
				packet.PutInt(caster.Handle);
				packet.PutInt(target1.Handle);
				packet.PutInt(target2?.Handle ?? 0);
				packet.AddStringId(effect);
				packet.PutFloat(scale);
				packet.AddStringId(soundEffect);
				packet.AddStringId(endEffect);
				packet.PutFloat(endEffectScale);
				packet.AddStringId(endSoundEffect);
				packet.AddStringId(effectSpeed);
				packet.PutFloat(speed);
				packet.PutFloat(easing);
				packet.PutFloat(gravity);
				packet.PutFloat(angle);
				packet.PutInt(hitIndex);
				packet.PutFloat(collisionRange);
				packet.PutFloat(creationDistance);
				packet.PutFloat(radiusSpeed);
				packet.PutInt(0);

				caster.Map.Broadcast(packet, caster);
			}

			/// <summary>
			/// Reflects a force effect from the caster to a new target.
			/// </summary>
			/// <param name="reflector">The actor reflecting the force.</param>
			/// <param name="originalCaster">The original caster of the force.</param>
			/// <param name="newTarget">The new target of the reflected force.</param>
			/// <param name="skillId">The ID of the skill associated with the force.</param>
			/// <param name="damage">The damage value to associate with the reflection.</param>
			/// <remarks>Corresponds to script_list.xml ClassID 1667: ForceReflect(self, skill, otherTarget, target, damage)</remarks>
			public static void PlayForceReflect(IActor reflector, IActor originalCaster, IActor newTarget, SkillId skillId, int damage)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayForceReflect);

				packet.PutInt(reflector.Handle);
				packet.PutInt(originalCaster.Handle);
				packet.PutInt(newTarget.Handle);
				packet.PutInt((int)skillId);
				packet.PutInt(damage);

				reflector.Map.Broadcast(packet);
			}

			/// <summary>
			/// Adds a persistent visual effect to an actor.
			/// </summary>
			/// <param name="actor">The actor to add the effect to.</param>
			/// <param name="effectName">The name of the effect.</param>
			/// <param name="scale">The scale of the effect.</param>
			/// <param name="skipIfExisting">If true, does not add the effect if it's already playing.</param>
			/// <param name="location">The attachment location on the actor's body.</param>
			/// <param name="applyActorScale">If true, the effect's scale is multiplied by the actor's scale.</param>
			/// <remarks>
			/// Corresponds to script_list.xml ClassID 676: AddEffect(self, EffctName, scale, skipIfExist, offset, applyScale)
			/// </remarks>
			public static void AddEffect(IActor actor, string effectName, float scale = 1, bool skipIfExisting = false, EffectLocation location = EffectLocation.Bottom, bool applyActorScale = false)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AddEffect);

				packet.PutInt(actor.Handle);
				packet.AddStringId(effectName);
				packet.PutFloat(scale);
				packet.PutByte(skipIfExisting);
				packet.PutInt((int)location);
				packet.PutByte(applyActorScale);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Removes a specific persistent visual effect from an actor by its name.
			/// </summary>
			/// <param name="actor">The actor to remove the effect from.</param>
			/// <param name="effectName">The name of the effect to remove.</param>
			/// <param name="forceDestroy">Whether to force the destruction of the effect.</param>
			/// <remarks>
			/// Corresponds to script_list.xml ClassID 677: RemoveEffect(self, EffctName, forceDestroy)
			/// </remarks>
			public static void RemoveEffectByName(IActor actor, string effectName, bool forceDestroy = false)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DetachEffect);

				packet.PutInt(actor.Handle);
				packet.PutByte(forceDestroy);
				packet.AddStringId(effectName);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Detach an effect from an actor.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="effectName"></param>
			/// <exception cref="ArgumentException"></exception>
			public static void DetachEffect(IActor actor, string effectName)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DetachEffect);

				packet.PutByte(1);
				packet.AddStringId(effectName);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Detach an effect from an actor.
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="actor"></param>
			/// <param name="effectName"></param>
			/// <exception cref="ArgumentException"></exception>
			public static void DetachEffect(IZoneConnection conn, IActor actor, string effectName)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DetachEffect);

				packet.PutByte(1);
				packet.AddStringId(effectName);

				conn.Send(packet);
			}

			/// <summary>
			/// Show combo effect
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="comboCount"></param>
			/// <param name="comboDuration"></param>
			/// <param name="i1"></param>
			public static void ShowComboEffect(IZoneConnection conn, int comboCount, float comboDuration, int hitCountToFever = 101)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ShowComboEffect);

				packet.PutInt(comboCount);
				packet.PutFloat(comboDuration);
				packet.PutInt(hitCountToFever);

				conn.Send(packet);
			}

			/// <summary>
			/// Appears to update information about a skill effect on the
			/// clients in range of entity.
			/// </summary>
			/// <remarks>
			/// Observed updating the origin position of the Earthquake
			/// effect. Once the packet was sent once, the dust cloud
			/// effect would always appear at the same location, even
			/// when the packet was no longer sent. Only if it was
			/// sent did the location update and the effect appeared
			/// in the right place.
			/// </remarks>
			/// <param name="entity"></param>
			/// <param name="targetHandle"></param>
			/// <param name="originPos"></param>
			/// <param name="direction"></param>
			/// <param name="farPos"></param>
			public static void UpdateSkillEffect(ICombatEntity entity, int targetHandle, Position originPos, Direction direction, Position farPos)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.UpdateSkillEffect);

				packet.PutInt(entity.Handle);
				if (Versions.Protocol > 500)
					packet.PutInt(0);
				else
					packet.PutByte(0);
				packet.PutInt(0);
				packet.PutInt(targetHandle);
				packet.PutPosition(originPos);
				packet.PutDirection(direction);
				packet.PutPosition(farPos);

				entity.Map.Broadcast(packet, entity);
			}

			/// <summary>
			/// Appears to update information about a skill effect on the
			/// clients in range of entity.
			/// </summary>
			/// <remarks>
			/// Observed updating the origin position of the Earthquake
			/// effect. Once the packet was sent once, the dust cloud
			/// effect would always appear at the same location, even
			/// when the packet was no longer sent. Only if it was
			/// sent did the location update and the effect appeared
			/// in the right place.
			/// </remarks>
			/// <param name="entity"></param>
			/// <param name="targetHandle"></param>
			/// <param name="originPos"></param>
			/// <param name="direction"></param>
			/// <param name="farPos"></param>
			public static void UpdateSkillEffect(ICombatEntity entity, int i1, int targetHandle, Position originPos, Direction direction, Position farPos)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.UpdateSkillEffect);

				packet.PutInt(entity.Handle);
				if (Versions.Protocol > 500)
					packet.PutInt(i1);
				else
					packet.PutByte((byte)i1);
				packet.PutInt(0);
				packet.PutInt(targetHandle);
				packet.PutPosition(originPos);
				packet.PutDirection(direction);
				packet.PutPosition(farPos);

				entity.Map.Broadcast(packet, entity);
			}

			/// <summary>
			/// Set an Actor's Color (Yellow, Magenta, Cyan, Alpha)
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="yellow"></param>
			/// <param name="magenta"></param>
			/// <param name="cyan"></param>
			/// <param name="alpha"></param>
			/// <param name="transitionDuration"></param>
			/// <param name="b6"></param>
			public static void SetActorColor(IActor actor,
				byte yellow, byte magenta, byte cyan,
				byte alpha, float transitionDuration, byte b6 = 1)
			{
				// color_R, color_G, color_B, colar_A, blendOption
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetActorColor);

				packet.PutInt(actor.Handle);
				packet.PutByte(yellow);
				packet.PutByte(magenta);
				packet.PutByte(cyan);
				packet.PutByte(alpha);
				packet.PutByte(1);
				packet.PutFloat(transitionDuration);
				packet.PutByte(b6);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Set Entity's Color (Yellow, Magenta, Cyan, Alpha)
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="actor"></param>
			/// <param name="yellow"></param>
			/// <param name="magenta"></param>
			/// <param name="cyan"></param>
			/// <param name="alpha"></param>
			/// <param name="transitionDuration"></param>
			/// <param name="b6"></param>
			public static void SetActorColor(IZoneConnection conn, IActor actor,
				byte yellow, byte magenta, byte cyan,
				byte alpha, float transitionDuration, byte b6)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetActorColor);

				packet.PutInt(actor.Handle);
				packet.PutByte(yellow);
				packet.PutByte(magenta);
				packet.PutByte(cyan);
				packet.PutByte(alpha);
				packet.PutByte(1);
				packet.PutFloat(transitionDuration);
				packet.PutByte(b6);

				conn.Send(packet);
			}

			/// <summary>
			/// Show or Hide the Actor Shadow
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="enabled">0 turns off shadow and any other value shows a shadow</param>
			public static void SetActorShadow(IActor actor, bool enabled)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetActorShadow);

				packet.PutInt(actor.Handle);
				packet.PutFloat(enabled ? 1 : 0);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Call a lua func
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="luaFuncPacketString"></param>
			/// <param name="i1"></param>
			/// <param name="i2"></param>
			/// <param name="b1"></param>
			/// <param name="b2"></param>
			/// <param name="f1"></param>
			public static void Skill_CallLuaFunc(IActor actor, string luaFuncPacketString, int i1, int i2, byte b1, byte b2, float f1)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_CallLuaFunc);

				packet.PutInt(actor.Handle);
				packet.AddStringId(luaFuncPacketString);
				packet.PutInt(i1);
				packet.PutInt(i2);
				packet.PutByte(b1);
				packet.PutByte(b2);
				packet.PutFloat(f1);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Skill Properties
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="b1"></param>
			/// <param name="skill"></param>
			/// <param name="propertyIds"></param>
			public static void SkillProperties(IZoneConnection conn, byte b1, Skill skill, params string[] propertyIds)
			{
				var properties = skill.Properties.GetSelect(propertyIds);
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetSkillProperties);

				packet.PutByte(b1);
				packet.PutInt((int)skill.Id);
				packet.PutShort(properties.GetByteCount());
				packet.AddProperties(properties);

				conn.Send(packet);
			}


			/// <summary>
			/// Set an actor's scale.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="i1"></param>
			/// <param name="scale"></param>
			/// <param name="transitionSpeed"></param>
			/// <param name="overwriteScale"></param>
			/// <param name="b2"></param>
			/// <param name="b3"></param>
			public static void SetScale(IActor actor, int i1, float scale, float transitionSpeed = 0, byte overwriteScale = 0, byte b2 = 0, byte b3 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetScale);

				packet.PutInt(actor.Handle);
				packet.PutInt(i1);
				packet.PutFloat(scale);
				packet.PutFloat(transitionSpeed);
				packet.PutByte(overwriteScale);
				packet.PutByte(b2);
				packet.PutByte(b3);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Set an actor's scale.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="animationName"></param>
			/// <param name="animationScale"></param>
			/// <param name="animationSpeed"></param>
			/// <param name="overwriteScale"></param>
			/// <param name="b2"></param>
			/// <param name="b3"></param>
			public static void SetScale(IActor actor, string animationName, float animationScale, float animationSpeed = 0, byte overwriteScale = 0, byte b2 = 0, byte b3 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetScale);

				packet.PutInt(actor.Handle);
				packet.AddStringId(animationName);
				packet.PutFloat(animationScale);
				packet.PutFloat(animationSpeed);
				packet.PutByte(overwriteScale);
				packet.PutByte(b2);
				packet.PutByte(b3);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Play discovery sound
			/// </summary>
			/// <param name="character"></param>
			public static void PlaySound(Character character, IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlaySound);

				packet.PutInt(actor.Handle);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Play an Arrow Effect
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="startingPosition"></param>
			/// <param name="endingPosition"></param>
			/// <param name="arrowEffectName"></param>
			/// <param name="arrowEffectScale"></param>
			/// <param name="arrowSpace"></param>
			/// <param name="arrowSpaceTime"></param>
			/// <param name="arrowLifeTime"></param>
			/// <param name="f4"></param>
			/// <param name="b1"></param>
			/// <exception cref="ArgumentException"></exception>
			public static void PlayArrowEffect(IActor actor, Position startingPosition, Position endingPosition,
				string arrowEffectName, float arrowEffectScale, float arrowSpace, float arrowSpaceTime,
				float arrowLifeTime, float f4 = 0, byte b1 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayArrowEffect);

				packet.PutPosition(startingPosition);
				packet.PutPosition(endingPosition);
				packet.AddStringId(arrowEffectName);
				packet.PutFloat(arrowEffectScale);
				packet.PutFloat(arrowSpace);
				packet.PutFloat(arrowSpaceTime);
				packet.PutFloat(arrowLifeTime);
				packet.PutFloat(f4);
				packet.PutInt(actor.Handle);
				packet.PutByte(b1);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Makes monster fade out over the given amount of time.
			/// </summary>
			/// <param name="monster"></param>
			/// <param name="duration"></param>
			public static void FadeOut(IMonster monster, TimeSpan duration)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.FadeOut);

				packet.PutInt(monster.Map.Id);
				packet.PutInt(monster.GenType);
				packet.PutFloat((float)duration.TotalSeconds);

				monster.Map.Broadcast(packet, monster, false);
			}

			/// <summary>
			/// Unknown effect currently works with I_SYS_heal2 as message and a number as parameter
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="message"></param>
			/// <param name="parameter"></param>
			public static void TextEffect(IActor actor, string message, string parameter)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.TextEffect);

				packet.PutInt(actor.Handle);
				packet.PutLpString(message);
				packet.PutLpString(parameter);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Unknown effect currently works with I_SYS_heal2 as message and a number as parameter
			/// </summary>
			/// <param name="character"></param>
			/// <param name="message"></param>
			/// <param name="parameter"></param>
			public static void TextEffect(Character character, string message, string parameter)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.TextEffect);

				packet.PutInt(character.Handle);
				packet.PutLpString(message);
				packet.PutLpString(parameter);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Sets a render option on an actor, such as "bigheadmode" or "Freeze".
			/// Broadcasts to all players on the map.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="optionName"></param>
			/// <param name="enabled"></param>
			public static void SetActorRenderOption(IActor actor, string optionName, bool enabled)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetActorRenderOption);

				packet.PutInt(actor.Handle);
				packet.PutLpString(optionName);
				packet.PutByte(enabled ? (byte)1 : (byte)0);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Updates the number of purchased character slots.
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="count"></param>
			public static void BarrackSlotCount(IZoneConnection conn, int count)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.BarrackSlotCount);

				packet.PutInt(count);

				conn.Send(packet);
			}

			/// <summary>
			/// Modify attack stance
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="skillId"></param>
			/// <param name="stanceName"></param>
			public static void SkillChangeAnimation(IActor actor, SkillId skillId, string stanceName = "None")
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillChangeAnimation);

				packet.PutInt(actor.Handle);
				packet.PutInt((int)skillId);
				packet.PutLpString(stanceName);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Unknown purpose yet. It could be a "target" packet. (this actor is targeting "id" actor
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="skill"></param>
			/// <param name="direction"></param>
			public static void SkillTargetAnimation(IActor actor, Skill skill, Direction direction, int i1,
				float f1 = 500, float f2 = 1, float f3 = 0, int i2 = 0, float f4 = 1, int i3 = 0, int i4 = 0, float f5 = 512, int i5 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillTargetAnimation);

				packet.PutInt(actor.Handle);
				packet.PutInt((int)skill.Id);
				packet.PutInt(actor.Handle);
				packet.PutDirection(actor.Direction);
				packet.PutInt(i1);
				packet.PutFloat(f1);
				packet.PutFloat(f2);
				packet.PutFloat(f3);
				packet.PutInt(i2);
				packet.PutFloat(f4);
				packet.PutInt(i3);
				packet.PutInt(i4);
				packet.PutFloat(f5);
				packet.PutInt(i5);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Attack broadcast?
			/// </summary>
			/// <param name="actor"></param>
			public static void SkillTargetAnimationEnd(IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillTargetAnimationEnd);

				packet.PutInt(actor.Handle);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Cancel attack, seen with pet companion on Archer/Hunter/Ranger
			/// </summary>
			/// <param name="actor"></param>
			public static void AttackCancelBow(IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AttackCancelBow);

				packet.PutInt(actor.Handle);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Usually after a skill has finished casting, might clean up animation?
			/// </summary>
			/// <remarks>
			/// i1 seems to be always 0
			/// </remarks>
			/// <param name="source"></param>
			/// <param name="i1"></param>
			public static void Skill_45(IActor source, int i1 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_45_Cloak);
				packet.PutInt(i1);

				source.Map.Broadcast(packet, source);
			}

			/// <summary>
			/// Skill cancel
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="skillId"></param>
			public static void SkillCancel(IActor actor, SkillId skillId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillCancel);

				packet.PutInt(actor.Handle);
				packet.PutInt((int)skillId);

				actor.Map.Broadcast(packet);
			}

			public static void MinimapMarker(IZoneConnection conn, IActor actor, byte b1, byte b2, byte b3)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MonsterMapMarker);

				packet.PutInt(actor.Handle);
				if (actor is IMonster monster)
					packet.PutInt(monster.Id);
				else
					packet.PutInt(1);
				packet.PutByte(b1);
				packet.PutByte(b2);
				packet.PutFloat(actor.Position.X);
				packet.PutFloat(actor.Position.Z);
				packet.PutByte(b3);

				conn.Send(packet);
			}

			public static void MinimapMarker(IMonster monster, byte b1, byte b2, byte b3)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MonsterMapMarker);

				packet.PutInt(monster.Handle);
				packet.PutInt(monster.Id);
				packet.PutByte(b1);
				packet.PutByte(b2);
				packet.PutFloat(monster.Position.X);
				packet.PutFloat(monster.Position.Z);
				packet.PutByte(b3);

				monster.Map.Broadcast(packet, monster);
			}

			public static void MinimapMarker(Character character, byte b1 = 2, byte b2 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.CharacterMapMarker);

				packet.PutInt(character.Handle);
				packet.PutShort((short)character.Job.Id);
				packet.PutByte(b1);
				packet.PutFloat(character.Position.X);
				packet.PutFloat(character.Position.Z);
				packet.PutByte(b2);

				character.Map.Broadcast(packet, character);
			}

			public static void RemoveMapMarker(IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.RemoveMapMarker);

				packet.PutInt(actor.Handle);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Sends account properties to character's client.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="propertyNames"></param>
			public static void AccountProperties(Character character, params string[] propertyNames)
			{
				var account = character.Connection.Account;
				var properties = propertyNames != null ? account.Properties.GetSelect(propertyNames) : account.Properties.GetAll();
				var propertySize = properties.GetByteCount();

				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AccountProperties);

				packet.PutLong(account.Id);
				packet.PutShort(propertySize);
				packet.AddProperties(properties);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Starts an effect associated with dynamic casted skill.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="skillId"></param>
			public static void Skill_DynamicCastStart(IActor actor, SkillId skillId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_DynamicCastStart);

				packet.PutInt(actor.Handle);
				packet.PutInt((int)skillId);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Ends an effect associated with dynamic casted skill.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="skillId"></param>
			/// <param name="value"></param>
			public static void Skill_DynamicCastEnd(IActor actor, SkillId skillId, float value)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_DynamicCastEnd);

				packet.PutInt(actor.Handle);
				packet.PutInt((int)skillId);
				packet.PutFloat(value);
				packet.PutByte(0);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// NPC Play a Track (Client side direction)
			/// </summary>
			public static void NPC_PlayTrack(IActor actor, string trackName, int i1, int i2, float f1)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.NPC_PlayTrack);
				packet.PutInt(actor.Handle);
				packet.PutLpString(trackName);
				packet.PutInt(i1);
				packet.PutInt(i2);
				if (Versions.Protocol > 500)
					packet.PutFloat(f1);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// NPC Play a Track (Client side direction)
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="actor"></param>
			/// <param name="trackName"></param>
			/// <param name="i1"></param>
			/// <param name="i2"></param>
			/// <param name="f1"></param>
			public static void NPC_PlayTrack(IZoneConnection conn, IActor actor, string trackName, int i1, int i2, float f1)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.NPC_PlayTrack);

				packet.PutInt(actor.Handle);
				packet.PutLpString(trackName);
				packet.PutInt(i1);
				packet.PutInt(i2);
				packet.PutFloat(f1);

				conn.Send(packet);
			}

			/// <summary>
			/// NPC Position For a script?
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="actor"></param>
			public static void SetNPCTrackPosition(IZoneConnection conn, IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetNPCTrackPosition);

				packet.PutInt(actor.Handle);
				packet.PutPosition(actor.Position);

				conn.Send(packet);
			}

			/// <summary>
			/// Setup a minigame
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="miniGameName"></param>
			/// <param name="b1"></param>
			/// <param name="f1"></param>
			/// <param name="l1"></param>
			/// <param name="parameter"></param>
			public static void MiniGame(IZoneConnection conn, string miniGameName, byte b1, float f1, long l1, string parameter = "")
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MiniGame);

				packet.PutLpString(miniGameName);
				packet.PutByte(b1);
				packet.PutFloat(f1);
				packet.PutEmptyBin(10);
				packet.PutLong(1);
				packet.PutEmptyBin(5);
				packet.PutLpString(parameter);

				conn.Send(packet);
			}

			/// <summary>
			/// Instance Dungeon Addon Message?
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="msg"></param>
			/// <param name="b1"></param>
			/// <param name="b2"></param>
			public static void IndunAddonMsg(IZoneConnection conn, string msg, byte b1 = 0, byte b2 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.IndunAddonMsg);

				packet.PutLpString(msg);
				packet.PutByte(b1);
				packet.PutByte(b2);

				conn.Send(packet);
			}

			/// <summary>
			/// Send Indun Addon Msg Param
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="i1"></param>
			/// <param name="packetString"></param>
			/// <param name="i2"></param>
			/// <exception cref="ArgumentException"></exception>
			public static void IndunAddonMsgParam(IZoneConnection conn, int i1, string packetString, int i2)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.IndunAddonMsgParam);

				packet.PutInt(i1);
				packet.AddStringId(packetString);
				packet.PutInt(i2);

				conn.Send(packet);
			}

			/// <summary>
			/// Client cutscene to play
			/// </summary>
			/// <param name="character"></param>
			/// <param name="cutSceneType"></param>
			/// <param name="b"></param>
			/// <param name="trackOrCharacter"></param>
			public static void LoadCutscene(Character character, int cutSceneType, bool b, string trackOrCharacter)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.LoadCutscene);

				packet.PutInt(cutSceneType);
				packet.PutByte(b);
				packet.PutLpString(trackOrCharacter);

				character.Connection.Send(packet);
			}

			/// <Summary>
			/// Used to show complex visual effects related to skills, called Pads.
			/// </summary>
			/// <param name="caster"></param>
			/// <param name="pad"></param>
			/// <param name="animationName"></param>
			/// <param name="f1"></param>
			/// <param name="f2"></param>
			/// <param name="f3"></param>
			/// <param name="isVisible"></param>
			public static void PadUpdate(ICombatEntity caster, Pad pad, string animationName, float f1, float f2, float f3, bool isVisible)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadUpdate);

				packet.PutInt(caster.Handle);
				packet.AddStringId(animationName);
				packet.PutInt((int)pad.Skill.Id);
				packet.PutInt(pad.Skill.Level);
				packet.PutPosition(pad.Position);
				packet.PutDirection(pad.Direction);
				packet.PutFloat(f1);
				packet.PutFloat(f2);
				packet.PutInt(pad.Handle);
				packet.PutInt(isVisible ? 1 : 0); // Possibly a bool with a 3 byte gap
				packet.PutEmptyBin(13);
				packet.PutFloat(f3);
				packet.PutEmptyBin(16);

				if (isVisible)
				{
					// Creating: broadcast to players near PAD position and track them
					foreach (var character in pad.Map.GetCharacters(c =>
						c.Position.InRange2D(pad.Position, Map.VisibleRange) &&
						c.Layer == pad.Layer))
					{
						pad.Observers.AddObserver(character);
						character.Connection?.Send(packet);
					}
				}
				else
				{
					// Destroying: send to ALL tracked observers
					foreach (var character in pad.Observers.GetObservers())
					{
						character.Connection?.Send(packet);
					}
					pad.Observers.Clear();
				}
			}

			/// <Summary>
			/// Used to show complex visual effects related to skills, called Pads.
			/// </summary>
			/// <param name="caster"></param>
			/// <param name="pad"></param>
			/// <param name="isVisible"></param>
			public static void PadUpdate(ICombatEntity caster, Pad pad, bool isVisible)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadUpdate);

				packet.PutInt(caster.Handle);
				packet.AddStringId(pad.Name);
				packet.PutInt((int)pad.Skill.Id);
				packet.PutInt(pad.Skill.Level);
				packet.PutPosition(pad.Position);
				packet.PutDirection(pad.Direction);
				packet.PutFloat(pad.NumArg1);
				packet.PutFloat(pad.NumArg2);
				packet.PutInt(pad.Handle);
				packet.PutInt(isVisible ? 1 : 0); // Possibly a bool with a 3 byte gap
				packet.PutEmptyBin(13);
				packet.PutFloat(pad.NumArg3);
				packet.PutEmptyBin(16);

				if (isVisible)
				{
					// Creating: broadcast to players near PAD position and track them
					foreach (var character in pad.Map.GetCharacters(c =>
						c.Position.InRange2D(pad.Position, Map.VisibleRange) &&
						c.Layer == pad.Layer))
					{
						pad.Observers.AddObserver(character);
						character.Connection?.Send(packet);
					}
				}
				else
				{
					// Destroying: send to ALL tracked observers
					foreach (var character in pad.Observers.GetObservers())
					{
						character.Connection?.Send(packet);
					}
					pad.Observers.Clear();
				}
			}

			/// <summary>
			/// Sends pad update to a specific character.
			/// Used when a character walks into visibility range of an existing pad.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="caster"></param>
			/// <param name="pad"></param>
			/// <param name="isVisible"></param>
			public static void PadUpdateToCharacter(Character character, ICombatEntity caster, Pad pad, bool isVisible)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadUpdate);

				packet.PutInt(caster.Handle);
				packet.AddStringId(pad.Name);
				packet.PutInt((int)pad.Skill.Id);
				packet.PutInt(pad.Skill.Level);
				packet.PutPosition(pad.Position);
				packet.PutDirection(pad.Direction);
				packet.PutFloat(pad.NumArg1);
				packet.PutFloat(pad.NumArg2);
				packet.PutInt(pad.Handle);
				packet.PutInt(isVisible ? 1 : 0);
				packet.PutEmptyBin(13);
				packet.PutFloat(pad.NumArg3);
				packet.PutEmptyBin(16);

				character.Connection?.Send(packet);
			}

			/// <summary>
			/// Creates a skill in client (MONSKL_CRE_PAD)
			/// </summary>
			/// <param name="caster"></param>
			/// <param name="skill"></param>
			/// <param name="padName"></param>
			/// <param name="position"></param>
			/// <param name="direction"></param>
			/// <param name="f1"></param>
			/// <param name="f2"></param>
			/// <param name="padHandle"></param>
			/// <param name="f3"></param>
			[Obsolete("Use PadUpdate instead.")]
			public static void RunPad(ICombatEntity caster, Skill skill, string padName,
				Position position, Direction direction, float posAngle, float posDist, int padHandle,
				float f3, bool isVisible = true)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillRunScript);

				packet.PutInt(caster.Handle);
				packet.AddStringId(padName);
				packet.PutInt((int)skill.Id);
				packet.PutInt(skill.Level); // Skill Level ?
				packet.PutPosition(position);
				packet.PutDirection(direction);
				packet.PutFloat(posAngle);
				packet.PutFloat(posDist);
				packet.PutInt(padHandle);
				packet.PutInt(!isVisible ? 1 : 0);
				packet.PutEmptyBin(13); // Unknown Bytes
				packet.PutFloat(f3);
				packet.PutEmptyBin(16); // Unknown Bytes

				caster.Map.Broadcast(packet, caster);
			}

			public static void PadLinkEffect(Pad pad, Position startPos, Position endPos, string effectId, int monsterId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadLinkEffect);

				packet.PutInt(pad.Handle);
				packet.PutPosition(startPos);
				packet.PutPosition(endPos);
				packet.AddStringId(effectId);
				packet.PutInt(monsterId);

				pad.Map.Broadcast(packet);
			}

			/// <summary>
			/// Sets the altitude of an actor associated with a pad.
			/// </summary>
			/// <remarks>
			/// Used in skills like Shield Lob to make the shield hover in the air.
			/// </remarks>
			/// <param name="pad"></param>
			/// <param name="actor"></param>
			/// <param name="altitude"></param>
			public static void PadSetMonsterAltitude(Pad pad, IActor actor, float altitude)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadSetMonsterAltitude);

				packet.PutInt(pad.Handle);
				packet.PutInt(actor.Handle);
				packet.PutFloat(altitude);
				packet.PutByte(1);

				pad.Map.Broadcast(packet);
			}

			/// <summary>
			/// Sets the altitude of an actor associated with a pad.
			/// </summary>
			/// <remarks>
			/// Used in skills like Shield Lob to make the shield hover in the air.
			/// </remarks>
			/// <param name="pad"></param>
			/// <param name="actor"></param>
			/// <param name="altitude"></param>
			public static void PadSetMonsterAltitude(IZoneConnection conn, Pad pad, IActor actor, float altitude)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadSetMonsterAltitude);

				packet.PutInt(pad.Handle);
				packet.PutInt(actor.Handle);
				packet.PutFloat(altitude);
				packet.PutByte(1);

				conn.Send(packet);
			}

			/// <summary>
			/// Create's a client side obstacle that is impassable.
			/// </summary>
			/// <param name="pad"></param>
			/// <param name="sizeX"></param>
			/// <param name="sizeY"></param>
			public static void PadCreateObstacle(Pad pad, float sizeX, float sizeY)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadCreateObstacle);

				packet.PutInt(pad.Handle);
				packet.PutFloat(sizeX);
				packet.PutFloat(sizeY);

				pad.Map.Broadcast(packet);
			}

			/// <summary>
			/// Adds a visual or gameplay effect to the specified pad with the given effect name and scale.
			/// </summary>
			/// <param name="pad">The pad to which the effect will be applied. Cannot be <c>null</c>.</param>
			/// <param name="effectName">The name of the effect to add. Must not be <c>null</c> or empty.</param>
			/// <param name="effectScale">The scale factor to apply to the effect. Typically, values greater than 1.0 increase the effect size, while
			/// values less than 1.0 decrease it.</param>
			public static void PadAddEffect(Pad pad, string effectName, float effectScale)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadAddEffect);

				packet.PutInt(pad.Handle);
				packet.AddStringId(effectName);
				packet.PutFloat(effectScale);

				pad.Map.Broadcast(packet);
			}


			/// <summary>
			/// Removes a specified effect from the given pad.
			/// </summary>
			/// <remarks>This method broadcasts the removal of the effect to all clients on the pad's map.</remarks>
			/// <param name="pad">The pad from which to remove the effect. Cannot be <c>null</c>.</param>
			/// <param name="effectName">The name of the effect to remove. Cannot be <c>null</c> or empty.</param>
			public static void PadRemoveEffect(Pad pad, string effectName)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadRemoveEffect);

				packet.PutInt(pad.Handle);
				packet.AddStringId(effectName);

				pad.Map.Broadcast(packet);
			}

			/// <summary>
			/// Creates a particle effect (or set desired animation)
			/// </summary>
			/// <param name="character"></param>
			/// <param name="actorId"></param>
			/// <param name="enable"></param>
			public static void ParticleEffect(Character character, int actorId, int enable)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ParticleEffect);

				packet.PutInt(actorId);
				packet.PutInt(enable);

				character.Map.Broadcast(packet);
			}

			/// <summary>
			/// Moves pad to the position on clients around it.
			/// </summary>
			/// <param name="pad"></param>
			/// <param name="dest"></param>
			/// <param name="movementSpeed"></param>
			public static void PadMoveTo(Pad pad, Position dest, float movementSpeed)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadMoveTo);

				packet.PutInt(pad.Handle);
				packet.PutPosition(dest);
				packet.PutByte(1);
				packet.PutFloat(movementSpeed);
				packet.PutFloat(1);

				pad.Map.Broadcast(packet, pad);
			}

			/// <summary>
			/// It seems to start an animation for a given effectId.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="effectId"></param>
			public static void PlayAnimationOnEffect_6D(IActor actor, int effectId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_6D);

				packet.PutInt(effectId);
				packet.PutByte(1);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Changes character behavior for a cutscene.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="active">Whether the cutscene is active.</param>
			/// <param name="movable">Whether the client can still move the character. If not, the server can control it.</param>
			/// <param name="hideUi">Whether to hide the UI while active.</param>
			public static void SetupCutscene(Character character, bool active, bool movable, bool hideUi)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetupCutscene);

				packet.PutByte(active);
				packet.PutByte(movable);
				packet.PutByte(hideUi);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Resets the standard animation for the specified actor.
			/// </summary>
			/// <remarks>This method broadcasts a packet to reset the standard animation of the actor within its
			/// map.</remarks>
			/// <param name="actor">The actor whose standard animation is to be reset. Cannot be null.</param>
			public static void ResetStdAnim(IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ResetStdAnim);

				packet.PutInt(actor.Handle);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Resets the running animation for the specified actor.
			/// </summary>
			/// <remarks>This method sends a packet to broadcast the reset animation command to all clients in the
			/// actor's map.</remarks>
			/// <param name="actor">The actor whose running animation is to be reset. Cannot be null.</param>
			public static void ResetRunAnim(IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ResetRunAnim);

				packet.PutInt(actor.Handle);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Client cutscene to play
			/// </summary>
			/// <param name="character"></param>
			/// <param name="trackName"></param>
			/// <param name="actors"></param>
			public static void StartCutscene(Character character, string trackName, params IActor[] actors)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.CutsceneTrack);

				packet.PutLpString(trackName);
				packet.PutLong(1);
				packet.PutInt(actors.Length + 2);
				packet.PutInt(0);
				packet.PutInt(0);
				for (var i = 0; i < actors.Length; i++)
					packet.PutInt(actors[i].Handle);
				packet.PutInt(1);
				packet.PutInt(character.Handle);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Client cutscene to play
			/// </summary>
			/// <param name="character"></param>
			/// <param name="trackName"></param>
			/// <param name="actors"></param>
			public static void StartCutscene(Character character, string trackName, params int[] actors)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.CutsceneTrack);

				packet.PutLpString(trackName);
				packet.PutLong(1);
				packet.PutInt(actors.Length);
				packet.PutInt(0);
				packet.PutInt(0);
				//packet.PutInt(character.Handle);
				for (var i = 0; i < actors.Length; i++)
					packet.PutInt(actors[i]);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Set's the track frame to advance to.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="frame"></param>
			public static void SetTrackFrame(Character character, int frame)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetTrackFrame);

				packet.PutInt(frame);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Hide a NPC until track starts
			/// </summary>
			/// <param name="character"></param>
			/// <param name="actor"></param>
			public static void DelayEnterWorld(Character character, IActor actor, float f1 = 0)
				=> DelayEnterWorld(character.Connection, actor, f1);

			/// <summary>
			/// Hide a NPC until track starts
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="actor"></param>
			public static void DelayEnterWorld(IZoneConnection conn, IActor actor, float f1)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DelayEnterWorld);

				packet.PutInt(actor.Handle);
				packet.PutFloat(f1);

				conn.Send(packet);
			}

			/// <summary>
			/// Hide a NPC until track starts
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="f1">Could be transition duration</param>
			public static void DelayEnterWorld(IActor actor, float f1 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DelayEnterWorld);

				packet.PutInt(actor.Handle);
				packet.PutFloat(f1);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Adjusts the hit delay for a skill.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="skillId"></param>
			public static void SetHitDelay(Character character, int skillId, float value)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetHitDelay);

				packet.PutInt(skillId);
				packet.PutFloat(value);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Adjusts the skill speed for a skill.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="skillId"></param>
			/// <param name="value"></param>
			public static void SetSkillSpeed(Character character, int skillId, float value)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetSkillSpeed);

				packet.PutInt(skillId);
				packet.PutFloat(value);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Adjusts the hit delay for a skill.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="skillId"></param>
			public static void SetSkillUseOverHeat(Character character, int skillId, float value)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetSkillUseOverHeat);

				packet.PutInt(skillId);
				packet.PutFloat(value);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Reset skill ?
			/// SKL_CANCEL_CANCEL
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="skillId"></param>
			public static void SkillCancelCancel(IActor actor, SkillId skillId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillCancelCancel);

				packet.PutInt(actor.Handle);
				packet.PutInt((int)skillId);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Sent with Ice Wall
			/// </summary>
			/// <remarks>
			/// Originally using Character sent to a single connection
			/// now broadcasted with an IActor.
			/// </remarks>
			/// <param name="actor"></param>
			/// <param name="skillHandle"></param>
			/// <param name="f1"></param>
			public static void Skill_7F(IActor actor, int skillHandle, float f1)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_7F);

				packet.PutInt(skillHandle);
				packet.PutFloat(f1);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Sent with Falconer Circling
			/// </summary>
			/// <param name="pad"></param>
			/// <param name="packetString"></param>
			/// <param name="f1"></param>
			public static void ChangeGroundEffect(Pad pad, string packetString, float f1)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ChangeGroundEffect);

				packet.PutInt(pad.Handle);
				packet.AddStringId(packetString);
				packet.PutFloat(f1);

				pad.Map.Broadcast(packet, pad);
			}

			/// <summary>
			/// Set the main attack skill (access by Z key).
			/// </summary>
			/// <param name="entity"></param>
			/// <param name="skillId"></param>
			public static void SetMainAttackSkill(Character entity, SkillId skillId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetMainAttackSkill);

				packet.PutInt(entity.Handle);
				packet.PutInt((int)skillId);

				entity.Connection.Send(packet);
			}

			/// <summary>
			/// Used for toggling a skill?
			/// </summary>
			/// <param name="character"></param>
			/// <param name="skillId"></param>
			public static void SkillToggle(Character character, SkillId skillId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillToggle);

				packet.PutInt(character.Handle);
				packet.PutInt((int)skillId);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Spin's an object (actor)
			/// </summary>
			/// <param name="actor"></param>
			public static void SpinThrow(IActor actor, float rotDelay, float accelTime, int rotCount, float rotSec, ICombatEntity target,
				Position throwPos, string throwKey, string monName, string nodeName, string dragEft, float dragEftScale, float sR)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SpinThrow);

				packet.PutInt(actor.Handle);
				packet.PutInt(target.Handle);
				//packet.PutFloat(rotDelay);
				packet.PutFloat(accelTime);
				packet.PutInt(rotCount);
				packet.PutFloat(rotSec);
				packet.PutPosition(throwPos);
				packet.AddStringId(monName);
				packet.AddStringId(nodeName);
				packet.AddStringId(dragEft);
				packet.PutFloat(dragEftScale);
				packet.PutFloat(sR);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Spin's an object (actor)
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="spinDelay"></param>
			/// <param name="spinCount"></param>
			/// <param name="rotationPerSecond"></param>
			/// <param name="velocityChangeTerm"></param>
			public static void SpinObject(IActor actor, float spinDelay = 0, int spinCount = -1, float rotationPerSecond = 0.2f, float velocityChangeTerm = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SpinObject);

				packet.PutInt(actor.Handle);
				packet.PutFloat(spinDelay);
				packet.PutInt(spinCount);
				packet.PutFloat(rotationPerSecond);
				packet.PutFloat(velocityChangeTerm);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Spin's an object (actor)
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="spinDelay"></param>
			/// <param name="spinCount"></param>
			/// <param name="rotationPerSecond"></param>
			/// <param name="velocityChangeTerm"></param>
			public static void SpinObject(IZoneConnection conn, IActor actor, float spinDelay = 0, int spinCount = -1, float rotationPerSecond = 0.2f, float velocityChangeTerm = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SpinObject);

				packet.PutInt(actor.Handle);
				packet.PutFloat(spinDelay);
				packet.PutInt(spinCount);
				packet.PutFloat(rotationPerSecond);
				packet.PutFloat(velocityChangeTerm);

				conn.Send(packet);
			}

			public static void CollToGround(IActor actor, Position position, float speed, float easing, int skillKey, string anim, float animSec)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillCollisionToGround);

				packet.PutInt(actor.Handle);
				packet.PutPosition(position);
				packet.PutFloat(speed);
				packet.PutFloat(easing);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Causes the caster to move towards a target, collide, and then potentially return to their original position.
			/// Frequently used for pet behaviors, such as the Hawk (pet_hawk) diving at an enemy.
			/// </summary>
			/// <param name="caster">The actor performing the movement (e.g., the pet).</param>
			/// <param name="target">The target entity to collide with.</param>
			/// <param name="syncKey">Synchronization key for client-server position tracking.</param>
			/// <param name="animation">The Animation ID/String to play during the action.</param>
			/// <param name="goTime">Duration (or speed factor) of the approach to the target.</param>
			/// <param name="goEasing">Easing function/value for the approach (e.g., acceleration).</param>
			/// <param name="backTime">Duration (or speed factor) of the return trip.</param>
			/// <param name="backEasing">Easing function/value for the return (e.g., deceleration).</param>
			/// <param name="collisionOffset">Distance to maintain from the target upon impact.</param>
			/// <param name="returnToOriginalPosition">If true, the actor moves back to where it started after collision.</param>
			public static void CollisionAndBack(
				IActor caster,
				IActor target,
				int syncKey,
				string animation,
				float goTime,
				float goEasing,
				float backTime,
				float backEasing,
				float collisionOffset,
				bool returnToOriginalPosition)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.CollisionAndBack);

				packet.PutInt(caster.Handle);
				packet.PutInt(target.Handle);
				packet.PutInt(syncKey);
				packet.AddStringId(animation);
				packet.PutFloat(goTime);
				packet.PutFloat(goEasing);
				packet.PutFloat(backTime);
				packet.PutFloat(backEasing);
				packet.PutFloat(collisionOffset);
				packet.PutByte(returnToOriginalPosition);

				caster.Map.Broadcast(packet);
			}

			public static void PenetratePosition(IActor actor, Position position, float pentrateHeight, int syncKey, string animation, float goTime, float goEasing, float backTime, float backEasing, float collisionOffset)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PenetratePosition);

				packet.PutInt(actor.Handle);
				packet.PutPosition(position);
				packet.PutFloat(pentrateHeight);
				packet.PutInt(syncKey);
				packet.AddStringId(animation);
				packet.PutFloat(goTime);
				packet.PutFloat(goEasing);
				packet.PutFloat(backTime);
				packet.PutFloat(backEasing);
				packet.PutFloat(collisionOffset);

				actor.Map.Broadcast(packet);
			}

			public static void ThrowAttachedMonster(IActor actor, ICombatEntity caster, string monName, Position throwPos, float throwSpd, float hideTime)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ThrowAttachedMonster);

				packet.PutInt(actor.Handle);
				packet.AddStringId(monName);
				packet.PutPosition(throwPos);
				packet.PutFloat(throwSpd);
				packet.PutFloat(hideTime);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Stop animation
			/// </summary>
			/// <param name="actor"></param>
			public static void StopAnimation(IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.StopAnimation);

				packet.PutInt(actor.Handle);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Pet play animation/state?
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="companion"></param>
			public static void PetPlayAnimation(IZoneConnection conn, Companion companion, int animationId = 8576, int i1 = 1, byte b1 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PetPlayAnimation);

				packet.PutInt(companion.Handle);
				packet.PutInt(animationId);
				packet.PutInt(i1);
				packet.PutByte(b1);

				conn.Send(packet);
			}

			public static void PlayConnectEffect(IActor actor, Position position, string effectName, float effectScale)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayConnectEffect);

				packet.PutInt(actor.Handle);
				packet.PutPosition(position);
				packet.AddStringId(effectName);
				packet.PutFloat(effectScale);
				packet.PutFloat(1f);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Show UI Effect
			/// </summary>
			/// <param name="character"></param>
			/// <param name="item"></param>
			/// <param name="style"></param>
			/// <param name="type"></param>
			public static void ShowItemBalloon(Character character, Item item = null, string type = "reward_itembox", string style = "{@st43}", string systemMessage = "AppraisalSuccess", float duration = 3)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ShowItemBalloon);

				packet.PutByte(1);
				packet.PutInt(character.Handle);
				packet.PutByte((byte)(item != null ? 0 : 1));
				if (item != null)
				{
					var properties = item.Properties.GetAll();
					packet.AddStringId(style);
					packet.AddMessageId(systemMessage);
					// Most likely LpString/MsgParams
					packet.PutShort(1);
					packet.PutByte(0);
					packet.PutFloat(duration);
					packet.PutFloat(0);
					packet.PutLpString(type);
					packet.PutInt(item.Amount);
					packet.PutInt(item.Id);
					packet.PutShort(properties.GetByteCount());
					packet.AddProperties(properties);
				}

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Play Item Get Animation
			/// </summary>
			/// <param name="character"></param>
			/// <param name="anim"></param>
			/// <param name="item"></param>
			/// <param name="nodeName"></param>
			public static void PlayItemGetAnim(Character character, string anim, Item? item, string nodeName = "None")
			{
				var properties = item.Properties.GetAll();
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayItemGetAnim);

				packet.PutByte(1);
				packet.PutInt(character.Handle);
				packet.PutByte((byte)(item != null ? 0 : 1));
				if (item != null)
				{
					packet.AddStringId(anim);
					packet.PutInt(item.Amount);
					packet.PutInt(item.Id);
					packet.PutShort(properties.GetByteCount());
					packet.AddProperties(properties);
				}
				packet.AddStringId(nodeName);

				character.Connection.Send(packet);
			}


			/// <summary>
			/// Opens book for the player.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="bookName"></param>
			public static void OpenBook(Character character, string bookName)
				=> ShowBook(character, bookName);

			/// <summary>
			/// Show book with text
			/// </summary>
			/// <param name="character"></param>
			public static void ShowBook(Character character, string text)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ShowBook);
				packet.PutInt(character.Handle);
				packet.PutLpString(text);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Show scroll background with text
			/// </summary>
			/// <param name="character"></param>
			public static void ShowScroll(Character character, string text)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ShowScroll);
				packet.PutInt(character.Handle);
				packet.PutLpString(text);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Jump to position
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="moveTime"></param>
			/// <param name="jumpPower"></param>
			public static void JumpToPosition(IActor actor, float moveTime, float jumpPower)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.JumpToPosition);
				packet.PutInt(actor.Handle);
				packet.PutPosition(actor.Position);
				packet.PutFloat(moveTime);
				packet.PutFloat(jumpPower);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Sends associated pets for a character.
			/// </summary>
			/// <param name="character"></param>
			public static void PetInfo(Character character)
			{
				var companions = character.Companions.GetList();

				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PetInfo);
				packet.PutInt(4); // 3 or 4
				packet.PutInt(companions.Count);
				foreach (var companion in companions)
					packet.AddCompanion(companion);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Pet Associate World Id and Handle
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="companion"></param>
			public static void Pet_AssociateHandleWorldId(IZoneConnection conn, Companion companion)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Pet_AssociateHandleWorldId);
				packet.PutInt(companion.Handle);
				packet.PutLong(companion.ObjectId);
				packet.PutByte(1);

				conn.Send(packet);
			}

			/// <summary>
			/// Pet Associate World Id and Handle
			/// </summary>
			/// <param name="character"></param>
			/// <param name="companion"></param>
			public static void Pet_AssociateHandleWorldId(Character character, Companion companion)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Pet_AssociateHandleWorldId);
				packet.PutInt(companion.Handle);
				packet.PutLong(companion.ObjectId);
				packet.PutByte(1);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Pet Unknown?
			/// </summary>
			/// <param name="character"></param>
			/// <param name="companion"></param>
			public static void PetExpUpdate(Character character, Companion companion)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PetExpUp);
				packet.PutLong(companion.ObjectId);
				packet.PutLong(companion.TotalExp);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Sends a command to clients to visually create a link between multiple actors.
			/// </summary>
			/// <param name="caster">The entity who created the link.</param>
			/// <param name="linkerId">A unique ID for this specific link instance.</param>
			/// <param name="linkTexture">The texture/style of the link cable.</param>
			/// <param name="linkedHandles">A list of actor handles to be linked together.</param>
			/// <param name="linkSecond">Visual effect duration.</param>
			/// <param name="linkEffect">Visual effect on link creation.</param>
			/// <param name="linkEffectScale">Scale of the creation effect.</param>
			/// <param name="linkSound">Sound effect on link creation.</param>
			public static void MakeLinker(IZoneConnection conn, IActor caster, int linkerId, string linkTexture, bool unkBool,
				List<int> linkedHandles, float linkSecond, string linkEffect, float linkEffectScale, string linkSound)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AddLinkEffect);

				packet.PutInt(caster.Handle);
				packet.PutInt(linkerId);
				packet.AddStringId(linkTexture);
				packet.PutByte(unkBool); // Unknown boolean flag
				packet.PutInt(linkedHandles.Count);
				foreach (var handle in linkedHandles)
				{
					packet.PutInt(handle);
				}
				packet.PutFloat(linkSecond);
				packet.AddStringId(linkEffect);
				packet.PutFloat(linkEffectScale);
				packet.AddStringId(linkSound);

				conn.Send(packet);
			}

			/// <summary>
			/// Sends a command to clients to visually create a link between multiple actors.
			/// </summary>
			/// <param name="caster">The entity who created the link.</param>
			/// <param name="linkerId">A unique ID for this specific link instance.</param>
			/// <param name="linkTexture">The texture/style of the link cable.</param>
			/// <param name="linkedHandles">A list of actor handles to be linked together.</param>
			/// <param name="linkSecond">Visual effect duration.</param>
			/// <param name="linkEffect">Visual effect on link creation.</param>
			/// <param name="linkEffectScale">Scale of the creation effect.</param>
			/// <param name="linkSound">Sound effect on link creation.</param>
			public static void MakeLinker(IActor caster, int linkerId, string linkTexture, bool unkBool,
				List<int> linkedHandles, float linkSecond, string linkEffect, float linkEffectScale, string linkSound)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AddLinkEffect);

				packet.PutInt(caster.Handle);
				packet.PutInt(linkerId);
				packet.AddStringId(linkTexture);
				packet.PutByte(unkBool); // Unknown boolean flag
				packet.PutInt(linkedHandles.Count);
				foreach (var handle in linkedHandles)
				{
					packet.PutInt(handle);
				}
				packet.PutFloat(linkSecond);
				packet.AddStringId(linkEffect);
				packet.PutFloat(linkEffectScale);
				packet.AddStringId(linkSound);

				caster.Map.Broadcast(packet, caster);
			}

			/// <summary>
			/// Sends a command to clients to visually destroy an existing link.
			/// </summary>
			/// <param name="caster">The entity who created the link.</param>
			/// <param name="linkerId">The unique ID of the link instance to destroy.</param>
			public static void DestroyLinker(IZoneConnection conn, IActor caster, int linkerId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DestroyLinkEffect);

				packet.PutInt(caster.Handle);
				packet.PutInt(linkerId);

				conn.Send(packet);
			}

			/// <summary>
			/// Sends a command to clients to visually destroy an existing link.
			/// </summary>
			/// <param name="caster">The entity who created the link.</param>
			/// <param name="linkerId">The unique ID of the link instance to destroy.</param>
			public static void DestroyLinker(IActor caster, int linkerId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DestroyLinkEffect);

				packet.PutInt(caster.Handle);
				packet.PutInt(linkerId);

				caster.Map.Broadcast(packet, caster);
			}

			/// <summary>
			/// Sends a command to clients to visually destroy an existing link.
			/// </summary>
			/// <param name="caster">The entity who created the link.</param>
			/// <param name="linkerId">The unique ID of the link instance to destroy.</param>
			public static void LinkEffectDestruct(IZoneConnection conn, IActor caster, int linkerId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.LinkEffectDestruct);

				packet.PutInt(caster.Handle);
				packet.PutInt(linkerId);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);

				conn.Send(packet);
			}

			/// <summary>
			/// Sends a command to clients to visually destroy an existing link.
			/// </summary>
			/// <param name="caster">The entity who created the link.</param>
			/// <param name="linkerId">The unique ID of the link instance to destroy.</param>
			public static void LinkEffectDestruct(IActor caster, int linkerId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.LinkEffectDestruct);

				packet.PutInt(caster.Handle);
				packet.PutInt(linkerId);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);

				caster.Map.Broadcast(packet, caster);
			}

			/// <summary>
			/// Controls a summoned object or target actor's behavior and UI synchronization.
			/// Commonly used for Druid's Telepathy or summoned creatures.
			/// </summary>
			/// <param name="owner">The actor issuing the control command (self).</param>
			/// <param name="target">The actor being controlled (subActor).</param>
			/// <param name="lookType">Determines how the target faces: 0 (None), 1 (Same Direction), 2 (Look at Target).</param>
			/// <param name="autoCancelWithSkill">If true, the control ends automatically when the skill finishes.</param>
			/// <param name="isSnowBall">Special flag for snowball-type mechanics.</param>
			/// <param name="addonName">The name of the buff/addon. If provided, a UI appears and the summon is dismissed if this buff is removed.</param>
			/// <param name="isSkillUsable">Determines if the controlled object can use skills (defaults to false/0).</param>
			public static void ControlObject(
				IActor owner,
				IActor? target,
				ControlLookType lookType,
				bool autoCancelWithSkill,
				bool isSnowBall,
				string addonName,
				bool isSkillUsable = false)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ControlObject);

				packet.PutInt(owner.Handle);
				packet.PutInt(target?.Handle ?? 0);

				packet.PutByte((byte)lookType);
				packet.PutByte(autoCancelWithSkill);
				packet.PutByte(isSnowBall);

				packet.AddStringId(addonName);

				packet.PutByte(isSkillUsable);

				owner.Map.Broadcast(packet);
			}

			/// <summary>
			/// Ride Pet
			/// </summary>
			/// <param name="character"></param>
			/// <param name="companion"></param>
			public static void RidePet(Character character, Companion companion)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.RidePet);

				packet.PutInt(character.Handle);
				packet.PutInt(companion.Handle);
				packet.PutByte(character.IsRiding);
				packet.PutByte(companion.IsRiding);
				packet.PutLpString(companion.Data.ClassName);

				character.Map.Broadcast(packet);
			}

			/// <summary>
			/// Pet handle association?
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="companion"></param>
			public static void PetOwner(IZoneConnection conn, Companion companion)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PetOwner);

				packet.PutInt(companion.Handle);
				packet.PutInt(companion.OwnerHandle);

				conn.Send(packet);
			}

			/// <summary>
			/// Offset Y an entity's position from "Ground"
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="yOffset"></param>
			public static void SetHeight(IActor actor, float yOffset)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.OffsetY);

				packet.PutInt(actor.Handle);
				packet.PutFloat(yOffset);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Configures whether an actor should automatically detach if the object it is currently attached to moves.
			/// Useful for "cling" or "mount" mechanics where the actor should fall off if the host moves.
			/// </summary>
			/// <param name="actor">The actor that is currently attached to something.</param>
			/// <param name="isEnabled">If true, the actor will automatically detach when the host moves.</param>
			/// <param name="detachAnimationId">The animation ID to play on the actor when the detachment occurs.</param>
			public static void AutoDetachWhenTargetMove(IActor actor, bool isEnabled, string detachAnimationId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AutoDetachWhenTargetMove);

				packet.PutInt(actor.Handle);
				packet.PutByte(isEnabled);
				packet.AddStringId(detachAnimationId);

				actor.Map.Broadcast(packet);
			}


			/// <summary>
			/// Skill Jump/Knockback?
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="position"></param>
			/// <param name="height"></param>
			/// <param name="angle"></param>
			/// <param name="time1"></param>
			/// <param name="easeIn"></param>
			/// <param name="time2"></param>
			/// <param name="easeOut"></param>
			public static void LeapJump(IActor actor, Position position,
				float height, float angle, float time1, float easeIn, float time2, float easeOut)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_MoveJump);

				packet.PutInt(actor.Handle);
				packet.PutPosition(position);
				packet.PutFloat(height);
				packet.PutFloat(angle);
				packet.PutFloat(time1);
				packet.PutFloat(easeIn);
				packet.PutFloat(time2);
				packet.PutFloat(easeOut);

				actor.Map.Broadcast(packet, actor);
			}


			/// <summary>
			/// Unknown Purpose Yet.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="b1"></param>
			public static void EnterDelayedActor(IActor actor, byte b1 = 1)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.EnterDelayedActor);
				packet.PutInt(actor.Handle);
				packet.PutByte(b1);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Skill Related - Krivis Divine Stigma
			/// Unknown Purpose Yet.
			/// </summary>
			/// <remarks>function SKL_TGT_ATTACH_FORCE(self, skl, time, easing, eft, eftoffset, eftScale, finEft, finEftScale)</remarks>
			public static void SkillTargetAttachForce(IActor actor, IActor target,
				TimeSpan time, float easing, string effect,
				float effectScale, EffectLocation heightOffset, string finishEffect = null, float finishEffectScale = 1f)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SkillTargetAttachForce);
				packet.PutInt(actor.Handle);
				packet.PutInt(target.Handle);
				packet.PutFloat((float)time.TotalSeconds);
				packet.PutFloat(easing);
				packet.AddStringId(effect);
				packet.PutFloat(effectScale);
				packet.PutInt((int)heightOffset);
				packet.AddStringId(finishEffect);
				packet.PutFloat(finishEffectScale);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Updated the player's collection list.
			/// </summary>
			/// <param name="character"></param>
			public static void ItemCollectionList(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ItemCollectionList);

				packet.Zlib(true, zpacket =>
				{
					zpacket.PutLong(character.ObjectId);
					zpacket.PutInt(character.Collections.Count);

					foreach (var collection in character.Collections.GetList())
					{
						var registeredItems = collection.GetRegisteredItems();

						zpacket.PutShort(collection.Id);
						zpacket.PutInt(registeredItems.Count);

						foreach (var itemId in registeredItems)
						{
							zpacket.PutInt(itemId);
							zpacket.PutLong(itemId);
							zpacket.PutShort(0);
						}
					}
				});

				character.Connection.Send(packet);
			}


			/// <summary>
			/// Unlocks a collection for the player.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="collectionId"></param>
			public static void UnlockCollection(Character character, int collectionId)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.UnlockCollection);
				packet.PutLong(character.ObjectId);
				packet.PutInt(collectionId);

				character.Connection.Send(packet);
			}


			/// <summary>
			/// Updates the collection for the player.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="collectionId"></param>
			/// <param name="itemId"></param>
			public static void UpdateCollection(Character character, int collectionId, int itemId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.UpdateCollection);

				packet.PutLong(character.ObjectId);
				packet.PutInt(collectionId);
				packet.PutLong(itemId);

				character.Connection.Send(packet);
			}

			public static void Unknown_E0(Character entity)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Unknown_E0);

				packet.PutInt(entity.Handle);
				packet.PutByte(0);

				entity.Map.Broadcast(packet);
			}

			/// <summary>
			/// Plays text effect on actor.
			/// </summary>
			/// <remarks>
			/// The text effect is a small floating text that appears above the
			/// given actor. The actual string displayed is dictated by the
			/// Lua function given as the "packetString" argument, which is
			/// looked up in the packet string database, to send a reference
			/// to that name in form of an integer. This means that you can
			/// only use functions found inside that database by default.
			/// The known functions used for this can also be found in the
			/// script file "script_client.ipf\reaction\spcitem_text.lua".
			/// 
			/// The num and str arguments are then passed to the Lua function,
			/// and using this data, it returns a string that the client will
			/// then use for the floating text effect.
			/// 
			/// The look of the effect meanwhile is determined by the idSpace
			/// and classId. Consider the idSpace a kind of category that
			/// affects what the text looks like. For example, "Ability" will
			/// produce a red text, while "Collection" will be green.
			/// 
			/// Known idSpaces:
			/// - None: Orange text floating up
			/// - Ability: Red text floating up
			/// - Collection: Green text floating up
			/// - Skill: Yellow text, emphasized in place
			/// - Item: Yellow text floating up
			/// - Card (Item+CardItemId): White text floating up + sound effect
			/// 
			/// The only known idSpace value that makes use of the classId is
			/// "Item", which displays a different effect if the classId is
			/// that of a card item.
			/// 
			/// For custom texts, we added a fake packet string called
			/// "SHOW_CUSTOM_TEXT", which you can use to send custom
			/// strings via the argStr argument. Unfortunately, the
			/// client does not appear to support style formatting
			/// for these effects.
			/// </remarks>
			/// <example>
			/// PlayTextEffect(actor, caster, "SHOW_DMG_BLOCK");
			/// PlayTextEffect(actor, caster, "SHOW_BUFF_TEXT", (float)BuffId.Link, null, "Skill");
			/// PlayTextEffect(actor, caster, "SHOW_CUSTOM_TEXT", 0, "Hello, world!");
			/// </example>
			/// <param name="actor"></param>
			/// <param name="caster"></param>
			/// <param name="packetString"></param>
			/// <param name="argNum"></param>
			/// <param name="argStr"></param>
			/// <param name="idSpace"></param>
			/// <param name="classId"></param>
			public static void PlayTextEffect(IActor actor, IActor caster, string packetString, float argNum = 0, string argStr = null, string idSpace = "None", int classId = 0)
			{
				// Replace SHOW_CUSTOM_TEXT with SHOW_BUFF_TEXT, to use that function,
				// which we hijack
				if (packetString == "SHOW_CUSTOM_TEXT")
				{
					packetString = "SHOW_BUFF_TEXT";
					argStr = "CUSTOM:" + argStr;
				}

				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayTextEffect);

				packet.PutInt(actor.Handle);
				packet.PutInt(caster.Handle);
				packet.AddStringId(packetString);
				packet.PutFloat(argNum);

				if (argStr == null)
					packet.PutShort(-1);
				else
					packet.PutLpString(argStr);

				packet.AddStringId(idSpace);
				packet.PutInt(classId);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Plays text effect on actor but local (to a single connection).
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="caster"></param>
			/// <param name="packetString"></param>
			/// <param name="argNum"></param>
			/// <param name="argStr"></param>
			/// <param name="idSpace"></param>
			/// <param name="classId"></param>
			/// <exception cref="ArgumentException"></exception>
			public static void PlayTextEffectLocal(IZoneConnection conn, IActor actor, IActor caster, string packetString, float argNum = 0, string argStr = null, string idSpace = "None", int classId = 0)
			{
				// Replace SHOW_CUSTOM_TEXT with SHOW_BUFF_TEXT, to use that function,
				// which we hijack
				if (packetString == "SHOW_CUSTOM_TEXT")
				{
					packetString = "SHOW_BUFF_TEXT";
					argStr = "CUSTOM:" + argStr;
				}

				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayTextEffect);

				packet.PutInt(actor.Handle);
				packet.PutInt(caster.Handle);
				packet.AddStringId(packetString);
				packet.PutFloat(argNum);

				if (argStr == null)
					packet.PutShort(-1);
				else
					packet.PutLpString(argStr);

				packet.AddStringId(idSpace);
				packet.PutInt(classId);

				conn.Send(packet);
			}

			/// <summary>
			/// Sent during login for unknown purpose
			/// </summary>
			/// <param name="character"></param>
			public static void Unknown_E4(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Unknown_E5);
				packet.PutInt(0);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// NPC Dialog in Floating Gold Text
			/// </summary>
			/// <remarks>Internally called SHOWBALLOON_MSG</remarks>
			/// <param name="actor"></param>
			/// <param name="text"></param>
			/// <param name="duration"></param>
			public static void Notice(IActor actor, string text, TimeSpan duration)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Notice);
				packet.PutLpString(text);
				packet.PutFloat((float)duration.TotalSeconds);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// NPC Dialog in Floating Gold Text
			/// </summary>
			/// <param name="character"></param>
			/// <param name="dialogKey"></param>
			/// <param name="duration"></param>
			public static void Notice(Character character, string dialogKey, float duration)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Notice);
				packet.PutLpString(dialogKey);
				packet.PutFloat(duration);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Zoomed Camera Motion ("Slow Motion")
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="factor"></param>
			/// <param name="time"></param>
			public static void SlowMotion(IActor actor, float factor, float time)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SlowMotion);
				packet.PutFloat(factor);
				packet.PutFloat(time);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Sends the total amount of players with each job
			/// </summary>
			/// <param name="character"></param>
			public static void JobCount(Character character)
			{
				var jobDictionary = ZoneServer.Instance.Database.GetGlobalJobCount();
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.JobHistoryList);

				packet.PutLong(character.ObjectId);
				packet.PutInt(jobDictionary.Count);
				foreach (var job in jobDictionary)
				{
					var jobData = ZoneServer.Instance.Data.JobDb.Find((JobId)job.Key);
					packet.PutLpString(jobData.ClassName);
					packet.PutInt(job.Value);
				}

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Broadcast party member data
			/// </summary>
			/// <param name="group"></param>
			/// <param name="member"></param>
			public static void PartyMemberData(IMember member, IGroup group)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PartyMemberData);
				packet.PutByte(member.IsOnline);
				packet.PutByte((byte)group.Type);
				packet.PutLong(group.ObjectId);
				packet.PutLong(member.AccountObjectId);
				packet.AddMember(member);

				group.Broadcast(packet);
			}


			/// <summary>
			/// Update the group's leader.
			/// </summary>
			/// <param name="group"></param>
			public static void PartyLeaderChange(IGroup group, long leaderId)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PartyLeaderChange);
				packet.PutByte((byte)group.Type);
				packet.PutLong(group.ObjectId);
				packet.PutLong(leaderId);

				group.Broadcast(packet);
			}

			/// <summary>
			/// Server response on Party Property Change
			/// </summary>
			/// <param name="group"></param>
			public static void PartyNameChange(IGroup group)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PartyNameChange);
				packet.PutByte((byte)group.Type);
				packet.PutLong(group.ObjectId);
				packet.PutInt(0);
				packet.PutLong(group.Owner.ObjectId);
				packet.PutLpString(group.Name);
				packet.PutInt(1);
				packet.PutByte(1);

				group.Broadcast(packet);
			}

			/// <summary>
			/// Sends Party Invite UI to player
			/// </summary>
			/// <param name="caster"></param>
			/// <param name="sender"></param>
			public static void PartyInvite(Character character, Character sender, GroupType partyType)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PartyInvite);
				packet.PutByte((byte)partyType);
				packet.PutLong(sender.AccountObjectId);
				packet.PutLpString(sender.TeamName);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Server response on Party Property Change
			/// </summary>
			/// <param name="group"></param>
			/// <param name="property"></param>
			public static void PartyPropertyUpdate(IGroup group, int propertyId, string propertyValue)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PartyPropertyChange);
				packet.PutByte((byte)group.Type);
				packet.PutLong(group.ObjectId);
				packet.PutInt(propertyId);
				packet.PutLpString(propertyValue);

				group.Broadcast(packet);
			}

			/// <summary>
			/// Server response on Party Property Change
			/// </summary>
			/// <param name="group"></param>
			/// <param name="property"></param>
			public static void PartyPropertyUpdate(IGroup group, PropertyList properties)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PartyPropertyChange);
				packet.PutByte((byte)group.Type);
				packet.PutLong(group.ObjectId);
				packet.AddProperties(properties);

				group.Broadcast(packet);
			}

			/// <summary>
			/// Server response on Party Member Property Change
			/// </summary>
			/// <param name="group"></param>
			/// <param name="property"></param>
			public static void PartyMemberPropertyUpdate(IGroup group, Character character, PropertyList properties)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PartyMemberPropertyChange);
				packet.PutByte((byte)group.Type);
				packet.PutLong(group.ObjectId);
				packet.PutLong(character.ObjectId);
				packet.AddProperties(properties);

				group.Broadcast(packet);
			}

			/// <summary>
			/// Summon (Monster) plays an animation
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="packetString"></param>
			/// <param name="f1"></param>
			public static void SummonPlayAnimation(IActor actor, string playAnimation, float f1)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SummonPlayAnimation);
				packet.PutInt(actor.Handle);
				packet.AddStringId(playAnimation);
				packet.PutFloat(f1);

				actor.Map.Broadcast(packet, actor);
			}

			public static void ShowHookEffect(IActor actor, string effectName, float effectScale, string linkTextureName, string actorNodeName, float speed, float easing, Position position)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ShowHookEffect);
				packet.PutInt(actor.Handle);
				packet.AddStringId(effectName);
				packet.PutFloat(effectScale);
				packet.AddStringId(linkTextureName);
				packet.AddStringId(actorNodeName);
				packet.PutFloat(speed);
				packet.PutFloat(easing);
				packet.PutPosition(position);

				actor.Map.Broadcast(packet, actor);
			}

			public static void RemoveHookEffect(IActor actor, int effectId = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MakeHookEffect);
				packet.PutInt(actor.Handle);
				packet.PutInt(0);
				packet.PutByte(0);

				actor.Map.Broadcast(packet, actor);
			}


			public static void MakeHookEffect(IActor actor, IActor target, string effectName, float effectScale, string linkTextureName, string actorNodeName, string targetNodeName, float speed, float easing)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MakeHookEffect);
				packet.PutInt(actor.Handle);
				packet.PutInt(target.Handle);
				packet.AddStringId(effectName);
				packet.PutFloat(effectScale);
				packet.AddStringId(linkTextureName);
				packet.AddStringId(actorNodeName);
				packet.AddStringId(targetNodeName);
				packet.PutFloat(speed);
				packet.PutFloat(easing);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Disables Hotkeys.
			/// </summary>
			/// <remarks>
			/// Only used for specific buffs?
			/// </remarks>
			/// <param name="actor"></param>
			/// <param name="buffName"></param>
			/// <param name="skillId"></param>
			/// <param name="b1"></param>
			public static void ApplyBuff(IActor actor, string buffName, SkillId skillId, bool isUsable)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ApplyBuff);
				packet.PutInt(actor.Handle);
				packet.PutLpString(buffName);
				packet.PutInt((int)skillId);
				packet.PutByte(isUsable);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Enables Hotkeys.
			/// </summary>
			/// <remarks>
			/// Only used for specific buffs?
			/// </remarks>
			/// <param name="entity"></param>
			/// <param name="buffName"></param>
			public static void RemoveBuff(Character entity, string buffName)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.RemoveBuff);
				packet.PutInt(entity.Handle);
				packet.PutLpString(buffName);

				entity.Connection.Send(packet);
			}

			/// <summary>
			/// Enables Hotkeys.
			/// </summary>
			/// <remarks>
			/// Only used for specific buffs?
			/// </remarks>
			/// <param name="actor"></param>
			/// <param name="buffName"></param>
			public static void RemoveBuff(IActor actor, string buffName)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.RemoveBuff);
				packet.PutInt(actor.Handle);
				packet.PutLpString(buffName);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Unknown use but related to Hoplite Stabbing Skill
			/// </summary>
			/// <param name="entity"></param>
			/// <param name="skillId"></param>
			public static void Skill_10D(Character entity, SkillId skillId)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_10D);
				packet.PutInt(entity.Handle);
				packet.PutInt((int)skillId);

				entity.Connection.Send(packet);
			}

			/// <summary>
			/// Unknown use but related to Hoplite Stabbing Skill
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="skillId"></param>
			public static void Skill_10D(IActor actor, SkillId skillId)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_10D);
				packet.PutInt(actor.Handle);
				packet.PutInt((int)skillId);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Unknown purpose yet.
			/// </summary>
			/// <remarks>Sent when using barbarian rogue burrow skill</remarks>
			/// <param name="caster"></param>
			public static void Skill_10E(IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_10E);
				packet.PutInt(actor.Handle);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Unknown purpose yet.
			/// </summary>
			/// <remarks>Sent when using barbarian stomping kick skill</remarks>
			/// <param name="caster"></param>
			public static void Unknown_10F(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Unknown_10F);
				packet.PutInt(character.Handle);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Sent for unknown purpose related to skill (Hwarang PyeonJeon)
			/// Might be related to canceling dynamic cast.
			/// </summary>
			/// <param name="actor"></param>
			public static void CancelDynamicCast(IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.CancelDynamicCast);
				packet.PutInt(actor.Handle);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Dummy (Unknown Purpose)
			/// </summary>
			/// <param name="character"></param>
			public static void Unknown_11A(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Unknown_11A);
				packet.PutInt(0); // ?

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Unknown Purpose
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="packetString"></param>
			/// <param name="shopType"></param>
			/// <param name="i1"></param>
			public static void Shop_Unknown11C(IZoneConnection conn, string packetString, PersonalShopType shopType, int i1 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Shop_Unknown11C);

				packet.AddStringId(packetString);
				packet.PutInt((int)shopType);
				packet.PutInt(i1);

				conn.Send(packet);
			}


			/// <summary>
			/// Toggles the visibility of an actor's hit radius (hitbox). 
			/// Typically used for debugging or specific boss mechanics to show the player where they can land hits.
			/// </summary>
			/// <param name="targetActor">The actor whose hit radius will be visualized.</param>
			/// <param name="isEnabled">True to show the radius, false to hide it.</param>
			/// <param name="viewer">Optional: The specific player who should see the radius. If null, the effect is broadcasted to everyone in range.</param>
			public static void EnableHitRadiusPreview(IActor targetActor, bool isEnabled, IActor? viewer = null)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.EnableHitRadiusPreview);

				// [Self] The actor whose radius is being drawn
				packet.PutInt(targetActor.Handle);

				// [isEnable] Toggle
				packet.PutByte(isEnabled);

				// [pc] The viewer's handle. 
				// If 0, the client often interprets this as "everyone" or "no specific owner".
				if (isEnabled)
					packet.PutInt(viewer?.Handle ?? 0);
				else
					packet.PutInt(0);

				if (viewer is Character character)
				{
					// Send only to the specific player intended to see it
					character.Connection.Send(packet);
				}
				else
				{
					// Broadcast to all players who can see the target actor
					targetActor.Map.Broadcast(packet);
				}
			}

			public static void Transmutation(IActor actor, int monsterId, byte b1 = 1)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Transmutation);

				packet.PutInt(actor.Handle);
				packet.PutInt(monsterId);
				packet.PutInt(0);
				packet.PutInt(0);
				if (monsterId > 0)
					packet.PutByte(b1);

				actor.Map.Broadcast(packet);
			}

			public static void Transmutation(IZoneConnection conn, IActor actor, int monsterId, byte b1 = 1)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Transmutation);

				packet.PutInt(actor.Handle);
				packet.PutInt(monsterId);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutByte(b1);

				conn.Send(packet);
			}

			public static void Skill_122(IActor actor, string effectId, bool isEnabled = true)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_122);

				packet.AddStringId(effectId);
				packet.PutInt(isEnabled ? actor.Handle : 0);

				actor.Map.Broadcast(packet);
			}

			public static void Skill_122(Character character, IActor actor, string effectId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_122);

				packet.AddStringId(effectId);
				packet.PutInt(actor?.Handle ?? 0);

				character.Connection.Send(packet);
			}

			public static void RunJumpRope(IActor actor, int effectHandle, string effect, float effectScale, Position position,
				float radius, float width, int ropeCount, float readySec, int loopCount, float loopSec, float height)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.RunJumpRope);

				packet.PutInt(effectHandle);
				packet.PutInt(actor.Handle);
				packet.PutInt(0);
				packet.AddStringId(effect);
				packet.PutFloat(effectScale);
				packet.PutPosition(position);
				packet.PutFloat(radius);
				packet.PutFloat(width);
				packet.PutInt(ropeCount);
				packet.PutFloat(readySec);
				packet.PutFloat(loopCount);
				packet.PutInt(0);
				packet.PutFloat(loopSec);
				packet.PutFloat(height);

				actor.Map.Broadcast(packet);
			}

			public static void RunJumpRope(IActor actor, int effectHandle)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.RunJumpRope);

				packet.PutInt(effectHandle);
				packet.PutInt(0);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Seen with Lightning Tower Skill
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="effectName"></param>
			/// <param name="effectScale"></param>
			/// <param name="chainDuration"></param>
			/// <param name="hitKeyList"></param>
			public static void ShootChainEffect(IActor actor, string effectName, float effectScale, float chainDuration, params (int, int)[] hitKeyList)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ShootChainEffect);

				packet.PutInt(actor.Handle);
				packet.AddStringId(effectName);
				packet.PutFloat(effectScale);
				packet.PutFloat(chainDuration);
				packet.PutInt(hitKeyList?.Length ?? 0);

				if (hitKeyList?.Length > 0)
				{
					foreach (var (handle, syncKey) in hitKeyList)
					{
						packet.PutInt(handle);
						packet.PutInt(syncKey);
					}
				}

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Used with Pyromancer's Prominence Skill
			/// </summary>
			/// <param name="entity"></param>
			/// <param name="effectHandle"></param>
			/// <param name="packetString1"></param>
			/// <param name="f1"></param>
			/// <param name="packetString2"></param>
			/// <param name="effectScale"></param>
			/// <param name="maxHeight"></param>
			/// <param name="i2"></param>
			/// <param name="f4"></param>
			/// <param name="f5"></param>
			/// <param name="i3"></param>
			/// <param name="f6"></param>
			/// <param name="position"></param>
			/// <param name="i4"></param>
			/// <remarks>
			/// PAD_SET_PROMINENCE(self, skl, pad, eft, eftScale, maxHeight, coreCount, hitRange, onGroundTime, prominenceCount, moveCount, attackTime, jumpMin, jumpMax, maxMoveRange, preEft, preEftScale, preEftSecond)
			/// </remarks>
			public static void Skill_124(IActor entity, int effectHandle, string packetString1, float f1, string packetString2,
				float effectScale, float maxHeight, int i2, float f4, float f5, int i3, float f6, Position position, int i4)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_124);

				packet.PutInt(effectHandle);
				packet.PutByte(0);
				packet.PutInt(entity.Handle);
				packet.AddStringId(packetString1);
				packet.PutFloat(f1);
				packet.AddStringId(packetString2);
				packet.PutFloat(effectScale);
				packet.PutFloat(maxHeight);
				packet.PutInt(i2);
				packet.PutFloat(f4);
				packet.PutFloat(f5);
				packet.PutInt(i3);
				packet.PutFloat(f6);
				packet.PutFloat(position.X);
				packet.PutFloat(position.Y); // Seems to not be used
				packet.PutFloat(position.Z);
				packet.PutInt(i4);

				entity.Map.Broadcast(packet);
			}

			/// <summary>
			/// Used with Pyromancer's Prominence Skill
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="effectHandle"></param>
			public static void Skill_124(IActor actor, int effectHandle)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_124);

				packet.PutInt(effectHandle);
				packet.PutByte(1);

				actor.Map.Broadcast(packet);
			}


			/// <summary>
			/// Used with Pyromancer's Prominence Skill
			/// </summary>
			/// <param name="entity"></param>
			/// <param name="effectHandle"></param>
			/// <param name="position1"></param>
			/// <param name="position2"></param>
			public static void Skill_124(IActor entity, int effectHandle, Position position1, Position position2)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_124);

				packet.PutInt(effectHandle);
				packet.PutByte(2);
				packet.PutFloat(position1.X);
				packet.PutFloat(position1.Y);
				packet.PutFloat(position1.Z);
				packet.PutFloat(position2.X);
				packet.PutFloat(position2.Y);
				packet.PutFloat(position2.Z);

				entity.Map.Broadcast(packet);
			}

			/// <summary>
			/// Used with Hunter's Coursing Skill
			/// </summary>
			/// <param name="actor"></param>
			/// <param name=""></param>
			public static void Skill_127(IActor actor, int targetHandle, int effectHandle, Position position)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_127);

				packet.PutInt(actor.Handle);
				packet.PutInt(targetHandle);
				if (targetHandle != 0)
				{
					packet.PutInt(effectHandle);
					packet.PutFloat(position.X);
					packet.PutFloat(0);
					packet.PutFloat(position.Z);
				}

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Unknown purpose yet.
			/// </summary>
			/// <param name="character"></param>
			public static void ChannelTraffic(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ChannelTraffic);

				packet.Zlib(true, zpacket =>
				{
					var availableZoneServers = ZoneServer.Instance.ServerList.GetZoneServers(character.MapId);

					zpacket.PutShort(character.MapId);
					zpacket.PutShort(availableZoneServers.Length);

					for (var channelId = 0; channelId < availableZoneServers.Length; ++channelId)
					{
						var zoneServerInfo = availableZoneServers[channelId];

						// The client uses the "channelId" as part of the
						// channel name. For example, id 0 becomes "Ch 1",
						// id 1 becomes "Ch 2", etc. Because of this we
						// can't just send anything here, it needs to be
						// a sequential number starting from 0 to match
						// official behavior.

						zpacket.PutShort(channelId);
						zpacket.PutShort(zoneServerInfo.CurrentPlayers);
						zpacket.PutShort(zoneServerInfo.MaxPlayers);
					}
				});

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Displays a skill announcement balloon above a monster, typically used
			/// for boss monsters to warn players of incoming special attacks.
			/// </summary>
			/// <remarks>
			/// MonsterUsePCSkill(Monster, 'SkillName', skillLevel, useCount, pcSkillInfo=nil, changeColor=0, dontSay=0, factor=0, isDuplicate=0, showCastingbar=0)
			/// </remarks>
			/// <param name="monster">The monster using the skill.</param>
			/// <param name="skillName">The name of the skill to display.</param>
			/// <param name="skillLevel">Skill level.</param>
			/// <param name="useCount">Use count.</param>
			/// <param name="pcSkillInfo">Skill ID (optional, 0 for none).</param>
			/// <param name="changeColor">Change color (0 = normal).</param>
			/// <param name="dontSay">Don't say (0 = show balloon).</param>
			/// <param name="factor">Factor value.</param>
			/// <param name="isDuplicate">Is duplicate.</param>
			/// <param name="showCastingbar">Show casting bar.</param>
			public static void MonsterUsePCSkill(IActor monster, SkillId skillId, int castTime, int changeColor = 0, int showCastingbar = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MonsterUsePCSkill);
				packet.PutInt(monster.Handle);
				packet.PutInt((int)skillId);
				packet.PutInt(castTime);
				packet.PutInt(changeColor);
				packet.PutInt(showCastingbar);
				monster.Map.Broadcast(packet, monster);
			}

			/// <summary>
			/// Set's a label on an Actor.
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="label"></param>
			public static void ActorLabel(IActor actor, string label)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetActorLabel);

				packet.PutInt(actor.Handle);
				packet.PutLpString(label);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Sent on login.
			/// Unknown purpose, could be old Greeting Message Packet?
			/// </summary>
			/// <param name="character"></param>
			public static void Unknown_134(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Unknown_134);

				packet.Zlib(true, zpacket =>
				{
					zpacket.PutLong(character.ObjectId);
					zpacket.PutByte(1);
					zpacket.PutEmptyBin(6);
				});

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Displays guild name under player and party name?
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="character"></param>
			public static void ShowParty(IZoneConnection conn, Character character)
			{
				var party = character.Connection.Party;
				// Guild references removed: Guild type deleted during Laima merge
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ShowParty);

				packet.PutInt(character.Handle);
				if (party != null)
				{
					packet.PutByte(1);
					packet.PutLpString(party.Name);
					packet.PutByte(3);
				}

				conn.Send(packet);
			}

			/// <summary>
			/// Displays guild name under player and party name?
			/// </summary>
			/// <param name="character"></param>
			public static void ShowParty(Character character)
			{
				var party = character.Connection.Party;
				// Guild references removed: Guild type deleted during Laima merge

				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ShowParty);

				packet.PutInt(character.Handle);
				if (party != null)
				{
					packet.PutByte(1);
					packet.PutLpString(party.Name);
					packet.PutByte(3);
				}

				character.Map.Broadcast(packet);
			}

			/// <summary>
			/// Sent with Resurrect Packets?
			/// </summary>
			/// <param name="actor"></param>
			public static void Revive(IZoneConnection conn, IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Revive);

				packet.PutInt(actor.Handle);
				packet.PutByte(0);

				conn.Send(packet);
			}

			/// <summary>
			/// Sent with Resurrect Packets?
			/// </summary>
			/// <param name="actor"></param>
			public static void Revive(IActor actor)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Revive);

				packet.PutInt(actor.Handle);
				packet.PutByte(0);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Shows shop setup/closing animation 
			/// eg: Blacksmith Iron appears/disappears in client
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="entity"></param>
			/// <param name="scriptFunc"></param>
			/// <param name="b1"></param>
			/// <param name="b2">If set to 0, animation plays in reverse? Used to close shop.</param>
			public static void ShopAnimation(IZoneConnection conn, Character entity, string scriptFunc, byte b1 = 1, byte b2 = 2)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ShopAnimation);

				packet.PutInt(entity.Handle);
				packet.PutLpString(scriptFunc);
				packet.PutByte(b1);
				packet.PutByte(b2);

				conn.Send(packet);
			}

			/// <summary>
			/// Sends the session key to the client.
			/// </summary>
			/// <param name="conn"></param>
			public static void SetSessionKey(IZoneConnection conn)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetSessionKey);

				packet.PutLpString(conn.SessionKey);
				packet.PutByte(1);

				conn.Send(packet);
			}

			public static void FlyWithObject(IActor actor, IActor? target, string nodeName = "", float addHeight = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.FlyWithObject);

				packet.PutInt(actor.Handle);
				packet.PutInt(target?.Handle ?? 0);
				if (target != null)
				{
					packet.PutLpString(nodeName);
					packet.PutFloat(addHeight);
				}
				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Status Effect
			/// </summary>
			/// <param name="actor"></param>
			/// <param name="duration"></param>
			/// <param name="effectName"></param>
			/// <param name="effectType"></param>
			public static void StatusEffect(IActor actor, float duration, string effectName, string effectType)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.StatusEffect);

				packet.PutInt(actor.Handle);
				packet.PutFloat(duration);
				packet.PutLpString(effectName);
				packet.PutLpString(effectType);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Sent when there is a double log-in.
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="msg"></param>
			public static void DisconnectError(IZoneConnection conn, string msg)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DisconnectError);
				packet.PutLpString(msg);
				packet.PutInt(0);
				packet.PutByte(0);

				conn.Send(packet);
			}

			/// <summary>
			/// Makes item monster appear to drop, by "throwing" it a certain
			/// distance from its current position.
			/// </summary>
			/// <param name="monster"></param>
			/// <param name="direction"></param>
			/// <param name="distance"></param>
			public static void ItemDrop(IMonster monster, Direction direction, float distance)
			{
				// The distance might be more like a force, since items fly
				// farther than they should with high distances. Whether this
				// is a problem, depends on the used distance and the pick up
				// range. With a very small pick up range, like 10, and a high
				// distance, like 110, there will be a slight desync, and you
				// might not get the item, even if you're right on top of it.
				// But since we won't usually use such small and high values,
				// it will probably be fine.

				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ItemDrop);

				packet.PutInt(monster.Handle);
				packet.PutInt((int)direction.NormalDegreeAngle);
				packet.PutFloat(distance);

				if (Versions.Client < KnownVersions.ClosedBeta2)
				{
					packet.PutInt(3138);
					packet.PutInt(868864);
					packet.PutInt(monster.Handle);
					packet.PutByte(0);
				}

				monster.Map.Broadcast(packet, monster, false);
			}

			/// <summary>
			/// Set a character's state.
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="character"></param>
			/// <param name="isHostile"></param>
			public static void FightState(IZoneConnection conn, Character character, bool isHostile)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.FightState);
				packet.PutInt(character.Handle);
				packet.PutPosition(character.Position);
				packet.PutByte(isHostile);

				conn.Send(packet);
			}

			/// <summary>
			/// Updates silver transactions for storage
			/// </summary>
			/// <param name="character">Character browsing storage</param>
			/// <param name="transactions">Silver transaction list</param>
			/// <param name="init">'True' will erase previous transactions.</param>
			public static void StorageSilverTransaction(Character character, StorageSilverTransaction[] transactions, bool init)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.StorageSilverTransaction);

				packet.Zlib(true, zpacket =>
				{
					zpacket.PutByte(init);
					zpacket.PutInt(transactions.Length);
					foreach (var trans in transactions)
					{
						zpacket.PutByte((byte)trans.Interaction);
						zpacket.PutLong(trans.SilverTransacted);
						zpacket.PutLong(trans.SilverTotal);
						zpacket.PutLong(trans.TransactionTime);
					}
				});

				character.Connection.Send(packet);
			}

			public static void MemberMapStatusUpdate(IGroup group, IMember member)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.MemberMapStatusUpdate);

				packet.PutByte((byte)group.Type);
				packet.PutLong(member.AccountObjectId);
				packet.PutShort(member.IsOnline ? member.MapId : 0);
				packet.PutShort(member.IsOnline ? member.Channel : 0);

				group.Broadcast(packet);
			}

			/// <summary>
			/// Updates which headgears are visible for the character on
			/// clients in range.
			/// </summary>
			/// <param name="character"></param>
			public static void HeadgearVisibilityUpdate(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.HeadgearVisibilityUpdate);

				packet.PutInt(character.Handle);
				packet.PutByte((character.VisibleEquip & VisibleEquip.Headgear1) != 0);
				packet.PutByte((character.VisibleEquip & VisibleEquip.Headgear2) != 0);
				packet.PutByte((character.VisibleEquip & VisibleEquip.Headgear3) != 0);
				packet.PutByte((character.VisibleEquip & VisibleEquip.Wig) != 0);

				character.Map.Broadcast(packet, character);
			}

			/// <summary>
			/// Sends headgear visibility info (including wig) to a specific connection.
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="character"></param>
			public static void HeadgearVisibilityUpdate(IZoneConnection conn, Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.HeadgearVisibilityUpdate);

				packet.PutInt(character.Handle);
				packet.PutByte((character.VisibleEquip & VisibleEquip.Headgear1) != 0);
				packet.PutByte((character.VisibleEquip & VisibleEquip.Headgear2) != 0);
				packet.PutByte((character.VisibleEquip & VisibleEquip.Headgear3) != 0);
				packet.PutByte((character.VisibleEquip & VisibleEquip.Wig) != 0);

				conn.Send(packet);
			}

			/// <summary>
			///
			/// </summary>
			/// <param name="clientMessageId"></param>
			/// <param name="i1"></param>
			public static void WorldClientMessage(int clientMessageId, int i1)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.WorldClientMessage);

				packet.PutInt(clientMessageId);
				packet.PutInt(i1);
				packet.PutByte(0);

				ZoneServer.Instance.World.Broadcast(packet);
			}

			public static void WorldMessage(byte b1, string message, string parameter = "")
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.WorldMessage);

				packet.PutByte(b1);
				packet.PutLpString(message);
				packet.PutLpString(parameter);

				ZoneServer.Instance.World.Broadcast(packet);
			}

			/// <summary>
			/// Send all skills and their properties a character has.
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="propertyIds"></param>
			public static void SetSkillsProperties(IZoneConnection conn, params string[] propertyIds)
			{
				var skills = conn.SelectedCharacter.Skills.GetList();
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetSkillsProperties);

				packet.PutInt(skills.Length);
				foreach (var skill in skills)
				{
					var properties = skill.Properties.GetSelect(propertyIds);

					packet.PutInt((int)skill.Id);
					packet.PutShort(properties.GetByteCount());
					packet.AddProperties(properties);
				}

				conn.Send(packet);
			}

			/// <summary>
			/// Sent at the start of instanced map (dungeon).
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="instanceType"></param>
			public static void SetMapMode(IZoneConnection conn, string instanceType)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.InstanceStart);

				packet.PutLpString(instanceType);

				conn.Send(packet);
			}

			/// <summary>
			/// Updates the skill UI with character job data.
			/// </summary>
			/// <param name="character"></param>
			public static void UpdateSkillUI(Character character)
			{
				// While the client will apparently gladly accept any combination
				// of jobs, the skill UI will only appear correctly if job
				// data for the character's current "display job" is sent.
				// For example, if the display job is Archer, data for *that*
				// job must be sent. Other base classes or higher jobs in the
				// same class do not work. Same thing for when the display
				// job is a higher job.
				// If data for the base job is sent though, other jobs will
				// appears as well. So it seems like you can create a Wizard/
				// Archer hybrid for example.

				var jobs = character.Jobs.GetList().OrderBy(a => a.Id);

				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.UpdateSkillUI);

				packet.PutLong(character.ObjectId);
				packet.PutInt(jobs.Count());
				foreach (var job in jobs)
				{
					packet.PutShort((short)job.Id);
					packet.PutShort((short)job.Level);
					packet.PutInt(0);
					packet.PutLong(job.TotalExp);
					packet.PutByte((byte)job.SkillPoints);
					packet.PutShort(0);
					packet.PutEmptyBin(5);
					packet.PutLong(job.AdvancementDate.ToFileTime());
					packet.PutLong(job.SelectionDate.ToFileTime());
				}

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Show Instance Dungeon Match Making UI
			/// </summary>
			/// <remarks>
			/// Client-side ShowIndunEnterDialog
			/// </remarks>
			/// <param name="entity"></param>
			/// <param name="instanceDungeonId"></param>
			/// <param name="allowAutoMatchReenter"></param>
			/// <param name="allowAutoMatch"></param>
			/// <param name="allowEnterNow"></param>
			/// <param name="allowAutoMatchParty"></param>
			/// <param name="b1"></param>
			public static void InstanceDungeonMatchMaking(Character entity, int instanceDungeonId, int allowAutoMatchReenter, int allowAutoMatch, int allowEnterNow, int allowAutoMatchParty, byte b1 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.InstanceDungeonMatchMaking);

				packet.PutInt(instanceDungeonId);
				packet.PutInt(allowAutoMatchReenter);
				packet.PutInt(allowAutoMatch);
				packet.PutInt(allowEnterNow);
				packet.PutInt(allowAutoMatchParty);
				packet.PutByte(b1);

				entity.Connection.Send(packet);
			}

			/// <summary>
			/// Shows the Instance Dungeon Matchmaking UI on the client.
			/// </summary>
			/// <remarks>
			/// Triggers the client-side function ShowIndunEnterDialog.
			/// </remarks>
			/// <param name="character">The character to send the packet to.</param>
			/// <param name="instanceDungeonId">The ID of the dungeon instance.</param>
			/// <param name="options">The configuration for which buttons are enabled on the UI.</param>
			public static void InstanceDungeonMatchMaking(Character character, int instanceDungeonId, DungeonOptions options)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.InstanceDungeonMatchMaking);

				packet.PutInt(instanceDungeonId);

				// The client expects integers (0 or 1), so we convert our booleans.
				packet.PutInt(options.AllowAutoMatchReenter ? 1 : 0);
				packet.PutInt(options.AllowAutoMatch ? 1 : 0);
				packet.PutInt(options.AllowEnterNow ? 1 : 0);
				packet.PutInt(options.AllowPartyMatch ? 1 : 0);
				packet.PutByte(options.IsGrowthSupportGuildParty ? (byte)1 : (byte)0);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Send fishing rank data
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="type"></param>
			public static void FishingRankData(IZoneConnection conn, string type)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				var rankingCount = 1;

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.FishingRankData);
				packet.PutLpString("Fishing");
				packet.PutLpString(type);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				if (rankingCount > 0)
				{
					packet.PutLong(conn.Account.ObjectId);
					packet.PutInt(100);
					packet.PutLpString(conn.SelectedCharacter.Name);
				}

				conn.Send(packet);
			}

			/// <summary>
			/// Initializing the adventure book
			/// </summary>
			/// <param name="conn"></param>
			public static void AdventureBook(IZoneConnection conn)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AdventureBook);

				packet.PutLpString("AdventureBook");
				packet.PutLpString("Initialization_point");
				packet.PutInt(-1);
				packet.PutInt(0);
				packet.PutInt(0); // Rank Points
				packet.PutByte(1);

				conn.Send(packet);
			}

			/// <summary>
			/// Unknown purpose, sent on login
			/// </summary>
			/// <param name="conn"></param>
			public static void AdventureBookRank(IZoneConnection conn)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.AdventureBookRank);

				packet.PutInt(1); // Current Rank
				packet.PutInt(1000); // ?
				packet.PutInt(100000); // Current Points
				packet.PutInt(3); // 3?
				for (var i = 0; i < 3; i++)
				{
					packet.PutLong(conn.SelectedCharacter.AccountObjectId);
					packet.PutLpString(conn.SelectedCharacter.TeamName);
					packet.PutInt(100000 - i);
				}
				// Nearest 5 ranks
				for (var i = 1; i < 6; i++)
				{
					packet.PutInt(i);
					packet.PutLong(conn.SelectedCharacter.AccountObjectId);
					packet.PutInt(100000 - i);
				}
				packet.PutInt(100000); // Current Points
				packet.PutInt(1);
				packet.PutLong(conn.SelectedCharacter.AccountObjectId);
				packet.PutInt(1); // Current Rank
				packet.PutInt(100000); // Current Points

				conn.Send(packet);
			}

			/// <summary>
			/// Unknown purpose, sent on login
			/// </summary>
			/// <param name="conn"></param>
			public static void Unknown198(IZoneConnection conn)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Unknown_198);

				packet.PutInt(0);
				packet.PutInt(0);

				conn.Send(packet);
			}

			/// <summary>
			/// Unknown purpose yet. (Dummy)
			/// </summary>
			/// <param name="character"></param>
			public static void Unknown_19B(IZoneConnection conn)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Unknown_19B);
				packet.PutLong(1);
				packet.PutByte(0);

				conn.Send(packet);
			}

			/// <summary>
			/// Unknown purpose yet. (Dummy)
			/// Set's the time for something.
			/// </summary>
			/// <remarks>
			/// 05/31/23 (Showed DateTime for 06/04/23 21:00)
			/// Resets on Sunday?
			/// </remarks>
			/// <param name="conn"></param>
			public static void Unknown_19D_SetTime(IZoneConnection conn)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Unknown_19D_SetTime);

				packet.PutByte(0);
				packet.PutDate(DateTime.Now.AddHours(2));

				conn.Send(packet);
			}

			/// <summary>
			/// Sent when Pet is activated or disabled
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="companion"></param>
			public static void PetIsInactive(IZoneConnection conn, Companion companion)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PetIsInactive);

				packet.PutInt(companion.Handle);
				packet.PutInt(companion.IsActivated ? 0 : 1); // Inverse of active

				conn.Send(packet);
			}

			/// <summary>
			/// Set the sub attack skill (access by C key).
			/// </summary>
			/// <param name="character"></param>
			public static void SetSubAttackSkill(Character character, SkillId skillId)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetSubAttackSkill);
				packet.PutInt(character.Handle);
				packet.PutInt((int)skillId);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Unknown purpose yet.
			/// </summary>
			/// <param name="character"></param>
			public static void Unknown_1A6(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Unknown_1A6);

				packet.PutShort(1);
				packet.PutInt(50);
				packet.PutInt(50);
				packet.PutInt(100);
				packet.PutInt(100);
				packet.PutFloat(0.3f);
				packet.PutFloat(3f);
				packet.PutInt(5000);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Sets the model for a pad to a certain item.
			/// </summary>
			/// <remarks>
			/// Used in skills like Throw Spear and Shield Lob.
			/// </remarks>
			/// <param name="actor"></param>
			/// <param name="xacHeadName"></param>
			/// <param name="itemId"></param>
			public static void PadSetModel(IActor actor, string xacHeadName, int itemId = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadSetModel);
				packet.PutInt(actor.Handle);
				packet.PutLpString(xacHeadName);
				packet.PutInt(itemId);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Sets the model for a pad to a certain item.
			/// </summary>
			/// <remarks>
			/// Used in skills like Throw Spear and Shield Lob.
			/// </remarks>
			/// <param name="actor"></param>
			/// <param name="xacHeadName"></param>
			/// <param name="itemId"></param>
			public static void PadSetModel(IZoneConnection conn, IActor actor, string xacHeadName, int itemId = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PadSetModel);
				packet.PutInt(actor.Handle);
				packet.PutLpString(xacHeadName);
				packet.PutInt(itemId);

				conn.Send(packet);
			}

			/// <summary>
			/// Updates weather wig eequipment is visible for the character
			/// on clients in range.
			/// </summary>
			/// <param name="character"></param>
			public static void WigVisibilityUpdate(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.WigVisibilityUpdate);

				packet.PutInt(character.Handle);
				packet.PutByte((character.VisibleEquip & VisibleEquip.Wig) != 0);

				character.Map.Broadcast(packet, character);
			}

			/// <summary>
			/// Used Medal Total
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="medals"></param>
			public static void UsedMedalTotal(IZoneConnection conn, int medals)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.UsedMedalTotal);

				packet.PutInt(medals);

				conn.Send(packet);
			}

			/// <summary>
			/// Unknown purpose yet.
			/// Sent on logging in
			/// </summary>
			/// <param name="conn"></param>
			public static void Unknown_1B6(IZoneConnection conn)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Unknown_1B6);

				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);

				conn.Send(packet);
			}

			/// <summary>
			/// Send client ToS Steam Achievement
			/// </summary>
			/// <param name="entity"></param>
			/// <param name="achievement"></param>
			public static void SteamAchievement(Character entity, string achievement)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SteamAchievement);
				packet.PutLpString(achievement);

				entity.Connection.Send(packet);
			}

			/// <summary>
			/// Rotates an actor on the given axes.
			/// </summary>
			/// <remarks>
			/// One usage example is Shield Lob where this is used to rotate the
			/// shield onto its side.
			/// </remarks>
			/// <param name="actor"></param>
			/// <param name="angleX"></param>
			/// <param name="angleY"></param>
			/// <param name="angleZ"></param>
			public static void ActorRotate(IActor actor, float angleX, float angleY, float angleZ)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ActorRotate);
				packet.PutInt(actor.Handle);
				packet.PutFloat(angleX);
				packet.PutFloat(angleY);
				packet.PutFloat(angleZ);

				actor.Map.Broadcast(packet);
			}

			/// <summary>
			/// Rotates an actor on the given axes.
			/// </summary>
			/// <remarks>
			/// One usage example is Shield Lob where this is used to rotate the
			/// shield onto its side.
			/// </remarks>
			/// <param name="actor"></param>
			/// <param name="angleX"></param>
			/// <param name="angleY"></param>
			/// <param name="angleZ"></param>
			public static void ActorRotate(IZoneConnection conn, IActor actor, float angleX, float angleY, float angleZ)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.ActorRotate);
				packet.PutInt(actor.Handle);
				packet.PutFloat(angleX);
				packet.PutFloat(angleY);
				packet.PutFloat(angleZ);

				conn.Send(packet);
			}

			/// <summary>
			/// Updates weather wig eequipment is visible for the character
			/// on clients in range.
			/// </summary>
			/// <param name="character"></param>
			public static void SubWeaponVisibilityUpdate(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SubWeaponVisibilityUpdate);

				packet.PutInt(character.Handle);
				packet.PutByte((character.VisibleEquip & VisibleEquip.SubWeapon) != 0);

				character.Map.Broadcast(packet, character);
			}

			public static void Skill_5F(Character character, int i1, float f1, float f2 = 0, byte b1 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_5F);

				packet.PutInt(i1);
				packet.PutFloat(f1);
				packet.PutFloat(f2);
				packet.PutByte(b1);

				character.Map.Broadcast(packet);
			}

			/// <summary>
			/// Play gathering corpse parts from an actor
			/// </summary>
			/// <param name="caster"></param>
			/// <param name="corpse"></param>
			public static void PlayGatherCorpseParts(IActor caster, IActor corpse)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayGatherCorpseParts);

				packet.PutInt(caster.Handle);
				packet.PutInt(corpse.Handle);

				caster.Map.Broadcast(packet, caster);
			}

			/// <summary>
			/// Related to casting Zombify
			/// </summary>
			/// <param name="caster"></param>
			public static void Skill_Unknown_D4(IActor caster)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Skill_Unknown_D4);

				packet.PutInt(caster.Handle);

				caster.Map.Broadcast(packet);
			}

			/// <summary>
			/// Create a ring of corpse parts
			/// </summary>
			/// <param name="caster"></param>
			/// <param name="effectHandle"></param>
			/// <param name="ringExpansionDuration"></param>
			/// <param name="ringRadius"></param>
			/// <param name="ringPartsRotationSpeed"></param>
			/// <param name="corpsePartCount"></param>
			/// <param name="monsterIds"></param>
			public static void PlayCorpsePartsRing(IActor caster, int effectHandle, float ringExpansionDuration, float ringRadius, float ringPartsRotationSpeed, int corpsePartCount, params int[] monsterIds)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayCorpsePartsRing);

				packet.PutInt(caster.Handle);
				packet.PutInt(effectHandle);
				packet.PutFloat(ringExpansionDuration);
				packet.PutFloat(ringRadius);
				packet.PutFloat(ringPartsRotationSpeed);
				packet.PutInt(monsterIds.Length);
				for (var i = 0; i < monsterIds.Length; i++)
				{
					packet.PutInt(monsterIds[i]);
					packet.PutByte((byte)i);
					packet.PutInt(corpsePartCount);
					packet.PutInt(corpsePartCount);
				}

				caster.Map.Broadcast(packet, caster);
			}

			/// <summary>
			/// Remove effect
			/// </summary>
			/// <param name="caster"></param>
			/// <param name="effectHandle"></param>
			public static void RemoveCorpseParts(IActor caster, int effectHandle)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.RemoveCorpseParts);

				packet.PutInt(caster.Handle);
				packet.PutInt(effectHandle);

				caster.Map.Broadcast(packet, caster);
			}

			public static void DropCorpseParts(IActor actor, int effectHandle, byte b1, int monsterId, byte b2, int i3, int i4)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DropCorpseParts);

				packet.PutInt(actor.Handle);
				packet.PutInt(effectHandle);
				packet.PutByte(b1);
				packet.PutInt(monsterId);
				packet.PutByte(b2);
				packet.PutInt(i3);
				packet.PutInt(i4);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="caster"></param>
			/// <param name="effectHandle"></param>
			/// <param name="position"></param>
			/// <param name="particleSpread"></param>
			/// <param name="startDelay"></param>
			/// <param name="particleSpeed"></param>
			/// <param name="f4"></param>
			/// <param name="animationDuration"></param>
			/// <param name="i2"></param>
			public static void PlayThrowCorpseParts(IActor caster, int effectHandle, Position position, float particleSpread, float startDelay, float particleSpeed, float f4, float animationDuration, int i2)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayThrowCorpseParts);

				packet.PutInt(effectHandle);
				packet.PutPosition(position);
				packet.PutFloat(particleSpread);
				packet.PutFloat(startDelay);
				packet.PutFloat(particleSpeed);
				packet.PutFloat(f4);
				packet.PutFloat(animationDuration);
				packet.PutInt(i2);

				caster.Map.Broadcast(packet, caster);
			}

			public static void Unknown_14A(IZoneConnection conn, byte b1, List<Character> characters)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.Unknown_14A);

				packet.PutByte(b1);
				packet.PutInt(characters.Count);

				foreach (var character in characters)
				{
					var jobs = character.Jobs.GetList();

					packet.PutLong(character.AccountObjectId);
					packet.PutByte(1);
					packet.PutString(character.TeamName, 64);
					packet.PutString(character.Name, 65);
					packet.PutByte(2);
					packet.PutShort((short)character.JobId);
					packet.PutInt((int)character.JobId);
					packet.PutInt(character.Job.Level);
					packet.PutShort((short)character.Gender);
					packet.PutShort((short)character.Hair);
					// 628333,628341,11006112,18015,0
					//int visualEquipIds[5];
					packet.PutEmptyBin(20);
					packet.PutInt(character.MapId);
					packet.PutInt(character.Level);
					packet.PutUInt(character.SkinColor);
					for (var i = 0; i < 4; i++)
					{
						if (character.Jobs.Count < i)
							packet.PutShort((short)jobs[i].Id);
						else
							packet.PutShort(0);
					}
					packet.PutLong(0);
				}

				conn.Send(packet);
			}

			/// <summary>
			/// Enable Action?
			/// </summary>
			/// <remarks>
			/// Team Battle League (TBL) Related?
			/// Seen during TBL
			/// </remarks>
			/// <param name="conn"></param>
			/// <param name="isEnabled"></param>
			public static void EnableAction(IZoneConnection conn, bool isEnabled)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.EnableAction);

				packet.PutByte(isEnabled);

				conn.Send(packet);
			}

			/// <summary>
			/// Show's a duel request to another player.
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="sender"></param>
			public static void RequestDuel(IZoneConnection conn, Character sender)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.RequestDuel);

				packet.PutInt(sender.Handle);
				packet.PutLpString(sender.TeamName);

				conn.Send(packet);
			}

			/// <summary>
			/// Show's a duel request to another player.
			/// </summary>
			/// <param name="conn"></param>
			/// <param name="instanceDungeonId"></param>
			public static void DungeonAutoMatching(IZoneConnection conn, int instanceDungeonId)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DungeonAutoMatching);

				packet.PutInt(instanceDungeonId);

				if (instanceDungeonId != 0)
				{
					packet.PutByte(0);
				}

				conn.Send(packet);
			}

			/// <summary>
			/// Update client with the party count and level information for dungeon auto-matching.
			/// </summary>
			/// <param name="conn">The connection through which the packet will be sent. Cannot be <c>null</c>.</param>
			/// <param name="partyCount">The number of players in the party. Must be a non-negative integer.</param>
			/// <param name="level">The current level of the party just leader or the party's average level.</param>
			/// <param name="maxLevel">The maximum level allowed for the dungeon. Must be greater than or equal to <paramref name="level"/>.</param>
			/// <param name="levelDiff">The level difference threshold for matching. Determines the range of levels considered for auto-matching.</param>
			/// <param name="dungeonName">The name of the dungeon for which the auto-matching is being performed. Cannot be <c>null</c> or empty.</param>
			public static void DungeonAutoMatchWithParty(IZoneConnection conn, int partyCount, int level, int maxLevel, int levelDiff, string dungeonName)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DungeonAutoMatchWithParty);

				packet.PutInt(partyCount);
				packet.PutInt(level);
				packet.PutInt(maxLevel);
				packet.PutInt(levelDiff);
				packet.PutLpString(dungeonName);

				conn.Send(packet);
			}

			public static void DungeonAutoMatchPartyCount(IZoneConnection conn, int partyQueueMemberCount, string memberStr, int i1 = 0)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DungeonAutoMatchPartyCount);

				packet.PutInt(partyQueueMemberCount);
				for (var i = 0; i < partyQueueMemberCount; i++)
				{
					// Sample Data: {accountObjectId}/{classId}/{level}/{characterObjectId}/{readyStatus:YES|NO}/
					packet.PutLpString(memberStr);
					packet.PutInt(i1);
				}

				conn.Send(packet);
			}

			public static void DungeonAutoMatchPartyCount(IZoneConnection conn, IList<string> memberStrings)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.DungeonAutoMatchPartyCount);

				packet.PutInt(memberStrings.Count);
				for (var i = 0; i < memberStrings.Count; i++)
				{
					// Sample Data: {accountObjectId}/{classId}/{level}/{characterObjectId}/{readyStatus:YES|NO}/
					packet.PutLpString(memberStrings[i]);
					packet.PutInt(0);
				}

				conn.Send(packet);
			}
		}

		public static class ZC_TO_CLIENT
		{
			/// <summary>
			/// Guild/Party Event Related?
			/// </summary>
			/// <remarks>Maybe like a trick packet or ZC_NORMAL </remarks>
			/// <param name="character"></param>
			/// <param name="message"></param>
			/// <param name="parameter"></param>
			/// <param name="duration"></param>
			public static void MessageParameter(Character character, string message, string parameter = "", int duration = 0)
			{
				var packet = new Packet(Op.ZC_TO_CLIENT);
				packet.PutInt(NormalOp.GuildOp.MessageParameter);

				packet.PutLong(character.Connection.Account.ObjectId);
				packet.PutLpString(message);
				packet.PutLpString(parameter);
				packet.PutInt(duration);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Guild/Party Event Related?
			/// </summary>
			/// <remarks>Maybe like a trick packet or ZC_NORMAL </remarks>
			/// <param name="party"></param>
			/// <param name="character"></param>
			/// <param name="message"></param>
			/// <param name="parameter"></param>
			public static void PartyMessage(Party party, Character character, string message, string parameter = "")
			{
				var packet = new Packet(Op.ZC_TO_CLIENT);
				packet.PutInt(NormalOp.GuildOp.GuildMessage);

				packet.PutLong(character.AccountDbId);
				packet.PutByte((byte)party.Type);
				packet.PutLong(party.ObjectId);
				packet.PutLpString(message);

				party.Broadcast(packet);
				packet.PutLong(character.ObjectId);
			}
		}
	}
}
