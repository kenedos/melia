//--- Melia Script ----------------------------------------------------------
// Molich over the Tankinta root
//--- Description -----------------------------------------------------------
// Molich comes down the rise the moment the second root is touched.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("THORN21_MQ05_TRACK")]
public class Thorn21Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN21_MQ05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(3291.9084f, 332.3721f, 1148.6813f));

		actors.Add(AddTrackActor(character, 400421, 2469.7488, 418.3634, 1383.1254, 341, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(3112.3901f, 332.3721f, 1158.4996f) }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 153011, 3305, 332.37, 1084, 82, new TrackActorSpec { Ai = "MON_DUMMY", Name = L("Bramble's Root") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 24:
				CreateBattleBoxInLayer(character, track);
				break;
			case 29:
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
