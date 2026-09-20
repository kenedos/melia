//--- Melia Script ----------------------------------------------------------
// Mummyghast at the church gate
//--- Description -----------------------------------------------------------
// The Light Crystal's glow draws something out of the dark.
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

[TrackScript("CHAPLE576_MQ_04_TRACK")]
public class Chaple576Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHAPLE576_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1728.83f, 0.43f, 425.82f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 41372, -1050.65, -1.79, 441.78, 110, new TrackActorSpec { Ai = "BasicBoss", EndPosition = new Position(-1407.02f, 0.42f, 419.56f) }));
		actors.Add(AddTrackActor(character, 147379, -1777.94, 0.42, 425.98, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 34:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
