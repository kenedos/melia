//--- Melia Script ----------------------------------------------------------
// Marnox in the Penitence Room
//--- Description -----------------------------------------------------------
// Marnox lies in wait beside the sealed chest that holds the Demon Orders.
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

[TrackScript("PRISON623_MQ_05_TRACK")]
public class Prison623Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON623_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(885.34f, 981.91f, 381.26f));
		actors.Add(character);

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(AddTrackActor(character, 156006, 854.65, 981.91, 412.96, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Priest Irma"), EndPosition = new Position(967.93f, 997.54f, 930.24f) }));
		actors.Add(AddTrackActor(character, 147469, 900.80, 981.91, 451.93, 0, prop));
		actors.Add(AddTrackActor(character, 147469, 930.76, 997.54, 674.26, 0, prop));
		actors.Add(AddTrackActor(character, 45324, 989.16, 997.54, 1000.21, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Sealed Chest") }));
		actors.Add(AddTrackActor(character, 58071, 945.24, 997.54, 673.29, 45, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(974.20f, 997.54f, 806.12f) }));
		actors.Add(AddTrackActor(character, 20024, 944.84, 997.54, 652.23, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 99:
				track.Dialog.SetTitle(L("Demon Lord Marnox"));
				StartDialog(track,
					L("It seems that everybody that says they are followers of the goddesses are half-witted. Do you still not know my strength after all that you have been through!"),
					L("I was waiting for you to come here. I will take care of you and torture the priest until the chest is unlocked!")
				);
				break;

			case 109:
				// The shadow Marnox rises from leaves on a Client="BOTH" row.
				RemoveTrackActor(character, track, 6);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
