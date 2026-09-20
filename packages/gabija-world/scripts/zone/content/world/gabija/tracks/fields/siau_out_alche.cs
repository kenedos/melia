//--- Melia Script ----------------------------------------------------------
// Vaidotas at the Vubbe Outpost
//--- Description -----------------------------------------------------------
// Vubbed cooks feed a fire beside the captive alchemist, then swarm the player.
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

[TrackScript("SIAU_OUT_ALCHE_TRACK")]
public class SiauOutAlcheTrack : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU_OUT_ALCHE_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		// The timeline has no line for the player, so the staging is the
		// trigger's own spot beside the captive.
		character.Movement.MoveTo(new Position(1309.12f, 147.36159f, 331.73f));

		actors.Add(AddTrackActor(character, 11120, 1287.4342, 197.81866, 213.98431, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 11120, 1269.3019, 197.93979, 177.70723, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 11120, 1248.7701, 197.92439, 237.68936, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57266, 1331.5354, 197.91174, 153.59682, 21, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1326.6726f, 197.59752f, 211.90953f) }));
		actors.Add(AddTrackActor(character, 57266, 1357.8835, 196.68309, 378.63419, 26, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1415.3765f, 196.68309f, 331.75012f) }));
		actors.Add(AddTrackActor(character, 47226, 1335.5791, 147.36159, 284.73895, 0, new TrackActorSpec { Ai = "MON_DUMMY" }));
		actors.Add(AddTrackActor(character, 20110, 1302.6486, 147.36159, 319.43243, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Vaidotas"), Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				RemoveTrackActor(character, track, 5);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
