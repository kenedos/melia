//--- Melia Script ----------------------------------------------------------
// Soul Starvation (2)
//--- Description -----------------------------------------------------------
// The demon guarding the Soul Starvation shows itself.
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

[TrackScript("KATYN_12_MQ_09_TRACK")]
public class Katyn12Mq09Track : TrackScript
{
	protected override void Load()
	{
		SetId("KATYN_12_MQ_09_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1964.05f, 250.77f, 159.63f));

		actors.Add(AddTrackActor(character, 151065, 1941.51, 250.56, 183.77, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Soul Starvation") }));
		actors.Add(AddTrackActor(character, 58013, 1906.23, 250.36, 225.55, 72, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		if (track.HasBattleBoxInLayer)
			return;

		// The arming frame is the cutscene's last one and may not be reported.
		CreateBattleBoxInLayer(character, track);
		SetTrackTendency(character, track);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 24:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The demon protecting the Soul Starvation has appeared!"), 5);
				break;
			case 35:
				if (!track.HasBattleBoxInLayer)
				{
					CreateBattleBoxInLayer(character, track);
					SetTrackTendency(character, track);
				}
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
