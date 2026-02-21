# NPC Scripting Guide
=============================================================================

## Table of Contents

1. [Introduction](#introduction)
2. [Quick Start](#quick-start)
3. [Script Structure](#script-structure)
4. [Creating NPCs](#creating-npcs)
5. [Dialog System](#dialog-system)
6. [Dialog Methods Reference](#dialog-methods-reference)
7. [Text Formatting](#text-formatting)
8. [Advanced Features](#advanced-features)
9. [Complete Examples](#complete-examples)
10. [Best Practices](#best-practices)

## Introduction

The Melia/Laima NPC scripting system allows you to create interactive NPCs with dialogs, shops, quests, and custom functionality. Scripts are written in C# and compiled at runtime, providing full access to the server API while maintaining type safety.

### Key Features:
- Asynchronous dialog system
- Multiple dialog interaction types (messages, selections, input, yes/no)
- Shop integration (both database and custom shops)
- Quest interaction and progress tracking
- Storage access (personal and team)
- Custom script functions
- Hot-reloading without server restart

## Quick Start

### Minimal NPC Example

```csharp
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;

public class SimpleNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Create an NPC at coordinates (100, 200) facing east (90 degrees)
		AddNpc(10011, "Friendly Villager", "f_siauliai_west", 100, 200, 90, async dialog =>
		{
			await dialog.Msg("Hello, traveler! Welcome to our village!");
		});
	}
}
```

## Script Structure

All NPC scripts inherit from `GeneralScript` and override the `Load()` method:

```csharp
using System;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Scripting.Shortcuts;

public class MyNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Your NPC creation code here
	}
}
```

### Required Namespaces:
- `Melia.Zone.Scripting` - Core scripting classes
- `Melia.Zone.Scripting.Dialogues` - Dialog system
- `static Melia.Zone.Scripting.Shortcuts` - Helper functions

## Creating NPCs

### Basic AddNpc Syntax

```csharp
AddNpc(monsterId, name, map, x, z, direction, dialogFunc);
```

**Parameters:**
- `monsterId` (int): The monster/NPC model ID
- `name` (string): Display name for the NPC
- `map` (string): Map class name (e.g., "f_siauliai_west")
- `x` (double): X coordinate
- `z` (double): Z coordinate
- `direction` (double): Direction in degrees (0-360, East=90, South=180, West=270, North=0)
- `dialogFunc` (DialogFunc): Async lambda function for dialog

### NPC with Dialog

```csharp
AddNpc(10011, "Merchant Karl", "f_siauliai_west", 100, 200, 90, async dialog =>
{
	var player = dialog.Player;

	dialog.SetTitle("Merchant Karl");
	dialog.SetPortrait("Dlg_port_merchant");

	await dialog.Msg("Hello! What can I do for you?");

	var response = await dialog.Select("What do you need?",
		Option("Buy items", "shop"),
		Option("Talk", "talk"),
		Option("Nothing", "nothing")
	);

	if (response == "shop")
	{
		await dialog.OpenShop("MerchantKarl");
	}
	else if (response == "talk")
	{
		await dialog.Msg("I've been trading here for 20 years!");
	}
});
```

### NPC with Unique Name

For tracking NPC-specific variables or preventing duplicates:

```csharp
AddNpc(10011, "Quest Giver", "unique_quest_npc", "f_siauliai_west", 100, 200, 90, async dialog =>
{
	var npc = dialog.Npc;

	// Use NPC-specific variables
	if (!npc.Vars.Has("QuestGiven"))
	{
		await dialog.Msg("I have a quest for you!");
		npc.Vars.Set("QuestGiven", true);
	}
	else
	{
		await dialog.Msg("Still working on that quest?");
	}
});
```

### Common Monster IDs

Some commonly used NPC model IDs:
- `10011` - Male villager
- `10012` - Female villager
- `10102` - Guard
- `10103` - Merchant
- `10104` - Priest
- `10301` - Old man
- `10302` - Old woman

## Dialog System

The dialog system is fully asynchronous and provides various interaction methods.

### Dialog Context

Within a dialog function, you have access to:

```csharp
async dialog =>
{
	var player = dialog.Player;    // The character talking to the NPC
	var npc = dialog.Npc;          // The NPC being talked to

	// Your dialog logic here
}
```

### Setting Title and Portrait

```csharp
dialog.SetTitle("Custom NPC Name");       // Override displayed name
dialog.SetPortrait("Dlg_port_merchant");  // Set portrait image
```

**Common Portraits:**
- `Dlg_port_merchant` - Merchant
- `Dlg_port_priest` - Priest
- `Dlg_port_guard` - Guard
- `Dlg_port_noble` - Noble
- See client data for full list

## Dialog Methods Reference

### 1. Msg - Display Messages

```csharp
await dialog.Msg("Simple message");
await dialog.Msg("Message with {pcname}");  // Player name replacement
await dialog.Msg("Formatted {0} message", variable);
```

### 2. Select - Multiple Choice Menu

```csharp
var response = await dialog.Select("Choose an option:",
	Option("First choice", "choice1"),
	Option("Second choice", "choice2"),
	Option("Third choice", "choice3")
);

if (response == "choice1")
{
	// Handle first choice
}
```

**Conditional Options:**

```csharp
var response = await dialog.Select("What would you like?",
	Option("Buy", "buy"),
	Option("Sell", "sell", () => player.Inventory.CountItem(ItemId.SomeItem) > 0)
);
```

### 3. YesNo - Binary Choice

```csharp
if (await dialog.YesNo("Do you want to continue?"))
{
	await dialog.Msg("Great! Let's proceed.");
}
else
{
	await dialog.Msg("Maybe next time.");
}
```

### 4. Input - Text Input

```csharp
var playerName = await dialog.Input("What's your name?");
await dialog.Msg($"Nice to meet you, {playerName}!");
```

**Validating Input:**

```csharp
var input = await dialog.Input("Enter an amount:");

if (int.TryParse(input, out var amount) && amount > 0)
{
	await dialog.Msg($"You entered: {amount}");
}
else
{
	await dialog.Msg("Invalid amount!");
}
```

### 5. OpenShop - Display Shop

```csharp
// Open a database shop
await dialog.OpenShop("MISC_Klaipeda");

// Open a custom shop (created via CreateShop)
await dialog.OpenShop("CustomShopName");
```

### 6. OpenPersonalStorage / OpenTeamStorage

```csharp
await dialog.OpenPersonalStorage();
// or
await dialog.OpenTeamStorage();
```

### 7. TimeAction - Progress Bar Action

```csharp
var result = await dialog.TimeAction(
	"Crafting item...",
	"Craft",
	"workbench_craft",
	TimeSpan.FromSeconds(5)
);

if (result == TimeActionResult.Okay)
{
	// Action completed successfully
}
else
{
	// Action was cancelled
}
```

### 8. SaveLocation - Set Respawn Point

```csharp
await dialog.SaveLocation();
await dialog.Msg("Your respawn point has been saved here.");
```

## Text Formatting

### Custom Codes

```csharp
"{pcname}"    // Player character name
"{teamname}"  // Player team name
"{fullname}"  // Character + team name
"{nl}"        // New line
"{np}"        // New paragraph (continue on new page)
```

### Client Text Codes

See [text_codes.md](text_codes.md) for complete reference:

```csharp
await dialog.Msg("{s35}Large text{/}");          // Size
await dialog.Msg("{#FF0000}Red text{/}");        // Color
await dialog.Msg("{ol}Outlined text{/}");        // Outline
await dialog.Msg("{img icon_item_silver 20 20}"); // Image
```

### Example with Formatting

```csharp
await dialog.Msg(
	"{s35}Welcome!{/}{nl}" +
	"{#FF0000}Important:{/} This quest is difficult!{nl}" +
	"Good luck, {pcname}!"
);
```

## Advanced Features

### 1. Creating Custom Shops

```csharp
protected override void Load()
{
	CreateShop("MyCustomShop", shop =>
	{
		shop.AddProduct(1, "Potion", 1, 100);     // ItemId, Amount, Price
		shop.AddProduct(2, "Hi-Potion", 1, 500);
		shop.AddProduct(3, "Ether", 1, 300);
	});

	AddNpc(10103, "Shop Owner", "f_siauliai_west", 100, 200, 90, async dialog =>
	{
		await dialog.OpenShop("MyCustomShop");
	});
}
```

### 2. Faction/Reputation Checks

```csharp
var factionId = "Klaipeda";
var reputation = ZoneServer.Instance.World.Factions.GetReputation(player, factionId);
var tier = ZoneServer.Instance.World.Factions.GetTier(reputation);

if (tier >= ReputationTier.Honored)
{
	await dialog.Msg("Welcome, honored friend!");
}
else
{
	await dialog.Msg("Prove yourself to our city first.");
}
```

### 3. Quest Integration

```csharp
var questId = new QuestId("MyNamespace", 1001);

if (!player.Quests.Has(questId))
{
	// Offer quest
	if (await dialog.YesNo("I have a task for you. Will you help?"))
	{
		player.Quests.Start(questId);
	}
}
else if (player.Quests.IsActive(questId))
{
	// Check progress
	await dialog.Msg("How's the quest going?");
}
else if (player.Quests.HasCompleted(questId))
{
	await dialog.Msg("Thank you for your help!");
}
```

### 4. Inventory Checks

```csharp
var requiredItemId = 640001;  // Apple
var requiredAmount = 5;

if (player.Inventory.CountItem(requiredItemId) >= requiredAmount)
{
	if (await dialog.YesNo($"You have {requiredAmount} apples. Trade them?"))
	{
		player.Inventory.Remove(requiredItemId, requiredAmount, InventoryItemRemoveMsg.Given);
		player.Inventory.Add(ItemId.Silver, 1000, InventoryAddType.PickUp);
		await dialog.Msg("Thanks! Here's your reward.");
	}
}
else
{
	await dialog.Msg($"Bring me {requiredAmount} apples.");
}
```

### 5. Persistent Variables

```csharp
// Per-character permanent variables
if (!player.Variables.Perm.Has("MetThisNpc"))
{
	await dialog.Msg("I don't think we've met before.");
	player.Variables.Perm.Set("MetThisNpc", true);
}
else
{
	await dialog.Msg("Good to see you again!");
}

// Temporary variables (cleared on logout)
player.Variables.Temp.Set("LastInteraction", DateTime.Now);
```

### 6. Warping Players

```csharp
if (await dialog.YesNo("Would you like to warp to the city?"))
{
	player.Warp("f_siauliai_west", 100, 200);
}
```

### 7. Effects and Animations

```csharp
// Play animation
dialog.PlayAnimation("Stand_Idle");

// Chat bubble
dialog.Chat("Look over here!");

// Show help popup
dialog.ShowHelp("tutorial_combat");
```

### 8. Area Triggers

Create invisible area triggers for cutscenes or events:

```csharp
AddAreaTrigger("f_siauliai_west", 500, 600, 50, async trigger =>
{
	var character = trigger.Initiator as Character;
	if (character == null) return;

	if (!character.Variables.Temp.Has("TriggeredEvent1"))
	{
		character.Variables.Temp.Set("TriggeredEvent1", true);
		character.ServerMessage("You discovered a hidden area!");
	}
});
```

## Complete Examples

### Example 1: Simple Merchant

```csharp
public class SimpleMerchantScript : GeneralScript
{
	protected override void Load()
	{
		CreateShop("SimpleShop", shop =>
		{
			shop.AddProduct(1, "Potion", 1, 50);
			shop.AddProduct(2, "Hi-Potion", 1, 200);
		});

		AddNpc(10103, "Merchant Tom", "f_siauliai_west", 100, 200, 90, async dialog =>
		{
			dialog.SetTitle("Merchant Tom");

			await dialog.Msg("Welcome to my shop!");

			var choice = await dialog.Select("What can I help you with?",
				Option("Buy items", "buy"),
				Option("Just browsing", "browse")
			);

			if (choice == "buy")
			{
				await dialog.OpenShop("SimpleShop");
			}
		});
	}
}
```

### Example 2: Quest NPC

```csharp
public class QuestNpcScript : GeneralScript
{
	private QuestId COLLECT_QUEST = new QuestId("Tutorial", 1);

	protected override void Load()
	{
		AddNpc(10011, "Elder John", "f_siauliai_west", 100, 200, 90, async dialog =>
		{
			var player = dialog.Player;

			dialog.SetTitle("Elder John");

			if (!player.Quests.Has(COLLECT_QUEST))
			{
				await dialog.Msg("Greetings, young adventurer!");

				if (await dialog.YesNo("We need help collecting herbs. Will you assist?"))
				{
					player.Quests.Start(COLLECT_QUEST);
					await dialog.Msg("Thank you! Bring me 10 herbs.");
				}
			}
			else if (player.Quests.IsActive(COLLECT_QUEST))
			{
				await dialog.Msg("Still looking for those herbs?");
			}
			else
			{
				await dialog.Msg("Thank you for your help earlier!");
			}
		});
	}
}
```

### Example 3: Interactive Teleporter

```csharp
public class TeleporterScript : GeneralScript
{
	protected override void Load()
	{
		AddNpc(40243, "Teleporter Crystal", "f_siauliai_west", 100, 200, 90, async dialog =>
		{
			var player = dialog.Player;

			await dialog.Msg("Where would you like to go?");

			var destination = await dialog.Select("Select destination:",
				Option("Klaipeda", "klaipeda"),
				Option("Orsha", "orsha"),
				Option("Fedimian", "fedimian"),
				Option("Cancel", "cancel")
			);

			switch (destination)
			{
				case "klaipeda":
					player.Warp("c_Klaipe", -42, 241);
					break;
				case "orsha":
					player.Warp("c_orsha", -213, 202);
					break;
				case "fedimian":
					player.Warp("c_fedimian", 19, -68);
					break;
			}
		});
	}
}
```

### Example 4: Advanced Guild NPC

```csharp
public class GuildNpcScript : GeneralScript
{
	protected override void Load()
	{
		AddNpc(10011, "[Guild Master]{nl}Marcus", "f_siauliai_west", 100, 200, 90, async dialog =>
		{
			var player = dialog.Player;

			dialog.SetTitle("Guild Master Marcus");

			var rank = player.Variables.Perm.Get("GuildRank", "None");

			if (rank == "None")
			{
				await dialog.Msg("Welcome! You're not a guild member yet.");

				if (await dialog.YesNo("Would you like to join our guild?"))
				{
					if (player.Silver >= 10000)
					{
						player.Silver -= 10000;
						player.Variables.Perm.Set("GuildRank", "Recruit");
						await dialog.Msg("Welcome to the guild!");
					}
					else
					{
						await dialog.Msg("You need 10,000 silver to join.");
					}
				}
			}
			else
			{
				await dialog.Msg($"Welcome back! Your rank: {rank}");

				var choice = await dialog.Select("What would you like?",
					Option("Guild Shop", "shop", () => rank != "Recruit"),
					Option("Storage", "storage"),
					Option("Rank Up", "rankup", () => rank == "Recruit"),
					Option("Leave", "leave")
				);

				switch (choice)
				{
					case "shop":
						await dialog.OpenShop("GuildShop");
						break;
					case "storage":
						await dialog.OpenTeamStorage();
						break;
					case "rankup":
						if (player.Variables.Perm.Get("GuildContribution", 0) >= 100)
						{
							player.Variables.Perm.Set("GuildRank", "Member");
							await dialog.Msg("Congratulations! You're now a full member!");
						}
						else
						{
							await dialog.Msg("You need 100 contribution points.");
						}
						break;
				}
			}
		});
	}
}
```

## Best Practices

### 1. Always Use await

All dialog methods are asynchronous:

```csharp
// CORRECT
await dialog.Msg("Hello");

// WRONG - Will not work
dialog.Msg("Hello");
```

### 2. Handle Invalid Input

```csharp
var input = await dialog.Input("Enter a number:");

if (!int.TryParse(input, out var number))
{
	await dialog.Msg("That's not a valid number!");
	return;
}
```

### 3. Use Variables for Tracking

```csharp
// Use NPC variables for NPC-specific state
if (npc.Vars.ActivateOnce("TreasureOpened"))
{
	// Give reward
}

// Use player permanent variables for persistent state
player.Variables.Perm.Set("CompletedTutorial", true);

// Use player temporary variables for session state
player.Variables.Temp.Set("InCutscene", true);
```

### 4. Validate Before Transactions

```csharp
if (player.Silver < cost)
{
	await dialog.Msg("You don't have enough silver.");
	return;
}

if (!player.Inventory.HasSpace)
{
	await dialog.Msg("Your inventory is full!");
	return;
}

player.Silver -= cost;
player.Inventory.Add(itemId, amount, InventoryAddType.PickUp);
```

### 5. Use Constants for IDs

```csharp
private const int APPLE_ID = 640001;
private const int QUEST_NPC_ID = 10011;
private static readonly QuestId MY_QUEST = new QuestId("MyMod", 1);
```

### 6. Structure Complex Dialogs

```csharp
AddNpc(10011, "Complex NPC", "f_siauliai_west", 100, 200, 90, async dialog =>
{
	await MainMenu(dialog);
});

private async Task MainMenu(Dialog dialog)
{
	var choice = await dialog.Select("Main menu:",
		Option("Shop", "shop"),
		Option("Quests", "quests"),
		Option("Info", "info")
	);

	switch (choice)
	{
		case "shop": await ShopMenu(dialog); break;
		case "quests": await QuestMenu(dialog); break;
		case "info": await InfoMenu(dialog); break;
	}
}

private async Task ShopMenu(Dialog dialog)
{
	await dialog.OpenShop("MyShop");
}
```

### 7. Tab Indentation (Windows Requirement)

**CRITICAL:** All script files MUST use tab indentation, not spaces:

```csharp
// CORRECT
public class MyScript : GeneralScript
{
→protected override void Load()
→{
→→AddNpc(...);
→}
}

// WRONG - Will cause issues
public class MyScript : GeneralScript
{
    protected override void Load()
    {
        AddNpc(...);
    }
}
```

### 8. Test Incrementally

- Start with a simple message
- Add one feature at a time
- Test after each addition
- Use console commands to reload scripts

### 9. Error Handling

```csharp
try
{
	var amount = int.Parse(await dialog.Input("Amount:"));
	// Process amount
}
catch (Exception ex)
{
	Log.Warning($"Error in NPC dialog: {ex.Message}");
	await dialog.Msg("An error occurred. Please try again.");
}
```

### 10. Localization Support

```csharp
// Use localization keys when available
await dialog.Msg("ETC_20150317_000001");  // Client dictionary key

// Or use custom localization
dialog.PlayerLocalization(out var L, out var LN);
await dialog.Msg(L("greeting_message"));
```

---

## Additional Resources

- [Quest Scripting Guide](quest_system.md)
- [Text Codes Reference](text_codes.md)
- [Scriptable Functions](scriptable_functions.md)
- Source code examples in `system/scripts/zone/`

For questions or issues, check the project repository or community forums.
