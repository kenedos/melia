//--- Melia Script ----------------------------------------------------------
// Varkis' Camp
//--- Description -----------------------------------------------------------
// The research burns and Varkis' spirit takes his leave.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("ROKAS29_VACYS6_TRACK")]
public class Rokas29Vacys6Track : TrackScript
{
	protected override void Load()
	{
		SetId("ROKAS29_VACYS6_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 46011, 163, 681, 769, 1, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("Bonfire") }));
		actors.Add(AddTrackActor(character, 152000, 168.58, 681.77, 792.07, 1, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Adventurer Varkis' Spirit") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 6:
				track.Dialog.SetTitle(L("Adventurer Varkis' Spirit"));
				StartDialog(track, L("Now I can leave without any regrets."), L("Goodbye. Be careful of that man..."));
				break;
			case 8:
				track.Actors[1].AttachEffect("F_burstup001_blue", 0.5f, EffectLocation.Bottom);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
