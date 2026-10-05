using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Miko
{
	/// <summary>
	/// Reduces critical resistance by 2% per Hamaya level.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Hamaya_TakeDamage)]
	public class Hamaya_TakeDamageOverride : BuffHandler
	{
		private const float CriticalResistanceReductionPerLevel = 0.02f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start)
				return;

			var skillLevel = Math.Max(0, (int)buff.NumArg1);
			if (skillLevel == 0)
				return;

			var currentCriticalResistance = Math.Max(0f, buff.Target.Properties.GetFloat(PropertyName.CRTDR));
			var reduction = currentCriticalResistance * CriticalResistanceReductionPerLevel * skillLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM, -reduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CRTDR_BM);
		}
	}
}
