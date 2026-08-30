using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Attack Weakened debuff, which lowers the target's
	/// physical and magical attack.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Debrave_Debuff)]
	public class Necromancer_Debrave_DebuffOverride : BuffHandler
	{
		private const float AttackReduction = 0.20f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			var patk = target.Properties.GetFloat(PropertyName.PATK);
			var matk = target.Properties.GetFloat(PropertyName.MATK);

			AddPropertyModifier(buff, target, PropertyName.PATK_BM, -MathF.Floor(patk * AttackReduction));
			AddPropertyModifier(buff, target, PropertyName.MATK_BM, -MathF.Floor(matk * AttackReduction));
			target.InvalidateProperties();
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MATK_BM);
			buff.Target.InvalidateProperties();
		}
	}
}
