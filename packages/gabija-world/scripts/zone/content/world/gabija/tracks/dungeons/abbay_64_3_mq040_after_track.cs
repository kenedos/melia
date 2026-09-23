//--- Melia Script ----------------------------------------------------------
// Rose Freed
//--- Description -----------------------------------------------------------
// The wizard's ritual breaks, the wizard vanishes and Edmundas runs to
// his sister.
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

[TrackScript("ABBAY_64_3_MQ040_AFTER_TRACK")]
public class Abbay643Mq040AfterTrack : TrackScript
{
	protected override void Load()
	{
		SetId("ABBAY_64_3_MQ040_AFTER_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(AddTrackActor(character, 153119, -1459, 622.42, 175, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Rose") }));
		actors.Add(AddTrackActor(character, 153110, -1548.75, 616.52, -192.70, 62, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Edmundas"), EndPosition = new Position(-1461f, 622.42f, 142f) }));
		actors.Add(AddTrackActor(character, 47123, -1459, 622.42, 175, 0, prop));
		actors.Add(AddTrackActor(character, 153120, -1454.40, 622.42, 244.96, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Mysterious Wizard") }));
		actors.Add(AddTrackActor(character, 47254, -1555, 621, 268, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[2].AttachEffect("F_levitation032_red", 0.8f, EffectLocation.Bottom);
				break;

			case 2:
				track.Actors[3].PlayEffect("F_smoke027_dark", 1f, 1, EffectLocation.Bottom);
				break;

			case 4:
				track.Actors[3].PlayEffect("F_smoke072_sviolet", 1f, 1, EffectLocation.Bottom);
				break;

			case 7:
				// The wizard vanishes on a Client="BOTH" row the client never reports.
				RemoveTrackActor(character, track, 3);
				track.Actors[2].AttachEffect("I_smoke008_red_noloop", 1, EffectLocation.Bottom);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
