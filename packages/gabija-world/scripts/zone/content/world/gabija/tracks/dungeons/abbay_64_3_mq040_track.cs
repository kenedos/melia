//--- Melia Script ----------------------------------------------------------
// The Kilnuma Oratorium
//--- Description -----------------------------------------------------------
// Rose stands bound on the wizard's magic circle while a Deathweaver
// guards the ritual.
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

[TrackScript("ABBAY_64_3_MQ040_TRACK")]
public class Abbay643Mq040Track : TrackScript
{
	protected override void Load()
	{
		SetId("ABBAY_64_3_MQ040_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1485.68f, 585.72f, -470.35f));

		var prop = new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(AddTrackActor(character, 153110, -1513, 584.96, -473, 83, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Edmundas"), EndPosition = new Position(-1548.75f, 616.52f, -192.70f) }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 153119, -1459, 622.42, 175, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Traveling Merchant Rose") }));
		actors.Add(AddTrackActor(character, 153120, -1454.40, 622.42, 244.96, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = L("Mysterious Wizard") }));
		actors.Add(AddTrackActor(character, 41380, -1369.30, 622.42, 369.40, 72, new TrackActorSpec { Ai = "TrackWaitMonster", EndPosition = new Position(-1369.30f, 622.42f, 182.64f) }));
		actors.Add(AddTrackActor(character, 47123, -1459.86, 622.42, 175.44, 0, prop));
		actors.Add(AddTrackActor(character, 20046, -1457.65, 622.42, 245.50, 0, prop));
		actors.Add(AddTrackActor(character, 47254, -1555, 621, 268, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
			case 28:
			case 38:
			case 48:
			case 58:
				track.Actors[5].PlayEffect("F_levitation015_dark", 1f, 1, EffectLocation.Bottom);
				break;

			case 64:
				track.Actors[3].PlayEffect("F_spin009", 0.5f, 1, EffectLocation.Bottom);
				break;

			case 68:
				track.Actors[3].PlayEffect("I_sphere011_mash", 0.6f, 1, EffectLocation.Bottom);
				break;

			case 69:
				SetTrackTendency(character, track);
				CreateBattleBoxInLayer(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
