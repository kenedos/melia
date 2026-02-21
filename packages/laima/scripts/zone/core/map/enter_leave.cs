using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Maps;
using Yggdrasil.Logging;

public class MapScript : GeneralScript
{

	protected override void Load()
	{
		Log.Debug("MapScripts Loaded");
	}

	[On("PlayerEnteredMap")]
	public void OnPlayerEnteredMap(object sender, PlayerEventArgs args)
	{
		//Log.Debug("On Player Entered Map");
		var character = args.Character;

		if (character.Map == null)
			return;

		switch (character.Map.Id)
		{
			case 1008:
				break;
			case 7000:
			case 7001:
			case 7002:
			{
				//Log.Debug($"On Player Entered Map: {character.Map.Id}");
				// Moved to CZ_LOAD_COMPLETE because this was too slow.
				break;
			}
		}
	}

	[On("PlayerLeftMap")]
	private void OnPlayerLeftMap(object sender, PlayerEventArgs args)
	{
		var character = args.Character;

		if (character.Map == null)
			return;
	}
}
