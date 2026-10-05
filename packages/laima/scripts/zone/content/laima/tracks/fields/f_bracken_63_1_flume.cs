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

[TrackScript("f_bracken_63_1_flume")]
public class fbracken631flume : TrackScript
{
	protected override void Load()
	{
		SetId("f_bracken_63_1_flume");
		//SetMap("f_bracken_63_1");
		//CenterPos(-256.82,978.94,2094.44);
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();
		character.Movement.MoveTo(new Position(-259.799f, 976.6173f, 2098.32f));
		actors.Add(character);

		var mob0 = Shortcuts.AddMonster(character, 155100, "", "f_bracken_63_1", -254.3194, 963.3553, 2126.975, 0);
		mob0.Visibility = ActorVisibility.Always;

		actors.Add(mob0);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 10:
			{
				track.Actors[0].AttachEffect("F_pc_jump_water", EffectLocation.Bottom);
				break;
			}
			case 91:
			{
				character.JumpToPosition(427.343994f, 5.266178f, 1800.973022f, 0.63f, 280);
				break;
			}
			case 99:
			{
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
