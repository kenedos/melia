//--- Melia Script ----------------------------------------------------------
// The Goddess' Hidden Message
//--- Description -----------------------------------------------------------
// The great statue of Goddess Laima in Zeraha answers the Revelator.
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

[TrackScript("ORCHARD_324_MQ_07_TRACK")]
public class Orchard324Mq07Track : TrackScript
{
	protected override void Load()
	{
		SetId("ORCHARD_324_MQ_07_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1410.57f, 1032.38f, 1152.60f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 147413, -1385.99, 1028.12, 1161.15, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 7:
				track.Actors[1].AttachEffect("F_smoke024_blue1", 2, EffectLocation.Bottom);
				break;

			case 9:
				track.Actors[1].AttachEffect("F_bg_light003_blue", 1, EffectLocation.Bottom);
				break;

			case 15:
				track.Actors[1].AttachEffect("F_magic_prison_line_blue", 4.5f, EffectLocation.Middle);
				break;

			case 17:
				track.Actors[1].AttachEffect("F_ground051_loop", 5, EffectLocation.Bottom);
				break;

			case 24:
				track.Actors[1].PlayEffect("F_buff_basic025_white_line", 10f, 1, EffectLocation.Bottom);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
