//--- Melia Script ----------------------------------------------------------
// The Silva Griffin of the Grynas Trail
//--- Description -----------------------------------------------------------
// The Murmillo Master's simple task is standing at the end of the trail.
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

[TrackScript("JOB_MURMILLO_8_1_TRACK")]
public class JobMurmillo81Track : TrackScript
{
	protected override void Load()
	{
		SetId("JOB_MURMILLO_8_1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(40.85f, 200.31f, 958.62f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 107017, -125.70, 200.31, 1171.06, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Silva Griffin") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				CreateBattleBoxInLayer(character, track);
				break;

			case 6:
				character.ServerMessage(L("You have found the Silva Griffin that the Murmillo Master was talking about!"));
				break;

			case 22:
				SetTrackTendency(character, track);
				break;

			case 24:
				character.ServerMessage(L("Defeat the Silva Griffin and prove your skills."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
