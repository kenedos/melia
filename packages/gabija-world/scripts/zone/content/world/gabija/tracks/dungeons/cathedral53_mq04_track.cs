//--- Melia Script ----------------------------------------------------------
// The candles of the Meile Oratorium
//--- Description -----------------------------------------------------------
// Maven's first secret is a count of candles, and the platform says how many.
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

[TrackScript("CHATHEDRAL53_MQ04_TRACK")]
public class Cathedral53Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHATHEDRAL53_MQ04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2292.63f, 29.94f, -57.31f));

		actors.Add(AddTrackActor(character, 147358, -1986.43, 0, -171.95, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Cathedral Candlestick") }));
		actors.Add(AddTrackActor(character, 147358, -2120.42, 0, -171.27, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Cathedral Candlestick") }));
		actors.Add(AddTrackActor(character, 147358, -2042.06, 0, 67.65, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Cathedral Candlestick") }));
		actors.Add(AddTrackActor(character, 147358, -2183.39, 0, 72.93, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Cathedral Candlestick") }));

		var platform = AddTrackNpc(character, 20024, L("Meile Oratorium Platform"), "d_cathedral_53", -2365.86, 29.51, -51.09, 0);
		platform.SetClickTrigger("DYNAMIC_DIALOG", DCathedral53QuestNpcsScript.MeilePlatformDialog);
		actors.Add(platform);

		actors.Add(character);

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the Loftlems and Coliflies that harry the oratorium while the candles are lit.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("Stage_01")
			.Monster(57376, -1809.14, 0, -139.79, 160)
			.Monster(57376, -1941.9, 0, 173.58, -135)
			.Monster(57376, -1878.74, 0, -317.04, 92)
			.Monster(57376, -1812.35, 0, -30.18, 172)
			.Monster(57405, -1958.05, 0, -350.52, 110)
			.Monster(57405, -2011.99, 0, 190.56, -76)
			.On(s => s.Alive(0) <= 0, s => s.Spawn(0, 1), 2)
			.On(s => s.Alive(1) <= 0, s => s.Spawn(1, 1), 2)
			.On(s => s.Alive(2) <= 0, s => s.Spawn(2, 1), 2)
			.On(s => s.Alive(3) <= 0, s => s.Spawn(3, 1), 2)
			.On(s => s.Alive(4) <= 0, s => s.Spawn(4, 1), 4)
			.On(s => s.Alive(5) <= 0, s => s.Spawn(5, 1), 4);

		game.Start("Stage_01");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 22:
				StartMinigame(character, track);
				character.ServerMessage(L("No candlesticks are lit. Read the platform and light the holy number of them."));
				break;
			case 24:
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
