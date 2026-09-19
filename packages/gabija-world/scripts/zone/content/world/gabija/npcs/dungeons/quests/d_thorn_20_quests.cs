//--- Melia Script ----------------------------------------------------------
// Sirdgela Forest Quest NPCs
//--- Description -----------------------------------------------------------
// The Believers of Goddess Saule holding the line against the Thorn Forest.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Quests.Objectives;
using Melia.Zone.World.Quests.Prerequisites;
using Melia.Zone.World.Quests.Rewards;
using static Melia.Zone.Scripting.Shortcuts;

public class DThorn20QuestNpcsScript : GeneralScript
{
	private readonly static QuestId Mq01 = new QuestId(20261);
	private readonly static QuestId Mq02 = new QuestId(20262);
	private readonly static QuestId Mq03 = new QuestId(20263);
	private readonly static QuestId Mq04 = new QuestId(20264);
	private readonly static QuestId Mq06 = new QuestId(20266);
	private readonly static QuestId Mq07 = new QuestId(20267);

	protected override void Load()
	{
		// Believer Alvydas
		//-------------------------------------------------------------------------
		AddNpc(147389, L("Believer Alvydas"), "THORN20_MQ01", "d_thorn_20", -732.61, -1851.02, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Believer Alvydas"));

			if (character.Quests.IsActive(Mq01) && character.Quests.IsCompletable(Mq01))
			{
				await dialog.Msg(L("Every one of them has been heard out, and every one of them is still standing."));
				await dialog.Msg(L("The forest has not taken this stretch yet. That is more than I expected of today."));
				character.Quests.Complete(Mq01);
				return;
			}

			if (!character.Quests.Has(Mq01) && character.Quests.MeetsPrerequisites(Mq01))
			{
				await dialog.Msg(L("The evil energy of Kvailas Forest is becoming stronger and our Believers are becoming weak."));

				var answer = await dialog.Select(L("I am worried that we may lose to the forces of evil. If only the Revelator was with us..."),
					Option(L("I am the Revelator and I'll help you"), "accept"),
					Option(L("About the evil energy in the Thorn Forest"), "explain"),
					Option(L("Kvailas Forest is more urgent"), "leave")
				);

				if (answer == "explain")
				{
					await dialog.Msg(L("Perhaps the source of evil in Kvailas Forest has become the forest itself?"));
					await dialog.Msg(L("I only hope this is just an ill-founded worry."));
					return;
				}

				if (answer == "accept")
				{
					character.Quests.Start(Mq01);
					await dialog.Msg(L("Really?"));
					await dialog.Msg(L("I will let the other Believers know that you are helping them."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq01))
			{
				await dialog.Msg(L("The evil energy is becoming stronger and the Believers are getting tired. The situation is bad."));
				return;
			}

			await dialog.Msg(L("Goddess Saule set us here, and here we stay."));
		});

		// Believer Raminta
		//-------------------------------------------------------------------------
		AddNpc(147397, L("Believer Raminta"), "THORN20_MQ02_BOSS", "d_thorn_20", -98.84, -260.26, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Believer Raminta"));

			if (character.Quests.IsActive(Mq02) && character.Quests.IsCompletable(Mq02))
			{
				await dialog.Msg(L("Thanks."));
				await dialog.Msg(L("I wonder what would have happened if you weren't here."));
				character.Quests.Complete(Mq02);

				if (character.Quests.IsActive(Mq01))
					character.Quests.CompleteObjective(Mq01, "helpRaminta");

				return;
			}

			if (!character.Quests.Has(Mq02) && character.Quests.MeetsPrerequisites(Mq02))
			{
				await dialog.Msg(L("There is a demon that I want you to defeat."));

				var answer = await dialog.Select(L("This monster is called Rikaus and it's at Thorny Pillar Garden. It has been spreading evil energy."),
					Option(L("I'll take care of it right away"), "accept"),
					Option(L("It will be safer to let the other Believers know"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq02);
					await dialog.Msg(L("Now is the time to defeat it."));
					await dialog.Msg(L("We spent a long time finding it."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq02))
			{
				await dialog.Msg(L("Rikaus is at Thorny Pillar Garden, north of here."));
				character.Quests.ReplayQuestTrack(Mq02);
				return;
			}

			await dialog.Msg(L("Goddess Saule would not want the evil energy spreading to other forests. Therefore, we're here."));
		});

		// Believer Evaldas
		//-------------------------------------------------------------------------
		AddNpc(147398, L("Believer Evaldas"), "THORN20_MQ03_TRACK", "d_thorn_20", -428.99, 647.14, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Believer Evaldas"));

			if (character.Quests.IsActive(Mq03) && character.Quests.IsCompletable(Mq03))
			{
				await dialog.Msg(L("Thank you for your help."));
				await dialog.Msg(L("Now the demons won't be able to act so freely."));

				var pick = await dialog.Select(L("Take a pair of these off us. They were cut for the forest."),
					Option(L("Sirdgela Pants"), "cloth"),
					Option(L("Sirdgela Leather Pants"), "leather"),
					Option(L("Sirdgela Scale Pants"), "plate")
				);

				switch (pick)
				{
					case "cloth": character.Quests.SelectReward(Mq03, 522166); break;
					case "leather": character.Quests.SelectReward(Mq03, 522167); break;
					case "plate": character.Quests.SelectReward(Mq03, 522168); break;
				}

				character.Quests.Complete(Mq03);

				if (character.Quests.IsActive(Mq01))
					character.Quests.CompleteObjective(Mq01, "helpEvaldas");

				return;
			}

			if (!character.Quests.Has(Mq03) && character.Quests.MeetsPrerequisites(Mq03))
			{
				var answer = await dialog.Select(L("While the Merog Shamans are performing their rituals, you might have a chance to try something. Can you take care of them?"),
					Option(L("I can give it a try"), "accept"),
					Option(L("I don't feel it"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq03);
					await dialog.Msg(L("Now we just need to find out the Merogs' plan."));
					await dialog.Msg(L("Until then, please defeat those shamans."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq03))
			{
				await dialog.Msg(L("The shamans hold their rituals west of here. Break them where they stand."));
				character.Quests.ReplayQuestTrack(Mq03);
				return;
			}

			await dialog.Msg(L("I gave up a long time ago since I can't see an end to that evil energy. I just think of it as my destiny."));
		});

		// Believer Zaneta
		//-------------------------------------------------------------------------
		AddNpc(147386, L("Believer Zaneta"), "THORN20_MQ04_TRACK", "d_thorn_20", -1223.18, 602.53, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Believer Zaneta"));

			if (character.Quests.IsActive(Mq04) && character.Quests.IsCompletable(Mq04))
			{
				await dialog.Msg(L("Who'd have thought that they would be using the power of our own altar?"));
				await dialog.Msg(L("I don't know how I can face the goddess now."));
				character.Quests.Complete(Mq04);

				if (character.Quests.IsActive(Mq01))
					character.Quests.CompleteObjective(Mq01, "helpZaneta");

				return;
			}

			if (!character.Quests.Has(Mq04) && character.Quests.MeetsPrerequisites(Mq04))
			{
				await dialog.Msg(L("I did not know those demons could use summoning crystals."));

				var answer = await dialog.Select(L("They may overwhelm us."),
					Option(L("I will destroy the summon crystal"), "accept"),
					Option(L("You'll be able to run away"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq04);
					await dialog.Msg(L("This is all my fault. I shouldn't have underestimated it."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq04))
			{
				await dialog.Msg(L("The crystals stand in the middle of the circles. Break them both."));
				character.Quests.ReplayQuestTrack(Mq04);
				return;
			}

			await dialog.Msg(L("Goddess Saule said to absolutely not speak of our presence. She said it would only bring further chaos to the world."));
		});

		// Believer Simas
		//-------------------------------------------------------------------------
		AddNpc(147389, L("Believer Simas"), "THORN20_MAGIC3STAGE", "d_thorn_20", 2163.39, -1415.44, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Believer Simas"));

			if (character.Quests.IsActive(Mq06) && character.Quests.IsCompletable(Mq06))
			{
				await dialog.Msg(L("We should be able to find a way to use it."));
				await dialog.Msg(L("Those demons used our altars, so we can use theirs too."));
				character.Quests.Complete(Mq06);

				if (character.Quests.IsActive(Mq01))
					character.Quests.CompleteObjective(Mq01, "helpSimas");

				return;
			}

			if (!character.Quests.Has(Mq06) && character.Quests.MeetsPrerequisites(Mq06))
			{
				await dialog.Msg(L("I want to study the Demon Summoning Circle that has been emanating evil energy and summoning demons over there."));

				var answer = await dialog.Select(L("It's a precious sample, so please go gentle so you don't break much of it."),
					Option(L("Leave it to me"), "accept"),
					Option(L("I don't want to be part of a weird research"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq06);
					await dialog.Msg(L("I am okay about the sample so just break it so that it won't activate."));
					await dialog.Msg(L("I am gonna take a look at it myself."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq06))
			{
				await dialog.Msg(L("The circle is just up the slope. Break the crystal at its centre."));
				character.Quests.ReplayQuestTrack(Mq06);
				return;
			}

			await dialog.Msg(L("Everything will be resolved when the evil energy deep inside Kvailas Forest is dealt with."));
			await dialog.Msg(L("It's a pity that everyone is only after what's right in front of them."));
		});

		// Believer Onute
		//-------------------------------------------------------------------------
		AddNpc(147397, L("Believer Onute"), "THORN20_MQ07_TRACK", "d_thorn_20", 2315.64, 58.20, 90, async dialog =>
		{
			var character = dialog.Player;

			dialog.SetTitle(L("Believer Onute"));

			if (character.Quests.IsActive(Mq07) && character.Quests.IsCompletable(Mq07))
			{
				await dialog.Msg(L("Thanks."));
				await dialog.Msg(L("They will not be able to attack us easily now."));

				var pick = await dialog.Select(L("Archon's own, and a set of forest cloth besides. Take what suits you."),
					Option(L("Sirdgela Robe"), "cloth"),
					Option(L("Sirdgela Leather Tunic"), "leather"),
					Option(L("Sirdgela Scale Mail"), "plate")
				);

				switch (pick)
				{
					case "cloth": character.Quests.SelectReward(Mq07, 532166); break;
					case "leather": character.Quests.SelectReward(Mq07, 532167); break;
					case "plate": character.Quests.SelectReward(Mq07, 532168); break;
				}

				character.Quests.Complete(Mq07);

				if (character.Quests.IsActive(Mq01))
					character.Quests.CompleteObjective(Mq01, "helpOnute");

				return;
			}

			if (!character.Quests.Has(Mq07) && character.Quests.MeetsPrerequisites(Mq07))
			{
				await dialog.Msg(L("The shamans are trying to summon Archon."));

				var answer = await dialog.Select(L("It should frighten them if you can defeat Archon and destroy their summoning circles."),
					Option(L("I'll defeat Archon"), "accept"),
					Option(L("Leave the vicinity"), "leave")
				);

				if (answer == "accept")
				{
					character.Quests.Start(Mq07);
					await dialog.Msg(L("It angers me to see those filthy demons freely roaming around."));
					return;
				}
				return;
			}

			if (character.Quests.IsActive(Mq07))
			{
				await dialog.Msg(L("Archon is summoned at the corrupted altar to the north-east."));
				character.Quests.ReplayQuestTrack(Mq07);
				return;
			}

			await dialog.Msg(L("I get angry every time I enter that Thorn Forest."));
			await dialog.Msg(L("How could such dirty things happen to the goddess' sacred forest?"));
		});

		// Hidden triggers
		//-------------------------------------------------------------------------
		AddQuestTrigger("THORN20_MQ02_TRACK", "d_thorn_20", -59.49, 351.33, 200, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq02) && !character.Quests.IsCompletable(Mq02))
				character.Quests.StartQuestTrack(Mq02);

			await Task.CompletedTask;
		});

		AddQuestTrigger("THORN20_MQ03_TRIGGER", "d_thorn_20", -813.34, 960.98, 240, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq03) && !character.Quests.IsCompletable(Mq03))
				character.Quests.StartQuestTrack(Mq03);

			await Task.CompletedTask;
		});

		AddQuestTrigger("THORN20_MQ07_TRIGGER", "d_thorn_20", 2526.68, 734.74, 300, async args =>
		{
			if (args.Initiator is not Character character)
				return;

			if (character.Quests.IsActive(Mq07) && !character.Quests.IsCompletable(Mq07))
				character.Quests.StartQuestTrack(Mq07);

			await Task.CompletedTask;
		});
	}
}

//-----------------------------------------------------------------------------
// QUEST DEFINITIONS
//-----------------------------------------------------------------------------

// 20261: Some Help
//-----------------------------------------------------------------------------
public class Thorn20Mq01Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20261);
		SetName(L("Some Help"));
		SetDescription(L("The Believers of Goddess Saule are spread thin across Sirdgela Forest. Hear out every one of them."));
		SetType(QuestType.Sub);
		SetLocation("d_thorn_20");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN20_MQ01", "d_thorn_20", L("Talk to Believer Alvydas"), L("Goddess Saule Believer Alvydas is watching you."));
		SetPhase(QuestStatus.InProgress, "THORN20_MQ01", "d_thorn_20", L("Look for Goddess Saule Believers"), L("Find the Believers of Goddess Saule in Sirdgela Forest and help them."));
		SetPhase(QuestStatus.Success, "THORN20_MQ01", "d_thorn_20", L("Talk to Believer Alvydas"), L("Helped every Believer of Goddess Saule. Talk to Believer Alvydas."));

		AddPrerequisite(new LevelPrerequisite(51));

		AddObjective("helpRaminta", L("Help Believer Raminta"), new ManualObjective());
		AddObjective("helpEvaldas", L("Help Believer Evaldas"), new ManualObjective());
		AddObjective("helpZaneta", L("Help Believer Zaneta"), new ManualObjective());
		AddObjective("helpSimas", L("Help Believer Simas"), new ManualObjective());
		AddObjective("helpOnute", L("Help Believer Onute"), new ManualObjective());

		AddReward(new ItemReward("expCard3", 1));
	}
}

