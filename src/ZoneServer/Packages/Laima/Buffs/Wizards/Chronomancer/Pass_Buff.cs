using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	[Package("laima")]
	[BuffHandler(BuffId.Pass_Buff)]
	public class Pass_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			if (target is not Character character)
				return;

			var skillLevel = buff.NumArg1;
			var cooldownComponent = character.Components.Get<CooldownComponent>();
			if (cooldownComponent == null)
				return;

			var reductionSeconds = 5 + skillLevel;
			var reduction = TimeSpan.FromSeconds(reductionSeconds);

			var activeCooldowns = cooldownComponent.GetAll();
			foreach (var cooldown in activeCooldowns)
			{
				if (cooldown.Remaining > TimeSpan.Zero)
				{
					cooldownComponent.ReduceCooldown(cooldown.Id, reduction);
				}
			}

			if (buff.Caster is ICombatEntity caster && target.Handle != caster.Handle)
			{
				Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_BUFF_TEXT", (float)buff.Id, null, "Item");
			}
		}
	}
}
