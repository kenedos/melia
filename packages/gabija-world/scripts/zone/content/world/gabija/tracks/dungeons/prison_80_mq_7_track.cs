//--- Melia Script ----------------------------------------------------------
// Grinender at the Common Room
//--- Description -----------------------------------------------------------
// The seal opens Grinender's magic circle, and Grinender comes up through
// it to defend the Common Room.
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

[TrackScript("PRISON_80_MQ_7_TRACK")]
public class Prison80Mq7Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON_80_MQ_7_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-118.21f, 290.48f, 523.44f));

		actors.Add(AddTrackActor(character, 147469, -112.28, 290.48, 514.09, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 58432, -148.79, 125.73, 1088.87, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_summon_ground_red", 10, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_levitation032_red", 8, EffectLocation.Bottom);
				break;

			case 9:
				if (track.Actors[1] is ICombatEntity grinender)
					grinender.StartBuff(BuffId.PRISON_80_MQ_7_BUFF);
				break;

			case 30:
				character.ServerMessage(L("Upon disabling the demon magic circle, Grinender appeared. Defeat Grinender."));
				break;

			case 34:
				// The broken magic circle, on a Client="BOTH" row at this frame.
				RemoveTrackActor(character, track, 0);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
