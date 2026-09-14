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
using Melia.Zone.World.Tracks;
using Yggdrasil.Logging;

[TrackScript("cart_slide")]
public class cartslide : TrackScript
{
	protected override void Load()
	{
		SetId("cart_slide");
		//SetMap("None");
		//CenterPos(-2264.81,788.79,1907.63);
	}

	public override IActor[] OnStart(Character character, Track track)
	{
		base.OnStart(character, track);

		var actors = new List<IActor>();
		var mob0 = Shortcuts.AddNpc(character, 147474, "UnvisibleName", character.Map.ClassName, -2562.85, 788.7881, 1885.766, 187.8571);
		actors.Add(mob0);

		character.Position = new Position(-2246.587f, 788.7881f, 1924.998f);
		actors.Add(character);

		var mob1 = Shortcuts.AddNpc(character, 20150, "UnvisibleName", character.Map.ClassName, -2319.807, 801.6678, 2045.689, 1);
		actors.Add(mob1);

		var mob2 = Shortcuts.AddNpc(character, 147355, "UnvisibleName", character.Map.ClassName, -2327.734, 801.6678, 2031.337, 1);
		actors.Add(mob2);

		return actors.ToArray();
	}

	public override async Task OnProgress(Character character, Track track, int frame)
	{
		switch (frame)
		{
			case 64:
			{
				//TRACK_BASICLAYER_MOVE();
				character.StopLayer();
				break;
			}
			default:
				Log.Warning("OnProgress: Unsupported frame {0} called from {1}.", frame, this.TrackId);
				break;
		}
		await base.OnProgress(character, track, frame);
	}
}
