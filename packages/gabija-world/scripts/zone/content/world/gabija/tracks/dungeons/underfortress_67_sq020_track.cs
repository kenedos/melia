//--- Melia Script ----------------------------------------------------------
// What came out of the soldier's grave
//--- Description -----------------------------------------------------------
// A Necroventer comes for the spirit rather than for the Revelator.
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

[TrackScript("UNDERFORTRESS_67_SQ020_TRACK")]
public class Underfortress67Sq020Track : TrackScript
{
	protected override void Load()
	{
		SetId("UNDERFORTRESS_67_SQ020_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1754.19f, 370.92f, 767.45f));

		actors.Add(AddTrackActor(character, 47170, 1769.67, 370.92, 778.65, 154, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Soldier's Grave") }));
		actors.Add(AddTrackActor(character, 57408, 1779.95, 370.92, 797.73, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Necroventer") }));
		actors.Add(AddTrackActor(character, 11283, 1776.07, 370.92, 802.04, 1, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Resentful Spirit") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 45:
				// The spirit the Necroventer drives off, before the fight is
				// armed.
				RemoveTrackActor(character, track, 2);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Put the Necroventer down before it takes the spirit!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
