//--- Melia Script ----------------------------------------------------------
// The Specter Monarch on Groundsle Hill
//--- Description -----------------------------------------------------------
// Priest Pranas is cornered at the priests' camp by a Specter Monarch.
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

[TrackScript("SIAU11RE_MQ_05_TRACK")]
public class Siau11reMq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU11RE_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-321.54f, 125.98f, 1229.54f));
		actors.Add(character);

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnVisibleName" };

		actors.Add(AddTrackActor(character, 155044, -521.98, 146.48, 1409.43, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Priest Pranas") }));
		actors.Add(AddTrackActor(character, 57997, -527.67, 146.48, 1467.92, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41294, -500.77, 146.48, 1423.05, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41294, -487.59, 146.48, 1502.24, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 41294, -576.01, 146.48, 1447.42, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 153041, -649.83, 146.48, 1493.23, 0, prop));
		actors.Add(AddTrackActor(character, 46011, -599.88, 146.48, 1493.83, 0, prop));
		actors.Add(AddTrackActor(character, 147375, -641.47, 146.48, 1569.29, 0, prop));
		actors.Add(AddTrackActor(character, 147375, -544.72, 146.48, 1575.42, 0, prop));
		actors.Add(AddTrackActor(character, 46212, -574.87, 146.48, 1512.99, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 35:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Defeat Specter Monarch and rescue Priest Pranas!"), 3);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
