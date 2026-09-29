//--- Melia Script ----------------------------------------------------------
// Dvasia Peak Quest NPCs
//--- Description -----------------------------------------------------------
// The commander leading the operation to open the road to the Great King's
// Gate.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Quests;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn22QuestNpcsScript : GeneralScript
{
	private readonly static QuestId SiaulWestHq01 = new QuestId(9100);

	protected override void Load()
	{
		// Commander Julian
		//-------------------------------------------------------------------------
		AddNpc(20107, L("Commander Julian"), "THORN22_JULIAN", "d_thorn_22", 13.53, -1289.19, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Commander Julian"));

			if (character.Quests.IsCompletable(SiaulWestHq01))
			{
				await dialog.Msg(L("That was great. You were of great help to this mission. What do you think about enlisting to the army?"));
				await dialog.CompleteQuest(SiaulWestHq01);
				return;
			}

			if (character.Quests.IsActive(SiaulWestHq01))
			{
				await dialog.Msg(L("Oh, I heard from Titas but I didn't think that's you. I feel reassured. Well then, please support the monster extermination mission."));
				return;
			}

			await Task.CompletedTask;
		});
	}
}
