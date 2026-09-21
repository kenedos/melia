//--- Melia Script ----------------------------------------------------------
// Great Cathedral Sanctuary Quest NPCs
//--- Description -----------------------------------------------------------
// The last two of Maven's keys, the demons who are tricked into spending the
// trap that guards one of them, and the door to Pasala Altar.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.Dialogues;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DCathedral56QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(20329);
	private readonly static QuestId Mq02_1 = new QuestId(20342);
	private readonly static QuestId Mq02 = new QuestId(20330);
	private readonly static QuestId Mq03 = new QuestId(20331);
	private readonly static QuestId Mq04 = new QuestId(20332);
	private readonly static QuestId Mq05 = new QuestId(20333);
	private readonly static QuestId Mq06 = new QuestId(20334);
	private readonly static QuestId Mq07 = new QuestId(20335);
	private readonly static QuestId Mq08 = new QuestId(20336);
	private readonly static QuestId Sq01 = new QuestId(20337);
	private readonly static QuestId Sq02 = new QuestId(20338);
	private readonly static QuestId Sq03 = new QuestId(20339);
	private readonly static QuestId Sq04 = new QuestId(50002);
	private readonly static QuestId Mq05Part3 = new QuestId(20340);

	private const int DocumentsNeeded = 5;
	private const int CursedOrbsNeeded = 10;
	private const int CursedOrbsPerPickup = 2;

	private const string DocumentVar = "Gabija.Cathedral56.Document";
	private const string LureVar = "Gabija.Cathedral56.Lure";
	private const string CandleVar = "Gabija.Cathedral56.Candle";
	private const string CursedOrbVar = "Gabija.Cathedral56.CursedOrb";

	private readonly static string[] KeyOrbs = { "Red", "Blue", "Yellow", "Green", "Purple" };

	protected override void Load()
	{
		// Bishop Aurelius' Spirit, at the Sanctuary entrance
		//-------------------------------------------------------------------------
		AddConditionalNpc(151033, L("Bishop Aurelius' Spirit"), "CHATHEDRAL56_MQ_BISHOP", "d_cathedral_56", 2149.98, 583.74, 53, c => !c.Quests.HasCompleted(Mq03), this.SanctuaryBishop);

		// Bishop Aurelius' Spirit, called up at Pasala Altar
		//-------------------------------------------------------------------------
		// The client summons him from the Spirit's Scripture wherever the player
		// stands; the port stands him on the quest's own Pasala Altar marker.
		AddConditionalNpc(151033, L("Bishop Aurelius' Spirit"), "CHATHEDRAL56_BISHOP", "d_cathedral_56", -1555, 736, 90, c => c.Quests.HasCompleted(Mq03) && !c.Quests.HasCompleted(Mq08), this.PasalaBishop);

		// Bishop Aurelius' Spirit, once the portal is open
		//-------------------------------------------------------------------------
		AddConditionalNpc(151033, L("Bishop Aurelius' Spirit"), "CHATHEDRAL56_MQ_BISHOP_AFTER", "d_cathedral_56", -1444.33, 759.75, 3, c => c.Quests.HasCompleted(Mq08) && !c.Quests.HasCompleted(Mq05Part3), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Bishop Aurelius' Spirit"));
			dialog.SetPortrait("Dlg_port_aurelius");

			if (!character.Quests.Has(Mq05Part3) && character.Quests.MeetsPrerequisites(Mq05Part3))
			{
				var answer = await dialog.SelectQuestOffer(Mq05Part3, L("Now all that's left is the Verification Test. The scripture that possesses my spirit, please open it in front of the altar of the revelation."),
					Option(L("Go to test the last verification"), "accept"),
					Option(L("Take some time to prepare"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq05Part3);
					await dialog.Msg(L("My guidance ends here. It has been very fortunate that I was able to guide you well."));
					await dialog.Msg(L("I am going back to the goddesses. Please, save the kingdom and the goddesses."));
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq05Part3))
			{
				await dialog.Msg(L("The portal at Pasala Altar leads to the room with the revelation. Maven's last test is inside it."));
				return;
			}

			await dialog.Msg(L("Farewell is short, but the relationship lasts long."));
			await dialog.Msg(L("Someday we will meet again by the goddess."));
		});

		// The papers and documents of the Sanctuary
		//-------------------------------------------------------------------------
		this.AddDemonDocument(1, 147312, L("Torn Page"), 1737.57, 726.46, 58);
		this.AddDemonDocument(2, 147312, L("Torn Page"), 1864.35, 116.64, 33);
		this.AddDemonDocument(3, 147312, L("Torn Page"), 1647.18, 323.89, 354);
		this.AddDemonDocument(4, 147311, L("Old Document"), 1941.21, 419.37, 90);
		this.AddDemonDocument(5, 147311, L("Old Document"), 2140.40, 104.78, 155);

		// Altar of Intelligence
		//-------------------------------------------------------------------------
		// The cutscene spawns its own copy for the defence, so this one stands
		// aside while the scroll is being written.
		AddConditionalNpc(151024, L("Altar of Intelligence"), "CHATHEDRAL56_MQ02", "d_cathedral_56", 1414.54, -471.55, 90, c => !c.Quests.IsActive(Mq02) || c.Quests.IsCompletable(Mq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Altar of Intelligence"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				var recovered = await character.TimeActions.StartAsync(L("Recovering the mixture..."), L("Cancel"), "SITGROPESET2", TimeSpan.FromSeconds(2));

				if (recovered != TimeActionResult.Completed)
					return;

				await dialog.Msg(L("The Demon Transformation Scroll lifts off the altar, finished."));
				await dialog.CompleteQuest(Mq02);
				character.LookAround();
				return;
			}

			await dialog.Msg(L("The Altar of Intelligence of Laukimas Antenave."));
		});

		// The scroll is written while the demons are held off.
		AddQuestTrigger("CHATHEDRAL56_MQ02_ARRIVE", "d_cathedral_56", 1458.91, -472.21, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
				character.Quests.StartQuestTrack(Mq02);

			await Task.CompletedTask;
		});

		// The three places the transformed Revelator is seen
		//-------------------------------------------------------------------------
		this.AddLurePoint(1, 331, 150);
		this.AddLurePoint(2, -600, -92);
		this.AddLurePoint(3, 929, 932);

		// Apgaule Altar
		//-------------------------------------------------------------------------
		// The cutscene spawns its own copy and the trap destroys it, so this
		// one stands aside for the length of the fight.
		AddConditionalNpc(153018, L("Apgaule Altar"), "CHATHEDRAL56_MQ04", "d_cathedral_56", -1014.99, 254.27, 0, c => !c.Quests.IsActive(Mq04), async dialog =>
		{
			await dialog.Msg(L("Apgaule Altar, and a socket in it that once held a key."));
		});

		// The trap spends itself when the demons reach the altar.
		AddQuestTrigger("CHATHEDRAL56_MQ04_ARRIVE", "d_cathedral_56", -1022.92, 129.19, 250, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq04) && !character.Quests.IsCompletable(Mq04))
				character.Quests.StartQuestTrack(Mq04);

			await Task.CompletedTask;
		});

		// Secret Statue of the Small Reception Room
		//-------------------------------------------------------------------------
		AddNpc(153017, L("Secret Statue"), "CHATHEDRAL56_MQ05_PUZZLE", "d_cathedral_56", -222.93, -1280.34, 135, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Secret Statue"));

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				dialog.SetTitle(L("Maven's Message"));

				await dialog.Msg(L("I am glad that my life's masterpiece has protected the revelation well."));
				await dialog.Msg(L("But, yours and my mission is not over yet. Savior, please open the door to Pasala Altar."));
				await dialog.Msg(L("If you really wish to obtain the revelation of the goddess, you will receive it at the end..."));
				await dialog.CompleteQuest(Mq05);
				character.ServerMessage(L("Acquired Maven's key!"));
				character.LookAround();
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("Four candlesticks stand around the statue, and the order matters as much as the number."));

				var lit = await character.TimeActions.StartAsync(L("Lighting the candles in order..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(3));

				if (lit != TimeActionResult.Completed)
					return;

				character.Quests.CompleteObjective(Mq05, "solveSecret");
				character.ServerMessage(L("The candles burn in Maven's order and the statue turns."));
				return;
			}

			await dialog.Msg(L("A statue with a secret behind it, and it is not telling."));
		});

		// The last secret is staged when the Small Reception Room is reached.
		AddQuestTrigger("CHATHEDRAL56_MQ05_ARRIVE", "d_cathedral_56", -216.44, -1202.18, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
				character.Quests.StartQuestTrack(Mq05);

			await Task.CompletedTask;
		});

		// The barrier candlesticks of the Tikinciuju Gallery
		//-------------------------------------------------------------------------
		this.AddBarrierCandle(1, -2093.69, -609.01);
		this.AddBarrierCandle(2, -2096.10, -372.86);

		// The five orbs of Pasala Altar
		//-------------------------------------------------------------------------
		this.AddKeyOrb(0, L("Red Orb"), L("It's emitting red light."), -1672.60, 835.23, 50);
		this.AddKeyOrb(1, L("Blue Orb"), L("It's emitting blue light."), -1673.53, 622.20, 46);
		this.AddKeyOrb(2, L("Yellow Orb"), L("It's emitting yellow light."), -1436.67, 854.26, 62);
		this.AddKeyOrb(3, L("Green Orb"), L("It's emitting green light."), -1378.55, 693.38, 48);
		this.AddKeyOrb(4, L("Purple Orb"), L("It's emitting purple light."), -1496.29, 567.74, 44);

		// The portal into the hidden room
		//-------------------------------------------------------------------------
		AddConditionalNpc(147469, L("Portal"), "CHATHEDRAL56_MQ08_POTAL", "d_cathedral_56", -1544.09, 729.35, 90, c => c.Quests.HasCompleted(Mq08), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Portal"));

			await dialog.Msg(L("The portal opens onto the Grand Corridor, at the far end where the altar of the revelation stands."));

			character.Warp("d_cathedral_54", 1542.78, 0.19, -2296.36);
		});

		// The door to Pasala Altar
		//-------------------------------------------------------------------------
		AddNpc(153012, L("Pasala Altar Door"), "CHATHEDRAL56_SQ01", "d_cathedral_56", -1527.70, 469.50, 359, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Pasala Altar Door"));

			if (character.Quests.IsActive(Mq06) && !character.Quests.IsCompletable(Mq06))
			{
				character.Quests.CompleteObjective(Mq06, "reachPasala");
				await dialog.Msg(L("The door to Pasala Altar stands shut in front of you."));
				return;
			}

			if (!character.Quests.Has(Sq01) && character.Quests.MeetsPrerequisites(Sq01))
			{
				var answer = await dialog.SelectQuestOffer(Sq01, L("The door to Pasala Altar is shut, and something behind it is very close."),
					Option(L("Open the door"), "accept"),
					Option(L("Leave it shut"), "leave")
				);

				if (answer != "accept")
					return;

				var opened = await character.TimeActions.StartAsync(L("Opening the door..."), L("Cancel"), "", TimeSpan.FromSeconds(2));

				if (opened != TimeActionResult.Completed)
					return;

				character.Quests.Start(Sq01);
				character.ServerMessage(L("The angry Naktis suddenly attacked."));
				return;
			}

			if (character.Quests.IsActive(Sq01))
			{
				await dialog.Msg(L("Naktis is between you and Pasala Altar."));
				character.Quests.ReplayQuestTrack(Sq01);
				return;
			}

			await dialog.Msg(L("The door to Pasala Altar."));
		});

		// Priest Prosit
		//-------------------------------------------------------------------------
		AddNpc(147398, L("Priest Prosit"), "CHATHEDRAL56SQ04_NPC", "d_cathedral_56", -706.38, -749.26, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Prosit"));

			if (character.Quests.IsActive(Sq02) && character.Quests.IsCompletable(Sq02))
			{
				await dialog.Msg(L("It is made by an interesting principle. Anyways, thanks for your cooperation."));
				await dialog.Msg(L("Ah, if you get a chance to go back to the Main Chamber, could you pass this document to Priest Aden?"));
				await dialog.CompleteQuest(Sq02);
				return;
			}

			if (character.Quests.IsActive(Sq04) && character.Quests.IsCompletable(Sq04))
			{
				await dialog.Msg(L("There are many who trample on this sacred place."));
				await dialog.Msg(L("I can't forgive them."));
				await dialog.CompleteQuest(Sq04);
				character.StartBuff(BuffId.CHATHEDRAL56_SQ04_HEAL, 1, 1, TimeSpan.FromMinutes(5), character);
				return;
			}

			if (!character.Quests.Has(Sq02) && character.Quests.MeetsPrerequisites(Sq02))
			{
				var answer = await dialog.SelectQuestOffer(Sq02, L("There are these orbs that keep spreading the curses. It's ineffective to destroy it. We can only stop the spreading of the curse through a holy ritual ceremony. If you find an orb, please bring it to me."),
					Option(L("I will bring it if you see it"), "accept"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "accept")
				{
					for (var i = 1; i <= 6; ++i)
						character.Variables.Perm.Set(CursedOrbVar + i, false);

					character.Quests.Start(Sq02);
					await dialog.Msg(L("They are set all over the west of the Sanctuary. Ten of them would be a good start."));
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Sq04) && character.Quests.MeetsPrerequisites(Sq04))
			{
				var answer = await dialog.SelectQuestOffer(Sq04, L("There once were always holy prayers echoing in this Great Cathedral. But, after Naktis' arrival, I've only seen unfaithful behavior. My mission is to retrieve the relics, but I can't stand and watch this anymore."),
					Option(L("I will defeat the monsters"), "accept"),
					Option(L("Decline"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq04);
					await dialog.Msg(L("Can you defeat the monsters that are roaming around the Great Cathedral?"));
				}
				return;
			}

			if (character.Quests.IsActive(Sq02))
			{
				await dialog.Msg(L("It seems that they are spreading Naktis' thoughts..."));
				await dialog.Msg(L("If we investigate it, we may be able to make use of this information."));
				return;
			}

			if (character.Quests.IsActive(Sq04))
			{
				await dialog.Msg(L("Everyone is angry, but they are hold it in with their objectives in mind."));
				await dialog.Msg(L("But I.. just can't stand it..."));
				return;
			}

			await dialog.Msg(L("There once were always holy prayers echoing in this Great Cathedral."));
		});

		// The cursed orbs of the west Sanctuary
		//-------------------------------------------------------------------------
		this.AddCursedOrb(1, -1777.29, -533.23, 321);
		this.AddCursedOrb(2, -1418.37, -768.05, 148);
		this.AddCursedOrb(3, -1016.88, -542.51, 348);
		this.AddCursedOrb(4, -1227.22, -635.94, 120);
		this.AddCursedOrb(5, -1597.06, -356.95, 204);
		this.AddCursedOrb(6, -1514.44, -566.90, 344);

		// Priest Goda
		//-------------------------------------------------------------------------
		AddNpc(147389, L("Priest Goda"), "CHATHEDRAL56_SQ03", "d_cathedral_56", -585.69, -818.91, 190, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Priest Goda"));

			if (character.Quests.IsActive(Sq03) && character.Quests.IsCompletable(Sq03))
			{
				await dialog.Msg(L("I will submit a report as soon as I return to the congregation."));
				await dialog.Msg(L("This should soothe the priests who were in despair. Thanks."));
				await dialog.CompleteQuest(Sq03);
				return;
			}

			if (!character.Quests.Has(Sq03) && character.Quests.MeetsPrerequisites(Sq03))
			{
				var answer = await dialog.SelectQuestOffer(Sq03, L("There is a dangerous demon luring priests and leading them to their deaths. It disguises itself to look like a secret of Maven. The location is Maskuote Narthex."),
					Option(L("I will investigate it"), "accept"),
					Option(L("I'm busy"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Sq03);
					await dialog.Msg(L("Please relieve our priests of this threat so they can concentrate on their mission."));
				}
				return;
			}

			if (character.Quests.IsActive(Sq03))
			{
				await dialog.Msg(L("The demon tricks are becoming more vicious."));
				await dialog.Msg(L("That demon is waiting for the next victim by hiding its evil energy."));
				return;
			}

			await dialog.Msg(L("The Great Cathedral has collapsed, but it still keeps its beauty."));
			await dialog.Msg(L("Especially, the secrets of Archbishop Maven, they look amazing every time we see them."));
		});

		// The false secret of Maskuote Narthex
		//-------------------------------------------------------------------------
		AddConditionalNpc(47254, L("Cathedral Desk"), "CHATHEDRAL56_NPC_HIDE", "d_cathedral_56", -2114.78, -427.79, 94, c => !c.Quests.HasCompleted(Sq03), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Cathedral Desk"));

			if (character.Quests.IsActive(Sq03) && !character.Quests.IsCompletable(Sq03))
			{
				var examined = await character.TimeActions.StartAsync(L("Examining the device..."), L("Cancel"), "LOOK", TimeSpan.FromSeconds(3));

				if (examined != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The secret of Maven comes apart and a Linkroller uncoils out of it."));
				character.Quests.StartQuestTrack(Sq03);
				return;
			}

			await dialog.Msg(L("A desk made to look like one of Maven's machines. It is neither."));
		});
	}

	/// <summary>
	/// The bishop's spirit at the Sanctuary entrance, who owns the demon
	/// transformation chain.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task SanctuaryBishop(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Bishop Aurelius' Spirit"));
		dialog.SetPortrait("Dlg_port_aurelius");

		if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
		{
			await dialog.Msg(L("You found it. Now let me see."));
			await dialog.Msg(L("Where was the clause.."));
			await dialog.CompleteQuest(Mq01);
			return;
		}

		if (character.Quests.IsActive(Mq02_1) && character.Quests.IsCompletable(Mq02_1))
		{
			await dialog.Msg(L("That will be enough."));
			await dialog.Msg(L("Now use the apparel to complete the scroll."));
			await dialog.CompleteQuest(Mq02_1);
			return;
		}

		if (!character.Quests.Has(Mq02_1) && character.Quests.MeetsPrerequisites(Mq02_1))
		{
			var answer = await dialog.SelectQuestOffer(Mq02_1, L("Ah. The method is rather simple. But you may have to put in some effort. First, get some articles of clothing from Naktis' servants."),
				Option(L("I'll go there"), "accept"),
				Option(L("Decline"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq02_1);
				await dialog.Msg(L("Using the demons... The goddesses may turn their heads away, but if this is the best method, I will do it."));
			}
			return;
		}

		if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
		{
			var answer = await dialog.SelectQuestOffer(Mq02, L("In this recipe, it says that you can complete the Demon Transformation Scroll at the Altar of Intelligence. Fortunately, the Altar of Intelligence is at Laukimas Antenave."),
				Option(L("I will complete the scroll"), "accept"),
				Option(L("Give me some time to prepare"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq02);
				await dialog.Msg(L("Resist against the demons' attacks while the scroll writes itself."));
			}
			return;
		}

		if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
		{
			var answer = await dialog.SelectQuestOffer(Mq03, L("Okay. Use the scroll to lure the demons to the Apgaule Altar. If you want to know what will happen... Well, you'll have to wait and see."),
				Option(L("Accept"), "accept"),
				Option(L("Reject"), "leave")
			);

			if (answer == "accept")
			{
				for (var i = 1; i <= 3; ++i)
					character.Variables.Perm.Set(LureVar + i, false);

				character.Quests.Start(Mq03);
				await dialog.Msg(L("The key is at Gaule Altar. I think you should leak that much information to Naktis' servants."));
				await dialog.Msg(L("Of course, you'll need to use the transformation scroll before doing so."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq01))
		{
			await dialog.Msg(L("There are documents here that describe a method of disguising as a demon in this Sanctuary."));
			await dialog.Msg(L("Through this method, you should be able to get the next key without shedding a drop of blood."));
			return;
		}

		if (character.Quests.IsActive(Mq02_1))
		{
			await dialog.Msg(L("This sanctuary is a treasure house of knowledge."));
			await dialog.Msg(L("You need to master the fundamentals of everything to protect the kingdom, and the revelation in the name of the goddess. Studying the demons is no exception."));
			return;
		}

		if (character.Quests.IsActive(Mq02))
		{
			await dialog.Msg(L("That's the passion of the priests who tried to protect the kingdom from the forces of evil."));
			await dialog.Msg(L("It continued on for hundreds of years until today."));
			return;
		}

		if (character.Quests.IsActive(Mq03))
		{
			await dialog.Msg(L("The secret supposedly unveils upon application of a holy force, but the trap will activate regardless if it detects an evil force."));
			await dialog.Msg(L("The plan is to get the key after exhausting that trap using Naktis' servants."));
			return;
		}

		await dialog.Msg(L("The Demon Lord of Curses, Naktis. She has not shown herself yet, but I'm sure she's somewhere inside the sanctuary."));
		await dialog.Msg(L("Perhaps she's waiting for us to get all the keys."));
	}

	/// <summary>
	/// The bishop as he waits at Pasala Altar, who owns the last two keys and
	/// the way into the hidden room.
	/// </summary>
	/// <param name="dialog"></param>
	private async Task PasalaBishop(Dialog dialog)
	{
		var character = dialog.Player;

		dialog.SetTitle(L("Bishop Aurelius' Spirit"));
		dialog.SetPortrait("Dlg_port_aurelius");

		if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
		{
			var answer = await dialog.SelectQuestOffer(Mq04, L("So the demons went to Apgaule Altar without any complaints? Now then, it is time we follow them."),
				Option(L("Tell him to go to Apgaule Altar"), "accept"),
				Option(L("Decline"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq04);
				await dialog.Msg(L("It is most definitely sinful to obtain the revelation in this manner."));
				await dialog.Msg(L("But of course, this is the only way to obtain Maven's fourth key."));
			}
			return;
		}

		if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
		{
			var answer = await dialog.SelectQuestOffer(Mq05, L("Finally, the last key. I've been waiting for this moment for hundreds of years. A sentiment that is neither joy nor emptiness fills my heart."),
				Option(L("I will find the key"), "accept"),
				Option(L("Give me some time"), "leave")
			);

			if (answer == "accept")
			{
				character.Quests.Start(Mq05);
				await dialog.Msg(L("The last secret is in the Small Reception Room."));
				await dialog.Msg(L("You should remember the order of the candles and their number."));
			}
			return;
		}

		if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
		{
			var answer = await dialog.SelectQuestOffer(Mq06, L("And so all five keys are now collected. Go to Pasala Altar. The secret there will lead you to the hidden room."),
				Option(L("I will go to Pasala Altar"), "accept"),
				Option(L("Decline"), "leave")
			);

			if (answer == "accept")
				character.Quests.Start(Mq06);

			return;
		}

		if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
		{
			var answer = await dialog.SelectQuestOffer(Mq07, L("You can't enter it now becuase the doors are sealed. Pass by the Tikinciuju Gallery and into the room. You'll find the barrier candlestick. The sealed door will open when you blow the candle."),
				Option(L("Look for a way to open the sealed door"), "accept"),
				Option(L("Not yet"), "leave")
			);

			if (answer == "accept")
			{
				character.Variables.Perm.Set(CandleVar + 1, false);
				character.Variables.Perm.Set(CandleVar + 2, false);

				character.Quests.Start(Mq07);
				await dialog.Msg(L("Two candlesticks hold the barrier up. Neither of them alone will do."));
			}
			return;
		}

		if (character.Quests.IsActive(Mq08) && character.Quests.IsCompletable(Mq08))
		{
			await dialog.Msg(L("I am honored to be able to now guide you to the hidden room."));
			await dialog.CompleteQuest(Mq08);
			character.LookAround();
			return;
		}

		if (character.Quests.IsActive(Mq04))
		{
			await dialog.Msg(L("Naktis' servants will be punished for prying on Maven's secret. Use that opportunity to get the key."));
			await dialog.Msg(L("Even if the goddess condemns me for this, I will hold my head high, because it was for the protection of the revelation."));
			return;
		}

		if (character.Quests.IsActive(Mq05))
		{
			await dialog.Msg(L("The number of seconds is important too, but for this secret you need to remember the sequence as well."));
			await dialog.Msg(L("It's similar to the first secret."));
			return;
		}

		if (character.Quests.IsActive(Mq08))
		{
			await dialog.Msg(L("Focus on the colors of the orbs."));
			await dialog.Msg(L("You should insert the key in orbs of the same color."));
			return;
		}

		await dialog.Msg(L("Keep your guard up."));
		await dialog.Msg(L("This may be the secret to the last key, but the demons are still watching."));
	}

	/// <summary>
	/// Adds one of the documents on demon transformation kept in the Sanctuary.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="monsterId"></param>
	/// <param name="name"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <param name="direction"></param>
	private void AddDemonDocument(int number, int monsterId, string name, double x, double z, double direction)
	{
		AddConditionalNpc(monsterId, name, "CHATHEDRAL56_MQ01_DOC" + number, "d_cathedral_56", x, z, direction, c => c.Quests.IsActive(Mq01), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(name);

			if (character.Variables.Perm.GetBool(DocumentVar + number, false))
			{
				await dialog.Msg(L("{#666666}*You have already taken this page*{/}"));
				return;
			}

			var read = await character.TimeActions.StartAsync(L("Reading the document..."), L("Cancel"), "READ", TimeSpan.FromSeconds(2));

			if (read != TimeActionResult.Completed)
				return;

			character.Variables.Perm.Set(DocumentVar + number, true);
			character.Inventory.Add(ItemId.CHATHEDRAL56_MQ01_ITEM, 1, InventoryAddType.PickUp);
			character.ServerMessage(LF("Documents found: {0}/{1}", character.Inventory.CountItem(ItemId.CHATHEDRAL56_MQ01_ITEM), DocumentsNeeded));
		});
	}

	/// <summary>
	/// Adds one of the three places the transformed Revelator has to be seen
	/// before the demons follow him to Apgaule Altar.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	private void AddLurePoint(int number, double x, double z)
	{
		AddQuestTrigger("CHATHEDRAL56_MQ03_LURE" + number, "d_cathedral_56", x, z, 250, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (!character.Quests.IsActive(Mq03) || character.Quests.IsCompletable(Mq03))
				return;

			if (character.Variables.Perm.GetBool(LureVar + number, false))
				return;

			character.Variables.Perm.Set(LureVar + number, true);

			var lured = 0;
			for (var i = 1; i <= 3; ++i)
			{
				if (character.Variables.Perm.GetBool(LureVar + i, false))
					lured++;
			}

			character.ServerMessage(LF("Demons drawn toward Apgaule Altar: {0}/{1}", lured, 3));

			if (lured >= 3)
			{
				character.Quests.CompleteObjective(Mq03, "lureTheDemons");
				character.ServerMessage(L("Every demon that saw you is on its way to Apgaule Altar."));
			}

			await Task.CompletedTask;
		});
	}

	/// <summary>
	/// Adds one of the two candlesticks that hold the barrier on the sealed
	/// door up.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	private void AddBarrierCandle(int number, double x, double z)
	{
		AddConditionalNpc(147358, L("Barrier Candlestick"), "CHATHEDRAL56_MQ07_HINT0" + number, "d_cathedral_56", x, z, 90, c => c.Quests.IsActive(Mq07), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Barrier Candlestick"));

			if (character.Variables.Perm.GetBool(CandleVar + number, false))
			{
				await dialog.Msg(L("{#666666}*This candle is already out*{/}"));
				return;
			}

			var blown = await character.TimeActions.StartAsync(L("Blowing the candle out..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

			if (blown != TimeActionResult.Completed)
				return;

			character.Variables.Perm.Set(CandleVar + number, true);

			if (!character.Variables.Perm.GetBool(CandleVar + 1, false) || !character.Variables.Perm.GetBool(CandleVar + 2, false))
			{
				character.ServerMessage(L("One candle is out. The barrier still holds on the other."));
				return;
			}

			character.Quests.CompleteObjective(Mq07, "openTheDoor");
			character.ServerMessage(L("Both candles are out and the sealed door has opened."));
		});
	}

	/// <summary>
	/// Adds one of the five coloured orbs Maven's keys are used on.
	/// </summary>
	/// <param name="index"></param>
	/// <param name="name"></param>
	/// <param name="line"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <param name="direction"></param>
	private void AddKeyOrb(int index, string name, string line, double x, double z, double direction)
	{
		var objectiveId = "orb" + KeyOrbs[index];

		AddNpc(147358, name, "CHATHEDRAL56_MQ08_" + KeyOrbs[index].ToUpper(), "d_cathedral_56", x, z, direction, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(name);

			await dialog.Msg(line);

			if (!character.Quests.IsActive(Mq08) || character.Quests.IsCompletable(Mq08))
				return;

			if (!character.Quests.TryGetById(Mq08, out var quest) || !quest.TryGetProgress(objectiveId, out var progress) || progress.Done)
				return;

			var used = await character.TimeActions.StartAsync(L("Setting the key into the orb..."), L("Cancel"), "SITGROPESET2", TimeSpan.FromSeconds(2));

			if (used != TimeActionResult.Completed)
				return;

			character.Quests.CompleteObjective(Mq08, objectiveId);
			character.ServerMessage(LF("The {0} takes its key.", name));
		});
	}

	/// <summary>
	/// Adds one of the orbs that spread Naktis' curse through the Sanctuary.
	/// </summary>
	/// <param name="number"></param>
	/// <param name="x"></param>
	/// <param name="z"></param>
	/// <param name="direction"></param>
	private void AddCursedOrb(int number, double x, double z, double direction)
	{
		AddConditionalNpc(151022, L("Cursed Orb"), "CHATHEDRAL56_SQ02_KILL" + number, "d_cathedral_56", x, z, direction, c => c.Quests.IsActive(Sq02), async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Cursed Orb"));

			if (character.Variables.Perm.GetBool(CursedOrbVar + number, false))
			{
				await dialog.Msg(L("{#666666}*You have already taken this orb*{/}"));
				return;
			}

			var taken = await character.TimeActions.StartAsync(L("Lifting the cursed orb..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

			if (taken != TimeActionResult.Completed)
				return;

			character.Variables.Perm.Set(CursedOrbVar + number, true);
			character.Inventory.Add(ItemId.CHATHEDRAL56_SQ02_ITEM, CursedOrbsPerPickup, InventoryAddType.PickUp);
			character.ServerMessage(LF("Cursed orbs collected: {0}/{1}", character.Inventory.CountItem(ItemId.CHATHEDRAL56_SQ02_ITEM), CursedOrbsNeeded));
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 20342: Adapting to Circumstances (2)
//-----------------------------------------------------------------------------
public class Cathedral56Mq02_1Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20342);
		SetName(L("Adapting to Circumstances (2)"));
		SetDescription(L("A demon's clothes are the first thing a demon transformation needs."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56_MQ_BISHOP", "d_cathedral_56", L("Talk to Bishop Aurelius"), L("Bishop Aurelius is thinking about something while reading the documents that contain the method of transforming into a demon. Talk to Bishop Aurelius again."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56_MQ_BISHOP", "d_cathedral_56", L("Collect demons' garments"), L("Bishop Aurelius asked you to collect materials to create the Demon Transformation Scroll. Defeat the demons and collect the materials."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56_MQ_BISHOP", "d_cathedral_56", L("Hand them over to Bishop Aurelius"), L("You've collected some demon garments. Return to Bishop Aurelius."));

		AddPrerequisite(new QuestStatusPrerequisite(20329, QuestStatus.Completed));

		AddPityDrop("CHATHEDRAL56_MQ02_1_ITEM", 0.3f, 7, 1, "Pawndel_blue", "Pawnd_purple", "NightMaiden_bow");

		AddObjective("collectGarments", L("Collect demons' garments"), new CollectItemObjective("CHATHEDRAL56_MQ02_1_ITEM", 5));

		AddReward(new ItemReward("expCard8", 1));
	}
}

// 20330: Adapting to Circumstances (3)
//-----------------------------------------------------------------------------
public class Cathedral56Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20330);
		SetName(L("Adapting to Circumstances (3)"));
		SetDescription(L("The Demon Transformation Scroll writes itself at the Altar of Intelligence, if it is left alone long enough."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56_MQ_BISHOP", "d_cathedral_56", L("Talk to Bishop Aurelius"), L("You've collected all the materials to transform into a demon. Talk to Bishop Aurelius again what to do next."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56_MQ02", "d_cathedral_56", L("Complete the Demon Transformation Scroll at the Altar of Intelligence"), L("Bishop Aurelius told you that you can complete the Demon Transformation Scroll at the Altar of Intelligence. Resist against the demons' attacks while you are completing the scroll."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56_MQ02", "d_cathedral_56", L("Acquire the Demon Transformation Scroll"), L("The Demon Transformation Scroll has been completed. Obtain the completed Demon Transformation Scroll."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHATHEDRAL56_MQ02_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20342, QuestStatus.Completed));

		AddObjective("completeTheScroll", L("Complete the Demon Transformation Scroll at the Altar of Intelligence"), new ManualObjective());

		AddReward(new ItemReward("expCard8", 1));
		AddReward(new ItemReward("CHATHEDRAL56_MQ02_ITEM", 1));
		AddReward(new TakeItemReward("CHATHEDRAL56_MQ02_1_ITEM"));
	}
}

// 20331: Adapting to Circumstances (4)
//-----------------------------------------------------------------------------
public class Cathedral56Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20331);
		SetName(L("Adapting to Circumstances (4)"));
		SetDescription(L("The demons have to be told where the key is, and they have to be told by one of their own."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56_MQ_BISHOP", "d_cathedral_56", L("Talk to Bishop Aurelius"), L("You are all prepared. Talk to Bishop Aurelius again."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56_MQ_BISHOP", "d_cathedral_56", L("Lure the demons to Apgaule Altar"), L("Lure the demons to Apgaule Altar after using the Demon Transformation Scroll."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56_MQ_BISHOP", "d_cathedral_56", L("Lure the demons to Apgaule Altar"), L("Lure the demons to Apgaule Altar after using the Demon Transformation Scroll."));

		AddPrerequisite(new QuestStatusPrerequisite(20330, QuestStatus.Completed));

		AddObjective("lureTheDemons", L("Lure the demons to Apgaule Altar"), new ManualObjective());

		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("CHATHEDRAL56_MQ02_ITEM", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The luring is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}
}

// 20332: Adapting to Circumstances (5)
//-----------------------------------------------------------------------------
public class Cathedral56Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20332);
		SetName(L("Adapting to Circumstances (5)"));
		SetDescription(L("The trap at Apgaule Altar spends itself on Naktis' servants, and the fourth key is left in the open."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56_BISHOP", "d_cathedral_56", L("Talk to Bishop Aurelius"), L("You've lured enough demons to Apgaule Altar. Talk to Bishop Aurelius again."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56_MQ04", "d_cathedral_56", L("Obtain Maven's Fourth Key"), L("You are all prepared. Get Maven's Fourth Key from the monsters at Apgaule Altar."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56_MQ04", "d_cathedral_56", L("Obtain Maven's Fourth Key"), L("You are all prepared. Get Maven's Fourth Key from the monsters at Apgaule Altar."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHATHEDRAL56_MQ04_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20331, QuestStatus.Completed));

		AddObjective("killDemons", L("Defeat the demons"), new KillObjective(6, "Pawndel_blue", "Pawnd_purple") { LayerOnly = true });

		AddReward(new ItemReward("CHATHEDRAL56_MQ04_PART2_ITEM", 1));
		AddReward(new ItemReward("expCard8", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.ServerMessage(L("Acquired Maven's key!"));
		character.Quests.Complete(this.QuestId);
	}
}

// 20333: The Last Key
//-----------------------------------------------------------------------------
public class Cathedral56Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20333);
		SetName(L("The Last Key"));
		SetDescription(L("The fifth secret is in the Small Reception Room, and it asks for an order as well as a count."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56_BISHOP", "d_cathedral_56", L("Talk to Bishop Aurelius"), L("Now is the time to obtain Maven's Fifth Key. Talk to Bishop Aurelius again."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56_MQ05_PUZZLE", "d_cathedral_56", L("Obtain Maven's Fifth Key by solving for the secret in the Small Reception Room"), L("Obtain the final key by solving for the Maven's secret at the Small Reception Room. If it is hard for you to solve the secret, call Bishop Aurelius to get some hints."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56_MQ05_PUZZLE", "d_cathedral_56", L("Obtain Maven's Fifth Key by solving for the secret in the Small Reception Room"), L("Obtain the final key by solving for the Maven's secret at the Small Reception Room. If it is hard for you to solve the secret, call Bishop Aurelius to get some hints."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Completed, "CHATHEDRAL56_MQ05_TRACK", 2000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20332, QuestStatus.Completed));

		AddObjective("solveSecret", L("Obtain Maven's Fifth Key by solving for the secret in the Small Reception Room"), new ManualObjective());

		AddReward(new ItemReward("CHATHEDRAL56_SQ01_ITEM", 1));
		AddReward(new ItemReward("expCard8", 2));
	}
}

// 20334: To Pasala Altar
//-----------------------------------------------------------------------------
public class Cathedral56Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20334);
		SetName(L("To Pasala Altar"));
		SetDescription(L("All five keys are in hand, and the hidden room is behind Pasala Altar."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56_BISHOP", "d_cathedral_56", L("Talk to Bishop Aurelius"), L("Acquired all the keys. Talk to Bishop Aurelius again."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56_SQ01", "d_cathedral_56", L("Enter into the Pasala Altar"), L("Bishop Aurelius told you to go to Pasala Altar that will lead you to a hidden room."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56_SQ01", "d_cathedral_56", L("Enter into the Pasala Altar"), L("Bishop Aurelius told you to go to Pasala Altar that will lead you to a hidden room."));

		AddPrerequisite(new QuestStatusPrerequisite(20333, QuestStatus.Completed));

		AddObjective("reachPasala", L("Enter into the Pasala Altar"), new ManualObjective());

		AddReward(new ItemReward("expCard8", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The arrival is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}
}

// 20335: The sealed door
//-----------------------------------------------------------------------------
public class Cathedral56Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20335);
		SetName(L("The sealed door"));
		SetDescription(L("The door to the hidden room is held shut by two candles in the Tikinciuju Gallery."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56_BISHOP", "d_cathedral_56", L("Look for a way to open the sealed door"), L("Look for a way to open the sealed door."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56_MQ07_HINT01", "d_cathedral_56", L("Look for a way to open the sealed door"), L("Look for a way to open the sealed door."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56_MQ07_HINT01", "d_cathedral_56", L("Go inside"), L("The sealed door is opened. Go inside."));

		// The client's track is the candle minigame, which has no server-side
		// equivalent; the candlesticks stand on the map instead.
		AddPrerequisite(new QuestStatusPrerequisite(20334, QuestStatus.Completed));

		AddObjective("openTheDoor", L("Look for a way to open the sealed door"), new ManualObjective());
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The client names no turn-in NPC and chains straight on to the orbs.
		character.Quests.Complete(this.QuestId);
	}
}

// 20336: The Secret at the End
//-----------------------------------------------------------------------------
public class Cathedral56Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20336);
		SetName(L("The Secret at the End"));
		SetDescription(L("Five orbs, five keys, and each key belongs to the orb of its own colour."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);
		SetReceive(QuestReceiveType.Auto);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56_BISHOP", "d_cathedral_56", L("Talk to Bishop Aurelius"), L("You've arrived at Pasala Altar. Talk to Bishop Aurelius again."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56_BISHOP", "d_cathedral_56", L("Look for the hidden path using the keys"), L("Use the keys from your inventory in front of the orbs to look for the hidden path. If it is hard for you solve the secret, then call Bishop Aurelius."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56_BISHOP", "d_cathedral_56", L("Move to the hidden room using the portal"), L("You've used all the keys of Maven. Move to the hidden room using the portal."));

		AddPrerequisite(new QuestStatusPrerequisite(20334, QuestStatus.Completed));
		AddPrerequisite(new QuestStatusPrerequisite(20335, QuestStatus.Completed));

		AddObjective("orbRed", L("Use Maven's key on the Red Orb"), new ManualObjective());
		AddObjective("orbBlue", L("Use Maven's key on the Blue Orb"), new ManualObjective());
		AddObjective("orbYellow", L("Use Maven's key on the Yellow Orb"), new ManualObjective());
		AddObjective("orbGreen", L("Use Maven's key on the Green Orb"), new ManualObjective());
		AddObjective("orbPurple", L("Use Maven's key on the Purple Orb"), new ManualObjective());

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new TakeItemReward("CHATHEDRAL53_MQ06_ITEM", 1));
		AddReward(new TakeItemReward("CHATHEDRAL54_MQ01_PART1_ITEM", 1));
		AddReward(new TakeItemReward("CHATHEDRAL54_MQ04_PART2_ITEM", 1));
		AddReward(new TakeItemReward("CHATHEDRAL56_MQ04_PART2_ITEM", 1));
		AddReward(new TakeItemReward("CHATHEDRAL56_SQ01_ITEM", 1));
	}
}

// 20337: Naktis' Wrath
//-----------------------------------------------------------------------------
public class Cathedral56Sq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20337);
		SetName(L("Naktis' Wrath"));
		SetDescription(L("The Demon Lord of Curses has been waiting behind the door to Pasala Altar the whole time."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56_SQ01", "d_cathedral_56", L("Enter into the Pasala Altar"), L("Open the door that will lead you to Pasala Altar."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56_SQ01", "d_cathedral_56", L("Defeat Naktis"), L("The angry Naktis suddenly attacked. Defeat Naktis."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56_SQ01", "d_cathedral_56", L("Defeat Naktis"), L("The angry Naktis suddenly attacked. Defeat Naktis."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHATHEDRAL56_SQ01_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(127));
		AddPrerequisite(new QuestStatusPrerequisite(20334, QuestStatus.Completed));

		AddObjective("killNaktis", L("Defeat Naktis"), new KillObjective(1, "boss_Naktis") { LayerOnly = true });

		AddReward(new ItemReward("expCard8", 2));
		AddReward(new ItemReward("Drug_AddStat", 1));
	}

	public override void OnSuccess(Character character, Quest quest)
	{
		base.OnSuccess(character, quest);

		// The fight is the quest; the client names no turn-in NPC.
		character.Quests.Complete(this.QuestId);
	}
}

// 20338: Cursed Orb
//-----------------------------------------------------------------------------
public class Cathedral56Sq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20338);
		SetName(L("Cursed Orb"));
		SetDescription(L("The orbs cannot be broken, so they have to be carried out one at a time."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56SQ04_NPC", "d_cathedral_56", L("Talk to Priest Prosit"), L("Talk to the priest."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56SQ04_NPC", "d_cathedral_56", L("Collect the curse-spreading orbs"), L("There are many orbs that are spreading the curse at various places in the Sanctuary. You can't destroy them normally so collect them and bring them to Priest Prosit."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56SQ04_NPC", "d_cathedral_56", L("Hand them over to Priest Prosit"), L("You've collected all the cursed orbs. Hand them over to Priest Prosit."));

		AddPrerequisite(new LevelPrerequisite(135));

		AddObjective("collectOrbs", L("Collect the curse-spreading orbs"), new CollectItemObjective("CHATHEDRAL56_SQ02_ITEM", 10));

		AddReward(new ItemReward("PRIST_REPORT03", 1));
		AddReward(new ItemReward("expCard8", 1));
		AddReward(new TakeItemReward("CHATHEDRAL56_SQ02_ITEM"));
	}
}

// 20339: Masquerade
//-----------------------------------------------------------------------------
public class Cathedral56Sq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20339);
		SetName(L("Masquerade"));
		SetDescription(L("Something at Maskuote Narthex is shaped like one of Maven's machines and is not one."));
		SetType(QuestType.Sub);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56_SQ03", "d_cathedral_56", L("Talk to Priest Goda"), L("Priest Goda seems to be wary about something. Talk to Priest Goda."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56_NPC_HIDE", "d_cathedral_56", L("Investigate the Platform of Maskuote Narthex"), L("Priest Goda asked you to defeat the demon that is disguised as a secret of Maven. Investigate the Platform of Maskuote Narthex and defeat the monster."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56_SQ03", "d_cathedral_56", L("Talk to Priest Goda"), L("You've found the demon who was disguised as a secret of Maven. Let Priest Goda know about it."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHATHEDRAL56_SQ03_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(135));

		AddObjective("killLinkroller", L("Defeat Linkroller"), new KillObjective(1, "boss_RingCrawler") { LayerOnly = true });

		AddReward(new ItemReward("expCard8", 2));
	}
}

// 20340: The Bishop's Last Mission (1)
//-----------------------------------------------------------------------------
public class Cathedral54Mq05Part3Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20340);
		SetName(L("The Bishop's Last Mission (1)"));
		SetDescription(L("Maven's last test stands between the portal and the revelation."));
		SetType(QuestType.Main);
		SetLocation("d_cathedral_56", "d_cathedral_54");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56_MQ_BISHOP_AFTER", "d_cathedral_56", L("Talk to Bishop Aurelius"), L("You've opened the portal that will lead you to a hidden room. Talk to Bishop Aurelius what to do next."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL54_MQ06_BOOK", "d_cathedral_54", L("Pass the Verification Test"), L("Use the portal in Pasala Altar and go to the room with the revelation. You need to pass Maven's final test to get the revelation."));
		SetPhase(QuestStatus.Success, "MQ05_PROOF_PRIST", "d_cathedral_54", L("Pass the Verification Test"), L("Use the portal in Pasala Altar and go to the room with the revelation. You need to pass Maven's final test to get the revelation."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHATHEDRAL54_MQ05_PART3_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(20336, QuestStatus.Completed));

		AddObjective("passTheTest", L("Pass the Verification Test"), new ManualObjective());

		AddReward(new ItemReward("expCard8", 2));
	}
}

