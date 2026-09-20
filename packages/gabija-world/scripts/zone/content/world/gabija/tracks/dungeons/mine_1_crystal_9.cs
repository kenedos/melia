//--- Melia Script ----------------------------------------------------------
// The Spare Purifier in District 4
//--- Description -----------------------------------------------------------
// A Bearkaras is bedded down against the spare purifier and wakes as the
// player reaches it.
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

[TrackScript("MINE_1_CRYSTAL_9_TRACK")]
public class Mine1Crystal9Track : TrackScript
{
	protected override void Load()
	{
		SetId("MINE_1_CRYSTAL_9_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1023.8325f, 32.716499f, -1093.7736f));

		actors.Add(AddTrackActor(character, 401141, 1306.8839, 32.716499, -896.01312, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 151007, 1295.7629, 32.716499, -905.01337, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 49:
				RemoveTrackActor(character, track, 1);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
