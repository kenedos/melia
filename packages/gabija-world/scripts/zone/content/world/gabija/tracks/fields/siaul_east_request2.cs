//--- Melia Script ----------------------------------------------------------
// Scouting the Northern Woods
//--- Description -----------------------------------------------------------
// A Bube warband works at the northern edge of the woods, with carts, logs
// and a bonfire about the site.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("SIAUL_EAST_REQUEST2_TRACK")]
public class SiaulEastRequest2Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAUL_EAST_REQUEST2_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2094.591f, 130.0327f, 1084.638f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 400201, -2342.139, 130.0227, 1249.375, 21, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57192, -2538.553, 130.0227, 1280.636, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57192, -2513.046, 130.0227, 1244.668, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57192, -2494.043, 130.0227, 1287.674, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41394, -2437.413, 130.0227, 1279.383, 32, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-2436.4873f, 130.0227f, 1419.7678f),
		}));
		actors.Add(AddTrackActor(character, 41394, -2518.838, 130.0227, 1181.755, 32, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-2437.9355f, 130.0227f, 1294.9543f),
		}));
		actors.Add(AddTrackActor(character, 57192, -2314.289, 130.0227, 1704.204, 0, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-2514.7795f, 130.0227f, 1205.9437f),
		}));
		actors.Add(AddTrackActor(character, 57192, -2287.562, 130.0227, 1697.603, 0, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-2477.6309f, 130.0227f, 1417.6759f),
		}));
		actors.Add(AddTrackActor(character, 57192, -2273.817, 130.0227, 1707.453, 0, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-2485.0540f, 130.0227f, 1475.1992f),
		}));
		actors.Add(AddTrackActor(character, 57192, -2271.178, 130.0227, 1678.047, 0, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-2401.5127f, 130.0227f, 1360.7666f),
		}));
		actors.Add(AddTrackActor(character, 57192, -2303.882, 130.0227, 1704.933, 0, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-2459.4973f, 130.0227f, 1324.1390f),
		}));
		actors.Add(AddTrackActor(character, 46011, -2511.173, 130.0227, 1271.35, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41394, -2542.822, 130.0227, 1201.724, 22, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-2453.4080f, 130.0227f, 1245.7179f),
		}));
		actors.Add(AddTrackActor(character, 41393, -2485.895, 130.0227, 1478.038, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41394, -2452.405, 130.0227, 1389.819, 0, new TrackActorSpec
		{
			Ai = "TrackWaitMonster",
			EndPosition = new Position(-2471.4373f, 130.0227f, 1423.2887f),
		}));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 29:
				// The cutscene's DRT_KILL rows are Client="BOTH" and the client
				// never reports those frames, so the despawns run here instead.
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 12);
				RemoveTrackActor(character, track, 13);
				RemoveTrackActor(character, track, 14);
				RemoveTrackActor(character, track, 15);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
