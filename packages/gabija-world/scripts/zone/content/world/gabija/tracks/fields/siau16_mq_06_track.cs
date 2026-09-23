//--- Melia Script ----------------------------------------------------------
// The Poata at the Migration Office
//--- Description -----------------------------------------------------------
// A Poata charges the settlers queuing at Orsha's Migration Office.
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

[TrackScript("SIAU16_MQ_06_TRACK")]
public class Siau16Mq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU16_MQ_06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(2041.96f, 25.35f, 318.78f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 151086, 2024.92, 25.35, 331.69, 15, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Officer Lutas"), EndPosition = new Position(1835.41f, 25.35f, 125.94f) }));
		actors.Add(AddTrackActor(character, 151081, 2012.45, 25.35, 298.29, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Settler"), EndPosition = new Position(1832.13f, 25.35f, 140.48f) }));
		actors.Add(AddTrackActor(character, 151092, 2002.78, 25.35, 273.69, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Settler"), EndPosition = new Position(1749.53f, 25.35f, 152.30f) }));
		actors.Add(AddTrackActor(character, 20062, 1990.50, 25.35, 247.65, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Settler"), EndPosition = new Position(1811.78f, 25.35f, 150.11f) }));
		actors.Add(AddTrackActor(character, 57993, 2013.32, 25.35, 791.11, 170, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(2124.69f, 25.35f, 374.74f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 49:
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);

				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Protect the people from the Poata!"), 3);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
