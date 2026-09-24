//--- Melia Script ----------------------------------------------------------
// Unfortunate Distrust
//--- Description -----------------------------------------------------------
// Yane finds the Kruvina device still shielded and parts ways with
// Melchioras at the manor entrance.
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

[TrackScript("CASTLE65_2_MQ01_TRACK")]
public class Castle652Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("CASTLE65_2_MQ01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1857.42f, 1.22f, -154.05f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 155095, 1241.98, -10.52, -269.36, 87, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Yane"), EndPosition = new Position(957.89f, 22.99f, -98.61f) }));
		actors.Add(AddTrackActor(character, 155096, 1251.39, -10.52, -210.06, 2, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Connor"), EndPosition = new Position(933.40f, 34.53f, -118.81f) }));
		actors.Add(AddTrackActor(character, 155113, 1149.07, -10.52, -281.30, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Mage Melchioras") }));
		actors.Add(AddTrackActor(character, 155094, 1188.85, -10.52, -350.10, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Mihail") }));
		actors.Add(AddTrackActor(character, 11282, 1188.49, -10.52, -156.07, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(887.61f, 32.09f, -112.52f) }));
		actors.Add(AddTrackActor(character, 11282, 1276.15, -10.52, -173.77, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(821.26f, 36.24f, -85.73f) }));
		actors.Add(AddTrackActor(character, 11283, 1144.92, -10.52, -161.95, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(889.60f, 32.09f, -107.67f) }));
		actors.Add(AddTrackActor(character, 11283, 1324.32, -10.52, -245.44, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(824.45f, 34.86f, -111.94f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 22:
				track.Dialog.SetTitle(L("Revelator Yane"));
				StartDialog(track,
					L("Wait, how did this happen? I thought you said the magic supply device had been destroyed!"),
					L("Isn't the protective shield around the Kruvina device still intact?")
				);
				break;

			case 24:
				track.Dialog.SetTitle(L("Mage Melchioras"));
				StartDialog(track,
					L("That's impossible... I was sure it would disappear..."),
					L("Wait a second! This can only mean Delmore Rephaim changed the device's layout after I left!")
				);
				break;

			case 26:
				track.Dialog.SetTitle(L("Revelator Yane"));
				StartDialog(track,
					L("Could it be? So you too were a pawn of that Lord Delmore..."),
					L("What you know is useless and now I'm not even sure I believe you. We need to find a way to stop this ourselves.")
				);
				break;

			case 28:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track, L("Yane. Wait. Wouldn't it be better to help Melchioras first and then find a new way to solve this?"));
				break;

			case 30:
				track.Dialog.SetTitle(L("Revelator Yane"));
				StartDialog(track, L("You will help Melchioras then. As far as I can tell it's time for us to act separately."));
				break;

			case 32:
				track.Dialog.SetTitle(L("Revelator Yane"));
				StartDialog(track, L("Alright everybody, let's go. We need to find a way to destroy the Kruvina device together."));
				break;

			case 39:
				track.Dialog.SetTitle(L("Mage Melchioras"));
				StartDialog(track, L("I was so oblivious. To think that Delmore Rephaim had anything to do with it..."));
				break;

			case 40:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track, L("I know what Yane is like. In a situation like this she's definitely going to try and destroy the device by force."));
				break;

			case 42:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track, L("I don't think our words can convince her. Melchioras, we need to find another way before Yane puts herself out there."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
