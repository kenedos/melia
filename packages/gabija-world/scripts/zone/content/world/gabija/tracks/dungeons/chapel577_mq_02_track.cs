//--- Melia Script ----------------------------------------------------------
// Recapturing the Bell Tower
//--- Description -----------------------------------------------------------
// The Necroventer holds the tower. Take it back.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CHAPLE577_MQ_02_TRACK")]
public class Chaple577Mq02Track : TrackScript
{
	/// <summary>
	/// The model the cutscene puts Follower Algis on screen as.
	/// </summary>
	private const int AlgisModelId = 11281;

	/// <summary>
	/// Where Algis sits in the cutscene's cast, which is what the hand-over
	/// uses to take him off again.
	/// </summary>
	private const int AlgisActorIndex = 3;

	protected override void Load()
	{
		SetId("CHAPLE577_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-633.69f, 35.92f, -962.92f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147352, 134, 165, -576, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 147358, -30.72, 35.93, -165.28, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Central Altar") }));

		// Algis walks into the tower with the character rather than standing
		// where the cutscene put him, and fights what comes at them on the way.
		// He is only here for the cutscene, so he is never given one outside it.
		var algis = AddTrackActor(character, AlgisModelId, -633, 36, -934, 90, new TrackActorSpec
		{
			Name = L("Follower Algis"),
			Faction = FactionType.Our_Forces,
			MaxHp = 9999,
			Level = 41,
			WalkSpeed = 70,
		});

		if (algis != null)
			QuestAlly.MakeAlly(algis, character);

		actors.Add(algis);
		actors.Add(AddTrackActor(character, 152003, 207.31, 164.86, -582.23, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	/// <summary>
	/// Takes Algis off with the cutscene, so he does not linger in the layer
	/// once it is over.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="track"></param>
	public override void OnHandOver(Character character, Track track)
	{
		base.OnHandOver(character, track);

		RemoveTrackActor(character, track, AlgisActorIndex);
	}

	/// <summary>
	/// Raises Necroventer beside the central altar.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("DefGroup")
			.Monster(41230, -44.85, 164.86, -624.79, -40, aggressive: false);

		game.Start("DefGroup");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				StartMinigame(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
