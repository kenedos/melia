using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Thaumaturge
{
	[Package("laima")]
	[BuffHandler(BuffId.SwellHands_Buff)]
	public class SwellHands_BuffOverride : BuffHandler
	{
		private const float BasePercent = 0.10f;
		private const float PercentPerLevel = 0.02f;
		private const float FlatPatkPerInt = 0.5f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var skillLevel = buff.NumArg1;
			var casterInt = buff.NumArg2;

			var byAbility = 1f;
			if (buff.Caster is ICombatEntity caster && caster.TryGetActiveAbilityLevel(AbilityId.Thaumaturge19, out var abilityLevel))
				byAbility += abilityLevel * 0.005f;

			var percentBonus = (BasePercent + PercentPerLevel * skillLevel) * byAbility;
			var flatBonus = (float)Math.Floor(FlatPatkPerInt * casterInt * byAbility);

			AddPropertyModifier(buff, target, PropertyName.PATK_RATE_BM, percentBonus);
			AddPropertyModifier(buff, target, PropertyName.PATK_BM, flatBonus);

			if (target.Handle != buff.Caster?.Handle)
			{
				Send.ZC_NORMAL.PlayTextEffect(target, buff.Caster, "SHOW_BUFF_TEXT", (float)buff.Id, null, "Item");
			}
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_BM);
		}
	}
}
