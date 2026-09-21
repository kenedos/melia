//--- Melia Script ----------------------------------------------------------
// The barrier comes down on Blut
//--- Description -----------------------------------------------------------
// Zydrone breaks her own seal so Blut can be reached, and she and Hauberk
// stay in the fight.
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

[TrackScript("VPRISON511_MQ_05_TRACK")]
public class Vprison511Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON511_MQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1741.74f, 346.41f, -489.68f));

		actors.Add(AddTrackActor(character, 154001, -1913.11, 358.05, -470.44, 12, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Sealing Barrier") }));
		actors.Add(AddTrackActor(character, 57582, -1809.31, 346.41, -484.99, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 99999, Level = 100, Name = L("Kupole Zydrone") }));
		actors.Add(AddTrackActor(character, 47514, -1968.89, 380.58, -450.47, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Demon Lord Blut") }));
		actors.Add(AddTrackActor(character, 57827, -1735.53, 346.41, -414.21, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, CombatNpc = true, MaxHp = 99999, Level = 100, Name = L("Demon Lord Hauberk") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 15:
				track.Dialog.SetTitle(L("Demon Lord Blut"));
				StartDialog(track, L("You can't trap me!"));
				break;

			case 29:
				// The barrier the cutscene tears open, before the fight is
				// armed.
				RemoveTrackActor(character, track, 0);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat Demon Lord Blut!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
