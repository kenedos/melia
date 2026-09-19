//--- Melia Script ----------------------------------------------------------
// The Mushcaria at Tustinti Plateau
//--- Description -----------------------------------------------------------
// The mane that the shaman doll needs, and the beast that keeps it.
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

[TrackScript("GELE572_MQ_09_TRACK")]
public class Gele572Mq09Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE572_MQ_09_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1115.23f, 462.37f, 305.16f));

		actors.Add(AddTrackActor(character, 57072, -1136.21, 462.37, 417.13, 92, new TrackActorSpec { Ai = "TrackWaitMonster", MaxHp = 5000 }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
