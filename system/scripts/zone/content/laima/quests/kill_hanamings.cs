//--- Melia Script ----------------------------------------------------------
// Hanaming Kill Quests
//--- Description -----------------------------------------------------------
// Automatically received test quest for killing Hanamings.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;

public class KillHanamings1QuestScript : QuestScript
{
	protected override void Load()
	{
		SetId(2000001);
		SetName("Hanamings Must Die");
		SetDescription("Collect their petals and get revenge for the fallen!");

		for (var i = 1; i <= 2; ++i)
		{
			var amount = i * 5;
			AddObjective("kill" + i, "Kill " + amount + " Hanamings", new KillObjective(amount, MonsterId.Hanaming));
		}
		AddObjective("collect", "Collect 5 Hanaming Petals.", new CollectItemObjective(ItemId.Leaf_Hanaming, 5));

		SetReceive(QuestReceiveType.Manual);
		AddPrerequisite(new LevelPrerequisite(1));

		AddDialogHook("Uska", "BeforeDialog", UskaDialog);
	}

	private async Task<HookResult> UskaDialog(Dialog dialog)
	{
		var character = dialog.Player;

		//if (character.Quests.IsPossible(this.QuestId))
		{
			if (character.Gender == Gender.Male)
				AddReward(new ItemReward(ItemId.HAIR_M_130, 1));
			else
				AddReward(new ItemReward(ItemId.HAIR_F_131, 1));
			character.Quests.Start(this.QuestId);
			return HookResult.Break;
		}

		//if (character.Inventory.HasItem(ItemId.Leaf_Hanaming, 5))
		{
			await dialog.Msg("Oh, you received my quest? Thank you very much for helping out with these pests.{nl}It was about time someone did something about them. Here's your reward.");

			character.Inventory.Remove(ItemId.Leaf_Hanaming, 5);
			character.Quests.Complete(this.QuestId);
		}

		return HookResult.Break;
	}
}
