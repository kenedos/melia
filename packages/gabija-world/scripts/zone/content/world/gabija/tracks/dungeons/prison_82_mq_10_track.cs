//--- Melia Script ----------------------------------------------------------
// His name is Zanas
//--- Description -----------------------------------------------------------
// Zanas pays the last of his soul to break the barrier, and Nebulas is
// left to answer for it.
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

[TrackScript("PRISON_82_MQ_10_TRACK")]
public class Prison82Mq10Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON_82_MQ_10_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-555.16f, 618.94f, -1415.12f));

		actors.Add(AddTrackActor(character, 151003, -550.00, 618.94, -1577.00, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 151107, -529.84, 618.94, -1555.70, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Zanas' Soul") }));
		actors.Add(AddTrackActor(character, 58412, -539.91, 553.88, -1048.72, 90, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-534.53f, 618.94f, -1422.30f) }));
		actors.Add(AddTrackActor(character, 20026, -550.00, 618.94, -1577.00, 4, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[0].AttachEffect("F_pattern008_violet_loop", 2.5f, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_bg_rize002_violet2", 0.8f, EffectLocation.Bottom);
				track.Actors[2].PlayEffect("F_lineup020_blue_mint", 0.6f);
				break;

			case 7:
				track.Actors[2].AttachEffect("F_ground023", 2, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("F_summon_ground_red2", 2, EffectLocation.Bottom);
				track.Dialog.SetTitle(L("Zanas' Soul"));
				track.Dialog.SetPortrait("Dlg_port_zanas_prison");
				StartDialog(track,
					L("This is my last calling."),
					L("I just hope this will be enough to complete my mission."),
					L("I'll deploy the Dominance Magic now."),
					L("Once it's activated, I'll be gone forever."),
					L("I'm sorry I couldn't be with you until the end."),
					L("Defeat Nebulas, take back the revelation.")
				);
				break;

			case 10:
				track.Actors[0].AttachEffect("I_smoke007_green", 1, EffectLocation.Bottom);
				track.Actors[0].AttachEffect("F_ground051_loop2", 3, EffectLocation.Bottom);
				break;

			case 14:
				track.Actors[3].AttachEffect("F_spread_out003_darkblue", 1.6f, EffectLocation.Middle);
				track.Actors[4].AttachEffect("F_light078_holy_yellow_loop", 5, EffectLocation.Bottom);
				track.Actors[4].AttachEffect("F_spread_in027_green_loop", 7, EffectLocation.Bottom);
				break;

			case 31:
				track.Dialog.SetTitle(L("Zanas' Soul"));
				track.Dialog.SetPortrait("Dlg_port_zanas_prison");
				StartDialog(track,
					L("It's too late, Nebulas!"),
					L("The demon barrier will be destroyed and you too will end at the hands of the Revelator.")
				);
				break;

			case 36:
				track.Actors[4].AttachEffect("F_spread_out039_green", 12, EffectLocation.Bottom);
				break;

			case 47:
				character.ServerMessage(L("Nebulas' power became weaker after disabling the demon barrier. Defeat Nebulas!"));
				break;

			case 48:
				// The barrier dies and Zanas fades at frame 40, on Client="BOTH" rows.
				RemoveTrackActor(character, track, 0);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 4);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
