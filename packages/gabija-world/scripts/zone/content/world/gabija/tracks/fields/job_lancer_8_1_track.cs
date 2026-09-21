//--- Melia Script ----------------------------------------------------------
// The traces in Nheto Forest
//--- Description -----------------------------------------------------------
// The grass the other lancer was searching is holding something, and what
// was watching it comes out of the treeline.
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

[TrackScript("JOB_LANCER_8_1_TRACK")]
public class JobLancer81Track : TrackScript
{
	protected override void Load()
	{
		SetId("JOB_LANCER_8_1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(154.87f, -68.53f, 14.97f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 157012, 141.93, -68.53, -2.15, 283, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Fall Grass") }));
		actors.Add(AddTrackActor(character, 46011, 151.78, -68.53, 41.71, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Bonfire") }));
		actors.Add(AddTrackActor(character, 147375, 77.22, -68.53, 51.75, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Tent") }));
		actors.Add(AddTrackActor(character, 58538, 39.51, -68.53, 260.98, 71, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(94.45f, -68.53f, 195.60f) }));
		actors.Add(AddTrackActor(character, 58539, 371.10, -70.10, 212.50, 35, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(320.08f, -68.53f, 174.55f) }));
		actors.Add(AddTrackActor(character, 58538, 290.99, -68.53, 244.98, 56, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(264.34f, -68.53f, 158.95f) }));
		actors.Add(AddTrackActor(character, 58538, 158.26, -68.53, 250.61, 50, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(168.67f, -68.53f, 180.57f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				CreateBattleBoxInLayer(character, track);
				break;

			case 1:
				character.ServerMessage(L("The grass is emitting an evil energy."));
				break;

			case 14:
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
