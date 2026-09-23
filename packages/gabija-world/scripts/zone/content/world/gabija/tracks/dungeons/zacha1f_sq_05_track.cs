//--- Melia Script ----------------------------------------------------------
// The last Magic Regulator
//--- Description -----------------------------------------------------------
// The second regulator, and the end of the mausoleum's restraint.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ZACHA1F_SQ_05_TRACK")]
public class Zacha1fSq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA1F_SQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 47261, 1112.34, 331.87, 1408.67, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Royal Mausoleum Magic Regulator"), MaxHp = 20 }));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Zinutekas that guard the magic regulator.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("DefGroup")
			.Monster(401301, 1014.34, 331.87, 1514.71, 0, count: 3, respawnSeconds: 15)
			.Monster(401301, 1214.11, 331.87, 1517.99, 0, count: 3, respawnSeconds: 15)
			.Monster(401301, 1211.74, 331.87, 1305.81, 0, count: 3, respawnSeconds: 15)
			.Monster(401301, 1006.19, 331.87, 1302.42, 0, count: 3, respawnSeconds: 15);

		game.Start("DefGroup");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				StartMinigame(character, track);
				break;
			case 6:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
