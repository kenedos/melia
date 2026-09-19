//--- Melia Script ----------------------------------------------------------
// The circle at Drugys Courtyard
//--- Description -----------------------------------------------------------
// Mothstem rises out of the circle with the Carcashu that feed it.
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

[TrackScript("HUEVILLAGE_58_4_MQ03_TRACK")]
public class Huevillage584Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_4_MQ03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-853.85791f, 107.0208f, -724.13483f));

		actors.Add(AddTrackActor(character, 147417, -879, 107, -737, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, Name = L("Binding Magic Circle") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 47320, -880.67719, 107.0208, -736.62323, 30, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57029, -945.01862, 107.0208, -629.69678, 22, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57029, -984.18121, 107.0208, -706.78961, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57029, -951.66699, 107.0208, -785.26941, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57029, -864.32813, 107.0208, -815.25787, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57029, -932.52539, 107.0208, -684.44812, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57029, -973.00098, 107.0208, -845.60712, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 6:
				track.Actors[0].AttachEffect("I_smoke013_dark1", 2, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_smoke004", 1.3f, EffectLocation.Bottom);
				break;
			case 8:
				RemoveTrackActor(character, track, 0);
				break;
			case 28:
				track.Actors[2].AttachEffect("F_ground083_smoke", 3, EffectLocation.Bottom);
				break;
			case 34:
				track.Actors[2].AttachEffect("F_ground083_smoke#Dummy_emitter", 4, EffectLocation.Bottom);
				break;
			case 40:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
