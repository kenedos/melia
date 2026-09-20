//--- Melia Script ----------------------------------------------------------
// Clymen at the stone lantern
//--- Description -----------------------------------------------------------
// A demon comes for the lantern the moment its fire is lit.
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

[TrackScript("ZACHA1F_SQ_02_TRACK")]
public class Zacha1fSq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA1F_SQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 47253, 1111, 253, -757, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Royal Mausoleum Stone Lantern") }));
		actors.Add(AddTrackActor(character, 57111, 692.36, 250.57, -991.88, 105, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1006.34f, 250.57f, -946.88f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				character.ServerMessage(L("As soon as you purified the magic, a demon appeared to stop you!"));
				break;
			case 16:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
