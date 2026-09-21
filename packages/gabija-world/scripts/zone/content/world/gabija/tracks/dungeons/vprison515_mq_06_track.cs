//--- Melia Script ----------------------------------------------------------
// The sealing of the crack
//--- Description -----------------------------------------------------------
// Six Kupoles ring the Vakarion Cathedral floor and hold the crack shut with
// the goddess standing in the middle of it.
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

[TrackScript("VPRISON515_MQ_06_TRACK")]
public class Vprison515Mq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("VPRISON515_MQ_06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-101.39f, 23.12f, -196.72f));

		actors.Add(AddTrackActor(character, 154010, -65.46, 23.12, -190.29, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Goddess Vakarine") }));
		actors.Add(AddTrackActor(character, 20026, -2.22, 23.06, -97.66, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Dimensional Crack") }));
		actors.Add(AddTrackActor(character, 154015, -212.49, 26.79, -146.71, 260, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Zydrone") }));
		actors.Add(AddTrackActor(character, 154014, 87.84, 23.12, -29.06, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Aldona") }));
		actors.Add(AddTrackActor(character, 154013, -240.99, 26.79, -42.34, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Daiva") }));
		actors.Add(AddTrackActor(character, 154012, 61.00, 26.79, 116.23, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Sigita") }));
		actors.Add(AddTrackActor(character, 154016, -197.30, 26.79, 94.58, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Medeina") }));
		actors.Add(AddTrackActor(character, 154011, -64.28, 26.79, 160.07, 120, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Kupole Audra") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20025, 62.19, 22.96, -148.69, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 49:
				// The client plays the defence as a minigame; the seal holds
				// with the cutscene either way.
				character.ServerMessage(L("The crack is holding. Stay with the goddess until it is shut."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
