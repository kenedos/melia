//--- Melia Script ----------------------------------------------------------
// The Bokor Master Reads the Slate
//--- Description -----------------------------------------------------------
// The Bokor Master summons the vision of the slate, and Laima speaks.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CMINE6_TO_KATYN7_2_TRACK")]
public class Cmine6ToKatyn72Track : TrackScript
{
	protected override void Load()
	{
		SetId("CMINE6_TO_KATYN7_2_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 20136, -22.83, 0.01, 32.79, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 154040, -63.55, 0.01, -19.95, 10, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47234, -4.82, 0.01, -4.23, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override void OnComplete(Character character, Track track)
	{
		character.Inventory.Remove(ItemId.Stonetablet01_Noread, 1, InventoryItemRemoveMsg.Given);
		character.Inventory.Add(ItemId.Stonetablet01, 1);
		character.Inventory.Add(ItemId.COLLECT_116, 1);

		base.OnComplete(character, track);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 7:
				track.Dialog.SetTitle(L("Bokor Master"));
				track.Dialog.SetPortrait("Dlg_port_BOCOR");
				StartDialog(track,
					L("So you are curious about this stone slate. If you wish, I shall show you the will of its creator."));
				break;
			case 36:
				track.Dialog.SetTitle(L("Goddess Laima"));
				track.Dialog.SetPortrait("Dlg_port_Raima");
				StartDialog(track,
					L("By the time you get this message, much time will have passed. I am Laima, the goddess of fate and wisdom."),
					L("600 years ago while creating this revelation, I foresaw a great deal of death and suffering. If you are seeing this, then I am afraid what I foresaw has come to pass."),
					L("The demons have started the first of the cataclysms, and the goddesses have vanished. I knew all of this would occur and did everything I could to prevent this fate, but my power was not enough."),
					L("Instead, I wandered through far-seeing dreams for an answer. You were at the end of them... a Savior."),
					L("Unfortunately, the demons could not stay still, knowing that I possess strength in clairvoyance. I was forced to hide until the time of your appearance."),
					L("And to protect you... I could do no more than to hide you amongst many other Revelators."),
					L("I decided to divide my being and remain within many revelations. Revelations that I continued to create secretly over a millennium."),
					L("Savior, if you can help me regain my form, we can stop the demons' sinister plans. You must recover the revelations that I have hidden throughout every axis of time."));
				break;
			case 39:
				StartDialog(track,
					L("This divine task may be a heavy burden, but it is something you must bear. Perhaps in the muddy midst of your trials and tribulations, something may come about."),
					L("Nevertheless, only we can save this world."),
					L("I have passed the next revelation to the first Paladin. He will be waiting for you at the highest flower garden."));
				break;
			case 42:
				track.Dialog.SetTitle(L("Bokor Master"));
				track.Dialog.SetPortrait("Dlg_port_BOCOR");
				StartDialog(track,
					L("Amazing. This must have been created hundreds of years ago... It seems to speak of you."),
					L("I don't feel any evil energy nor malicious intent in this. If so, this revelation must truly have come from the Goddess Laima..."),
					L("I'm surprised about this talk of a 'Savior'."),
					L("Amongst the many Revelators, the one who can save the world and the goddesses... The revelation says you are that Savior."),
					L("You better keep quiet about this. Unless you want the demons looking for you."));
				break;
			case 45:
				StartDialog(track,
					L("I don't want to talk anymore about Saviors and such. Now, you should hurry back to Uska."),
					L("He will tell you where to go. May the goddess bless you in the future."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
