using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Wizards.Necromancer
{
	/// <summary>
	/// Handler for the Attack Weakened debuff, which lowers the target's
	/// physical and magical attack by 2% per Flesh: Demoralize level.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Debrave_Debuff)]
	public class Necromancer_Debrave_DebuffOverride : BuffHandler
	{
		private const float AttackReductionPerLevel = 0.02f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var reduction = AttackReductionPerLevel * buff.NumArg1;

			var patk = target.Properties.GetFloat(PropertyName.PATK);
			var matk = target.Properties.GetFloat(PropertyName.MATK);

			AddPropertyModifier(buff, target, PropertyName.PATK_BM, -MathF.Floor(patk * reduction));
			AddPropertyModifier(buff, target, PropertyName.MATK_BM, -MathF.Floor(matk * reduction));
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
