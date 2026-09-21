//--- Melia Script ----------------------------------------------------------
// The guards on the drill ground
//--- Description -----------------------------------------------------------
// Delus and two of his men are backing up the drill ground with the Ticen
// coming up it after them.
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

[TrackScript("UNDERFORTRESS_66_MQ010_TRACK")]
public class Underfortress66Mq010Track : TrackScript
{
	protected override void Load()
	{
		SetId("UNDERFORTRESS_66_MQ010_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1291.37f, 136.59f, -200.86f));

		actors.Add(AddTrackActor(character, 153040, 1250.97, 136.59, -205.20, 70, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Amanda"), EndPosition = new Position(1233.88f, 136.59f, -95.00f) }));
		actors.Add(AddTrackActor(character, 10033, 1037.04, 136.59, -60.03, 51, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 9999, Name = L("Royal Army Guard Delus"), EndPosition = new Position(1119.03f, 136.59f, -55.11f) }));
		actors.Add(AddTrackActor(character, 10033, 1036.25, 136.59, -86.98, 64, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 9999, Name = L("Royal Army Guard"), EndPosition = new Position(1163.92f, 136.59f, -99.41f) }));
		actors.Add(AddTrackActor(character, 10033, 1056.49, 136.59, -35.01, 54, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 9999, Name = L("Royal Army Guard"), EndPosition = new Position(1142.54f, 136.59f, -33.10f) }));
		actors.Add(AddTrackActor(character, 57956, 926.11, 136.59, -44.76, 67, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1100.79f, 136.59f, -58.88f) }));
		actors.Add(AddTrackActor(character, 57956, 792.13, 136.59, -63.28, 88, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1143.83f, 136.59f, -78.96f) }));
		actors.Add(AddTrackActor(character, 57956, 911.46, 136.59, -78.33, 94, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1136.40f, 136.59f, -99.40f) }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 57956, 864.23, 136.59, -96.21, 65, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1110.45f, 136.59f, -71.67f) }));
		actors.Add(AddTrackActor(character, 57956, 890.94, 136.59, -39.66, 61, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1120.87f, 136.59f, -31.58f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 12:
				track.Dialog.SetTitle(L("Grave Robber Amanda"));
				StartDialog(track, L("Wait! Can you hear something?"));
				break;

			case 37:
				character.ServerMessage(L("The Royal Army guards are being chased by the monsters! Save them for now."));
				break;

			case 39:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
