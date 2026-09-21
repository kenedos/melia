//--- Melia Script ----------------------------------------------------------
// The last monument
//--- Description -----------------------------------------------------------
// What was guarding the Commanding Monument comes out of the stone.
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

[TrackScript("REMAINS40_MQ_07_TRACK")]
public class Remains40Mq07Track : TrackScript
{
	protected override void Load()
	{
		SetId("REMAINS40_MQ_07_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(3632.71f, 645.74f, 2620f));

		actors.Add(AddTrackActor(character, 41385, 3534.65, 645.74, 2723.86, 6, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(3600.07f, 645.74f, 2650.92f) }));
		actors.Add(AddTrackActor(character, 20026, 3529.92, 645.74, 2735.93, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 33:
				CreateBattleBoxInLayer(character, track);
				break;
			case 34:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
