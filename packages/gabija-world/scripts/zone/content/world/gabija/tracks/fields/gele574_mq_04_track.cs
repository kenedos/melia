//--- Melia Script ----------------------------------------------------------
// The Nepenthes at Piene Field
//--- Description -----------------------------------------------------------
// Burning the fat wakes the sleeping plant, and its sap can be taken.
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

[TrackScript("GELE574_MQ_04_TRACK")]
public class Gele574Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("GELE574_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(2295.41, 102.65, -771.70));

		actors.Add(AddTrackActor(character, 57015, 2314.19, 102.65, -722.37, 0, new TrackActorSpec { Ai = "BasicBoss" }));
		actors.Add(character);

		return actors.ToArray();
	}
}
