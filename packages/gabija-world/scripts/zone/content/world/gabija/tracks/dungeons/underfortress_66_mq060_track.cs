//--- Melia Script ----------------------------------------------------------
// The Specter Monarch in the camp
//--- Description -----------------------------------------------------------
// The whole detachment is petrified where it stands, and what did it is
// still in the camp.
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

[TrackScript("UNDERFORTRESS_66_MQ060_TRACK")]
public class Underfortress66Mq060Track : TrackScript
{
	protected override void Load()
	{
		SetId("UNDERFORTRESS_66_MQ060_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1935.58f, 217.28f, 366.81f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 151029, 1803.60, 217.28, 262.44, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Supply Box") }));
		actors.Add(AddTrackActor(character, 151029, 1802.24, 217.28, 499.58, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Supply Box") }));
		actors.Add(AddTrackActor(character, 151029, 1887.61, 217.28, 441.21, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Supply Box") }));
		actors.Add(AddTrackActor(character, 10032, 1944.43, 217.28, 386.44, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard Delus") }));
		actors.Add(AddTrackActor(character, 10032, 1871.18, 217.28, 477.73, 9, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard") }));
		actors.Add(AddTrackActor(character, 10032, 1827.25, 217.28, 276.37, 6, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard") }));
		actors.Add(AddTrackActor(character, 10032, 1910.92, 217.28, 437.55, 14, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard") }));
		actors.Add(AddTrackActor(character, 10032, 1769.45, 217.28, 331.02, 59, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard") }));
		actors.Add(AddTrackActor(character, 10032, 1768.51, 217.28, 427.38, 20, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard") }));
		actors.Add(AddTrackActor(character, 153040, 1932.61, 217.28, 285.00, 42, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, WalkSpeed = 50, MaxHp = 200, Name = L("Amanda"), EndPosition = new Position(1518.25f, 136.59f, 384.66f) }));
		actors.Add(AddTrackActor(character, 57956, 1808.56, 217.28, 563.84, 66, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1841.57f, 217.28f, 521.39f) }));
		actors.Add(AddTrackActor(character, 57956, 1794.04, 217.28, 538.81, 66, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1763.70f, 217.28f, 494.63f) }));
		actors.Add(AddTrackActor(character, 57956, 1773.65, 217.28, 548.24, 86, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1846.74f, 217.28f, 474.72f) }));
		actors.Add(AddTrackActor(character, 57960, 1847.24, 217.28, 573.17, 93, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1901.55f, 217.28f, 476.65f) }));
		actors.Add(AddTrackActor(character, 57956, 1787.37, 217.28, 599.63, 85, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1764.17f, 217.28f, 535.25f) }));
		actors.Add(AddTrackActor(character, 57960, 1827.16, 217.28, 606.18, 135, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 57960, 1839.18, 217.28, 568.44, 88, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1896.39f, 217.28f, 527.65f) }));
		actors.Add(AddTrackActor(character, 20016, 1993.04, 217.28, 327.29, 24, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard") }));
		actors.Add(AddTrackActor(character, 147469, 1788.29, 217.28, 587.32, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Hidden Area") }));
		actors.Add(AddTrackActor(character, 154029, 1915.08, 217.28, 433.61, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Petrified Kingdom Guard") }));
		actors.Add(AddTrackActor(character, 154029, 1827.19, 217.28, 277.30, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Petrified Kingdom Guard") }));
		actors.Add(AddTrackActor(character, 154028, 1868.85, 217.28, 473.03, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Petrified Kingdom Guard") }));
		actors.Add(AddTrackActor(character, 154027, 1936.87, 217.28, 340.57, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Petrified Kingdom Guard") }));
		actors.Add(AddTrackActor(character, 154026, 1768.80, 217.28, 423.47, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Petrified Kingdom Guard") }));
		actors.Add(AddTrackActor(character, 154029, 1770.75, 217.28, 330.52, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Petrified Kingdom Guard") }));
		actors.Add(AddTrackActor(character, 154029, 1886.53, 217.28, 392.47, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Petrified Kingdom Guard") }));
		actors.Add(AddTrackActor(character, 103021, 1809.02, 217.28, 548.78, 200, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Specter Monarch") }));
		actors.Add(AddTrackActor(character, 20026, 1607.10, 160.94, 382.77, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 6:
				track.Dialog.SetTitle(L("Royal Army Guard Delus"));
				StartDialog(track, L("We appreciate your help, but we can't just ignore the royal order."));
				break;

			case 74:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat the Specter Monarch!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}

	public override void OnHandOver(Character character, Track track)
	{
		// Every guard the Monarch petrifies carries Client="BOTH", so the
		// removals only ever run from here.
		RemoveTrackActor(character, track, 3);
		RemoveTrackActor(character, track, 4);
		RemoveTrackActor(character, track, 5);
		RemoveTrackActor(character, track, 6);
		RemoveTrackActor(character, track, 7);
		RemoveTrackActor(character, track, 8);
		RemoveTrackActor(character, track, 9);
		RemoveTrackActor(character, track, 18);
	}
}
