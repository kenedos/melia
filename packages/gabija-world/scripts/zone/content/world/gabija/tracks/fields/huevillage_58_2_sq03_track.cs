//--- Melia Script ----------------------------------------------------------
// The Blue Woodspirit at Slepingas Stream
//--- Description -----------------------------------------------------------
// The spirit circling the Obelisk turns on whoever reads it.
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

[TrackScript("HUEVILLAGE_58_2_SQ03_TRACK")]
public class Huevillage582Sq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_2_SQ03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(861.57751f, 0.6496f, 232.13797f));

		actors.Add(AddTrackActor(character, 400742, 814.65997, 0.64999998, 606.78003, 106, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 147414, 859, 0, 205, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, MaxHp = 100, Name = L("Obelisk") }));
		actors.Add(AddTrackActor(character, 400741, 814.65601, 0.6496, 606.77509, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[2].AttachEffect("I_smoke046_levitation", 3.5f, EffectLocation.Bottom);
				break;
			case 4:
				track.Actors[2].AttachEffect("F_bg_smoke001", 2, EffectLocation.Bottom);
				break;
			case 12:
				track.Actors[2].AttachEffect("F_fire003_violet", 1, EffectLocation.Bottom);
				break;
			case 17:
				track.Actors[2].AttachEffect("F_fire003_violet", 1.5f, EffectLocation.Bottom);
				break;
			case 25:
				track.Actors[0].AttachEffect("F_fire003_violet", 1.7f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_smoke029_violet", 2.5f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_burstup001_violet", 1.2f, EffectLocation.Bottom);
				break;
			case 26:
				track.Actors[0].AttachEffect("F_ground004_violet", 2.5f, EffectLocation.Bottom);
				RemoveTrackActor(character, track, 2);
				break;
			case 34:
				track.Actors[0].AttachEffect("F_fire003_violet", 1.6f, EffectLocation.Bottom);
				break;
			case 39:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
