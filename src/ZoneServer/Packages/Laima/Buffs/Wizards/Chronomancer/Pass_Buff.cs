using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	[Package("laima")]
	[BuffHandler(BuffId.Pass_Buff)]
	public class Pass_BuffOverride : BuffHandler
	{
		private const float BaseReduction = 0.30f;
		private const float ReductionPerLevel = 0.03f;
		private const float MaxReduction = 0.80f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			if (target is not Character character)
				return;

			var skillLevel = buff.NumArg1;
			var cooldownComponent = character.Components.Get<CooldownComponent>();
			if (cooldownComponent == null)
				return;

			var reductionRate = BaseReduction + ReductionPerLevel * skillLevel;

			if (buff.NumArg2 == 1)
				reductionRate *= 1.5f;

			reductionRate = Math.Min(reductionRate, MaxReduction);

			var bonusReduction = TimeSpan.Zero;
			if (buff.Caster is ICombatEntity abilCaster
				&& abilCaster.TryGetActiveAbilityLevel(AbilityId.Chronomancer9, out var chrono9Level)
				&& RandomProvider.Get().Next(1, 101) <= chrono9Level * 0.5f)
			{
				bonusReduction = TimeSpan.FromSeconds(3);
			}

			var activeCooldowns = cooldownComponent.GetAll();
			foreach (var cooldown in activeCooldowns)
			{
				if (cooldown.Id == CooldownId.Pass)
					continue;

				if (cooldown.Remaining > TimeSpan.Zero)
				{
					var reduction = TimeSpan.FromTicks((long)(cooldown.Remaining.Ticks * reductionRate)) + bonusReduction;
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
