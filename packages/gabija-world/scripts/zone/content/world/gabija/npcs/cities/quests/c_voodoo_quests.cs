//--- Melia Script ----------------------------------------------------------
// Bokor Master's Home Quest NPCs
//--- Description -----------------------------------------------------------
// The Bokor Master, who reads the slate the Crystal Mine pillar held.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using static Melia.Zone.Scripting.Shortcuts;

public class CVoodooQuestNpcsScript : GeneralScript
{
	private readonly static QuestId Slate2 = new QuestId(20051);

	protected override void Load()
	{
		// Bokor Master
		//-------------------------------------------------------------------------
		AddNpc(20136, L("[Bokor Master]{nl}Mama Marie Lavoie"), "MASTER_BOCORS", "c_voodoo", -22, 32, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bokor Master"));
			dialog.SetPortrait("Dlg_port_BOCOR");

			if (character.Quests.IsActive(Slate2) && character.Quests.IsCompletable(Slate2))
			{
				await dialog.Msg(L("Please use your time wisely. As no one sees your future better than yourself."));
				return;
			}

			if (character.Quests.IsActive(Slate2))
			{
				character.Quests.ReplayQuestTrack(Slate2);
				return;
			}

			await dialog.Msg(L("Please use your time wisely. As no one sees your future better than yourself."));
		});
	}
}
