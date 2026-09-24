//--- Melia Script ----------------------------------------------------------
// Owl Sculpture in Danger
//--- Description -----------------------------------------------------------
// A Moa corrupted by the demons closes in on the Owl Chief Sculpture.
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

[TrackScript("KATYN_10_MQ_03_TRACK")]
public class Katyn10Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("KATYN_10_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(2412.96f, 200.96f, -474.21f));

		actors.Add(AddTrackActor(character, 20135, 1931.59, 200.96, -530.08, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Owl Chief Sculpture") }));
		actors.Add(AddTrackActor(character, 58012, 1949.18, 200.96, -366.61, 56, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(1941.93f, 200.96f, -445.03f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_light015_violet1", 2, EffectLocation.Middle);
				break;
			case 7:
				track.Actors[1].AttachEffect("F_levitation007_red", 1, EffectLocation.Bottom);
				break;
			case 12:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The Owl Sculpture is asking for help!"), 5);
				break;
			case 19:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
