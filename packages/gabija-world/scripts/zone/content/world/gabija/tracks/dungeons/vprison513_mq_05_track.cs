//--- Melia Script ----------------------------------------------------------
// The sealing of Hauberk
//--- Description -----------------------------------------------------------
// Four Kupoles stand around the Galutin cell, and Hauberk works out what the
// whole chase was for.
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

[TrackScript("VPRISON513_MQ_05_TRACK")]
public class Vprison513Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON513_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(938.96f, -17.01f, 123.19f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 57827, 1082.31, 30.34, 298.36, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 160, Name = L("Demon Lord Hauberk") }));
		actors.Add(AddTrackActor(character, 154012, 1316.44, 91.04, 897.86, 356, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Sigita") }));
		actors.Add(AddTrackActor(character, 154014, 840.78, 30.34, 354.20, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Aldona") }));
		actors.Add(AddTrackActor(character, 154015, 1258.84, 30.34, 262.30, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Zydrone") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 18:
				track.Dialog.SetTitle(L("Demon Lord Hauberk"));
				StartDialog(track,
					L("From the beginning, all of you were the same..."),
					L("I have been fooled.")
				);
				break;

			case 46:
				track.Dialog.SetTitle(L("Demon Lord Hauberk"));
				StartDialog(track, L("My soul is mine, only mine!"));
				break;

			case 49:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Get Hauberk!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
