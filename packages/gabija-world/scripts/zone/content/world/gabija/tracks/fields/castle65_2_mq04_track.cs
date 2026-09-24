//--- Melia Script ----------------------------------------------------------
// The Device on the Handicraft Workshop Road
//--- Description -----------------------------------------------------------
// The crystal reveals a hidden device, and Yane arrives with news of a
// way to destroy the Kruvina.
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

[TrackScript("CASTLE65_2_MQ04_TRACK")]
public class Castle652Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("CASTLE65_2_MQ04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(496.04f, 104.29f, 1254.04f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 155105, 433.11, 104.29, 1283.32, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 155095, 512.54, 104.29, 965.97, 67, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Yane"), EndPosition = new Position(508.46f, 102.59f, 811.79f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Clear, L("A hidden Demonic Power Supply Device has shown up!"), 3);
				track.Actors[1].PlayEffect("F_lineup009_ground", 2f, 1, EffectLocation.Bottom);
				break;

			case 14:
				track.Dialog.SetTitle(L("Revelator Yane"));
				StartDialog(track,
					L("Oh... you're that new Revelator from earlier. You're helping out Mihail and Melchioras, are you?"),
					L("That's good. The information you found is important, you should let them know, too."),
					L("Looking through a suspicious area we found a secret document hidden by Lord Delmore. We also found a way to destroy the Kruvina device."),
					L("We're all set here... Go and tell Mihail to come meet us by the device at the Palma Plaza."),
					L("We're going to join forces and strike at once. You join in too, if you can.")
				);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
