//--- Melia Script ----------------------------------------------------------
// Laima Starter Map
//--- Description -----------------------------------------------------------
// New character starts here and learns about the game and lore.
//---------------------------------------------------------------------------

using Melia.Zone;
using Melia.Zone.Scripting;
using static Melia.Zone.Scripting.Shortcuts;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Shared.World;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.World.Items;
using Melia.Zone.Events.Arguments;
using System;
using Melia.Shared.Scripting;
using Yggdrasil.Logging;
using Melia.Zone.World;
using Melia.Zone.World.Actors;

public class LaimaStarterMapNpcScript : GeneralScript
{
	protected override void Load()
	{
		// Goddess Laima
		//-------------------------------------------------------------------------
		AddNpc(20079, "[Goddess] Laima", "id_maple_01", -201, -2, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle("Laima");
			dialog.SetPortrait("Dlg_port_Raima");

			await dialog.Msg(L("Welcome, mysterious one. I am Laima, goddess of Fate, and creator of this sanctuary."));

			var spokeToDngels = await dialog.Select(L("Have you spoken with the angels in this sanctuary?"),
				Option(L("Yes, I have."), "yes"),
				Option(L("Not yet."), "no")
			);

			if (spokeToDngels == "no")
			{
				await dialog.Msg(L("I see. It might be beneficial to seek them out. They can provide valuable insights about this sanctuary and our world."));
			}
			else
			{
				await dialog.Msg(L("Excellent. I hope they were able to offer you some clarity about your situation."));
			}

			await dialog.Msg(L("Though I am the goddess of Fate, your future remains a mystery even to me. Your presence here is unprecedented, and your path is yours to choose."));

			while (true)
			{
				var response = await dialog.Select(L("What path do you wish to take?"),
					Option(L("Return to the material world"), "leave"),
					Option(L("Stay in the sanctuary"), "stay")
				);

				switch (response)
				{
					case "stay":
						await dialog.Msg(L("A noble choice. Your help will be most welcome in this important task."));
						await dialog.Msg(L("Souls arrive in the sanctuary sporadically. Sometimes we may see several in a day, other times it might be weeks before a new soul appears. This duty requires patience, but it is an important task to keep souls pure from corruption and evil powers."));
						dialog.Close();
						break;
					case "leave":
						await dialog.Msg(L("I understand your desire to explore the world beyond this sanctuary. While I cannot foresee your future, I believe you have an important role to play."));
						await dialog.Msg(L("Remember, you can always return to this sanctuary if you need guidance. Now, where would you like to begin your journey?"));

						var destination = await dialog.Select(L("Choose your starting point:"),
							Option(L("Klaipeda"), "Klaipe"),
							Option(L("Orsha"), "orsha"),
							Option(L("Fedimian"), "fedimian")
						);

						switch (destination)
						{
							case "Klaipe":
								GiveItem(character, ItemId.Scroll_Warp_Klaipe, 5);
								AddToQuickSlot(character, ItemId.Scroll_Warp_Klaipe, 20);
								character.SetRegionFaction(FactionId.Klaipeda);
								break;
							case "orsha":
								GiveItem(character, ItemId.Scroll_Warp_Orsha, 5);
								AddToQuickSlot(character, ItemId.Scroll_Warp_Orsha, 20);
								character.SetRegionFaction(FactionId.Orsha);
								break;
							case "fedimian":
								GiveItem(character, ItemId.Scroll_Warp_Fedimian, 5);
								AddToQuickSlot(character, ItemId.Scroll_Warp_Fedimian, 20);
								character.SetRegionFaction(FactionId.Fedimian);
								break;
							default:
								Log.Debug("Closing Dialog 2.");
								break;
						}

						WarpAndSaveCity(character, "c_" + destination);
						return;
					default:
						return;
				}
			}
		});

