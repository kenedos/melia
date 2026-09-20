//--- Melia Script ----------------------------------------------------------
// Lithorex at Serno Highland
//--- Description -----------------------------------------------------------
// Lithorex breaks out of the rock face and runs Varkis down.
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

[TrackScript("ROKAS29_VACYS2_TRACK")]
public class Rokas29Vacys2Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS29_VACYS2_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1691.25f, 470.82f, 429.08f));

		actors.Add(AddTrackActor(character, 152000, 1365.74, 470.82, 638.16, 20, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Adventurer Varkis"), EndPosition = new Position(1663.34f, 470.82f, 644.79f) }));
		actors.Add(AddTrackActor(character, 57504, 1255.48, 470.82, 607.48, 85, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 20025, 1282.58, 470.82, 583.92, 6, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, 1384.34, 470.82, 596.22, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, 1438.45, 470.87, 595.36, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, 1488.25, 471.10, 595.19, 25, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, 1175.44, 470.82, 329.72, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, EndPosition = new Position(1108.51f, 470.82f, 306.59f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 10:
				track.Actors[3].AttachEffect("F_levitation017_smoke", 0.8f, EffectLocation.Middle);
				break;
			case 33:
				track.Actors[4].AttachEffect("F_rize010_blue", 3, EffectLocation.Bottom);
				track.Actors[4].AttachEffect("F_rize004_dark", 4, EffectLocation.Bottom);
				break;
			case 34:
				track.Actors[4].AttachEffect("F_explosion100_blue", 2, EffectLocation.Bottom);
				track.Actors[5].AttachEffect("F_rize010_blue", 3, EffectLocation.Bottom);
				track.Actors[5].AttachEffect("F_rize004_dark", 4, EffectLocation.Bottom);
				break;
			case 35:
				track.Actors[5].AttachEffect("F_explosion100_blue", 2, EffectLocation.Bottom);
				track.Actors[6].AttachEffect("F_rize010_blue", 3, EffectLocation.Bottom);
				track.Actors[6].AttachEffect("F_rize004_dark", 4, EffectLocation.Bottom);
				break;
			case 36:
				track.Actors[6].AttachEffect("F_explosion100_blue", 2, EffectLocation.Bottom);
				break;
			case 44:
				// Varkis is struck down at frame 41 and the effect anchors are spent.
				RemoveTrackActor(character, track, 0);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 7);

				character.ServerMessage(L("Varkis is in danger! Defeat Lithorex and save him!"));
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
