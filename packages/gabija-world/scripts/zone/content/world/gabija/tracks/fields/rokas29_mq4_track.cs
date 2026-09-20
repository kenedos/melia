//--- Melia Script ----------------------------------------------------------
// The Epitaph left of Dykyne Fork
//--- Description -----------------------------------------------------------
// Zinutekas close in from the rocks the moment the epitaph is touched.
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

[TrackScript("ROKAS29_MQ4_TRACK")]
public class Rokas29Mq4Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS29_MQ4_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 153053, -680, 681, 360, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Epitaph") }));
		actors.Add(AddTrackActor(character, 57777, -941.72, 681.69, 435.99, 42, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-766.76f, 681.69f, 396.70f) }));
		actors.Add(AddTrackActor(character, 57777, -914.45, 681.69, 551.68, 42, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-730.67f, 681.69f, 427.28f) }));
		actors.Add(AddTrackActor(character, 57777, -912.97, 681.69, 336.44, 24, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-809.23f, 681.69f, 355.01f) }));
		actors.Add(AddTrackActor(character, 57777, -887.25, 681.69, 311.23, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-763.92f, 681.69f, 330.61f) }));
		actors.Add(AddTrackActor(character, 57777, -643.71, 681.69, 538.24, 21, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-670.53f, 681.69f, 450.88f) }));
		actors.Add(AddTrackActor(character, 57777, -684.82, 681.69, 598.01, 28, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-679.75f, 681.69f, 477.59f) }));
		actors.Add(AddTrackActor(character, 57777, -833.02, 681.69, 596.64, 32, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-747.16f, 681.69f, 490.95f) }));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		CreateBattleBoxInLayer(character, track);
		SetTrackTendency(character, track);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				character.ServerMessage(L("Defeat the monsters that appeared near the epitaph!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
