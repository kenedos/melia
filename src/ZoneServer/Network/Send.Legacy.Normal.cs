using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Zone.Network.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Network
{
	public static partial class Send
	{
		public static partial class ZC_NORMAL
		{
			/// <summary>
			/// Plays an animation effect (Older Client Version).
			/// </summary>
			/// <remarks>
			/// This function sends the packet with sub-opcode 0x15, used in older clients.
			/// Corresponds to script_list.xml ClassID 673: PlayEffect
			/// </remarks>
			public static void PlayEffect_15(IActor actor, string effectName, float scale = 1, EffectLocation location = EffectLocation.Bottom, bool skipIfExisting = false, bool applyActorScale = false)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayEffect_15);

				packet.PutInt(actor.Handle);
				packet.AddStringId(effectName);
				packet.PutFloat(scale);
				packet.PutByte(skipIfExisting);
				packet.PutInt((int)location);
				packet.PutByte(applyActorScale);

				actor.Map.Broadcast(packet, actor);
			}

			/// <summary>
			/// Controls a skill's visual effects (Older Client Version).
			/// </summary>
			/// <remarks>
			/// This function sends the packet with sub-opcode 0x16, used in older clients.
			/// </remarks>
			public static void PlayForceEffect_16(int forceId, IActor caster, IActor source, IActor target,
				string effect, float scale, string soundEffect, string endEffect, float endEffectScale, string endSoundEffect, string effectSpeed, float speed)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.PlayForceEffect_16);

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

				source.Map.Broadcast(packet, target);
			}

			public static void MarketItemList(IZoneConnection conn)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(0xF4);
				packet.PutBinFromHex("010000000008000000080400004C0200009F1C0000007A5EE75819D1013408000001000000D6BD0200751C00001AD901000D060000D51C00000500506F6C6900000C00561C0000803F621C00000000210000001F10000000A5451CAB16D101C409000001000000E11F00001F100000F5DC01007E360000020F000009004D61727368616C6C00002D00621C00000000461D08004C656F6E61646F00401D0A004175726153776F726400431D090050726F6E7465726100FC0000008813000080578FEEA117D101C4090000010000004098010088130000F5DC0100611B0100020F000005004F72636100001800621C00000000461D05004F72636100431D05004F72636100FD0000008813000080DE22F2A117D101C4090000010000004198010088130000F5DC0100611B0100020F000005004F72636100001800621C00000000461D05004F72636100431D05004F72636100850200004314000080197A750E18D101C409000001000000940602004D1100001AD90100C8150100010F000006004C656F6E61000018004F1C00004045561C00000040621C00000000601C00004040560000005614000000F216BBCE17D101C409000001000000827A0200B5110000F5DC01009AB90000010F0000060052616C6C7900001000621C00000000461D060052616C6C7900760000006D190000000D130CD918D101C409000001000000B4A10100EA190000F5DC0100230C00002113000006004C7961726100001F00621C00000000461D06004C7961726100431D0B00477265617420486F726E00780000006D1900008092C819D918D101C409000001000000B2A10100EA190000F5DC0100230C00002113000006004C7961726100001F00621C00000000461D06004C7961726100431D0B00477265617420486F726E00");

				conn.Send(packet);
			}
			public static void Unknown_9A(IZoneConnection conn)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(0x9A);

				packet.PutInt(0);
				packet.PutByte(0);

				conn.Send(packet);
			}

			public static void Unknown_CF(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(0xCF);

				packet.PutLong(character.ObjectId);
				packet.PutInt(0);

				character.Connection.Send(packet);
			}

			public static void Unknown_DE(IZoneConnection conn)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(0xDE);

				packet.PutInt(0);

				conn.Send(packet);
			}

			public static void Unknown_EB(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(0xEB);
				packet.PutLong(character.ObjectId);
				packet.PutInt(1);
				packet.PutInt(1);
				packet.PutShort((short)character.JobId);
				packet.PutShort(1);
				packet.PutInt(0);
				packet.PutLong(DateTime.Now.Add(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now)).ToFileTime());

				character.Connection.Send(packet);
			}

			public static void Unknown_128(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);

				packet.PutSubOp(0x128);
				packet.Zlib(true, zpacket =>
				{
					zpacket.PutLong(character.ObjectId);
					zpacket.PutInt(1);
					zpacket.PutShort(0);
					zpacket.PutByte(0);
				});

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Sets the character's greeting message.
			/// </summary>
			/// <param name="character"></param>
			public static void SetGreetingMessage(Character character)
			{
				var packet = new Packet(Op.ZC_NORMAL);
				packet.PutSubOp(NormalOpType.Zone, NormalOp.Zone.SetGreetingMessage);

				packet.PutLong(character.ObjectId);
				packet.PutInt(0);
				packet.PutLpString(character.GreetingMessage);

				character.Connection.Send(packet);
			}
		}
	}
}
