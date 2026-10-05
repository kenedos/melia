//--- Melia Script ----------------------------------------------------------
// Tenet Church B1 Quest NPCs
//--- Description -----------------------------------------------------------
// The Followers holding the basement, and the altars and demons their quests
// run on.
//---------------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DChapel575QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq02 = new QuestId(8520);
	private readonly static QuestId Mq03 = new QuestId(8521);
	private readonly static QuestId Mq04 = new QuestId(8522);
	private readonly static QuestId Mq05 = new QuestId(8523);
	private readonly static QuestId Mq06 = new QuestId(8524);
	private readonly static QuestId Mq07 = new QuestId(8525);

	public const int HolyStoneChargeNeeded = 1000;
	private const int HolyStoneMonsterId = 46219;
	private const int HolyStoneLifeSeconds = 60;
	private const int HolyStoneChargePerKill = 50;
	private const float HolyStoneRange = 150;
	private readonly static ConcurrentDictionary<long, Npc> HolyStones = new();
	private readonly static Position HolyStoneArea = new Position(-858, 0, -173);
	private readonly static QuestId Mq08 = new QuestId(8526);
	private readonly static QuestId Mq09 = new QuestId(8527);

	// The corridor the demon barrier stands in runs from z -844 to z -723 on
	// both grounds, so a circle this wide seals it without touching the halls
	// that open out further along.
	private readonly static Position DemonBarrierPosition = new Position(363, 0, -783);
	private const float DemonBarrierRadius = 68;
	private MapBarrier DemonBarrier;

	protected override void Load()
	{
		if (ZoneServer.Instance.World.TryGetMap("d_chapel_57_5", out var chapelBasement))
			this.DemonBarrier = new MapBarrier(chapelBasement, "Chapel575DemonBarrier", DemonBarrierPosition, DemonBarrierRadius);

		// Follower Tomas
		//-------------------------------------------------------------------------
		AddNpc(147399, L("Follower Tomas"), "CHAPEL_TOMAS", "d_chapel_57_5", -489, 618, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Tomas"));

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("Great."));
				await dialog.Msg(L("Please leave activating the altar to me and go meet Vaidas at Himnas Chapel."));
				await dialog.CompleteQuest(Mq04);
				return;
			}

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("You are indeed the Revelator."));
				await dialog.Msg(L("Now, we can concentrate on protecting the altar."));
				await dialog.CompleteQuest(Mq02);
				return;
			}

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Thanks."));
				await dialog.Msg(L("I will share them with the other brothers before the demons find out."));
				await dialog.CompleteQuest(Mq03);
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("The Unknocker guards the altar. I will lure it - strike then."));
				character.Quests.ReplayQuestTrack(Mq04);
				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("Don't worry, the barrier at the main entrance to the 1st floor can be offset by the church's power."));
				var answer = await dialog.SelectQuestOffer(Mq04, L("That's what the altars are for."),
					Option(L("I'm ready"), "accept"),
					Option(L("I can't believe it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("Attack the Unknocker when I lure it away from the altar."));
				}
				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("The Yognomes are trying to eat the altar."));
				var answer = await dialog.SelectQuestOffer(Mq02, L("I want you to defeat them."),
					Option(L("I'll take care of it quickly"), "accept"),
					Option(L("I'll go when it is safer"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq02);

				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				await dialog.Msg(L("If you don't mind, can you get Eyes of Madness from Rodelins?"));
				var answer = await dialog.SelectQuestOffer(Mq03, L("They are handy when you want to hide yourself from demons."),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("I'll wait a little bit"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq03);

				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("The Yognomes are still at the altar. Thin them out."));
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("Kill the Rodelins and bring back their Eyes of Madness."));
				return;
			}

			await dialog.Msg(L("The Yognomes are still at the altar. Thin them out."));
		});

		// Follower Tiberius
		//-------------------------------------------------------------------------
		AddNpc(147400, L("Follower Tiberius"), "CHAPEL_TABERIJUS", "d_chapel_57_5", 140, 588, 0, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Tiberius"));

			if (character.Quests.IsActive(Mq05) && character.Quests.IsCompletable(Mq05))
			{
				await dialog.Msg(L("Good. This will be enough."));
				await dialog.CompleteQuest(Mq05);
				return;
			}

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("I'm thrilled to see that it went with a bang."));
				await dialog.Msg(L("But I think further research is needed still."));
				await dialog.CompleteQuest(Mq06);
				return;
			}

			if (!character.Quests.Has(Mq05) && character.Quests.MeetsPrerequisites(Mq05))
			{
				await dialog.Msg(L("I am making a Holy Bomb to attack demons with."));
				var answer = await dialog.SelectQuestOffer(Mq05, L("I can complete it if I obtain Purified Essence. Can you help me?"),
					Option(L("Alright, I'll help you"), "accept"),
					Option(L("It doesn't seem like a good idea"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("You can obtain Vicious Essences from the demons."));
					await dialog.Msg(L("You can purify these essences at the Altar of Purification, which is down from here."));
					character.Quests.Start(Mq05);
				}
				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("I want to test it out on Glizardon."));
				var answer = await dialog.SelectQuestOffer(Mq06, L("I'll give you the Holy Bomb. Can you put it onto a Glizardon's back?"),
					Option(L("Hmm, alright"), "accept"),
					Option(L("I need more preparation"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("Don't trust the water's effects too much, or Glizardon may find out about it."));
					character.Quests.Start(Mq06);
					character.Inventory.Add(650714, 1, InventoryAddType.PickUp);
					character.LookAround();
				}
				return;
			}

			if (character.Quests.IsActive(Mq05))
			{
				await dialog.Msg(L("The Altar of Purification is below. Purify the Vicious Essences there."));
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("Put the Holy Bomb on a Glizardon's back and run."));
				return;
			}

			await dialog.Msg(L("A bomb in the church is not ideal, but the goddess will forgive us."));
		});

		// Follower Vaidas
		//-------------------------------------------------------------------------
		AddNpc(147400, L("Follower Vaidas"), "CHAPEL_VIDAS", "d_chapel_57_5", -360, -94, 34, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Follower Vaidas"));

			if (character.Quests.IsActive(Mq09) && character.Quests.IsCompletable(Mq09))
			{
				await dialog.Msg(L("Is Brother Vaidutis alright?"));
				await dialog.Msg(L("I would have been in serious trouble if not for your help."));
				await dialog.Msg(L("Vaidutis went up to the 1st floor. Go to him."));
				return;
			}

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("I have never seen a stone filled with this much holiness."));
				await dialog.Msg(L("Thank you very much for giving it so much effort."));
				await dialog.CompleteQuest(Mq07);
				return;
			}

			if (character.Quests.IsActive(Mq08) && character.Quests.IsCompletable(Mq08))
			{
				await dialog.Msg(L("I would never have thought that you would defeat them this quickly."));
				await dialog.Msg(L("Thank you for your help."));
				await dialog.CompleteQuest(Mq08);
				return;
			}

			if (character.Quests.IsActive(Mq09))
			{
				await dialog.Msg(L("Break the barrier at the central altar and find Vaidutis."));
				character.Quests.ClearQuestTrack(Mq09);
				return;
			}

			if (!character.Quests.Has(Mq09) && character.Quests.MeetsPrerequisites(Mq09))
			{
				await dialog.Msg(L("It is reassuring to hear that Brother Tomas will be in charge of the altar."));
				var answer = await dialog.SelectQuestOffer(Mq09, L("The demons have made a magic barrier at the basement's central altar. Break it and look for Brother Vaidutis."),
					Option(L("I will go to the 1st floor"), "accept"),
					Option(L("Look for another way"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("The altars in the church are made to stop demons. Of course, only we, the Paladins, know how to use them."));
					character.Quests.Start(Mq09);
					this.SetDemonBarrier(true);
					character.LookAround();
				}
				return;
			}

			if (!character.Quests.Has(Mq08) && character.Quests.MeetsPrerequisites(Mq08))
			{
				await dialog.Msg(L("The attacks from the demons are quite aggressive."));
				var answer = await dialog.SelectQuestOffer(Mq08, L("I think they would be discouraged if we could somehow defeat the Glizardons..."),
					Option(L("I will try"), "accept"),
					Option(L("That's too much to handle"), "leave")
				);

				if (answer == "accept")
					character.Quests.Start(Mq08);

				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("I only have one empty Holy Stone."));
				var answer = await dialog.SelectQuestOffer(Mq07, L("Can you help me to fill this stone, please?"),
					Option(L("I can help you"), "accept"),
					Option(L("Hold on a little longer"), "leave")
				);

				if (answer == "accept")
				{
					await dialog.Msg(L("When you place the Holy Stone, the lives of the demons defeated near it will fill it."));
					await dialog.Msg(L("Defeat the demons near the Holy Stone."));
					character.Quests.Start(Mq07);
					character.Inventory.Add(650716, 1, InventoryAddType.PickUp);
				}
				return;
			}

			if (character.Quests.IsActive(Mq08))
			{
				await dialog.Msg(L("The Glizardons roam the basement. Two of them should dishearten the rest."));
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Place the Holy Stone and defeat the demons near it."));
				return;
			}

			await dialog.Msg(L("We can hold this floor a while longer, if the stones can be filled."));
		});

		// Altar of Purification
		//-------------------------------------------------------------------------
		AddNpc(147357, L("Altar of Purification"), "CHAPLE575_MQ_05", "d_chapel_57_5", 439, -109, 45, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Altar of Purification"));

			if (character.Quests.IsActive(Mq05) && !character.Quests.IsCompletable(Mq05))
			{
				if (!character.Inventory.HasItem(ItemId.CHAPLE575_MQ_05_ITEM))
				{
					character.ServerMessage(L("You don't have enough Vicious Essences"));
					return;
				}

				if (character.Inventory.CountItem(ItemId.CHAPLE575_MQ_05_1_ITEM) >= 15)
				{
					character.ServerMessage(L("Acquired enough Purified Essences"));
					return;
				}

				var purified = await character.TimeActions.StartAsync(L("Purifying"), L("Cancel"), "MAKING", TimeSpan.FromSeconds(2));

				if (purified != TimeActionResult.Completed || !character.Inventory.HasItem(ItemId.CHAPLE575_MQ_05_ITEM))
					return;

				character.Inventory.RemoveItem(ItemId.CHAPLE575_MQ_05_ITEM, 1);
				character.Inventory.Add(ItemId.CHAPLE575_MQ_05_1_ITEM, 1, InventoryAddType.PickUp);
				character.ServerMessage(L("Acquired a Purified Essence"));
				return;
			}

			await dialog.Msg(L("An altar set aside for cleansing tainted things."));
		});

		// Glizardons
		//-------------------------------------------------------------------------
		QuestSpots.Add(new QuestSpotSpec
		{
			Prefix = "CHAPLE575_MQ_06",
			MonsterId = 57021,
			Name = L("Glizardon"),
			Map = "d_chapel_57_5",
			Points = [(911, 450, 90), (543, 399, 90), (191, 258, 90)],
			Quest = Mq06,
			IsActive = c => c.Quests.IsActive(Mq06) && !c.Quests.IsCompletable(Mq06),
			Requirement = c => c.IsBuffActive(BuffId.CHAPLE575_MQ_06) ? null : L("The Glizardon would notice you. Drink the Namott Holy Water first."),
			TimedLabel = L("Attaching"),
			TimedAnim = "MAKING",
			Seconds = 1,
			IdleMessage = L("A hulking Glizardon, slow and sure of itself."),
			OnDone = (character, npc) =>
			{
				character.ServerMessage(L("You've attached the Holy Bomb onto Glizardon!"));
				character.Quests.AddObjectiveProgress(Mq06, "bombGlizardon");
				character.RemoveBuff(BuffId.CHAPLE575_MQ_06);
				if (npc != null)
					_ = character.PlayEffectToGround("F_light016", npc.Position, 2f, 2000f);
			},
		});

		// Underground Central Barrier
		//-------------------------------------------------------------------------
		// The fence in the gap is what the client draws and the barrier's
		// obstacle is what the player actually walks into, so both go up
		// together.
		AddConditionalNpc(MonsterId.Block_Fence, "", "CHAPLE575_MQ_09_FENCE", "d_chapel_57_5", 363, -783, 0, IsDemonBarrierUp)
			.WithEffect("F_lineup021_alpha", 1f, EffectLocation.Bottom);

		AddConditionalNpc(40071, L("Underground Central Barrier"), "CHAPLE575_MQ_09", "d_chapel_57_5", 363, -782, 90, IsDemonBarrierUp, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Underground Central Barrier"));

			if (character.Quests.IsActive(Mq09) && !character.Quests.IsCompletable(Mq09))
			{
				var brokeSeal = await character.TimeActions.StartAsync(L("Breaking the seal..."), L("Cancel"), "SITGROPE", TimeSpan.FromSeconds(2));

				if (brokeSeal != TimeActionResult.Completed)
					return;

				character.ServerMessage(L("The barrier shudders and gives way. Something massive stirs behind it."));

				this.SetDemonBarrier(false);
				character.Quests.StartQuestTrack(Mq09);
				return;
			}

			await dialog.Msg(L("A dark barrier blocks the way to the first floor."));
		}).WithEffect("F_lineup021_alpha", 1f, EffectLocation.Bottom);

	}

	/// <summary>
	/// Returns whether Gesti's barrier is still sealing the way up.
	/// </summary>
	/// <param name="character"></param>
	/// <returns></returns>
	private static bool IsDemonBarrierUp(Character character)
		=> character.Quests.IsActive(Mq09) && !character.Quests.IsCompletable(Mq09);

	/// <summary>
	/// Puts the wall in the corridor up or takes it down.
	/// </summary>
	/// <param name="blocked"></param>
	private void SetDemonBarrier(bool blocked)
	{
		this.DemonBarrier?.Set(blocked);
	}

	/// <summary>
	/// Keeps the wall in step with the quest for a character coming back to the
	/// floor.
	/// </summary>
	[On("PlayerEnteredMap")]
	public void OnPlayerEnteredMap(object sender, PlayerEventArgs args)
	{
		var character = args.Character;

		if (character.Map?.ClassName != "d_chapel_57_5")
			return;

		this.SetDemonBarrier(IsDemonBarrierUp(character));
	}

	/// <summary>
	/// Drinks the Namott Holy Water, hiding the character from the demons for a while.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_CHAPLE575_MQ_06_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "d_chapel_57_5" || character.Layer != 0 || !character.Quests.IsActive(Mq06) || character.Quests.IsCompletable(Mq06))
		{
			character.ServerMessage(L("There is no need to drink the holy water now."));
			return ItemUseResult.OkayNotConsumed;
		}

		character.StartBuff(BuffId.CHAPLE575_MQ_06, 1, 0, TimeSpan.FromSeconds(10), character);
		character.ServerMessage(L("You drank the Namott Holy Water. The demons cannot see you for a while!"));

		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Places the empty Holy Stone on the ground beside the character.
	/// </summary>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_CHAPLE575_MQ_07_ITEM(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		if (character.Map.ClassName != "d_chapel_57_5" || character.Layer != 0 || !character.Quests.IsActive(Mq07) || character.Quests.IsCompletable(Mq07))
		{
			character.ServerMessage(L("The Holy Stone does not react."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (character.Position.Get2DDistance(HolyStoneArea) > 650)
		{
			character.ServerMessage(L("Place the Holy Stone where the demons gather, as Vaidas told you."));
			return ItemUseResult.OkayNotConsumed;
		}

		if (TryGetHolyStone(character, out _))
		{
			character.ServerMessage(L("The Holy Stone is already placed."));
			return ItemUseResult.OkayNotConsumed;
		}

		var stone = new Npc(HolyStoneMonsterId, L("Holy Stone"), character.Position.GetRelative(character.Direction, 30), character.Direction.Backwards);
		stone.Layer = character.Layer;
		stone.Faction = FactionType.Neutral;
		stone.DisappearTime = GameClock.LocalNow.AddSeconds(HolyStoneLifeSeconds);
		stone.VisibleTo = c => c == character;
		stone.WithEffect("F_light055_blue", 2f, EffectLocation.Top).WithEffect("F_light055_blue", 8f, EffectLocation.Bottom);

		character.Map.AddMonster(stone);
		HolyStones[character.ObjectId] = stone;

		character.ServerMessage(L("You placed the Holy Stone. Defeat the demons near it to charge it!"));

		return ItemUseResult.OkayNotConsumed;
	}

	/// <summary>
	/// Charges the character's Holy Stone when a demon dies near it.
	/// </summary>
	[On("EntityKilled")]
	public void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Target is not Mob mob || mob.Faction != FactionType.Monster)
			return;

		var character = mob.GetKillBeneficiary(args.Attacker);
		if (character == null || !character.Quests.IsActive(Mq07) || character.Quests.IsCompletable(Mq07))
			return;

		if (!TryGetHolyStone(character, out var stone) || !mob.Position.InRange2D(stone.Position, HolyStoneRange))
			return;

		character.Quests.AddObjectiveProgress(Mq07, "chargeStone", HolyStoneChargePerKill);
		mob.PlayEffect("F_light015_violet1", 1f);
	}

	/// <summary>
	/// Returns the character's Holy Stone if it is still standing.
	/// </summary>
	private static bool TryGetHolyStone(Character character, out Npc stone)
	{
		if (HolyStones.TryGetValue(character.ObjectId, out stone) && stone.Map != null && stone.Map == character.Map && stone.DisappearTime > GameClock.LocalNow)
			return true;

		HolyStones.TryRemove(character.ObjectId, out _);
		stone = null;
		return false;
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 8520: Nibble Nibble
//-----------------------------------------------------------------------------
public class Chaple575Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8520);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetName(L("Nibble Nibble"));
		SetDescription(L("The Yognomes are trying to eat the altar. Thin them out."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_TOMAS", "d_chapel_57_5", L("Talk to Follower Tomas"), L("Follower Tomas wants help on the Tenet Church 1F."));
		SetPhase(QuestStatus.InProgress, "CHAPEL_TOMAS", "d_chapel_57_5", L("Defeat Yognome"), L("Defeat the Yognomes trying to eat the altar."));
		SetPhase(QuestStatus.Success, "CHAPEL_TOMAS", "d_chapel_57_5", L("Talk to Follower Tomas"), L("Return to Follower Tomas."));

		AddPrerequisite(new LevelPrerequisite(30));

		AddObjective("killYognome", L("Defeat Yognome"), new KillObjective(8, "Yognome"));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
	}
}

