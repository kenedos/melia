//--- Melia Script ----------------------------------------------------------
// To the Government Ruins
//--- Description -----------------------------------------------------------
// Yane and Mihail lead the way past the barrier to the altar holding
// Melchioras.
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

[TrackScript("CASTLE65_3_MQ04_TRACK")]
public class Castle653Mq04Track : TrackScript
{
	protected override void Load()
	{
		SetId("CASTLE65_3_MQ04_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-697.74f, 92.56f, -355.52f));

		var device = new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(character);
		actors.Add(AddTrackActor(character, 155095, -718.17, 93.95, -372.99, 78, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Yane"), EndPosition = new Position(57.50f, 67.71f, -176.52f) }));
		actors.Add(AddTrackActor(character, 155094, -706.21, 93.88, -429.81, 77, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Mihail"), EndPosition = new Position(157.74f, 67.71f, -162.11f) }));
		actors.Add(AddTrackActor(character, 155096, -650.08, 93.08, -348.71, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelator Connor") }));
		actors.Add(AddTrackActor(character, 155106, 99.75, 67.71, -142.96, 0, device));
		actors.Add(AddTrackActor(character, 155113, 102.15, 67.71, -148.15, 2, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Mage Melchioras") }));
		actors.Add(AddTrackActor(character, 47106, -848.72, 93.08, -291.36, 0, device));
		actors.Add(AddTrackActor(character, 47106, -756.79, 93.08, -199.82, 0, device));
		actors.Add(AddTrackActor(character, 47106, -848.72, 93.08, -421.75, 0, device));
		actors.Add(AddTrackActor(character, 47106, -756.79, 93.08, -513.67, 0, device));
		actors.Add(AddTrackActor(character, 47106, -626.79, 93.08, -513.67, 0, device));
		actors.Add(AddTrackActor(character, 47106, -534.87, 93.08, -421.75, 0, device));
		actors.Add(AddTrackActor(character, 47106, -534.87, 93.08, -291.36, 0, device));
		actors.Add(AddTrackActor(character, 47106, -626.79, 93.08, -199.82, 0, device));
		actors.Add(AddTrackActor(character, 155117, -409.77, 83.01, -293.15, 0, device));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 0:
				track.Actors[4].AttachEffect("F_light081_ground_orange_loop2", 1.5f, EffectLocation.Bottom);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
