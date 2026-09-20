//--- Melia Script ----------------------------------------------------------
// Rexipher takes Cyrenia Odell
//--- Description -----------------------------------------------------------
// Rexipher drops the historian's disguise at the Viesha Altar and leaves
// with Odell.
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

[TrackScript("ROKAS30_MQ8_TRACK")]
public class Rokas30Mq8Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS30_MQ8_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1362.04f, 215.75f, -534.87f));

		actors.Add(AddTrackActor(character, 147345, -1397.74, 215.75, -282.99, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Historian Cyrenia Odell"), EndPosition = new Position(-1474.23f, 215.75f, -144.99f) }));
		actors.Add(AddTrackActor(character, 47413, -1471.85, 215.75, -128.18, 26, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Rexipher") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 17:
				track.Actors[1].AttachEffect("F_burstup001_dark", 0.5f, EffectLocation.Bottom);
				track.Actors[1].AttachEffect("F_smoke017_red", 0.5f, EffectLocation.Bottom);
				break;
			case 26:
				track.Actors[1].AttachEffect("F_smoke019_dark_loop", 2.5f, EffectLocation.Bottom);
				break;
			case 49:
				track.Actors[1].AttachEffect("F_smoke005_dark", 1, EffectLocation.Bottom);
				track.Actors[1].AttachEffect("F_burstup001_dark", 0.5f, EffectLocation.Bottom);

				RemoveTrackActor(character, track, 0);
				RemoveTrackActor(character, track, 1);

				character.ServerMessage(L("Rexipher has taken Cyrenia Odell! Follow him to Zachariel Crossroads."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
