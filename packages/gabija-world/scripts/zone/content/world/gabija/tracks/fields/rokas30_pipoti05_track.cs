//--- Melia Script ----------------------------------------------------------
// The last mark on the treasure map
//--- Description -----------------------------------------------------------
// The Werewolf comes down the ridge the moment the last chest is found.
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

[TrackScript("ROKAS30_PIPOTI05_TRACK")]
public class Rokas30Pipoti05Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS30_PIPOTI05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1491.80f, 215.74f, -402.60f));

		actors.Add(AddTrackActor(character, 147392, -1510.02, 215.74, -414.29, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Treasure Chest") }));
		actors.Add(AddTrackActor(character, 57416, -721.48, 215.74, -248.68, 259, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1282.05f, 215.74f, -337.03f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 10:
				character.ServerMessage(L("A chest has appeared! Get past the Werewolf and look inside."));
				CreateBattleBoxInLayer(character, track);
				break;
			case 11:
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
