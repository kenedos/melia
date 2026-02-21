using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Dungeons;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// Contains implementations of common dungeon/mission functions
	/// to provide a simplified scripting API for common instance-wide actions.
	/// </summary>
	public abstract partial class DungeonScript
	{
		/// <summary>
		/// Sends an addon message to all characters in the dungeon instance.
		/// This is the C# equivalent of a generic MGAME_MSG function.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		/// <param name="functionName">The client addon function to call.</param>
		/// <param name="stringParameter">The string parameter to send.</param>
		/// <param name="intParameter">The integer parameter to send.</param>
		public virtual void MGameMessage(InstanceDungeon instance, string functionName, string stringParameter = null, int intParameter = 0)
		{
			foreach (var character in instance.Characters)
			{
				character?.AddonMessage(functionName, stringParameter, intParameter);
			}
		}

		/// <summary>
		/// Runs a client-side script for all players in the dungeon.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		/// <param name="script">The script string to execute.</param>
		public virtual void MGameRunClientScript(InstanceDungeon instance, string script)
		{
			foreach (var character in instance.Characters)
			{
				if (character?.Connection != null)
				{
					Send.ZC_EXEC_CLIENT_SCP(character.Connection, script);
				}
			}
		}

		/// <summary>
		/// Sets a timer variable in the dungeon instance and optionally opens a timer UI for all players.
		/// Replicates MGAME_SET_TIMER.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		/// <param name="key">The variable key to store the timer seconds.</param>
		/// <param name="duration">The duration of the timer.</param>
		/// <param name="uiMessage">Optional UI message to display in the client timer.</param>
		public virtual void MGameSetTimer(InstanceDungeon instance, string key, TimeSpan duration, string uiMessage = null)
		{
			instance.Vars.Set($"{key}_START_TIME", DateTime.UtcNow.Ticks);
			instance.Vars.Set(key, (int)duration.TotalSeconds);

			if (string.IsNullOrEmpty(uiMessage) || uiMessage == "None") return;

			string scriptToRun = key != "ToEndBattle"
				? $"OPEN_MGAME_TIMER(\"{uiMessage}#{key}\")"
				: "GUILDBATTLE_BATTLE_START_C()";

			this.MGameRunClientScript(instance, scriptToRun);
		}

		/// <summary>
		/// Changes the background music for all players in the instance.
		/// Replicates MGAME_START_CHANGE_BGM.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		/// <param name="bgmName">The class name of the BGM to play.</param>
		public virtual void MGameChangeBgm(InstanceDungeon instance, string bgmName)
		{
			if (string.IsNullOrEmpty(bgmName) || bgmName == "None") return;

			foreach (var character in instance.Characters)
			{
				if (character != null)
				{
					// This assumes a client script function 'PlayMusic' exists, which is common.
					this.MGameRunClientScript(instance, $"PlayMusic('{bgmName}', 1)");
				}
			}
		}

		/// <summary>
		/// Warps all players in the dungeon to a specific position.
		/// Replicates MGAME_EXEC_SETPOS.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		/// <param name="position">The position to warp players to.</param>
		public virtual void MGameWarpPlayers(InstanceDungeon instance, Position position)
		{
			foreach (var character in instance.Characters)
			{
				// The character.Warp method handles the teleportation logic.
				character?.SetPosition(position);
			}
		}

		/// <summary>
		/// Sets a value in the instance's shared variable dictionary.
		/// Replicates MGAME_C_SET_MVALUE.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		/// <param name="key">The name of the variable.</param>
		/// <param name="value">The value to set.</param>
		public virtual void MGameSetValue<T>(InstanceDungeon instance, string key, T value)
		{
			instance.Vars.Set(key, value);
		}

		/// <summary>
		/// Adds a numerical value to an existing instance variable.
		/// Replicates MGAME_C_SET_MVALUE_ADD.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		/// <param name="key">The name of the variable.</param>
		/// <param name="valueToAdd">The value to add.</param>
		public virtual void MGameAddValue(InstanceDungeon instance, string key, int valueToAdd)
		{
			var currentValue = instance.Vars.GetInt(key, 0);
			instance.Vars.SetInt(key, currentValue + valueToAdd);
		}

		/// <summary>
		/// Ends the dungeon instance and cleans it up.
		/// Replicates MGAME_END.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		/// <param name="isFailure">True to send the "FAIL" message, false otherwise.</param>
		public virtual void MGameEnd(InstanceDungeon instance, bool isFailure)
		{
			this.DungeonEnded(instance, isFailure);
		}

		/// <summary>
		/// Sends a formatted UI message with a specific icon to all players.
		/// Replicates MGAME_SET_DM_ICON.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		/// <param name="message">The text to display.</param>
		/// <param name="icon">The name of the icon (e.g., "scroll", "clear").</param>
		/// <param name="durationSeconds">How long the message should stay on screen.</param>
		public virtual void MGameSetIconMessage(InstanceDungeon instance, string message, string icon, int durationSeconds)
		{
			string functionName = $"NOTICE_Dm_{icon}";
			this.MGameMessage(instance, functionName, message, durationSeconds);
		}

		/// <summary>
		/// Displays a message that includes the current value of an instance variable.
		/// Replicates MGAME_MSG_VALUE.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		/// <param name="variableName">The name of the instance variable to display.</param>
		/// <param name="targetValue">The target value (e.g., for "5 / 10").</param>
		/// <param name="messageText">The base text of the message.</param>
		/// <param name="icon">The icon to display with the message.</param>
		/// <param name="duration">How long the message should stay on screen.</param>
		public virtual void MGameMessageWithValue(InstanceDungeon instance, string variableName, int targetValue, string messageText, string icon, int duration = 5)
		{
			if (duration <= 0) duration = 1;

			// Handle direct messages without a variable.
			if (string.IsNullOrEmpty(variableName) || variableName.Equals("DIRECT", StringComparison.OrdinalIgnoreCase))
			{
				MGameSetIconMessage(instance, messageText, icon, duration);
				return;
			}

			var currentValue = instance.Vars.GetInt(variableName);
			if (currentValue < 0 || currentValue >= targetValue) return;

			// The {nl} tag is a common client-side newline convention.
			var formattedMessage = $"{messageText}{{nl}}( {currentValue} / {targetValue} )";
			MGameSetIconMessage(instance, formattedMessage, icon, duration);
		}

		/// <summary>
		/// Revives all dead players in the instance to full health.
		/// Replicates MGAME_EXEC_REVIVE.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		public virtual void MGameReviveAll(InstanceDungeon instance)
		{
			foreach (var character in instance.Characters)
			{
				if (character != null && character.IsDead)
				{
					character.Resurrect(ResurrectOptions.TryAgain);
					//Send.ZC_REVIVE(character, character.ObjectId, character.Hp);
					//Send.ZC_MSG_STATE_CHANGE_2(character, Shared.Game.Const.State.Dead, 0);
					character.LookAround();
				}
			}
		}

		/// <summary>
		/// Freezes or unfreezes all actors on the instance layer.
		/// Replicates MGAME_EXEC_FREEZEZONE.
		/// </summary>
		/// <param name="instance">The dungeon instance.</param>
		/// <param name="freeze">True to freeze, false to unfreeze.</param>
		public virtual void MGameFreezeLayer(InstanceDungeon instance, bool freeze)
		{
			if (!ZoneServer.Instance.World.TryGetMap(this.MapName, out var map)) return;

			if (freeze)
			{
				map.FreezeLayer(instance.Layer);
			}
			else
			{
				map.UnfreezeLayer(instance.Layer);
			}
		}

		public virtual void MGameReturn()
		{
			// TODO: Figure out what this does?
		}
	}
}
