//--- Melia Script ----------------------------------------------------------
// The Revelator's Magic Circle
//--- Description -----------------------------------------------------------
// Bishop Urbonas tests whether the player is the Revelator of the legends.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("PRISON621_MQ_06_TRACK")]
public class Prison621Mq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON621_MQ_06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(523.07f, 430.99f, 660.73f));
		actors.Add(character);

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(AddTrackActor(character, 154057, 522.07, 430.99, 683.01, 44, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Bishop Urbonas") }));
		actors.Add(AddTrackActor(character, 147358, 805.91, 323.04, 578.69, 0, prop));
		actors.Add(AddTrackActor(character, 147358, 822.15, 323.04, 761.64, 0, prop));
		actors.Add(AddTrackActor(character, 147358, 987.77, 323.04, 752.99, 0, prop));
		actors.Add(AddTrackActor(character, 147358, 965.82, 323.04, 575.53, 0, prop));
		actors.Add(AddTrackActor(character, 147469, 899.11, 323.04, 664.95, 13, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Dialog.SetTitle(L("Bishop Urbonas"));
				StartDialog(track,
					L("We have long prepared to receive the Revelator in accordance to the legends."),
					L("What stands below are four sacred candlesticks... They are crafted in accordance to legend passed only to the bishops of Orsha."),
					L("So that we could bring and confirm Revelators at any time."),
					L("Now. Step onto the magic circle between the candlesticks. If you are indeed a Revelator, the magic circle will react and you will be able to oppose the Demon Lord with that power.")
				);
				break;

			case 69:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, L("An unknown sacred power is bursting inside the body!"), 5);
				character.Quests.CompleteObjective(new QuestId(60120), "checkCircle");
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
