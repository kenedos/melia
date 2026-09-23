//--- Melia Script ----------------------------------------------------------
// Writing the Demon Transformation Scroll
//--- Description -----------------------------------------------------------
// The Altar of Intelligence writes the scroll by itself, if the demons are
// kept off it long enough.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CHATHEDRAL56_MQ02_TRACK")]
public class Cathedral56Mq02Track : TrackScript
{
	private readonly static QuestId Mq02 = new QuestId(20330);

	protected override void Load()
	{
		SetId("CHATHEDRAL56_MQ02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1458.91f, 0.50f, -472.21f));

		actors.Add(AddTrackActor(character, 151024, 1414, 0.50, -471, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, MaxHp = 120, Name = L("Altar of Intelligence") }));
		actors.Add(character);

		return actors.ToArray();
	}

	/// <summary>
	/// Starts the waves of Naktis' servants that attack the altar while the scroll is written.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);

		game.Stage("MON_GEN")
			.Monster(57367, 1731.28, 0.5, -375.89, -49, respawnSeconds: 20)
			.Monster(57367, 1699.78, 0.5, -543.4, -77, respawnSeconds: 20)
			.Monster(57371, 1685.52, 0.5, -464.69, 176, respawnSeconds: 20)
			.On(s => s.Elapsed >= 20, s => s.Game.StartStage("MON_GEN02"));

		game.Stage("MON_GEN02")
			.Monster(57371, 1681.67, 0.5, -560.44, -143, respawnSeconds: 20)
			.Monster(57371, 1698.19, 0.5, -385.06, -149, respawnSeconds: 20)
			.Monster(57370, 1695.82, 0.5, -476.96, -163, respawnSeconds: 20)
			.Monster(57370, 1796.84, 0.5, -453.64, -152, respawnSeconds: 20)
			.On(s => s.Elapsed >= 40, s => s.Game.StartStage("MON_GEN03"));

		game.Stage("MON_GEN03")
			.Monster(57367, 1787.7, 0.5, -465.31, -170)
			.Monster(57371, 1705.33, 0.5, -461.58, 179)
			.Monster(57371, 1725.01, 0.5, -501.57, 178)
			.Monster(57370, 1712.32, 0.5, -553.04, 158)
			.Monster(57370, 1753.47, 0.5, -393.59, 173)
			.On(s => s.Elapsed >= 5 && s.Alive() <= 0, s => { s.Game.Character.ServerMessage(L("The scroll has finished writing itself.")); s.Game.CompleteObjective(Mq02, "completeTheScroll"); }, 1);

		game.Start("MON_GEN");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 2:
				character.ServerMessage(L("Defend against attacks from Naktis' servants until the transformation scroll is complete!"));
				break;

			case 7:
				StartMinigame(character, track);
				break;
			case 9:
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
