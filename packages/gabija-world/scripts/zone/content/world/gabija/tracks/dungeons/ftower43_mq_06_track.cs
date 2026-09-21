//--- Melia Script ----------------------------------------------------------
// The second control valve
//--- Description -----------------------------------------------------------
// Antares opens the wrong line and the valve takes him with it.
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

[TrackScript("FTOWER43_MQ_06_TRACK")]
public class Ftower43Mq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER43_MQ_06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1249.09f, 432.74f, -785.09f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147504, 1447.05, 432.75, -788.39, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Magic Control Valve") }));
		actors.Add(AddTrackActor(character, 151005, 1454.39, 432.74, -803.32, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Antares") }));
		actors.Add(AddTrackActor(character, 147449, 1142.73, 389.88, -790.18, 51, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Grita"), EndPosition = new Position(1348.34f, 432.74f, -799.55f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 5:
				track.Dialog.SetTitle(L("Antares"));
				StartDialog(track, L("When I release this valve, my grand experiment will be complete and you'll see a most spectacular sight!"));
				break;
			case 28:
				RemoveTrackActor(character, track, 1);
				break;
			case 30:
				RemoveTrackActor(character, track, 2);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
