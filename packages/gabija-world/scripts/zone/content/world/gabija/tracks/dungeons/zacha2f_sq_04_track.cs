//--- Melia Script ----------------------------------------------------------
// The Relit Stone Lantern
//--- Description -----------------------------------------------------------
// The mausoleum's guardians come for the lantern as soon as it catches.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ZACHA2F_SQ_04_TRACK")]
public class Zacha2fSq04Track : TrackScript
{
	private const int LanternQuestId = 8436;

	protected override void Load()
	{
		SetId("ZACHA2F_SQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 47253, 2108.88, 749.95, 470.57, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, MaxHp = 120, Name = L("Royal Mausoleum Stone Lantern") }));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Echads and then the guardians that come for the relit lantern.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("DefGroup")
			.Monster(41275, 1914.3, 714.79, 251.24, 0, respawnSeconds: 20)
			.Monster(41275, 1998.97, 714.79, 235.67, 0, respawnSeconds: 20)
			.Monster(41275, 2088.55, 714.61, 248.86, 0, respawnSeconds: 20)
			.Monster(41275, 2169.88, 714.54, 249.91, 0, respawnSeconds: 20)
			.Monster(41275, 2251.26, 714.54, 249.73, 0, respawnSeconds: 20)
			.On(s => s.Elapsed >= 40, s => { s.Game.ClearStage("DefGroup"); s.Game.StartStage("stage_02"); });

		game.Stage("stage_02")
			.Monster(41277, 2277.75, 714.45, 228.17, 134)
			.Monster(401241, 2216.12, 714.47, 231.6, 109)
			.Monster(401241, 2043.99, 714.71, 237.8, 109)
			.Monster(41277, 1943.84, 714.79, 248.39, 92)
			.Monster(401241, 2117.94, 714.54, 241.36, 117)
			.On(s => s.Elapsed >= 5 && s.Alive() <= 0, s => s.Game.CompleteObjective(LanternQuestId, "lightLantern"), 1);

		game.Start("DefGroup");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				StartMinigame(character, track);
				break;
			case 8:
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
