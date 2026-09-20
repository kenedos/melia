//--- Melia Script ----------------------------------------------------------
// Rexipher breaks the lanterns
//--- Description -----------------------------------------------------------
// The historian's shape falls away and the four lanterns go out with it.
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

[TrackScript("ZACHA2F_MQ_03_TRACK")]
public class Zacha2fMq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA2F_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-143.60f, 648.11f, -1061.22f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 47253, 4.61, 648.12, -722.34, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Royal Mausoleum Stone Lantern") }));
		actors.Add(AddTrackActor(character, 47253, -377.14, 648.12, -726.53, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Royal Mausoleum Stone Lantern") }));
		actors.Add(AddTrackActor(character, 47253, -208.58, 648.12, -882.65, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Royal Mausoleum Stone Lantern") }));
		actors.Add(AddTrackActor(character, 47253, -207.57, 648.12, -570.20, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Royal Mausoleum Stone Lantern") }));
		actors.Add(AddTrackActor(character, 41229, -209.30, 616.86, -724.52, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Rexipher") }));
		actors.Add(AddTrackActor(character, 47413, -206.52, 616.86, -727.60, 24, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Rexipher") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Dialog.SetTitle(L("Rexipher"));
				track.Dialog.SetPortrait("Dlg_port_LEXIPER");
				StartDialog(track, L("Impossible."));
				break;
			case 27:
				track.Actors[1].AttachEffect("F_rize001_green", 0.5f, EffectLocation.Bottom);
				track.Actors[2].AttachEffect("F_rize001_green", 0.5f, EffectLocation.Bottom);
				track.Actors[3].AttachEffect("F_rize001_green", 0.5f, EffectLocation.Bottom);
				track.Actors[4].AttachEffect("F_rize001_green", 0.5f, EffectLocation.Bottom);
				break;
			case 54:
				// The historian's shape and the four lanterns are both gone by the
				// time the cast is handed back.
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 2);
				RemoveTrackActor(character, track, 3);
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
