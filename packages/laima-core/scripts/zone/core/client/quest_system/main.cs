//--- Melia Script ----------------------------------------------------------
// Quest System
//--- Description -----------------------------------------------------------
// Adds client-side support for our custom quest system.
//---------------------------------------------------------------------------

using System.Globalization;
using System.Linq;
using System.Text;
using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Yggdrasil.Logging;
using Yggdrasil.Util.Commands;
using static Melia.Zone.Scripting.Shortcuts;

public class CustomQuestSystemClientScript : ClientScript
{
	protected override void Load()
	{
		this.LoadAllScripts();

		AddChatCommand("quest", "<complete|cancel|track|warp>", "", 0, 99, HandleQuest);
		AddChatCommand("questsearch", "<text>", "", 0, 99, HandleQuestSearch);
		AddChatCommand("questmarkdebug", "", "", 99, 99, HandleQuestMarkDebug);
	}

	private CommandResult HandleQuestMarkDebug(Character sender, Character target, string message, string commandName, Arguments args)
	{
		if (args.Count >= 2 && args.Get(0) == "zoom")
		{
			Send.ZC_EXEC_CLIENT_SCP(sender.Connection, "camera.CustomZoom(" + args.Get(1) + ", 0.3, 0) ui.SysMsg('zoom " + args.Get(1) + "')");
			return CommandResult.Okay;
		}

		var map = sender.Map;
		var npcs = map.GetNpcs(a => a.UniqueName != null && a.Id != MonsterId.HiddenTrigger)
			.OrderBy(a => a.Position.Get2DDistance(sender.Position))
			.Take(6)
			.ToList();

		var entries = string.Join(",", npcs.Select(a => "{\"" + map.ClassName + "_" + a.UniqueName.ToLowerInvariant() + "\"," + a.Handle + "}"));

		var lua = new StringBuilder();
		lua.Append("if QM_QU==nil then QM_QU=quest.QuestUpdate quest.QuestUpdate=function(a,b) ui.SysMsg('QU '..tostring(a)..' '..tostring(b)) return QM_QU(a,b) end end\n");
		lua.Append("local k={" + entries + "} local done=false\n");
		lua.Append("for i=1,#k do local t=_G['g_table_'..k[i][1]] local a=world.GetActor(k[i][2]) local s=k[i][1]\n");
		lua.Append("if a then local p=a:GetPos() s=s..string.format(' actor=%.0f,%.0f,%.0f',p.x,p.y,p.z) end\n");
		lua.Append("if t then local n=t[3] s=s..string.format(' event=%.0f,%.0f,%.0f',n[2],n[3],n[4])\n");
		lua.Append("if not done then done=true QM_QU(k[i][1],'None') QM_QU(k[i][1],'I_quest_mask_possible') s=s..' FORCED' end end\n");
		lua.Append("ui.SysMsg(s) end\n");

		Send.ZC_EXEC_CLIENT_SCP(sender.Connection, lua.ToString());
		sender.Quests.UpdateClient_QuestMarks();
		return CommandResult.Okay;
	}

	protected override void Ready(Character character)
	{
		this.SendAllScripts(character);
		character.Quests.UpdateClient();
	}

	private CommandResult HandleQuestSearch(Character sender, Character target, string message, string commandName, Arguments args)
	{
		var searchText = args.Count > 0 ? args.Get(0) : "";
		var lua = "M_QUESTS_SET_SEARCH(\"" + searchText.Replace("\"", "\\\"") + "\")";
		Send.ZC_EXEC_CLIENT_SCP(sender.Connection, lua);
		return CommandResult.Okay;
	}

	private CommandResult HandleQuest(Character sender, Character target, string message, string commandName, Arguments args)
	{
		if (args.Count < 2)
		{
			Log.Debug("CustomQuestSystemClientScript: Not enough arguments for quest command in message '{0}'.", message);
			return CommandResult.Okay;
		}

		var hexObjectId = args.Get(1).Replace("0x", "");

		if (!long.TryParse(hexObjectId, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var questObjectId))
		{
			Log.Debug("CustomQuestSystemClientScript: Failed to parse quest object id '{0}' in message '{1}'.", args.Get(1), message);
			return CommandResult.Okay;
		}

		if (!sender.Quests.TryGet(questObjectId, out var quest))
		{
			Log.Debug("CustomQuestSystemClientScript: User '{0}' tried to interact with a quest they don't have.", sender.Username);
			return CommandResult.Okay;
		}

		var action = args.Get(0).ToLowerInvariant();

		switch (action)
		{
			case "complete":
			{
				if (!ZoneServer.Instance.Conf.World.GetBool("quest_completion_from_ui", false))
				{
					Log.Debug("CustomQuestSystemClientScript: User '{0}' tried to complete a quest from the UI while it's disabled.", sender.Username);
					return CommandResult.Okay;
				}

				if (!sender.Quests.IsCompletable(quest.Data.Id))
				{
					Log.Debug("CustomQuestSystemClientScript: User '{0}' tried to complete a quest that isn't completable.", sender.Username);
					return CommandResult.Okay;
				}

				sender.Quests.Complete(quest);
				break;
			}
			case "warp":
			{
				if (!ZoneServer.Instance.Conf.World.GetBool("quest_return_button", true))
				{
					Log.Debug("CustomQuestSystemClientScript: User '{0}' tried to warp back while the return button is disabled.", sender.Username);
					return CommandResult.Okay;
				}

				if (!quest.ObjectivesCompleted)
				{
					Log.Debug("CustomQuestSystemClientScript: User '{0}' tried to warp back on a quest that isn't done.", sender.Username);
					return CommandResult.Okay;
				}

				if (!QuestComponent.TryGetPhaseDestination(quest, out var mapClassName, out var position))
				{
					sender.ServerMessage(L("There's nowhere to return to for this quest."));
					return CommandResult.Okay;
				}

				sender.Warp(mapClassName, position);
				break;
			}
			case "cancel":
			{
				if (!quest.Data.Cancelable)
				{
					Log.Debug("CustomQuestSystemClientScript: User '{0}' tried to cancel a quest that can't be canceled.", sender.Username);
					return CommandResult.Okay;
				}

				sender.Quests.Cancel(quest);
				break;
			}
			case "track":
			{
				if (args.Count < 3)
				{
					Log.Debug("CustomQuestSystemClientScript: Not enough arguments for 'track' action in message '{0}'.", message);
					return CommandResult.Okay;
				}

				var enabled = args.Get(2) == "true";

				quest.Tracked = enabled;
				break;
			}
			default:
			{
				Log.Debug("CustomQuestSystemClientScript: Unknown action '{0}' in message '{1}'.", action, message);
				break;
			}
		}

		return CommandResult.Okay;
	}
}
