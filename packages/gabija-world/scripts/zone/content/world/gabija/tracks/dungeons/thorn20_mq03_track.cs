//--- Melia Script ----------------------------------------------------------
// The Merog ritual west of the camp
//--- Description -----------------------------------------------------------
// The circle is already burning when the ritual is interrupted.
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

[TrackScript("THORN20_MQ03_TRACK")]
public class Thorn20Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN20_MQ03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 153019, -812.91998, 517.96002, 966.12, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 41440, -683.27295, 518.01038, 867.73993, 215, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400381, -795.51788, 517.80212, 854.98529, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 400381, -677.62463, 518.03693, 958.28229, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 153029, -810.26697, 517.95245, 965.08899, 123, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, Name = L("Demon Summoning Circle") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_ground122_dark", 2, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_bg_smoke003", 1.4f, EffectLocation.Bottom);
				break;
			case 9:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
