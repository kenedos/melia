// ===================================================================
// CharacterDialog.cs - NPC and item dialog interactions
// ===================================================================
using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;
using static Melia.Shared.Util.TaskHelper;

namespace Melia.Zone.World.Actors.Characters
{
	public partial class Character
	{
		#region NPC Interaction
		private bool IsHidden(Npc npc)
		{
			var variableName = npc.GenType < 1_000_000 ? $"Melia.NPC.Visibility.{npc.Map.Id}:{npc.GenType}" : $"Melia.NPC.Visibility.{npc.Map.Id}:{npc.DialogName}";
			if (this.Variables.Perm.Has(variableName))
				return this.Variables.Perm.GetBool(variableName);
			return false;
		}

		/// <summary>
		/// Hides an NPC from this character by storing a visibility
		/// variable in the character's permanent variables.
		/// </summary>
		public void HideNPC(string npcDialogName)
		{
			if (!ZoneServer.Instance.World.NPCs.TryGetValue(npcDialogName, out var npc))
				return;
			if (npc.GenType < 1_000_000)
				this.Variables.Perm.SetBool($"Melia.NPC.Visibility.{npc.Map.Id}:{npc.GenType}", false);
			else
				this.Variables.Perm.SetBool($"Melia.NPC.Visibility.{npc.Map.Id}:{npc.DialogName}", true);
		}

		/// <summary>
		/// Makes a previously hidden NPC visible again for this character.
		/// </summary>
		public void UnHideNPC(string npcDialogName)
		{
			if (!ZoneServer.Instance.World.NPCs.TryGetValue(npcDialogName, out var npc))
				return;
			if (npc.GenType < 1_000_000)
				this.Variables.Perm.SetBool($"Melia.NPC.Visibility.{npc.Map.Id}:{npc.GenType}", true);
			else
				this.Variables.Perm.SetBool($"Melia.NPC.Visibility.{npc.Map.Id}:{npc.DialogName}", true);
		}

		/// <summary>
		/// Set a map-specific NPC state.
		/// </summary>
		public void SetMapNPCState(Npc npc, NpcState state)
		{
			this.Variables.Perm.Set($"{npc.Map.Id}:{npc.GenType}", (short)state);
			Send.ZC_SET_NPC_STATE(this.Connection, npc, (short)state);
		}

		/// <summary>
		/// Get a map-specific NPC state, returning the NPC's default state if not found.
		/// </summary>
		public NpcState GetMapNPCState(MonsterInName minMon)
		{
			if (minMon is Npc && this.Variables.Perm.Has($"{minMon.Map.Id}:{minMon.GenType}"))
				return (NpcState)this.Variables.Perm.GetShort($"{minMon.Map.Id}:{minMon.GenType}");
			return minMon.State;
		}

		/// <summary>
		/// Starts a dialog between the character and the given actor.
		/// </summary>
		public void StartDialog(IMonster monster)
			=> this.StartDialog(monster, monster.DialogName);

		/// <summary>
		/// Starts a dialog between the character and the given actor.
		/// </summary>
		public void StartDialog(IActor actor, string dialogName)
		{
			if (actor == null)
				throw new InvalidOperationException("StartDialog: Starting a remote dialog with a null actor.");
			if (!ZoneServer.Instance.DialogFunctions.TryGet(dialogName, out var dialogFunc))
			{
				if (actor is not Npc npc || npc.DialogFunc == null)
					throw new InvalidOperationException("StartDialog: Dialog function not found.");
				else
					dialogFunc = npc.DialogFunc;
			}

			// Create dialog SYNCHRONOUSLY to prevent race conditions.
			// This ensures CurrentDialog is set before we return, blocking
			// any subsequent CZ_CLICK_TRIGGER packets from starting new dialogs.
			Dialog dialog;
			try
			{
				dialog = new Dialog(this, actor, null);
			}
			catch (InvalidOperationException)
			{
				// Dialog already in progress - silently ignore.
				// This can happen with rapid clicking or network latency.
				return;
			}

			async Task RunActorDialogAsync(Dialog dlg)
			{
				await using (dlg)
				{
					try
					{
						dlg.State = DialogState.Active;
						await dialogFunc(dlg);
					}
					catch (OperationCanceledException) { /* Normal exit path */ }
					catch (Exception ex)
					{
						Log.Error($"An exception occurred during an NPC dialog for '{this.Name}' with NPC '{actor?.Name}'. Error: {ex}");
					}
					finally
					{
						dlg.State = DialogState.Ended;
						dlg.Close();
						dlg.Leave();
					}
				}
			}
			CallSafe(RunActorDialogAsync(dialog));
		}

		/// <summary>
		/// Starts a dialog between the character and the given NPC, using a specific dialog function.
		/// </summary>
		public void StartDialog(MonsterInName npc, DialogFunc dialogFunc)
		{
			if (npc == null)
				throw new InvalidOperationException("Starting a remote dialog with a null NPC");
			if (dialogFunc == null)
				throw new InvalidOperationException($"NPC '{npc.Name}' doesn't have a dialog function assigned to it.");

			// Create dialog SYNCHRONOUSLY to prevent race conditions.
			Dialog dialog;
			try
			{
				dialog = new Dialog(this, npc, null);
			}
			catch (InvalidOperationException)
			{
				// Dialog already in progress - silently ignore.
				return;
			}

			async Task RunNpcDialogAsync(Dialog dlg)
			{
				await using (dlg)
				{
					try
					{
						dlg.State = DialogState.Active;
						await dialogFunc(dlg);
					}
					catch (OperationCanceledException) { /* Normal exit path */ }
					catch (Exception ex)
					{
						Log.Error($"An exception occurred during an NPC dialog for '{this.Name}' with NPC '{npc?.Name}'. Error: {ex}");
					}
					finally
					{
						dlg.State = DialogState.Ended;
						dlg.Close();
						dlg.Leave();
					}
				}
			}
			CallSafe(RunNpcDialogAsync(dialog));
		}

		/// <summary>
		/// Starts a dialog from an item.
		/// </summary>
		public void StartDialog(Item item, Func<Dialog, Task> dialogFunc)
		{
			// Create dialog SYNCHRONOUSLY to prevent race conditions.
			Dialog dialog;
			try
			{
				dialog = new Dialog(this, null, item);
			}
			catch (InvalidOperationException)
			{
				// Dialog already in progress - silently ignore.
				return;
			}

			async Task RunItemDialogAsync(Dialog dlg)
			{
				await using (dlg)
				{
					try
					{
						dlg.State = DialogState.Active;
						await dialogFunc(dlg);
					}
					catch (OperationCanceledException) { /* Normal exit path */ }
					catch (Exception ex)
					{
						Log.Error($"An exception occurred during an item dialog for '{this.Name}'. Item: '{item?.Name}'. Error: {ex}");
					}
					finally
					{
						dlg.State = DialogState.Ended;
						dlg.Close();
						dlg.Leave();
					}
				}
			}
			CallSafe(RunItemDialogAsync(dialog));
		}
		#endregion
	}
}
