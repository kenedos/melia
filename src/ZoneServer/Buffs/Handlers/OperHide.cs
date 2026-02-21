using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the OperHide Buff, which resets the target's threat levels.
	/// </summary>
	[BuffHandler(BuffId.OperHide)]
	public class OperHide : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.Map.AlertAis(new HateResetAlert(buff.Target));
		}
	}
}
