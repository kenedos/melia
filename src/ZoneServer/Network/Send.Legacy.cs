using System;
using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Shared.Network.Helpers;
using Melia.Zone.Network.Helpers;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Network
{
	public static partial class Send
	{
		/// <summary>
		/// Sends ZC_UI_INFO_LIST to character.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_UI_INFO_LIST(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_UI_INFO_LIST);

			packet.PutInt(0); // ?
			packet.PutInt(0); // ?

			conn.Send(packet);
		}

		/// <summary>
		/// Parties nearby list?
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_NEAR_PARTY_LIST(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_NEAR_PARTY_LIST);

			//packet.PutLong(0); // Party Id?
			//packet.PutShort(0);
			packet.PutBinFromHex("10BDAE0100C2F63300031C65A6000000000095190000657600005061727479233333313200003300330031003200000000000000000000000000000000000000000000000000000000000000000000000000C6076B00010010014F6C646661677300000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000002000050010003230313531313031353038313533320004000000803F3000C053064A01C6076B00010010014F6C6466616773000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000187BAE01BDE35500E90300000DCB00004F6C64666167730000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000004E61696B6F0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007BA30F080000000129AE012A9509003D000000703F6FC3B1EC1443B8E89CC234020000D1040000900300004D0800001200391C007C8D46381C00108A453A1C1872BE491C65A600000000007100000082A100004D6973736F6E7331303000007300310030003000000000000000000000000000000000000000000000000000000000000000000000000000FE690705010010015563686F61000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000002C00050010003230313531323230383135313033370004000000803F3000B85F084A14000000803F39000000000001FE690705010010015563686F610000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000187BAE01BDE35500E90300003BC700005563686F6100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000004C6F7264000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007BC30B0F0000000115AE015F950900C80000009120A6C1D4EC1443C9A280C2C70500009C1E0000C70500009C1E000000001C65A60000000000DA0A0000AF8900004D6172696D6265727300620065007200730000000000000000000000000000000000000000000000000000000000000000000000000000004F60C811010010014443616D7062656C6C0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000050000002B000500100032303135313134323631323038323600030001000004000000803F3000EC38084A3600000000400585071503010010014179756B69000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000008326820701001001416572646F720000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000038E99F00FFFFFFFF000000009E0D0300416572646F720000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000054616579000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000DA50F080000000227570045950900C60000003E57F8C36A1F0243E12E8B44F607000006150000F60700006F1600000000E7AAD008010010014D6F6E746D6F72656E6379000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000007FC571090100100157616C6C6B657200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000010000000000000000000000000000000000000000000000000000000000000000004F60C811010010014443616D7062656C6C00000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000187BAE01BDE35500E903000049CD00004443616D7062656C6C000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000004C6F68617A0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000007BA10F0F0000000125AE0100000000130000001E9B34C346EE1443C7CCA643EB00000023060000350100003F0600001200391C00340346381C00008B443A1C1872BE49");

			conn.Send(packet);
		}

		public static void ZC_WIKI_LIST(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_WIKI_LIST);

			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_CONNECT_OK to connection, verifying the connection and
		/// giving information about the character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_CONNECT_OK_L(IZoneConnection conn, Character character)
		{
			var packet = new Packet(Op.ZC_CONNECT_OK);

			packet.PutByte(0); // gameMode 0 = NormalMode, 1 = x
			packet.PutInt(1292150020);
			packet.PutByte(3); // isGM (< 3)?
			packet.PutEmptyBin(10);

			packet.PutLpString(conn.SessionKey);

			// [i109XX (2015-12-01)]
			// [i11025 (2016-02-26)] Removed?
			{
				//packet.PutShort(0xFB31); // ?
			}

			packet.PutInt(character.Handle);
			packet.PutInt(0);

			packet.AddAppearancePc(character);

			packet.PutFloat(character.Position.X);
			packet.PutFloat(character.Position.Y);
			packet.PutFloat(character.Position.Z);
			packet.PutInt((int)character.Exp);
			packet.PutInt((int)character.MaxExp);
			packet.PutInt(0);

			packet.PutLong(character.ObjectId);
			packet.PutLong(character.ObjectId + 1); // PCEtc GUID? socialInfoId

			packet.PutInt(character.Hp);
			packet.PutInt(character.MaxHp);
			packet.PutShort(character.Sp);
			packet.PutShort(character.MaxSp);
			packet.PutInt(character.Stamina);
			packet.PutInt(character.MaxStamina);
			packet.PutShort(0); // Shield
			packet.PutShort(0); // MaxShield

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_START_GAME to connection, which assumingly is the signal
		/// for the client to switch from load to map screen.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_START_GAME_L(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_START_GAME);

			packet.PutFloat(1); // Affects the speed of everything happening in the client o.o
			packet.PutFloat(1); // serverAppTimeOffset
			packet.PutFloat(1); // globalAppTimeOffset
			packet.PutLong(DateTime.Now.Add(TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now)).ToFileTime());

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_START_INFO to connection.
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_START_INFO_L(IZoneConnection conn)
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
		public static void ZC_MYPC_ENTER_L(Character character)
		{
			var packet = new Packet(Op.ZC_MYPC_ENTER);

			packet.PutFloat(character.Position.X);
			packet.PutFloat(character.Position.Y);
			packet.PutFloat(character.Position.Z);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Makes character appear on connection's client, by sending ZC_ENTER_PC.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_ENTER_PC_L(IZoneConnection conn, Character character)
		{
			var packet = new Packet(Op.ZC_ENTER_PC);

			packet.PutInt(character.Handle);
			packet.PutFloat(character.Position.X);
			packet.PutFloat(character.Position.Y);
			packet.PutFloat(character.Position.Z);
			packet.PutFloat(1);
			packet.PutInt(0);
			packet.PutShort(0);
			packet.PutLong(character.SocialUserId); // PCEtc GUID? socialInfoId
			packet.PutByte(0); // Pose
			packet.PutFloat(character.Properties.GetFloat(PropertyName.MSPD));
			packet.PutInt(0);
			packet.PutInt(character.Hp);
			packet.PutInt(character.MaxHp);
			packet.PutShort(character.Sp);
			packet.PutShort(character.MaxSp);
			packet.PutInt(0); // [i11025 (2016-02-26)]
			packet.PutInt(character.Stamina);
			packet.PutInt(character.MaxStamina);
			packet.PutByte(0);
			packet.PutShort(0);
			packet.PutInt(-1); // titleAchievmentId
			packet.PutInt(0);
			packet.PutByte(0);
			packet.AddCommander(character);

			// [i11025 (2016-02-26)] Removed?
			//packet.PutString("None", 49); // Party name

			// [i10622 (2015-10-22)] ?
			// [i11025 (2016-02-26)] Removed?
			{
				//packet.PutShort(0);
				//packet.PutInt(0);
				//packet.PutInt(0);
				//packet.PutInt(0);
				//packet.PutInt(0);
				//packet.PutInt(0);
				//packet.PutInt(0);
				//packet.PutInt(0);
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Broadcasts ZC_ENTER_MONSTER on monster's map, making it appear.
		/// </summary>
		/// <param name="monster"></param>
		public static void ZC_ENTER_MONSTER_L(IMonster monster)
		{
			var packet = new Packet(Op.ZC_ENTER_MONSTER);
			packet.AddMonster(monster);

			monster.Map.Broadcast(packet);
		}

		/// <summary>
		/// Sends ZC_ENTER_MONSTER to connection, making it appear.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="monster"></param>
		public static void ZC_ENTER_MONSTER_L(IZoneConnection conn, IMonster monster)
		{
			var packet = new Packet(Op.ZC_ENTER_MONSTER);
			packet.AddMonster(monster);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_QUICK_SLOT_LIST to connection, containing the
		/// list of hotkeys?
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_QUICK_SLOT_LIST_L(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_QUICK_SLOT_LIST);

			packet.PutInt(0);
			packet.PutShort(0);
			//...

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_SKILL_LIST to character, containing a list
		/// of all the character's skills.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SKILL_LIST_L(Character character)
		{
			var packet = new Packet(Op.ZC_SKILL_LIST);
			var skills = new[] { 1, 101, 105, 108, 20, 3, 100, 10002, 10003 };

			packet.PutInt(character.Handle);
			packet.PutShort(skills.Length); // count

			packet.PutShort(0); // No compressionZC
								//packet.BeginZlib();
			foreach (var skill in skills)
				packet.AddSkill(new Skill(character, (SkillId)skill, 1));
			//packet.EndZlib();

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Adds skill for character, by sending ZC_SKILL_ADD to its connection.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skillId"></param>
		public static void ZC_SKILL_ADD_L(Character character, Skill skill)
		{
			var packet = new Packet(Op.ZC_SKILL_ADD);

			packet.PutByte(0); // REGISTER_QUICK_SKILL ?
			packet.PutByte(0); // SKILL_LIST_GET ?
			packet.PutLong(0); // ?
			packet.AddSkill(skill);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_SKILLMAP_LIST to character.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_SKILLMAP_LIST_L(Character character)
		{
			var packet = new Packet(Op.ZC_SKILLMAP_LIST);

			packet.PutInt(0); // ?

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_OPTION_LIST to connection, containing the saved
		/// account options, like "Show Exp Aquired".
		/// </summary>
		/// <param name="conn"></param>
		public static void ZC_OPTION_LIST_L(IZoneConnection conn)
		{
			var packet = new Packet(Op.ZC_OPTION_LIST);

			packet.PutString(conn.Account.Settings.ToString());

			conn.Send(packet);
		}

		/// <summary>
		/// Sends ZC_ACHIEVE_POINT_LIST to character.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_ACHIEVE_POINT_LIST_L(Character character)
		{
			var packet = new Packet(Op.ZC_ACHIEVE_POINT_LIST);

			packet.PutInt(0); // ?

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_CHAT_MACRO_LIST to character.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_CHAT_MACRO_LIST_L(Character character)
		{
			var packet = new Packet(Op.ZC_CHAT_MACRO_LIST);

			packet.PutInt(0); // ?

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_UI_INFO_LIST to character.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_UI_INFO_LIST(Character character)
		{
			var packet = new Packet(Op.ZC_UI_INFO_LIST);

			packet.PutInt(0); // ?
			packet.PutInt(0); // ?

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_COOLDOWN_LIST to character, containing list of all
		/// cooldowns?
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_COOLDOWN_LIST(Character character)
		{
			var packet = new Packet(Op.ZC_COOLDOWN_LIST);

			packet.PutLong(0); // socialInfoId ?
			packet.PutInt(0); // ?

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_JOB_PTS to character, updating their job points.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_JOB_PTS(Character character)
		{
			var packet = new Packet(Op.ZC_JOB_PTS);

			packet.PutShort((short)character.Job.Id);
			packet.PutShort(1);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_ABILITY_LIST to character, containing a list of all
		/// their abilities.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_ABILITY_LIST_L(Character character)
		{
			var packet = new Packet(Op.ZC_ABILITY_LIST);

			var abilities = new[] { 10001, 10003, 10009, 10012, 10013, 10014, 101001 };

			packet.PutInt(character.Handle);
			packet.PutShort(abilities.Length); // count

			packet.PutShort(0); // No compression (client handler tests this short for compression marker, comment this line if using compression)
								//packet.BeginZlib();
			foreach (var ability in abilities)
			{
				packet.PutLong(0); // Some kind of GUID? o.O
				packet.PutInt(ability);
				packet.PutShort(6); // properties size (some abilities doesn't have properties, like weapon wielding)
				packet.PutShort(255); // ?
				packet.PutShort(25); //Level
				packet.PutFloat(10);
			}
			//packet.EndZlib();

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_ITEM_INVENTORY_LIST to character, containing a list of
		/// all items in their inventory.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_ITEM_INVENTORY_LIST_L(Character character)
		{
			var items = character.Inventory.GetItems();

			var packet = new Packet(Op.ZC_ITEM_INVENTORY_LIST);

			packet.PutInt(items.Count);
			packet.PutShort(0); // Compression
			foreach (var item in items)
			{
				packet.PutInt(item.Value.Id);
				packet.PutShort(0); // Size of the object at the end
				packet.PutEmptyBin(2);
				packet.PutLong(item.Value.ObjectId);
				packet.PutInt(item.Value.Amount);
				packet.PutInt(item.Value.Price);
				packet.PutInt(item.Key);
				packet.PutInt(1); // ?
								  //packet.PutEmptyBin(0);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends ZC_ITEM_INVENTORY_INDEX_LIST to character, containing a list
		/// of indices for all items in the inventory. This updates their order.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_ITEM_INVENTORY_INDEX_LIST_L(Character character)
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
		public static void ZC_ITEM_INVENTORY_INDEX_LIST_L(Character character, InventoryCategory category)
		{
			ZC_ITEM_INVENTORY_INDEX_LIST(character, character.Inventory.GetIndices(category));
		}

		public static void ZC_WIKI_COUNT_UPDATE(Character character, int wikiId, WikiType type, int i1, int i2)
		{
			var packet = new Packet(Op.ZC_WIKI_COUNT_UPDATE);

			packet.PutInt(wikiId);
			packet.PutByte((byte)type);
			packet.PutInt(i1);
			packet.PutInt(i2);

			character.Connection.Send(packet);
		}

		public static void ZC_WIKI_INT_PROP_UPDATE(Character character, int wikiId, WikiType type, int i1)
		{
			var packet = new Packet(Op.ZC_WIKI_INT_PROP_UPDATE);

			packet.PutInt(wikiId);
			packet.PutByte((byte)type);
			packet.PutInt(i1);

			character.Connection.Send(packet);
		}
	}
}
