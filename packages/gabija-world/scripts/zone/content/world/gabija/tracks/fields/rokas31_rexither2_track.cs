//--- Melia Script ----------------------------------------------------------
// Rexipher keeps his word
//--- Description -----------------------------------------------------------
// Odell is handed back, and Cactusvel is handed over with her.
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

[TrackScript("ROKAS31_REXITHER2_TRACK")]
public class Rokas31Rexither2Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS31_REXITHER2_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-138.12f, 107.10f, -522.66f));

		actors.Add(AddTrackActor(character, 47413, -158.85, 107.10, -500.81, 40, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Rexipher") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 41348, -193.90, 107.10, -411.17, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147345, -115.50, 107.10, -378.52, 33, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Historian Cyrenia Odell") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 10:
				track.Actors[2].AttachEffect("F_burstup007_smoke", 1, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("F_burstup001_smoke1", 1, EffectLocation.Bottom);
				break;
			case 39:
				// Rexipher fades out before the summon is handed over.
				RemoveTrackActor(character, track, 0);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
