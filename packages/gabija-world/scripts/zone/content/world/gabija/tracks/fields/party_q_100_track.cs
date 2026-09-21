//--- Melia Script ----------------------------------------------------------
// Holding the Sealed Tower
//--- Description -----------------------------------------------------------
// The demons start on the tower itself, and it has to stand through it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("PARTY_Q_100_TRACK")]
public class PartyQ100Track : TrackScript
{
	protected override void Load()
	{
		SetId("PARTY_Q_100_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 147414, 1079, -73, 4705, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, MaxHp = 200, Name = L("Seal Tower") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				character.ServerMessage(L("Monsters started to attack the Seal Tower!"));
				break;

			case 14:
				// The client runs the defence as a minigame; the tower holds
				// with the cutscene either way.
				CreateBattleBoxInLayer(character, track);
				character.ServerMessage(L("The tower held. Report it to Priest Ramelie."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
