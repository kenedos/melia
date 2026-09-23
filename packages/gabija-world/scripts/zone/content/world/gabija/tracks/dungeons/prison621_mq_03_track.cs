//--- Melia Script ----------------------------------------------------------
// The Cursed Idol of the Torture Material Room
//--- Description -----------------------------------------------------------
// The idol calls the prison's demons down on whoever disturbs it.
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

[TrackScript("PRISON621_MQ_03_TRACK")]
public class Prison621Mq03Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON621_MQ_03_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-1259.56f, 407.43f, 100.10f));
		actors.Add(character);

		actors.Add(AddTrackActor(character, 47150, -1376.95, 407.43, 92.11, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Cursed Idol") }));

		var demon = new TrackActorSpec { Ai = "TrackWaitMonster" };
		actors.Add(AddTrackActor(character, 57991, -1424.15, 418.61, 409.45, 48, demon));
		actors.Add(AddTrackActor(character, 57991, -1357.57, 407.43, 424.11, 66, demon));
		actors.Add(AddTrackActor(character, 57991, -1183.07, 427.43, 342.58, 54, demon));
		actors.Add(AddTrackActor(character, 57991, -1620.15, 427.43, 295.96, 47, demon));
		actors.Add(AddTrackActor(character, 57991, -1586.09, 427.43, 2.11, 38, demon));
		actors.Add(AddTrackActor(character, 58002, -1434.92, 427.43, -138.46, 87, demon));
		actors.Add(AddTrackActor(character, 58002, -1609.96, 407.43, 113.83, 86, demon));
		actors.Add(AddTrackActor(character, 58002, -1257.83, 427.43, -126.23, 100, demon));
		actors.Add(AddTrackActor(character, 58002, -1111.06, 427.43, -88.87, 105, demon));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[1].AttachEffect("F_pattern008_violet_loop", 1, EffectLocation.Bottom);
				break;

			case 14:
				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("The demons that sensed someone's presence are coming!"), 5);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
