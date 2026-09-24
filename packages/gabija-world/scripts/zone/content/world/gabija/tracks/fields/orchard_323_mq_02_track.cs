//--- Melia Script ----------------------------------------------------------
// The Mysterious Girl (1)
//--- Description -----------------------------------------------------------
// A girl wrapped in light flees past Luvda Cliff with a demon at her heels.
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

[TrackScript("ORCHARD_323_MQ_02_TRACK")]
public class Orchard323Mq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("ORCHARD_323_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-992.86f, 0.87f, -302.90f));

		actors.Add(AddTrackActor(character, 47236, -775.56, 0.87, -463.62, 74, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Mysterious Girl"), EndPosition = new Position(-674.73f, 0.87f, -208.90f) }));
		actors.Add(AddTrackActor(character, 58087, -865.30, -47.52, -631.69, 37, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Demon"), EndPosition = new Position(-878.38f, -8.64f, -449.80f) }));
		actors.Add(AddTrackActor(character, 58158, -926.23, -47.52, -647.64, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-689.76f, 0.87f, -124.20f) }));
		actors.Add(AddTrackActor(character, 58158, -884.92, -41.97, -679.13, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-612.89f, 0.87f, -158.82f) }));
		actors.Add(AddTrackActor(character, 58158, -836.62, -47.52, -681.58, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(-648.29f, 0.87f, -135.84f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				track.Actors[0].AttachEffect("F_buff_basic025_white_line", 2, EffectLocation.Bottom);
				break;

			case 35:
				track.Dialog.SetTitle(L("Nameless Demon"));
				track.Dialog.SetPortrait("Dlg_port_ziaurah");
				StartDialog(track, L("Chase after the girl! She must not meet with the Revelator!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
