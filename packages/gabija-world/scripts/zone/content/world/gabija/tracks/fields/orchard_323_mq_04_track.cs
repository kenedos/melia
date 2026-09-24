//--- Melia Script ----------------------------------------------------------
// The Mysterious Girl (2)
//--- Description -----------------------------------------------------------
// The girl appears in a burst of light and drives off the ferrets
// cornering the Village Priest.
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

[TrackScript("ORCHARD_323_MQ_04_TRACK")]
public class Orchard323Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("ORCHARD_323_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-560.15f, 0.87f, -17.08f));

		actors.Add(AddTrackActor(character, 47236, -505.53, 0.87, 167.56, 220, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Mysterious Girl") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 147407, -524.22, 0.87, 177.49, 8, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Village Priest"), EndPosition = new Position(-378.93f, 0.87f, 334.46f) }));
		actors.Add(AddTrackActor(character, 57853, -617.05, 0.87, 118.94, 28, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-281.50f, 0.87f, 203.55f) }));
		actors.Add(AddTrackActor(character, 57854, -517.24, 0.87, 98.94, 30, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-287.70f, 0.87f, 202.33f) }));
		actors.Add(AddTrackActor(character, 57852, -439.85, 0.87, 159.54, 22, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-283.14f, 0.87f, 204.57f) }));
		actors.Add(AddTrackActor(character, 58043, -452.24, 0.87, 233.57, 21, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-275.78f, 0.87f, 210.92f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 26:
				track.Actors[0].PlayEffect("F_buff_basic025_white_line", 1f, 1, EffectLocation.Bottom);
				track.Actors[0].PlayEffect("F_burstup001_yellow", 1f, 1, EffectLocation.Bottom);
				break;

			case 28:
				for (var i = 3; i <= 6; i++)
					track.Actors[i].PlayEffect("F_circle020_light", 1f, 1, EffectLocation.Bottom);
				break;

			case 36:
				track.Actors[0].AttachEffect("F_buff_basic025_white_line", 2, EffectLocation.Bottom);
				break;

			case 39:
				track.Dialog.SetTitle(L("Village Priest"));
				StartDialog(track,
					L("Just now... A female child? Am I correct?"),
					L("What is that strange light... What is happening?"),
					L("...I will first return back to town and inform that the ferrets have appeared. Thanks for rescuing me.")
				);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
