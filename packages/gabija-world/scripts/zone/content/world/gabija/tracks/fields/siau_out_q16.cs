//--- Melia Script ----------------------------------------------------------
// Clearing the Mine Road
//--- Description -----------------------------------------------------------
// The explosives go up, the wagons burn, and the Vubbes come running.
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

[TrackScript("SIAU_OUT_Q16_TRACK")]
public class SiauOutQ16Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU_OUT_Q16_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-63.313873f, 145.2419f, -798.0307f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 45315, -82, 156, -612, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 45315, -41.535027, 157.88466, -606.04529, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 45315, -54.278545, 159.19519, -580.19104, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 106000, -122.34067, 153.713, -255.46884, 23, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-110.78307f, 148.14761f, -757.62317f) }));
		actors.Add(AddTrackActor(character, 106000, -62.511948, 153.713, -255.01018, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-57.673908f, 145.2319f, -821.33154f) }));
		actors.Add(AddTrackActor(character, 106000, -33.494717, 153.713, -307.05743, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-29.352127f, 148.14751f, -773.08704f) }));
		actors.Add(AddTrackActor(character, 40120, 228, 42, -1210, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 40070, 129, 153, -18, 0, new TrackActorSpec { Ai = "MON_DUMMY", Name = L("Lot 2 Closure Notice"), Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 40070, -125, 153, -443, 0, new TrackActorSpec { Ai = "MON_DUMMY", Name = L("Notice"), Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Vubbe miners the explosion draws out of the mine.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("DefGroup")
			.Monster(106000, -79.39, 151.4, -440.5, 0, respawnSeconds: 10)
			.Monster(106000, -97.71, 150.56, -414.69, 0, respawnSeconds: 10)
			.Monster(106000, -98.75, 150.72, -317.44, 0, respawnSeconds: 10)
			.Monster(106000, -73.3, 151.71, -369.01, 0, respawnSeconds: 10)
			.Monster(106000, -93.6, 150.38, -348.04, 0, respawnSeconds: 10)
			.Monster(57266, -94.79, 150.52, -284.55, 0, respawnSeconds: 10)
			.Monster(106000, -76.99, 151.12, -276.06, 0, respawnSeconds: 10)
			.Monster(106000, -83.61, 150.99, -318.74, 0, respawnSeconds: 10)
			.Monster(106000, -106.28, 151.16, -330.83, 0, respawnSeconds: 10)
			.Monster(106000, -98.7, 150.65, -381.61, 0, respawnSeconds: 10)
			.Monster(106000, -69.51, 152.06, -396.93, 0, respawnSeconds: 10)
			.Monster(106000, -61.99, 151.86, -355.92, 0, respawnSeconds: 10)
			.Monster(106000, -72.39, 151.41, -336.77, 0, respawnSeconds: 10)
			.Monster(106000, -82.17, 151.12, -342.99, 0, count: 2, respawnSeconds: 25)
			.Monster(57266, -90.06, 150.42, -359.72, 0, count: 2, respawnSeconds: 25)
			.Monster(106000, -85.28, 150.86, -394.69, 0, count: 2, respawnSeconds: 25)
			.Monster(106000, -47.15, 152.68, -329.35, 0, count: 2, respawnSeconds: 25)
			.Monster(106000, -68.93, 151.57, -304.09, 0, count: 2, respawnSeconds: 25);

		game.Start("DefGroup");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				CreateBattleBoxInLayer(character, track);
				break;
			case 15:
				track.Actors[1].AttachEffect("F_bg_fire001", 1.5f, EffectLocation.Middle);
				break;
			case 16:
				track.Actors[3].AttachEffect("F_bg_fire001", 1.6f, EffectLocation.Middle);
				break;
			case 17:
				track.Actors[2].AttachEffect("F_bg_fire001", 1.8f, EffectLocation.Middle);
				break;
			case 28:
				track.Actors[2].AttachEffect("F_explosion050_fire", 7f, EffectLocation.Middle);
				break;
			case 29:
				track.Actors[1].AttachEffect("F_explosion050_fire", 7f, EffectLocation.Middle);
				break;
			case 30:
				track.Actors[3].AttachEffect("F_explosion050_fire", 8f, EffectLocation.Middle);
				break;
			case 31:
				track.Actors[2].AttachEffect("F_explosion050_fire", 6f, EffectLocation.Middle);
				break;
			case 33:
				track.Actors[1].AttachEffect("F_explosion050_fire", 8f, EffectLocation.Middle);
				break;
			case 34:
				track.Actors[2].AttachEffect("F_explosion050_fire", 7.5f, EffectLocation.Middle);
				break;
			case 47:
				RemoveTrackActor(character, track, 1);
				break;
			case 58:
				RemoveTrackActor(character, track, 2);
				break;
			case 59:
				RemoveTrackActor(character, track, 3);
				break;
			case 67:
				StartMinigame(character, track);
				break;
			case 79:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
