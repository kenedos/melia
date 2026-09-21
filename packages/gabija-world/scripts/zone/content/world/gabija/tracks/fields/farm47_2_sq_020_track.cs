//--- Melia Script ----------------------------------------------------------
// The chest in the field
//--- Description -----------------------------------------------------------
// Reaching for the lid brings the field's monsters in, and the chest has to
// be broken open instead.
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

[TrackScript("FARM47_2_SQ_020_TRACK")]
public class Farm472Sq020Track : TrackScript
{
	protected override void Load()
	{
		SetId("FARM47_2_SQ_020_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(903.70f, 72.61f, -893.32f));

		actors.Add(AddTrackActor(character, 152019, 925.30, 72.60, -850.04, 0, new TrackActorSpec { Ai = "TrackWaitMonster", MaxHp = 20, Name = L("Old Chest") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 2:
				// The client plays the break-in as a minigame; the fight the
				// box arms is what the server owes it.
				character.ServerMessage(L("Fight the monsters off and break the chest open!"));
				break;

			case 4:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
