using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.PlagueDoctor
{
	/// <summary>
	/// Handler for Modafinil, which raises movement speed, up to twice as
	/// much the higher the caster's SPR is against the target's level.
	/// </summary>
	[Package("laima-skills")]
	[BuffHandler(BuffId.Modafinil_Buff)]
	public class PlagueDoctor_Modafinil_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var bonus = GetCaptionRatio(buff, 1);

			if (buff.Caster is ICombatEntity caster && target.Level > 0)
				bonus *= 1 + Math.Clamp(caster.Properties.GetFloat(PropertyName.MNA) / target.Level, 0, 1);

			UpdatePropertyModifier(buff, target, PropertyName.MSPD_BM, bonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}
	}
}
