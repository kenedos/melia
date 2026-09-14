using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Tracks;
using Yggdrasil.Logging;

[TrackScript("f_orchard_34_2_Flume")]
public class forchard342Flume : TrackScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2_Flume");
		//SetMap("f_orchard_34_2");
		//CenterPos(1933.90,399.69,-455.72);
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();
		character.Movement.MoveTo(new Position(1933.662f, 399.6894f, -455.4826f));
		actors.Add(character);

		var mob0 = Shortcuts.AddMonster(character, 155060, "", "f_orchard_34_2", 2105.654, 386.3208, -383.9777, 70.5);
		mob0.Visibility = ActorVisibility.Always;
		actors.Add(mob0);

		var mob1 = Shortcuts.AddMonster(character, 155061, "", "f_orchard_34_2", 2135.453, 388.0683, -395.2737, 0);
		actors.Add(mob1);

		var mob2 = Shortcuts.AddMonster(character, 147501, "UnvisibleName", "f_orchard_34_2", 1906.933, 399.6894, -451.2874, 0);
		actors.Add(mob2);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 19:
			{
				break;
			}
			// End
			case 140:
			{
				character.JumpToPosition(-154, -147, 18, 0.5f, 300);
				break;
			}
			default:
				Log.Warning("OnProgress: Unsupported frame {0} called from {1}.", frame, this.TrackId);
				break;
		}
		await base.OnProgress(character, track, frame);
	}

	public override void OnComplete(Character character, Track track)
	{
		character.StopLayer();

		foreach (var actor in track.Actors)
		{
			if (actor != character && actor is IMonster monster)
				character.Map.RemoveMonster(monster);
		}
	}
}
