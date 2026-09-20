//--- Melia Script ----------------------------------------------------------
// Crystal Wall of the Closed Area
//--- Description -----------------------------------------------------------
// The Vubbe magic stones burn out the crystal wall sealing the closed area,
// and the seal gives way.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ACT4_MINE3_ENTER_TRACK")]
public class Act4Mine3EnterTrack : TrackScript
{
	protected override void Load()
	{
		SetId("ACT4_MINE3_ENTER_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(114.4157f, 183.9297f, -159.437f));

		actors.Add(AddTrackActor(character, 151014, 129.075, 183.6362, -112.4293, 590, new TrackActorSpec { Ai = "TrackWaitMonster", Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 20024, 129.08, 183.64, -112.43, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = "UnvisibleName" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		RemoveTrackActor(character, track, 0);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 36:
				RemoveTrackActor(character, track, 0);
				break;
			case 37:
				RemoveTrackActor(character, track, 0);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
