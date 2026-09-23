//--- Melia Script ----------------------------------------------------------
// The Gray Golem at Deer Hooves Lot
//--- Description -----------------------------------------------------------
// The planned settler camp turns out to be a Gray Golem's ground.
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

[TrackScript("SIAU11RE_SQ_04_TRACK")]
public class Siau11reSq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU11RE_SQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1321.54f, 109.64f, -1329.39f));
		actors.Add(character);

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnVisibleName" };

		actors.Add(AddTrackActor(character, 153041, 1383.23, 109.64, -1305.35, 0, prop));
		actors.Add(AddTrackActor(character, 153041, 1375.87, 109.64, -1320.07, 0, prop));
		actors.Add(AddTrackActor(character, 147375, 1447.57, 109.64, -1330.53, 0, prop));
		actors.Add(AddTrackActor(character, 47161, 1316.52, 109.64, -1300.67, 0, prop));
		actors.Add(AddTrackActor(character, 46011, 1357.33, 109.64, -1374.31, 0, prop));
		actors.Add(AddTrackActor(character, 57998, 1448.09, 109.64, -1500.67, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 103028, 1448.09, 109.64, -1500.67, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 20024, 1448.09, 109.64, -1500.67, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 40:
				// The staged golem leaves on a Client="BOTH" row.
				RemoveTrackActor(character, track, 7);

				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Defeat the Grey Golem that suddenly appeared!"), 3);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
