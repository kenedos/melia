using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers.Common
{
	/// <summary>
	/// Handler for ScatterCaltrop_Debuff, which affects the movement speed on use.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ScatterCaltrop_Debuff)]
	public class ScatterCaltrop_DebuffOverride : BuffHandler
	{
		private const float BaseMspdRateDebuff = 0.15f;
		private const float MspdDebuffRatePerLevel = 0.05f;

		/// <summary>
		/// Starts buff, modifying the movement speed.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = buff.Caster;
			var moveSpeedRate = BaseMspdRateDebuff + (buff.NumArg2 * MspdDebuffRatePerLevel);
			moveSpeedRate = Math.Min(0.8f, moveSpeedRate);

			Send.ZC_SHOW_EMOTICON(target, "I_emo_slowdown", buff.Duration);
			Send.ZC_NORMAL.PlayTextEffect(target, caster, AnimationName.ShowBuffText, (float)BuffId.Common_Slow, null);

			var reduceMspd = target.Properties.GetFloat(PropertyName.MSPD) * moveSpeedRate;

			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -reduceMspd);
		}

		/// <summary>
		/// Ends the buff, resetting the movement speed.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
