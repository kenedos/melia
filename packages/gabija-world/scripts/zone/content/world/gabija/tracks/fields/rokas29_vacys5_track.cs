//--- Melia Script ----------------------------------------------------------
// Hogma at Apatinis Cliff
//--- Description -----------------------------------------------------------
// A Hogma patrol walks in on the last of Varkis' caches.
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

[TrackScript("ROKAS29_VACYS5_TRACK")]
public class Rokas29Vacys5Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS29_VACYS5_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-772.73f, 680.64f, -567.06f));

		actors.Add(AddTrackActor(character, 47308, -381.54, 680.64, -463.37, 76, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-669.06f, 680.65f, -502.83f) }));
		actors.Add(AddTrackActor(character, 47308, -446.31, 680.64, -454.40, 60, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-674.37f, 680.65f, -455.86f) }));
		actors.Add(AddTrackActor(character, 47308, -491.32, 680.64, -361.21, 38, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-599.85f, 680.65f, -463.85f) }));
		actors.Add(AddTrackActor(character, 47308, -418.38, 680.64, -411.07, 52, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-570.45f, 680.65f, -538.26f) }));
		actors.Add(AddTrackActor(character, 47309, -413.83, 680.64, -503.11, 56, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-621.41f, 680.65f, -552.22f) }));
		actors.Add(AddTrackActor(character, 155026, -792, 680, -567, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				character.ServerMessage(L("Monsters are attacking. Put them down quickly."));
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
