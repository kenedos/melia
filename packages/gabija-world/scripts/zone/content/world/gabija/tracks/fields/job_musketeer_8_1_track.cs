//--- Melia Script ----------------------------------------------------------
// The Musketeer Master's practice poles
//--- Description -----------------------------------------------------------
// Three pells west of the camp, each one worth a shot only while it is lit.
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

[TrackScript("JOB_MUSKETEER_8_1_TRACK")]
public class JobMusketeer81Track : TrackScript
{
	protected override void Load()
	{
		SetId("JOB_MUSKETEER_8_1_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-415.99f, 745.69f, -1646.35f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 58014, -491.80, 745.69, -1635.80, 0, new TrackActorSpec { Ai = "BT_Dummy", Name = L("Practice Pole") }));
		actors.Add(AddTrackActor(character, 58014, -442.97, 745.69, -1575.66, 0, new TrackActorSpec { Ai = "BT_Dummy", Name = L("Practice Pole") }));
		actors.Add(AddTrackActor(character, 58014, -450.78, 745.69, -1710.90, 0, new TrackActorSpec { Ai = "BT_Dummy", Name = L("Practice Pole") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;

			case 4:
				character.ServerMessage(L("Attack the Pell when it is shining!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
