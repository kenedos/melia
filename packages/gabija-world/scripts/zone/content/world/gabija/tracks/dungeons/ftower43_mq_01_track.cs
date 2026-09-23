//--- Melia Script ----------------------------------------------------------
// Antares at the third floor stairs
//--- Description -----------------------------------------------------------
// The imprisoned magician throws his Red Infrorocktors at the landing.
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

[TrackScript("FTOWER43_MQ_01_TRACK")]
public class Ftower43Mq01Track : TrackScript
{
	private readonly static double[,] RocktorSpots =
	{
		{ -2615.97, 556.43, 167.79, -2485.72, 524.67, 3.98 },
		{ -2672.66, 576.25, 207.78, -2524.66, 524.12, 22.42 },
		{ -2565.97, 537.07, 353.63, -2542.85, 523.85, 68.43 },
		{ -2687.56, 576.25, 237.97, -2568.10, 523.50, 58.92 },
		{ -2707.00, 576.25, 238.32, -2591.57, 524.83, 104.41 },
		{ -2514.53, 595.12, 450.39, -2510.76, 524.31, 115.91 },
	};

	private readonly static double[] RocktorDirections = { 50, 54, 68, 49, 38, 54 };

	protected override void Load()
	{
		SetId("FTOWER43_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2527.46f, 525.34f, -190.20f));

		actors.Add(AddTrackActor(character, 151005, -2405.94, 525.34, 54.75, 43, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Antares") }));

		for (var i = 0; i < RocktorSpots.GetLength(0); i++)
		{
			var endPosition = new Position((float)RocktorSpots[i, 3], (float)RocktorSpots[i, 4], (float)RocktorSpots[i, 5]);
			actors.Add(AddTrackActor(character, 57053, RocktorSpots[i, 0], RocktorSpots[i, 1], RocktorSpots[i, 2], RocktorDirections[i], new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = endPosition }));
		}

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147449, -2555, 525, -190, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Grita") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 7:
				track.Dialog.SetTitle(L("Antares"));
				track.Dialog.SetPortrait("Dlg_port_antares");
				StartDialog(track, L("What? You still try and disturb my experiments? I will not allow this!"));
				break;
			case 29:
				// Antares leaves with the cutscene; his own kill frame is past
				// the track's last reported one.
				RemoveTrackActor(character, track, 0);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Defeat all the Red Infrorocktors Antares summoned!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
