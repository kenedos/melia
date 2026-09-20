//--- Melia Script ----------------------------------------------------------
// The will of King Zachariel
//--- Description -----------------------------------------------------------
// The burial chamber opens and Goddess Laima tells the Revelator what the
// Great King was left to keep.
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

[TrackScript("ZACHA5_SPIRIT_TRACK")]
public class Zacha5SpiritTrack : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA5_SPIRIT_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-3017.63f, 448.92f, 455.88f));

		actors.Add(AddTrackActor(character, 20025, -2658.36, 453.20, 492.23, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 154040, -2657.97, 453.20, 490.91, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Goddess Laima") }));
		actors.Add(AddTrackActor(character, 47234, -2657.98, 453.20, 490.91, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Revelation Slate") }));
		actors.Add(AddTrackActor(character, 153032, -3014.89, 448.92, 494.20, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("King Zachariel's Spirit") }));
		actors.Add(AddTrackActor(character, 12082, -2952.67, 448.92, 572.00, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 12082, -3080.78, 448.92, 560.56, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 12082, -2950.16, 448.92, 415.50, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 12082, -3074.86, 448.92, 404.75, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 153024, -3018.78, 448.92, 493.31, 5, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Great King Zachariel's Coffin") }));
		actors.Add(AddTrackActor(character, 20025, -2657.33, 453.20, 492.78, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -2280.70, 407.74, 657.64, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -2515.55, 384.26, 47.85, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -2962.51, 403.73, 663.78, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 20025, -2911.61, 407.49, 228.40, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[5].AttachEffect("F_bg_statu_blue", 0.7f, EffectLocation.Bottom);
				track.Actors[6].AttachEffect("F_bg_statu_blue", 0.7f, EffectLocation.Bottom);
				track.Actors[7].AttachEffect("F_bg_statu_blue", 0.7f, EffectLocation.Bottom);
				track.Actors[8].AttachEffect("F_bg_statu_blue", 0.7f, EffectLocation.Bottom);
				break;
			case 33:
				track.Dialog.SetTitle(L("King Zachariel's Will"));
				StartDialog(track, L("Goddess Laima... The Revelator who you foretold has finally come..."));
				break;
			case 36:
				track.Actors[11].AttachEffect("F_line022_yellow", 6, EffectLocation.Bottom);
				break;
			case 37:
				track.Actors[10].AttachEffect("F_bg_light003_yellow", 2, EffectLocation.Middle);
				break;
			case 39:
				track.Actors[12].AttachEffect("F_line022_yellow", 6, EffectLocation.Top);
				break;
			case 41:
				track.Actors[13].AttachEffect("F_line022_yellow", 6, EffectLocation.Top);
				RemoveTrackActor(character, track, 4);
				break;
			case 45:
				track.Actors[14].AttachEffect("F_line022_yellow", 6, EffectLocation.Top);
				break;
			case 54:
				track.Actors[2].AttachEffect("F_light078_holy_yellow_loop", 2, EffectLocation.Bottom);
				RemoveTrackActor(character, track, 10);
				break;
			case 63:
				track.Dialog.SetTitle(L("Goddess Laima"));
				track.Dialog.SetPortrait("Dlg_port_Raima");
				StartDialog(track, L("A thousand years ago, before your generation..."), L("This is the story of the time before I left the revelation with the Great King Zachariel."));
				break;
			case 66:
				track.Actors[2].AttachEffect("F_lineup004", 4, EffectLocation.Bottom);
				break;
			case 74:
				track.Actors[0].AttachEffect("F_lineup015_blue", 4, EffectLocation.Bottom);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
