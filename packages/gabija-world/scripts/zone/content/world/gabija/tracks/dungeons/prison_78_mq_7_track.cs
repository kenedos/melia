//--- Melia Script ----------------------------------------------------------
// Mandara in the Offender Institution
//--- Description -----------------------------------------------------------
// The demon guarding the Visiting Room rises behind a barrier the Magic
// Control Scroll has to keep breaking.
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

[TrackScript("PRISON_78_MQ_7_TRACK")]
public class Prison78Mq7Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON_78_MQ_7_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(597.55f, 620.48f, 1411.15f));

		actors.Add(AddTrackActor(character, 152031, 634.00, 640.24, 1687.00, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 58431, 634.71, 640.24, 1793.18, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 57707, 423.13, 619.54, 2351.90, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(422.57f, 619.42f, 2209.14f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 11:
			case 13:
			case 15:
				track.Actors[3].AttachEffect("F_smoke129_spreadout", 1, EffectLocation.Bottom);
				break;

			case 17:
				track.Actors[3].AttachEffect("F_smoke129_spreadout", 1, EffectLocation.Bottom);
				track.Actors[3].AttachEffect("F_burstup003_1", 1, EffectLocation.Bottom);
				break;

			case 33:
				track.Actors[1].PlayEffect("F_ground058_smoke", 1.5f);
				break;

			case 39:
				track.Actors[1].PlayEffect("F_ground058_smoke", 1f);
				break;

			case 44:
				track.Actors[1].PlayEffect("F_burstup001_smoke1", 1f);
				break;

			case 51:
				if (track.Actors[1] is ICombatEntity mandara)
					mandara.StartBuff(BuffId.PRISON_78_MQ_7_BUFF);
				break;

			case 52:
				// The Mandara the new one devours at frame 28, on a Client="BOTH" row.
				RemoveTrackActor(character, track, 3);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				character.ServerMessage(L("Use the Magic Control Scroll to weaken Mandara and defeat it!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
