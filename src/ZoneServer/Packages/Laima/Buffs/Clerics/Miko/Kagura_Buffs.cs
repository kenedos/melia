using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Miko
{
	/// <summary>
	/// Stores Kagura's outgoing damage bonus.
	/// The calculation is applied before SCR_SkillHit.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.KaguraDance_Buff)]
	public class KaguraDance_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}
	}

	/// <summary>
	/// Reduces critical resistance by 3% per Nightingale Dance level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Kagura_Debuff)]
	public class Kagura_DebuffOverride : BuffHandler
	{
		private const float CriticalResistanceReductionPerLevel = 0.03f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start)
				return;

			var abilityLevel = Math.Clamp((int)buff.NumArg1, 0, 5);
			if (abilityLevel == 0)
				return;

			var currentCriticalResistance = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.CRTDR));
			var reduction = currentCriticalResistance * CriticalResistanceReductionPerLevel * abilityLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM, -reduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM);
		}
	}
}
