//--- Melia Script ----------------------------------------------------------
// The circle at Vapsva Vacant Lot
//--- Description -----------------------------------------------------------
// Merge comes up out of the second circle with its Tiny Mages.
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

[TrackScript("HUEVILLAGE_58_4_MQ04_TRACK")]
public class Huevillage584Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("HUEVILLAGE_58_4_MQ04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(419.7446f, -7.123497f, 672.9124f));

		actors.Add(AddTrackActor(character, 147417, 433.9258, -7.123497, 691.9702, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, Name = L("Binding Magic Circle") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 47510, 426, -6, 705, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57606, 301.5718, -6.463215, 706.8207, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57606, 523.5879, -6.094409, 657.9331, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57606, 435.9238, -6.500697, 824.3185, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("I_smoke013_dark1", 2, EffectLocation.Bottom);
				break;
			case 5:
				RemoveTrackActor(character, track, 0);
				break;
			case 9:
				track.Actors[2].AttachEffect("F_ground083_smoke", 2, EffectLocation.Bottom);
				break;
			case 15:
				track.Actors[2].AttachEffect("F_ground083_smoke", 3, EffectLocation.Bottom);
				break;
			case 29:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
