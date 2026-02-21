using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Numerics;
using Melia.Shared.Data.Database;
using Melia.Shared.Game;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.Network;
using Melia.Shared.Network.Helpers;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Util;
using Melia.Shared.Versioning;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Events;
using Melia.Zone.Network.Helpers;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Groups;
// using Melia.Zone.World.GuildColony; // Removed: GuildColony namespace deleted
// using Melia.Zone.World.Houses; // Removed: Houses namespace deleted
using Melia.Zone.World.Items;
using Melia.Zone.World.Maps;
using Yggdrasil.Extensions;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Melia.Zone.Network
{
	public static partial class Send
	{
		/// <summary>
		/// Sends ZC_CONNECT_OK to connection, verifying the connection and
		/// giving information about the character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_CONNECT_OK(IZoneConnection conn, Character character)
		{
			var packet = new Packet(Op.ZC_CONNECT_OK);

			packet.PutByte(0); // gameMode 0 = NormalMode, 1 = SingleMode
			packet.PutInt(1281523659); // was 1281523659 now 1277746433
			packet.PutByte((byte)character.PermissionLevel);
			if (Versions.Protocol > 500)
			{
				packet.PutEmptyBin(10);
				packet.PutInt(0);
				packet.PutShort(0);
				packet.PutInt(40588976); // 44266500
				packet.PutEmptyBin(10);

				// These bytes set the integrated and integrated dungeon server
				// settings.
				packet.PutByte(0);
				packet.PutByte(0);

				// [i348202, 2021-12-15]
				// Where exactly this bytes goes is currently unknown
				packet.PutByte(0);
			}
			else
			{
				packet.PutByte(0);
				packet.PutByte(0x4E);
				packet.PutEmptyBin(8);
			}

			packet.PutLpString(conn.SessionKey);
			packet.AddCommander(character);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_CONNECT_FAILED to connection, disconnecting the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="type"></param>
		/// <param name="msg"></param>
		public static void ZC_CONNECT_FAILED(IZoneConnection conn, int type, string msg = "")
		{
			var packet = new Packet(Op.ZC_CONNECT_FAILED);

			packet.PutInt(type);
			packet.PutString(msg);

			conn.Send(packet);
			conn.Close();
		}

		/// <summary>
		/// Move Animation ?
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="animationId"></param>
		/// <param name="b2"></param>
		public static void ZC_MOVE_ANIM(IActor entity, FixedAnimation animationId, byte b2)
		{
			var packet = new Packet(Op.ZC_MOVE_ANIM);

			packet.PutInt(entity.Handle);
			packet.PutByte((byte)animationId);
			packet.PutByte(b2);

			entity.Map.Broadcast(packet);
		}

		/// <summary>
		/// Attach to Object
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="attachToEntity"></param>
		/// <param name="nodeName"></param>
		/// <param name="targetNodeName"></param>
		/// <param name="attachSeconds"></param>
		/// <param name="l1"></param>
		/// <param name="l2"></param>
		/// <param name="attachAnimation"></param>
		/// <param name="b1"></param>
		public static void ZC_ATTACH_TO_OBJ(IActor actor, IActor attachToEntity, string nodeName,
			string targetNodeName, float attachSeconds = 0, float f1 = 0, float f2 = 0, float f3 = 0,
			float distance = 0, string attachAnimation = "None", byte b1 = 0, byte b2 = 0, byte b3 = 0)
		{
			var packet = new Packet(Op.ZC_ATTACH_TO_OBJ);

			packet.PutInt(actor.Handle);
			packet.PutInt(attachToEntity?.Handle ?? 0);
			packet.AddStringId(nodeName);
			packet.AddStringId(targetNodeName);
			packet.PutFloat(attachSeconds);
			packet.PutFloat(0);
			packet.PutFloat(0);
			packet.PutFloat(0);
			packet.PutFloat(distance);
			packet.AddStringId(attachAnimation);
			packet.PutByte(b1);
			packet.PutByte(b2);
			packet.PutByte(b3);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Detach from another object (usually another actor).
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="detachFromActor"></param>
		public static void ZC_DETACH_TO_OBJ(IActor actor, IActor detachFromActor)
		{
			var packet = new Packet(Op.ZC_DETACH_FROM_OBJ);

			packet.PutInt(actor.Handle);
			packet.PutInt(detachFromActor?.Handle ?? 0);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Force's an actor to look backwards from a specific actor.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="runFromActor"></param>
		public static void ZC_RUN_FROM(IActor actor, IActor runFromActor)
		{
			var packet = new Packet(Op.ZC_RUN_FROM);

			packet.PutInt(actor.Handle);
			packet.PutInt(runFromActor?.Handle ?? 0);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Force's an actor to look at a specific actor.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="lookAtActor"></param>
		public static void ZC_LOOKAT_OBJ(IActor actor, IActor lookAtActor)
		{
			var packet = new Packet(Op.ZC_LOOKAT_OBJ);

			packet.PutInt(actor.Handle);
			packet.PutInt(lookAtActor?.Handle ?? 0);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Standing Animation ?
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="animationId"></param>
		public static void ZC_STD_ANIM(IActor actor, FixedAnimation animationId)
		{
			var packet = new Packet(Op.ZC_STD_ANIM);

			packet.PutInt(actor.Handle);
			packet.PutByte((byte)animationId);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Sends ZC_START_GAME to connection, which assumingly is the signal
		/// for the client to switch from load to map screen.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_START_GAME(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_START_GAME);

			packet.PutFloat(1); // Affects the speed of everything happening in the client o.o
			packet.PutFloat((float)ZoneServer.Instance.World.WorldTime.Elapsed.TotalSeconds); // serverAppTimeOffset
			packet.PutFloat((float)ZoneServer.Instance.ServerTime.Elapsed.TotalSeconds); // globalAppTimeOffset
			packet.PutLong(DateTime.Now.AddHours(-4).ToFileTime());
			if (Versions.Client >= 344887)
				packet.PutByte(0); // [i344887, 2021-11-09]
			if (Versions.Client >= 395869)
				packet.PutString(DateTime.Now.AddHours(-4).ToDateTimeString(), 20);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_START_INFO to connection.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_START_INFO(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_START_INFO);

			packet.PutInt(1); // count
			{
				packet.PutShort((short)conn.SelectedCharacter.JobId);
				packet.PutInt(0); // 1270153646, 2003304878
				packet.PutInt(0);
				packet.PutShort(1);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_MYPC_ENTER to character.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_MYPC_ENTER(Character character)
		{
			var packet = new Packet(Op.ZC_MYPC_ENTER);

			packet.PutFloat(character.Position.X);
			packet.PutFloat(character.Position.Y);
			packet.PutFloat(character.Position.Z);

			if (Versions.Protocol > 500)
			{
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutInt(0);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Makes character appear on connection's client, by sending ZC_ENTER_PC.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_ENTER_PC(IZoneConnection conn, DummyCharacter character)
		{
			var packet = new Packet(Op.ZC_ENTER_PC);

			packet.PutInt(character.Handle);
			packet.PutFloat(character.Position.X);
			packet.PutFloat(character.Position.Y);
			packet.PutFloat(character.Position.Z);
			packet.PutFloat(character.Direction.Cos);
			packet.PutFloat(character.Direction.Sin);
			packet.PutByte((byte)RelationType.Friendly); // Changes name color (0 = Green (Friendly?), 1 = Enemy?, 2 = Neutral?, 3 = Black
			packet.PutByte(0);
			packet.PutLong(character.SocialUserId);
			packet.PutByte(character.Pose); // Pose
			packet.PutFloat(character.MSPD);
			packet.PutFloat(character.MovingShot);
			packet.PutInt(character.Hp);
			packet.PutInt(character.MaxHp);
			packet.PutShort(character.Sp);
			packet.PutShort(character.MaxSp);
			if (Versions.Client >= 11025)
				packet.PutInt(0); // [i11025 (2016-02-26)]
			if (Versions.Client >= 373230)
				packet.PutInt(0); // [i373230 (2023-05-10)]
			packet.PutInt(character.Stamina);
			packet.PutInt(character.MaxStamina);
			packet.PutByte(0);
			packet.PutShort(0);
			packet.PutShort(0);
			packet.PutShort(0); // Seen values: 7
			packet.PutInt(-1); // titleAchievementId
			packet.PutByte(0);
			packet.AddAppearancePc(character);
			if (Versions.Client == KnownVersions.ClosedBeta1)
				packet.PutString("None", 49);
			else
				packet.PutInt(0);
			packet.PutFloat((float)ZoneServer.Instance.World.WorldTime.Elapsed.TotalSeconds);
			packet.PutByte(0);
			if (Versions.Client >= 381165)
				packet.PutByte(0); // [i381165 (2024-01-09)]

			// [i381490 (2023-12-XX)]
			// The new byte should be either this or the previous one.
			// packet.PutByte(0); [Removed 2024-03-07]

			conn.Send(packet);
		}

		/// <summary>
		/// Makes character appear on connection's client, by sending ZC_ENTER_PC.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_ENTER_PC(IZoneConnection conn, Character character)
		{
			var relationship = (byte)Math.Clamp((int)conn.SelectedCharacter.GetRelation(character), 0, 2);

			var packet = new Packet(Op.ZC_ENTER_PC);

			packet.PutInt(character.Handle);
			packet.PutFloat(character.Position.X);
			packet.PutFloat(character.Position.Y);
			packet.PutFloat(character.Position.Z);
			packet.PutFloat(character.Direction.Cos);
			packet.PutFloat(character.Direction.Sin);
			packet.PutByte(relationship); // Changes name color (0 = Green (Friendly?), 1 = Enemy?, 2 = Neutral?, 3 = Black
			packet.PutByte(0);
			packet.PutLong(character.SocialUserId);
			packet.PutByte(character.Pose); // Pose
			packet.PutFloat(character.MSPD);
			packet.PutFloat(character.MovingShot);
			packet.PutInt(character.Hp);
			packet.PutInt(character.MaxHp);
			packet.PutShort(character.Sp);
			packet.PutShort(character.MaxSp);
			if (Versions.Client >= 11025)
				packet.PutInt(0); // [i11025 (2016-02-26)]
			if (Versions.Client >= 373230)
				packet.PutInt(0); // [i373230 (2023-05-10)]
			packet.PutInt(character.Stamina);
			packet.PutInt(character.MaxStamina);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte(character.IsSitting);
			if (Versions.Client >= 364853)
			{
				packet.PutInt(character.EquippedTitleId); // Equippable Title Id
				packet.PutInt(-1); // titleAchievementId
			}
			else
			{
				packet.PutInt(-1); // titleAchievementId
				packet.PutInt(0);
			}
			packet.PutByte(0);
			packet.AddAppearancePc(character);
			if (Versions.Client < KnownVersions.OpenBeta)
			{
				if (character.Connection.Party != null)
					packet.PutString(character.Connection.Party.Name, 49);
				else
					packet.PutString("None", 49);
			}
			if (Versions.Protocol > 500)
			{
				packet.PutInt(0);
				packet.PutFloat((float)ZoneServer.Instance.World.WorldTime.Elapsed.TotalSeconds);
				packet.PutByte(0);
			}
			if (Versions.Client >= 381165)
				packet.PutByte(0); // [i381165 (2024-01-09)]

			// [i381490 (2023-12-XX)]
			// The new byte should be either this or the previous one.
			// packet.PutByte(0); [Removed 2024-03-07]
			if (Versions.Client >= 381490 && Versions.Client < 383360)
				packet.PutByte(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_ENTER_MONSTER to connection, making it appear.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="monster"></param>
		public static void ZC_ENTER_MONSTER(IZoneConnection conn, IMonster monster)
		{
			var packet = new Packet(Op.ZC_ENTER_MONSTER);
			packet.AddMonster(monster);

			conn.Send(packet);
		}

		/// <summary>
		/// Broadcasts ZC_ENTER_MONSTER on monster's map, making it appear.
		/// </summary>
		/// <param name="monster"></param>
		public static void ZC_ENTER_MONSTER(IMonster monster)
		{
			var packet = new Packet(Op.ZC_ENTER_MONSTER);
			packet.AddMonster(monster);

			monster.Map.Broadcast(packet, monster);
		}

		/// <summary>
		/// Adds a session object and its properties.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="sessionObject"></param>
		/// <param name="questId"></param>
		public static void ZC_SESSION_OBJ_ADD(Character character, SessionObject sessionObject, int questId = 0)
		{
			var propertyList = sessionObject.Properties.GetAll();
			var propertiesSize = propertyList.GetByteCount();

			var packet = new Packet(Op.ZC_SESSION_OBJ_ADD);
			packet.PutInt(sessionObject.Id);
			packet.PutInt(0);
			packet.PutLong(sessionObject.ObjectId);
			packet.PutInt(0);
			packet.PutShort(propertiesSize);
			packet.PutShort(0);
			packet.AddProperties(propertyList);
			packet.PutInt(questId);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Remove Session Object
		/// </summary>
		/// <param name="character"></param>
		/// <param name="sessionId"></param>
		public static void ZC_SESSION_OBJ_REMOVE(Character character, int sessionId)
		{
			var packet = new Packet(Op.ZC_SESSION_OBJ_REMOVE);
			packet.PutInt(sessionId);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Exact purpose unknown, but it stops the animation of Multishot.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_SKILL_DISABLE(IActor actor)
		{
			var packet = new Packet(Op.ZC_SKILL_DISABLE);


			packet.PutInt(actor.Handle);
			packet.PutByte(0);
			packet.PutInt(0); // Random number, presumably unused


			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Sends the player's saved hotkeys to the client.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_QUICK_SLOT_LIST(Character character)
		{
			// If no hotkeys were saved yet, we don't need to send anything.
			var quickSlotRows = character.Variables.Perm.Get<byte>("Melia.QuickSlotRows", 20);
			var serialized = character.Variables.Perm.Get<string>("Melia.QuickSlotList",
				"#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,0,0#None,1,0#None,2,0#None,3,0#");

			var packet = new Packet(Op.ZC_QUICK_SLOT_LIST);

			if (Versions.Protocol > 500)
			{
				var compressedData = packet.CompressData(p =>
				{
					var quickSlotsStr = serialized.Split(['#'], StringSplitOptions.RemoveEmptyEntries);

					p.PutByte(quickSlotRows);

					for (var i = 0; i < 50; ++i)
					{
						var split = quickSlotsStr[i].Split([','], StringSplitOptions.RemoveEmptyEntries);

						var type = Enum.Parse<QuickSlotType>(split[0]);
						var id = int.Parse(split[1]);
						var objectId = long.Parse(split[2]);

						if (type == QuickSlotType.Item)
						{
							if (character.Inventory.TryGetItemOrEquip(objectId, out var item) && item.Id == id)
							{
								objectId = item.ObjectId;
							}
							else
							{
								item = character.Inventory.GetItems().Values.FirstOrDefault(item => item.Id == id);
								if (item != null)
								{
									objectId = item.ObjectId;
								}
								else
								{
									item = character.Inventory.GetEquip().Values.FirstOrDefault(item => item.Id == id);

									if (item != null)
									{
										objectId = item.ObjectId;
									}
									else
									{
										type = QuickSlotType.None;
										id = 0;
										objectId = 0;
									}
								}
							}
						}

						p.PutByte((byte)type);
						p.PutInt(id);
						p.PutLong(objectId);
					}

					for (var i = 0; i < 4; ++i)
					{
						var split = quickSlotsStr[i].Split([','], StringSplitOptions.RemoveEmptyEntries);

						var type = Enum.Parse(typeof(QuickSlotType), split[0]);
						var id = int.Parse(split[1]);
						var objectId = long.Parse(split[2]);

						p.PutByte((byte)type);
						p.PutInt(id);
						p.PutLong(objectId);
					}

					p.PutByte(0);
					p.PutByte(0);
				});

				packet.PutInt(compressedData.Length);
				packet.PutByte(0);
				packet.PutBin(compressedData);
			}
			else
			{
				packet.PutInt(0);
				packet.PutShort(0);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_SKILL_LIST to character, containing a list
		/// of all the character's skills.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SKILL_LIST(Character character)
		{
			// [Note]
			// If you ever find yourself wondering why the client isn't
			// displaying any skills, make sure the NormalOp for the skill
			// UI update is correct (ZC_NORMAL_UpdateSkillUI).
			// Without that packet, the client won't display any skills
			// because it contains some job information that are used
			// to determin which skills to display.

			foreach (var skill in ZoneServer.Instance.Data.SkillDb.FindAll(s => s.Name == "Basic Attack"))
			{
				character.Skills.AddSilent(new Skill(character, skill.Id));
			}

			var skills = character.Skills.GetList();

			var packet = new Packet(Op.ZC_SKILL_LIST);
			packet.PutInt(character.Handle);
			packet.PutShort(skills.Length);
			if (Versions.Protocol > 500)
				packet.PutByte(0);

			packet.Zlib(true, zpacket =>
			{
				foreach (var skill in skills)
					zpacket.AddSkill(skill);
			});

			// This follows at the end of the packet after the skills,
			// but it's not clear what this is for. It honestly seems
			// like garbage data, with left over floats and properties,
			// but it's a bit long for that. A couple dozens updates
			// ago there were also only two bytes before the properties,
			// but one garbage byte at the end of the packet to pad it.
			// We'll just mimic the official packets for now.
			//
			// Alternative theory: It could also be a string that wasn't
			// zeroed, and maybe a few bytes or something.
			if (Versions.Protocol > 500)
			{
				packet.PutByte(0x00);
				packet.PutByte(0x80);
				packet.PutByte(0x3F);
				packet.PutInt(PropertyTable.GetId("Skill", "SkillFactor"));
				packet.PutFloat(1);
				packet.PutInt(PropertyTable.GetId("Skill", "CaptionTime"));
				packet.PutFloat(0);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Adds skill for character, by sending ZC_SKILL_ADD to its connection.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skill"></param>
		public static void ZC_SKILL_ADD(Character character, Skill skill)
		{
			// Passive skills aren't added to the quickbar
			var addToQuickbar = skill.Data.ActivationType == SkillActivationType.ActiveSkill;

			var packet = new Packet(Op.ZC_SKILL_ADD);

			if (Versions.Protocol > 500)
				packet.PutLong(character.ObjectId);
			packet.PutByte(addToQuickbar);
			packet.PutByte(0); // SKILL_LIST_GET ?
			packet.PutLong(0); // ?
			packet.AddSkill(skill);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Cancels a skill cast, usually sent when a monster has died.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_SKILL_CAST_CANCEL(IActor actor)
		{
			var packet = new Packet(Op.ZC_SKILL_CAST_CANCEL);

			packet.PutInt(actor.Handle);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Cancels a skill cast, usually sent when a monster has died.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_SKILL_USE_CANCEL(IActor actor)
		{
			var packet = new Packet(Op.ZC_SKILL_USE_CANCEL);

			packet.PutInt(actor.Handle);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Shows skill use for character, by sending ZC_SKILL_FORCE_TARGET to its connection.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_FORCE_TARGET(ICombatEntity entity, ICombatEntity target, Skill skill, params SkillHitInfo[] hits)
			=> ZC_SKILL_FORCE_TARGET(entity, target, skill, (IEnumerable<SkillHitInfo>)hits);

		/// <summary>
		/// Shows skill use for character, by sending ZC_SKILL_FORCE_TARGET to its connection.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_FORCE_TARGET(ICombatEntity entity, ICombatEntity target, Skill skill, IEnumerable<SkillHitInfo> hits)
			=> ZC_SKILL_FORCE_TARGET(entity, target, skill, hits?.FirstOrDefault()?.ForceId ?? 0, hits);

		/// <summary>
		/// Shows skill use for character, by sending ZC_SKILL_FORCE_TARGET to its connection.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="forceId"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_FORCE_TARGET(ICombatEntity entity, ICombatEntity target, Skill skill, int forceId, IEnumerable<SkillHitInfo> hits)
		{
			var shootTime = skill.Properties.GetFloatSafe(PropertyName.ShootTime);
			var sklSpdRate = skill.Properties.GetFloatSafe(PropertyName.SklSpdRate);

			var packet = new Packet(Op.ZC_SKILL_FORCE_TARGET);

			packet.PutInt((int)skill.Id);
			packet.PutInt(entity.Handle);
			packet.PutFloat(entity.Direction.Cos);
			packet.PutFloat(entity.Direction.Sin);
			if (Versions.Protocol > 500)
				packet.PutInt(1);
			else
				packet.PutInt(-255);
			packet.PutFloat(shootTime);
			packet.PutFloat(1);
			packet.PutInt(0);
			packet.PutInt(forceId);
			packet.PutFloat(sklSpdRate);

			if (Versions.Protocol > 500)
				packet.PutInt(0);
			packet.PutInt(target?.Handle ?? 0);
			packet.PutInt(0);
			packet.PutFloat(512f);
			packet.PutFloat(hits?.FirstOrDefault()?.UnkFloat ?? 0);

			packet.PutByte((byte)(hits?.Count() ?? 0));
			if (hits != null)
			{
				foreach (var hit in hits)
					packet.AddSkillHitInfo(hit);
			}

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Shows skill use for character, but allows substituting a different
		/// skill id for the visual effects. Mainly intended for custom skills
		/// and the like.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="visualSkillId"></param>
		/// <param name="forceId"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_FORCE_TARGET_DUMMY(ICombatEntity entity, ICombatEntity target, Skill skill, SkillId visualSkillId, int forceId, IEnumerable<SkillHitInfo> hits)
		{
			var shootTime = skill.Properties.GetFloat(PropertyName.ShootTime);
			var sklSpdRate = skill.Properties.GetFloat(PropertyName.SklSpdRate);

			var packet = new Packet(Op.ZC_SKILL_FORCE_TARGET);

			packet.PutInt((int)visualSkillId);
			packet.PutInt(entity.Handle);
			packet.PutFloat(entity.Direction.Cos);
			packet.PutFloat(entity.Direction.Sin);
			packet.PutInt(1);
			packet.PutFloat(shootTime);
			packet.PutFloat(1);
			packet.PutInt(0);
			packet.PutInt(forceId);
			packet.PutFloat(sklSpdRate);

			packet.PutInt(0);
			packet.PutInt(target?.Handle ?? 0);
			packet.PutInt(0);
			packet.PutFloat(512f);
			packet.PutInt(0);

			packet.PutByte((byte)(hits?.Count() ?? 0));
			if (hits != null)
			{
				foreach (var hit in hits)
					packet.AddSkillHitInfo(hit);
			}

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Shows entity using the skill.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skill"></param>
		/// <param name="targetPos"></param>
		/// <param name="forceId"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_FORCE_GROUND(ICombatEntity entity, Skill skill, Position targetPos, int forceId, IEnumerable<SkillHitInfo> hits)
		{
			var shootTime = skill.Properties.GetFloat(PropertyName.ShootTime);
			var sklSpdRate = skill.Properties.GetFloat(PropertyName.SklSpdRate);

			var packet = new Packet(Op.ZC_SKILL_FORCE_GROUND);

			packet.PutInt((int)skill.Id);
			packet.PutInt(entity.Handle);
			packet.PutFloat(entity.Direction.Cos);
			packet.PutFloat(entity.Direction.Sin);
			packet.PutInt(1);
			packet.PutFloat(shootTime);
			packet.PutFloat(1);
			packet.PutInt(0);
			packet.PutInt(forceId);
			packet.PutFloat(sklSpdRate);

			// This does _not_ look like a handle to me... And sending a
			// single target handle for an AoE skill packet doesn't make
			// sense either. Let's send 0 for now.
			//if (targets != null && targetCount == 1)
			//	packet.PutInt(targets.First().Handle);
			//else
			packet.PutInt(0);
			packet.PutInt(0);

			packet.PutPosition(targetPos);

			packet.PutInt(0);
			packet.PutFloat(512);
			packet.PutInt(0);

			// Not too sure if this packet does hits, since it looks a lot
			// like the ZC_SKILL_MELEE_GROUND
			packet.PutByte(0);
			//packet.PutByte((byte)(hits?.Count() ?? 0));
			//if (hits != null)
			//{
			//foreach (var hit in hits)
			//packet.AddSkillHitInfo(hit);
			//}

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Shows entity using the skill.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skill"></param>
		/// <param name="targetPos"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_MELEE_GROUND(ICombatEntity entity, Skill skill, Position targetPos, params SkillHitInfo[] hits)
			=> ZC_SKILL_MELEE_GROUND(entity, skill, targetPos, (IEnumerable<SkillHitInfo>)hits);

		/// <summary>
		/// Shows entity using the skill.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skill"></param>
		/// <param name="targetPos"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_MELEE_GROUND(ICombatEntity entity, Skill skill, Position targetPos, IEnumerable<SkillHitInfo> hits)
			=> ZC_SKILL_MELEE_GROUND(entity, skill, targetPos, hits?.FirstOrDefault()?.ForceId ?? 0, hits);

		/// <summary>
		/// Shows entity using the skill.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skill"></param>
		/// <param name="targetPos"></param>
		/// <param name="forceId"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_MELEE_GROUND(ICombatEntity entity, Skill skill, Position targetPos, int forceId, IEnumerable<SkillHitInfo> hits)
		{
			var shootTime = skill.Properties.GetFloat(PropertyName.ShootTime);
			var sklSpdRate = skill.Properties.GetFloatSafe(PropertyName.SklSpdRate);
			var enableCastMove = skill.Properties.GetFloat(PropertyName.EnableShootMove) == 1f;

			var packet = new Packet(Op.ZC_SKILL_MELEE_GROUND);

			packet.PutInt((int)skill.Id);
			packet.PutInt(entity.Handle);
			packet.PutFloat(entity.Direction.Cos);
			packet.PutFloat(entity.Direction.Sin);
			packet.PutInt(1);
			packet.PutFloat(shootTime);
			packet.PutFloat(1);

			// Previously (Int(0))
			packet.PutByte(enableCastMove); // Move while cast animation happens
			packet.PutByte(0); // Not too sure what this one does, but was also set to 1 on Elemental Burst
			packet.PutShort(0);

			packet.PutInt(forceId);
			packet.PutFloat(sklSpdRate);

			// This does _not_ look like a handle to me... And sending a
			// single target handle for an AoE skill packet doesn't make
			// sense either. Let's send 0 for now.
			//if (targets != null && targetCount == 1)
			//	packet.PutInt(targets.First().Handle);
			//else
			if (Versions.Protocol > 500)
				packet.PutInt(0);

			packet.PutPosition(targetPos);

			packet.PutShort((short)(hits?.Count() ?? 0));
			if (hits != null)
			{
				foreach (var hit in hits)
					packet.AddSkillHitInfo(hit);
			}

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Shows entity using the skill.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skill"></param>
		/// <param name="target"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_MELEE_TARGET(ICombatEntity entity, Skill skill, ICombatEntity target, params SkillHitInfo[] hits)
			=> ZC_SKILL_MELEE_TARGET(entity, skill, target, (IEnumerable<SkillHitInfo>)hits);

		/// <summary>
		/// Shows entity using the skill on the target.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skill"></param>
		/// <param name="target"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_MELEE_TARGET(ICombatEntity entity, Skill skill, ICombatEntity target, IEnumerable<SkillHitInfo> hits)
		{
			var shootTime = skill.Properties.GetFloat(PropertyName.ShootTime);
			var sklSpdRate = skill.Properties.GetFloat(PropertyName.SklSpdRate);
			var forceId = hits?.FirstOrDefault()?.ForceId ?? 0;

			var packet = new Packet(Op.ZC_SKILL_MELEE_TARGET);

			packet.PutInt((int)skill.Id);
			packet.PutInt(entity.Handle);
			packet.PutFloat(entity.Direction.Cos);
			packet.PutFloat(entity.Direction.Sin);
			packet.PutInt(1);
			packet.PutFloat(shootTime);
			packet.PutFloat(1);
			packet.PutInt(0);
			packet.PutInt(forceId);
			packet.PutFloat(sklSpdRate);

			// This does _not_ look like a handle to me... And sending a
			// single target handle for an AoE skill packet doesn't make
			// sense either. Let's send 0 for now.
			//if (targets != null && targetCount == 1)
			//	packet.PutInt(targets.First().Handle);
			//else
			packet.PutInt(0);

			packet.PutInt(target.Handle);

			packet.PutByte((byte)(hits?.Count() ?? 0));
			if (hits != null)
			{
				foreach (var hit in hits)
					packet.AddSkillHitInfo(hit);
			}

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Updates the skill's overheat counter.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skill"></param>
		public static void ZC_OVERHEAT_CHANGED(Character character, Skill skill)
		{
			// Melia uses OvearheatResetTime, but in Laima we patched
			// the client to use all overheat reset times equal to the skill's
			// default cooldown time. This simpler system allows us to customize
			// skills overheats without having to constantly change cooldown.ies
			//var resetTime = skill.OverheatData.OverheatResetTime.TotalMilliseconds;
			var resetTime = skill.Data.CooldownTime.TotalMilliseconds;

			// The "overheat time" needs to be "one resetTime" more than
			// one would expect to display the correct amount of overheat
			// on the client.
			// For example, if the skill has a reset time of 10,000, and
			// has one overheat count, the client needs 20,000 to display
			// one bubble on the skill icon.
			var overheatCount = skill.OverheatCounter == 0 ? 0 : skill.OverheatCounter + 1;
			var overheatTime = overheatCount * resetTime;

			var packet = new Packet(Op.ZC_OVERHEAT_CHANGED);

			packet.PutLong(character.ObjectId);
			packet.PutInt((int)skill.Data.OverheatGroup);
			if (Versions.Protocol > 500)
			{
				packet.PutInt((int)overheatTime);
				packet.PutInt(0);
				packet.PutInt((int)resetTime);
				packet.PutByte(0);
				packet.PutByte(0xFF);
				packet.PutByte(0xFF);
				packet.PutByte(0xFF);
			}
			packet.PutLong(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates a cooldown on the character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="cooldown"></param>
		public static void ZC_COOLDOWN_CHANGED(Character character, Cooldown cooldown)
		{
			var packet = new Packet(Op.ZC_COOLDOWN_CHANGED);

			packet.PutLong(character.ObjectId);
			packet.PutInt((int)cooldown.Id);
			if (Versions.Protocol > 500)
				packet.PutInt((int)cooldown.Remaining.TotalMilliseconds);
			packet.PutInt((int)cooldown.Duration.TotalMilliseconds);
			if (Versions.Protocol > 500)
				packet.PutByte(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates a cooldown on the character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="cooldown"></param>
		public static void ZC_COOLDOWN_CHANGED(Character character, CooldownId cooldown)
		{
			var packet = new Packet(Op.ZC_COOLDOWN_CHANGED);

			packet.PutLong(character.ObjectId);
			packet.PutInt((int)cooldown);
			if (Versions.Protocol > 500)
				packet.PutInt((int)TimeSpan.Zero.TotalMilliseconds);
			packet.PutInt((int)TimeSpan.Zero.TotalMilliseconds);
			if (Versions.Protocol > 500)
				packet.PutByte(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_CUSTOM_DIALOG to connection, containing the name of the
		/// shop to open.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="function"></param>
		public static void ZC_CUSTOM_DIALOG(IZoneConnection conn, string function, string dialogStr = "", int argCount = 0)
		{
			var packet = new Packet(Op.ZC_CUSTOM_DIALOG);

			packet.PutString(function, 33);
			packet.PutString(dialogStr, 32);
			packet.PutInt(argCount);

			conn.Send(packet);
		}

		/// <summary>
		/// ? sent after party is created related /memberInfoForAct?
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_TO_SOMEWHERE_CLIENT(Character character)
		{
			var party = character.Connection.Party;
			var packet = new Packet(Op.ZC_TO_SOMEWHERE_CLIENT);
			packet.PutLong(0);
			packet.PutInt(1);
			packet.PutInt(1);
			packet.PutLpString(character.TeamName);

			packet.Zlib(true, zpacket =>
			{
				zpacket.PutInt(0xDC2);
				zpacket.PutShort(0);
				zpacket.PutShort(0);
				zpacket.PutLong(1000555709005824);
				zpacket.PutString(character.TeamName, 65);
				zpacket.PutLong(party?.ObjectId ?? 0);
				zpacket.PutLong(character.Connection.Account.ObjectId);
				zpacket.PutString(character.TeamName, 64);
				zpacket.PutString(character.Name, 64);
				zpacket.PutShort(0);
				zpacket.PutShort((short)character.JobId);
				zpacket.PutInt((int)character.JobId);
				zpacket.PutInt(3);
				zpacket.PutByte(0x80);
				zpacket.PutByte(0x80);
				zpacket.PutByte(0x80);
				zpacket.PutByte(0xFF);
				zpacket.PutEmptyBin(12);
				zpacket.PutShort(0); // Properties Size
				zpacket.PutShort(0); // ETC Properties Size
				zpacket.PutShort(character.Jobs.Count);
				zpacket.PutInt((int)character.JobId);
				zpacket.PutInt(0);
				zpacket.PutLong(404);
				zpacket.PutLong(11632643);
				zpacket.PutLong(0x0D);
				zpacket.PutLong(0x27);
			});

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_SKILLMAP_LIST to character.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SKILLMAP_LIST(Character character)
		{
			var packet = new Packet(Op.ZC_SKILLMAP_LIST);

			packet.PutInt(0); // ?

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_OPTION_LIST to client, containing the saved
		/// account options, like "Show Exp Aquired".
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_OPTION_LIST(IZoneConnection conn)
		{
			// Officials don't always send all options, but only the ones
			// that were changed from their default values, resulting in
			// an empty string in this packet if no options were changed
			// yet. We could technically do that as well, but we'd need
			// an options db, for the defaults. And it's one packet with
			// 500 bytes, that's sent once on login. Who cares?

			var packet = new Packet(Op.ZC_OPTION_LIST);
			packet.PutString(conn.Account.Settings.ToString());

			conn.Send(packet);
		}

		/// <summary>
		/// Enable HUD/Disable HUD
		/// </summary>
		/// <param name="character"></param>
		/// <param name="layer"></param>
		/// <param name="enabled">layer needs to be set to 0 to turn off</param>
		public static void ZC_SET_LAYER(Character character, int layer, bool enabled)
		{
			var packet = new Packet(Op.ZC_SET_LAYER);
			packet.PutInt(layer);
			packet.PutByte(enabled);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_ACHIEVE_POINT_LIST to character.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_ACHIEVE_POINT_LIST(Character character)
		{
			var packet = new Packet(Op.ZC_ACHIEVE_POINT_LIST);

			// Shared amongst the account
			packet.PutShort(0); // Achievement Count
			packet.PutShort(0); // Title Count?
			/**
			// TODO enable when achievements are added.
			foreach (var achievementPoint in conn.Account.AchievementPoints)
			{
				packet.PutInt(achievementPoint.id);
				packet.PutInt(achievementPoint.count);
			}
			foreach (var achievement in conn.Account.Achievements)
				packet.PutInt(achievement);
			**/

			character.Connection.Send(packet);
		}

		public static void ZC_WORLD_MSG(Character character, int achievementId, int achievementPointId, int achievementPointValue)
		{
			var packet = new Packet(Op.ZC_WORLD_MSG);

			packet.PutInt(0);
			packet.PutString(character.Name, 64);
			packet.PutInt(achievementId);
			packet.PutInt(achievementPointId);
			packet.PutInt(achievementPointValue);
			packet.PutByte(0);

			ZoneServer.Instance.World.Broadcast(packet);
		}

		/// <summary>
		/// Send Achievement Points Update to client
		/// </summary>
		/// <param name="character"></param>
		/// <param name="achievementPointId"></param>
		/// <param name="achievementPoints"></param>
		public static void ZC_ACHIEVE_POINT(Character character, int achievementPointId, int achievementPoints, int achievementId)
		{
			var packet = new Packet(Op.ZC_ACHIEVE_POINT);

			packet.PutInt(achievementPointId);
			packet.PutInt(achievementPoints);
			packet.PutInt(achievementId);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Equip an achievement title
		/// </summary>
		/// <param name="character"></param>
		/// <param name="achievementId"></param>
		public static void ZC_ACHIEVE_EQUIP(Character character, int achievementId)
		{
			var packet = new Packet(Op.ZC_ACHIEVE_EQUIP);

			packet.PutLong(0);
			packet.PutInt(0);
			packet.PutInt(character.Handle);
			packet.PutInt(character.EquippedTitleId);
			packet.PutInt(-1);

			character.Map.Broadcast(packet);
		}

		/// <summary>
		/// Sends chat macros to the character.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_CHAT_MACRO_LIST(Character character)
		{
			var packet = new Packet(Op.ZC_CHAT_MACRO_LIST);

			var macros = character.Connection.Account.GetChatMacros();

			packet.PutInt(macros.Length);
			foreach (var macro in macros)
			{
				packet.PutInt(macro.Index);
				packet.PutString(macro.Message, 128);
				packet.PutInt(macro.Pose);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Register a quick slot.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="type"></param>
		/// <param name="id"></param>
		/// <param name="slot"></param>
		public static void ZC_QUICKSLOT_REGISTER(IZoneConnection conn, QuickSlotType type, int id, int slot)
		{
			var packet = new Packet(Op.ZC_QUICKSLOT_REGISTER);

			packet.PutString(type.ToString(), 32);
			packet.PutInt(id);
			packet.PutInt(slot);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_NPC_STATE_LIST to character.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_NPC_STATE_LIST(Character character)
		{
			var packet = new Packet(Op.ZC_NPC_STATE_LIST);

			var npcs = character.Map.GetNpcs(a => a.State == NpcState.Highlighted);
			var npcCount = npcs?.Count() ?? 0;

			packet.PutInt(npcCount);
			if (Versions.Protocol > 500)
				packet.PutShort(0);

			if (npcs != null)
			{
				packet.Zlib(true, zpacket =>
				{
					foreach (var npc in npcs)
					{
						zpacket.PutInt(character.MapId);
						zpacket.PutInt(npc.GenType);
						zpacket.PutShort((short)npc.State);
						zpacket.PutShort(0);
					}
				});
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates an NPC's state on a specific client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="npc"></param>
		public static void ZC_SET_NPC_STATE(IZoneConnection conn, MonsterInName npc, short state)
		{
			var packet = new Packet(Op.ZC_SET_NPC_STATE);

			packet.PutInt(npc.Map.Id);
			packet.PutInt(npc.GenType);
			packet.PutShort(state);
			packet.PutEmptyBin(2);

			conn.Send(packet);
		}

		/// <summary>
		/// Updates an NPC's state on all clients in range of it.
		/// </summary>
		/// <param name="npc"></param>
		public static void ZC_SET_NPC_STATE(MonsterInName npc)
		{
			var packet = new Packet(Op.ZC_SET_NPC_STATE);

			packet.PutInt(npc.Map.Id);
			packet.PutInt(npc.GenType);
			packet.PutShort((short)npc.State);
			packet.PutEmptyBin(2);

			npc.Map.Broadcast(packet, npc, false);
		}

		/// <summary>
		/// Updates an NPC's state on all clients in range of it.
		/// </summary>
		/// <param name="npc"></param>
		public static void ZC_SET_NPC_STATE(Npc npc)
		{
			var packet = new Packet(Op.ZC_SET_NPC_STATE);

			packet.PutInt(npc.Map.Id);
			packet.PutInt(npc.GenType);
			packet.PutShort((short)npc.State);
			packet.PutEmptyBin(2);

			npc.Map.Broadcast(packet, npc, false);
		}

		/// <summary>
		/// Sends a list of cooldowns to the client to update them.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="cooldowns"></param>
		public static void ZC_COOLDOWN_LIST(Character character, IEnumerable<Cooldown> cooldowns)
		{
			var packet = new Packet(Op.ZC_COOLDOWN_LIST);

			packet.PutLong(character.ObjectId);
			packet.PutInt(cooldowns?.Count() ?? 0);

			if (cooldowns != null)
			{
				foreach (var cooldown in cooldowns)
				{
					packet.PutInt((int)cooldown.Id);
					if (Versions.Protocol > 500)
						packet.PutInt((int)cooldown.Remaining.TotalMilliseconds);
					packet.PutInt((int)cooldown.Duration.TotalMilliseconds);
				}
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_JOB_PTS to character, updating their job points.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="job"></param>
		public static void ZC_JOB_PTS(Character character, Job job)
		{
			var packet = new Packet(Op.ZC_JOB_PTS);

			if (Versions.Protocol > 500)
				packet.PutLong(character.ObjectId);
			packet.PutShort((short)job.Id);
			packet.PutShort((short)job.SkillPoints);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_ABILITY_LIST to character, containing a list of all
		/// their abilities.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_ABILITY_LIST(Character character)
		{
			var abilities = character.Abilities.GetList();
			if (true)
			{
				var abilityData = ZoneServer.Instance.Data.AbilityDb.FindAll(a => a.Id >= AbilityId.SwapWeapon && a.Id <= AbilityId.CompanionRide);
				var abilityTreeData = ZoneServer.Instance.Data.AbilityTreeDb.FindAll(a => a.JobId == character.JobId && !a.HasUnlockScript);
				foreach (var ability in abilityData)
				{
					character.Abilities.AddSilent(new Ability(ability.Id, 1));
				}
				foreach (var ability in abilityTreeData)
				{
					character.Abilities.AddSilent(new Ability(ability.AbilityId, 1));
				}
				abilities = character.Abilities.GetList();
			}

			var packet = new Packet(Op.ZC_ABILITY_LIST);

			if (Versions.Protocol > 500)
			{
				packet.PutShort(0);
				packet.PutByte(1);
				packet.PutInt(abilities.Length);

				foreach (var ability in abilities)
				{
					//var propertyList = ability.Properties.GetAll();
					//var propertiesSize = propertyList.GetByteCount();

					packet.PutLong(ability.ObjectId);
					packet.PutInt((int)ability.Id);
					packet.PutShort(0); // propertiesSize
					packet.PutShort(0);

					// The ability properties are weird. Not only do they
					// seem to use a count instead of a size now and include
					// some byte, but they also don't appear unless one of
					// them has a non-default value? Without properties the
					// ability is displayed as level 1.

					//if (propertiesSize > 0)
					//	packet.AddProperties(propertyList);

					var sendProperties = ability.Level > 1 || !ability.Active;

					if (!sendProperties)
					{
						packet.PutInt(0);
					}
					else
					{
						var propertyList = ability.Properties.GetAll();

						packet.PutInt(propertyList.Count);
						foreach (var property in propertyList)
						{
							var propertyId = PropertyTable.GetId("Ability", property.Ident);

							packet.PutInt(propertyId);
							packet.PutByte(0);

							switch (property)
							{
								case FloatProperty floatProperty:
									packet.PutFloat(floatProperty.Value);
									break;

								case StringProperty stringProperty:
									packet.PutLpString(stringProperty.Value);
									break;

								default:
									throw new ArgumentException($"Unknown property type: {property.GetType().Name}");
							}
						}
					}
				}
			}
			else
			{
				packet.PutInt(character.Handle);
				packet.PutShort(abilities.Length);

				packet.Zlib(false, zpacket =>
				{
					foreach (var ability in abilities)
					{
						var propertyList = ability.Properties.GetAll();
						var propertiesSize = propertyList.GetByteCount();

						zpacket.PutLong(ability.ObjectId);
						zpacket.PutInt((int)ability.Id);
						zpacket.PutShort(propertiesSize);
						zpacket.PutShort(0); // alignment bytes

						if (propertiesSize > 0)
							zpacket.AddProperties(propertyList);
					}
					zpacket.PutByte(0);
					// Maybe Common Abilities?
				});
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Broadcasts ZC_MOVE_SPEED in range of character, updating their move speed.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_MOVE_SPEED(ICombatEntity actor, float f1 = 0)
		{
			var packet = new Packet(Op.ZC_MOVE_SPEED);

			packet.PutInt(actor.Handle);
			packet.PutFloat(actor.Properties.GetFloat(PropertyName.MSPD));
			packet.PutFloat(actor.Properties.GetFloat(PropertyName.MovingShot));

			// [i11257 (2016-03-25)]
			if (Versions.Client >= 11257)
			{
				packet.PutByte(0);
			}
			if (Versions.Protocol > 500)
			{
				// Because all ICombatEntity's don't have an object id, this is here.
				if (actor is IPropertyObject obj)
					packet.PutLong(obj.ObjectId);
				else
					packet.PutLong(0);
				//packet.PutLong(character.ObjectId);
			}


			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Broadcasts ZC_CASTING_SPEED in range of character, updating their casting speed.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_CASTING_SPEED(Character character)
		{
			if (Versions.Client <= KnownVersions.ClosedBeta1)
				return;

			var packet = new Packet(Op.ZC_CASTING_SPEED);

			packet.PutInt(character.Handle);
			packet.PutFloat(character.Properties.GetFloat(PropertyName.CastingSpeed));
			packet.PutLong(character.ObjectId);

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Send ZC_LEAVE_TRIGGER after dialog close.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_LEAVE_TRIGGER(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_LEAVE_TRIGGER);

			conn.Send(packet);
		}

		/// <summary>
		/// Send Achievements Unlocked
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SPLIT_ACHIEVE_POINT_LIST(Character character)
		{
			var pointIds = character.Achievements.GetPointIds();
			var packet = new Packet(Op.ZC_SPLIT_ACHIEVE_POINT_LIST);

			packet.PutShort(pointIds.Length);
			packet.PutByte(1);
			foreach (var pointId in pointIds)
			{
				packet.PutInt(pointId);
				packet.PutInt(character.Achievements.GetPoints(pointId));
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send Achievements Unlocked
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SPLIT_ACHIEVE_SET(Character character)
		{
			var achievements = character.Achievements.GetAchievements();
			var packet = new Packet(Op.ZC_SPLIT_ACHIEVE_SET);

			packet.PutShort(achievements.Length);
			packet.PutByte(1);
			packet.PutByte(1);
			foreach (var achievement in achievements)
				packet.PutInt(achievement);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_ITEM_INVENTORY_LIST to character, containing a list of
		/// all items in their inventory.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_ITEM_INVENTORY_LIST(Character character)
		{
			var items = character.Inventory.GetItems();

			var packet = new Packet(Op.ZC_ITEM_INVENTORY_LIST);

			packet.PutInt(items.Count);
			packet.Zlib(true, zpacket =>
			{
				foreach (var item in items)
				{
					var propertyList = item.Value.Properties.GetAll();
					var propertiesSize = propertyList.GetByteCount();

					zpacket.PutInt(item.Value.Id);
					zpacket.PutShort(propertiesSize);
					zpacket.PutEmptyBin(2);
					zpacket.PutLong(item.Value.ObjectId);
					zpacket.PutInt(item.Value.Amount);
					zpacket.PutInt(item.Value.Price);
					zpacket.PutInt(item.Key);
					zpacket.PutInt(1);
					zpacket.AddProperties(propertyList);
				}
			});

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_ITEM_INVENTORY_DIVISION_LIST to character, containing
		/// a list of all items in their inventory, split into pages of 10 items.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_ITEM_INVENTORY_DIVISION_LIST(Character character)
		{
			if (Versions.Client <= KnownVersions.ClosedBeta1)
				return;

			const int ItemsPerPage = 500;
			var items = character.Inventory.GetItems().ToList();
			var totalItems = items.Count;
			var totalPages = (int)Math.Ceiling(totalItems / (double)ItemsPerPage);

			for (var pageIndex = 0; pageIndex < totalPages; pageIndex++)
			{
				var isFirstPage = pageIndex == 0;
				var isLastPage = pageIndex == totalPages - 1;

				var pageItems = items
					.Skip(pageIndex * ItemsPerPage)
					.Take(ItemsPerPage)
					.ToList();

				var packet = new Packet(Op.ZC_ITEM_INVENTORY_DIVISION_LIST);

				packet.PutInt(pageItems.Count);
				packet.PutByte(isFirstPage);
				packet.PutByte(isLastPage);

				packet.Zlib(true, zpacket =>
				{
					foreach (var item in pageItems)
					{
						var propertyList = item.Value.Properties.GetAll();
						var propertiesSize = propertyList.GetByteCount();

						zpacket.PutInt(item.Value.Id);
						zpacket.PutShort(propertiesSize);
						zpacket.PutEmptyBin(2);
						zpacket.PutLong(item.Value.ObjectId);
						zpacket.PutInt(item.Value.Amount);
						zpacket.PutInt(item.Value.Price);
						zpacket.PutInt(0);
						zpacket.PutInt(item.Key);
						zpacket.AddProperties(propertyList);

						if (item.Value.ObjectId != 0)
						{
							zpacket.PutShort(0);
							zpacket.PutLong(item.Value.ObjectId);
							zpacket.PutShort(0);
						}
					}
				});

				character.Connection.Send(packet);
			}
		}

		/// <summary>
		/// Sends ZC_ITEM_INVENTORY_LIST to character, containing a list of
		/// all their equipment.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_ITEM_EQUIP_LIST(Character character)
		{
			// Initially this packet simply contained all equipment,
			// but they changed this at some point and split it up
			// over several instances.

			var equip = character.Inventory.GetEquip();

			if (Versions.Protocol > 500)
			{
				var packetCount = Math.Ceiling(equip.Count / 5f);

				//if (equip.Count != InventoryDefaults.EquipSlotCount)
				//	throw new InvalidOperationException("Incorrect amount of equipment (" + equip.Count + ").");

				for (var i = 0; i < packetCount; ++i)
				{
					var first = (i == 0);
					var minIndex = i * 5;
					var maxIndex = (i + 1) * 5;

					// Seems like the byte is always 1 on the first packet and
					// 0 on subsequent ones. Not quite sure what's up with the
					// rest. Kinda looks like paging, though that shouldn't
					// be necessary for this at all... w/e.

					var packet = new Packet(Op.ZC_ITEM_EQUIP_LIST);

					packet.PutByte(first);
					packet.PutInt(minIndex);
					packet.PutInt(maxIndex);

					for (var j = minIndex; j < maxIndex && j < InventoryDefaults.EquipSlotCount; ++j)
					{
						var equipSlot = (EquipSlot)j;
						var equipItem = equip[equipSlot];

						var propertyList = equipItem.Properties.GetAll();
						var propertiesSize = propertyList.GetByteCount();

						packet.PutInt(equipItem.Id);
						packet.PutShort(propertiesSize);
						packet.PutEmptyBin(2);
						packet.PutLong(equipItem.ObjectId);
						packet.PutByte((byte)equipSlot);
						packet.PutEmptyBin(3);
						packet.PutInt(0);
						packet.PutShort(0);
						packet.AddProperties(propertyList);

						if (equipItem.ObjectId != 0)
						{
							packet.PutShort(0);
							packet.PutLong(equipItem.ObjectId);
							if (!equipItem.HasSockets)
								packet.PutShort(0);
							else
							{
								var gems = equipItem.GetGemSockets();
								packet.PutShort(gems.Count);

								short gemIndex = 0;
								foreach (var gem in gems)
								{
									packet.AddSocket(gemIndex, gem);
									gemIndex++;
								}
							}
						}
					}
					character.Connection.Send(packet);
				}
			}
			else
			{
				var packet = new Packet(Op.ZC_ITEM_EQUIP_LIST);

				foreach (var equipItem in equip)
				{
					var propertyList = equipItem.Value.Properties.GetAll();
					var propertiesSize = propertyList.GetByteCount();

					packet.PutInt(equipItem.Value.Id);
					if (equipItem.Value.ObjectId != 0)
						packet.PutShort(propertiesSize);
					else
						packet.PutShort(0);
					packet.PutEmptyBin(2);
					packet.PutLong(equipItem.Value.ObjectId);
					packet.PutByte((byte)equipItem.Key);
					packet.PutEmptyBin(3);
					packet.PutInt(0);
					if (equipItem.Value.ObjectId != 0)
						packet.AddProperties(propertyList);
				}

				character.Connection.Send(packet);
			}
		}

		/// <summary>
		/// Unequips an item and optionally shows a UI message.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		/// <param name="message"></param>
		public static void ZC_EQUIP_ITEM_REMOVE(Character character, Item item, int message)
		{
			var packet = new Packet(Op.ZC_EQUIP_ITEM_REMOVE);
			packet.PutLong(item.ObjectId);

			// TODO: Make message an enumeration.
			packet.PutInt(message);

			character.Connection.Send(packet);
		}


		/// <summary>
		/// Updates the durability of an item in an equipment slot.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="slot"></param>
		/// <param name="durability">Value in thousandths that the item has remaining.</param>
		public static void ZC_CHANGE_EQUIP_DURABILITY(Character character, EquipSlot slot, int durability)
		{
			var packet = new Packet(Op.ZC_CHANGE_EQUIP_DURABILITY);
			packet.PutByte((byte)slot);
			packet.PutInt(durability);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send ZC_CHAT a specific actor.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void ZC_CHAT(IZoneConnection conn, IActor actor, string format, params object[] args)
		{
			if (args.Length > 0)
				format = string.Format(format, args);

			var teamName = "";
			var name = actor.Name;
			var jobId = JobId.Swordsman;
			var gender = Gender.Male;
			var hair = 0;

			if (actor is Character speaker)
			{
				teamName = speaker.TeamName;
				jobId = speaker.JobId;
				gender = speaker.Gender;
				hair = speaker.Hair;
			}

			var packet = new Packet(Op.ZC_CHAT);

			packet.PutInt(actor.Handle);
			packet.PutString(teamName, 64);
			packet.PutString(name, 65);
			packet.PutByte(0); // -11, -60, -1, -19, 1
			packet.PutShort((short)jobId);
			packet.PutInt((int)jobId); // 1, 10, 11
			packet.PutByte((byte)gender);
			packet.PutByte((byte)hair);
			packet.PutEmptyBin(2);
			packet.PutInt(0); // 628051

			// [i11257 (2016-03-25)] ?
			if (Versions.Client >= 11257)
			{
				packet.PutInt(1004);
			}
			packet.PutInt(0); //i3
			packet.PutInt(0); //i4
			packet.PutInt(0); //i5

			packet.PutFloat(0); // Display time in seconds, min cap 5s
			if (Versions.Client >= 170175)
				packet.PutEmptyBin(16); // [i170175] ?
			if (Versions.Client >= 339415)
				packet.PutEmptyBin(16); // [i339415] ?
			packet.PutByte(1);
			if (Versions.Client >= 373230)
				packet.PutString(ZoneServer.Instance.ServerList.GroupData.Nation, 64); // [i373230]
			packet.PutString(format);

			conn.Send(packet);
		}

		/// <summary>
		/// Broadcasts ZC_CHAT in range of actor.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public static void ZC_CHAT(IActor actor, string format, params object[] args)
		{
			if (args.Length > 0)
				format = string.Format(format, args);

			var teamName = "";
			var name = actor.Name;
			var jobId = JobId.Swordsman;
			var gender = Gender.Male;
			var hair = 0;

			if (actor is Character character)
			{
				teamName = character.TeamName;
				jobId = character.JobId;
				gender = character.Gender;
				hair = character.Hair;
			}

			var packet = new Packet(Op.ZC_CHAT);

			packet.PutInt(actor.Handle);
			packet.PutString(teamName, 64);
			packet.PutString(name, 65);
			packet.PutByte(0); // -11, -60, -1, -19, 1
			packet.PutShort((short)jobId);
			packet.PutInt((int)jobId); // 1, 10, 11
			packet.PutByte((byte)gender);
			packet.PutByte((byte)hair);
			packet.PutEmptyBin(2);
			packet.PutInt(0); // 628051

			// [i11257 (2016-03-25)] ?
			if (Versions.Client >= 11257)
			{
				packet.PutInt(1004);
			}

			if (Versions.Protocol > 500)
			{
				packet.PutInt(0); //i3
				packet.PutInt(0); //i4
				packet.PutInt(0); //i5
			}

			packet.PutFloat(0); // Display time in seconds, min cap 5s
			if (Versions.Client >= 170175)
				packet.PutEmptyBin(16); // [i170175] ?
			if (Versions.Client >= 339415)
				packet.PutEmptyBin(16); // [i339415] ?
			if (Versions.Protocol > 500)
				packet.PutByte(1);
			if (Versions.Client >= 373230)
				packet.PutString(ZoneServer.Instance.ServerList.GroupData.Nation, 64); // [i373230]
			packet.PutString(format);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Send ZC_SYSTEM_MSG to character.
		/// </summary>
		/// <param name="character">Character to send packet to.</param>
		/// <param name="clientMessage">Id of the message to use.</param>
		/// <param name="parameters">Optional list of message parameters.</param>
		public static void ZC_SYSTEM_MSG(Character character, int clientMessage, params MsgParameter[] parameters)
		{
			var packet = new Packet(Op.ZC_SYSTEM_MSG);

			packet.PutInt(clientMessage);
			packet.PutByte((byte)parameters.Length);
			packet.PutByte(1); // type? 0 = also show in red letters on the screen
			if (Versions.Client >= 219527)
				packet.PutString("", 8); // added i219527
			if (Versions.Client >= 336041)
				packet.PutByte(0); // added i336041
			if (Versions.Client >= 339415)
				packet.PutByte(0); // added i339415
			if (Versions.Client >= 354444)
				packet.PutInt(0);  // added i354444
			if (Versions.Client >= 373230)
			{
				packet.PutByte(0); // added i373230
				packet.PutByte(1); // added i373230
			}

			foreach (var parameter in parameters)
			{
				packet.PutLpString(parameter.Key);
				packet.PutLpString(parameter.Value);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send ZC_SYSTEM_MSG to character.
		/// </summary>
		/// <param name="character">Character to send packet to.</param>
		/// <param name="clientMessage">Id of the message to use.</param>
		/// <param name="parameters">Optional list of message parameters.</param>
		public static void ZC_SYSTEM_MSG(Character character, int clientMessage, byte messageType, params MsgParameter[] parameters)
		{
			var packet = new Packet(Op.ZC_SYSTEM_MSG);

			packet.PutInt(clientMessage);
			packet.PutByte((byte)parameters.Length);
			packet.PutByte(messageType); // type? 0 = also show in red letters on the screen
			if (Versions.Client >= 219527)
				packet.PutString("", 8); // added i219527
			if (Versions.Client >= 336041)
				packet.PutByte(0); // added i336041
			if (Versions.Client >= 339415)
				packet.PutByte(0); // added i339415
			if (Versions.Client >= 354444)
				packet.PutInt(0);  // added i354444
			if (Versions.Client >= 373230)
			{
				packet.PutByte(0); // added i373230
				packet.PutByte(1); // added i373230
			}

			foreach (var parameter in parameters)
			{
				packet.PutLpString(parameter.Key);
				packet.PutLpString(parameter.Value);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send ZC_SYSTEM_MSG to character.
		/// </summary>
		/// <param name="character">Character to send packet to.</param>
		/// <param name="clientMessage">Id of the message to use.</param>
		/// <param name="parameters">Optional list of message parameters.</param>
		public static void ZC_SYSTEM_MSG(Character character, int clientMessage, bool chatFrameOnly, string chatTextColor, params MsgParameter[] parameters)
		{
			var packet = new Packet(Op.ZC_SYSTEM_MSG);

			packet.PutInt(clientMessage);
			packet.PutByte((byte)parameters.Length);
			packet.PutByte(chatFrameOnly); // type? 0 = also show in red letters on the screen
			if (Versions.Client >= 219527)
				packet.PutString(chatTextColor, 8); // added i219527
			if (Versions.Client >= 336041)
				packet.PutByte(0); // added i336041
			if (Versions.Client >= 339415)
				packet.PutByte(0); // added i339415
			if (Versions.Client >= 354444)
				packet.PutInt(0);  // added i354444
			if (Versions.Client >= 373230)
				packet.PutByte(0); // added i373230
			if (Versions.Client >= 373230)
				packet.PutByte(1); // added i373230

			foreach (var parameter in parameters)
			{
				packet.PutLpString(parameter.Key);
				packet.PutLpString(parameter.Value);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Broadcasts ZC_JUMP in range of character, making them jump.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		/// <param name="unkFloat"></param>
		/// <param name="unkByte"></param>
		public static void ZC_JUMP(Character character, Position pos, Direction dir, float unkFloat, byte unkByte)
		{
			var packet = new Packet(Op.ZC_JUMP);

			packet.PutInt(character.Handle);
			packet.PutFloat(character.Properties.GetFloat(PropertyName.JumpPower));
			packet.PutInt(character.GetJumpType());
			packet.PutByte(0);  // 1 or 0
			if (Versions.Client >= KnownVersions.OpenBeta)
			{
				packet.PutPosition(pos);
				packet.PutDirection(dir);
				packet.PutFloat(unkFloat);
				packet.PutEmptyBin(13);
				packet.PutLong(unkByte);
				packet.PutShort(0);
				packet.PutByte(0);
			}

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Update's the character's sitting status, either making them
		/// sit down or stand up.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_REST_SIT(IZoneConnection conn, Character character)
		{
			var packet = new Packet(Op.ZC_REST_SIT);

			packet.PutInt(character.Handle);
			packet.PutByte(0);

			// [i11257 (2016-03-25)]
			// If this is set incorrectly, the character "freezes" and
			// doesn't animate while running around anymore.
			if (Versions.Client >= 11257)
			{
				packet.PutByte(character.IsSitting);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Update's the character's sitting status, either making them
		/// sit down or stand up.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_REST_SIT(Character character)
		{
			var packet = new Packet(Op.ZC_REST_SIT);

			packet.PutInt(character.Handle);
			packet.PutByte(0);

			// [i11257 (2016-03-25)]
			// If this is set incorrectly, the character "freezes" and
			// doesn't animate while running around anymore.
			if (Versions.Client >= 11257)
			{
				packet.PutByte(character.IsSitting);
			}

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Sends ZC_ITEM_REMOVE to character, which removes the given item
		/// or amount from the inventory.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="worldId"></param>
		/// <param name="amount"></param>
		/// <param name="msg"></param>
		/// <param name="invType"></param>
		public static void ZC_ITEM_REMOVE(Character character, long worldId, int amount, InventoryItemRemoveMsg msg, InventoryType invType)
		{
			var packet = new Packet(Op.ZC_ITEM_REMOVE);

			packet.PutLong(worldId);
			packet.PutInt(amount);
			if (Versions.Protocol > 500)
				packet.PutInt(0);
			packet.PutByte((byte)msg);
			packet.PutByte((byte)invType);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_ITEM_INVENTORY_INDEX_LIST to character, containing a list
		/// of indices for all items in the inventory. This updates their order.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_ITEM_INVENTORY_INDEX_LIST(Character character)
		{
			ZC_ITEM_INVENTORY_INDEX_LIST(character, character.Inventory.GetIndices());
		}

		/// <summary>
		/// Sends ZC_ITEM_INVENTORY_INDEX_LIST to character, containing a list
		/// of indices for all items in the given category of the inventory.
		/// This updates their order.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="category"></param>
		public static void ZC_ITEM_INVENTORY_INDEX_LIST(Character character, InventoryCategory category)
		{
			ZC_ITEM_INVENTORY_INDEX_LIST(character, character.Inventory.GetIndices(category));
		}

		/// <summary>
		/// Sends ZC_ITEM_INVENTORY_INDEX_LIST to character, containing a list
		/// of indices for items in an inventory. This updates their order.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="indices"></param>
		public static void ZC_ITEM_INVENTORY_INDEX_LIST(Character character, IDictionary<int, long> indices)
		{
			var packet = new Packet(Op.ZC_ITEM_INVENTORY_INDEX_LIST);

			packet.PutInt(indices.Count);
			foreach (var index in indices)
			{
				packet.PutLong(index.Value);
				packet.PutInt(index.Key);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Broadcasts ZC_UPDATED_PCAPPEARANCE in range of character, updating
		/// their appearance.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_UPDATED_PCAPPEARANCE(Character character)
		{
			var packet = new Packet(Op.ZC_UPDATED_PCAPPEARANCE);

			packet.PutInt(character.Handle);
			packet.AddAppearancePc(character);

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Sends ZC_ITEM_ADD to character, adding the item to the inventory.
		/// </summary>
		/// <remarks>
		/// "Updating" stack by *adding* items to it is an ADD as well.
		/// </remarks>
		/// <param name="character">Character to send packet to.</param>
		/// <param name="item">Item added or updated.</param>
		/// <param name="index">Index of the item in the inventory.</param>
		/// <param name="amount">Amount to add.</param>
		/// <param name="addType">The way the add is displayed?</param>
		public static void ZC_ITEM_ADD(Character character, Item item, int index, int amount, InventoryAddType addType, InventoryType inventoryType = InventoryType.Inventory, float notificationDelay = 0f)
		{
			// For some reason this packet requires properties on the item,
			// otherwise the client crashes. Let's catch this here for the
			// moment, as it seems to be an issue exclusive to this packet,
			// and maybe we'll figure out why exactly it happens.
			var propertyList = item.Properties.GetAll();
			if (propertyList.Count == 0)
				propertyList.Add(new FloatProperty(PropertyName.CoolDown, 0));

			var propertiesSize = propertyList.GetByteCount();

			var packet = new Packet(Op.ZC_ITEM_ADD);

			packet.PutLong(item.ObjectId);
			packet.PutInt(amount);
			if (Versions.Protocol > 500)
				packet.PutInt(0);
			packet.PutInt(index);
			packet.PutInt(item.Id);
			packet.PutShort(propertiesSize);
			packet.PutByte((byte)addType);
			packet.PutFloat(notificationDelay); // Notification delay
			packet.PutByte((byte)inventoryType); // InvType
			packet.PutByte(0);
			packet.PutByte(0);
			packet.AddProperties(propertyList);

			if (Versions.Protocol > 500)
			{
				if (item.ObjectId != 0)
				{
					packet.PutShort(0);
					packet.PutLong(item.ObjectId);
					if (item.HasSockets)
					{
						var socketIndex = (short)0;
						var gems = item.GetGemSockets();

						packet.PutShort(gems.Count);

						foreach (var gem in gems)
							packet.AddSocket(socketIndex++, gem);
					}
					else
						packet.PutShort(0);
				}
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_MOVE_BARRACK to connection, informing client that it's
		/// save to disconnect?
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_MOVE_BARRACK(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_MOVE_BARRACK);
			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_LOGOUT_OK to connection, informing client that it's
		/// save to disconnect?
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_LOGOUT_OK(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_LOGOUT_OK);
			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_CAMPINFO to connection.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_CAMPINFO(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_CAMPINFO); // Size: 18 (12)
			packet.PutEmptyBin(12);
			conn.Send(packet);
		}

		/// <summary>
		/// Broadcasts ZC_SET_POS in range of actor, updating its position.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_SET_POS(IActor actor)
			=> ZC_SET_POS(actor, actor.Position);

		/// <summary>
		/// Broadcasts ZC_SET_POS in range of actor, updating its position.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_SET_POS(IActor actor, Position pos, bool slowCamera = false)
		{
			var packet = new Packet(Op.ZC_SET_POS);

			packet.PutInt(actor.Handle);
			packet.PutPosition(pos);
			if (Versions.Protocol > 500)
				packet.PutByte(slowCamera);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Sends ZC_MOVE_ZONE_OK to connection, telling the client where to
		/// connect to, and which map to load.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="channelId"></param>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		/// <param name="mapId"></param>
		public static void ZC_MOVE_ZONE_OK(Character character, int channelId, string ip, int port, int mapId)
		{
			var packet = new Packet(Op.ZC_MOVE_ZONE_OK);

			packet.PutInt(210004);
			packet.PutInt(IPAddress.Parse(ip).ToInt32());
			packet.PutInt(port);
			packet.PutInt(mapId);
			packet.PutFloat(38);       // Camera X
			packet.PutFloat(45);       // Camera Y
			packet.PutFloat(200);      // Zoom Min
			packet.PutFloat(2200);     // Zoom Max
			packet.PutFloat(1000);     // Zoom Start
			packet.PutInt(26);         // Position?
			packet.PutInt(20);         // Position?
			packet.PutInt(59);         // Position?
			packet.PutShort(0);        // Direction?
			packet.PutByte((byte)channelId);
			if (Versions.Protocol > 500)
				packet.PutLong(character.ObjectId);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Instructs client to prepare for moving to a different map,
		/// and potentially a different zone server.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_MOVE_ZONE(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_MOVE_ZONE);
			packet.PutByte(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Broadcasts ZC_PC in range of character, updating certain information.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="updateType"></param>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <param name="strArg1"></param>
		public static void ZC_PC(Character character, PcUpdateType updateType, int arg1, int arg2, string strArg1 = null)
		{
			var packet = new Packet(Op.ZC_PC);

			packet.PutInt(character.Handle);
			packet.PutInt((int)updateType);
			packet.PutInt(arg1);
			packet.PutInt(arg2);

			if (strArg1 != null)
				packet.PutLpString(strArg1);

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Updates all of character's  properties.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_OBJECT_PROPERTY(Character character)
		{
			ZC_OBJECT_PROPERTY(character.Connection, character);
		}

		/// <summary>
		/// Updates character's given properties.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="propertyNames"></param>
		public static void ZC_OBJECT_PROPERTY(Character character, params string[] propertyNames)
		{
			ZC_OBJECT_PROPERTY(character.Connection, character, propertyNames);
		}

		/// <summary>
		/// Updates object's given properties.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="obj"></param>
		public static void ZC_OBJECT_PROPERTY(IZoneConnection conn, IPropertyObject obj)
			=> ZC_OBJECT_PROPERTY(conn, obj.ObjectId, obj.Properties.GetAll());

		/// <summary>
		/// Updates object's given properties.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="obj"></param>
		/// <param name="properties"></param>
		public static void ZC_OBJECT_PROPERTY(IZoneConnection conn, IPropertyObject obj, Properties properties)
			=> ZC_OBJECT_PROPERTY(conn, obj.ObjectId, properties.GetAll());

		/// <summary>
		/// Updates object's given properties.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="obj"></param>
		/// <param name="propertyNames"></param>
		public static void ZC_OBJECT_PROPERTY(IZoneConnection conn, IPropertyObject obj, params string[] propertyNames)
			=> ZC_OBJECT_PROPERTY(conn, obj.ObjectId, obj.Properties.GetSelect(propertyNames));

		/// <summary>
		/// Updates object's given properties.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="objectId"></param>
		/// <param name="propertyList"></param>
		public static void ZC_OBJECT_PROPERTY(IZoneConnection conn, long objectId, PropertyList propertyList)
		{
			var packet = new Packet(Op.ZC_OBJECT_PROPERTY);

			packet.PutLong(objectId);
			if (Versions.Protocol > 500)
				packet.PutInt(0); // isTrickPacket
			packet.AddProperties(propertyList);

			conn.Send(packet);
		}

		/// <summary>
		/// Updates object's given properties.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="obj"></param>
		/// <param name="propertyNames"></param>
		public static void ZC_OBJECT_PROPERTY(IActor entity, IPropertyObject obj, params string[] propertyNames)
			=> ZC_OBJECT_PROPERTY(entity, obj.ObjectId, obj.Properties.GetSelect(propertyNames));

		/// <summary>
		/// Updates object's given properties.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="objectId"></param>
		/// <param name="propertyList"></param>
		public static void ZC_OBJECT_PROPERTY(IActor actor, long objectId, PropertyList propertyList)
		{
			var packet = new Packet(Op.ZC_OBJECT_PROPERTY);

			packet.PutLong(objectId);
			packet.PutInt(0); // isTrickPacket
			packet.AddProperties(propertyList);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Send a trade request to another player acknowledgement
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_EXCHANGE_REQUEST_ACK(Character character)
		{
			var packet = new Packet(Op.ZC_EXCHANGE_REQUEST_ACK);

			packet.PutString(character.Name, 65);
			packet.PutByte(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send a trade request to another player
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_EXCHANGE_REQUEST_RECEIVED(Character character, string requesterName)
		{
			var packet = new Packet(Op.ZC_EXCHANGE_REQUEST_RECEIVED);

			packet.PutString(requesterName, 65);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send start trade to client
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_EXCHANGE_START(Character character, string tradePartnerTeamName)
		{
			var packet = new Packet(Op.ZC_EXCHANGE_START);

			packet.PutString(tradePartnerTeamName, 65);
			packet.PutByte(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send item offer to client
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_EXCHANGE_OFFER_ACK(Character character, bool sameAsSender, Item item, int amount)
		{
			var packet = new Packet(Op.ZC_EXCHANGE_OFFER_ACK);

			var propertyList = item.Properties.GetAll();
			var propertiesSize = propertyList.GetByteCount();

			packet.PutByte((byte)(sameAsSender ? 0 : 1));
			packet.PutInt(0);
			packet.PutInt(-1);
			packet.PutLong(item.ObjectId);
			packet.PutInt(item.Id);
			packet.PutInt(amount);
			packet.PutShort(propertiesSize);
			packet.AddProperties(propertyList);
			packet.PutShort(0);
			packet.PutLong(item.ObjectId);
			if (!item.HasSockets)
				packet.PutShort(0);
			else
			{
				var gems = item.GetGemSockets();
				packet.PutShort(gems.Count);

				short gemIndex = 0;
				foreach (var gem in gems)
				{
					packet.AddSocket(gemIndex, gem);
					gemIndex++;
				}
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send exchange initial agree acknowledgement to client
		/// </summary>
		/// <param name="character"></param>
		/// <param name="isSameAsSender"></param>
		public static void ZC_EXCHANGE_AGREE_ACK(Character character, bool isSameAsSender)
		{
			var packet = new Packet(Op.ZC_EXCHANGE_AGREE_ACK);
			packet.PutByte((byte)(isSameAsSender ? 0 : 1));

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send exchange final agree acknowledgement to client
		/// </summary>
		/// <param name="character"></param>
		/// <param name="isSameAsSender"></param>
		public static void ZC_EXCHANGE_FINALAGREE_ACK(Character character, bool isSameAsSender)
		{
			var packet = new Packet(Op.ZC_EXCHANGE_FINALAGREE_ACK);

			packet.PutByte((byte)(isSameAsSender ? 0 : 1));

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send trade successfully completed
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_EXCHANGE_SUCCESS(Character character)
		{
			var packet = new Packet(Op.ZC_EXCHANGE_SUCCESS);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send trade canceled
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_EXCHANGE_CANCEL_ACK(Character character)
		{
			var packet = new Packet(Op.ZC_EXCHANGE_CANCEL_ACK);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates actor's rotation for characters in range of it.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_ROTATE(IActor actor)
		{
			var packet = new Packet(Op.ZC_ROTATE);

			packet.PutInt(actor.Handle);
			packet.PutFloat(actor.Direction.Cos);
			packet.PutFloat(actor.Direction.Sin);
			if (Versions.Protocol > 500)
			{
				packet.PutByte(1); // Seems to be 1
				packet.PutByte(1); // Seems to be 1
				packet.PutInt(0);
			}
			else
			{
				packet.PutByte(1);
				packet.PutByte(0);
			}

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Updates actor's head rotation for characters in range of it.
		/// </summary>
		/// <param name="entity"></param>
		public static void ZC_HEAD_ROTATE(IActor entity)
		{
			var packet = new Packet(Op.ZC_HEAD_ROTATE);

			packet.PutInt(entity.Handle);
			packet.PutFloat(entity.Direction.Cos);
			packet.PutFloat(entity.Direction.Sin);

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Broadcasts a Lua addon event on the client with the given name,
		/// prefixed with "OPEN_DLG_".
		/// </summary>
		/// <example>
		/// ZC_CUSTOM_DIALOG(character, "warehouse", "", 0);
		/// This call raises "OPEN_DLG_WAREHOUSE", which the client scripts
		/// subscribe to as such:
		/// addon:RegisterMsg("OPEN_DLG_WAREHOUSE", "ON_OPEN_WAREHOUSE");
		/// </example>
		/// <param name="character"></param>
		/// <param name="evName"></param>
		/// <param name="argStr"></param>
		/// <param name="argNum"></param>
		public static void ZC_CUSTOM_DIALOG(Character character, string evName, string argStr = "", int argNum = 0)
		{
			var packet = new Packet(Op.ZC_CUSTOM_DIALOG);

			packet.PutString(evName, 33);
			packet.PutString(argStr, 32);
			packet.PutInt(argNum);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_DIALOG_OK to connection, containing a dialog message.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="msg"></param>
		public static void ZC_DIALOG_OK(IZoneConnection conn, string msg)
		{
			var packet = new Packet(Op.ZC_DIALOG_OK);

			packet.PutInt(0); // handle?
			packet.PutString(msg);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_DIALOG_NEXT to connection, containing a dialog message.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="msg"></param>
		public static void ZC_DIALOG_NEXT(IZoneConnection conn, string msg)
		{
			var packet = new Packet(Op.ZC_DIALOG_NEXT);

			packet.PutInt(0); // handle?
			packet.PutString(msg);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_DIALOG_SELECT to connection, containing a dialog message
		/// and a list of selectable options.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="arguments"></param>
		public static void ZC_DIALOG_SELECT(IZoneConnection conn, params string[] arguments)
			=> ZC_DIALOG_SELECT(conn, (IEnumerable<string>)arguments);

		/// <summary>
		/// Sends ZC_DIALOG_SELECT to connection, containing a dialog message
		/// and a list of selectable options.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="arguments"></param>
		public static void ZC_DIALOG_SELECT(IZoneConnection conn, IEnumerable<string> arguments)
		{
			if (arguments == null || !arguments.Any())
				return;

			var packet = new Packet(Op.ZC_DIALOG_SELECT);

			packet.PutInt(0); // handle?
			if (Versions.Protocol > 500)
				packet.PutByte((byte)arguments.Count());
			else
				packet.PutShort(arguments.Count());

			// If both bytes are 0, the message before the selection is
			// displayed as expected. First the message, then the options
			// pop up. If only the first byte is 1, the options and the
			// message both immediately appear together, and if only the
			// second byte is set, only the NPC portrait appears. If both
			// are 1, only the options appear, without message.
			// This might be useful for selection boxes without dialog.
			if (Versions.Client >= 171032)
				packet.PutByte(0); // [i171032] ?
			if (Versions.Client >= 337645)
				packet.PutByte(0); // [i337645] ?

			if (Versions.Client >= 337645)
				packet.PutShort(0); // [i337645] ?

			foreach (var arg in arguments)
				packet.PutLpString(arg);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_DIALOG_CLOSE to connection, which closes the currently
		/// open dialog.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_DIALOG_CLOSE(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_DIALOG_CLOSE);
			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_DIALOG_STRINGINPUT to connection, containing a dialog
		/// message, and requesting putting in a string.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="msg"></param>
		public static void ZC_DIALOG_STRINGINPUT(IZoneConnection conn, string msg)
		{
			var packet = new Packet(Op.ZC_DIALOG_STRINGINPUT);

			packet.PutInt(0); // handle?
			packet.PutString(msg);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_DIALOG_NUMBERRANGE over connection, containing a dialog
		/// message, and requesting putting in a number.
		/// </summary>
		/// <remarks>
		/// Due to number range using CZ_DIALOG_SELECT for its response,
		/// the max range is 0~255, since that packet only holds a
		/// byte. The dialog window for this packet seems a little
		/// unfinished, and I didn't see any packets for it yet,
		/// it can be assumed that this feature, albeit working,
		/// isn't 100% done yet.
		/// </remarks>
		/// <param name="conn"></param>
		/// <param name="msg"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		public static void ZC_DIALOG_NUMBERRANGE(IZoneConnection conn, string msg, int min = 0, int max = 255)
		{
			min = Math2.Clamp(0, 255, min);
			max = Math2.Clamp(0, 255, max);
			if (max < min)
				max = min;

			var packet = new Packet(Op.ZC_DIALOG_NUMBERRANGE);

			packet.PutInt(0); // handle?
			packet.PutString(msg, 128);
			packet.PutInt(min);
			packet.PutInt(max);

			conn.Send(packet);
		}

		/// <summary>
		/// Removes actor from all clients on the map it's on.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_LEAVE(IActor actor, LeaveType leaveType = LeaveType.NoEffect)
		{
			var packet = new Packet(Op.ZC_LEAVE);

			packet.PutInt(actor.Handle);
			packet.PutShort((short)leaveType); // 0 shows a blue effect when the entity disappears

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Removes actor from map on connection's client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="actor"></param>
		public static void ZC_LEAVE(IZoneConnection conn, IActor actor)
		{
			var s1 = 1;

			// Items don't seem to disappear with our default, 1, nor with
			// 2, which is used on officials. 4 does get rid of the items
			// though. However, if you use 4, the pick up animation doesn't
			// play. I'm guessing the item can't be removed if it's supposed
			// to get picked up for this very reason, so we'll check whether
			// it was picked up or not.
			if (actor is ItemMonster itemMonster)
				s1 = itemMonster.PickedUp ? 2 : 4;
			else if (actor is Companion)
				s1 = 4;

			var packet = new Packet(Op.ZC_LEAVE);

			packet.PutInt(actor.Handle);
			packet.PutShort(s1); // 0 shows a blue effect when the entity disappears

			conn.Send(packet);
		}

		/// <summary>
		/// Makes actor appear dead on all clients in range of it.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_DEAD(IActor actor, IActor killer = null, bool showCorpse = true, bool isOverkill = false, bool isSpecialDrop = false)
		{
			var packet = new Packet(Op.ZC_DEAD);

			packet.PutInt(actor.Handle);
			// 'showCorpse' Does not seem to work for normal
			// monsters, but seems to be needed for destroying player created
			// entities like Quarrel Shooter's Pavise
			packet.PutByte(showCorpse);
			packet.PutByte(0); // expInfoCount
			packet.PutByte(isOverkill); // isOverkill
			packet.PutByte(isSpecialDrop); // specialDrop
			packet.PutPosition(actor.Position);
			packet.PutInt(0);

			//for expInfoCount:
			//{
			//	packet.PutInt(killer.Handle);
			//	packet.PutInt(0);
			//	packet.PutLong(exp);
			//	packet.PutLong(jobExp);
			//	packet.PutLong(exp);
			//}

			// The overkill amount is the percentage displayed on the
			// client. It needs to be at least 100 for the overkill
			// effect to appear.
			//if (isOverkill)
			//	packet.PutByte((byte)overkillAmount);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Opens resurrection dialog on character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="options"></param>
		public static void ZC_RESURRECT_DIALOG(Character character, ResurrectOptions options)
		{
			var packet = new Packet(Op.ZC_RESURRECT_DIALOG);

			if (Versions.Protocol > 500)
			{
				packet.PutInt((int)options);
				packet.PutString("", 512);
				packet.PutByte(0);
			}
			else
			{
				packet.PutByte((byte)options);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Acknowledges the character's resurrection request and toggles
		/// the dialog off.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_RESURRECT_SAVE_POINT_ACK(Character character)
		{
			var packet = new Packet(Op.ZC_RESURRECT_SAVE_POINT_ACK);
			packet.PutByte(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Acknowledges the character's resurrection request and toggles
		/// the dialog off.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_RESURRECT_HERE_ACK(Character character)
		{
			var packet = new Packet(Op.ZC_RESURRECT_HERE_ACK);
			packet.PutByte(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Resurrects the character for all clients in range.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_RESURRECT(Character character)
		{
			var hp = character.Properties.GetFloat(PropertyName.HP);
			var maxHp = character.Properties.GetFloat(PropertyName.MHP);

			var packet = new Packet(Op.ZC_RESURRECT);
			packet.PutInt(character.Handle);
			packet.PutInt((int)hp);
			packet.PutInt((int)maxHp);

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Informs players about a hit that occured, and about the target's
		/// new hp, after damage was applied.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="hitInfo"></param>
		public static void ZC_HIT_INFO(ICombatEntity attacker, ICombatEntity target, HitInfo hitInfo)
		{
			var packet = new Packet(Op.ZC_HIT_INFO);

			packet.PutInt(target.Handle);
			packet.PutInt(attacker.Handle);
			packet.PutInt((int)hitInfo.SkillId);

			packet.AddHitInfo(hitInfo);

			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(hitInfo.ForceId);
			if (Versions.Client > KnownVersions.ClosedBeta1)
			{
				packet.PutByte(0);
				packet.PutByte(0);
				packet.PutFloat(hitInfo.UnkFloat1);
				packet.PutFloat(hitInfo.UnkFloat2);
				packet.PutInt(hitInfo.HitCount);
				packet.PutByte(1);
				packet.PutInt(0);
				packet.PutInt((int)hitInfo.DamageDelay.TotalMilliseconds);
			}
			else
			{
				packet.PutByte(1);
				packet.PutInt((int)hitInfo.DamageDelay.TotalMilliseconds);
			}

			target.Map.Broadcast(packet, target);
		}

		/// <summary>
		/// Informs players about a hit that occured, and about the target's
		/// new hp, after damage was applied.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_HIT_INFO(IActor attacker, params SkillHitInfo[] hits)
			=> ZC_SKILL_HIT_INFO(attacker, (IEnumerable<SkillHitInfo>)hits);

		/// <summary>
		/// Informs players about a hit that occured, and about the target's
		/// new hp, after damage was applied.
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="hits"></param>
		public static void ZC_SKILL_HIT_INFO(IActor attacker, IEnumerable<SkillHitInfo> hits)
		{
			var packet = new Packet(Op.ZC_SKILL_HIT_INFO);

			packet.PutInt(attacker.Handle);
			packet.PutByte((byte)hits.Count());

			foreach (var skillHit in hits)
				packet.AddSkillHitInfo(skillHit);

			attacker.Map.Broadcast(packet, attacker);
		}

		/// <summary>
		/// Updates character's level.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_PC_LEVELUP(Character character)
		{
			var packet = new Packet(Op.ZC_PC_LEVELUP);
			packet.PutInt(character.Handle);
			packet.PutInt(character.Level);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates exp and max exp.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="exp"></param>
		public static void ZC_MAX_EXP_CHANGED(Character character, int exp)
		{
			var packet = new Packet(Op.ZC_MAX_EXP_CHANGED);

			packet.PutInt(exp);
			if (Versions.Protocol > 500)
			{
				packet.PutLong(character.Exp);
				packet.PutLong(character.MaxExp);
				packet.PutLong(character.TotalExp);
			}
			else
			{
				packet.PutInt((int)character.Exp);
				packet.PutInt((int)character.MaxExp);
				if (Versions.Client > KnownVersions.ClosedBeta1)
					packet.PutInt((int)character.TotalExp);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Notification about acquired exp from killing a monster?
		/// </summary>
		/// <param name="character"></param>
		/// <param name="exp"></param>
		/// <param name="jobExp"></param>
		/// <param name="monster"></param>
		public static void ZC_EXP_UP_BY_MONSTER(Character character, long exp, long jobExp, IMonster monster)
		{
			var packet = new Packet(Op.ZC_EXP_UP_BY_MONSTER);

			if (Versions.Protocol > 500)
			{
				packet.PutLong(exp);
				packet.PutLong(jobExp);
				packet.PutLong(exp);
			}
			else
			{
				packet.PutInt((int)exp);
				packet.PutInt((int)jobExp);
			}
			packet.PutInt(monster?.Handle ?? 0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Adds exp.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="exp"></param>
		/// <param name="jobExp"></param>
		public static void ZC_EXP_UP(Character character, long exp, long jobExp)
		{
			var packet = new Packet(Op.ZC_EXP_UP);

			if (Versions.Protocol > 500)
			{
				packet.PutLong(exp);
				packet.PutLong(jobExp);
				packet.PutLong(exp);
			}
			else
			{
				packet.PutInt((int)exp);
				packet.PutInt((int)jobExp);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Adds job exp.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="exp"></param>
		public static void ZC_JOB_EXP_UP(Character character, long exp)
		{
			var packet = new Packet(Op.ZC_JOB_EXP_UP);

			if (Versions.Protocol > 500)
			{
				packet.PutLong(character.ObjectId);
				packet.PutLong(exp);
			}
			else
			{
				packet.PutInt((int)exp);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Executes Lua addon function.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="msg"></param>
		/// <param name="argNum"></param>
		/// <param name="argStr"></param>
		public static void ZC_ADDON_MSG(Character character, string msg, int argNum = 0, string argStr = null, byte notUsed = 0)
		{
			var packet = new Packet(Op.ZC_ADDON_MSG);

			var msgByteLength = packet.GetByteLength(msg);
			if (msgByteLength > byte.MaxValue)
				throw new ArgumentException($"Message is too long with {msgByteLength} bytes. The maximum length is {byte.MaxValue}.");

			packet.PutByte((byte)msgByteLength);
			packet.PutInt(argNum);
			packet.PutByte(notUsed);
			packet.PutRawString(msg);

			if (argStr != null)
				packet.PutRawString(argStr);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Executes Lua addon function on broadcast to visible players.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="msg"></param>
		/// <param name="argNum"></param>
		/// <param name="argStr"></param>
		public static void ZC_ADDON_MSG(IActor actor, string msg, int argNum = 0, string argStr = null)
		{
			var packet = new Packet(Op.ZC_ADDON_MSG);

			var msgByteLength = packet.GetByteLength(msg);
			if (msgByteLength > byte.MaxValue)
				throw new ArgumentException($"Message is too long with {msgByteLength} bytes. The maximum length is {byte.MaxValue}.");

			packet.PutByte((byte)msgByteLength);
			packet.PutInt(argNum);
			packet.PutByte(0);
			packet.PutRawString(msg);

			if (argStr != null)
				packet.PutRawString(argStr);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Shows a skill balloon UI above a monster with optional casting bar.
		/// </summary>
		/// <param name="monster">The monster actor to show the balloon above.</param>
		/// <param name="skillName">The skill name to display.</param>
		/// <param name="castTimeMs">Cast time in milliseconds.</param>
		/// <param name="showCastingBar">If true, shows a casting bar with gauge.</param>
		/// <param name="changeColor">If true, displays the text in yellow.</param>
		public static void MonsterSkillBalloon(IActor monster, string skillName, int castTimeMs, bool showCastingBar = true, bool changeColor = false)
		{
			var args = $"{skillName}|{monster.Handle}|{castTimeMs}|{(showCastingBar ? 1 : 0)}|{(changeColor ? 1 : 0)}";
			ZC_ADDON_MSG(monster, "MON_SKILL_BALLOON", 0, args);
		}

		/// <summary>
		/// Cancels/hides a monster's skill balloon UI.
		/// </summary>
		/// <param name="monster">The monster actor whose balloon to cancel.</param>
		public static void MonsterSkillBalloonCancel(IActor monster)
		{
			ZC_ADDON_MSG(monster, "MON_SKILL_BALLOON_CANCEL", 0, monster.Handle.ToString());
		}

		/// <summary>
		/// Plays a sound effect.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="animationName"></param>
		/// <param name="loop"></param>
		/// <param name="f1"></param>
		/// <param name="b2"></param>
		public static void ZC_PLAY_SOUND(IActor actor, string animationName, bool loop = false, float volumeFix = -1.0f, bool useSetBalanceVolume = false)
		{
			var packet = new Packet(Op.ZC_PLAY_SOUND);

			packet.PutInt(actor.Handle);
			packet.AddStringId(animationName);
			packet.PutByte(loop);
			if (Versions.Protocol > 500)
			{
				packet.PutFloat(volumeFix);
				packet.PutByte(useSetBalanceVolume);
			}

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Stop playing a sound effect
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="soundEffectName"></param>
		public static void ZC_STOP_SOUND(IActor actor, string soundEffectName)
		{
			var packet = new Packet(Op.ZC_STOP_SOUND);

			packet.PutInt(actor.Handle);
			packet.AddStringId(soundEffectName);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Plays a sound effect.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="musicName"></param>
		/// <param name="s1"></param>
		/// <param name="b1"></param>
		public static void ZC_PLAY_MUSICQUEUE(IActor actor, string musicName, short s1 = 0, byte b1 = 0)
		{
			var packet = new Packet(Op.ZC_PLAY_MUSICQUEUE);

			packet.PutInt(actor.Handle);
			packet.AddStringId(musicName);
			packet.PutShort(s1);
			packet.PutByte(b1);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Stop playing a sound effect
		/// </summary>
		/// <param name="character"></param>
		/// <param name="soundName"></param>
		public static void ZC_STOP_MUSICQUEUE(Character character, string soundName)
		{
			var packet = new Packet(Op.ZC_STOP_MUSICQUEUE);

			packet.PutInt(character.Handle);
			packet.AddStringId(soundName);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_PC_PROP_UPDATE to character, updating a property.
		/// </summary>
		/// <remarks>
		/// This packet only supports property ids that are a ushort value (0-65535)
		/// Usually Account Properties are sent via this packet.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="property"></param>
		/// <param name="value"></param>
		public static void ZC_PC_PROP_UPDATE(Character character, int property, byte value)
		{
			var packet = new Packet(Op.ZC_PC_PROP_UPDATE);

			packet.PutInt(property);
			packet.PutByte(value); // ?

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Shows emoticon for actor on nearby clients.
		/// </summary>
		/// <remarks>
		/// For some available emoticons, search the packet string data for
		/// entries with "_emo_" in their names, such as "I_emo_fear".
		/// </remarks>
		/// <param name="actor"></param>
		/// <param name="emoticonName"></param>
		/// <param name="duration">Time to show to the emoticon for.</param>
		public static void ZC_EMOTICON(IActor actor, string emoticonName, TimeSpan duration)
		{
			var packet = new Packet(Op.ZC_EMOTICON);

			packet.PutInt(actor.Handle);
			packet.AddStringId(emoticonName);
			packet.PutInt((int)duration.TotalMilliseconds);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Shows emoticon for actor on nearby clients.
		/// </summary>
		/// <remarks>
		/// For some available emoticons, search the packet string data for
		/// entries with "_emo_" in their names, such as "I_emo_fear".
		/// </remarks>
		/// <param name="actor"></param>
		/// <param name="emoticonName"></param>
		/// <param name="duration">Time to show to the emoticon for.</param>
		public static void ZC_SHOW_EMOTICON(IActor actor, string emoticonName, TimeSpan duration)
		{
			var packet = new Packet(Op.ZC_SHOW_EMOTICON);

			packet.PutInt(actor.Handle);
			packet.AddStringId(emoticonName);
			packet.PutInt((int)duration.TotalMilliseconds);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Shows emoticon for actor to a specific connection.
		/// </summary>
		/// <remarks>
		/// For some available emoticons, search the packet string data for
		/// entries with "_emo_" in their names, such as "I_emo_fear".
		/// </remarks>
		/// <param name="conn"></param>
		/// <param name="actor"></param>
		/// <param name="emoticonName"></param>
		/// <param name="duration">Time to show to the emoticon for.</param>
		public static void ZC_SHOW_EMOTICON(IZoneConnection conn, IActor actor, string emoticonName, TimeSpan duration)
		{
			var packet = new Packet(Op.ZC_SHOW_EMOTICON);

			packet.PutInt(actor.Handle);
			packet.AddStringId(emoticonName);
			packet.PutInt((int)duration.TotalMilliseconds);

			conn.Send(packet);
		}

		/// <summary>
		/// Hold move path?
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="b1"></param>
		public static void ZC_HOLD_MOVE_PATH(IActor actor, bool b1)
		{
			var packet = new Packet(Op.ZC_HOLD_MOVE_PATH);

			packet.PutInt(actor.Handle);
			packet.PutByte(b1);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Sends ZC_LOGIN_TIME to connection.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="now"></param>
		public static void ZC_LOGIN_TIME(IZoneConnection conn, DateTime now)
		{
			var packet = new Packet(Op.ZC_LOGIN_TIME);
			if (Versions.Protocol > 500)
				packet.PutLong(now.ToFileTime());
			else
				packet.PutLong(now.ToUnixTimeSeconds() * 1000);

			conn.Send(packet);
		}

		public static void ZC_LAYER_PC_LIST(IZoneConnection conn, int i1, List<Character> characters)
		{
			var packet = new Packet(Op.ZC_LAYER_PC_LIST);

			packet.PutInt(i1);
			packet.PutInt(characters.Count);

			foreach (var character in characters)
			{
				var jobs = character.Jobs.GetList();
				packet.PutInt(RandomProvider.Next(2)); // Could be team id? 1 or 2
				packet.PutInt(0);
				packet.PutInt(0); // Random values 4334, 1015, 603
				packet.PutLong(character.AccountObjectId);
				packet.PutString(character.TeamName, 64);
				packet.PutInt(0);
				packet.PutString(character.TeamName, 64);
				packet.PutString(character.Name, 64);
				packet.PutShort(0);
				packet.PutShort((short)character.JobId);
				packet.PutInt((int)character.JobId);
				packet.PutInt(character.Job.Level);
				packet.PutShort((short)character.Gender);
				packet.PutShort((short)character.Hair);
				// 628333,628341,11006112,18015,0
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
				packet.PutInt(character.Hp);
				packet.PutInt(character.MaxHp);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Sends the visible areas of a map to a character.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_MAP_REVEAL_LIST(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_MAP_REVEAL_LIST);

			var revealedMaps = conn.Account.GetRevealedMaps();

			packet.PutInt(revealedMaps.Length);

			// Officials appear to compress the actual data nowadays, but it seems
			// like the client can still handle the raw data as well.
			foreach (var revealedMap in revealedMaps)
			{
				packet.PutInt(revealedMap.MapId);
				packet.PutBin(revealedMap.Explored);
				packet.PutFloat(revealedMap.Percentage);
				packet.PutFloat(revealedMap.Percentage);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Sends list of IES modifications to client.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_IES_MODIFY_LIST(IZoneConnection conn)
		{
			var allMods = ZoneServer.Instance.IesMods;
			var modLists = IesHelper.BuildModLists(allMods);

			foreach (var modList in modLists)
			{
				var packet = new Packet(Op.ZC_IES_MODIFY_LIST);
				packet.AddIesModList(modList);

				conn.Send(packet);
			}
		}

		/// <summary>
		/// Broadcasts ZC_QUICK_ROTATE in range of an actor, putting them
		/// in a certain direction "quickly".
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_QUICK_ROTATE(IActor actor)
		{
			var dir = actor.Direction;

			var packet = new Packet(Op.ZC_QUICK_ROTATE);

			packet.PutInt(actor.Handle);
			packet.PutFloat(dir.Cos);
			packet.PutFloat(dir.Sin);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Broadcasts ZC_POSE in range of character, putting them into the
		/// given pose.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="pose"></param>
		public static void ZC_POSE(Character character, int pose)
		{
			var pos = character.Position;
			var dir = character.Direction;

			var packet = new Packet(Op.ZC_POSE);

			packet.PutInt(character.Handle);
			packet.PutInt(pose);
			packet.PutFloat(pos.X);
			packet.PutFloat(pos.Y);
			packet.PutFloat(pos.Z);
			packet.PutFloat(dir.Cos);
			packet.PutFloat(dir.Sin);
			if (Versions.Client > KnownVersions.ClosedBeta1)
				packet.PutByte(1);

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Updates "shield" (?) for actor on nearby clients.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="shield"></param>
		public static void ZC_UPDATE_SHIELD(IActor actor, long shield, byte b1 = 1)
		{
			var packet = new Packet(Op.ZC_UPDATE_SHIELD);

			packet.PutInt(actor.Handle);
			// These are guesses when they were changed.
			if (Versions.Client > KnownVersions.PreReBuild)
			{
				packet.PutLong(shield);
				packet.PutByte(b1);
			}
			else if (Versions.Client > KnownVersions.ClosedBeta1 && Versions.Client <= KnownVersions.PreReBuild)
			{
				packet.PutInt((int)shield);
			}
			else
				packet.PutShort((short)shield);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Updates "shield" (?) for actor on nearby clients.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="shield"></param>
		public static void ZC_UPDATE_SHIELD(IZoneConnection conn, IActor actor, long shield, byte b1 = 0)
		{
			var packet = new Packet(Op.ZC_UPDATE_SHIELD);

			packet.PutInt(actor.Handle);
			// These are guesses when they were changed.
			if (Versions.Client > KnownVersions.PreReBuild)
			{
				packet.PutLong(shield);
				packet.PutByte(b1);
			}
			else if (Versions.Client > KnownVersions.ClosedBeta1 && Versions.Client <= KnownVersions.PreReBuild)
			{
				packet.PutInt((int)shield);
			}
			else
				packet.PutShort((short)shield);

			conn.Send(packet);
		}

		/// <summary>
		/// Makes entity move in the given direction for clients in range.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		/// <param name="unkFloat"></param>
		public static void ZC_MOVE_DIR(ICombatEntity entity, Position pos, Direction dir, float unkFloat)
		{
			var packet = new Packet(Op.ZC_MOVE_DIR);

			packet.PutInt(entity.Handle);
			packet.PutPosition(pos);
			packet.PutDirection(dir);
			packet.PutByte(1); // 0 = reduced movement speed... walk mode?
			packet.PutFloat(entity.Properties.GetFloat(PropertyName.MSPD));
			if (Versions.Protocol > 500)
			{
				packet.PutFloat(unkFloat);
				packet.PutEmptyBin(24);
				packet.PutInt(6);
				packet.PutInt(0);
				packet.PutByte(1);
			}
			else
			{
				packet.PutByte(1);
				packet.PutFloat(unkFloat);
			}

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Broadcasts ZC_MOVE_STOP in range of character, informing other
		/// characters about the movement stop.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="pos"></param>
		public static void ZC_MOVE_STOP(IActor actor, Position pos, byte b1 = 0)
		{
			var packet = new Packet(Op.ZC_MOVE_STOP);

			packet.PutInt(actor.Handle);
			packet.PutPosition(pos);
			packet.PutByte(b1);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Makes player character stop moving on clients in range of it.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		public static void ZC_PC_MOVE_STOP(ICombatEntity entity, Position pos, Direction dir)
		{
			var packet = new Packet(Op.ZC_PC_MOVE_STOP);

			packet.PutInt(entity.Handle);
			packet.PutPosition(pos);
			packet.PutByte(1);
			packet.PutDirection(dir);
			packet.PutFloat(228787.3f); // timestamp

			if (Versions.Protocol > 500)
				packet.PutEmptyBin(24);

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Sends ZC_DIALOG_TRADE to connection, containing the name of the
		/// shop to open.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="shopName"></param>
		public static void ZC_DIALOG_TRADE(IZoneConnection conn, string shopName)
		{
			var packet = new Packet(Op.ZC_DIALOG_TRADE);
			packet.PutString(shopName, 33);

			conn.Send(packet);
		}

		/// <summary>
		/// Notifies the client that the skill is ready? Exact purpose
		/// currently unknown.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skill"></param>
		/// <param name="position1"></param>
		/// <param name="position2"></param>
		public static void ZC_SKILL_READY(ICombatEntity entity, Skill skill, Position position1, Position position2)
			=> ZC_SKILL_READY(entity, skill, 0, position1, position2);

		/// <summary>
		/// Notifies the client that the skill is ready? Exact purpose
		/// currently unknown.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="skill"></param>
		/// <param name="i1"></param>
		/// <param name="position1"></param>
		/// <param name="position2"></param>
		public static void ZC_SKILL_READY(ICombatEntity entity, Skill skill, int i1, Position position1, Position position2)
		{
			var packet = new Packet(Op.ZC_SKILL_READY);

			packet.PutInt(entity.Handle);
			packet.PutInt((int)skill.Id);
			packet.PutFloat(1);
			packet.PutFloat(1);
			if (Versions.Client > KnownVersions.ClosedBeta1)
				packet.PutInt(i1);
			packet.PutPosition(position1);
			packet.PutPosition(position2);

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Adjusts the time speed of the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="timeFactor"></param>
		public static void ZC_TIME_FACTOR(IZoneConnection conn, float timeFactor = 1)
		{
			var packet = new Packet(Op.ZC_TIME_FACTOR);
			packet.PutFloat(timeFactor);

			conn.Send(packet);
		}

		/// <summary>
		/// Updates actor's team id on the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="actor"></param>
		/// <param name="team">The team ID which is a value of either '0', '1', or '2'.</param>
		public static void ZC_TEAMID(IZoneConnection conn, IActor actor, byte team)
		{
			var packet = new Packet(Op.ZC_TEAMID);
			packet.PutInt(actor.Handle);
			packet.PutByte(team);

			conn.Send(packet);
		}

		/// <summary>
		/// Updates actor's team id on the client.
		/// </summary>
		/// <param name="team">The team ID which is a value of either '0', '1', or '2'.</param>
		public static void ZC_TEAMID(Character character, byte team)
		{
			var packet = new Packet(Op.ZC_TEAMID);
			packet.PutInt(character.Handle);
			packet.PutByte(team);

			character.Map.Broadcast(packet);
		}

		/// <summary>
		/// Update actor's motion blur.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="blurState"></param>
		public static void ZC_MOTIONBLUR(IActor actor, bool blurState)
		{
			var packet = new Packet(Op.ZC_TEAMID);
			packet.PutInt(actor.Handle);
			packet.PutByte(blurState);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Notifies clients around the character about the action taken.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="argStr"></param>
		public static void ZC_CLIENT_DIRECT(Character character, int type, string argStr)
		{
			var packet = new Packet(Op.ZC_CLIENT_DIRECT);

			packet.PutInt(character.Handle);
			packet.PutInt(type);
			packet.PutString(argStr, 16);

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Makes the character the owner of actor.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="actor"></param>
		public static void ZC_OWNER(Character character, IActor actor)
		{
			var packet = new Packet(Op.ZC_OWNER);
			packet.PutInt(actor.Handle);
			packet.PutInt(character.Handle);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Draws square area on ground at position for characters in range
		/// of the source caster.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="position"></param>
		/// <param name="targetPosition"></param>
		/// <param name="width"></param>
		public static void ZC_SKILL_RANGE_DBG(IActor caster, Position position, Direction direction, Position targetPosition, float f1, float f2, float f3 = 0, float f4 = -1, float f5 = 0, float f6 = 0)
		{
			var packet = new Packet(Op.ZC_SKILL_RANGE_DBG);

			packet.PutInt(caster.Handle);
			packet.PutInt(0);
			packet.PutFloat(f1);
			packet.PutPosition(position);
			packet.PutDirection(direction);
			packet.PutPosition(targetPosition);
			packet.PutFloat(f2);
			packet.PutFloat(f3);
			packet.PutFloat(f4);
			packet.PutFloat(f5); // 0 = not drawn
			packet.PutFloat(f6);

			caster.Map.Broadcast(packet, caster);
		}

		/// <summary>
		/// Draws fan area on ground at position for characters in range
		/// of the caster.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="position"></param>
		/// <param name="direction"></param>
		/// <param name="radius"></param>
		/// <param name="radianHalfAngle"></param>
		public static void ZC_SKILL_RANGE_FAN(IActor caster, Position position, Direction direction, float radius, float radianHalfAngle)
		{
			var packet = new Packet(Op.ZC_SKILL_RANGE_FAN);

			packet.PutInt(caster.Handle);
			packet.PutFloat(position.X);
			packet.PutFloat(position.Y);
			packet.PutFloat(position.Z);
			packet.PutFloat(direction.Cos);
			packet.PutFloat(direction.Sin);
			packet.PutFloat(radius);
			packet.PutFloat(radianHalfAngle);
			packet.PutInt(1); // 0 = not drawn
			packet.PutInt(0);

			caster.Map.Broadcast(packet, caster);
		}

		/// <summary>
		/// Draws square area on ground at position for characters in range
		/// of the source caster.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="position"></param>
		/// <param name="targetPosition"></param>
		/// <param name="width"></param>
		public static void ZC_SKILL_RANGE_SQUARE(IActor caster, Position position, Position targetPosition, float width, bool drawn = true)
		{
			var packet = new Packet(Op.ZC_SKILL_RANGE_SQUARE);

			packet.PutInt(caster.Handle);
			packet.PutEmptyBin(2);
			packet.PutFloat(position.X);
			packet.PutFloat(position.Y);
			packet.PutFloat(position.Z);
			packet.PutFloat(targetPosition.X);
			packet.PutFloat(targetPosition.Y);
			packet.PutFloat(targetPosition.Z);
			packet.PutFloat(width);
			packet.PutInt(drawn ? 1 : 0); // 0 = not drawn
			packet.PutInt(0);

			caster.Map.Broadcast(packet, caster);
		}

		/// <summary>
		/// Draws circle area on ground at position for characters in range
		/// of the caster.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="position"></param>
		/// <param name="radius"></param>
		public static void ZC_SKILL_RANGE_CIRCLE(IActor caster, Position position, float radius)
		{
			var packet = new Packet(Op.ZC_SKILL_RANGE_CIRCLE);

			packet.PutInt(caster.Handle);
			if (Versions.Protocol > 500)
				packet.PutEmptyBin(2);
			else
			{
				packet.PutByte(1);
				packet.PutByte(1);
			}
			packet.PutFloat(position.X);
			packet.PutFloat(position.Y);
			packet.PutFloat(position.Z);
			packet.PutFloat(radius);
			if (Versions.Protocol > 500)
			{
				packet.PutInt(1); // 0 = not drawn
				packet.PutInt(0); // 1 = drawn weaker?
			}

			caster.Map.Broadcast(packet, caster);
		}

		/// <summary>
		/// Debug Position
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="fromCellPos"></param>
		/// <param name="toCellPos"></param>
		public static void ZC_POS_DBG(IActor actor, Position fromCellPos, Position toCellPos)
		{
			var packet = new Packet(Op.ZC_POS_DBG);

			packet.PutInt(actor.Handle);
			packet.PutInt((int)fromCellPos.X);
			packet.PutInt((int)fromCellPos.Z);
			packet.PutInt((int)fromCellPos.Y);
			packet.PutInt((int)toCellPos.X);
			packet.PutInt((int)toCellPos.Z);
			packet.PutInt((int)toCellPos.Y);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Updates entity's attack state.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="inAttackState"></param>
		public static void ZC_PC_ATKSTATE(ICombatEntity entity, bool inAttackState)
		{
			var packet = new Packet(Op.ZC_PC_ATKSTATE);
			packet.PutInt(entity.Handle);
			packet.PutByte(inAttackState);

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Updates creature's SP
		/// </summary>
		/// <param name="character"></param>
		/// <param name="currentSp"></param>
		/// <param name="displayGain">
		/// If true, the client displays the amount of SP gained
		/// above the character's head. Doesn't display anything
		/// if SP were lost.
		/// </param>
		public static void ZC_UPDATE_SP(Character character, float currentSp, bool displayGain)
		{
			var packet = new Packet(Op.ZC_UPDATE_SP);
			packet.PutInt(character.Handle);
			packet.PutInt((int)currentSp);
			packet.PutByte(displayGain);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates an actor's Max Hp
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="maxHp"></param>
		public static void ZC_UPDATE_MHP(IActor actor, int maxHp)
		{
			var packet = new Packet(Op.ZC_UPDATE_MHP);

			packet.PutInt(actor.Handle);
			packet.PutInt(maxHp);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Updates a characters HP for damage and healing.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="amount"></param>
		/// <param name="currentHp"></param>
		/// <param name="priority"></param>
		public static void ZC_ADD_HP(IActor actor, float amount, float currentHp, int priority)
		{
			// For some reason they send 1 for the amount if the expected
			// amount was negative, such as in the case of damage?
			// Or at least this appeared to be the case at some point in
			// time. We should probably double-check it, but then again,
			// the client doesn't really use that value. It simply
			// takes the new HP to update its UI.

			var isDamage = (amount < 0);
			var adjustedAmount = (isDamage ? 1 : amount);

			var packet = new Packet(Op.ZC_ADD_HP);
			packet.PutInt(actor.Handle);
			packet.PutInt((int)adjustedAmount);
			packet.PutInt((int)currentHp);
			packet.PutInt(priority);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Updates creature's HP and SP stats.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="priority"></param>
		public static void ZC_UPDATE_ALL_STATUS(ICombatEntity entity, int priority)
		{
			var hp = (int)entity.Properties.GetFloat(PropertyName.HP);
			var maxHp = (int)entity.Properties.GetFloat(PropertyName.MHP);
			var sp = (int)entity.Properties.GetFloat(PropertyName.SP);
			var maxSp = (int)entity.Properties.GetFloat(PropertyName.MSP);

			var packet = new Packet(Op.ZC_UPDATE_ALL_STATUS);

			packet.PutInt(entity.Handle);
			packet.PutInt(hp);
			packet.PutInt(maxHp);
			if (Versions.Protocol > 500)
			{
				packet.PutInt(sp);
				packet.PutInt(maxSp);
			}
			else
			{
				packet.PutShort(sp);
				packet.PutShort(maxSp);
			}
			packet.PutInt(priority);

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Updates character's HP or SP and displays a floating text
		/// with the modifier, notifying the player of the change.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="amount"></param>
		/// <param name="newValue"></param>
		/// <param name="type"></param>
		public static void ZC_HEAL_INFO(ICombatEntity entity, float amount, float newValue, HealType type)
		{
			var packet = new Packet(Op.ZC_HEAL_INFO);

			packet.PutInt(entity.Handle);
			packet.PutInt((int)amount);
			packet.PutInt((int)newValue);
			packet.PutInt(0);
			if (Versions.Protocol > 500)
			{
				packet.PutShort(0);
				packet.PutShort(1);
			}
			packet.PutShort((short)type); // !0 = blue text
			packet.PutInt(0);
			packet.PutShort(0);

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Set an entity's state as friendly or hostile
		/// </summary>
		public static void ZC_CHANGE_RELATION(IZoneConnection conn, int handle, RelationType relation)
		{
			var packet = new Packet(Op.ZC_CHANGE_RELATION);

			packet.PutInt(handle);
			packet.PutByte((byte)relation);

			conn.Send(packet);
		}

		/// <summary>
		/// Updates the stance of a character.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_STANCE_CHANGE(Character character)
		{
			var packet = new Packet(Op.ZC_STANCE_CHANGE);
			packet.PutInt(character.Handle);
			packet.PutInt(character.Stance);

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Updates the actor's faction on the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="actor"></param>
		/// <param name="faction"></param>
		public static void ZC_FACTION(IZoneConnection conn, IActor actor, FactionType faction)
		{
			var packet = new Packet(Op.ZC_FACTION);

			packet.PutInt(actor.Handle);
			packet.PutInt((int)faction);

			conn.Send(packet);
		}

		/// <summary>
		/// Updates the actor's faction on the client.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_FACTION(IActor actor)
		{
			var packet = new Packet(Op.ZC_FACTION);

			packet.PutInt(actor.Handle);
			packet.PutInt((int)actor.Faction);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Sends a list of help topics to the client.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_HELP_LIST(Character character)
		{
			// Get only the basic help topics for now. We probably need
			// a character or account based list of help topics the
			// player can see, potentially incl. the information of
			// whether they've read a specific topic yet.

			var defaultList = ZoneServer.Instance.Data.HelpDb.Entries.Values.Where(a => a.BasicHelp);

			var packet = new Packet(Op.ZC_HELP_LIST);

			packet.PutInt(defaultList.Count());
			foreach (var data in defaultList)
			{
				packet.PutInt(data.Id);
				packet.PutByte(0); // Unknown, maybe "has seen" toggle?
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Opens help panel for the given topic on character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="helpTopicId"></param>
		/// <param name="maybeSeen"></param>
		public static void ZC_HELP_ADD(Character character, int helpTopicId, bool maybeSeen)
		{
			var packet = new Packet(Op.ZC_HELP_ADD);
			packet.PutInt(helpTopicId);
			packet.PutByte(maybeSeen);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Requests the client to send information that needs to be saved
		/// before exiting?
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_SAVE_INFO(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_SAVE_INFO);
			conn.Send(packet);
		}

		/// <summary>
		/// Acknowledges the client that the loading screen has completed.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_LOAD_COMPLETE(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_LOAD_COMPLETE);
			conn.Send(packet);
		}

		/// <summary>
		/// Send to open up the attendance reward UI (supposed when an attendance reward is available to be retrieved). 
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_ATTENDANCE_REWARD_CHECK_UI_ON(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_ATTENDANCE_REWARD_CHECK_UI_ON);
			packet.PutByte(false);
			packet.PutByte(true);

			conn.Send(packet);
		}

		/// <summary>
		/// Sent to check previously attendance rewarded items.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="eventAttendanceData"></param>
		/// <param name="rewards"></param>
		/// <param name="openUi"></param>
		public static void ZC_ATTENDANCE_RECEIPT_REWARD(IZoneConnection conn, EventAttendanceData eventAttendanceData, List<AttendanceRewardEntry> rewards, bool openUi)
		{
			var packet = new Packet(Op.ZC_ATTENDANCE_RECEIPT_REWARD);

			packet.PutShortDate(eventAttendanceData.StartTime);
			packet.PutShortDate(eventAttendanceData.EndTime);
			packet.PutInt(eventAttendanceData.Id);
			packet.PutInt(rewards.Count);

			for (var i = 0; i < rewards.Count; i++)
			{
				var openUiValue = openUi ? 1 : 256;
				packet.PutShort(i == 0 ? openUiValue : 0);
				packet.PutInt(eventAttendanceData.Id);
				packet.PutShortDate(rewards[i].RewardTime);
				packet.PutShort(rewards[i].DayOffSet);
			}

			packet.PutShort(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends session objects to character's client.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SESSION_OBJECTS(Character character)
		{
			var sessionObjects = character.SessionObjects.GetList();

			var packet = new Packet(Op.ZC_SESSION_OBJECTS);

			packet.PutShort(sessionObjects.Length);
			packet.PutByte(false);
			packet.PutFloat(565831.1f);

			foreach (var sessionObject in sessionObjects)
			{
				var propertyList = sessionObject.Properties.GetAll();
				var propertiesSize = propertyList.GetByteCount();

				packet.PutInt(sessionObject.Id);
				packet.PutInt(0);
				packet.PutLong(sessionObject.ObjectId);
				packet.PutFloat(0);
				packet.PutShort(propertiesSize);
				packet.PutShort(0); // Alignment?

				packet.AddProperties(propertyList);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates premium state properties on client.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_SEND_CASH_VALUE(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_SEND_CASH_VALUE);

			// Normal
			packet.PutInt(4); // count?
			{
				packet.PutLpString("speedUp");
				if (Versions.Protocol > 500)
					packet.PutDouble(0);
				else
					packet.PutFloat(0);

				// Product registration possible regardless of market upper/lower limit
				packet.PutLpString("marketUpMax");
				if (Versions.Protocol > 500)
					packet.PutDouble(1);
				else
					packet.PutFloat(1);

				packet.PutLpString("marketSellCom");
				if (Versions.Protocol > 500)
					packet.PutDouble(30);
				else
					packet.PutFloat(30);

				packet.PutLpString("abilityMax");
				if (Versions.Protocol > 500)
					packet.PutDouble(1);
				else
					packet.PutFloat(1);
			}

			// Premium Type 1
			packet.PutInt(4);
			{
				packet.PutLpString("speedUp");
				if (Versions.Protocol > 500)
					packet.PutDouble(3);
				else
					packet.PutFloat(3);

				packet.PutLpString("marketUpMax");
				if (Versions.Protocol > 500)
					packet.PutDouble(5);
				else
					packet.PutFloat(5);

				packet.PutLpString("marketSellCom");
				if (Versions.Protocol > 500)
					packet.PutDouble(10);
				else
					packet.PutFloat(10);

				packet.PutLpString("abilityMax");
				if (Versions.Protocol > 500)
					packet.PutDouble(3);
				else
					packet.PutFloat(3);
			}

			// Token
			packet.PutInt(4);
			{
				packet.PutLpString("speedUp");
				if (Versions.Protocol > 500)
					packet.PutDouble(3);
				else
					packet.PutFloat(3);

				packet.PutLpString("marketUpMax");
				if (Versions.Protocol > 500)
					packet.PutDouble(10);
				else
					packet.PutFloat(10);

				packet.PutLpString("marketSellCom");
				if (Versions.Protocol > 500)
					packet.PutDouble(10);
				else
					packet.PutFloat(10);

				packet.PutLpString("abilityMax");
				if (Versions.Protocol > 500)
					packet.PutDouble(2);
				else
					packet.PutFloat(2);
			}

			// ?
			packet.PutInt(4);
			{
				packet.PutInt(7);
				if (Versions.Protocol > 500)
					packet.PutDouble(2.5f);
				else
					packet.PutFloat(2.5f);

				packet.PutInt(5);
				if (Versions.Protocol > 500)
					packet.PutDouble(2);
				else
					packet.PutFloat(2);

				packet.PutInt(3);
				if (Versions.Protocol > 500)
					packet.PutDouble(1.5f);
				else
					packet.PutFloat(1.5f);

				packet.PutInt(1);
				if (Versions.Protocol > 500)
					packet.PutDouble(1);
				else
					packet.PutFloat(1);
			}

			packet.PutInt(33);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_DIRECTION_APC packet to initialize track animation.
		/// </summary>
		/// <param name="conn">The connection to send to.</param>
		/// <param name="actor">The actor to animate.</param>
		/// <param name="packetStringId">The packet string ID for the track.</param>
		/// <param name="i1">Track start index.</param>
		/// <param name="i2">Track end index.</param>
		/// <param name="f1">Time parameter.</param>
		public static void ZC_DIRECTION_APC(IZoneConnection conn, IActor actor, int packetString, int i1, int i2, float f1)
		{
			var packet = new Packet(Op.ZC_DIRECTION_APC);

			packet.PutInt(actor.Handle);
			packet.PutInt(packetString);
			packet.PutInt(i1);
			packet.PutInt(i2);
			packet.PutFloat(f1);

			conn.Send(packet);
		}

		/// <summary>
		/// Updates premium state on client.
		/// </summary>
		/// <remarks>
		/// May crash the client if a correct ZC_SEND_CASH_VALUE packet was not
		/// sent first.
		/// </remarks>
		/// <param name="conn"></param>
		/// <param name="state"></param>
		public static void ZC_SEND_PREMIUM_STATE(IZoneConnection conn, PremiumState state)
		{
			var packet = new Packet(Op.ZC_SEND_PREMIUM_STATE);

			packet.PutByte((byte)state.Type);
			packet.PutByte(state.Active);
			packet.PutInt(state.RemainingSeconds);
			packet.PutInt(state.NumArg);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_RESPONSE_GUILD_INDEX to client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		/// <param name="guild"></param>
		// Removed: Guild type deleted during Laima merge
		// public static void ZC_RESPONSE_GUILD_INDEX(IZoneConnection conn, Character character, Guild guild) { ... }
		public static void ZC_RESPONSE_GUILD_INDEX(IZoneConnection conn, Character character, object guild)
		{
			var packet = new Packet(Op.ZC_RESPONSE_GUILD_INDEX);

			packet.PutInt(character.Handle);
			packet.PutLong(0); // Guild Id - always 0 since Guild type removed
			packet.PutShort(1001);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_SET_CHATBALLOON_SKIN to visible players on the map.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SET_CHATBALLOON_SKIN(Character character)
		{
			var packet = new Packet(Op.ZC_SET_CHATBALLOON_SKIN);

			packet.PutInt(character.Handle);
			packet.PutInt(character.ChatBalloon);
			packet.PutShortDate(character.ChatBalloonExpiration);

			character.Map.Broadcast(packet);
		}

		/// <summary>
		/// Sends ZC_MYPAGE_MAP to client (dummy).
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_MYPAGE_MAP(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_MYPAGE_MAP);

			packet.PutInt(1);
			packet.PutByte(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_GUESTPAGE_MAP to client (dummy).
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_GUESTPAGE_MAP(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_GUESTPAGE_MAP);

			packet.PutInt(1);
			packet.PutByte(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends PC Bang rental item list to client (dummy)..
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_PCBANG_SHOP_RENTAL(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_PCBANG_SHOP_RENTAL);

			packet.PutInt(29); // Count

			packet.PutString("PC_SWD01_137", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_TSW01_129", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_STF01_137", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_TBW01_137", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_BOW01_130", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_MAC01_136", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_SPR01_119", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_TSP01_111", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_TSF01_129", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_RAP01_103", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_TOP01_139", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_LEG01_139", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_FOOT01_139", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_HAND01_139", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_TOP01_140", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_LEG01_140", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_FOOT01_140", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_HAND01_140", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_TOP01_141", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_LEG01_141", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_FOOT01_141", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_HAND01_141", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_SHD100_101", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_DAG100_101", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_CAN100_101", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_PST100_101", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_MUS100_101", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_TMAC02_103", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			packet.PutString("PC_TRK02_101", 64);
			packet.PutInt(0);
			packet.PutInt(11);
			packet.PutInt(10);
			packet.PutInt(1);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_RES_BEAUTYSHOP_PURCHASED_HAIR_LIST to character (dummy).
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_RES_BEAUTYSHOP_PURCHASED_HAIR_LIST(Character character)
		{
			var packet = new Packet(Op.ZC_RES_BEAUTYSHOP_PURCHASED_HAIR_LIST);

			packet.PutInt(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_ADDITIONAL_SKILL_POINT to character (dummy).
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_ADDITIONAL_SKILL_POINT(Character character)
		{
			var packet = new Packet(Op.ZC_ADDITIONAL_SKILL_POINT);
			packet.PutInt(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_HARDCODED_SKILL to the character.
		/// </summary>
		/// <param name="summon"></param>
		public static void ZC_HARDCODED_SKILL(Summon summon, int i3)
		{
			var packet = new Packet(Op.ZC_HARDCODED_SKILL);

			packet.PutInt(1);
			packet.PutInt(summon.OwnerHandle);
			packet.PutInt(summon.Handle);
			packet.PutInt(summon.Id);
			packet.PutInt(i3);

			summon.Map.Broadcast(packet, summon);
		}

		/// <summary>
		/// Display a "ball of stamina" flying from the the monster to the character.
		/// </summary>
		/// <param name="toActor"></param>
		/// <param name="fromActor"></param>
		/// <param name="stamina"></param>
		public static void ZC_MON_STAMINA(IActor toActor, IActor fromActor, int stamina)
		{
			var packet = new Packet(Op.ZC_MON_STAMINA);

			packet.PutInt(fromActor.Handle);
			packet.PutInt(toActor.Handle);
			packet.PutInt(0); // Custom script id, sent back via CZ_CUSTOM_SCP if != 0
			packet.PutInt(stamina);

			toActor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Brocasts action PKS (?) in range of the receiving actor.
		/// </summary>
		/// <param name="toActor"></param>
		/// <param name="fromActor"></param>
		/// <param name="type"></param>
		/// <param name="numArg1"></param>
		/// <param name="numArg2"></param>
		/// <param name="numArg3"></param>
		/// <param name="numArg4"></param>
		/// <param name="numArg5"></param>
		public static void ZC_ACTION_PKS(IActor toActor, IActor fromActor, byte type, int numArg1 = 0, int numArg2 = 0, int numArg3 = 0, int numArg4 = 0, int numArg5 = 0)
		{
			var packet = new Packet(Op.ZC_ACTION_PKS);

			packet.PutInt(fromActor.Handle);
			packet.PutInt(toActor.Handle);
			packet.PutByte(type);
			packet.PutInt(numArg1);
			packet.PutInt(numArg2);
			packet.PutInt(numArg3);
			packet.PutInt(numArg4);
			packet.PutInt(numArg5);

			toActor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Shakes actor
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="duration"></param>
		/// <param name="vibrationStrength"></param>
		/// <param name="frequency"></param>
		/// <param name="verticalSpeed"></param>
		/// <param name="i1"></param>
		public static void ZC_VIBRATE(IActor actor, float duration, float vibrationStrength, float frequency, float verticalSpeed, int i1 = 0)
		{
			var packet = new Packet(Op.ZC_VIBRATE);

			packet.PutInt(actor.Handle);
			packet.PutFloat(duration);
			packet.PutFloat(vibrationStrength);
			packet.PutFloat(frequency);
			packet.PutFloat(verticalSpeed);
			packet.PutInt(i1);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Sends ZC_SET_DAYLIGHT_INFO to character (dummy).
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SET_DAYLIGHT_INFO(Character character)
		{
			var packet = new Packet(Op.ZC_SET_DAYLIGHT_INFO);
			packet.PutInt(292);
			packet.PutLong(1);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates the daylight settings for the given character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="enabled"></param>
		/// <param name="parameters"></param>
		public static void ZC_DAYLIGHT_FIXED(Character character, bool enabled, DaylightParameters parameters)
		{
			var packet = new Packet(Op.ZC_DAYLIGHT_FIXED);

			packet.PutInt(enabled ? 1 : 0);
			packet.PutByte(0);
			packet.PutFloat(parameters.FR);
			packet.PutFloat(parameters.FG);
			packet.PutFloat(parameters.FB);
			packet.PutFloat(parameters.MapLightStrength);
			packet.PutFloat(parameters.ModelLightStrength);

			// [i361296]
			{
				packet.PutByte(0);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates the daylight settings for all characters on all maps.
		/// </summary>
		/// <param name="enabled"></param>
		/// <param name="parameters"></param>
		public static void ZC_DAYLIGHT_FIXED(bool enabled, DaylightParameters parameters)
		{
			var packet = new Packet(Op.ZC_DAYLIGHT_FIXED);

			packet.PutInt(enabled ? 1 : 0);
			packet.PutByte(0);
			packet.PutFloat(parameters.FR);
			packet.PutFloat(parameters.FG);
			packet.PutFloat(parameters.FB);
			packet.PutFloat(parameters.MapLightStrength);
			packet.PutFloat(parameters.ModelLightStrength);

			// [i361296]
			{
				packet.PutByte(0);
			}

			ZoneServer.Instance.World.Broadcast(packet);
		}

		/// <summary>
		/// Setup a shop
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_AUTOSELLER_LIST(Character character)
		{
			var shop = character.Connection.ShopCreated;
			if (shop == null)
			{
				Log.Error("ZC_AUTOSELLER_LIST: {0} doesn't have a shop open.", character.Connection.Account.Name);
				return;
			}

			var packet = new Packet(Op.ZC_AUTOSELLER_LIST);

			packet.PutInt(character.Handle);
			packet.AddStringId(shop.ShopAnimation);
			packet.PutByte(shop.IsClosed);
			packet.PutInt((int)shop.Type);
			packet.PutInt(shop.SkillIcon);
			if (!shop.IsClosed)
			{
				packet.PutInt(shop.Level);
				packet.PutString(shop.Name, 64);
				packet.PutInt(shop.Products.Count);
				foreach (var product in shop.Products.Values)
				{
					packet.PutInt(product.ItemId);
					packet.PutInt(product.RequiredAmount); // Amount Left
					packet.PutInt(product.Price);
					packet.PutInt(product.Amount);
					if (shop.SkillIcon == 0)
						packet.PutEmptyBin(260);
					else
						packet.PutEmptyBin(256);
				}
			}
			else
			{
				packet.PutInt(0);
				packet.PutString("", 64);
				packet.PutInt(0);
			}

			character.Map.Broadcast(packet);
		}

		/// <summary>
		/// Setup a shop
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_AUTOSELLER_LIST(IZoneConnection conn, Character character)
		{
			var shop = character.Connection.ShopCreated;
			if (shop == null)
			{
				Log.Error("ZC_AUTOSELLER_LIST: {0} doesn't have a shop open.", character.Connection.Account.Name);
				return;
			}

			var packet = new Packet(Op.ZC_AUTOSELLER_LIST);

			packet.PutInt(character.Handle);
			packet.PutInt(shop.EffectId);
			packet.PutByte(shop.IsClosed);
			packet.PutInt((int)shop.Type);
			packet.PutInt(shop.SkillIcon);
			if (!shop.IsClosed)
			{
				packet.PutInt(shop.Level);
				packet.PutString(shop.Name, 64);
				packet.PutInt(shop.Products.Count);
				if (shop.SkillIcon == (int)SkillId.Pardoner_Oblation)
					packet.PutInt(0);
				foreach (var product in shop.Products.Values)
				{
					packet.PutInt(product.ItemId);
					packet.PutInt(product.RequiredAmount); // Amount Left
					packet.PutInt(product.Price);
					packet.PutInt(product.Amount);
					if (shop.SkillIcon == 0)
						packet.PutEmptyBin(260);
					else
						packet.PutEmptyBin(256);
				}
			}
			else
			{
				packet.PutInt(0);
				packet.PutString("", 64);
				packet.PutInt(0);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Show Shop Title
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_AUTOSELLER_TITLE(Character character)
		{
			var shop = character.Connection.ShopCreated;
			if (shop == null)
			{
				Log.Error("ZC_AUTOSELLER_TITLE: {0} doesn't have a shop open.", character.Connection.Account.Name);
				return;
			}

			var packet = new Packet(Op.ZC_AUTOSELLER_TITLE);

			packet.PutInt(character.Handle);
			packet.PutInt((int)shop.Type);
			if (!shop.IsClosed)
				packet.PutString(shop.Name, 64);
			else
				packet.PutString("", 64);
			packet.PutInt(shop.SkillIcon);
			packet.PutInt(shop.Level);
			packet.PutByte(0);

			character.Map.Broadcast(packet);
		}

		/// <summary>
		/// Show Shop Title
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_AUTOSELLER_TITLE(IZoneConnection conn, Character character)
		{
			var shop = character.Connection.ShopCreated;
			if (shop == null)
			{
				Log.Error("ZC_AUTOSELLER_TITLE: {0} doesn't have a shop open.", character.Connection.Account.Name);
				return;
			}

			var packet = new Packet(Op.ZC_AUTOSELLER_TITLE);

			packet.PutInt(character.Handle);
			packet.PutInt((int)shop.Type);
			if (!shop.IsClosed)
				packet.PutString(shop.Name, 64);
			else
				packet.PutString("", 64);
			packet.PutInt(shop.SkillIcon);
			packet.PutInt(shop.Level);
			packet.PutByte(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Show Shop Title
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_AUTOSELLER_TITLE(IZoneConnection conn, Character character, ShopData shop)
		{
			if (shop == null)
			{
				Log.Error("ZC_AUTOSELLER_TITLE: {0} doesn't have a shop open.", character.Connection.Account.Name);
				return;
			}

			var packet = new Packet(Op.ZC_AUTOSELLER_TITLE);

			packet.PutInt(character.Handle);
			packet.PutInt((int)shop.Type);
			if (!shop.IsClosed)
				packet.PutString(shop.Name, 64);
			else
				packet.PutString("", 64);
			packet.PutInt(shop.SkillIcon);
			packet.PutInt(shop.Level);
			packet.PutByte(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Sent on spawning a summoned monster
		/// </summary>
		/// <param name="character"></param>
		/// <param name="summon"></param>
		public static void ZC_IS_SUMMON_SORCERER_MONSTER(Character character, IMonster summon)
		{
			var packet = new Packet(Op.ZC_IS_SUMMON_SORCERER_MONSTER);

			packet.PutInt(summon.Handle);
			packet.PutByte(character.Handle == summon.OwnerHandle); // I think this 0 if it's not your monster

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sent on spawning a summoned monster
		/// </summary>
		/// <param name="character"></param>
		/// <param name="summon"></param>
		public static void ZC_IS_SUMMON_SORCERER_MONSTER(Character character, IMonster summon, bool isOwner)
		{
			var packet = new Packet(Op.ZC_IS_SUMMON_SORCERER_MONSTER);

			packet.PutInt(summon.Handle);
			packet.PutByte(isOwner); // I think this 0 if it's not your monster

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates the character's damage font skin (?) on the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_RES_DAMAGEFONT_SKIN(IZoneConnection conn, Character character)
		{
			var skinId = character.Variables.Perm.GetInt("Melia.DamageFontSkin", 1);

			var packet = new Packet(Op.ZC_RES_DAMAGEFONT_SKIN);

			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(character.Handle);
			packet.PutInt(skinId);

			conn.Send(packet);
		}

		/// <summary>
		/// Updates the character's damage effect skin (?) on the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_RES_DAMAGEEFFECT_SKIN(IZoneConnection conn, Character character)
		{
			var skinId = character.Variables.Perm.GetInt("Melia.DamageEffectSkin", 1);

			var packet = new Packet(Op.ZC_RES_DAMAGEEFFECT_SKIN);

			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(character.Handle);
			packet.PutInt(skinId);

			conn.Send(packet);
		}

		/// <summary>
		/// Summons a Kupole?
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_SUMMON_CUPOLE(IZoneConnection conn, Character character)
		{
			var kupoleId = character.Variables.Perm.GetInt("Melia.KupoleId", 1);

			var packet = new Packet(Op.ZC_SUMMON_CUPOLE);

			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(character.Handle);
			packet.PutInt(kupoleId);

			conn.Send(packet);
		}

		/// <summary>
		/// Summons a Kupole?
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SUMMON_CUPOLE(Character character, int kupoleId = -1)
		{
			if (kupoleId == -1)
				kupoleId = character.Variables.Perm.GetInt("Melia.KupoleId", 1);

			var packet = new Packet(Op.ZC_SUMMON_CUPOLE);

			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(character.Handle);
			packet.PutInt(kupoleId);

			character.Map.Broadcast(packet);
		}

		/// <summary>
		/// Unsummons a Kupole?
		/// </summary>
		/// <remarks>Doesn't work</remarks>
		/// <param name="character"></param>
		public static void ZC_UNSUMMON_CUPOLE(Character character)
		{
			var packet = new Packet(Op.ZC_UNSUMMON_CUPOLE);

			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(character.Handle);
			packet.PutInt(0);

			character.Map.Broadcast(packet);
		}

		/// <summary>
		/// Sends companion auto-attack mode status to client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="companion"></param>
		public static void ZC_PET_AUTO_ATK(Character character, Companion companion)
		{
			var packet = new Packet(Op.ZC_PET_AUTO_ATK);
			packet.PutLong(companion.ObjectId);
			packet.PutInt(0); // Unknown
			packet.PutByte(companion.IsAggressiveMode ? (byte)1 : (byte)0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates entity's guard state on clients in range.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="active"></param>
		/// <param name="dir"></param>
		public static void ZC_GUARD(ICombatEntity entity, bool active, Direction dir)
		{
			var packet = new Packet(Op.ZC_GUARD);

			packet.PutInt(entity.Handle);
			packet.PutByte(active);
			packet.PutDirection(dir);

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Plays sound for clients in range of the actor, sending the packet with
		/// either the male or female string, depending on the actor's gender.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="packetStringMale"></param>
		/// <param name="packetStringFemale"></param>
		public static void ZC_PLAY_SOUND_Gendered(IActor actor, string packetStringMale, string packetStringFemale)
		{
			var gender = (actor is Character character ? character.Gender : Gender.Male);
			var packetString = (gender == Gender.Male ? packetStringMale : packetStringFemale);

			ZC_PLAY_SOUND(actor, packetString);
		}

		/// <summary>
		/// Stops the sound for all clients in range of the actor, sending
		/// the packet with either the male or female string, depending on
		/// the actor's gender.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="packetStringMale"></param>
		/// <param name="packetStringFemale"></param>
		public static void ZC_STOP_SOUND_Gendered(IActor actor, string packetStringMale, string packetStringFemale)
		{
			var gender = (actor is Character character ? character.Gender : Gender.Male);
			var packetString = (gender == Gender.Male ? packetStringMale : packetStringFemale);

			ZC_STOP_SOUND(actor, packetString);
		}

		/// <summary>
		/// Updates character's stamina, setting it to the given value.
		/// </summary>
		/// <remarks>
		/// Stamina is communicated in thousands, so 1000 is 1 stamina point.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="stamina"></param>
		public static void ZC_STAMINA(Character character, int stamina)
		{
			var packet = new Packet(Op.ZC_STAMINA);
			packet.PutInt(stamina);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_WEEKLY_BOSS_ACCUMULATED_DAMAGE to character (dummy).
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_WEEKLY_BOSS_ACCUMULATED_DAMAGE(Character character)
		{
			var packet = new Packet(Op.ZC_WEEKLY_BOSS_ACCUMULATED_DAMAGE);
			packet.PutLong(42); // Subsequent packets increase this value
			packet.PutInt(0);

			character.Connection.Send(packet);
		}

		public static void ZC_WEEKLY_BOSS_ABSOLUTE_REWARD_LIST(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_WEEKLY_BOSS_ABSOLUTE_REWARD_LIST);

			packet.PutInt(0); // Week Number
			packet.PutInt(0); // Count

			/**
			typedef struct
			{
				lpString itemName1;
				int i4;
			} RewardItem <optimize=false>;

			typedef struct
			{
				int64 l1;
				int itemCount;
				RewardItem items[itemCount];
			} RewardEntry <optimize=false>;

			int weekNum; // 90
			int count; // 17

			RewardEntry entries[count];

			foreach (var entry in character.WeeklyBossRewardList)
			{
				packet.PutLong(entry.AmountRequired);
				packet.PutInt(entry.ItemCount);
				foreach (var item in entry.Items)
				{
					packet.PutLpString(item.Name);
					packet.PutInt(item.Amout);
				}
			}
			**/

			conn.Send(packet);
		}

		public static void ZC_WEEKLY_BOSS_RANKING_REWARD_LIST(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_WEEKLY_BOSS_RANKING_REWARD_LIST);

			/**
			typedef struct
			{
				lpString itemName1;
				int i4;
			} RewardItem <optimize=false>;

			typedef struct
			{
				int64 l1;
				int itemCount;
				RewardItem items[itemCount];
			} RewardEntry <optimize=false>;

			int weekNum; // 90
			int count; // 17

			RewardEntry entries[count];
			**/

			packet.PutInt(0); // Week Number
			packet.PutInt(0); // Count			

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_PCBANG_SHOP_COMMON to character.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_PCBANG_SHOP_COMMON(Character character)
		{
			var packet = new Packet(Op.ZC_PCBANG_SHOP_COMMON);

			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(character.Connection.Account.PopoPoints);
			packet.PutInt(0);

			packet.PutShort(2021);
			packet.PutShort(7);
			packet.PutShort(3);
			packet.PutShort(28);
			packet.PutLong(0);

			packet.PutShort(2021);
			packet.PutShort(10);
			packet.PutShort(5);
			packet.PutShort(1);
			packet.PutLong(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends Popo Shop Catalog (Items for sale).
		/// Dummy implementation, Replace with a scriptable function or db?
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_PCBANG_SHOP_POINTSHOP_CATALOG(Character character)
		{
			var packet = new Packet(Op.ZC_PCBANG_SHOP_POINTSHOP_CATALOG);
			var count = 100;
			var dateTime = DateTime.Now;

			packet.PutInt(count);
			for (var i = 0; i < count; i++)
			{
				// For each item structure
				packet.PutInt(i + 1);
				packet.PutString("Common_" + i, 64);
				packet.PutInt(0); // Purchase Count
				packet.PutInt((i % 3) + 1); // Shop Category Type (1 = Regular, 2 = Rotational, 3 = Event)
				packet.PutInt(100 + i); // Item Cost
				packet.PutInt(999); // Maximum Purchase Amount
				packet.PutString(ZoneServer.Instance.Data.ItemDb.Entries.Values.Random().ClassName, 64);
				packet.PutInt(0);
				packet.PutInt(20); // Item Amount
				packet.PutInt(dateTime.Year);
				packet.PutInt(dateTime.Month);
				packet.PutInt(1);
				packet.PutInt(dateTime.Year);
				packet.PutInt(dateTime.Month);
				packet.PutInt(dateTime.AddDays(-1).Day);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Popo shop items bought count
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_PCBANG_SHOP_POINTSHOP_BUY_COUNT(Character character)
		{
			var packet = new Packet(Op.ZC_PCBANG_SHOP_POINTSHOP_BUY_COUNT);
			var count = 0;

			packet.PutInt(count);
			for (var i = 0; i < count; i++)
			{
				//packet.PutLong(entity.Connection.Account.ObjectId);
				//packet.PutInt(itemId);
				//packet.PutInt(itemAmount);
				//packet.PutShortDate(purchaseDate);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_TRUST_INFO to character (dummy).
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_TRUST_INFO(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_TRUST_INFO);

			packet.PutEmptyBin(20);
			packet.PutLong(1000000);
			packet.PutLong(30000000);
			packet.PutLong(15000000);
			packet.PutLong(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Object properties sent via Property Name instead of Id
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="objectId"></param>
		/// <param name="properties"></param>
		public static void ZC_OBJECT_PROPERTY_BY_NAMES(IZoneConnection conn, long objectId, PropertyList properties)
		{
			var packet = new Packet(Op.ZC_OBJECT_PROPERTY_BY_NAMES);

			packet.PutLong(objectId);
			packet.PutInt(properties.Count);
			foreach (var property in properties)
			{
				packet.PutLpString(property.Ident);
				packet.PutByte((byte)property.Type);
				switch (property)
				{
					case FloatProperty floatProperty: packet.PutFloat(floatProperty.Value); break;
					case StringProperty stringProperty: packet.PutLpString(stringProperty.Value); break;
				}
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Set your character state as friendly or hostile mode?
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="isHostile"></param>
		public static void ZC_FRIENDLY_STATE(IZoneConnection conn, bool isHostile)
		{
			var packet = new Packet(Op.ZC_FRIENDLY_STATE);

			packet.PutByte(isHostile);

			conn.Send(packet);
		}

		/// <summary>
		/// Silver gacha win?
		/// Goddess' Grace?
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="itemId"></param>
		public static void ZC_WIN_FIELDBOSS_WORLD_EVENT_ITEM(Character entity, int itemId)
		{
			var packet = new Packet(Op.ZC_WIN_FIELDBOSS_WORLD_EVENT_ITEM);

			packet.PutInt(itemId);

			entity.Connection.Send(packet);
		}

		/// <summary>
		/// Silver gacha lose?
		/// Goddess' Grace?
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="itemId"></param>
		public static void ZC_LOSE_FIELDBOSS_WORLD_EVENT_ITEM(Character entity, int itemId)
		{
			var packet = new Packet(Op.ZC_LOSE_FIELDBOSS_WORLD_EVENT_ITEM);

			packet.PutInt(itemId);

			entity.Connection.Send(packet);
		}

		/// <summary>
		/// Weekly Boss Start Time
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_WEEKLY_BOSS_START_TIME(Character character)
		{
			var packet = new Packet(Op.ZC_WEEKLY_BOSS_START_TIME);

			packet.PutShortDate(DateTime.Now);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Weekly Boss End Time
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_WEEKLY_BOSS_END_TIME(Character character)
		{
			var packet = new Packet(Op.ZC_WEEKLY_BOSS_END_TIME);

			packet.PutShortDate(DateTime.Now.AddDays(1));

			character.Connection.Send(packet);
		}

		public static void ZC_WEEKLY_BOSS_PATTERN_INFO(Character character, MonsterData boss)
		{
			var packet = new Packet(Op.ZC_WEEKLY_BOSS_PATTERN_INFO);

			packet.PutString(boss.ClassName, 48);
			packet.PutShort(0); // Could be part of the ClassName string
			packet.PutByte(0); // Could be part of the ClassName string
			packet.PutString(boss.Attribute.ToString(), 20); // Earth
			packet.PutString(boss.Race.ToString(), 20); // Klaida
			packet.PutString(boss.ArmorMaterial.ToString(), 20); // Iron
			packet.PutInt(4); // 4
			packet.PutInt(1); // 1
			packet.PutInt(11204); // 11204
			packet.PutInt(11); // 11
			packet.PutInt(7); // 7
			packet.PutInt(10); // 10
			packet.PutInt(45); // 45
			packet.PutInt(22); // 22

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Weekly Boss "Enable Class Ranking Season"
		/// </summary>
		/// <param name="character"></param>
		/// <param name="enabled"></param>
		public static void ZC_REQUEST_WEEKLY_BOSS_ENABLE_CLASS_RANKING_SEASON(Character character, bool enabled)
		{
			var packet = new Packet(Op.ZC_REQUEST_WEEKLY_BOSS_ENABLE_CLASS_RANKING_SEASON);

			packet.PutByte(enabled);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Plays animation for actor on nearby clients.
		/// </summary>
		/// <param name="actor">Entity to animate.</param>
		/// <param name="animationName">Name of the animation to play (uses packet string database to retrieve the id of the string).</param>
		/// <param name="stopOnLastFrame">If true, the animation plays once and then stops on the last frame.</param>
		public static void ZC_PLAY_ANI(IZoneConnection conn, IActor actor, string animationName,
			bool stopOnLastFrame = false, float readyTime = 0, float animationSpeed = 1, byte b1 = 0)
		{
			var packet = new Packet(Op.ZC_PLAY_ANI);

			packet.PutInt(actor.Handle);
			packet.AddStringId(animationName);
			packet.PutByte(stopOnLastFrame);
			packet.PutByte(0);
			packet.PutFloat(readyTime);
			packet.PutFloat(animationSpeed);

			// [i373230] Maybe added earlier
			if (Versions.Client > 373230)
			{
				packet.PutByte(b1);
				packet.PutByte(0);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Plays animation for actor on nearby clients.
		/// </summary>
		/// <param name="actor">Entity to animate.</param>
		/// <param name="animationName">Name of the animation to play (uses packet string database to retrieve the id of the string).</param>
		/// <param name="stopOnLastFrame">If true, the animation plays once and then stops on the last frame.</param>
		public static void ZC_PLAY_ANI(IActor actor, string animationName,
			bool stopOnLastFrame = false, float readyTime = 0, float animationSpeed = 1, byte b1 = 0)
		{
			var packet = new Packet(Op.ZC_PLAY_ANI);

			packet.PutInt(actor.Handle);
			packet.AddStringId(animationName);
			packet.PutByte(stopOnLastFrame);
			packet.PutByte(0);
			packet.PutFloat(readyTime);
			packet.PutFloat(animationSpeed);

			// [i373230] Maybe added earlier
			if (Versions.Client > 373230)
			{
				packet.PutByte(b1);
				packet.PutByte(0);
			}

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Sends a list of common skills.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_COMMON_SKILL_LIST(Character character)
		{
			var skills = character.Skills.GetList(skill => skill.IsCommon);

			var packet = new Packet(Op.ZC_COMMON_SKILL_LIST);

			packet.PutInt(skills?.Length ?? 0);
			packet.Zlib(true, zpacket =>
			{
				foreach (var skill in skills)
					zpacket.PutInt((int)skill.Id);
				zpacket.PutEmptyBin(16);
			});

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_PCBANG_POINT to character (dummy).
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_PCBANG_POINT(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_PCBANG_POINT);

			packet.PutInt(-1);
			packet.PutInt(980); //Increasing Value each time this packet is sent
			packet.PutInt(1620); //Max?

			conn.Send(packet);
		}

		/// <summary>
		/// Updates character's movement speed.
		/// </summary>
		/// <param name="entity"></param>
		public static void ZC_MSPD(ICombatEntity entity)
		{
			var packet = new Packet(Op.ZC_MSPD);

			packet.PutInt(entity.Handle);
			packet.PutFloat(entity.Properties.GetFloat(PropertyName.MSPD));
			if (Versions.Protocol > 500)
				packet.PutLong(0);

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Updates an entity's movement speed.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="objectId"></param>
		/// <param name="speed"></param>
		public static void ZC_MSPD(Character character, Actor actor, long objectId, float speed)
		{
			var packet = new Packet(Op.ZC_MSPD);

			packet.PutInt(actor.Handle);
			packet.PutFloat(speed);
			if (Versions.Protocol > 500)
				packet.PutLong(objectId);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates an entity's movement speed.
		/// </summary>
		/// <remarks>
		/// Should this broadcasted?
		/// </remarks>
		/// <param name="actor"></param>
		/// <param name="objectId"></param>
		/// <param name="speed"></param>
		public static void ZC_MSPD(Actor actor, long objectId, float speed)
		{
			var packet = new Packet(Op.ZC_MSPD);

			packet.PutInt(actor.Handle);
			packet.PutFloat(speed);
			if (Versions.Protocol > 500)
				packet.PutLong(objectId);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Sends ZC_CUSTOM_COMMANDER_INFO to character (dummy).
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="value"></param>
		public static void ZC_CUSTOM_COMMANDER_INFO(Character character, CommanderInfoType type, int value)
		{
			var packet = new Packet(Op.ZC_CUSTOM_COMMANDER_INFO);

			packet.PutLong(0);
			packet.PutInt(0);
			packet.PutShort((short)type);
			packet.PutInt(value);
			packet.PutInt(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends Lua script to client for execution.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="script"></param>
		public static void ZC_EXEC_CLIENT_SCP(IZoneConnection conn, string script)
		{
			if (Versions.Protocol < 500)
				return;
			var packet = new Packet(Op.ZC_EXEC_CLIENT_SCP);
			packet.PutString(script); // CHECK_APPLICATION_LIST("693014448046952")

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_SOLO_DUNGEON_RANKING to character (dummy).
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_SOLO_DUNGEON_RANKING(IZoneConnection conn, List<Character> characters)
		{
			var packet = new Packet(Op.ZC_SOLO_DUNGEON_RANKING);

			packet.PutLong(1);
			packet.PutInt(characters.Count); // Ranker Count
			foreach (var character in characters)
			{
				var jobs = character.Jobs.GetList().ToList();

				packet.PutLong(character.AccountDbId);
				packet.PutLpString(character.Name);
				packet.PutInt(character.Level);
				packet.PutLong(0); // Guild ID? Party ID? Team ID?
				packet.PutLpString(character.TeamName);
				packet.PutLong(character.ObjectId);
				packet.PutInt(character.Level);
				packet.PutInt(17);

				packet.PutInt(jobs.Count);
				foreach (var job in jobs)
					packet.PutInt((int)job.Id);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_RESET_VIEW to connection (dummy).
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_RESET_VIEW(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_RESET_VIEW);
			conn.Send(packet);
		}

		/// <summary>
		/// Not too sure what this does, maybe for store purchases?
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_SET_WEBSERVICE_URL(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_SET_WEBSERVICE_URL);

			packet.PutString("http://127.0.0.1:9004", 128);
			packet.PutString("http://127.0.0.1:9005", 128);

			conn.Send(packet);
		}

		/// <summary>
		/// Decrease silver
		/// </summary>
		/// <param name="character"></param>
		/// <param name="amount"></param>
		public static void ZC_DECREASE_SILVER(Character character, int amount)
		{
			var packet = new Packet(Op.ZC_DECREASE_SILVER);

			packet.PutInt(amount);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Reload Sell List, sent after market register/cancel.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_RELOAD_SELL_LIST(Character character)
		{
			var packet = new Packet(Op.ZC_RELOAD_SELL_LIST);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_RESPONSE_FIELD_BOSS_EXIST to character (dummy).
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_RESPONSE_FIELD_BOSS_EXIST(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_RESPONSE_FIELD_BOSS_EXIST);
			packet.PutInt(0); // 0 usually

			conn.Send(packet);
		}

		/// <summary>
		/// Rank System Time Table Response
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_RESPONSE_RANK_SYSTEM_TIME_TABLE(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_RESPONSE_RANK_SYSTEM_TIME_TABLE);

			var count = 6;
			packet.PutInt(count);

			for (var i = 0; i < count; i++)
			{
				var dt = DateTime.Now;
				// Start Time
				packet.PutShortDate(dt);
				// End Time
				packet.PutShortDate(dt.AddDays(1));
				// Reset Time Start ?
				packet.PutShortDate(dt.AddDays(1));
				// Reset Time End ?
				packet.PutShortDate(dt.AddDays(1));
				packet.PutInt(dt.ToInt32());
				packet.PutInt(0);
				packet.PutInt(40 + i); // Rank Week?
				packet.PutInt(0);
				packet.PutInt(1);
				packet.PutInt(2);
				packet.PutString("Week", 64);
				if (i == 0 || (i + 1) % 2 != 0)
					packet.PutString("TOSHero_Straight", 40);
				else
					packet.PutString("TOSHero_Wheel", 40);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_WEEKLY_BOSS_NOW_WEEK_NUM to character (dummy).
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_WEEKLY_BOSS_NOW_WEEK_NUM(IZoneConnection conn, int weekNumber)
		{
			var packet = new Packet(Op.ZC_WEEKLY_BOSS_NOW_WEEK_NUM);
			packet.PutInt(weekNumber);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_RESPONSE_BORUTA_NOW_WEEK_NUM to character (dummy).
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_RESPONSE_BORUTA_NOW_WEEK_NUM(IZoneConnection conn, int weekNumber)
		{
			var packet = new Packet(Op.ZC_RESPONSE_BORUTA_NOW_WEEK_NUM);
			packet.PutInt(weekNumber);

			conn.Send(packet);
		}

		/// <summary>
		/// Reset the client's Assister Cards Addon
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_ANCIENT_CARD_RESET(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_ANCIENT_CARD_RESET);

			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Unknown purpose, sent when changing map and tutorial related?
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="controlScript"></param>
		/// <param name="enabled"></param>
		public static void ZC_ENABLE_CONTROL(IZoneConnection conn, string controlScript, bool enabled)
		{
			var packet = new Packet(Op.ZC_ENABLE_CONTROL);

			packet.PutInt(0);
			packet.PutString(controlScript, 64);
			packet.PutByte(enabled); // 0 or 1

			conn.Send(packet);
		}

		/// <summary>
		/// Custom Camera Zoom
		/// Used to Zoom In or Out
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_CUSTOM_CAMERA_ZOOM(IZoneConnection conn, float distance, float time, float easing)
		{
			var packet = new Packet(Op.ZC_CUSTOM_CAMERA_ZOOM);

			packet.PutFloat(distance);
			packet.PutFloat(time);
			packet.PutFloat(easing);

			conn.Send(packet);
		}

		/// <summary>
		/// Used to broadcast "shockwave"
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="i1"></param>
		/// <param name="range"></param>
		/// <param name="shakePower"></param>
		/// <param name="duration"></param>
		/// <param name="shakeAmount"></param>
		/// <param name="shakeDirection"></param>
		/// <param name="delay"></param>
		public static void ZC_CHANGE_CAMERA_ZOOM(IActor actor, int i1, float range, float shakePower, float duration, float shakeAmount, float shakeDirection, float delay = 0.08460541f)
		{
			var packet = new Packet(Op.ZC_CHANGE_CAMERA_ZOOM);

			packet.PutInt(1);
			packet.PutInt(i1);
			packet.PutFloat(range);
			packet.PutFloat(shakePower);
			packet.PutFloat(duration);
			packet.PutFloat(shakeAmount);
			packet.PutFloat(shakeDirection);
			packet.PutFloat(delay);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Unknown purpose, sent when changing map and tutorial related?
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_LOCK_KEY(Character character, string controlScript, bool enabled)
		{
			var packet = new Packet(Op.ZC_LOCK_KEY);

			packet.PutString(controlScript, 64);
			packet.PutByte(enabled);
			packet.PutInt(character.Handle);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Creates a layer box that limit's player movement.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="actor"></param>
		/// <param name="leftPos"></param>
		/// <param name="rightPos"></param>
		/// <param name="width"></param>
		public static void ZC_CREATE_LAYERBOX(Character character, IActor actor, Position position, float dirX, float dirY, float width, float height, byte openEdges = 0)
		{
			var packet = new Packet(Op.ZC_CREATE_LAYERBOX);
			// posx, posz, dirx, diry, width, height, openIndex
			packet.PutInt(actor.Handle);
			packet.PutPosition2D(position);
			packet.PutFloat(dirX);
			packet.PutFloat(dirY);
			packet.PutFloat(width);
			packet.PutFloat(height);
			packet.PutFloat(openEdges);

			character.Connection.Send(packet);
		}


		/// <summary>
		/// Shifts in-game camera to create cinematic effect, isn't removed
		/// automatically.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="actor"></param>
		/// <param name="leftPos"></param>
		/// <param name="rightPos"></param>
		/// <param name="width"></param>
		public static void ZC_CREATE_SCROLLLOCKBOX(Character character, IActor actor, Position leftPos, Position rightPos, float width)
		{
			var packet = new Packet(Op.ZC_CREATE_SCROLLLOCKBOX);

			packet.PutInt(actor.Handle);
			packet.PutPosition2D(leftPos);
			packet.PutPosition2D(rightPos);
			packet.PutFloat(width);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Remove scroll lock box.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_REMOVE_SCROLLLOCKBOX(Character character)
		{
			var packet = new Packet(Op.ZC_REMOVE_SCROLLLOCKBOX);

			packet.PutInt(character.Handle);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Unknown Purpose.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="messageId"></param>
		public static void ZC_SHARED_MSG(IZoneConnection conn, int messageId)
		{
			var packet = new Packet(Op.ZC_SHARED_MSG);
			packet.PutInt(messageId);

			conn.Send(packet);
		}

		/// <summary>
		/// Debug Test? (0 shows position updates)
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_TEST_DBG(IActor actor)
		{
			var packet = new Packet(Op.ZC_TEST_DBG);

			packet.PutInt(actor.Handle);
			packet.PutInt(0);
			packet.PutPosition(actor.Position);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Resets overheat?
		/// </summary>
		/// <param name="character"></param>
		/// <param name="overheatId"></param>
		/// <param name="overheatValue"></param>
		public static void ZC_OVERHEAT_RESET_TIME(Character character, int overheatId, TimeSpan overheatValue)
		{
			var packet = new Packet(Op.ZC_OVERHEAT_RESET_TIME);

			packet.PutLong(character.ObjectId);
			packet.PutInt(overheatId); // 1551 or 2255
			packet.PutInt((int)overheatValue.TotalMilliseconds);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Exact purpose unknown, presumed to be related to keeping
		/// skills, hits, animations, and/or effects in sync somehow.
		/// </summary>
		/// <remarks>
		/// The key identifies the sync-procedure across the different
		/// sync packets.
		/// </remarks>
		/// <param name="caster"></param>
		/// <param name="key"></param>
		/// <param name="f1"></param>
		public static void ZC_SYNC_START(IActor caster, int key, float f1)
		{
			var packet = new Packet(Op.ZC_SYNC_START);

			packet.PutInt(key);
			packet.PutFloat(f1);

			if (caster is Character character)
				character.Connection.Send(packet);
		}

		/// <summary>
		/// Exact purpose unknown, presumed to be related to keeping
		/// skills, hits, animations, and/or effects in sync somehow.
		/// </summary>
		/// <remarks>
		/// The key identifies the sync-procedure across the different
		/// sync packets.
		/// </remarks>
		/// <param name="caster"></param>
		/// <param name="key"></param>
		/// <param name="f1"></param>
		public static void ZC_SYNC_END(IActor caster, int key, float f1)
		{
			var packet = new Packet(Op.ZC_SYNC_END);

			packet.PutInt(key);
			packet.PutFloat(f1);

			if (caster is Character character)
				character.Connection.Send(packet);
		}

		/// <summary>
		/// Unknown Purpose.
		/// Time set to Zero.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="skillActorId"></param>
		public static void ZC_SYNC_EXEC_BY_SKILL_TIME(IActor actor, int skillActorId)
		{
			ZC_SYNC_EXEC_BY_SKILL_TIME(actor, skillActorId, TimeSpan.Zero);
		}

		/// <summary>
		/// Exact purpose unknown, presumed to be related to keeping
		/// skills, hits, animations, and/or effects in sync somehow.
		/// </summary>
		/// <remarks>
		/// The key identifies the sync-procedure across the different
		/// sync packets.
		/// </remarks>
		/// <param name="actor"></param>
		/// <param name="key"></param>
		/// <param name="f1"></param>
		public static void ZC_SYNC_EXEC_BY_SKILL_TIME(IActor actor, int key, TimeSpan f1)
		{
			var packet = new Packet(Op.ZC_SYNC_EXEC_BY_SKILL_TIME);

			packet.PutInt(actor.Handle);
			packet.PutInt(key);
			packet.PutInt((int)f1.TotalMilliseconds);

			if (actor is Character character)
				character.Connection.Send(packet);
		}

		/// <summary>
		/// Exact purpose unknown, presumed to be related to keeping
		/// skills, hits, animations, and/or effects in sync somehow.
		/// </summary>
		/// <remarks>
		/// The key identifies the sync-procedure across the different
		/// sync packets.
		/// </remarks>
		/// <param name="actor"></param>
		/// <param name="key"></param>
		public static void ZC_SYNC_EXEC(IActor actor, int key)
		{
			var packet = new Packet(Op.ZC_SYNC_EXEC);
			packet.PutInt(key);

			if (actor is Character character)
				character.Connection.Send(packet);
		}

		/// <summary>
		/// Unknown Purpose.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_EQUIP_CARD_INFO(Character character)
		{
			var cards = character.Inventory.GetCards();

			var packet = new Packet(Op.ZC_EQUIP_CARD_INFO);
			packet.PutShort(cards.Count); // Count?

			foreach (var card in cards)
			{
				packet.PutLpString(card.Value.Data.ClassName);
				packet.PutLong((long)card.Value.Properties[PropertyName.ItemExp]);
				packet.PutInt(card.Key);
				packet.PutShort((short)card.Value.Properties[PropertyName.CardLevel]);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Unequips an item and optionally shows a UI message.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_EQUIP_GEM_INFO(Character character)
		{
			if (Versions.Client <= KnownVersions.ClosedBeta1)
				return;

			var items = character.Inventory.GetItems(item => item.Data.Type == ItemType.Equip).Values;

			var packet = new Packet(Op.ZC_EQUIP_GEM_INFO);
			packet.PutByte(0);
			packet.PutInt(items.Count); // Gem Count?
			packet.Zlib(true, zpacket =>
			{
				foreach (var item in items)
				{
					var index = (short)0;
					var gems = item.GetGemSockets();

					zpacket.PutLong(item.ObjectId);
					zpacket.PutShort(gems.Count); // Gem Data? Property Size?

					foreach (var gem in gems)
						zpacket.AddSocket(index++, gem);
				}
			});

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Unequips an item and optionally shows a UI message.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_EQUIP_GEM_INFO(Character character, Item item)
		{
			var packet = new Packet(Op.ZC_EQUIP_GEM_INFO);
			packet.PutByte(0);
			packet.PutInt(1); // Gem Count?
			packet.Zlib(true, zpacket =>
			{
				var index = (short)0;
				var gems = item.GetGemSockets();

				zpacket.PutLong(item.ObjectId);
				zpacket.PutShort(item.MaxSockets); // Gem Data? Property Size?

				foreach (var gem in gems)
					zpacket.AddSocket(index++, gem);
			});

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates the list of sold items in an NPC shop or the warehouse items, splitting items into pages of 10.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="items">List of items, with the key being the position/indices.</param>
		public static void ZC_SOLD_ITEM_DIVISION_LIST(Character character, InventoryType type, Dictionary<int, Item> items)
		{
			const int ItemsPerPage = 10;
			var totalItems = items.Count;
			var totalPages = (int)Math.Ceiling(totalItems / (double)ItemsPerPage);

			var orderedItems = items.OrderBy(a => a.Key).ToList();

			for (var pageIndex = 0; pageIndex < totalPages; pageIndex++)
			{
				var isFirstPage = pageIndex == 0;
				var isLastPage = pageIndex == totalPages - 1;

				var pageItems = orderedItems
					.Skip(pageIndex * ItemsPerPage)
					.Take(ItemsPerPage)
					.ToList();

				var packet = new Packet(Op.ZC_SOLD_ITEM_DIVISION_LIST);

				packet.PutByte((byte)type);
				packet.PutInt(pageItems.Count); // Total number of items, not just this page
				packet.PutByte(true); // Some constant value, preserved from the original implementation
				packet.PutByte(isFirstPage);
				packet.PutByte(isLastPage);

				if (pageItems.Count != 0)
				{
					packet.Zlib(true, zpacket =>
					{
						foreach (var itemKV in pageItems)
						{
							var position = itemKV.Key;
							var item = itemKV.Value;

							var isSilver = item.Id == ItemId.Silver;
							var propertyList = item.Properties.GetAll();

							// Ensure every item has at least one property.
							if (propertyList.Count == 0)
								propertyList.Add(new FloatProperty(PropertyName.CoolDown, 0));

							var propertiesSize = propertyList.GetByteCount();

							zpacket.PutInt(item.Id);
							zpacket.PutInt(propertiesSize);
							zpacket.PutLong(isSilver ? 0 : item.ObjectId);
							zpacket.PutInt(item.Amount);
							zpacket.PutInt(item.Price);
							zpacket.PutInt(1);
							zpacket.PutInt(position);
							zpacket.AddProperties(propertyList);

							if (!isSilver && item.ObjectId > 0)
							{
								zpacket.PutShort(0);
								zpacket.PutLong(item.ObjectId);
								if (item.HasSockets)
								{
									var socketIndex = (short)0;
									var gems = item.GetGemSockets();

									zpacket.PutShort(gems.Count);

									foreach (var gem in gems)
										zpacket.AddSocket(socketIndex++, gem);
								}
								else
									zpacket.PutShort(0);
							}
						}
					});
				}
				else
				{
					// Compressed empty packet logic preserved.
					packet.PutBinFromHex("8DFA 02000000 0300");
				}

				character.Connection.Send(packet);
			}
		}

		/// <summary>
		/// Display's a character's information in a side-panel.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		/// <param name="openWindow"></param>
		/// <param name="like"></param>
		public static void ZC_PROPERTY_COMPARE(IZoneConnection conn, Character character, bool openWindow, bool like)
		{
			var jobs = character.Jobs.GetList();
			var properties = character.Properties.GetAll();
			var propertiesSize = properties.GetByteCount();
			var etcProperties = character.Etc.Properties.GetAll();
			var etcPropertiesSize = etcProperties.GetByteCount();
			var equipItems = character.Inventory.GetEquip();

			var packet = new Packet(Op.ZC_PROPERTY_COMPARE);

			packet.PutInt(character.Handle);
			packet.PutString(character.Name, 65);
			packet.PutLong(character.ObjectId);
			packet.PutLong(character.AccountObjectId);
			packet.PutInt(0);
			packet.PutByte(openWindow);
			packet.PutByte(like);
			packet.PutInt(-1); // adventurerIndex
			packet.PutInt(0); // adventurerRank
			packet.PutInt(0); // achievementTitleCount
			packet.PutInt(0);// achievementCount

			// General character info? Same as in Friends list.
			{
				packet.PutString(character.TeamName, 64);
				packet.PutString(character.Name, 65);
				packet.PutByte(0);
				packet.PutShort((short)character.JobId);
				packet.PutInt((int)character.JobId);
				packet.PutInt(character.JobLevel);
				packet.PutShort((short)character.Gender);
				packet.PutShort(character.Hair);
				// Visual Equip Ids
				packet.PutEmptyBin(20);
				packet.PutInt(character.MapId);
				packet.PutInt(character.Level);
				packet.PutUInt(character.SkinColor);

				for (var i = 0; i < 4; ++i)
				{
					var jobId = jobs.ElementAtOrDefault(i)?.Id ?? 0;
					packet.PutShort((int)jobId);
				}

				packet.PutLong(0);
			}

			packet.PutShort(propertiesSize);
			packet.PutShort(etcPropertiesSize);

			packet.AddAppearancePc(character);

			packet.AddProperties(properties);
			packet.AddProperties(etcProperties);

			// [i3XXXXX]
			// It's currently unknown what this is or when exactly it was added.
			{
				packet.PutByte(1);
				packet.PutInt(0);
				packet.PutInt(35);
			}

			foreach (var equipItemKv in equipItems)
			{
				var equipSlot = equipItemKv.Key;
				var equipItem = equipItemKv.Value;

				var equipItemProperties = equipItem.Properties.GetAll();
				var equipItemPropertiesSize = equipItemProperties.GetByteCount();

				packet.PutInt(equipItem.Id);
				packet.PutShort(equipItemPropertiesSize);
				packet.PutShort(equipItem.Amount);
				packet.PutLong(equipItem.ObjectId);
				packet.PutInt((int)equipSlot);
				packet.PutInt(0);
				packet.PutShort(0);

				if (equipItemPropertiesSize > 0)
				{
					packet.AddProperties(equipItemProperties);

					packet.PutShort(0); // sealOptionSize
					packet.PutLong(equipItem.ObjectId);
					packet.PutShort(0); // gemCount
				}
			}

			packet.PutShort(jobs.Length);
			foreach (var job in jobs)
			{
				packet.PutShort((short)job.Id);
				packet.PutByte(0x00);
				packet.PutByte(0xB1);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
				packet.PutInt(0);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Updates monster appearance on clients in range of the monster.
		/// </summary>
		/// <param name="monster"></param>
		public static void ZC_UPDATED_MONSTERAPPEARANCE(IMonster monster)
		{
			var packet = new Packet(Op.ZC_UPDATED_MONSTERAPPEARANCE);
			packet.PutInt(monster.Handle);
			packet.AddMonsterAppearanceBase(monster);

			packet.PutInt(monster.GetByteCount());
			packet.AddMonsterAppearance(monster);

			monster.Map.Broadcast(packet, monster);
		}

		/// <summary>
		/// Display a buff on client UI
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buff"></param>
		public static void ZC_BUFF_ADD(ICombatEntity entity, Buff buff)
		{
			var packet = new Packet(Op.ZC_BUFF_ADD);
			packet.AddTargetedBuff(buff);

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Update a buff after a buff has been added
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buff"></param>
		public static void ZC_BUFF_UPDATE(ICombatEntity entity, Buff buff)
		{
			var packet = new Packet(Op.ZC_BUFF_UPDATE);
			packet.AddTargetedBuff(buff);

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Remove buff from client UI
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="buff"></param>
		public static void ZC_BUFF_REMOVE(ICombatEntity entity, Buff buff)
		{
			var packet = new Packet(Op.ZC_BUFF_REMOVE);

			packet.PutInt(entity.Handle);
			packet.PutInt((int)buff.Id);
			packet.PutByte(0);
			if (Versions.Client > KnownVersions.ClosedBeta1)
			{
				packet.PutInt(buff.Handle);
			}
			else
				packet.PutShort(0);
			if (Versions.Protocol > 500)
				packet.PutByte(0);

			entity.Map.Broadcast(packet, entity);

			if (entity is Character character)
				character.Connection?.Party?.BroadcastMemberBuffUpdate(character, packet);
		}

		/// <summary>
		/// Updates the entity's buffs on the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="entity"></param>
		public static void ZC_BUFF_LIST(IZoneConnection conn, IActor entity)
		{
			var buffs = entity.Components.Get<BuffComponent>();
			var buffCount = buffs?.Count ?? 0;

			var packet = new Packet(Op.ZC_BUFF_LIST);

			packet.PutInt(entity.Handle);
			packet.PutByte((byte)buffCount);
			if (buffCount > 0)
			{
				foreach (var buff in buffs.GetList())
					packet.AddBuff(buff);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Updates the entity's buffs on all clients in range.
		/// </summary>
		/// <param name="entity"></param>
		public static void ZC_BUFF_LIST(IActor entity)
		{
			var buffs = entity.Components.Get<BuffComponent>();
			var buffCount = buffs?.Count ?? 0;

			var packet = new Packet(Op.ZC_BUFF_LIST);

			packet.PutInt(entity.Handle);
			packet.PutByte((byte)buffCount);
			if (buffCount > 0)
			{
				foreach (var buff in buffs.GetList())
					packet.AddBuff(buff);
			}

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Buff list, sent when a entity spawns?
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="entity"></param>
		public static void ZC_BUFF_LIST(IZoneConnection conn, ICombatEntity entity)
		{
			var buffs = entity.Components.Get<BuffComponent>();
			var buffCount = buffs?.Count ?? 0;

			var packet = new Packet(Op.ZC_BUFF_LIST);

			packet.PutInt(entity.Handle);
			packet.PutByte((byte)buffCount);
			if (buffCount > 0)
			{
				foreach (var buff in buffs.GetList())
					packet.AddBuff(buff);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Clear buff buff handle association? Sent on entity death and
		/// removal.
		/// </summary>
		/// <param name="entity"></param>
		public static void ZC_BUFF_CLEAR(ICombatEntity entity)
		{
			var packet = new Packet(Op.ZC_BUFF_CLEAR);

			packet.PutInt(entity.Handle);
			packet.PutByte(1);

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Clear buff buff handle association? Sent on entity death and
		/// removal.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="entity"></param>
		public static void ZC_BUFF_CLEAR(IZoneConnection conn, ICombatEntity entity)
		{
			var packet = new Packet(Op.ZC_BUFF_CLEAR);

			packet.PutInt(entity.Handle);
			packet.PutByte(1);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_UI_OPEN to character
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="script"></param>
		/// <param name="isOn"></param>
		public static void ZC_UI_OPEN(IZoneConnection conn, string script, bool isOn)
		{
			var packet = new Packet(Op.ZC_UI_OPEN);
			packet.PutString(script, 32);
			packet.PutByte(isOn);

			conn.Send(packet);
		}

		/// <summary>
		/// ?
		/// </summary>
		/// <param name="character"></param>
		/// <param name="hookId"></param>
		public static void ZC_ENTER_HOOK(Character character, int hookId)
		{
			var packet = new Packet(Op.ZC_ENTER_HOOK);

			packet.PutInt(hookId);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Notifies a specific client that the actor is leaving a trigger event.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_LEAVE_HOOK(Character character, int hookId)
		{
			var packet = new Packet(Op.ZC_LEAVE_HOOK);

			packet.PutInt(hookId);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Notifies nearby clients that the actor is leaving a trigger event.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_LEAVE_HOOK(IActor actor)
		{
			var packet = new Packet(Op.ZC_LEAVE_HOOK);

			packet.PutInt(actor.Handle);

			// TODO: Does this actually need to be broadcasted?
			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Ground Effect for Skills
		/// </summary>
		/// <remarks>
		/// PlayGroundEffect(caster, eft, scl, x, y, z, lifeTime, key, radAngle, delayTime)
		/// </remarks>
		/// <param name="actor"></param>
		/// <param name="targetPosition"></param>
		/// <param name="effectName"></param>
		/// <param name="scale"></param>
		/// <param name="duration"></param>
		/// <param name="delay"></param>
		/// <param name="angle"></param>
		/// <param name="s1"></param>
		/// <param name="s2"></param>
		/// <param name="f1"></param>
		/// <param name="b1"></param>
		/// <param name="b2"></param>
		public static void ZC_GROUND_EFFECT(IActor actor, Position targetPosition, string effectName,
			float scale = 1, float duration = 0, float delay = 0, float angle = 0, short s1 = 0, short s2 = 0, float f1 = 0, byte b1 = 0, byte b2 = 0)
		{
			var packet = new Packet(Op.ZC_GROUND_EFFECT);

			packet.PutInt(actor.Handle);
			packet.AddStringId(effectName);
			packet.PutPosition(targetPosition);
			packet.PutFloat(scale); // 1
			packet.PutFloat(duration);
			packet.PutFloat(delay); // 0.2031306
			packet.PutFloat(angle);
			packet.PutShort(s1);
			packet.PutShort(s2);
			//packet.PutFloat(f5); // 0.1015625
			packet.PutFloat(f1);
			packet.PutByte(b1);
			// Noticed in i370431
			packet.PutByte(b2);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Related to monster flying?
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="flyHeight"></param>
		/// <param name="raiseSpeed"></param>
		public static void ZC_FLY(IActor actor, float flyHeight = 0, float raiseSpeed = 0)
		{
			var packet = new Packet(Op.ZC_FLY);

			packet.PutInt(actor.Handle);
			packet.PutFloat(flyHeight);
			packet.PutFloat(raiseSpeed);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Configures advanced behavior options for an actor's flying/vertical movement logic.
		/// </summary>
		/// <param name="actor">The actor to configure.</param>
		/// <param name="useLastValidFloorHeight">If true, uses the last known valid ground height if the actor moves over an invalid/void area.</param>
		/// <param name="useClientAnim">If true, the client handles the rising/falling animation states rather than the server.</param>
		/// <param name="allowConcurrentMovement">If true, allow other movement/vertical logic to process simultaneously with FlyMath.</param>
		/// <param name="deferUntilSpawned">If true, delays the execution of fly commands if they are received exactly when the actor is spawning.</param>
		public static void ZC_FLY_OPTION(
			IActor actor,
			bool useLastValidFloorHeight = false,
			bool useClientAnim = false,
			bool allowConcurrentMovement = false,
			bool deferUntilSpawned = false)
		{
			var packet = new Packet(Op.ZC_FLY_OPTION);

			packet.PutInt(actor.Handle);

			packet.PutByte(useLastValidFloorHeight);
			packet.PutByte(useClientAnim);
			packet.PutByte(allowConcurrentMovement);
			packet.PutByte(deferUntilSpawned);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Sets monsters to specific height from ground
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_FLY_HEIGHT(IActor actor, float maxHeight)
		{
			var packet = new Packet(Op.ZC_FLY_HEIGHT);

			packet.PutInt(actor.Handle);
			packet.PutFloat(maxHeight);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Sets monsters to specific height from ground
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_FLY_HEIGHT(IZoneConnection conn, IActor actor, float maxHeight)
		{
			var packet = new Packet(Op.ZC_FLY_HEIGHT);

			packet.PutInt(actor.Handle);
			packet.PutFloat(maxHeight);

			conn.Send(packet);
		}

		/// <summary>
		/// Controls an actor's vertical "Fly" movement math. 
		/// Used to make monsters or objects hover or rise to a specific height with easing.
		/// </summary>
		/// <param name="actor">The actor to apply the flying effect to.</param>
		/// <param name="maxHeight">The maximum altitude the actor will reach.</param>
		/// <param name="raiseTime">The time (duration) it takes to reach the max height.</param>
		/// <param name="easing">Easing value for smooth transition (0 for linear, higher for curves).</param>
		/// <param name="playAfterBorn">If true, the flying effect begins immediately upon spawning/initialization.</param>
		public static void ZC_FLY_MATH(
			IActor actor,
			float maxHeight,
			float raiseTime,
			float easing,
			bool playAfterBorn = true)
		{
			var packet = new Packet(Op.ZC_FLY_MATH);

			packet.PutInt(actor.Handle);
			packet.PutFloat(maxHeight);
			packet.PutFloat(raiseTime);
			packet.PutFloat(easing);

			packet.PutByte(playAfterBorn);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Updates buff's time?
		/// </summary>
		/// <remarks>
		/// Used in long duration buffs that might require
		/// a resync with the server?
		/// </remarks>
		/// <param name="entity"></param>
		/// <param name="buff"></param>
		public static void ZC_BUFF_UPDATE_TIME(ICombatEntity entity, Buff buff)
		{
			var packet = new Packet(Op.ZC_BUFF_UPDATE_TIME);

			packet.PutLong(0);
			packet.PutInt(0);
			packet.PutInt(entity.Handle);
			packet.PutInt((int)buff.Id);
			packet.PutInt(0); // Increasing Value?

			entity.Map.Broadcast(packet, entity);
		}

		/// <summary>
		/// Used to cancel actor to a prop (example: chair)
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="animation"></param>
		/// <param name="isEnabled"></param>
		public static void ZC_PLAY_PAIR_ANIMATION(IActor actor, string animation, bool isEnabled)
		=> ZC_PLAY_PAIR_ANIMATION(actor, animation, "", "", "", isEnabled);

		/// <summary>
		/// Used to pair actor to a prop (example: chair)
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="animationName"></param>
		/// <param name="parameter1"></param>
		/// <param name="parameter2"></param>
		/// <param name="isEnabled"></param>
		public static void ZC_PLAY_PAIR_ANIMATION(IActor actor, string animationName, string parameter1 = "None", string parameter2 = "None", bool isEnabled = true)
		=> ZC_PLAY_PAIR_ANIMATION(actor, "", animationName, parameter1, parameter2, isEnabled);

		/// <summary>
		/// Used to pair actor to a prop (example: chair)
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="animation"></param>
		/// <param name="animationName"></param>
		/// <param name="parameter1"></param>
		/// <param name="parameter2"></param>
		public static void ZC_PLAY_PAIR_ANIMATION(IActor actor, string animation, string animationName, string parameter1, string parameter2, bool isEnabled)
		{
			var packet = new Packet(Op.ZC_PLAY_PAIR_ANIMATION);

			packet.PutInt(actor.Handle);
			packet.PutString(animation, 64); // ""
			packet.PutString(animationName, 128); //BARRACK_SIT
			packet.PutString(parameter1, 64); // None
			packet.PutString(parameter2, 64); // None
			packet.PutInt(0);
			packet.PutShort(isEnabled ? 1 : 0);
			packet.PutByte(0);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Attach to Slot
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="slot"></param>
		/// <param name="xacName"></param>
		/// <param name="attachNode"></param>
		public static void ZC_ATTACH_TO_SLOT(IActor actor, int slot, string xacName = null, string attachNode = null)
		{
			var packet = new Packet(Op.ZC_ATTACH_TO_SLOT);

			packet.PutInt(actor.Handle);
			packet.PutInt(slot);
			packet.AddStringId(xacName);
			packet.AddStringId(attachNode);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Shows/Removes Guild Colony Tower map marker
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="isMarkerVisible"></param>
		public static void ZC_BROAD_CAST_MY_INFO(IMonster actor, bool isMarkerVisible = false)
		{
			var packet = new Packet(Op.ZC_BROAD_CAST_MY_INFO);

			packet.PutInt(actor.Id);
			packet.PutInt(actor.Handle);
			packet.PutPosition(actor.Position);
			packet.PutByte(isMarkerVisible);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Send adventure book info
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="infoList"></param>
		public static void ZC_ADVENTURE_BOOK_INFO(Character character, AdventureBookType type, SortedList<int, int> infoList)
		{
			var packet = new Packet(Op.ZC_ADVENTURE_BOOK_INFO);

			packet.PutInt(infoList.Count);
			packet.PutShort((short)type);
			packet.PutByte(1);
			packet.PutByte(1);
			packet.PutShort(1);

			packet.Zlib(true, zpacket =>
			{
				foreach (var info in infoList)
				{
					zpacket.PutInt(info.Key);
					zpacket.PutInt(info.Value);
				}
			});

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send adventure book info
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="itemList"></param>
		public static void ZC_ADVENTURE_BOOK_INFO(Character character, AdventureBookType type, SortedList<int, AdventureBookItemEntry> itemList)
		{
			var packet = new Packet(Op.ZC_ADVENTURE_BOOK_INFO);

			packet.PutInt(itemList.Count);
			packet.PutShort((short)type);
			packet.PutByte(1);
			packet.PutByte(1);
			packet.PutShort(1);

			packet.Zlib(true, zpacket =>
			{
				foreach (var info in itemList)
				{
					zpacket.PutInt(info.Key);
					zpacket.PutInt(info.Value.CraftedCount);
					zpacket.PutInt(info.Value.ObtainedCount);
					zpacket.PutInt(info.Value.UsedCount);
				}
			});

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send adventure book info
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="infoList"></param>
		public static void ZC_ADVENTURE_BOOK_INFO(Character character, AdventureBookType type, SortedList<int, SortedList<int, int>> infoList)
		{
			var packet = new Packet(Op.ZC_ADVENTURE_BOOK_INFO);

			packet.PutInt(infoList.Count);
			packet.PutShort((short)type);
			packet.PutByte(1);
			packet.PutByte(1);
			packet.PutShort(1);

			packet.Zlib(true, zpacket =>
			{
				foreach (var info in infoList)
				{
					zpacket.PutInt(info.Key);
					zpacket.PutInt(info.Value.Count);
					foreach (var subInfo in info.Value)
					{
						zpacket.PutInt(subInfo.Key);
						zpacket.PutInt(subInfo.Value);
					}
				}
			});

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Plays item pick up animation for the actor and item monster.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="itemMonster"></param>
		/// <param name="amount"></param>
		public static void ZC_ITEM_GET(IActor actor, IActor itemMonster, int amount)
		{
			var packet = new Packet(Op.ZC_ITEM_GET);

			packet.PutInt(actor.Handle);
			packet.PutInt(itemMonster.Handle);
			packet.PutInt(amount);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Makes actor move between the given positions.
		/// </summary>
		/// <remarks>
		/// The positions must use the cell for Y, instead of the height,
		/// otherwise the client is not able to handle the packet.
		/// </remarks>
		/// <param name="actor"></param>
		/// <param name="fromCellPos"></param>
		/// <param name="toCellPos"></param>
		/// <param name="speed"></param>
		/// <param name="autoRotate">
		/// If actor should automatically rotate towards destination
		/// </param>
		public static void ZC_MOVE_PATH(IActor actor, Position fromCellPos, Position toCellPos, float speed, bool autoRotate = true)
		{
			var packet = new Packet(Op.ZC_MOVE_PATH);
			packet.AddCellMovement(actor, fromCellPos, toCellPos, speed, autoRotate);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Broadcasts a movement action for the specified actor from one position to another.
		/// </summary>
		/// <remarks>This method creates a movement packet and broadcasts it to all entities on the actor's
		/// map.</remarks>
		/// <param name="actor">The actor performing the movement. Cannot be <see langword="null"/>.</param>
		/// <param name="fromPos">The starting position of the movement.</param>
		/// <param name="toPos">The destination position of the movement.</param>
		/// <param name="speed">The speed at which the actor moves.</param>
		/// <param name="time">The time taken to complete the movement, in seconds.</param>
		/// <param name="i1">An optional parameter for additional movement data. Defaults to 0.</param>
		public static void ZC_MOVE_POS(IActor actor, Position fromPos, Position toPos, float speed, float time, int i1 = 0)
		{
			var packet = new Packet(Op.ZC_MOVE_POS);
			packet.AddMovement(actor, fromPos, toPos, speed, time, i1);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// ZC_MOVE_BEZIER
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_MOVE_BEZIER(IActor actor, Position destination, Position q0, Position q1, float time)
		{
			var packet = new Packet(Op.ZC_MOVE_BEZIER);

			packet.PutInt(actor.Handle);
			packet.PutPosition(destination);
			packet.PutPosition(q0);
			packet.PutPosition(q1);
			packet.PutFloat(time);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Makes actor move between the given positions on the character's client.
		/// </summary>
		/// <remarks>
		/// The positions must use the cell for Y, instead of the height,
		/// otherwise the client is not able to handle the packet.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="actor"></param>
		/// <param name="fromCellPos"></param>
		/// <param name="toCellPos"></param>
		/// <param name="speed"></param>
		/// <param name="autoRotate">
		/// If actor should automatically rotate towards destination
		/// </param>
		public static void ZC_MOVE_PATH(Character character, IActor actor, Position fromCellPos, Position toCellPos, float speed, bool autoRotate = true)
		{
			var packet = new Packet(Op.ZC_MOVE_PATH);
			packet.AddCellMovement(actor, fromCellPos, toCellPos, speed, autoRotate);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Removes the skill for the character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skillId"></param>
		public static void ZC_SKILL_REMOVE(Character character, SkillId skillId)
		{
			var packet = new Packet(Op.ZC_SKILL_REMOVE);
			packet.PutInt(character.Handle);
			packet.PutInt((int)skillId);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Informs client that the item has been used, playing animations
		/// for some types.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="itemClassId"></param>
		public static void ZC_ITEM_USE(Character character, int itemClassId)
		{
			var packet = new Packet(Op.ZC_ITEM_USE);
			packet.PutInt(character.Handle);
			packet.PutInt(itemClassId);

			character.Connection.Send(packet);
		}

		/// Cancel "something" due to mouse movement (walking)
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_CANCEL_MOUSE_MOVE(Character character)
		{
			var packet = new Packet(Op.ZC_CANCEL_MOUSE_MOVE);

			packet.PutInt(character.Handle);

			character.Connection.Send(packet);
		}


		/// <summary>
		/// Set "ex"-property
		/// </summary>
		/// <param name="character"></param>
		/// <param name="msgParams"></param>
		public static void ZC_SEND_PC_EXPROP(Character character, params MsgParameter[] msgParams)
		{
			var packet = new Packet(Op.ZC_SEND_PC_EXPROP);

			packet.PutInt(msgParams?.Length ?? 0);
			if (msgParams != null)
			{
				foreach (var param in msgParams)
				{
					packet.PutLpString(param.Key);
					packet.PutFloat(float.Parse(param.Value));
				}
			}

			character.Connection.Send(packet);
		}

		// Removed: ZC_COLONY_OCCUPATION_INFO referenced GuildColonyInfo from deleted GuildColony namespace
		// public static void ZC_COLONY_OCCUPATION_INFO(List<GuildColonyInfo> colonyInfos) { ... }

		/// <summary>
		/// Displays area of affect for skill.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skillClassName"></param>
		/// <param name="duration"></param>
		/// <param name="area"></param>
		public static void ZC_START_RANGE_PREVIEW(ICombatEntity caster, string skillClassName, TimeSpan duration, ISplashArea area)
			=> ZC_START_RANGE_PREVIEW(caster, skillClassName, duration, area.SplashType, area.OriginPos, area.Direction, area.Height, area.Width);

		/// <summary>
		/// Displays area of affect for skill.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="casterHandle"></param>
		/// <param name="skillClassName"></param>
		/// <param name="duration"></param>
		/// <param name="area"></param>
		public static void ZC_START_RANGE_PREVIEW(Map map, int casterHandle, string skillClassName, TimeSpan duration, ISplashArea area)
			=> ZC_START_RANGE_PREVIEW(map, casterHandle, skillClassName, duration, area.SplashType, area.OriginPos, area.Direction, area.Height, area.Width);

		/// <summary>
		/// Displays area of affect for skill.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skillClassName"></param>
		/// <param name="duration"></param>
		/// <param name="splashType"></param>
		/// <param name="originPos"></param>
		/// <param name="direction"></param>
		/// <param name="height"></param>
		/// <param name="width"></param>
		public static void ZC_START_RANGE_PREVIEW(ICombatEntity caster, string skillClassName, TimeSpan duration, SplashType splashType, Position originPos, Direction direction, float height, float width)
			=> ZC_START_RANGE_PREVIEW(caster.Map, caster.Handle, skillClassName, duration, splashType, originPos, direction, height, width);

		/// <summary>
		/// Displays area of affect for skill.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="casterHandle"></param>
		/// <param name="skillClassName"></param>
		/// <param name="duration"></param>
		/// <param name="splashType"></param>
		/// <param name="originPos"></param>
		/// <param name="direction"></param>
		/// <param name="height"></param>
		/// <param name="width"></param>
		public static void ZC_START_RANGE_PREVIEW(Map map, int casterHandle, string skillClassName, TimeSpan duration, SplashType splashType, Position originPos, Direction direction, float height, float width)
		{
			var packet = new Packet(Op.ZC_START_RANGE_PREVIEW);

			packet.PutInt(casterHandle);
			packet.PutString(skillClassName, 64);
			packet.PutString(splashType.ToString(), 10);
			packet.PutLong(0);
			packet.PutLong(0);
			packet.PutFloat(0);
			packet.PutShort(0);
			packet.PutFloat((float)duration.TotalSeconds);
			packet.PutFloat(0);
			packet.PutPosition(originPos);
			packet.PutDirection(direction);
			packet.PutFloat(0); // animation start distance, if > length, animation starts past the splash area and goes back towards it
			packet.PutFloat(height);
			packet.PutFloat(width);
			packet.PutByte(0);

			map.Broadcast(packet);
		}

		/// <summary>
		/// Dummy Equip for Skill
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="weaponId"></param>
		/// <param name="subweaponId"></param>
		public static void ZC_EQUIP_DUMMY_FOR_SKILL(IActor actor, int subweaponId, int weaponId)
		{
			var packet = new Packet(Op.ZC_EQUIP_DUMMY_FOR_SKILL);
			packet.PutInt(actor.Handle);
			packet.PutInt(subweaponId);
			packet.PutInt(weaponId);
			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Show an aura on a weapon
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="equipSlot"></param>
		/// <param name="auraName"></param>
		/// <param name="i2"></param>
		/// <param name="b1"></param>
		public static void ZC_SET_AURA_INFO(IActor actor, EquipSlot equipSlot, string auraName, int i2 = 1, byte b1 = 0)
		{
			var packet = new Packet(Op.ZC_SET_AURA_INFO);

			packet.PutInt(actor.Handle);
			packet.PutInt((int)equipSlot);
			packet.PutString(auraName, 256);
			packet.PutInt(i2);
			packet.PutByte(b1);

			actor.Map.Broadcast(packet, actor);
		}

		/// <summary>
		/// Show an aura on a weapon
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="actor"></param>
		/// <param name="equipSlot"></param>
		/// <param name="auraName"></param>
		/// <param name="i2"></param>
		/// <param name="b1"></param>
		public static void ZC_SET_AURA_INFO(IZoneConnection conn, IActor actor, EquipSlot equipSlot, string auraName, int i2 = 1, byte b1 = 0)
		{
			var packet = new Packet(Op.ZC_SET_AURA_INFO);

			packet.PutInt(actor.Handle);
			packet.PutInt((int)equipSlot);
			packet.PutString(auraName, 256);
			packet.PutInt(i2);
			packet.PutByte(b1);

			conn.Send(packet);
		}

		/// <summary>
		/// Apply HUD Skin for "Myself"
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SEND_APPLY_HUD_SKIN_MYSELF(IZoneConnection conn, Character character)
		{
			var skinId = character.Variables.Perm.GetInt("Melia.HudSkin", 0);

			var packet = new Packet(Op.ZC_SEND_APPLY_HUD_SKIN_MYSELF);

			packet.PutInt(character.Handle);
			packet.PutInt(skinId);

			conn.Send(packet);
		}

		/// <summary>
		/// Apply HUD Skin for "Other" characters
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SEND_APPLY_HUD_SKIN_OTHER(IZoneConnection conn, Character character)
		{
			var skinId = character.Variables.Perm.GetInt("Melia.HudSkin", 0);

			var packet = new Packet(Op.ZC_SEND_APPLY_HUD_SKIN_OTHER);

			packet.PutInt(character.Handle);
			packet.PutInt(skinId);

			conn.Send(packet);
		}

		/// <summary>
		/// Set Aura Intensive Info
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="actor"></param>
		/// <param name="intensity"></param>
		public static void ZC_SET_AURA_INTENSIVE_INFO(IZoneConnection conn, IActor actor, float intensity)
		{
			var packet = new Packet(Op.ZC_SET_AURA_INTENSIVE_INFO);

			packet.PutInt(actor.Handle);
			packet.PutFloat(intensity);

			conn.Send(packet);
		}

		/// <summary>
		/// Apply HUD Skin for "Party" characters
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		/// <param name="party"></param>
		public static void ZC_SEND_APPLY_HUD_SKIN_PARTY(IZoneConnection conn, Character character, Party party)
		{
			var skinId = character.Variables.Perm.GetInt("Melia.HudSkin", 0);

			var packet = new Packet(Op.ZC_SEND_APPLY_HUD_SKIN_PARTY);

			packet.PutInt(character.Handle);
			packet.PutInt(skinId);
			packet.PutLong(party.ObjectId);

			conn.Send(packet);
		}

		/// <summary>
		/// Updates the character's HUD mode (?) on the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_SEND_MODE_HUD_SKIN(IZoneConnection conn, Character character, byte b1 = 0)
		{
			var packet = new Packet(Op.ZC_SEND_MODE_HUD_SKIN);

			packet.PutInt(character.Handle);
			packet.PutByte(b1);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send unable to target list.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="monsterIds"></param>
		public static void ZC_SEND_NONE_TARGETING_LIST(Character character, params int[] monsterIds)
		{
			var packet = new Packet(Op.ZC_SEND_NONE_TARGETING_LIST);

			packet.PutInt(character.Handle);
			packet.PutInt(monsterIds.Length);
			foreach (var monsterId in monsterIds)
			{
				packet.PutInt(monsterId);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// New Particle Effect "System"
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="packetStringId"></param>
		/// <param name="scale"></param>
		/// <param name="position"></param>
		/// <param name="direction"></param>
		/// <param name="b1"></param>
		public static void ZC_UNITY_GROUND_EFFECT(IActor actor, int packetStringId, float scale, Position position, Direction direction, byte b1)
		{
			var packet = new Packet(Op.ZC_UNITY_GROUND_EFFECT);

			packet.PutInt(actor.Handle);
			packet.PutInt(0);
			packet.PutInt(packetStringId);
			packet.PutFloat(scale);
			packet.PutPosition(position);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutDirection(direction);
			packet.PutByte(b1);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// New Particle Effect "System"
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="packetStringId"></param>
		/// <param name="scale"></param>
		/// <param name="position"></param>
		/// <param name="f1"></param>
		/// <param name="f2"></param>
		/// <param name="f3"></param>
		/// <param name="direction"></param>
		/// <param name="b1"></param>
		public static void ZC_UNITY_GROUND_EFFECT(IActor actor, int packetStringId, float scale, Position position,
			float f1,
			float f2,
			float f3,
			Direction direction, byte b1 = 0)
		{
			var packet = new Packet(Op.ZC_UNITY_GROUND_EFFECT);

			packet.PutInt(actor.Handle);
			packet.PutInt(0);
			packet.PutInt(packetStringId);
			packet.PutFloat(scale);
			packet.PutPosition(position);
			packet.PutFloat(f1);
			packet.PutFloat(f2);
			packet.PutFloat(f3);
			packet.PutDirection(direction);
			packet.PutByte(b1);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Updates certain skill properties, such as speed rate, hit
		/// delay, and max overheat count.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skills"></param>
		public static void ZC_UPDATE_SKL_SPDRATE_LIST(Character character, params Skill[] skills)
		{
			var packet = new Packet(Op.ZC_UPDATE_SKL_SPDRATE_LIST);

			packet.PutInt(skills.Length);

			foreach (var skill in skills)
			{
				packet.PutInt((int)skill.Id);
				packet.PutFloat(skill.Properties.GetFloat(PropertyName.SklSpdRate));
				packet.PutFloat(skill.Properties.GetFloat(PropertyName.HitDelay));
				packet.PutFloat(skill.Data.OverheatCount);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Send's character instance dungeon info.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_CHARACTER_INDUN_INFO_RESPONSE(Character character)
		{
			var packet = new Packet(Op.ZC_CHARACTER_INDUN_INFO_RESPONSE);

			// IndunInfo doesn't exist so sending 0 for now.
			//packet.PutInt(indunInfo.Count);
			packet.PutInt(0);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Not sure, but its send when instanced dungeon queueing.
		/// </summary>
		public static void ZC_HOLD_EXP_BOOK_TIME(IZoneConnection conn, Character character)
		{
			var packet = new Packet(Op.ZC_HOLD_EXP_BOOK_TIME);

			packet.PutEmptyBin(12);
			packet.PutInt(character?.Handle ?? 0);
			packet.PutShort((short)(character != null ? 256 : 0));
			packet.PutInt(1);
			packet.PutByte(0);
			packet.PutInt(-1);

			conn.Send(packet);
		}

		/// <summary>
		/// Enable after image on actor.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="f1"></param>
		/// <param name="f2"></param>
		/// <param name="f3"></param>
		/// <param name="f4"></param>
		/// <param name="f5"></param>
		/// <param name="f6"></param>
		public static void ZC_ON_AFTER_IMAGE(IActor actor, float f1, float f2, float f3, float f4, float f5, float f6)
		{
			var packet = new Packet(Op.ZC_ON_AFTER_IMAGE);

			packet.PutInt(actor.Handle);
			packet.PutFloat(f1);
			packet.PutFloat(f2);
			packet.PutFloat(f3);
			packet.PutFloat(f4);
			packet.PutFloat(f5);
			packet.PutFloat(f6);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Disable after image on an actor.
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_OFF_AFTER_IMAGE(IActor actor)
		{
			var packet = new Packet(Op.ZC_OFF_AFTER_IMAGE);

			packet.PutInt(actor.Handle);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Toggles character's fluting stance.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="enabled"></param>
		public static void ZC_READY_FLUTING(Character character, bool enabled)
		{
			var packet = new Packet(Op.ZC_READY_FLUTING);

			packet.PutInt(character.Handle);
			packet.PutByte(enabled);

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Makes character play a note on their flute.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="note"></param>
		/// <param name="octave"></param>
		/// <param name="semitone"></param>
		/// <param name="animate"></param>
		public static void ZC_PLAY_FLUTING(Character character, int note, int octave, bool semitone, bool animate)
		{
			var packet = new Packet(Op.ZC_PLAY_FLUTING);

			packet.PutInt(character.Handle);
			packet.PutInt(note);
			packet.PutInt(octave);
			packet.PutByte(semitone);
			packet.PutByte(animate);

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Stops character playing the current note on their flute.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="note"></param>
		/// <param name="octave"></param>
		/// <param name="semitone"></param>
		public static void ZC_STOP_FLUTING(Character character, int note, int octave, bool semitone)
		{
			var packet = new Packet(Op.ZC_STOP_FLUTING);

			packet.PutInt(character.Handle);
			packet.PutInt(note);
			packet.PutInt(octave);
			packet.PutByte(semitone);

			character.Map.Broadcast(packet, character);
		}

		/// <summary>
		/// Set scrolling limits on zoom from mouse wheel
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="minDist"></param>
		/// <param name="maxDist"></param>
		/// <param name="zoomUnit"></param>
		public static void ZC_CUSTOM_WHEEL_ZOOM(Character character, byte type, float minDist, float maxDist, float zoomUnit)
		{
			var packet = new Packet(Op.ZC_CUSTOM_WHEEL_ZOOM);

			packet.PutByte(type);
			packet.PutFloat(minDist);
			packet.PutFloat(maxDist);
			packet.PutFloat(zoomUnit);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Notice sent to seller on item sold in market
		/// </summary>
		/// <param name="character"></param>
		/// <param name="itemId"></param>
		/// <param name="itemAmount"></param>
		/// <param name="i2"></param>
		public static void ZC_SOLD_ITEM_NOTICE(Character character, int itemId, int itemAmount, int i2 = 1)
		{
			var packet = new Packet(Op.ZC_SOLD_ITEM_NOTICE);

			packet.PutString(character.Name, 128);
			packet.PutLong(character.AccountObjectId);
			packet.PutInt(itemAmount);
			packet.PutInt(itemId);
			packet.PutInt(i2);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Broadcasts text to all players in the world.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="text"></param>
		public static void ZC_TEXT(NoticeTextType type, string text)
		{
			var packet = new Packet(Op.ZC_TEXT);

			packet.PutByte((byte)type);
			packet.PutByte(0);
			packet.PutString(text);

			ZoneServer.Instance.World.Broadcast(packet);
		}

		/// <summary>
		/// Sends text to a specific player.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="text"></param>
		public static void ZC_TEXT(Character character, NoticeTextType type, string text)
		{
			var packet = new Packet(Op.ZC_TEXT);

			packet.PutByte((byte)type);
			packet.PutByte(0);
			packet.PutString(text);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// CZ_PING response (Pong)
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_PING(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_PING);

			packet.PutEmptyBin(12);

			conn.Send(packet);
		}

		/// <summary>
		/// Broadcasts knockback info
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="kbInfo"></param>
		public static void ZC_KNOCKBACK_INFO(IActor actor, KnockBackInfo kbInfo)
		{
			if (kbInfo == null)
				return;

			var packet = new Packet(Op.ZC_KNOCKBACK_INFO);

			packet.PutInt(actor.Handle);
			packet.AddKnockbackInfo(kbInfo);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Broadcasts knockdown info
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="kbInfo"></param>
		public static void ZC_KNOCKDOWN_INFO(IActor actor, KnockBackInfo kbInfo)
		{
			if (kbInfo == null)
				return;

			var packet = new Packet(Op.ZC_KNOCKDOWN_INFO);

			packet.PutInt(actor.Handle);
			packet.AddKnockdownInfo(kbInfo);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Delayed Rotate Move Start (?)
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_DELAYED_ROTATE_MOVE_START(IActor actor, float f1, float f2, float f3)
		{
			var packet = new Packet(Op.ZC_DELAYED_ROTATE_MOVE_START);

			packet.PutInt(actor.Handle);
			packet.PutFloat(f1);
			packet.PutFloat(f2);
			packet.PutFloat(f3);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Delayed Rotate Move End (?)
		/// </summary>
		/// <param name="actor"></param>
		public static void ZC_DELAYED_ROTATE_MOVE_END(IActor actor)
		{
			var packet = new Packet(Op.ZC_DELAYED_ROTATE_MOVE_END);

			packet.PutInt(actor.Handle);

			actor.Map.Broadcast(packet);
		}

		/// <summary>
		/// Fixes the camera at the given position on the character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="pos">Position to fix the camera at.</param>
		/// <param name="zoomLevel">Defines the zoom level for the fixed camera. Use 0 for no change.</param>
		public static void ZC_FIXCAMERA(Character character, Position pos, float zoomLevel)
		{
			var packet = new Packet(Op.ZC_FIXCAMERA);
			packet.PutPosition(pos);
			packet.PutFloat(zoomLevel);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Unfixes the camera on the character's client and makes it
		/// follow them again.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_CANCEL_FIXCAMERA(Character character)
		{
			var packet = new Packet(Op.ZC_CANCEL_FIXCAMERA);
			character.Connection.Send(packet);
		}

		/// <summary>
		/// Lock or unlock an item in the character's inventory.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		public static void ZC_ITEM_LOCK_STATE(Character character, Item item)
		{
			var packet = new Packet(Op.ZC_ITEM_LOCK_STATE);

			packet.PutLong(item.ObjectId);
			packet.PutByte(item.IsLocked);

			character.Connection.Send(packet);
		}

		public static void ZC_ALTER_HIT_RADIUS(IActor caster, IActor target, float f1, float f2 = 0)
		{
			var packet = new Packet(Op.ZC_ALTER_HIT_RADIUS);

			packet.PutInt(target.Handle);
			packet.PutFloat(f1);
			packet.PutFloat(f2);
			packet.PutInt(caster.Handle);

			caster.Map.Broadcast(packet);
		}

		/// <summary>
		/// Shakes the actor up and down.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="isOn"></param>
		/// <param name="maxHeight"></param>
		/// <param name="speed"></param>
		public static void ZC_VERTICAL_MOTION(IActor actor, bool isOn, float maxHeight = 0, float speed = 0)
		{
			var packet = new Packet(Op.ZC_VERTICAL_MOTION);

			packet.PutInt(actor.Handle);
			packet.PutByte(isOn);
			packet.PutFloat(maxHeight);
			packet.PutFloat(speed);

			actor.Map.Broadcast(packet);
		}
	}
}
