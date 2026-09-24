//--- Melia Script ----------------------------------------------------------
// Tyronas Hydra at the Heralve Vacant Lot
//--- Description -----------------------------------------------------------
// The Tyronas Hydra takes Modis' bait and tears through the trap.
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

[TrackScript("F_3CMLAKE_84_MQ_06_TRACK")]
public class F3Cmlake84Mq06Track : TrackScript
{
	protected override void Load()
	{
		SetId("F_3CMLAKE_84_MQ_06_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(628.14f, 263.09f, -236.86f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 57194, 656.14, 263.09, -218.72, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(AddTrackActor(character, 58209, 973.85, 151.22, 280.23, 0, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(678.79f, 263.09f, -167.21f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				CreateBattleBoxInLayer(character, track);
				break;

			case 73:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("It seems that the Hydra is furious!{nl}Defeat Hydra!"), 6);
				break;

			case 85:
				// The trap dies on a Client="BOTH" row the client never reports.
				RemoveTrackActor(character, track, 1);

				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
