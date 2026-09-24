//--- Melia Script ----------------------------------------------------------
// The Magic Power Supply Device at the Ishinti Crossroads
//--- Description -----------------------------------------------------------
// Mihail detonates the Magic Concentration Orbs around the device.
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

[TrackScript("CASTLE65_1_MQ05_TRACK")]
public class Castle651Mq05Track : TrackScript
{
	protected override void Load()
	{
		SetId("CASTLE65_1_MQ05_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(313.85f, 81.76f, 231.86f));

		var prop = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 155094, 328.53, 81.76, 196.95, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Mihail"), EndPosition = new Position(459.63f, 81.82f, -11.64f) }));
		actors.Add(AddTrackActor(character, 155105, 46.02, 81.76, 534.29, 0, prop));
		actors.Add(AddTrackActor(character, 155116, 46.02, 81.76, 534.29, 0, prop));
		actors.Add(AddTrackActor(character, 155107, 38.77, 81.76, 513.37, 0, prop));
		actors.Add(AddTrackActor(character, 155107, 61.42, 81.76, 519.42, 0, prop));
		actors.Add(AddTrackActor(character, 155107, 66.16, 81.76, 541.83, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 14:
				track.Actors[1].PlayEffect("F_buff_basic008_blue", 1f, 1, EffectLocation.Bottom);
				break;

			case 15:
				track.Actors[4].PlayEffect("F_circle006", 2f, 1, EffectLocation.Bottom);
				break;

			case 17:
				track.Actors[6].PlayEffect("F_circle006", 2f, 1, EffectLocation.Bottom);
				break;

			case 19:
				track.Actors[4].PlayEffect("F_explosion015", 1f, 1, EffectLocation.Bottom);
				track.Actors[5].PlayEffect("F_circle006", 2f, 1, EffectLocation.Bottom);
				break;

			case 20:
				track.Actors[6].PlayEffect("F_explosion015", 1f, 1, EffectLocation.Bottom);
				break;

			case 21:
				track.Actors[5].PlayEffect("F_explosion015", 1f, 1, EffectLocation.Bottom);
				break;

			case 23:
				track.Actors[2].PlayEffect("F_explosion049_fire", 1f, 1, EffectLocation.Bottom);
				break;

			case 26:
				track.Actors[2].PlayEffect("F_explosion012", 1f, 1, EffectLocation.Bottom);
				break;

			case 29:
				track.Actors[2].PlayEffect("F_explosion041_smoke", 1f, 1, EffectLocation.Bottom);
				break;

			case 31:
				// The orbs and the device die on Client="BOTH" rows the client never reports.
				RemoveTrackActor(character, track, 4);
				RemoveTrackActor(character, track, 5);
				RemoveTrackActor(character, track, 6);
				RemoveTrackActor(character, track, 2);
				track.Actors[3].PlayEffect("F_explosion097", 1f, 1, EffectLocation.Bottom);
				break;

			case 33:
				track.Actors[3].PlayEffect("F_smoke023_red", 2f, 1, EffectLocation.Bottom);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
