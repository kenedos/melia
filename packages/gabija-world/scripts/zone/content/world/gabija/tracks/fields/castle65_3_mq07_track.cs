//--- Melia Script ----------------------------------------------------------
// The Barricade at the Odaginkas Vacant Lot
//--- Description -----------------------------------------------------------
// Mihail sets up a bomb at the sealed gate while the demons swarm in.
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

[TrackScript("CASTLE65_3_MQ07_TRACK")]
public class Castle653Mq07Track : TrackScript
{
	private const long QuestId = 70446;
	private const int BombSeconds = 120;

	protected override void Load()
	{
		SetId("CASTLE65_3_MQ07_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(878.26f, 0.75f, -1366.35f));

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 155094, 901.45, 0.75, -1327.67, 90, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Mihail"), Level = 100, EndPosition = new Position(1163.10f, 0.75f, -1572.72f) }));
		actors.Add(AddTrackActor(character, 47255, 590.20, 0.75, -697.66, 0, prop));
		actors.Add(AddTrackActor(character, 147379, 1197.16, 2.61, -1555.22, 2, prop));
		actors.Add(AddTrackActor(character, 47241, 550.59, 0.74, -1049.98, 0, prop));
		actors.Add(AddTrackActor(character, 47241, 587.10, 0.75, -1283.40, 0, prop));
		actors.Add(AddTrackActor(character, 47241, 583.46, 0.75, -1219.16, 0, prop));
		actors.Add(AddTrackActor(character, 47241, 615.28, 0.75, -1138.31, 0, prop));
		actors.Add(AddTrackActor(character, 147374, 575.41, 0.75, -1161.79, 0, prop));
		actors.Add(AddTrackActor(character, 47255, 142.36, 0.75, -1084.26, 0, prop));

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the demons that keep coming while Mihail sets up the bomb.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("Stage01")
			.Monster(58077, 1087.00, 0.75, -1764.91, respawnSeconds: 7)
			.Monster(58077, 972.37, 0.75, -1650.38, respawnSeconds: 7)
			.Monster(58077, 963.73, 0.75, -1507.08, respawnSeconds: 8)
			.On(s => s.Elapsed >= BombSeconds, s =>
			{
				s.Game.CompleteObjective(QuestId, "protectMihail");
				s.Game.ClearStage("Stage01");
			}, 1);

		game.Start("Stage01");

		character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Protect Mihail for two minutes while he sets up the bomb!"), 5);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 5:
				track.Actors[1].PlayEffect("I_emo_exclamation", 1f, 1, EffectLocation.Top);
				break;

			case 6:
				track.Actors[0].PlayEffect("I_emo_exclamation", 1f, 1, EffectLocation.Top);
				break;

			case 13:
				track.Actors[2].PlayEffect("F_explosion042_smoke", 1f, 1, EffectLocation.Bottom);
				break;

			case 18:
				track.Actors[9].PlayEffect("F_burstup007_smoke2", 1f, 1, EffectLocation.Bottom);
				break;

			case 26:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track, L("It won't move. It's completely stuck to the spot."));
				break;

			case 48:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track, L("I think I can break it down with a bomb. Two minutes should be enough. Please hold back the demons."));
				break;

			case 67:
				CreateBattleBoxInLayer(character, track);
				StartMinigame(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
