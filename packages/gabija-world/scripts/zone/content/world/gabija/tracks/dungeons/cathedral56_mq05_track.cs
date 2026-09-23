//--- Melia Script ----------------------------------------------------------
// The candles of the Small Reception Room
//--- Description -----------------------------------------------------------
// Maven's last secret asks for the order of the candles as well as the count.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CHATHEDRAL56_MQ05_TRACK")]
public class Cathedral56Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("CHATHEDRAL56_MQ05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		var statue = AddTrackNpc(character, 153017, L("Secret Statue"), "d_cathedral_56", -222.92, 0.49, -1280.34, 0);
		statue.SetClickTrigger("DYNAMIC_DIALOG", DCathedral56QuestNpcsScript.SecretStatueDialog);
		actors.Add(statue);

		actors.Add(AddTrackActor(character, 147358, -289.02, 0, -1299, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Great Cathedral Candlestick") }));
		actors.Add(AddTrackActor(character, 147358, -289.50, 0, -1169, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Great Cathedral Candlestick") }));
		actors.Add(AddTrackActor(character, 147358, -150.89, 0, -1303.78, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Great Cathedral Candlestick") }));
		actors.Add(AddTrackActor(character, 147358, -159.56, 0, -1166, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Great Cathedral Candlestick") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				CreateBattleBoxInLayer(character, track);
				character.ServerMessage(L("Four candlesticks stand around the statue. Read the statue and light them in Maven's order."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
