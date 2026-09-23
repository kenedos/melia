//--- Melia Script ----------------------------------------------------------
// The magic control room
//--- Description -----------------------------------------------------------
// Grita works the control circle while the Miniverns come for her.
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

[TrackScript("FTOWER44_MQ_04_TRACK")]
public class Ftower44Mq04Track : TrackScript
{
	private readonly static double[,] MinivernSpots =
	{
		{ -1604.69, 813.69 }, { -1699.47, 709.76 }, { -1497.40, 682.54 },
		{ -1712.98, 566.97 }, { -1530.36, 587.96 },
	};

	private readonly static double[] MinivernDirections = { 18, 0, 0, 0, 0 };

	protected override void Load()
	{
		SetId("FTOWER44_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1585.03f, 536.94f, 574.63f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147449, -1597.26, 536.99, 579.03, 47, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Grita") }));
		actors.Add(AddTrackActor(character, 20026, -1603.54, 537.00, 614.04, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Magic Circle") }));

		for (var i = 0; i < MinivernSpots.GetLength(0); i++)
			actors.Add(AddTrackActor(character, 57050, MinivernSpots[i, 0], 536.99, MinivernSpots[i, 1], MinivernDirections[i], new TrackActorSpec { Ai = "TrackWaitMonster" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				track.Dialog.SetTitle(L("Grita"));
				track.Dialog.SetPortrait("Dlg_port_Grita");
				StartDialog(track, L("I will correct the disrupted magic of this tower. I will have to use magic at first, so monsters will rush in. Please protect me."));
				break;
			case 43:
				character.ServerMessage(L("Protect Grita while she controls the magic of the tower!"));
				break;
			case 44:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
