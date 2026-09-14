//--- Melia Script ----------------------------------------------------------
// Shadow Syndicate Broker
//--- Description -----------------------------------------------------------
// A shady NPC who acts as a gateway to the Shadow Syndicate faction.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World;
using static Melia.Zone.Scripting.Shortcuts;

public class CSyndicateBrokerNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Broker hidden in Fedimian's back alleys
		// AddNpc(155082, "[Shady Figure]", "SyndicateBroker", "c_fedimian", 169, 367, 0, async dialog =>
		// {
		// 	var player = dialog.Player;
		// 	var factions = ZoneServer.Instance.World.Factions;
		// 
		// 	dialog.SetTitle("Shady Figure");
		// 	dialog.SetPortrait("Dlg_port_assassin_2"); // Using an appropriate portrait
		// 
		// 	// Check reputation with the local city faction
		// 	if (factions.HasTierOrHigher(player, FactionId.Fedimian, ReputationTier.Liked))
		// 	{
		// 		await dialog.Msg("*The figure shrinks back into the shadows, avoiding your gaze.* Move along, citizen. Nothing to see here.");
		// 		return;
		// 	}
		// 
		// 	// Check if player is already a member of the syndicate
		// 	if (factions.HasTierOrHigher(player, FactionId.ShadowSyndicate, ReputationTier.Neutral))
		// 	{
		// 		await SyndicateMemberDialog(dialog);
		// 	}
		// 	else
		// 	{
		// 		await SyndicateIntroductionDialog(dialog);
		// 	}
		// });
	}

	private async Task SyndicateIntroductionDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var factions = ZoneServer.Instance.World.Factions;

		await dialog.Msg("*The figure sizes you up, a glint in their eyes.* You don't look like the goody-two-shoes types that run this city. I can tell you've got... ambition.");

		var response = await dialog.Select("There are circles that operate outside the 'law'. Circles that understand that silver is the only true king. Are you interested in hearing more?",
			Option("I'm listening.", "listen"),
			Option("I'm not interested in trouble.", "decline")
		);

		if (response == "listen")
		{
			await dialog.Msg("We are the Shadow Syndicate. We handle business that the city guard won't touch. Bounties, acquisitions, information... we have a hand in it all. Working with us has its perks, but it means getting your hands dirty.");
			var joinResponse = await dialog.Select("So, what do you say? Want to make a real name for yourself?",
				Option("Sign me up.", "join"),
				Option("I need to think about it.", "decline_2")
			);

			if (joinResponse == "join")
			{
				// Set initial reputation. Neutral with Syndicate, Hated with Fedimian.
				factions.SetReputation(player, FactionId.ShadowSyndicate, 0); // Neutral
				factions.ModifyReputation(player, FactionId.Fedimian, -250); // Becomes Disliked
				await dialog.Msg("*A wry smile crosses the figure's face.* Welcome to the family. Your reputation precedes you now. Don't disappoint us.");
			}
			else
			{
				await dialog.Msg("Smart. A healthy dose of caution will serve you well. Find me when you're ready to stop playing by their rules.");
			}
		}
		else
		{
			await dialog.Msg("A shame. The world belongs to those who are willing to take it. Remember that.");
		}
	}

	private async Task SyndicateMemberDialog(Dialog dialog)
	{
		var player = dialog.Player;
		var factions = ZoneServer.Instance.World.Factions;
		var syndicateRep = factions.GetReputation(player, FactionId.ShadowSyndicate);
		var tier = factions.GetTier(syndicateRep);

		await dialog.Msg($"Welcome back. Your current standing with the Syndicate is {factions.GetTierName(tier)}.");

		// This is where you can expand the NPC's functionality.
		// For example, offer a special shop, repeatable quests, or a way to place bounties anonymously.

		var response = await dialog.Select("What business do you have with the Syndicate today?",
			Option("I need to place a bounty, quietly.", "bounty"),
			Option("Any work available?", "quests", () => tier >= ReputationTier.Liked), // Quests available at 'Liked'
			Option("Nothing right now.", "leave")
		);

		if (response == "bounty")
		{
			await dialog.Msg("Anonymity is our specialty. Tell me the name and the price. The Syndicate will handle the rest.");
			// This could be a parallel way to call the BountyManager, but maybe with a fee, or it doesn't hurt lawful rep.
			// For now, let's just use the same system as an example.
			var targetName = await dialog.Input("Who is the target? (Team Name)");
			var target = ZoneServer.Instance.World.GetCharacterByTeamName(targetName);
			if (target == null)
			{
				await dialog.Msg($"Never heard of 'em. Make sure you've got the right name.");
				return;
			}

			var amountStr = await dialog.Input($"And the price for their head?");
			if (!int.TryParse(amountStr, out var amount) || amount <= 0)
			{
				await dialog.Msg("Don't waste my time with pocket change.");
				return;
			}

			ZoneServer.Instance.World.BountyManager.PlaceBounty(player, target, amount);
			await dialog.Msg("Consider it done. The bounty is posted.");
		}
		else if (response == "quests")
		{
			await dialog.Msg("We always have work for our trusted members. Heists, 'acquisitions'... you know the type. Check back later when I have something for you.");
			// TODO: Hook into a quest system for repeatable Shadow Syndicate quests.
		}
		else
		{
			await dialog.Msg("Don't keep the shadows waiting too long.");
		}
	}
}
