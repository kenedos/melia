//--- Melia Script ----------------------------------------------------------
// The Gargoyle of the gathering place
//--- Description -----------------------------------------------------------
// The sculpture comes off its plinth, and the Silence Scroll keeps the
// Royal Army from hearing any of it.
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

[TrackScript("FLASH64_MQ_03_TRACK")]
public class Flash64Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("FLASH64_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-141.64f, 843.06f, 2002.73f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 20025, -141.64, 843.06, 2062.56, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-137.95f, 843.06f, 2060.75f) }));
		actors.Add(AddTrackActor(character, 58045, -141.64, 843.06, 2062.56, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Gargoyle Sculpture") }));
		actors.Add(AddTrackActor(character, 57588, -141.64, 843.06, 2062.56, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Gargoyle") }));
		actors.Add(AddTrackActor(character, 57588, -141.64, 843.06, 2062.56, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Gargoyle") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 59:
				// The sculpture and the first Gargoyle the cutscene breaks,
				// before the fight is armed.
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Your surroundings become quiet with the Silence Scroll. Defeat Gargoyle!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