// 50002: Purifying the Great Cathedral
//-----------------------------------------------------------------------------
public class Cathedral56Sq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(50002);
		SetName(L("Purifying the Great Cathedral"));
		SetDescription(L("Priest Prosit has run out of patience with the demons walking the Sanctuary."));
		SetType(QuestType.Repeat);
		SetLocation("d_cathedral_56");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHATHEDRAL56SQ04_NPC", "d_cathedral_56", L("Talk to Priest Prosit"), L("Priest Prosit seems to have many grievances. Talk to Priest Prosit."));
		SetPhase(QuestStatus.InProgress, "CHATHEDRAL56SQ04_NPC", "d_cathedral_56", L("Defeat the demons that are ruining the Great Cathedral"), L("Priest Prosit can't forgive the demons that are ruining the Great Cathedral. Defeat the demons on behalf of Priest Prosit."));
		SetPhase(QuestStatus.Success, "CHATHEDRAL56SQ04_NPC", "d_cathedral_56", L("Talk to Priest Prosit"), L("You've defeated many demons. Talk to Priest Prosit."));

		AddPrerequisite(new LevelPrerequisite(135));

		AddObjective("killDemons", L("Defeat the demons that are ruining the Great Cathedral"), new KillObjective(15, "Pawnd_purple", "Pawndel_blue"));
	}
}
