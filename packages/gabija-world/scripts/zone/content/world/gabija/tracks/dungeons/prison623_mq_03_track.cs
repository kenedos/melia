//--- Melia Script ----------------------------------------------------------
// The Second Watchtower's Idol
//--- Description -----------------------------------------------------------
// Destroying the cursed idol brings out the Denoptic guarding it.
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

[TrackScript("PRISON623_MQ_03_TRACK")]
public class Prison623Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON623_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-41.61f, 1508.25f, -939.16f));

		actors.Add(AddTrackActor(character, 47150, -26.79, 1508.25, -907.05, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Cursed Idol") }));
		actors.Add(AddTrackActor(character, 58001, 307.66, 1252.09, -1318.00, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-225.73f, 1508.25f, -1209.95f) }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_pattern008_violet_loop", 1, EffectLocation.Bottom);
				break;

			case 38:
				// The destroyed idol leaves on a Client="BOTH" row.
				RemoveTrackActor(character, track, 0);

				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("As you destroyed the Cursed Idol, Denoptic appeared and started to attack!"), 10);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
