//--- Melia Script ----------------------------------------------------------
// The Minotaur on Greate Stone Face Hill
//--- Description -----------------------------------------------------------
// A Minotaur tears through the camp the priests abandoned.
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

[TrackScript("SIAU15RE_MQ_05_TRACK")]
public class Siau15reMq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU15RE_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2895.10f, 850.46f, 446.81f));
		actors.Add(character);

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(AddTrackActor(character, 57995, -3007.42, 864.42, 905.02, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2976.93f, 864.42f, 845.44f) }));
		actors.Add(AddTrackActor(character, 147375, -3079.45, 864.42, 745.49, 0, prop));
		actors.Add(AddTrackActor(character, 147375, -3010.63, 864.42, 913.34, 0, prop));
		actors.Add(AddTrackActor(character, 46011, -2916.68, 864.42, 812.68, 0, prop));
		actors.Add(AddTrackActor(character, 58009, -3030.82, 864.42, 755.69, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-3049.21f, 932.58f, 462.80f) }));
		actors.Add(AddTrackActor(character, 147322, -3040.68, 864.42, 818.55, 0, prop));
		actors.Add(AddTrackActor(character, 58009, -3020.83, 864.42, 796.29, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2927.18f, 864.42f, 503.76f) }));
		actors.Add(AddTrackActor(character, 58009, -2967.59, 864.42, 825.03, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2824.59f, 864.42f, 599.67f) }));
		actors.Add(AddTrackActor(character, 58009, -3122.52, 864.42, 859.40, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-3231.70f, 894.99f, 718.63f) }));
		actors.Add(AddTrackActor(character, 58010, -2937.01, 864.42, 926.09, 52, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2822.69f, 959.37f, 1077.37f) }));
		actors.Add(AddTrackActor(character, 58010, -3002.03, 864.42, 881.46, 32, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 58010, -2908.42, 864.42, 848.54, 322, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-2527.45f, 673.46f, 726.86f) }));
		actors.Add(AddTrackActor(character, 147312, -3007.42, 864.42, 905.02, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Unknown Diary") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 45:
				// The fleeing Popolions and Pokubus and the torn tent leave on Client="BOTH" rows.
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 7);
				RemoveTrackActor(character, track, 8);
				RemoveTrackActor(character, track, 9);
				RemoveTrackActor(character, track, 10);
				RemoveTrackActor(character, track, 11);
				RemoveTrackActor(character, track, 12);

				character.AddonMessage(AddonMessage.NOTICE_Dm_Scroll, L("Something has fallen from the tent that the Minotaur had destroyed.{nl}Defeat Minotaur and check what has fallen down!"), 5);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
