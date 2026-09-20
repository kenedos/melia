//--- Melia Script ----------------------------------------------------------
// The malfunctioning guardian devices
//--- Description -----------------------------------------------------------
// The two stone lanterns that keep calling guardians up.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ZACHA1F_MQ_03_TRACK")]
public class Zacha1fMq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA1F_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 47253, -1054.24, 334.83, 383.32, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Malfunctioning Guardian Device"), Level = 87, MaxHp = 25 }));
		actors.Add(AddTrackActor(character, 47253, -943.20, 334.83, 383.33, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Malfunctioning Guardian Device"), Level = 87, MaxHp = 25 }));
		actors.Add(AddTrackActor(character, 47252, -1020, 249, -477, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Defense System Manual") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
