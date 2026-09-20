//--- Melia Script ----------------------------------------------------------
// The Echad that cannot tell friend from foe
//--- Description -----------------------------------------------------------
// The Echad cut down their own before they turn on the Revelator.
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

[TrackScript("ZACHA2F_MQ_04_TRACK")]
public class Zacha2fMq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA2F_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-193.91f, 714.10f, -38.34f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 47252, -182, 711, -67, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Royal Mausoleum Tombstone") }));
		actors.Add(AddTrackActor(character, 41274, -71.47, 717.02, 66.55, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41274, -266.11, 717.02, 59.47, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41274, -188.47, 717.02, 85.38, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41275, -151.83, 717.02, 111.76, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Malfunctioning Echad") }));
		actors.Add(AddTrackActor(character, 41275, -284.90, 717.02, 92.39, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Malfunctioning Echad") }));
		actors.Add(AddTrackActor(character, 41275, -76.85, 717.02, 92.57, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Malfunctioning Echad") }));
		actors.Add(AddTrackActor(character, 41275, -206.53, 717.02, 126.13, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Malfunctioning Echad") }));
		actors.Add(AddTrackActor(character, 41275, -252.93, 713.56, 272.14, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Malfunctioning Echad") }));
		actors.Add(AddTrackActor(character, 41275, -217.10, 713.59, 291.18, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Malfunctioning Echad") }));
		actors.Add(AddTrackActor(character, 41275, -94.17, 714.08, 272.82, 5, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Malfunctioning Echad") }));
		actors.Add(AddTrackActor(character, 401241, -228.29, 713.55, 261.20, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 401241, -79.30, 714.27, 255.55, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 18:
				character.ServerMessage(L("Defeat the Echad that cannot identify friends from foes!"));
				break;
			case 24:
				// The Karas and the Vikaras are cut down by the Echad themselves.
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 12);
				RemoveTrackActor(character, track, 13);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
