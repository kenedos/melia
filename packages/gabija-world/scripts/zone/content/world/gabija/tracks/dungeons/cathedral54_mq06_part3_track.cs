//--- Melia Script ----------------------------------------------------------
// The Revelation of the Great Cathedral
//--- Description -----------------------------------------------------------
// Laima speaks from the slate about the world on the other side of the crack.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Quests;
using Melia.Zone.World.Tracks;
using static Melia.Zone.Scripting.Shortcuts;

[TrackScript("CHATHEDRAL54_MQ06_PART3_TRACK")]
public class Cathedral54Mq06Part3Track : TrackScript
{
	private readonly static QuestId Mq06Part3 = new QuestId(20341);

	protected override void Load()
	{
		SetId("CHATHEDRAL54_MQ06_PART3_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1547.90f, 0.04f, -1515.45f));

		actors.Add(character);
		actors.Add(AddTrackActor(character, 20025, 1502.64, -0.35, -1223.75, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces }));
		actors.Add(AddTrackActor(character, 47234, 1547.04, 11.06, -1353.86, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelation Slate") }));
		actors.Add(AddTrackActor(character, 154043, 1547.04, 11.06, -1353.86, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Altar of the Revelation") }));
		actors.Add(AddTrackActor(character, 154042, 1519.97, 0.19, -1135.04, 4, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Candlestick") }));
		actors.Add(AddTrackActor(character, 154044, 1554.03, 0.19, -1151.27, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Angel Statue") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 62:
				track.Dialog.SetTitle(L("Goddess Laima"));
				track.Dialog.SetPortrait("Dlg_port_Raima");
				StartDialog(track,
					L("If this revelation reaches you, then it would mean that Maven accomplished his mission splendidly."),
					L("Here, I would like to tell you about the principles of a world different from this one."),
					L("There is another world beyond the world that you live in. You cannot cross both worlds unless you have supreme powers."),
					L("At least, that was before the Divine Tree emerged. The aftermath of the disaster left a wound, opening cracks that divided these two worlds."),
					L("And then, the demons freely crossed between worlds through the dimensional crack. The dimensional crack is scattered across many places."),
					L("But the biggest problem would be the one in Demon Prison where the most dangerous demons are imprisoned."),
					L("If the demons cross the dimensional crack, and join hands together with their brethren at the Demon Prison, the world will be in for another kind of threat.")
				);
				break;

			case 70:
				StartDialog(track,
					L("I've told sister Vakarine to promise me that she'd prepare for the worst."),
					L("But the uprising of the demons that broke through might be hindering her."),
					L("The breach of corruption, once open, will continue to expand. Revelator, please help Vakarine as soon as possible.")
				);
				break;

			case 89:
				// The slate the revelation leaves behind is spent.
				RemoveTrackActor(character, track, 2);
				character.Quests.CompleteObjective(Mq06Part3, "hearTheRevelation");
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
