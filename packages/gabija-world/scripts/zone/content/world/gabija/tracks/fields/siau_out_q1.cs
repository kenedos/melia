//--- Melia Script ----------------------------------------------------------
// The Vubbe Raid on Miners' Village
//--- Description -----------------------------------------------------------
// Mercenaries and guards hold the village gate while the Vubbes break through.
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

[TrackScript("SIAU_OUT_Q1_TRACK")]
public class SiauOutQ1Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU_OUT_Q1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(433.40222f, 37.428799f, -1488.7626f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 10023, 277.19135, 37.4188, -1237.8811, 58, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(486.17722f, 37.4188f, -1072.9946f) }));
		actors.Add(AddTrackActor(character, 10023, 240.61942, 42.792099, -1226.9606, 29, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 10023, 204.23793, 37.4188, -1294.1318, 59, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(282.48199f, 40.257553f, -1222.7212f) }));
		actors.Add(AddTrackActor(character, 41206, 81.597458, 37.4188, -1380.5377, 57, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(382.33456f, 37.4188f, -1122.6268f) }));
		actors.Add(AddTrackActor(character, 11120, 416.06805, 42.792099, -1250.7441, 25, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(537.15192f, 42.792099f, -1130.9011f) }));
		actors.Add(AddTrackActor(character, 11160, 378.18661, 42.792099, -1198.8369, 49, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(563.61206f, 42.792099f, -1062.1309f) }));
		actors.Add(AddTrackActor(character, 20011, 154.48376, 82.188278, -1090.9487, 76, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(60.252472f, 37.4188f, -1386.3247f) }));
		actors.Add(AddTrackActor(character, 11120, 434.49924, 42.404747, -1318.0049, 48, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(530.65814f, 42.792099f, -1107.3235f) }));
		actors.Add(AddTrackActor(character, 11120, 390.11716, 37.4188, -1120.5322, 58, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(505.48935f, 37.4188f, -1044.0007f) }));
		actors.Add(AddTrackActor(character, 10023, 251.12672, 37.4188, -1315.533, 64, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(354.09637f, 37.4188f, -1313.3849f) }));
		actors.Add(AddTrackActor(character, 41206, 119.87142, 41.057682, -1452.5143, 54, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(107.95893f, 59.224022f, -1175.3545f) }));
		actors.Add(AddTrackActor(character, 40120, 228, 42, -1210, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 39:
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
