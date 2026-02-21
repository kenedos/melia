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

[TrackScript("f_orchard_34_2_slider")]
public class forchard342slider : TrackScript
{
	protected override void Load()
	{
		SetId("f_orchard_34_2_slider");
		//SetMap("f_orchard_34_2");
		//CenterPos(-1682.90,346.20,56.95);
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();
		var mob0 = Shortcuts.AddMonster(character, 155058, "", "f_orchard_34_2", -1797.292, 368.7016, 187.4255, 60.71429);
		mob0.Visibility = ActorVisibility.Always;
		actors.Add(mob0);

		character.Movement.MoveTo(new Position(-1680.834f, 346.2044f, 58.14703f));
		actors.Add(character);

		var mob1 = Shortcuts.AddMonster(character, 147501, "UnvisibleName", "f_orchard_34_2", -1711.564, 346.2044, 59.92449, 0);
		actors.Add(mob1);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		Console.WriteLine("Frame: " + frame);
		switch (frame)
		{
			case 13:
			{
				break;
			}
			case 56:
			{
				character.JumpToPosition(-1141.0433f, -157.64105f, 38.907497f, 0.3f, 200);
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