		// Raphaella Angel
		//-------------------------------------------------------------------------
		AddNpc(57579, "[Angel] Raphaella", "id_maple_01", 377, 1181, 0, async dialog =>
		{
			dialog.SetTitle("Raphaella");

			await dialog.Msg(L("Welcome, lost soul. I am Raphaella, an angel in service of Goddess Laima. Your presence here is... unexpected, but not unwelcome."));

			while (true)
			{
				var response = await dialog.Select(L("What would you like to know?"),
					Option(L("Tell me about this sanctuary."), "sanctuary"),
					Option(L("What happened to the goddesses?"), "goddesses"),
					Option(L("Why am I here?"), "player"),
					Option(L("That's all for now."), "end")
				);

				switch (response)
				{
					case "sanctuary":
						await dialog.Msg(L("This is the Maple Leaf Sanctuary, created by Goddess Laima after the great war. It serves as a beacon for lost souls and a refuge for those seeking the missing goddesses."));
						await dialog.Msg(L("The sanctuary's power draws in beings from various worlds, hoping to find clues about the goddesses' whereabouts."));
						break;
					case "goddesses":
						await dialog.Msg(L("A terrible war raged between the goddesses and the demon lords. Both sides suffered greatly, and many lives were lost."));
						await dialog.Msg(L("In the aftermath, several goddesses vanished. We don't know how many survived or where they might be. That's why this sanctuary exists - to find them."));
						break;
					case "player":
						await dialog.Msg(L("You are a unique case. We typically guide lost souls of this world, but you seem to be from another world entirely. Your arrival was unforeseen."));
						await dialog.Msg(L("Perhaps you have a greater purpose here. The sanctuary's power must have drawn you for a reason."));
						break;
					case "end":
						await dialog.Msg(L("At the peak of this sanctuary you will find Goddess Laima herself, please talk to her, she will guide you further on your journey. May the light of the goddesses guide your path."));
						return;
					default:
						return;
				}
			}
		});

		// Seraphina Angel
		//-------------------------------------------------------------------------
		AddNpc(57582, "[Angel] Seraphina", "id_maple_01", 891, 680, 45, async dialog =>
		{
			dialog.SetTitle("Seraphina");

			await dialog.Msg(L("Greetings, traveler from another realm. I am Seraphina, keeper of celestial knowledge."));

			while (true)
			{
				var response = await dialog.Select(L("What wisdom do you seek?"),
					Option(L("Tell me about the world's state."), "world"),
					Option(L("What are demon lords?"), "demons"),
					Option(L("How can I help?"), "help"),
					Option(L("I need time to process this."), "end")
				);

				switch (response)
				{
					case "world":
						await dialog.Msg(L("Our world is in a remarkable state of recovery. Despite the scars left by the war between goddesses and demon lords, humanity has shown incredible resilience."));
						await dialog.Msg(L("The human kingdoms have flourished, building great cities and advancing in knowledge and technology. However, many wild regions are still plagued by monsters and dark energies."));
						break;
					case "demons":
						await dialog.Msg(L("Demon lords are powerful entities of chaos and destruction. They sought to overthrow the goddesses and claim our world for themselves."));
						await dialog.Msg(L("While the war has ended, some demon lords may still lurk in the shadows, waiting to strike again. But fear not, Goddess Laima will be watching over you."));
						break;
					case "help":
						await dialog.Msg(L("I'm not certain of your role here, traveler. Your arrival is shrouded in mystery, even to us angels."));
						await dialog.Msg(L("However, if you seek guidance, I suggest you seek out Goddess Laima herself. You can find her at the highest point of this sanctuary."));
						await dialog.Msg(L("She may have insights into your purpose and how you can aid our world. Be prepared for a journey of self-discovery and great responsibility."));
						break;
					case "end":
						await dialog.Msg(L("Take your time to understand your place in this new world. Remember, even in darkness, hope shines eternal."));
						return;
					default:
						return;
				}
			}
		});

		// Celeste Angel
		//-------------------------------------------------------------------------
		AddNpc(57581, "[Angel] Celeste", "id_maple_01", 648, 363, 90, async dialog =>
		{
			dialog.SetTitle("Celeste");

			await dialog.Msg(L("Welcome to our sanctuary, traveler. I am Celeste, guardian of lost souls. Your presence here is most intriguing."));

			while (true)
			{
				var response = await dialog.Select(L("What would you like to discuss?"),
					Option(L("How many souls have you guided?"), "souls"),
					Option(L("How did you find Goddess Laima?"), "finding_laima"),
					Option(L("What happens to the guided souls?"), "fate"),
					Option(L("I've heard enough for now."), "end")
				);

				switch (response)
				{
					case "souls":
						await dialog.Msg(L("In the time since this sanctuary's creation, we've guided a few hundred souls. Each one is unique, with its own story and purpose."));
						await dialog.Msg(L("It's a small number compared to the vastness of existence, but each soul we help finds its way is a victory against the darkness."));
						break;
					case "finding_laima":
						await dialog.Msg(L("After the great war, Raphaella, Seraphina, and I were lost, much like the souls we now guide. The sanctuary's power drew us here, just as it draws others."));
						await dialog.Msg(L("We found Goddess Laima at the heart of this place. She offered us purpose and hope in the aftermath of the conflict. Now, we serve her by assisting lost souls to find their place in this world or beyond."));
						break;
					case "fate":
						await dialog.Msg(L("The ultimate fate of each soul is chosen by Goddess Laima herself. She sees paths and possibilities that even we angels cannot comprehend."));
						await dialog.Msg(L("Many souls are guided back to the world, finding places where they can be happy and at peace. Some might become adventurers like yourself, while others may live quieter lives."));
						await dialog.Msg(L("Regardless of their path, each soul leaves this sanctuary with a renewed sense of purpose and hope."));
						break;
					case "end":
						await dialog.Msg(L("May you find your own path, traveler, whether it leads you back to the world or onwards to new adventures. The sanctuary's light will always welcome you, should you need guidance."));
						return;
					default:
						return;
				}
			}
		});

