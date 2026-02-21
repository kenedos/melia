using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Aukuras, increases HP and SP Regen speed.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Aukuras_Buff)]
	public class Aukuras_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			var hpRegenSpeed = 10000f;
			var hpRegen = buff.NumArg2;

			AddPropertyModifier(buff, target, PropertyName.RHPTIME_BM, hpRegenSpeed);
			AddPropertyModifier(buff, target, PropertyName.RHP_BM, hpRegen);

			// Only apply SP regen to player characters (monsters don't have SP properties)
			if (target is not Mob)
			{
				var spRegenSpeed = 10000f;
				var spRegen = buff.NumArg2;

				AddPropertyModifier(buff, target, PropertyName.RSPTIME_BM, spRegenSpeed);
				AddPropertyModifier(buff, target, PropertyName.RSP_BM, spRegen);
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			RemovePropertyModifier(buff, target, PropertyName.RHPTIME_BM);
			RemovePropertyModifier(buff, target, PropertyName.RHP_BM);

			// Only remove SP properties if they were added (non-monsters)
			if (target is not Mob)
			{
				RemovePropertyModifier(buff, target, PropertyName.RSPTIME_BM);
				RemovePropertyModifier(buff, target, PropertyName.RSP_BM);
			}
		}
	}
}