// 20262: Caught in the Middle
//-----------------------------------------------------------------------------
public class Thorn20Mq02Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20262);
		SetName(L("Caught in the Middle"));
		SetDescription(L("Rikaus has been spreading evil energy through Thorny Pillar Garden."));
		SetType(QuestType.Sub);
		SetLocation("d_thorn_20");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN20_MQ02_BOSS", "d_thorn_20", L("Talk to Believer Raminta"), L("Goddess Saule Believer Raminta needs your help."));
		SetPhase(QuestStatus.InProgress, "THORN20_MQ02_TRACK", "d_thorn_20", L("Defeat the source of the evil energy"), L("Believer Raminta says she found the demon spreading an evil energy. Defeat Rikaus for her."));
		SetPhase(QuestStatus.Success, "THORN20_MQ02_BOSS", "d_thorn_20", L("Report to Believer Raminta"), L("Defeated Rikaus that was spreading evil energy. Report to Believer Raminta."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "THORN20_MQ02_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(51));

		AddObjective("killRikaus", L("Defeat Rikaus"), new KillObjective(1, "boss_spector_gh") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 20263: The Art of Interference
//-----------------------------------------------------------------------------
public class Thorn20Mq03Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20263);
		SetName(L("The Art of Interference"));
		SetDescription(L("The Merog Shamans hold their rituals west of the Believers' camp. Break them up."));
		SetType(QuestType.Sub);
		SetLocation("d_thorn_20");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN20_MQ03_TRACK", "d_thorn_20", L("Talk to Believer Evaldas"), L("Goddess Saule Believer Evaldas needs your help."));
		SetPhase(QuestStatus.InProgress, "THORN20_MQ03_TRIGGER", "d_thorn_20", L("Interrupt the Merog Shaman's Ritual"), L("Believer Evaldas asked you to defeat the Shamans. Stop their rituals and defeat them."));
		SetPhase(QuestStatus.Success, "THORN20_MQ03_TRACK", "d_thorn_20", L("Report to Evaldas"), L("Defeat the Merog Shamans. Tell Believer Evaldas about it."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "THORN20_MQ03_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(51));

		AddObjective("killShamans", L("Defeat the Merog Shaman performing a ritual"), new KillObjective(3, "merog_wizzard"));

		AddReward(new ItemReward("expCard3", 2));
		AddReward(new SelectItemReward("LEG02_166", "LEG02_167", "LEG02_168"));
	}
}

// 20264: Don't Panic
//-----------------------------------------------------------------------------
public class Thorn20Mq04Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20264);
		SetName(L("Don't Panic"));
		SetDescription(L("The Merogs turned Goddess Saule's altars into summoning circles. Break the crystals at their centre."));
		SetType(QuestType.Sub);
		SetLocation("d_thorn_20");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN20_MQ04_TRACK", "d_thorn_20", L("Talk to Believer Zaneta"), L("Goddess Saule Believer Zaneta needs your help."));
		SetPhase(QuestStatus.InProgress, "THORN20_MQ04_TRACK", "d_thorn_20", L("Destroy the Demon Summoning Crystal"), L("Those Merogs prepared a Demon Summoning Circle that summons demons in Goddess Saule's Altar. Destroy the summoning crystal in the center and stop the demons."));
		SetPhase(QuestStatus.Success, "THORN20_MQ04_TRACK", "d_thorn_20", L("Report to Believer Zaneta"), L("Destroyed all the summoning crystals. Go and comfort Zaneta."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "THORN20_MQ04_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(51));

		AddObjective("breakCrystals", L("Destroy the Demon Summoning Crystal"), new KillObjective(2, "npc_pollution_crystal") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 20266: Unexpected Research
//-----------------------------------------------------------------------------
public class Thorn20Mq06Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20266);
		SetName(L("Unexpected Research"));
		SetDescription(L("Believer Simas wants the summoning circle broken, but not broken up."));
		SetType(QuestType.Sub);
		SetLocation("d_thorn_20");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN20_MAGIC3STAGE", "d_thorn_20", L("Talk to Believer Simas"), L("Goddess Saule Believer Simas needs your help."));
		SetPhase(QuestStatus.InProgress, "THORN20_MAGIC3STAGE", "d_thorn_20", L("Destroy the Demon Summoning Circle that is exuding evil energy"), L("Believer Simas says the Demon Summoning Circle is exuding evil energy along with demons and it is worth researching. Destroy the summoning circle."));
		SetPhase(QuestStatus.Success, "THORN20_MAGIC3STAGE", "d_thorn_20", L("Report to Believer Simas"), L("The summoning circle does not seem to work anymore. Tell Simas that he can study it."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "THORN20_MQ06_TRACK", 4000, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(51));

		AddObjective("breakCrystal", L("Destroy the Demon Summoning Crystal"), new KillObjective(1, "npc_pollution_crystal") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
	}
}

// 20267: Something Blasphemous
//-----------------------------------------------------------------------------
public class Thorn20Mq07Quest : QuestScript
{
	protected override void Load()
	{
		SetClientId(20267);
		SetName(L("Something Blasphemous"));
		SetDescription(L("The shamans are summoning Archon at one of the goddess' own altars."));
		SetType(QuestType.Sub);
		SetLocation("d_thorn_20");
		SetAutoTracked(true);
		SetCancelable(true);

		SetPhase(QuestStatus.Possible, "THORN20_MQ07_TRACK", "d_thorn_20", L("Talk to Believer Onute"), L("Goddess Saule Believer Onute needs your help."));
		SetPhase(QuestStatus.InProgress, "THORN20_MQ07_TRIGGER", "d_thorn_20", L("Defeat Archon and the Demon Summoning Circle"), L("Believer Onute is angry that the altar turned into a Demon Summoning Circle. Defeat Archon and destroy the Demon Summoning Circle."));
		SetPhase(QuestStatus.Success, "THORN20_MQ07_TRACK", "d_thorn_20", L("Report to Believer Onute"), L("Got rid of all the ominous things Onute mentioned. Report to Believer Onute."));

		SetTrack(QuestStatus.InProgress, QuestStatus.Success, "THORN20_MQ07_TRACK", 4000, autoStart: false, partyPlay: true);

		AddPrerequisite(new LevelPrerequisite(51));

		AddObjective("killArchon", L("Defeat Archon"), new KillObjective(1, "boss_archon") { LayerOnly = true });
		AddObjective("breakCrystal", L("Destroy the Demon Summoning Crystal"), new KillObjective(1, "npc_pollution_crystal") { LayerOnly = true });

		AddReward(new ItemReward("expCard3", 3));
		AddReward(new ItemReward("HAND02_121", 1));
		AddReward(new SelectItemReward("TOP02_166", "TOP02_167", "TOP02_168"));
	}
}
