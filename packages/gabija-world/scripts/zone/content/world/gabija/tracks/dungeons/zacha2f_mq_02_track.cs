//--- Melia Script ----------------------------------------------------------
// The stone lanterns of the second floor
//--- Description -----------------------------------------------------------
// A guardian turns on the lanterns it was built to keep lit.
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

[TrackScript("ZACHA2F_MQ_02_TRACK")]
public class Zacha2fMq02Track : TrackScript
{
	protected override void Load()
	{
		SetId("ZACHA2F_MQ_02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-146.05f, 648.12f, -1431.69f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 47260, -188, 645, -1420, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 47253, -210, 649, -882, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, MaxHp = 150, Name = L("Royal Mausoleum Stone Lantern") }));
		actors.Add(AddTrackActor(character, 47253, 3, 649, -722, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, MaxHp = 150, Name = L("Royal Mausoleum Stone Lantern") }));
		actors.Add(AddTrackActor(character, 47253, -208, 649, -566, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, MaxHp = 150, Name = L("Royal Mausoleum Stone Lantern") }));
		actors.Add(AddTrackActor(character, 47253, -379, 649, -723, 0, new TrackActorSpec { Ai = "MON_DUMMY", Faction = FactionType.Our_Forces, MaxHp = 150, Name = L("Royal Mausoleum Stone Lantern") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 7:
				character.ServerMessage(L("Protect the Royal Mausoleum's Stone Lanterns from the guardians!"));
				break;
			case 19:
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
