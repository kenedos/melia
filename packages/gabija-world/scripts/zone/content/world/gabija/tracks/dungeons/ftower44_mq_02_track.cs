//--- Melia Script ----------------------------------------------------------
// The magic stabilizer
//--- Description -----------------------------------------------------------
// The device comes on and the floor's Miniverns come in after the noise.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("FTOWER44_MQ_02_TRACK")]
public class Ftower44Mq02Track : TrackScript
{
	private readonly static double[,] MinivernSpots =
	{
		{ -140, 120 }, { -90, 300 }, { 10, 400 }, { 160, 430 }, { 320, 390 },
		{ 430, 260 }, { 460, 110 }, { 420, -40 }, { 300, -150 }, { 150, -190 },
		{ 0, -160 }, { -110, -50 }, { -60, 220 }, { 60, 330 }, { 250, 320 },
		{ 380, 190 }, { 390, 20 }, { 280, -90 }, { 110, -120 }, { -30, 30 },
	};

	protected override void Load()
	{
		SetId("FTOWER44_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 151050, 32, 440, 310, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Sealed Stone") }));

		// The client plays this phase as a minigame; the monsters its notice
		// describes are spawned into the track's own layer instead.
		for (var i = 0; i < MinivernSpots.GetLength(0); i++)
			actors.Add(AddTrackActor(character, 57050, MinivernSpots[i, 0], 440, MinivernSpots[i, 1], 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 4:
				character.ServerMessage(L("As the magic stabilizing device came on, the monsters rushed in!"));
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
