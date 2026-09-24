//--- Melia Script ----------------------------------------------------------
// Rocksodon at the Drava Chapel Lot
//--- Description -----------------------------------------------------------
// A Rocksodon bursts out of the water after Napalis.
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

[TrackScript("F_3CMLAKE_83_SQ_03_TRACK")]
public class F3Cmlake83Sq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("F_3CMLAKE_83_SQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1526.61f, 303.58f, 605.96f));

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 57867, -1514.06, 303.58, 949.27, 148, new TrackActorSpec { Ai = "TrackWaitMonster", Level = 67 }));
		actors.Add(AddTrackActor(character, 57867, -1179.96, 70.85, 940.11, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 12082, -1735.02, 303.58, 954.95, 2, prop));
		actors.Add(AddTrackActor(character, 147469, -1518.43, 303.58, 947.11, 0, prop));
		actors.Add(AddTrackActor(character, 147469, -1492.05, 303.58, 901.28, 0, prop));
		actors.Add(AddTrackActor(character, 147469, -1505.99, 303.58, 945.13, 1, prop));
		actors.Add(AddTrackActor(character, 153093, -1502.08, 303.58, 923.06, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		if (frame >= 52 && frame <= 60)
			track.Actors[5].AttachEffect("F_pc_run_water", 20, EffectLocation.Top);

		switch (frame)
		{
			case 0:
				CreateBattleBoxInLayer(character, track);
				break;

			case 33:
				track.Actors[6].AttachEffect("F_bg_drop_water004##2", 10, EffectLocation.Bottom);
				break;

			case 36:
				track.Actors[6].AttachEffect("F_bg_drop_water003##2", 20, EffectLocation.Bottom);
				break;

			case 52:
				track.Actors[1].AttachEffect("I_ground008_water", 10, EffectLocation.Bottom);
				break;

			case 54:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Rocksodon is attacking from under the water!"), 3);
				break;

			case 64:
				// The water effects and the second Rocksodon die on Client="BOTH" rows the client never reports.
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 7);

				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
