using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	public class ScriptInvisibleEffect : Effect
	{
		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			Send.ZC_NORMAL.DelayEnterWorld(conn, actor, 0);
		}
	}
}
