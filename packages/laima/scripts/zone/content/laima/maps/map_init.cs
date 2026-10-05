//--- Melia Script ----------------------------------------------------------
// Map Initialization
//--- Description -----------------------------------------------------------
// Setups map specific tracks
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Scripting;
using Melia.Shared.Game.Const;
using Melia.Zone;
using Melia.Zone.Events;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Melia.Zone.Events.Arguments;
public class MapInitializationScript : GeneralScript
{
	[On("PlayerReady")]
	public void OnPlayerReady(object sender, PlayerEventArgs args)
	{
		var character = args.Character;
		var map = character.Map;

		if (map == null)
			return;

		switch (map.Id)
		{
			//case
		}
	}
}

