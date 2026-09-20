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

		actors.Add(AddTrackActor(character, 45315, -82, 156, -612, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 45315, -41.535027, 157.88466, -606.04529, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 45315, -54.278545, 159.19519, -580.19104, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 106000, -122.34067, 153.713, -255.46884, 23, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-110.78307f, 148.14761f, -757.62317f) }));
		actors.Add(AddTrackActor(character, 106000, -62.511948, 153.713, -255.01018, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-57.673908f, 145.2319f, -821.33154f) }));
		actors.Add(AddTrackActor(character, 106000, -33.494717, 153.713, -307.05743, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-29.352127f, 148.14751f, -773.08704f) }));
		actors.Add(AddTrackActor(character, 40120, 228, 42, -1210, 0, new TrackActorSpec { Ai = "MON_DUMMY" }));
		actors.Add(AddTrackActor(character, 40070, 129, 153, -18, 0, new TrackActorSpec { Ai = "MON_DUMMY", Name = L("Lot 2 Closure Notice") }));
		actors.Add(AddTrackActor(character, 40070, -125, 153, -443, 0, new TrackActorSpec { Ai = "MON_DUMMY", Name = L("Notice") }));

		return actors.ToArray();
	}

	// The cutscene's own cast is three Vubbes; the rest come running once
	// the camera lets go, outside the actor list the client indexes.
	public override void OnHandOver(Character character, Track track)
	{
		AddRunningVubbe(character, -138, -712);
		AddRunningVubbe(character, -14, -742);
		AddRunningVubbe(character, -84, -676);
	}

	/// <summary>
	/// Spawns one of the Vubbes the explosion draws in, already coming for
	/// the player.
	/// </summary>
	private static void AddRunningVubbe(Character character, double x, double z)
	{
		var monster = AddTrackMonster(character, 106000, L("Vubbe Miner"), "None", x, 148, z, 0);

		if (monster == null)
			return;

		monster.Tendency = TendencyType.Aggressive;
		monster.InsertHate(character);
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
