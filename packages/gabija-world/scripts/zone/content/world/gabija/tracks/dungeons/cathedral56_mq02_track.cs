//--- Melia Script ----------------------------------------------------------
// Writing the Demon Transformation Scroll
//--- Description -----------------------------------------------------------
// The Altar of Intelligence writes the scroll by itself, if the demons are
// kept off it long enough.
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

[TrackScript("CHATHEDRAL56_MQ02_TRACK")]
public class Cathedral56Mq02Track : TrackScript
{
	private readonly static QuestId Mq02 = new QuestId(20330);

	protected override void Load()
	{
		SetId("CHATHEDRAL56_MQ02_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1458.91f, 0.50f, -472.21f));

		actors.Add(AddTrackActor(character, 151024, 1414, 0.50, -471, 0, new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, MaxHp = 120, Name = L("Altar of Intelligence") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 2:
				character.ServerMessage(L("Defend against attacks from Naktis' servants until the transformation scroll is complete!"));
				break;

			case 9:
				// The client runs the defence as a minigame; the server plays
				// the scroll out and leaves the fight to the map's own demons.
				character.Quests.CompleteObjective(Mq02, "completeTheScroll");
				character.ServerMessage(L("The scroll has finished writing itself."));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
