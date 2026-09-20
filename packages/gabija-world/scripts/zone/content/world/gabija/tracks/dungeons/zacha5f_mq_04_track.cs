//--- Melia Script ----------------------------------------------------------
// Rexipher at the last guardian
//--- Description -----------------------------------------------------------
// The false revelation does not hold him long, and he comes back for the real one.
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

[TrackScript("ZACHA5F_MQ_04_TRACK")]
public class Zacha5fMq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA5F_MQ_04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-2495.67f, 363.56f, -1136.87f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147467, -2491.93, 363.56, -1094.53, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, Name = L("The Last Guardian"), Level = 95 }));
		actors.Add(AddTrackActor(character, 41229, -2491.15, 368.30, -358.58, 49, new TrackActorSpec { Ai = "TrackWaitMonster", Name = L("Rexipher"), EndPosition = new Position(-2478.71f, 368.30f, -201.14f) }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				track.Actors[1].AttachEffect("F_cleric_energy_blast_shot", 2, EffectLocation.Bottom);
				break;
			case 23:
				track.Actors[2].AttachEffect("F_levitation036_smoke_dark_green", 1.7f, EffectLocation.Bottom);
				break;
			case 24:
				track.Actors[2].AttachEffect("F_spread_out004_dark", 3, EffectLocation.Bottom);
				break;
			case 33:
				track.Dialog.SetTitle(L("Rexipher"));
				track.Dialog.SetPortrait("Dlg_port_LEXIPER");
				StartDialog(track, L("Hmpf... Who would've thought that I would be challenged by humans..."), L("Your death shall be utmost painful!"));
				break;
			case 39:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
