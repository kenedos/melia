//--- Melia Script ----------------------------------------------------------
// Maven's Verification Test
//--- Description -----------------------------------------------------------
// The Priest of Evidence tests the Revelator before the room with the revelation.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CHATHEDRAL54_MQ05_PART3_TRACK")]
public class Cathedral54Mq05Part3Track : TrackScript
{
	private const int TestQuestId = 20340;

	// The HP lock's threshold is not in the client data.
	private const float TestPassedHpRate = 0.1f;

	protected override void Load()
	{
		SetId("CHATHEDRAL54_MQ05_PART3_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 57223, 1549.51, 0.19, -2016.26, 0, new TrackActorSpec { Ai = "BasicBoss", Name = L("Priest of Evidence"), Level = 145 }));
		actors.Add(AddTrackActor(character, 47254, 1514.68, 0, -1900.88, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Maven's Message") }));
		actors.Add(AddTrackActor(character, 147350, 1580, 0.19, -1857, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 153028, 1584.30, 0.19, -1864.70, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	/// <summary>
	/// Passes the test once the Priest of Evidence is worn down to his HP lock.
	/// </summary>
	private static void WatchThePriest(Character character, Track track)
	{
		if (track.Actors[0] is not Mob priest)
			return;

		var game = new TrackMinigame(character, track);

		game.Stage("test")
			.On(s => priest.IsDead || priest.Hp <= priest.MaxHp * TestPassedHpRate, s =>
			{
				priest.Map?.RemoveMonster(priest);
				s.Game.Character.ServerMessage(L("The Priest of Evidence lowers his weapon. The test is passed."));
				s.Game.CompleteObjective(TestQuestId, "passTheTest");
			}, 1);

		game.Start("test");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 4:
				track.Dialog.SetTitle(L("Maven's Message"));
				StartDialog(track,
					L("Are you the Revelator of the goddesses or the one who belongs to the mighty power of the darkness?"),
					L("The evil darkness can not cross here.")
				);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				WatchThePriest(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
