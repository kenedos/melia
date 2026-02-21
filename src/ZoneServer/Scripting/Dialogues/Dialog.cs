using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Events;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting.Hooking;
using Melia.Zone.World;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Yggdrasil.Extensions;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Logging;
using static Melia.Zone.Scripting.Hooking.DialogHook;
using static Melia.Zone.Scripting.Shortcuts;

namespace Melia.Zone.Scripting.Dialogues
{
	/// <summary>
	/// Manages a dialog between a player and an NPC and allows sending
	/// of messages to the player.
	/// </summary>
	public class Dialog : IAsyncDisposable
	{
		private const string NpcNameSeperator = "*@*";
		private const string NpcDialogTextSeperator = "\\";
		private static readonly Regex ReplaceWhitespace = new(@"\s+", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private string _response;
		private readonly SemaphoreSlim _resumeSignal = new(0);
		private readonly CancellationTokenSource _cancellation = new();
		private readonly IZoneConnection _connection;
		private readonly object _resumeLock = new();

		/// <summary>
		/// Returns a reference to the actor that initiated the trigger.
		/// </summary>
		public IActor Initiator { get; }

		/// <summary>
		/// Returns a reference to the actor that was triggered.
		/// </summary>
		public IActor Trigger { get; }

		/// <summary>
		/// Returns a reference to the item that was triggered.
		/// </summary>
		public Item Item { get; }

		/// <summary>
		/// Returns a reference to the character that initiated the dialog.
		/// </summary>
		public Character Player
		{
			get
			{
				if (this.Initiator is not Character character)
					throw new InvalidOperationException($"The triggerer is not of type Character, but {this.Initiator.GetType().Name}.");

				return character;
			}
		}

		/// <summary>
		/// Returns a reference to the NPC the player is talking to.
		/// </summary>
		public Npc Npc => this.Trigger as Npc;

		/// <summary>
		/// Gets or sets the dialog's initiation type.
		/// </summary>
		public DialogStartType StartType { get; set; }

		/// <summary>
		/// Gets or sets the dialog's current state.
		/// </summary>
		public DialogState State { get; set; }

		/// <summary>
		/// Gets the type of response the dialog is currently expecting.
		/// Used to prevent stale packets from being processed.
		/// </summary>
		public DialogResponseType ExpectedResponseType { get; private set; }

		/// <summary>
		/// Returns the data for a potentially open shop.
		/// </summary>
		public ShopData Shop { get; private set; }

		/// <summary>
		/// Returns the title that's display on the dialog window.
		/// </summary>
		public string Title { get; private set; }

		/// <summary>
		/// Returns the name of the NPC dialog to use for displaying
		/// a portrait.
		/// </summary>
		public string Portrait { get; private set; }

		/// <summary>
		/// Creates and prepares a new dialog between the initiator
		/// and the trigger.
		/// </summary>
		/// <param name="initiator"></param>
		/// <param name="trigger"></param>
		public Dialog(IActor initiator, IActor trigger)
		{
			this.Initiator = initiator;
			this.Trigger = trigger;

			if (initiator is Character character)
			{
				_connection = character.Connection;

				if (_connection.CurrentDialog != null)
				{
					// Prevent starting a new dialog if one is already in progress.
					throw new InvalidOperationException($"Character '{character.Name}' tried to start a dialog while another was already active.");
				}
				_connection.CurrentDialog = this;
			}
		}

		/// <summary>
		/// Creates and prepares a new dialog between the initiator
		/// and the trigger.
		/// </summary>
		/// <param name="initiator"></param>
		/// <param name="triggerItem"></param>
		public Dialog(IActor initiator, IActor trigger, Item triggerItem) : this(initiator, trigger)
		{
			this.Item = triggerItem;
		}

		/// <summary>
		/// Sets response and resumes dialog after a Select.
		/// </summary>
		/// <param name="response"></param>
		/// <param name="responseType">The type of response being provided.</param>
		internal void Resume(string response, DialogResponseType responseType)
		{
			lock (_resumeLock)
			{
				if (this.State != DialogState.Waiting)
					return;

				// Validate that the response type matches what we're expecting.
				// This prevents stale packets (e.g., CZ_DIALOG_ACK arriving when
				// expecting CZ_DIALOG_SELECT) from being processed incorrectly.
				if (this.ExpectedResponseType != responseType)
					return;

				this.State = DialogState.Active;
				this.ExpectedResponseType = DialogResponseType.None;
				_response = response;
			}

			if (response != null)
				Send.ZC_DIALOG_CLOSE(this.Player.Connection);

			_resumeSignal.Release();
		}

		/// <summary>
		/// Sets the title to display on the dialog window. Set to null
		/// for the default title.
		/// </summary>
		/// <param name="title"></param>
		public void SetTitle(string title)
			=> this.Title = title;

		/// <summary>
		/// Sets the dialog class to use in message, which affects the
		/// displayed portrait. Set to null for the default.
		/// </summary>
		/// <remarks>
		/// The image name refers to the name of an image file in the
		/// client's npcimg folder, without the file extension. However,
		/// the desired image needs to be referenced in the dialog database
		/// for the image to be recognized.
		/// </remarks>
		/// <param name="imageName"></param>
		public void SetPortrait(string imageName)
			=> this.Portrait = imageName;

		/// <summary>
		/// Returns delegates that translate strings to the language
		/// selected by the player.
		/// </summary>
		/// <param name="L"></param>
		/// <param name="LN"></param>
		public void PlayerLocalization(out Func<string, string> L, out Func<string, string, int, string> LN)
		{
			L = this.Player.Localizer.Get;
			LN = this.Player.Localizer.GetPlural;
		}

		/// <summary>
		/// Prepares message to get ready to be sent to the client.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		private string FrameMessage(string message)
		{
			if (IsLocalizationKey(message))
				return WrapLocalizationKey(message);

			if (this.IsClientDialog(message))
				return message;

			message = this.InsertNewLine(message);
			message = this.ReplaceCustomCodes(message);
			message = this.AddNpcIdentity(message);

			return message;
		}

		public string InsertNewLine(string text)
		{
			var formattedText = new StringBuilder();
			var currentLength = 0;

			foreach (var word in text.Split(' '))
			{
				formattedText.Append(word);
				currentLength += word.Length;

				//if (currentLength + word.Length > 120 || word.EndsWith('.') || word.EndsWith('!') || word.EndsWith('?'))

				if (currentLength > 0)
				{
					formattedText.Append(' ');
					currentLength++;
				}

				if (currentLength > 80)
				{
					formattedText.Append("{nl}");
					currentLength = 0;
				}
			}

			return formattedText.ToString();
		}

		/// <summary>
		/// Adds NPC name and portrait to message.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		private string AddNpcIdentity(string message)
		{
			if (this.IsClientDialog(message))
				return message;

			// If the title was set to a valid dialog entry, we'll use that
			// one to get the title and portrait from the dialog database
			if (this.Npc != null && this.Title != null && this.Portrait == null && ZoneServer.Instance.Data.DialogDb.Contains(this.Title))
			{
				message = this.Title + NpcDialogTextSeperator + message;
				return message;
			}

			// Prepend title, controlling title displayed on the dialog
			// window.
			if (!message.Contains(NpcNameSeperator) && !message.Contains(NpcDialogTextSeperator))
			{
				var dialogTitle = this.GetNpcDialogTitle();
				if (!string.IsNullOrEmpty(dialogTitle))
				{
					message = dialogTitle + NpcNameSeperator + message;
				}
			}

			// Prepend dialog class name if one was set. This controls the
			// portrait and also the title if no custom title was set.
			if (this.Npc != null && !message.Contains(NpcDialogTextSeperator) && this.Portrait != null)
			{
				message = this.Portrait + NpcDialogTextSeperator + message;
			}

			return message;
		}

		/// <summary>
		/// Returns the title to display on the dialog window.
		/// </summary>
		/// <returns></returns>
		private string GetNpcDialogTitle()
		{
			if (this.Title != null)
				return this.Title;

			if (this.Npc == null)
				return string.Empty;

			// If not title was set, we use the NPC's name, and
			// since NPCs often times use a two line name, with a
			// tag and their actual name, we need to remove excess
			// whitespaces and line breaks from it, so it displays
			// properly, in one line, during the dialog.
			// Could possibly be done once on creation.

			var dialogDisplayName = this.Npc.Name;
			dialogDisplayName = dialogDisplayName.Replace("{nl}", " ");
			dialogDisplayName = dialogDisplayName.Replace("[", "");
			dialogDisplayName = dialogDisplayName.Replace("]", "");
			dialogDisplayName = ReplaceWhitespace.Replace(dialogDisplayName, " ");

			return dialogDisplayName;
		}

		/// <summary>
		/// Replaces custom codes in message.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		private string ReplaceCustomCodes(string message)
		{
			// {pcname} Character name
			if (message.Contains("{pcname}"))
				message = message.Replace("{pcname}", this.Player.Name);

			// {teamname} Character team name
			if (message.Contains("{teamname}"))
				message = message.Replace("{teamname}", this.Player.TeamName);

			// {fullname} Character name + team name
			if (message.Contains("{fullname}"))
				message = message.Replace("{fullname}", this.Player.Name + " " + this.Player.TeamName);

			return message;
		}

		/// <summary>
		/// Returns true if value looks like a localization key.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		internal static bool IsLocalizationKey(string value)
		{
			return (value.StartsWith("ETC_") || value.StartsWith("QUEST_"));
		}

		/// <summary>
		/// Returns true if value is a known client-side dialog name.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private bool IsClientDialog(string value)
			=> ZoneServer.Instance.Data.DialogDb.Exists(value);

		/// <summary>
		/// Wraps key with dictonary id code.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		internal static string WrapLocalizationKey(string key)
		{
			if (key.StartsWith("@dicID_^*$") && key.EndsWith("$*^"))
				return key;

			return ("@dicID_^*$" + key + "$*^");
		}

		/// <summary>
		/// Sends message to player, adding to any messages already
		/// in the dialog box..
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public async Task Msg(string format, params object[] args)
		{
			if (args.Length > 0)
				format = string.Format(format, args);

			await this.Msg(format);
		}

		/// <summary>
		/// Sends message to player, adding to any messages already
		/// in the dialog box.
		/// </summary>
		/// <param name="text"></param>
		public async Task Msg(string text)
		{
			ZoneServer.Instance.ServerEvents.PlayerDialog.Raise(new PlayerDialogEventArgs(this.Player, this.Npc, this.GetNpcDialogTitle(), text));

			text = this.FrameMessage(text);
			Send.ZC_DIALOG_OK(this.Player.Connection, text);

			this.ExpectedResponseType = DialogResponseType.Ack;
			await this.GetClientResponse();
		}

		/// <summary>
		/// Returns a mutable list of options to be passed to the Select
		/// method.
		/// </summary>
		/// <param name="options"></param>
		/// <returns></returns>
		public List<DialogOption> CreateOptions(params DialogOption[] options)
			=> options.Where(option => option.Enabled()).ToList();

		/// <summary>
		/// Creates a mutable list of options that can be modified before
		/// it's passed to the Select method.
		/// </summary>
		/// <example>
		// var options = dialog.Options(Option("Nothing", "nothing"), Option("Everything", "everything"));
		// if (xHappened)
		//     options.Add(Option("OMG, did you hear?", "omg"));
		// 
		// await dialog.Select("What's up?", options);
		/// </example>
		/// <param name="options"></param>
		/// <returns></returns>
		public DialogOptionList Options(params DialogOption[] options)
			=> new(options);

		/// <summary>
		/// Shows a menu with options to select from, returns the key
		/// of the selected option.
		/// </summary>
		/// <param name="text">Text to display with the options.</param>
		/// <param name="options">List of options to select from.</param>
		/// <returns></returns>
		public async Task<string> Select(string text, params DialogOption[] options)
			=> await this.Select(text, (IEnumerable<DialogOption>)options);

		/// <summary>
		/// Shows a menu with options to select from, returns the key
		/// of the selected option.
		/// </summary>
		/// <param name="text">Text to display with the options.</param>
		/// <param name="options">List of options to select from.</param>
		/// <returns></returns>
		public async Task<string> Select(string text, IEnumerable<DialogOption> options)
		{
			// Go through SelectSimple to get the integer response
			// and then look up the key in the options to return it.
			var enabledOptions = options.Where(a => a.Enabled());
			var optionsTexts = enabledOptions.Select(a => a.Text);
			var selectedIndex = await this.Select(text, optionsTexts);

			var response = enabledOptions.ElementAt(selectedIndex - 1).Key;
			return response;
		}

		/// <summary>
		/// Shows a menu with options to select from and returns the
		/// index of the selected option, starting at 1. Returns 0 in
		/// case of errors.
		/// </summary>
		/// <param name="text">Text to display with the options.</param>
		/// <param name="options">List of options to select from.</param>
		/// <returns></returns>
		public async Task<int> Select(string text, params string[] options)
			=> await this.Select(text, (IEnumerable<string>)options);

		/// <summary>
		/// Shows a menu with options to select from and returns the
		/// index of the selected option, starting at 1. Returns 0 in
		/// case of errors.
		/// </summary>
		/// <param name="text">Text to display with the options.</param>
		/// <param name="options">List of options to select from.</param>
		/// <returns></returns>
		public async Task<int> Select(string text, IEnumerable<string> options)
		{
			if (this.Npc != null)
			{
				ZoneServer.Instance.ServerEvents.PlayerDialog.Raise(new PlayerDialogEventArgs(this.Player, this.Npc, this.GetNpcDialogTitle(), text));
			}

			text = this.FrameMessage(text);

			var arguments = new List<string>();
			arguments.Add(text);
			arguments.AddRange(options);

			Send.ZC_DIALOG_SELECT(this.Player.Connection, arguments);

			this.ExpectedResponseType = DialogResponseType.Select;
			var response = await this.GetClientResponse();

			// Parse selected index
			if (!int.TryParse(response, out var selectedIndex))
			{
				Log.Warning("Dialog.SelectSimple: Unexpected non-integer response '{0}'.", response);
				selectedIndex = 0;
				this.Close();
			}
			// Check range
			else if (selectedIndex < 0 || selectedIndex > options.Count())
			{
				Log.Warning("Dialog.SelectSimple: Unexpected out-of-range response '{0}/{1}'.", selectedIndex, options.Count());
				selectedIndex = 0;
				this.Close();
			}

			return selectedIndex;
		}

		/// <summary>
		/// Shows a dialog with "Yes" and "No" options.
		/// </summary>
		/// <param name="text">The question to ask the player.</param>
		/// <returns>True if the player selected "Yes", false otherwise.</returns>
		public async Task<bool> YesNo(string text)
		{
			// The keys "1" and "2" are what the client sends back for the first and second option respectively.
			// Using the integer Select method and checking the result is the most direct way.
			var result = await this.Select(text, L("Yes"), L("No"));

			// result will be 1 for "Yes", 2 for "No", and 0 if the dialog was closed/cancelled.
			return result == 1;
		}

		/// <summary>
		/// Sends dialog input message, showing a message and a text field
		/// for the user to put in a string.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public async Task<string> Input(string message)
		{
			message = this.FrameMessage(message);
			Send.ZC_DIALOG_STRINGINPUT(this.Player.Connection, message);

			this.ExpectedResponseType = DialogResponseType.StringInput;
			var response = await this.GetClientResponse();
			return response;
		}

		/// <summary>
		/// Starts a time action, showing a progressbar with the message
		/// and character animation for the given duration.
		/// </summary>
		/// <param name="displayText"></param>
		/// <param name="animationName"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public async Task<TimeActionResult> TimeAction(string displayText, string animationName, TimeSpan duration)
			=> await this.TimeAction(displayText, "None", animationName, duration);

		/// <summary>
		/// Starts a time action, showing a progressbar with the message
		/// and character animation for the given duration.
		/// </summary>
		/// <param name="displayText"></param>
		/// <param name="buttonText"></param>
		/// <param name="animationName"></param>
		/// <param name="duration"></param>
		/// <returns></returns>
		public async Task<TimeActionResult> TimeAction(string displayText, string buttonText, string animationName, TimeSpan duration)
		{
			Send.ZC_DIALOG_CLOSE(this.Player.Connection);
			return await this.Player.Components.Get<TimeActionComponent>().StartAsync(displayText, buttonText, animationName, duration);
		}

		/// <summary>
		/// Executes the given hooks, if any, and returns true if any were
		/// executed.
		/// </summary>
		/// <param name="hookName"></param>
		/// <returns></returns>
		public async Task<bool> HooksByDialogName(string hookName)
		{
			return await this.Hooks(this.Npc.DialogName, hookName);
		}

		/// <summary>
		/// Executes the given hooks, if any, and returns true if any were
		/// executed.
		/// </summary>
		/// <param name="hookName"></param>
		/// <returns></returns>
		public async Task<bool> Hooks(string hookName)
		{
			return await this.Hooks(this.Npc.UniqueName, hookName);
		}

		/// <summary>
		/// Executes the given hooks, if any, and returns true if any were
		/// executed.
		/// </summary>
		/// <param name="hookName"></param>
		/// <returns></returns>
		public async Task<bool> Hooks(string owner, string hookName)
		{
			// Skip hooks for mobs.
			if (this.Initiator is Mob)
				return false;

			var hooks = ScriptHooks.GetAll<DialogHook>(owner, hookName);
			if (hooks.Length == 0)
				return false;

			var wasHooked = false;

			foreach (var hook in hooks)
			{
				var result = await hook.Func(this);

				switch (result)
				{
					case HookResult.Skip:
						continue;

					case HookResult.Continue:
						wasHooked = true;
						continue;

					case HookResult.Break:
						wasHooked = true;
						break;
				}
			}

			return wasHooked;
		}

		/// <summary>
		/// Executes the given hooks sequentially until one returns Break.
		/// Returns the HookResult of the *last executed* hook that didn't return Skip,
		/// or Skip if all hooks returned Skip. Prioritizes returning Break if encountered.
		/// </summary>
		/// <param name="hooks">The hook functions to execute.</param>
		/// <returns>The final HookResult (Skip, Continue, or Break).</returns>
		public async Task<HookResult> Hooks(params DialogHookFunc[] hooks) // Assuming DialogHookFunc is Func<Dialog, Task<HookResult>>
		{
			// Skip hooks for mobs - Keep this check if desired
			if (this.Initiator is Mob)
				return HookResult.Skip;

			if (hooks == null || hooks.Length == 0)
				return HookResult.Skip;

			HookResult finalResult = HookResult.Skip; // Default to Skip

			foreach (var hook in hooks)
			{
				if (hook == null) continue; // Skip null hooks

				var result = await hook(this);

				// Update finalResult based on the current hook's outcome
				if (result == HookResult.Continue)
				{
					finalResult = HookResult.Continue; // Mark that at least one hook continued
				}
				else if (result == HookResult.Break)
				{
					finalResult = HookResult.Break; // Break is prioritized
					break; // Stop executing further hooks
				}
				// If result is Skip, finalResult remains unchanged unless it was already Continue/Break
			}

			return finalResult;
		}

		/// <summary>
		/// Waits for the client to respond and returns the response.
		/// </summary>
		/// <returns></returns>
		private async Task<string> GetClientResponse()
		{
			lock (_resumeLock)
			{
				this.State = DialogState.Waiting;
			}

			await _resumeSignal.WaitAsync(_cancellation.Token);

			return _response;
		}

		/// <summary>
		/// Cancels a waiting dialog by signaling its cancellation token.
		/// This unblocks any pending GetClientResponse() call so the
		/// dialog task can exit cleanly via OperationCanceledException.
		/// </summary>
		internal void Cancel()
		{
			try { _cancellation.Cancel(); }
			catch (ObjectDisposedException) { }
		}

		/// <summary>
		/// Closes the dialog.
		/// </summary>
		/// <exception cref="OperationCanceledException"></exception>
		public void Close()
		{
			Send.ZC_DIALOG_CLOSE(this.Player.Connection);
			throw new OperationCanceledException("Dialog closed by script.");
		}

		/// <summary>
		/// Opens the player's personal storage.
		/// </summary>
		/// <returns></returns>
		public async Task OpenPersonalStorage()
		{
			var result = this.Player.PersonalStorage.Open();
			if (result != StorageResult.Success)
				return;
			this.ExpectedResponseType = DialogResponseType.Ack;
			await this.GetClientResponse();
		}

		/// <summary>
		/// Opens the player's team/account storage.
		/// </summary>
		/// <returns></returns>
		public async Task OpenTeamStorage()
		{
			var result = this.Player.TeamStorage.Open();
			if (result != StorageResult.Success)
				return;
			this.ExpectedResponseType = DialogResponseType.Ack;
			await this.GetClientResponse();
		}

		/// <summary>
		/// Opens the shop with the given name for the player.
		/// </summary>
		/// <param name="shopName"></param>
		public async Task OpenShop(string shopName)
		{
			if (!ZoneServer.Instance.Data.ShopDb.TryFind(shopName, out var shopData))
				throw new ArgumentException($"Shop '{shopName}' not found.");

			await this.OpenShop(shopData);
		}

		/// <summary>
		/// Opens the given shop for the player.
		/// </summary>
		/// <param name="shopData"></param>
		public async Task OpenShop(ShopData shopData)
		{
			this.Shop = shopData;

			// If this is a custom shop, we need to set it up on the client
			// by executing some custom Lua code.
			if (shopData.IsCustom)
			{
				// --- Calculate Reputation Discount ---
				var discountMultiplier = 1.0f; // 1.0 means no discount (100% price)
				var shopFactionId = this.Npc.GetRegionFaction(); // Determine shop's faction
				var factionDisplayName = "The Local Authorities"; // Default display name

				if (!string.IsNullOrEmpty(shopFactionId))
				{
					factionDisplayName = ZoneServer.Instance.World.Factions.GetFactionDisplayName(shopFactionId);
					var reputation = ZoneServer.Instance.World.Factions.GetReputation(this.Player, shopFactionId);
					var tier = ZoneServer.Instance.World.Factions.GetTier(reputation);

					// Define discounts per tier (as multipliers: 0.90 = 10% discount)
					switch (tier)
					{
						case ReputationTier.Hated: discountMultiplier = 1.25f; break;
						case ReputationTier.Disliked: discountMultiplier = 1.10f; break;
						case ReputationTier.Neutral: break;
						case ReputationTier.Liked: discountMultiplier = 0.90f; break;
						case ReputationTier.Honored: discountMultiplier = 0.75f; break;
						default: discountMultiplier = 1.0f; break;
					}

					// Notify player of discount/markup (only if not neutral)
					if (discountMultiplier < 1.0f)
					{
						var discountPercent = (int)((1.0f - discountMultiplier) * 100);
						// Use a non-blocking message or send after shop opens if possible
						this.Player.ServerMessage(L($"Your {tier} standing with {factionDisplayName} grants you a {discountPercent}% discount here!"));
					}
					else if (discountMultiplier > 1.0f)
					{
						var markupPercent = (int)((discountMultiplier - 1.0f) * 100);
						this.Player.ServerMessage(L($"Your {tier} standing with {factionDisplayName} results in a {markupPercent}% price increase here!"));
					}
				}
				// --- End Calculate Reputation Discount ---

				// Start receival of the shop data
				Send.ZC_EXEC_CLIENT_SCP(this.Player.Connection, "Melia.Comm.BeginRecv('CustomShop')");

				// Append products and send them in intervals when the calls
				// get too close to the max script length
				var sb = new StringBuilder();
				foreach (var productData in shopData.Products.Values)
				{
					var meetsRequirement = true;
					if (!string.IsNullOrEmpty(productData.RequiredFactionId))
					{
						// Compare player's current integer rep with the required integer value
						var playerRep = ZoneServer.Instance.World.Factions.GetReputation(this.Player, productData.RequiredFactionId);
						if (playerRep < productData.RequiredTierValue) // Check against stored integer value
						{
							meetsRequirement = false; // Player doesn't meet reputation requirement
						}
					}

					if (!meetsRequirement)
						continue;

					var basePrice = (int)(productData.Price * productData.PriceMultiplier * discountMultiplier);
					sb.AppendFormat("{{ {0},{1},{2},{3} }},", productData.Id, productData.ItemId, productData.Amount, basePrice);

					if (sb.Length > ClientScript.ScriptMaxLength * 0.8)
					{
						Send.ZC_EXEC_CLIENT_SCP(this.Player.Connection, $"Melia.Comm.Recv('CustomShop', {{ {sb} }})");
						sb.Clear();
					}
				}

				// Send remaining products, which will cover all items
				// if the max script length wasn't exceeded, and the
				// remaining ones that weren't sent yet.
				if (sb.Length > 0)
				{
					Send.ZC_EXEC_CLIENT_SCP(this.Player.Connection, $"Melia.Comm.Recv('CustomShop', {{ {sb} }})");
					sb.Clear();
				}

				// End receival of the shop data and set it
				Send.ZC_EXEC_CLIENT_SCP(this.Player.Connection, "Melia.Comm.ExecData('CustomShop', M_SET_CUSTOM_SHOP)");
				Send.ZC_EXEC_CLIENT_SCP(this.Player.Connection, "Melia.Comm.EndRecv('CustomShop')");

				// Open the shop
				Send.ZC_DIALOG_TRADE(this.Player.Connection, "MeliaCustomShop");
			}
			else
			{
				Send.ZC_DIALOG_TRADE(this.Player.Connection, shopData.Name);
			}

			this.ExpectedResponseType = DialogResponseType.Ack;
			await this.GetClientResponse();
		}

		/// <summary>
		/// Opens a custom companion shop with the given name.
		/// </summary>
		/// <param name="shopName"></param>
		public async Task OpenCustomCompanionShop(string shopName)
		{
			if (!ZoneServer.Instance.Data.CompanionShopDb.TryFind(shopName, out var shopData))
				throw new ArgumentException($"Companion shop '{shopName}' not found.");

			// Start receival of companion data
			Send.ZC_EXEC_CLIENT_SCP(this.Player.Connection, "Melia.Comm.BeginRecv('CustomCompanionShop')");

			// Send companion data
			var sb = new StringBuilder();
			foreach (var productData in shopData.Products.Values)
			{
				sb.AppendFormat("{{\"{0}\",{1}}},", productData.CompanionClassName, productData.Price);

				if (sb.Length > ClientScript.ScriptMaxLength * 0.8)
				{
					Send.ZC_EXEC_CLIENT_SCP(this.Player.Connection, $"Melia.Comm.Recv('CustomCompanionShop', {{ {sb} }})");
					sb.Clear();
				}
			}

			// Send remaining companions
			if (sb.Length > 0)
			{
				Send.ZC_EXEC_CLIENT_SCP(this.Player.Connection, $"Melia.Comm.Recv('CustomCompanionShop', {{ {sb} }})");
				sb.Clear();
			}

			// End receival and set the companion shop
			Send.ZC_EXEC_CLIENT_SCP(this.Player.Connection, "Melia.Comm.ExecData('CustomCompanionShop', M_SET_CUSTOM_COMPANION_SHOP)");
  
			Send.ZC_EXEC_CLIENT_SCP(this.Player.Connection, "Melia.Comm.EndRecv('CustomCompanionShop')");

			// Open the companion shop UI
			Send.ZC_ADDON_MSG(this.Player, "OPEN_DLG_COMPANIONSHOP", 0, "Normal");

			this.ExpectedResponseType = DialogResponseType.Ack;
			await this.GetClientResponse();
		}

		/// <summary>
		/// Saves the respawn location of the player in their current position.
		/// </summary>
		/// <returns></returns>
		public async Task SaveLocation()
		{
			this.Player.SetCityReturnLocation(new Location(this.Player.Map.Id, this.Player.Position));

			await Task.Yield();
		}

		/// <summary>
		/// Execute a client side script
		/// </summary>
		/// <param name="script"></param>
		/// <param name="args">arguments for the script</param>
		public async Task ExecuteScript(string script, params object[] args)
		{
			this.State = DialogState.Ended;
			this.Player.ExecuteClientScript(script, args);

			await Task.Yield();
		}

		/// <summary>
		/// Execute a client side script
		/// </summary>
		/// <param name="script"></param>
		/// <param name="stringParameter">arguments for the script</param>
		/// /// <param name="intParameter">arguments for the script</param>
		public void OpenAddon(string script, string stringParameter = null, int intParameter = 0)
		{
			this.State = DialogState.Ended;
			Task.Delay(100)
				.ContinueWith(_ => this.Player.AddonMessage(script, stringParameter, intParameter));
		}

		/// <summary>
		/// Custom dialog, predefined dialogs in the client
		/// </summary>
		/// <param name="function"></param>
		/// <param name="dialog"></param>
		/// <param name="argCount"></param>
		public async Task OpenCustomDialog(string function, string dialog = "", int argCount = 0)
		{
			Send.ZC_CUSTOM_DIALOG(this.Player.Connection, function, dialog, argCount);

			this.ExpectedResponseType = DialogResponseType.Ack;
			await this.GetClientResponse();
		}

		/// <summary>
		/// Open/Close predefined UIs in the client
		/// </summary>
		/// <param name="function"></param>
		/// <param name="isOpen"></param>
		public void OpenUI(string function, bool isOpen = true)
		{
			Send.ZC_UI_OPEN(this.Player.Connection, function, isOpen);
		}

		/// <summary>
		/// Leave the dialog trigger
		/// </summary>
		/// <remarks>
		/// Not too sure if this is used for anything - @SalmanTKhan
		/// </remarks>
		public void Leave()
		{
			Send.ZC_LEAVE_TRIGGER(this.Player.Connection);
		}

		/// <summary>
		/// Play an animation only visible to the dialog's 
		/// target player.
		/// </summary>
		/// <param name="animationId"></param>
		public void PlayAnimation(string animationId, bool stopOnLastFrame = false)
		{
			Send.ZC_PLAY_ANI(this.Player.Connection, this.Npc, animationId, stopOnLastFrame);
		}

		/// <summary>
		/// Play an animation only visible to the dialog's 
		/// target player.
		/// </summary>
		/// <param name="animationId"></param>
		public void PlayAnimation(IActor actor, string animationId, bool stopOnLastFrame = false)
		{
			Send.ZC_PLAY_ANI(this.Player.Connection, actor, animationId, stopOnLastFrame);
		}

		/// <summary>
		/// Show a chat bubble only visible to the dialog's 
		/// target player.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void Chat(string format, params object[] args)
		{
			Send.ZC_CHAT(this.Player.Connection, this.Npc, format, args);
		}

		/// <summary>
		/// Show a help message to the dialog's 
		/// target player.
		/// </summary>
		/// <param name="helpName"></param>
		public void ShowHelp(string helpName)
		{
			this.Player.ShowHelp(helpName);
		}

		/// <summary>
		/// Show a book to a player with given text.
		/// </summary>
		/// <param name="bookText"></param>
		public void ShowBook(string bookText)
		{
			Send.ZC_NORMAL.ShowBook(this.Player, bookText);
		}

		/// <summary>
		/// Show a scroll to a player with given text.
		/// </summary>
		/// <param name="scrollText"></param>
		public void ShowScroll(string scrollText)
		{
			Send.ZC_NORMAL.ShowScroll(this.Player, scrollText);
		}

		/// <summary>
		/// Unhide an NPC for a specific player
		/// </summary>
		/// <param name="npcName"></param>
		public void UnHideNPC(string npcName)
		{
			this.Player.UnHideNPC(npcName);
		}

		public void HideNPC(string npcName)
		{
			this.Player.HideNPC(npcName);
		}

		/// <summary>
		/// Attach an effect to an actor, only visible to dialog's player.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="packetString"></param>
		/// <param name="scale"></param>
		/// <param name="heightOffset"></param>
		public void AttachEffect(IActor actor, string packetString, float scale, EffectLocation heightOffset)
		{
			actor.AttachEffect(this.Player.Connection, packetString, scale, heightOffset);
		}

		/// <summary>
		/// Detach an effect from an actor, only visible to dialog's player.
		/// </summary>
		/// <param name="actor"></param>
		/// <param name="packetString"></param>
		public void DetachEffect(IActor actor, string packetString)
		{
			actor.DetachEffect(this.Player.Connection, packetString);
		}

		/// <summary>
		/// Implements IAsyncDisposable. This method is automatically called
		/// at the end of an 'await using' block.
		/// </summary>
		public ValueTask DisposeAsync()
		{
			// Cancel any pending semaphore waits so the dialog task
			// can exit instead of hanging forever.
			try { _cancellation.Cancel(); } catch (ObjectDisposedException) { }

			// Dispose unmanaged resources held by the CTS and semaphore.
			_cancellation.Dispose();
			_resumeSignal.Dispose();

			// Clean up the connection state.
			if (_connection?.CurrentDialog == this)
			{
				_connection.CurrentDialog = null;
			}
			return ValueTask.CompletedTask;
		}
	}

	/// <summary>
	/// A function that can be called to handle a dialog.
	/// </summary>
	/// <param name="dialog"></param>
	/// <returns></returns>
	public delegate Task DialogFunc(Dialog dialog);

	/// <summary>
	/// A function that returns a variable number of options and returns
	/// which one was selected.
	/// </summary>
	/// Necessary because you can't use params in Func/Action.
	/// <param name="options"></param>
	/// <returns></returns>
	public delegate Task<int> DialogSelectFunc(params string[] options);

	/// <summary>
	/// Defines how the dialog was initiated
	/// </summary>
	public enum DialogStartType
	{
		Trigger,
		Enter,
		Leave,
	}
}
