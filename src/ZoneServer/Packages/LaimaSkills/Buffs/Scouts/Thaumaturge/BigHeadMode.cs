using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Scouts.Thaumaturge
{
	[Package("laima-skills")]
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

			var intRatePercent = GetCaptionRatio(buff, 1);
			var intFlat = GetCaptionRatio(buff, 2);

			var baseInt = target.Properties.GetFloat(PropertyName.INT);
			var intFromRate = baseInt * intRatePercent / 100f;
			var totalIntBonus = (float)Math.Floor(intFlat + intFromRate);

			AddPropertyModifier(buff, target, PropertyName.INT_BM, totalIntBonus);

			if (caster.TryGetActiveAbilityLevel(AbilityId.Thaumaturge16, out var thaum16Level))
			{
				var quickCastDuration = TimeSpan.FromMilliseconds(thaum16Level * 3000);
				caster.StartBuff(BuffId.QuickCast_Buff, skillLevel, 0, quickCastDuration, caster, SkillId.Chronomancer_QuickCast);
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
