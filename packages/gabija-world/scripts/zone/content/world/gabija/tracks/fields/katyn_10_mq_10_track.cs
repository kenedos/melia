//--- Melia Script ----------------------------------------------------------
// Another Owl
//--- Description -----------------------------------------------------------
// Mardas tells the Owl Chief Sculpture what he saw at Delmore Castle.
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

[TrackScript("KATYN_10_MQ_10_TRACK")]
public class Katyn10Mq10Track : TrackScript
{
	protected override void Load()
	{
		SetId("KATYN_10_MQ_10_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1985.74f, 200.96f, -476.83f));

		actors.Add(AddTrackActor(character, 20135, 1942.89, 200.96, -530.45, 95, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Owl Chief Sculpture") }));
		actors.Add(AddTrackActor(character, 151077, 2025, 200.96, -535, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Liaison Officer Mardas") }));
		actors.Add(character);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 68:
				track.Dialog.SetTitle(L("Liaison Officer Mardas"));
				track.Dialog.SetPortrait("Dlg_port_Mardas");
				StartDialog(track,
					L("My hometown, the Delmore Castle... is a truly beautiful place. The neighbors are so kind and the lord is also generous."),
					L("I was able to receive many rewards and apply for a long vacation thanks to my diligent service as a liaison officer. I was excited at the thought of hosting a feast with the neighbors."),
					L("But what I saw when I arrived home... was an empty town with nobody in sight. Well, to be completely honest... It wasn't totally empty."),
					L("Demons suddenly appeared from every direction. I ran like hell."),
					L("Some sights caught my eye even amongst all that chaos. Light beads being moved somewhere, the red gemstone... I still can't forget it."),
					L("That blackish red gemstone that was shining and shimmering in a nauseating way."),
					L("At first, I thought that it was a simple demon attack. That there would be survivors hiding somewhere... That my parents would have made it out safely..."),
					L("I mean, it's not like demon attacks are uncommon these days... But it wasn't just an attack. All those light beads... I never knew that they were the souls of the townspeople..."),
					L("I have nothing left to lose. I don't want to run away anymore."),
					L("Please, I beg you. I don't want to see the Divine Tree take the spirits of our people...")
				);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
