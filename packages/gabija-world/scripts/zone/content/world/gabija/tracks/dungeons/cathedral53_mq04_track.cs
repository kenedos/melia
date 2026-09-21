//--- Melia Script ----------------------------------------------------------
// The candles of the Meile Oratorium
//--- Description -----------------------------------------------------------
// Maven's first secret is a count of candles, and the platform says how many.
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

[TrackScript("CHATHEDRAL53_MQ04_TRACK")]
public class Cathedral53Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHATHEDRAL53_MQ04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2292.63f, 29.94f, -57.31f));

		// The platform itself stands on the map, so only the candlesticks the
		// cutscene lights are spawned here.
		actors.Add(AddTrackActor(character, 147358, -1986.43, 0, -171.95, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Cathedral Candlestick") }));
		actors.Add(AddTrackActor(character, 147358, -2120.42, 0, -171.27, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Cathedral Candlestick") }));
		actors.Add(AddTrackActor(character, 147358, -2042.06, 0, 67.65, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Cathedral Candlestick") }));
		actors.Add(AddTrackActor(character, 147358, -2183.39, 0, 72.93, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Cathedral Candlestick") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 22:
				// The client plays a candle-lighting minigame here, which the
				// server has no equivalent for; the platform takes the count.
				character.ServerMessage(L("No candlesticks are lit. Read the platform and light the holy number of them."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
