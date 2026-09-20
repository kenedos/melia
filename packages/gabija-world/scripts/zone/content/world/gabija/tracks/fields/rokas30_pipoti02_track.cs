//--- Melia Script ----------------------------------------------------------
// The first mark on the treasure map
//--- Description -----------------------------------------------------------
// A locked chest, and Yonazolem sitting on the key.
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

[TrackScript("ROKAS30_PIPOTI02_TRACK")]
public class Rokas30Pipoti02Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS30_PIPOTI02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(429.10f, 402.11f, 1038.03f));

		actors.Add(AddTrackActor(character, 147392, 359, 402, 1078, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Treasure Chest") }));
		actors.Add(AddTrackActor(character, 57406, 715.08, 402.11, 1272.27, 19, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(650.69f, 402.11f, 1205.93f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_light061", 0.5f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_lineup015_blue", 1, EffectLocation.Bottom);
				break;
			case 29:
				character.ServerMessage(L("A locked treasure chest appeared with Yonazolem beside it! Put Yonazolem down for the key."));
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
