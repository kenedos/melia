//--- Melia Script ----------------------------------------------------------
// Arrival at the West Forest Camp
//--- Description -----------------------------------------------------------
// The opening walk into the camp, where Knight Titas is first seen.
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

[TrackScript("SIAU_WEST_START_TRACK")]
public class SiauWestStartTrack : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU_WEST_START_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-576, 261, -918));
		actors.Add(character);

		var sentry = new TrackActorSpec { Name = L("Sentry"), Faction = FactionType.Neutral, Level = 1, Ai = "TrackWaitMonster" };
		actors.Add(AddTrackActor(character, 10020, -589, 261, -822, 0, sentry));
		actors.Add(AddTrackActor(character, 10020, -509, 261, -822, 0, sentry));

		actors.Add(AddTrackActor(character, 20107, -596, 261, -715, 0, new TrackActorSpec
		{
			Name = L("Knight Titas"),
			Faction = FactionType.Neutral,
			Level = 1,
			Ai = "TrackWaitMonster",
		}));

		actors.Add(AddTrackActor(character, 10020, -622, 261, -760, 0, sentry));
		actors.Add(AddTrackActor(character, 10020, -621, 261, -705, 0, sentry));
		actors.Add(AddTrackActor(character, 10033, -647, 261, -949, 1, sentry));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 64:
				character.StopLayer();
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
