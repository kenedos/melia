//--- Melia Script ----------------------------------------------------------
// The Monocle on Premier Eminent
//--- Description -----------------------------------------------------------
// What the Monocle shows standing where the keeper is standing is a Galok.
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

[TrackScript("UNDERFORTRESS_69_MQ010_TRACK")]
public class Underfortress69Mq010Track : TrackScript
{
	protected override void Load()
	{
		SetId("UNDERFORTRESS_69_MQ010_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(2192.39f, 566.38f, 16.02f));

		actors.Add(AddTrackActor(character, 153040, 2101.22, 566.82, 34.97, 71, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Amanda"), EndPosition = new Position(1989.83f, 570.36f, 17.44f) }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 153139, 1768.25, 582.42, -90.76, 20, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Premier Eminent"), EndPosition = new Position(1782.64f, 545.77f, -168.52f) }));
		actors.Add(AddTrackActor(character, 57018, 1781.92, 551.88, -150.23, 29, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral, Name = L("Premier Eminent") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 49:
				character.ServerMessage(L("The Monocle shows a demon standing where the keeper is. Tell Amanda."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
