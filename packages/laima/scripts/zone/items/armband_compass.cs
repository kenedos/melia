using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using static Melia.Zone.Scripting.Shortcuts;

public class ArmbandCompassScripts : GeneralScript
{
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ARCHEOLOGY_COMPASS(Character character, Item item, string strArg, float numArg1, float numArg2)
	{
		var nearest = ArmbandBurrowRegistry.FindNearest(character.Map.ClassName, character.Position);

		if (nearest == null)
		{
			character.ServerMessage(L("The needle spins idle."));
			return ItemUseResult.OkayNotConsumed;
		}

		var (pos, dist) = nearest.Value;
		var dx = pos.X - character.Position.X;
		var dz = pos.Z - character.Position.Z;

		var ew = dx + dz;
		var ns = dz - dx;

		string direction;
		if (System.MathF.Abs(ns) >= System.MathF.Abs(ew))
			direction = ns >= 0 ? L("north") : L("south");
		else
			direction = ew >= 0 ? L("east") : L("west");

		character.ServerMessage(LF("The needle points {0}.", direction));
		return ItemUseResult.OkayNotConsumed;
	}
}
