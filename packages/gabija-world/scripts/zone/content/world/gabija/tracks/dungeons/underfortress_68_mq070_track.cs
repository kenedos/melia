//--- Melia Script ----------------------------------------------------------
// What the keeper is doing on the battlefield
//--- Description -----------------------------------------------------------
// Premier Eminent is standing among the demons giving them orders, and none
// of them is attacking him.
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

[TrackScript("UNDERFORTRESS_68_MQ070_TRACK")]
public class Underfortress68Mq070Track : TrackScript
{
	protected override void Load()
	{
		SetId("UNDERFORTRESS_68_MQ070_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(2559.92f, 444.03f, -289.02f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 153139, 2420.93, 379.67, 110.45, 35, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Premier Eminent"), EndPosition = new Position(2391.07f, 368.22f, 141.47f) }));
		actors.Add(AddTrackActor(character, 57895, 2282.39, 350.13, 166.85, 39, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(2350.17f, 363.19f, 126.05f) }));
		actors.Add(AddTrackActor(character, 57896, 2349.03, 350.13, 284.42, 50, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(2393.89f, 359.98f, 194.96f) }));
		actors.Add(AddTrackActor(character, 57895, 2297.99, 350.13, 218.64, 29, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, EndPosition = new Position(2344.26f, 351.22f, 183.70f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 20:
				track.Dialog.SetTitle(L("Grave Robber Amanda"));
				StartDialog(track, L("Premier Eminent is giving some orders to the demons."));
				break;

			case 38:
				character.ServerMessage(L("Inform Amanda what you just witnessed."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
