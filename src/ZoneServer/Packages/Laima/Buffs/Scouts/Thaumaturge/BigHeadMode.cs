using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Thaumaturge
{
	[Package("laima")]
	[BuffHandler(BuffId.BigHeadMode)]
	public class BigHeadModeOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = buff.Caster as ICombatEntity;
			if (caster == null) return;

			if (!target.IsBuffActive(BuffId.Thurisaz_Buff))
			{
				Send.ZC_NORMAL.SetActorRenderOption(target, "bigheadmode", true);
			}

			var skillLevel = buff.NumArg1;

			var byAbility = 1f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Thaumaturge15, out var thaum15Level))
				byAbility += thaum15Level * 0.005f;

			var intRatePercent = (20f + 2f * skillLevel) * byAbility;
			var intFlat = (25f + 2.5f * skillLevel) * byAbility;

			var baseInt = target.Properties.GetFloat(PropertyName.INT);
			var intFromRate = baseInt * intRatePercent / 100f;
			var totalIntBonus = (float)Math.Floor(intFlat + intFromRate);

			AddPropertyModifier(buff, target, PropertyName.INT_BM, totalIntBonus);

			if (caster.TryGetActiveAbilityLevel(AbilityId.Thaumaturge16, out var thaum16Level))
			{
				var quickCastDuration = TimeSpan.FromMilliseconds(thaum16Level * 3000);
				caster.StartBuff(BuffId.QuickCast_Buff, skillLevel, 0, quickCastDuration, caster);
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			Send.ZC_NORMAL.SetActorRenderOption(target, "bigheadmode", false);

			RemovePropertyModifier(buff, target, PropertyName.INT_BM);
		}
	}
}
