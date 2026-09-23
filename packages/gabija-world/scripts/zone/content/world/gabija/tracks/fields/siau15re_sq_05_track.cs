//--- Melia Script ----------------------------------------------------------
// The Nest on Bonan Forest Road
//--- Description -----------------------------------------------------------
// Burning the nest brings its owner, a Chafer, out to fight.
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

[TrackScript("SIAU15RE_SQ_05_TRACK")]
public class Siau15reSq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("SIAU15RE_SQ_05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1149.84f, 922.76f, 1278.09f));
		actors.Add(character);

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(AddTrackActor(character, 103034, -1159.16, 922.76, 1301.43, 0, prop));
		actors.Add(AddTrackActor(character, 47204, -1135.92, 922.76, 1290.13, 0, prop));
		actors.Add(AddTrackActor(character, 47204, -1186.55, 922.76, 1299.60, 0, prop));
		actors.Add(AddTrackActor(character, 47204, -1181.76, 922.76, 1323.29, 0, prop));
		actors.Add(AddTrackActor(character, 47204, -1132.40, 922.76, 1350.08, 0, prop));
		actors.Add(AddTrackActor(character, 57996, -1139.27, 922.76, 1310.14, 42, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1072.18f, 922.76f, 1301.47f) }));
		actors.Add(AddTrackActor(character, 47204, -1101.17, 922.76, 1328.40, 0, prop));
		actors.Add(AddTrackActor(character, 47204, -1064.64, 922.76, 1283.66, 0, prop));
		actors.Add(AddTrackActor(character, 47204, -1054.51, 922.76, 1322.53, 0, prop));
		actors.Add(AddTrackActor(character, 47204, -1107.34, 922.76, 1354.49, 0, prop));
		actors.Add(AddTrackActor(character, 103034, -1150.48, 922.76, 1313.67, 0, prop));
		actors.Add(AddTrackActor(character, 103034, -1165.31, 922.76, 1333.46, 0, prop));
		actors.Add(AddTrackActor(character, 103034, -1132.75, 922.76, 1310.94, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 55:
				// The burnt nest and grass leave on Client="BOTH" rows.
				for (var i = 1; i <= 13; ++i)
				{
					if (i != 6)
						RemoveTrackActor(character, track, i);
				}

				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Chafer, the owner of the nest is attacking!"), 3);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
