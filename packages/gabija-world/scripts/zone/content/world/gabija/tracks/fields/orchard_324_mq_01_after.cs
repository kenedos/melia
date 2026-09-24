//--- Melia Script ----------------------------------------------------------
// Divine Encounter, aftermath
//--- Description -----------------------------------------------------------
// The beaten Zaura swears revenge and flees, leaving Goddess Lada behind.
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

[TrackScript("ORCHARD_324_MQ_01_AFTER")]
public class Orchard324Mq01After : TrackScript
{
	protected override void Load()
	{
		SetId("ORCHARD_324_MQ_01_AFTER");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-79.44f, -2.15f, 695.08f));

		var ward = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Redemption Ward") };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 156043, -46.79, 0.81, 870.83, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Goddess Lada") }));
		actors.Add(AddTrackActor(character, 155024, -46.98, 0.81, 813.68, 0, ward));
		actors.Add(AddTrackActor(character, 155024, -91.38, 0.81, 897.45, 0, ward));
		actors.Add(AddTrackActor(character, 155024, 1.99, 0.81, 897.57, 7, ward));
		actors.Add(AddTrackActor(character, 58087, -61.85, -2.15, 576.58, 1, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Demon Lord Zaura") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[1].AttachEffect("I_chain004_mash_loop", 4, EffectLocation.Bottom);
				track.Actors[1].AttachEffect("F_pattern008_violet_loop", 2, EffectLocation.Bottom);
				for (var i = 2; i <= 4; i++)
					track.Actors[i].AttachEffect("I_ground001_yellow_loop", 1, EffectLocation.Bottom);
				break;

			case 28:
				track.Dialog.SetTitle(L("Demon Lord Zaura"));
				track.Dialog.SetPortrait("Dlg_port_ziaurah");
				StartDialog(track, L("Don't think this is the end... If you think I'll let you rescue Lada, think again!"));
				break;

			case 29:
				track.Actors[5].AttachEffect("F_circle25_red", 8, EffectLocation.Bottom);
				break;

			case 33:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The Demon Lord Zaura ran away!"), 3);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