// 8521: Mark Territory
//-----------------------------------------------------------------------------
public class Chaple575Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8521);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetName(L("Mark Territory"));
		SetDescription(L("The Rodelins carry Eyes of Madness that hide a man from demons."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_TOMAS", "d_chapel_57_5", L("Talk to Follower Tomas"), L("Follower Tomas wants help on the Tenet Church 1F."));
		SetPhase(QuestStatus.InProgress, "CHAPEL_TOMAS", "d_chapel_57_5", L("Collect the Eyes of Madness"), L("Defeat Rodelins and retrieve their Eyes of Madness."));
		SetPhase(QuestStatus.Success, "CHAPEL_TOMAS", "d_chapel_57_5", L("Talk to Follower Tomas"), L("Return to Follower Tomas."));

		AddPityDrop("CHAPLE575_MQ_03_ITEM", 0.8f, 3, 1, "zombiegirl2_chpel");

		AddPrerequisite(new LevelPrerequisite(30));

		AddObjective("collectEyes", L("Defeat Rodelins and obtain Eyes of Madness"), new CollectItemObjective("CHAPLE575_MQ_03_ITEM", 6));

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("CHAPLE575_MQ_03_ITEM"));
	}
}

// 8522: Church Underground Passage
//-----------------------------------------------------------------------------
public class Chaple575Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8522);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetName(L("Church Underground Passage"));
		SetDescription(L("An Unknocker guards the altar. Put it down so the barrier can be offset."));
		SetType(QuestType.Main);
		SetLocation("d_chapel_57_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_TOMAS", "d_chapel_57_5", L("Talk to Follower Tomas"), L("Follower Tomas wants help on the 1st floor of the Tenet Church."));
		SetPhase(QuestStatus.InProgress, "CHAPLE575_MQ_04", "d_chapel_57_5", L("Defeat Unknocker"), L("Attack the Unknocker when Follower Tomas lures it."));
		SetPhase(QuestStatus.Success, "CHAPEL_TOMAS", "d_chapel_57_5", L("Talk to Follower Tomas"), L("Talk to Follower Tomas again."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHAPLE575_MQ_04_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8609, QuestStatus.Completed));

		AddObjective("killUnknocker", L("Defeat Unknocker"), new KillObjective(1, "boss_Unknocker") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("TreasureboxKey2", 1));
	}
}

