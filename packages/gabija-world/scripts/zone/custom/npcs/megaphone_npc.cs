//--- Melia Script ----------------------------------------------------------
// Megaphone Distributor NPC
//--- Description -----------------------------------------------------------
// Gives free megaphones to players who don't already have one.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Items;
using static Melia.Zone.Scripting.Shortcuts;

public class MegaphoneDistributorNpc : GeneralScript
{
	private const int MicItemId = 645001;

	protected override void Load()
	{
		AddNpc(20188, L("[Party Planner] Vivienne"), "c_Klaipe", -348, -127, 180, NpcDialog);
		AddNpc(20188, L("[Party Planner] Vivienne"), "c_orsha", -2, -14, 90, NpcDialog);
		AddNpc(20188, L("[Party Planner] Vivienne"), "c_fedimian", 155, -153, 0, NpcDialog);
	}

	private async Task NpcDialog(Dialog dialog)
	{
		var player = dialog.Player;

		dialog.SetTitle(L("Vivienne"));

		await dialog.Msg(L("Hey there, cutie! I'm Vivienne, party planner extraordinaire!"));
		await dialog.Msg(L("You know what makes a party really pop? When everyone can hear about it! That's where my little magic helpers come in."));

		if (player.Inventory.HasItem(MicItemId))
		{
			await dialog.Msg(L("Ooh, you've already got a Megaphone! Perfect! Now you can let everyone know when and where the fun's happening!"));
			await dialog.Msg(L("Just use /y and your message to shout it out to everyone! Go spread the good vibes!"));
			return;
		}

		var selection = await dialog.Select(L("You look like someone who knows how to have a good time! Want a Megaphone so you can rally the troops for some fun?"),
			Option(L("Yes please!"), "give"),
			Option(L("No thanks, I'm more of a wallflower."), "decline")
		);

		if (selection == "decline")
		{
			await dialog.Msg(L("Aww, that's a shame! But hey, if you ever change your mind and want to join the party, you know where to find me!"));
			return;
		}

		var mic = new Item(MicItemId, 1);
		player.Inventory.Add(mic);

		await dialog.Msg(L("Here you go, hun! One magical Megaphone, fresh and ready to use!"));
		await dialog.Msg(L("Just type /y followed by whatever you wanna say. Like: /y Party at the fountain, everyone's invited!"));
		await dialog.Msg(L("Now go out there and spread those good vibes! And remember, be nice to each other. Life's too short for drama!"));
	}
}
