//--- Melia Script ----------------------------------------------------------
// Kruvina and the Revelators
//--- Description -----------------------------------------------------------
// Delmore Rephaim springs his trap at the Palma Central Plaza and escapes
// with Melchioras.
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

[TrackScript("CASTLE65_2_MQ05_TRACK")]
public class Castle652Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("CASTLE65_2_MQ05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-391.62f, 183.94f, 207.34f));

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 155104, -801.40, 187.38, 148.53, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 155095, -731.33, 187.38, 137.73, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Yane"), EndPosition = new Position(-1085.77f, 187.38f, 141.42f) }));
		actors.Add(AddTrackActor(character, 155096, -727.74, 187.38, 167.10, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Connor"), EndPosition = new Position(-1085.82f, 187.38f, 127.51f) }));
		actors.Add(AddTrackActor(character, 155094, -553.35, 187.38, 145.52, 65, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Mihail"), EndPosition = new Position(-907.41f, 187.38f, 124.87f) }));
		actors.Add(AddTrackActor(character, 155101, -889.83, 187.38, 152.19, 49, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Delmore Rephaim") }));
		actors.Add(AddTrackActor(character, 155113, -411.60, 186.67, 243.66, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Mage Melchioras"), EndPosition = new Position(-758.48f, 187.38f, 146.94f) }));
		actors.Add(AddTrackActor(character, 11282, -755.92, 187.38, 104.17, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 11282, -769.20, 187.38, 199.24, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 11283, -811.22, 187.38, 97.87, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 11283, -849.74, 187.38, 178.86, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20026, -735.35, 187.38, 215.94, 0, prop));
		actors.Add(AddTrackActor(character, 20024, -802.63, 187.38, 6.54, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 2:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track, L("Stop! Everybody, step away from the device!"));
				break;

			case 6:
				track.Dialog.SetTitle(L("Revelator Yane"));
				StartDialog(track, L("Welcome, Mihail. Now let's hurry and destroy this vicious device."));
				break;

			case 7:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track, L("I know, just get away from the device, quick! Yane! Hey!"));
				break;

			case 18:
				track.Dialog.SetTitle(L("Delmore Rephaim"));
				track.Dialog.SetPortrait("Dlg_port_CastleLord");
				StartDialog(track,
					L("What a nice gesture to see you all gathered around here. Now, give everything to me!"),
					L("Give me more life... More power!")
				);
				break;

			case 22:
				track.Actors[1].AttachEffect("F_light096_red_loop", 1f, EffectLocation.Bottom);
				track.Actors[1].AttachEffect("F_pattern013_ground", 4.5f, EffectLocation.Bottom);
				track.Actors[12].AttachEffect("F_smoke145_dark_ground_loop2", 7f, EffectLocation.Bottom);
				break;

			case 35:
				track.Actors[6].AttachEffect("F_levitation005_dark_blue", 2f, EffectLocation.Middle);
				track.Dialog.SetTitle(L("Mage Melchioras"));
				track.Dialog.SetPortrait(null);
				StartDialog(track,
					L("No!! How could this happen..."),
					L("I'll take down the magic of the device even if I have to give my whole power and life for it! Please save me!")
				);
				break;

			case 42:
				track.Actors[6].AttachEffect("F_light080_blue_loop", 1f, EffectLocation.Middle);
				track.Dialog.SetTitle(L("Delmore Rephaim"));
				track.Dialog.SetPortrait("Dlg_port_CastleLord");
				StartDialog(track,
					L("Ah... are you Melchioras, the traitor? I have to thank you."),
					L("You and Revelator Yane there as well... You have no idea how fun it was to watch you from above...")
				);
				break;

			case 61:
				track.Actors[1].PlayEffect("F_explosion069_blue", 3f, 1, EffectLocation.Bottom);
				break;

			case 79:
				track.Dialog.SetTitle(L("Delmore Rephaim"));
				track.Dialog.SetPortrait("Dlg_port_CastleLord");
				StartDialog(track,
					L("What are you doing, Melchioras!! Go hide in a corner somewhere like the coward you are!!"),
					L("Alright... Let's see just how bravely you can destroy it. I'll give you a taste of how the Kruvina is really used!")
				);
				break;

			case 105:
				// Rephaim and Melchioras vanish on Client="BOTH" rows the client never reports.
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);

				track.Dialog.SetTitle(L("Revelator Mihail"));
				track.Dialog.SetPortrait(null);
				StartDialog(track, L("Is everyone alright?"));
				break;

			case 106:
				track.Dialog.SetTitle(L("Revelator Yane"));
				StartDialog(track,
					L("What have I done... It was all my fault... I was so careless..."),
					L("Melchioras is in danger. I'll save him!")
				);
				break;

			case 109:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track, L("Yane! Wait!"));
				break;

			case 111:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track, L("We should go after her. Those who are injured should stay here and take care of the other demons."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