// 8523: Light Attack (1)
//-----------------------------------------------------------------------------
public class Chaple575Mq05Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8523);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetName(L("Light Attack (1)"));
		SetDescription(L("Tiberius needs Purified Essence for his Holy Bomb."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_TABERIJUS", "d_chapel_57_5", L("Talk to Follower Tiberius"), L("Tiberius is waiting for someone's help in the Tenet Church 1F."));
		SetPhase(QuestStatus.InProgress, "CHAPLE575_MQ_05", "d_chapel_57_5", L("Obtained the Purified Essences"), L("Purify Vicious Essences at the Altar of Purification."));
		SetPhase(QuestStatus.Success, "CHAPEL_TABERIJUS", "d_chapel_57_5", L("Talk to Follower Tiberius"), L("Return to Tiberius."));

		AddPrerequisite(new LevelPrerequisite(30));

		AddObjective("purifyEssence", L("Obtain the Purified Essence"), new CollectItemObjective("CHAPLE575_MQ_05_1_ITEM", 5));

		AddPityDrop("CHAPLE575_MQ_05_ITEM", 1.0f, 0, 1, "zombiegirl2_chpel", "Yognome");

		AddReward(new ItemReward("expCard3", 1));
		AddReward(new TakeItemReward("CHAPLE575_MQ_05_1_ITEM"));
		AddReward(new TakeItemReward("CHAPLE575_MQ_05_ITEM"));
	}
}

