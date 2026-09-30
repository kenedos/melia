//--- Melia Script ----------------------------------------------------------
// The first Magic Regulator
//--- Description -----------------------------------------------------------
// The activation stones turn the regulator on the Revelator.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ZACHA1F_SQ_04_TRACK")]
public class Zacha1fSq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA1F_SQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 47261, -1006, 332, 1408, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Royal Mausoleum Magic Regulator"), MaxHp = 20 }));

		return actors.ToArray();
	}

	/// <summary>
	/// Sends Vekarabes at the player from five points around the regulator, a few at a time.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("DefGroup")
			.Monster(401081, -1104.02, 332.60, 1521.03, count: 2, respawnSeconds: 10)
			.Monster(401081, -903.93, 332.60, 1515.20, count: 2, respawnSeconds: 10)
			.Monster(401081, -907.30, 332.60, 1302.04, count: 2, respawnSeconds: 10)
			.Monster(401081, -1109.72, 332.60, 1298.93, count: 2, respawnSeconds: 10)
			.Monster(401081, -1283.34, 347.83, 1458.38, count: 2, respawnSeconds: 10);

		game.Start("DefGroup");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				StartMinigame(character, track);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
