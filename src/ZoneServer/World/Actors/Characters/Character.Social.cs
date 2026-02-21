// ===================================================================
// CharacterSocial.cs - Communication, party, and social features
// ===================================================================
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Characters
{
	public partial class Character
	{
		/// <summary>
		/// Displays server message in character's chat.
		/// </summary>
		public void ServerMessage(string format, params object[] args)
		{
			if (args.Length > 0)
				format = string.Format(format, args);

			if (ZoneServer.Instance.Data.SystemMessageDb.Contains("{Hour}:{Min}"))
				this.SystemMessage("{Hour}:{Min}", new MsgParameter("Hour", "Server "), new MsgParameter("Min", " " + format));
			else
				this.SystemMessage("{Year}.{Month}.{Day}. {Hour}:{Min}",
					new MsgParameter("Year", ""), new MsgParameter("Month", ""), new MsgParameter("Day", ""),
					new MsgParameter("Hour", "Server "), new MsgParameter("Min", " " + format));
		}

		/// <summary>
		/// Displays system message in character's chat.
		/// </summary>
		public void SystemMessage(string className, params MsgParameter[] args)
		{
			SystemMessage(className, true, "", args);
		}

		/// <summary>
		/// Displays system message in character's chat.
		/// </summary>
		public void SystemMessage(string className, bool chatFrameOnly, string chatTextColor = "", params MsgParameter[] args)
		{
			if (!ZoneServer.Instance.Data.SystemMessageDb.TryFind(className, out var sysMsgData))
				throw new System.ArgumentException($"System message '{className}' not found.");

			Send.ZC_SYSTEM_MSG(this, sysMsgData.ClassId, chatFrameOnly, chatTextColor, args);
		}

		/// <summary>
		/// Sends server message to character as a popup box.
		/// </summary>
		public void MsgBox(string format, params object[] args)
		{
			if (args.Length > 0)
				format = string.Format(format, args);

			if (format.Contains('\''))
				format = format.Replace("'", "\\'");

			Send.ZC_EXEC_CLIENT_SCP(this.Connection, "ui.MsgBox('" + format + "')");
		}


		/// <summary>
		/// Sends an addon message to the client.
		/// </summary>
		public void AddonMessage(string function, string stringParameter = null, int intParameter = 0)
		{
			Send.ZC_ADDON_MSG(this, function, intParameter, stringParameter);
		}

		/// <summary>
		/// Sends a message to all players on the server.
		/// </summary>
		public void WorldMessage(string message)
		{
			Send.ZC_NORMAL.WorldMessage(1, message);
		}

		/// <summary>
		/// Shows the "Click here to join main char" on character login.
		/// </summary>
		public async Task ShowMainChatOnLogin()
		{
			await Task.Delay(5000);

			var oldKey = this.Variables.Perm.GetString("Melia.Main.Welcome");

			if (string.IsNullOrEmpty(oldKey) || oldKey != this.Connection.SessionKey)
			{
				this.ServerMessage("Click here to join the main chat: {a SLC 0@@@557516819791873}{#0000FF}{img link_whisper 24 24}Main{/}{/}{/}");
			}

			this.Variables.Perm.SetString("Melia.Main.Welcome", this.Connection.SessionKey);
		}

		/// <summary>
		/// Gets all party members within a specified range.
		/// </summary>
		public IEnumerable<Character> GetPartyMembersInRange(float range = 0, bool areAlive = true)
		{
			return this.Map.GetPartyMembersInRange(this, this.Position, range, areAlive);
		}
	}
}