// 8524: Light Attack (2)
//-----------------------------------------------------------------------------
public class Chaple575Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8524);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetName(L("Light Attack (2)"));
		SetDescription(L("Tiberius wants to test the Holy Bomb on a Glizardon."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_TABERIJUS", "d_chapel_57_5", L("Talk to Follower Tiberius"), L("Tiberius is waiting for someone's help in the Tenet Church 1F."));
		SetPhase(QuestStatus.InProgress, "CHAPLE575_MQ_06_2", "d_chapel_57_5", L("Defeat Glizardon using the Holy Bomb"), L("Put the Holy Bomb on a Glizardon's back."));
		SetPhase(QuestStatus.Success, "CHAPEL_TABERIJUS", "d_chapel_57_5", L("Talk to Follower Tiberius"), L("Return to Tiberius."));

		AddPrerequisite(new QuestStatusPrerequisite(8523, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(30));

		AddObjective("bombGlizardon", L("Defeat Glizardon using the Holy Bomb"), new ManualObjective(2));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("CHAPLE575_MQ_06_ITEM"));
	}
}

// 8525: Till the Last Drop
//-----------------------------------------------------------------------------
public class Chaple575Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8525);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetName(L("Till the Last Drop"));
		SetDescription(L("Vaidas only has one empty Holy Stone left. Fill it with demon lives."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_VIDAS", "d_chapel_57_5", L("Talk to Follower Vaidas"), L("Vaidas is waiting for help at the Tenet Church B1."));
		SetPhase(QuestStatus.InProgress, "CHAPEL_VIDAS", "d_chapel_57_5", L("Charge the Holy Stone"), L("Place the Holy Stone and defeat the demons near it."));
		SetPhase(QuestStatus.Success, "CHAPEL_VIDAS", "d_chapel_57_5", L("Talk to Follower Vaidas"), L("Return to Vaidas."));

		AddPrerequisite(new QuestStatusPrerequisite(8524, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(30));

		AddObjective("chargeStone", L("Charge the Holy Stone"), new ManualObjective(DChapel575QuestNpcsScript.HolyStoneChargeNeeded));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new TakeItemReward("CHAPLE575_MQ_07_ITEM"));
	}
}

