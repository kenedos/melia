using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Iron Hook buff on the caster.
	/// Locks attack and movement while holding the hook.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.IronHook)]
	public class IronHook_BuffOverride : BuffHandler
	{
		public override void OnEnd(Buff buff)
		{
			if (buff.Caster is not ICombatEntity caster)
				return;

			caster.Interrupt();

			if (buff.Vars.TryGet<List<ICombatEntity>>("Melia.IronHook.Targets", out var targets))
			{
				foreach (var target in targets)
				{
					Send.ZC_NORMAL.RemoveHookEffect(caster);
					Send.ZC_NORMAL.RemoveEffectByName(caster, "Warrior_Pull", true);

					target.RemoveBuff(BuffId.IronHooked);
				}
				buff.Vars.Remove("Melia.IronHook.Targets");
			}
		}
	}
}
