//--- Melia Script ----------------------------------------------------------
// Bramble in Giliaii Courtyard
//--- Description -----------------------------------------------------------
// The Demon Lord and the revelation it took, at the end of the Thorn Forest.
//---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("THORN21_MQ07_TRACK")]
public class Thorn21Mq07Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN21_MQ07_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(5471.6978f, 333.2023f, -191.21075f));

		actors.Add(AddTrackActor(character, 400901, 5924.4668, 333.2023, -196.01424, 45, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47234, 5984.5435, 333.21231, -202.09409, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral, Name = L("Revelation") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 12080, 5447.1763, 333.21231, -197.30769, 60, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Neutral }));

		return actors.ToArray();
	}

	/// <summary>
	/// Lets the six patches of the courtyard flare up at random and poison
	/// whoever stands in them without the Enhanced Thorn Flower Stimulant.
	/// </summary>
	private static void StartHazards(Character character, Track track)
	{
		var game = new TrackMinigame(character, track);
		var random = new Random();

		var nodes = new (double X, double Z, int RestSeconds)[]
		{
			(5661.06, -315.42, 10), (5663.11, -105.50, 20), (5850.39, -319.12, 15),
			(5934.21, -71.06, 5), (5787.59, -182.31, 5), (5652.19, -276.71, 5),
		};

		var stage = game.Stage("Stage01");
		var armed = new bool[nodes.Length];
		var ticks = new int[nodes.Length];
		var cooldowns = new int[nodes.Length];
		var restUntil = new DateTime[nodes.Length];

		foreach (var node in nodes)
			stage.Monster(12080, node.X, 333.20, node.Z, aggressive: false, level: 62);

		stage.On(s => true, s =>
		{
			for (var i = 0; i < nodes.Length; ++i)
			{
				var hazard = s.Living(i).FirstOrDefault();
				if (hazard == null || DateTime.Now < restUntil[i])
					continue;

				if (!armed[i])
				{
					armed[i] = random.Next(1, 8) <= 4;
					continue;
				}

				ticks[i]++;

				if (ticks[i] == 1)
				{
					hazard.PlayEffect("F_burstup001_red", 0.7f);
				}
				else if (ticks[i] >= 4 && ticks[i] < 10)
				{
					hazard.PlayEffect("F_smoke017_red_1", 0.7f);

					foreach (var member in s.Game.Members)
					{
						if (member.IsDead || member.Map != hazard.Map || member.Layer != hazard.Layer || member.Position.Get2DDistance(hazard.Position) > 65)
							continue;

						if (member.IsBuffActive(BuffId.THORN21_MQ07_THORNDRUG))
							continue;

						member.StartBuff(BuffId.Rage_Rockto_spd_down, 3, 0, TimeSpan.FromSeconds(3), hazard);
						member.TakeSimpleHit(90, hazard);
						member.PlayEffect("F_smoke064_red", 1f);
					}
				}
				else if (ticks[i] >= 10 && ++cooldowns[i] >= 4)
				{
					armed[i] = false;
					ticks[i] = 0;
					cooldowns[i] = 0;
					restUntil[i] = DateTime.Now.AddSeconds(nodes[i].RestSeconds);
				}
			}
		});

		game.Start("Stage01");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[1].AttachEffect("F_cleric_melstis_loop_ground", 2, EffectLocation.Bottom);
				break;
			case 25:
				character.ServerMessage(L("Cross here after drinking the Enhanced Thorn Flower Stimulant!"));
				break;
			case 26:
				StartHazards(character, track);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
