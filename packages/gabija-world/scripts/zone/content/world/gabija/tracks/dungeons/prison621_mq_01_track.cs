//--- Melia Script ----------------------------------------------------------
// Into Ashaq Underground Prison
//--- Description -----------------------------------------------------------
// Priest Pranas and the Chasers step into the prison and feel its curse.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("PRISON621_MQ_01_TRACK")]
public class Prison621Mq01Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON621_MQ_01_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-477.57f, 325.85f, -807.07f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 155044, -507.00, 328.73, -625.64, 87, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Priest Pranas"), EndPosition = new Position(-599.78f, 325.68f, -179.75f) }));
		actors.Add(AddTrackActor(character, 147403, -523.57, 327.39, -694.08, 95, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Chaser Torvana"), EndPosition = new Position(-363.66f, 325.43f, -139.95f) }));
		actors.Add(AddTrackActor(character, 147406, -467.25, 327.50, -688.54, 76, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Chaser Daramaus"), EndPosition = new Position(-373.49f, 325.81f, -197.36f) }));
		actors.Add(AddTrackActor(character, 40071, -506.01, 331.45, -486.64, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 11:
				track.Actors[2].PlayEffect("F_spread_out004_dark", 1f);
				break;

			case 12:
				track.Actors[3].PlayEffect("F_spread_out004_dark", 1f);
				break;

			case 16:
				character.PlayEffect("F_light018_yellow", 1f, 1, EffectLocation.Middle);
				break;

			case 54:
				character.Quests.CompleteObjective(new QuestId(60115), "followPranas");
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
