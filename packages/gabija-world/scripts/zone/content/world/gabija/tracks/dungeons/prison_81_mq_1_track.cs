//--- Melia Script ----------------------------------------------------------
// The Road Back
//--- Description -----------------------------------------------------------
// Demon magic rises across the Workshop and closes the way ahead.
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

[TrackScript("PRISON_81_MQ_1_TRACK")]
public class Prison81Mq1Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON_81_MQ_1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1208.52f, 168.67f, -1025.40f));

		actors.Add(AddTrackActor(character, 151107, -1171.00, 168.67, -1044.00, 65, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Zanas' Soul") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 147455, -607.00, 168.67, -734.00, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 20:
				track.Actors[2].PlayEffect("F_pattern008_violet", 1.5f);
				break;

			case 27:
				track.Actors[2].PlayEffect("F_levitation004_violet_ride", 8f);
				break;

			case 29:
				track.Actors[2].PlayEffect("F_burstup023_smoke", 2f);
				break;

			case 30:
				track.Actors[2].PlayEffect("F_smoke101_dark", 5f);
				track.Actors[2].AttachEffect("F_ground_change_dark", 26, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("I_pattern003_explosion_mash_violet", 13, EffectLocation.Bottom);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
