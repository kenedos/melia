using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;

namespace Melia.Zone.Buffs.Handlers.Clerics.Exorcist
{
	/// <summary>
	/// Handler for Rubric's slow, which lowers movement speed by 30%.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Rubric_DeBuff)]
	public class Exorcist_Rubric_DeBuffOverride : BuffHandler
	{
		private const float SlowRate = 0.30f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start)
				return;

			var reduction = buff.Target.Properties.GetFloat(PropertyName.MSPD) * SlowRate;
			AddPropertyModifier(buff, buff.Target, PropertyName.MSPD_BM, -reduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}

	/// <summary>
	/// Handler for [Arts] Rubric: Corruption's Attack Weakened, which lowers
	/// attack by 1% per stack.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Rubric_Hidden_Debuff)]
	public class Exorcist_Rubric_Hidden_DebuffOverride : BuffHandler
	{
		private const float AttackRatePerStack = 0.01f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var reduction = -AttackRatePerStack * buff.OverbuffCounter;

			UpdatePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM, reduction);
			UpdatePropertyModifier(buff, buff.Target, PropertyName.MATK_RATE_BM, reduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MATK_RATE_BM);
		}
	}
}
