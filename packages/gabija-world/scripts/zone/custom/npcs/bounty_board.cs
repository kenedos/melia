//--- Melia Script ----------------------------------------------------------
// Bounty Boards
//--- Description -----------------------------------------------------------
// Provides a lawful interface for the Bounty Hunter system in major cities.
//---------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Zone;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World;
using static Melia.Zone.Scripting.Shortcuts;

public class CBountyBoardNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Bounty Board in Klaipeda
		// AddNpc(MonsterId.Reputation_Board1, "Bounty Board", "KlaipedaBountyBoard", "c_Klaipe", 86.1, 175.7, 90, async dialog =>
		// {
		// 	await BountyBoardDialog(dialog, FactionId.Klaipeda);
		// });
		// 
		// // Bounty Board in Orsha
		// AddNpc(MonsterId.Reputation_Board1, "Bounty Board", "OrshaBountyBoard", "c_orsha", 278.4, 163.0, 0, async dialog =>
		// {
		// 	await BountyBoardDialog(dialog, FactionId.Orsha);
		// });
		// 
		// // Bounty Board in Fedimian
		// AddNpc(MonsterId.Reputation_Board1, "Bounty Board", "FedimianBountyBoard", "c_fedimian", -252, -457, 180, async dialog =>
		// {
		// 	await BountyBoardDialog(dialog, FactionId.Fedimian);
		// });
	}

	private async Task BountyBoardDialog(Dialog dialog, string localFaction)
	{
		var player = dialog.Player;
		var factions = ZoneServer.Instance.World.Factions;
		var bounties = ZoneServer.Instance.World.BountyManager;

		dialog.SetTitle($"Bounty Board ({factions.GetFactionDisplayName(localFaction)})");
		dialog.SetPortrait("Dlg_port_bulletin_board");

		if (factions.HasTierOrLower(player, localFaction, ReputationTier.Disliked))
		{
			await dialog.Msg("The board is covered in official notices, but a stern-looking guard eyes you with suspicion. You get the feeling you're not welcome here.");
			return;
		}

		var mainSelect = await dialog.Select("The board is plastered with official notices and wanted posters. What would you like to do?",
			Option("View Top Bounties", "view"),
			Option("Place a Bounty", "place"),
			Option("What is this?", "info"),
			Option("Leave", "leave")
		);

		switch (mainSelect)
		{
			case "view":
				var topBounties = bounties.GetTopBounties(5);
				if (!topBounties.Any())
				{
					await dialog.Msg("The board is surprisingly clean. It seems there are no significant bounties active at the moment.");
					return;
				}

				await dialog.Msg("The most wanted individuals are listed here:");
				var bountyText = "";
				foreach (var bounty in topBounties)
				{
					bountyText += $"{bounty.TargetName} - {bounty.TotalBounty:N0} Silver{Environment.NewLine}";
				}
				await dialog.Msg(bountyText);
				break;

			case "place":
				await dialog.Msg("To place a bounty, you must declare the target and the amount you wish to post. The city guard will handle the paperwork... for a small fee, of course.");
				var targetName = await dialog.Input("Enter the Team Name of the person you wish to place a bounty on:");

				if (string.IsNullOrWhiteSpace(targetName))
				{
					await dialog.Msg("You must specify a target.");
					return;
				}

				var target = ZoneServer.Instance.World.GetCharacterByTeamName(targetName);
				if (target == null)
				{
					await dialog.Msg($"There is no one by the name '{targetName}' currently known in this kingdom.");
					return;
				}

				var amountStr = await dialog.Input($"How much silver will you post for the bounty on {target.Name}?");
				if (!int.TryParse(amountStr, out var amount) || amount <= 0)
				{
					await dialog.Msg("That is not a valid amount.");
					return;
				}

				// The PlaceBounty method already contains all the logic for checking minimums, silver, etc.
				bounties.PlaceBounty(player, target, amount);
				break;

			case "info":
				await dialog.Msg($"This board is maintained by the city guard of {factions.GetFactionDisplayName(localFaction)}. We provide a service for citizens to lawfully post bounties on individuals who have wronged them.");
				await dialog.Msg("Once a bounty is claimed, the full amount is awarded to the hunter. We encourage skilled adventurers to help us keep the peace. Your good standing with us may even lead to additional rewards.");
				break;

			case "leave":
				await dialog.Msg("Stay vigilant out there.");
				break;
		}
	}
}
