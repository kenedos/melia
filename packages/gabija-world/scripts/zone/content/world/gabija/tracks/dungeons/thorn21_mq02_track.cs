//--- Melia Script ----------------------------------------------------------
// The altar at Thornbush Rest Place
//--- Description -----------------------------------------------------------
// The altar runs its purification while the corruption gathers around it.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("THORN21_MQ02_TRACK")]
public class Thorn21Mq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("THORN21_MQ02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		actors.Add(AddTrackActor(character, 20026, 931.00488, 208.0713, -1253.0702, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 20026, 1050.9071, 208.0713, -1330.4659, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 20026, 1063.7241, 208.0713, -1190.1243, 17, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Neutral }));
		actors.Add(AddTrackActor(character, 46213, 1012.1256, 208.0713, -1241.4298, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, MaxHp = 100, Name = L("Altar of Purification") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 1:
				character.ServerMessage(L("Protect the Altar of Purification from the monsters."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
