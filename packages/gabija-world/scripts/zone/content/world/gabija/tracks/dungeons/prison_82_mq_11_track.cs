//--- Melia Script ----------------------------------------------------------
// The Revelation of Kalejimas
//--- Description -----------------------------------------------------------
// The King's Jewels open the Confessional, and Goddess Laima speaks of
// the goddess the demon queen means to take.
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

[TrackScript("PRISON_82_MQ_11_TRACK")]
public class Prison82Mq11Track : TrackScript
{
	protected override void Load()
	{
		SetId("PRISON_82_MQ_11_TRACK");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(-422.57f, 413.80f, -387.74f));

		actors.Add(AddTrackActor(character, 151109, -424.00, 413.80, -345.00, 30, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Secret Device") }));
		actors.Add(character);
		actors.Add(AddTrackActor(character, 47234, -424.00, 413.80, -345.00, 0, new TrackActorSpec { Ai = "TrackWaitMonster", Faction = FactionType.Our_Forces, Name = L("Revelation Slate") }));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 13:
				track.Actors[0].AttachEffect("F_magic_prison_line_yellow", 2, EffectLocation.Bottom);
				break;

			case 32:
				track.Dialog.SetTitle(L("Goddess Laima"));
				track.Dialog.SetPortrait("Dlg_port_Raima");
				StartDialog(track,
					L("Savior."),
					L("Many are those who continue to sacrifice for the salvation of our world."),
					L("I knew well what destiny awaited Zanas..."),
					L("And I knew he himself was ready to accept it, yet I had no choice but to trust him with the revelation."),
					L("The power that led Zanas, a mere boy, unto the revelation..."),
					L("His last breath, his last prayer... were but for your own and our world's salvation."),
					L("He shall be remembered for as long as I live."),
					L("I understand this is a sin that cannot be forgiven."),
					L("But the lives being lost in our world every day are far too many."),
					L("A grave disaster is upon us..."),
					L("The same disaster that forced us to give up on the petrification curse, that nearly reduced the kingdom to ruins."),
					L("The goddess that could not be saved will soon be free of Her chains."),
					L("Past chains of a relinquished salvation crushing Her..."),
					L("The one taken by the curse of the demon queen shall descend once more as the Goddess of Destruction."),
					L("Savior. You must go to the caves holding the hidden goddess captive."),
					L("Please... protect Her from the demon queen.")
				);
				break;

			case 43:
				character.ServerMessage(L("Obtained the Revelation of Kalejimas Prison!"));
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
