//--- Melia Script ----------------------------------------------------------
// The first Magic Regulator
//--- Description -----------------------------------------------------------
// The activation stones turn the regulator on the Revelator.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ZACHA1F_SQ_04_TRACK")]
public class Zacha1fSq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA1F_SQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 47261, -1006, 332, 1408, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Royal Mausoleum Magic Regulator"), MaxHp = 20 }));

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
		}

		await base.OnProgress(character, track, frame);
	}
}
