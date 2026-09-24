//--- Melia Script ----------------------------------------------------------
// Delmore Rephaim's Last Stand
//--- Description -----------------------------------------------------------
// Mihail blows the gate open and Delmore Rephaim turns himself into a
// Kruvina.
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

[TrackScript("CASTLE65_3_MQ08_TRACK")]
public class Castle653Mq08Track : TrackScript
{
	protected override void Load()
	{
		SetId("CASTLE65_3_MQ08_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1144.54f, 0.03f, -1619.34f));

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 155101, 1603.01, 75.58, -709.16, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Delmore Rephaim") }));
		actors.Add(AddTrackActor(character, 151001, 1597.82, 75.58, -721.72, 0, prop));
		actors.Add(AddTrackActor(character, 58039, 1604.71, 75.58, -716.67, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Delmore Rephaim"), Level = 87 }));
		actors.Add(AddTrackActor(character, 20024, 1603.01, 75.58, -709.16, 0, prop));
		actors.Add(AddTrackActor(character, 155094, 1163.10, 0.75, -1572.72, 32, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Mihail"), EndPosition = new Position(1111.15f, 0.75f, -1596.07f) }));
		actors.Add(AddTrackActor(character, 147379, 1197.16, 2.61, -1555.22, 0, prop));
		actors.Add(AddTrackActor(character, 20024, 1241.93, 14.10, -1552.54, 0, prop));
		actors.Add(AddTrackActor(character, 20024, 1603.01, 75.58, -709.16, 0, prop));

		return actors.ToArray();
	}

	public override void OnHandOver(Character character, Track track)
	{
		ArmBoss(character, track);
	}

	/// <summary>
	/// Clears the cutscene's props and turns the Kruvina loose.
	/// </summary>
	private static void ArmBoss(Character character, Track track)
	{
		if (track.HasBattleBoxInLayer)
			return;

		// The lord, the crystal and the gate die on Client="BOTH" rows the client never reports.
		RemoveTrackActor(character, track, 1);
		RemoveTrackActor(character, track, 2);
		RemoveTrackActor(character, track, 6);
		RemoveTrackActor(character, track, 8);

		CreateBattleBoxInLayer(character, track);
		SetTrackTendency(character, track);
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 15:
				track.Actors[6].PlayEffect("F_explosion049_fire", 5f, 1, EffectLocation.Middle);
				break;

			case 20:
				track.Dialog.SetTitle(L("Revelator Mihail"));
				StartDialog(track,
					L("Success!"),
					L("The explosion is sure to make demons come running here. I'll try and stop them as much as I can; you take care of Delmore Rephaim.")
				);
				break;

			case 42:
				track.Dialog.SetTitle(L("Delmore Rephaim"));
				track.Dialog.SetPortrait("Dlg_port_CastleLord");
				StartDialog(track,
					L("You're here..."),
					L("You must think I lost now that you found me. But I won't lose that easily."),
					L("It's true... I don't think I can become a god. But I can turn myself into something more noble."),
					L("All my possessions, my people, my knowledge, they're still not enough! Revelators... Their power alone will guide me.")
				);
				break;

			case 47:
				track.Actors[2].PlayEffect("F_light047_red", 1f, 1, EffectLocation.Middle);
				break;

			case 53:
				track.Dialog.SetTitle(L("Delmore Rephaim"));
				track.Dialog.SetPortrait("Dlg_port_CastleLord");
				StartDialog(track,
					L("Surprised? This Kruvina is the first piece made with all my loyal vassals."),
					L("Consider it an honor to be its first victim. Don't worry, I'll make sure to turn all the other Revelators into Kruvina so you won't feel lonely!")
				);
				break;

			case 93:
				track.Actors[3].PlayEffect("F_smoke042_red", 1f, 1, EffectLocation.Bottom);
				break;

			case 115:
				ArmBoss(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
