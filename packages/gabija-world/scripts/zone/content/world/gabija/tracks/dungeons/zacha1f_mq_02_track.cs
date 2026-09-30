//--- Melia Script ----------------------------------------------------------
// The Royal Cubes of the first floor
//--- Description -----------------------------------------------------------
// The four cubes around the manual turn on whoever reads it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ZACHA1F_MQ_02_TRACK")]
public class Zacha1fMq02Track : TrackScript
{
	private const int CubeCount = 4;

	protected override void Load()
	{
		SetId("ZACHA1F_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 47262, -876, 252, -835, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 47262, -1145, 252, -840, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 47262, -1154, 252, -1105, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 47262, -869, 252, -1113, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 47252, -567, 253, -933, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Royal Mausoleum Cube Manual") }));

		return actors.ToArray();
	}

	/// <summary>
	/// Keeps Boowooks coming, and lets each cube absorb one that is worn down next to it.
	/// </summary>
	private static void StartMinigame(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);
		var cooldowns = new int[CubeCount];

		game.Stage("DefGroup")
			.Monster(401121, -1014.02, 252.76, -1109.41, respawnSeconds: 1)
			.Monster(401121, -1007.12, 252.76, -837.18, respawnSeconds: 1)
			.Monster(401121, -879.98, 252.76, -983.68, respawnSeconds: 1)
			.On(s => true, s =>
			{
				for (var i = 0; i < CubeCount; ++i)
				{
					if (cooldowns[i] > 0)
					{
						cooldowns[i] = (cooldowns[i] + 1) % 11;
						continue;
					}

					if (track.Actors[i] is not Actor cube)
						continue;

					var boowook = Enumerable.Range(0, 3)
						.SelectMany(spawn => s.Living(spawn))
						.FirstOrDefault(mob => mob.Position.Get2DDistance(cube.Position) <= 80 && mob.Hp * 100f / mob.MaxHp < 30);

					if (boowook == null)
						continue;

					cooldowns[i] = 1;
					cube.PlayEffect("F_archer_smokebomb_shot_ground", 1.8f);
					boowook.Kill(null);

					foreach (var member in s.Game.Members)
						member.Quests.AddObjectiveProgress(new QuestId(8211), "purifyCubes");
				}
			});

		game.Start("DefGroup");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				StartMinigame(character, track);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
