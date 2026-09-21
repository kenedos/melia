//--- Melia Script ----------------------------------------------------------
// The Archon of the fusion room
//--- Description -----------------------------------------------------------
// The machine's keeper comes down on whoever opens it.
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

[TrackScript("FTOWER42_SQ_06_TRACK")]
public class Ftower42Sq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("FTOWER42_SQ_06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1357.58f, 8.35f, -2365.59f));

		actors.Add(AddTrackActor(character, 147307, -1386.18, 8.36, -2368.94, 21, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Flame Fusion Machine") }));
		actors.Add(AddTrackActor(character, 57096, -1239.54, 8.61, -2127.06, 13, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 54:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				character.ServerMessage(L("Defeat the Archon guarding the Flame Fusion Machine!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
