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
	protected override void Load()
	{
		SetId("FTOWER44_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 151050, 32, 440, 310, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Sealed Stone") }));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Minivern pairs the stabilizing device draws in.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("stage1")
			.Monster(57050, 429.03, 398.53, 99.23, 0)
			.Monster(57050, -193.5, 398.27, 107.94, 0)
			.On(s => s.Alive(0, 1) <= 0, s => { s.Spawn(0, 2); s.Spawn(1, 2); });

		game.Start("stage1");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				StartMinigame(character, track);
				break;
			case 4:
				character.ServerMessage(L("As the magic stabilizing device came on, the monsters rushed in!"));
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
