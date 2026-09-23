//--- Melia Script ----------------------------------------------------------
// The Cerberus of the Felon Prison
//--- Description -----------------------------------------------------------
// The red energy breaks the cage, and the Cerberus inside breaks free.
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

[TrackScript("d_prison_62_3_kerberos")]
public class DPrison623KerberosTrack : TrackScript
{
	protected override void Load()
	{
		SetId("d_prison_62_3_kerberos");
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();

		character.Movement.MoveTo(new Position(1725.88f, 997.07f, -265.70f));
		actors.Add(character);

		var prop = new TrackActorSpec { Ai = "BT_Dummy", Faction = FactionType.Our_Forces, Name = "UnvisibleName" };

		actors.Add(AddTrackActor(character, 58252, 1598.45, 821.61, 35.43, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 58252, 1701.16, 997.07, -85.88, 0, new TrackActorSpec { Ai = "TrackWaitMonster" }));
		actors.Add(AddTrackActor(character, 153103, 1726.38, 997.07, -144.82, 0, prop));
		actors.Add(AddTrackActor(character, 153104, 1525.98, 997.13, 165.84, 0, prop));
		actors.Add(AddTrackActor(character, 20024, 1726.38, 997.07, -144.82, 0, prop));
		actors.Add(AddTrackActor(character, 151099, 1614.74, 997.07, 30.09, 0, prop));
		actors.Add(AddTrackActor(character, 151099, 1655.46, 997.07, 92.80, 0, prop));
		actors.Add(AddTrackActor(character, 151100, 1614.37, 997.07, 77.65, 0, prop));
		actors.Add(AddTrackActor(character, 151100, 1750.72, 997.07, 74.29, 0, prop));
		actors.Add(AddTrackActor(character, 151101, 1750.84, 997.07, -12.73, 0, prop));
		actors.Add(AddTrackActor(character, 151101, 1682.78, 997.07, -59.46, 0, prop));
		actors.Add(AddTrackActor(character, 151100, 1707.47, 997.08, 96.15, 0, prop));
		actors.Add(AddTrackActor(character, 151099, 1614.69, 997.07, -22.87, 0, prop));
		actors.Add(AddTrackActor(character, 147469, 1428.69, 997.07, 163.72, 0, prop));

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 44:
				// The broken cage and the Cerberus' shadow leave on Client="BOTH" rows.
				RemoveTrackActor(character, track, 1);
				RemoveTrackActor(character, track, 4);
				for (var i = 6; i <= 14; ++i)
					RemoveTrackActor(character, track, i);

				character.AddonMessage(AddonMessage.NOTICE_Dm_Exclaimation, L("Cerberus appeared after destroying the wires!{nl}Defeat Cerberus"), 5);
				CreateBattleBoxInLayer(character, track);
				SetTrackTendency(character, track);
				break;
		}

		await base.OnProgress(character, track, frame);
	}
}
