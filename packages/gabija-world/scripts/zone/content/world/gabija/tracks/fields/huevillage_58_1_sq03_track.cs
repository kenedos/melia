//--- Melia Script ----------------------------------------------------------
// Merregina in the Holy Pond
//--- Description -----------------------------------------------------------
// The corruption under the water takes a shape of its own.
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

[TrackScript("HUEVILLAGE_58_1_SQ03_TRACK")]
public class Huevillage581Sq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_1_SQ03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-655.7644f, 230.98869f, 900.77814f));

		actors.Add(AddTrackActor(character, 57031, -981.45374, 230.9787, 1046.6323, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20026, -985, 231, 1035, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[2].AttachEffect("F_smoke043_loop", 10, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("F_ground100_green", 7, EffectLocation.Bottom);
				break;
			case 29:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