// 8526: Bully
//-----------------------------------------------------------------------------
public class Chaple575Mq08Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8526);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetName(L("Bully"));
		SetDescription(L("Defeat the Glizardons that roam the Tenet Church B1."));
		SetType(QuestType.Sub);
		SetLocation("d_chapel_57_5");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_VIDAS", "d_chapel_57_5", L("Talk to Follower Vaidas"), L("Vaidas is waiting for help at the Tenet Church B1."));
		SetPhase(QuestStatus.InProgress, "CHAPEL_VIDAS", "d_chapel_57_5", L("Defeat Glizardon"), L("Defeat the Glizardons that roam the Tenet Church B1."));
		SetPhase(QuestStatus.Success, "CHAPEL_VIDAS", "d_chapel_57_5", L("Talk to Follower Vaidas"), L("Return to Vaidas."));

		AddPrerequisite(new QuestStatusPrerequisite(8524, QuestStatus.Completed));
		AddPrerequisite(new LevelPrerequisite(30));

		AddObjective("killGlizardon", L("Defeat Glizardon"), new KillObjective(2, "Glizardon"));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new ItemReward("Drug_SP1_Q", 30));
	}
}

// 8527: Beyond the Darkness
//-----------------------------------------------------------------------------
public class Chaple575Mq09Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(8527);
		SetUnlock(QuestUnlockType.AllAtOnce);
		SetName(L("Beyond the Darkness"));
		SetDescription(L("Break the barrier Gesti left at the basement's central altar and find Vaidutis."));
		SetType(QuestType.Main);
		SetLocation("d_chapel_57_5", "d_chapel_57_6");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "CHAPEL_VIDAS", "d_chapel_57_5", L("Talk to Follower Vaidas"), L("Follower Tomas asked you to find Vaidutis at the Tenet Church B1."));
		SetPhase(QuestStatus.InProgress, "CHAPLE575_MQ_09", "d_chapel_57_5", L("Deactivate the barrier that Gesti created"), L("Destroy the barrier at the basement's central altar."));
		SetPhase(QuestStatus.Success, "CHAPEL_VIRGINIJA", "d_chapel_57_6", L("Find Follower Vaidutis"), L("Look for Vaidutis, who ran away to the 1st floor."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "CHAPLE575_MQ_09_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new QuestStatusPrerequisite(8522, QuestStatus.Completed));

		AddObjective("killCyclops", L("Defeat Cyclops"), new KillObjective(1, "boss_Strongholder_Q1") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("NECK02_113", 1));
		AddReward(new SelectItemReward("SWD02_123", "TSW02_119", "STF02_118", "TSF02_118", "TBW02_121", "BOW02_117", "MAC02_120", "SPR02_115"));
	}
}
