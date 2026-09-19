//--- Melia Script ----------------------------------------------------------
// Colimencia in Dvyni Wetland
//--- Description -----------------------------------------------------------
// What flattened the flower bed comes up out of the standing water.
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

[TrackScript("HUEVILLAGE_58_3_SQ02_TRACK")]
public class Huevillage583Sq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_3_SQ02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(503.58026f, -86.707603f, 496.03918f));

		actors.Add(AddTrackActor(character, 47318, 439.42139, -86.707443, 207.03458, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147412, 498, -86, 512, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Name = L("Strongly Scented Soul Flower") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20025, 439.42001, -86.709999, 207.03, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[3].AttachEffect("F_bg_water008##6", 1, EffectLocation.Bottom);
				break;
			case 3:
				track.Actors[3].AttachEffect("F_bg_water008##6", 1.5f, EffectLocation.Bottom);
				break;
			case 8:
				track.Actors[3].AttachEffect("F_bg_water008##5", 2, EffectLocation.Bottom);
				break;
			case 9:
				track.Actors[3].AttachEffect("F_bg_water008##5", 2.5f, EffectLocation.Bottom);
				break;
			case 29:
				RemoveTrackActor(character, track, 3);
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
