//--- Melia Script ----------------------------------------------------------
// The altar at Thornbush Rest Place
//--- Description -----------------------------------------------------------
// The altar runs its purification while the corruption gathers around it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("THORN21_MQ02_TRACK")]
public class Thorn21Mq02Track : TrackScript
{
	// The purification's length is not in the client data.
	private const int AltarHoldSeconds = 60;

	protected override void Load()
	{
		SetId("THORN21_MQ02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 20026, 931.00488, 208.0713, -1253.0702, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 20026, 1050.9071, 208.0713, -1330.4659, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 20026, 1063.7241, 208.0713, -1190.1243, 17, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 46213, 1012.1256, 208.0713, -1241.4298, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, MaxHp = 100, Name = L("Altar of Purification") }));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the monsters that close in on the Altar of Purification.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("Stage_03")
			.Monster(41266, 782.7, 210.81, -985.14, -6)
			.Monster(41440, 991.67, 208.07, -1553.86, 131)
			.Monster(41268, 867.98, 208.07, -1260.05, 0)
			.Monster(41268, 1161.35, 208.07, -1201.43, 0)
			.On(s => s.Alive(0) <= 0, s => s.Spawn(0, 1))
			.On(s => s.Alive(1) <= 0, s => s.Spawn(1, 1))
			.On(s => s.Alive(2) <= 0, s => s.Spawn(2, 1))
			.On(s => s.Alive(3) <= 0, s => s.Spawn(3, 1))
			.On(s => s.Elapsed >= AltarHoldSeconds, s => { s.Game.ClearStage("Stage_03"); s.Game.CompleteObjective(20269, "holdAltar"); }, 1);

		game.Start("Stage_03");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 8:
				StartMinigame(character, track);
				break;
			case 9:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
			case 1:
				character.ServerMessage(L("Protect the Altar of Purification from the monsters."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
