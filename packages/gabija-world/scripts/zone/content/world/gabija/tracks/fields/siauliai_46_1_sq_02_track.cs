//--- Melia Script ----------------------------------------------------------
// The Manticen at the Rankis Seal
//--- Description -----------------------------------------------------------
// The thing that pulled the tower apart comes back to see what is left.
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

[TrackScript("SIAULIAI_46_1_SQ_02_TRACK")]
public class Siauliai461Sq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAULIAI_46_1_SQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 57289, -236.99, 234.68, -1034.04, 71, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Manticen"), EndPosition = new Position(-210.16f, 234.68f, -983.49f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat the Manticen!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
