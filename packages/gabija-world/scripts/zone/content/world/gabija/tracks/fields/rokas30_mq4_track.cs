//--- Melia Script ----------------------------------------------------------
// Rexipher at the Chesed Altar
//--- Description -----------------------------------------------------------
// Rexipher sets the Hogma on the altar and walks away from the fight.
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

[TrackScript("ROKAS30_MQ4_TRACK")]
public class Rokas30Mq4Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS30_MQ4_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(510.46f, 371.96f, 818.11f));

		actors.Add(AddTrackActor(character, 47413, 536.66, 402.12, 1138.97, 6, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Rexipher"), EndPosition = new Position(553.21f, 402.11f, 1218.77f) }));
		actors.Add(AddTrackActor(character, 41433, 528.01, 402.11, 1077.82, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 41433, 440.07, 402.11, 1089.49, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41433, 604.60, 402.11, 1057.62, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47308, 432.72, 402.11, 1017.02, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47308, 626.76, 402.11, 973.48, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 9:
				track.Actors[0].AttachEffect("buff_icon_dark", 2, EffectLocation.Bottom);
				break;
			case 19:
				// Rexipher fades out of the scene rather than joining the fight.
				RemoveTrackActor(character, track, 0);

				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
