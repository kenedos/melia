//--- Melia Script ----------------------------------------------------------
// Rajatadpoles at the Anga Hall
//--- Description -----------------------------------------------------------
// Rajatadpoles surround Elder Eloizard and a town youth near the Anga Hall.
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

[TrackScript("F_3CMLAKE_83_MQ_03_TRACK")]
public class F3Cmlake83Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("F_3CMLAKE_83_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-899.05f, 303.55f, -553.18f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 41308, -1054.90, 303.55, -627.09, 36, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1055.47f, 303.55f, -678.70f) }));
		actors.Add(AddTrackActor(character, 41308, -1007.82, 303.55, -732.87, 31, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41308, -941.98, 303.55, -789.36, 25, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-992.56f, 303.55f, -752.92f) }));
		actors.Add(AddTrackActor(character, 41308, -908.21, 303.55, -750.14, 16, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-935.67f, 303.55f, -717.42f) }));
		actors.Add(AddTrackActor(character, 41308, -1121.72, 303.55, -597.38, 52, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1080.19f, 303.55f, -649.83f) }));
		actors.Add(AddTrackActor(character, 41308, -1083.03, 303.55, -557.29, 24, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1050.74f, 303.55f, -590.92f) }));
		actors.Add(AddTrackActor(character, 152002, -1073.38, 303.55, -776.44, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Elder Eloizard") }));
		actors.Add(AddTrackActor(character, 147482, -1109.81, 303.55, -749.00, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Town Youth") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				CreateBattleBoxInLayer(character, track);
				break;

			case 14:
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
