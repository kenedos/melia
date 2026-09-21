//--- Melia Script ----------------------------------------------------------
// The golem at the forum
//--- Description -----------------------------------------------------------
// What was set to watch the sealed stone in the forum steps forward.
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

[TrackScript("FTOWER42_SQ_02_TRACK")]
public class Ftower42Sq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER42_SQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1162.12f, 71.25f, -29.97f));

		actors.Add(AddTrackActor(character, 57058, 1013.71, 71.25, 142.01, 50, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47257, 1190.06, 71.25, -23.94, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat the Golem guarding the sealed stone!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
