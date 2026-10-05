//--- Melia Script ----------------------------------------------------------
// Gesti cornered in the central hall
//--- Description -----------------------------------------------------------
// The altar trap springs, and the Divine Sphere charges.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CHAPLE577_MQ_09_TRACK")]
public class Chaple577Mq09Track : TrackScript
{
	private const int QuestId = 8536;
	private const int GestiIndex = 2;
	private const float GestiFleeHpRate = 0.5f;

	protected override void Load()
	{
		SetId("CHAPLE577_MQ_09_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(113.50f, 164.86f, -635.30f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147390, 110, 165, -579, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Follower Algis") }));
		actors.Add(AddTrackActor(character, 57055, -29.07, 48.71, -137.31, 3, new TrackActorSpec { Ai = "BasicBoss", Name = L("Demon Queen Gesti") }));
		actors.Add(AddTrackActor(character, MonsterId.Block_Fence_2, -232.52, 35.92, -57.02, 91, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, MonsterId.Block_Fence_2, -234.81, 35.92, -226.78, 91, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, MonsterId.Block_Fence_2, -117.63, 35.92, -311.72, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, MonsterId.Block_Fence_2, 76.77, 35.92, -312.60, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 147352, 134, 165, -576, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 147373, 112.36, 164.86, -606.13, 4, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 152003, 207.14, 164.86, -582.65, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	/// <summary>
	/// Makes Gesti speak and flee once she is worn down to half her HP, ending the fight.
	/// </summary>
	private static void WatchGesti(Character character, Track track)
	{
		if (track.Actors[GestiIndex] is not Mob gesti)
			return;

		var game = new TrackMinigame(character, track);

		game.Stage("fight")
			.On(s => gesti.IsDead || gesti.Hp <= gesti.MaxHp * GestiFleeHpRate, s =>
			{
				track.Dialog.SetTitle(L("Demon Queen Gesti"));
				track.Dialog.SetPortrait("Dlg_port_Gesti");
				StartDialog(track, L("Insolent humans."));

				RemoveTrackActor(s.Game.Character, track, GestiIndex);
				s.Game.CompleteObjective(QuestId, "fightGesti");
			}, 1);

		game.Start("fight");
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 3:
				track.Dialog.SetTitle(L("Follower Algis"));
				track.Dialog.SetPortrait("Dlg_port_algis");
				StartDialog(track, L("The barrier is up and running."),
					L("Please deal with Gesti while I activate the Divine Sphere."));
				break;
			case 30:
				character.ServerMessage(L("The Holy Sphere has been activated!"));
				character.ServerMessage(L("Protect yourself from Gesti while the Divine Sphere is being activated!"));
				break;
			case 32:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
			case 38:
				character.ServerMessage(L("Oppose Gesti until the Divine Sphere is activated!"));
				break;
			case 39:
				track.Dialog.SetTitle(L("Demon Queen Gesti"));
				track.Dialog.SetPortrait("Dlg_port_Gesti");
				StartDialog(track, L("The Revelator has come here alone."),
					L("Exactly what I wanted."));
				WatchGesti(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
