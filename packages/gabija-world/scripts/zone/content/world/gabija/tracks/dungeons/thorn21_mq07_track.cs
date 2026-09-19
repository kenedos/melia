//--- Melia Script ----------------------------------------------------------
// Bramble in Giliaii Courtyard
//--- Description -----------------------------------------------------------
// The Demon Lord and the revelation it took, at the end of the Thorn Forest.
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

[TrackScript("THORN21_MQ07_TRACK")]
public class Thorn21Mq07Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN21_MQ07_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(5471.6978f, 333.2023f, -191.21075f));

		actors.Add(AddTrackActor(character, 400901, 5924.4668, 333.2023, -196.01424, 45, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47234, 5984.5435, 333.21231, -202.09409, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Name = L("Revelation") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 12080, 5447.1763, 333.21231, -197.30769, 60, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[1].AttachEffect("F_cleric_melstis_loop_ground", 2, EffectLocation.Bottom);
				break;
			case 25:
				character.ServerMessage(L("Cross here after drinking the Enhanced Thorn Flower Stimulant!"));
				break;
			case 26:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
