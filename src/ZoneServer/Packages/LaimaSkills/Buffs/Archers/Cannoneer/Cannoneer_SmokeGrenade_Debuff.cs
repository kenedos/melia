using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Archers.Cannoneer
{
	/// <summary>
	/// Handler for Smoke Grenade, which lowers accuracy and evasion by the
	/// skill's ratio in percent.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.SmokeGrenade_Debuff)]
	public class Cannoneer_SmokeGrenade_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start)
				return;

			var target = buff.Target;
			var rate = GetCaptionRatio(buff, 2) / 100f;

			AddPropertyModifier(buff, target, PropertyName.HR_BM, -target.Properties.GetFloat(PropertyName.HR) * rate);
			AddPropertyModifier(buff, target, PropertyName.DR_BM, -target.Properties.GetFloat(PropertyName.DR) * rate);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.HR_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.DR_BM);
		}
	}
}