		// 10 HP & SP Potions Lv 1 Treasure Chest
		// ------------------------------------------------------------------------
		AddNpc(147392, "Lv1 Treasure Chest", "id_maple_01", 753, 491, 45, async dialog =>
		{
			await OpenChest(dialog.Player, dialog.Npc);
			dialog.Player.Inventory.Add(ItemId.Drug_HP1, 10, InventoryAddType.PickUp);
			dialog.Player.Inventory.Add(ItemId.Drug_SP1, 10, InventoryAddType.PickUp);
		});

		// Trigger Tutorial Messages on Area Entry
		//-------------------------------------------------------------------------
		AddAreaTrigger("id_maple_01", 29, 1140, 100, async triggerFunc =>
		{
			if (triggerFunc.Initiator is not Character character)
				return;
			if (!character.Variables.Temp.ActivateOnce("Laima.Tutorial.Chatting"))
				return;
			await Task.Delay(200); // Slight delay for the message to appear
			character.StartDialog(triggerFunc.Npc, async dialogFunc =>
			{
				character.ShowHelp("TUTO_CHATTING", true);
			});
		});
		AddAreaTrigger("id_maple_01", 853, 1020, 100, async triggerFunc =>
		{
			if (triggerFunc.Initiator is not Character character)
				return;
			if (!character.Variables.Temp.ActivateOnce("Laima.Tutorial.Skill"))
				return;
			await Task.Delay(200); // Slight delay for the message to appear
			character.StartDialog(triggerFunc.Npc, async dialogFunc =>
			{
				character.ShowHelp("TUTO_SKILL", true);
			});
		});
		AddAreaTrigger("id_maple_01", 367, 260, 100, async triggerFunc =>
		{
			if (triggerFunc.Initiator is not Character character)
				return;
			if (!character.Variables.Temp.ActivateOnce("Laima.Tutorial.Inventory"))
				return;
			await Task.Delay(200); // Slight delay for the message to appear
			character.StartDialog(triggerFunc.Npc, async dialogFunc =>
			{
				character.ShowHelp("TUTO_INVEN", true);
			});
		});
	}

	// Helper function to get an item
	private static void GiveItem(Character character, int itemId, int amount)
	{
		if (character.Inventory.HasItem(itemId))
			return;

		var item = new Item(itemId, amount);
		character.Inventory.AddSilent(item);
	}

	// Helper function to add item to quick slot
	private static void AddToQuickSlot(Character character, int itemId, int slot)
	{
		Send.ZC_QUICKSLOT_REGISTER(character.Connection, QuickSlotType.Item, itemId, slot);
	}

	// Helper function to warp the player to a city
	private void WarpAndSaveCity(Character character, string mapName)
	{
		if (ZoneServer.Instance.Data.MapDb.TryFind(mapName, out var cityMap))
		{
			var warpPosition = cityMap.DefaultPosition;

			switch (cityMap.ClassName)
			{
				case "c_Klaipe":
					warpPosition = new Position(-161, 149, 54);
					break;

				case "c_orsha":
					warpPosition = new Position(157, 176, 268);
					break;

				case "c_fedimian":
					warpPosition = new Position(-277, 162, -281);
					break;
			}

			var location = new Location(cityMap.Id, warpPosition);
			character.SetCityReturnLocation(location);
			character.Warp(cityMap.Id, warpPosition);
		}
		else
			character.WorldMessage(L("I'm sorry, I can't take you there right now."));
	}

	[On("PlayerReady")]
	public void OnPlayerReady(object sender, PlayerEventArgs args)
	{
		var character = args.Character;

		if (!character.Variables.Perm.ActivateOnce("Laima.Tutorial.Movement"))
		{
			return;
		}

		// Show movement tutorial after a brief delay
		Task.Delay(1000).ContinueWith(_ =>
		{
			if (character == null)
				return;

			character.ShowHelp("TUTO_MOVE_KB", true);

			AddToQuickSlot(character, ItemId.Drug_HP1, 11);
			AddToQuickSlot(character, ItemId.Drug_SP1, 12);
			AddToQuickSlot(character, ItemId.Drug_STA1_Q, 13);
		});
	}
}
