//--- Melia Script ----------------------------------------------------------
// The Resounding Bombs go off
//--- Description -----------------------------------------------------------
// The guard line leaves its posts for the noise, and Amanda runs the gap.
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

[TrackScript("UNDERFORTRESS_65_MQ050_TRACK")]
public class Underfortress65Mq050Track : TrackScript
{
	protected override void Load()
	{
		SetId("UNDERFORTRESS_65_MQ050_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 153040, -532.77, 174.71, -711.39, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, MaxHp = 50, WalkSpeed = 50, Name = L("Amanda") }));
		actors.Add(AddTrackActor(character, 10032, -237.22, 327.33, -638.34, 90, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard"), EndPosition = new Position(-599.28f, 306.37f, -425.00f) }));
		actors.Add(AddTrackActor(character, 10032, -306.07, 327.33, -618.37, 86, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard"), EndPosition = new Position(-584.95f, 284.17f, -397.62f) }));
		actors.Add(AddTrackActor(character, 10032, -281.46, 327.33, -666.19, 62, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard"), EndPosition = new Position(-592.07f, 327.33f, -476.14f) }));
		actors.Add(AddTrackActor(character, 20026, 583.08, 238.80, 168.61, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 10032, 56.94, 248.80, -479.31, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Level = 200, Name = L("Royal Army Guard") }));
		actors.Add(AddTrackActor(character, 10032, 239.86, 326.16, -698.51, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Level = 200, Name = L("Royal Army Guard") }));
		actors.Add(AddTrackActor(character, 10032, 561.67, 326.16, -779.59, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Level = 200, Name = L("Royal Army Guard") }));
		actors.Add(AddTrackActor(character, 10032, 519.62, 331.58, -534.75, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Level = 200, Name = L("Royal Army Guard") }));
		actors.Add(AddTrackActor(character, 10032, 728.49, 333.25, -696.05, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Level = 200, Name = L("Royal Army Guard") }));
		actors.Add(AddTrackActor(character, 10032, -168.39, 266.12, -514.91, 74, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard"), EndPosition = new Position(-288.29f, 327.33f, -617.47f) }));
		actors.Add(AddTrackActor(character, 10032, -194.21, 276.43, -527.76, 62, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard"), EndPosition = new Position(-367.75f, 327.33f, -603.49f) }));
		actors.Add(AddTrackActor(character, 10032, -190.86, 309.74, -579.67, 46, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Royal Army Guard"), EndPosition = new Position(-329.07f, 327.33f, -644.97f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				character.ServerMessage(L("The Resounding Bomb detonated."));
				break;

			case 33:
				track.Dialog.SetTitle(L("Grave Robber Amanda"));
				StartDialog(track,
					L("We've succeeded distracting the guards."),
					L("Let's go deep inside!")
				);
				break;
		}

		await base.OnProgress(character, track, frame);
	}

	public override void OnHandOver(Character character, Track track)
	{
		// Every guard the cutscene walks off carries Client="BOTH", so the
		// removals only ever run from here.
		RemoveTrackActor(character, track, 1);
		RemoveTrackActor(character, track, 2);
		RemoveTrackActor(character, track, 3);
		RemoveTrackActor(character, track, 10);
		RemoveTrackActor(character, track, 11);
		RemoveTrackActor(character, track, 12);
	}
}
