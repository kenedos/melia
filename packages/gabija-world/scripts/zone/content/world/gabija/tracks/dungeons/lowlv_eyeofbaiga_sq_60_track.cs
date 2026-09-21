//--- Melia Script ----------------------------------------------------------
// The portal the tracker found
//--- Description -----------------------------------------------------------
// A Gazing Golem steps out of the portal the Linker Master's device was
// pointed at.
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

[TrackScript("LOWLV_EYEOFBAIGA_SQ_60_TRACK")]
public class LowlvEyeofbaigaSq60Track : TrackScript
{
	protected override void Load()
	{
		SetId("LOWLV_EYEOFBAIGA_SQ_60_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(250.72f, 346.15f, 1430.11f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 57454, 581.90, 346.15, 1446.96, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Gazing Golem") }));
		actors.Add(AddTrackActor(character, 20026, 627.79, 346.15, 1449.06, 64, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				character.ServerMessage(L("A Gazing Golem has appeared."));
				break;

			case 24:
				// The portal the Golem came through closes with the cutscene.
				RemoveTrackActor(character, track, 2);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
