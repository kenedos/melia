using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Divine Might debuff. If the target is a
	/// Velnias-race monster, deals Holy magic damage to the caster.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DivineMight_Debuff)]
	public class Oracle_DivineMight_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;

			if (target.Race == RaceType.Velnias && buff.Caster is ICombatEntity caster)
			{
				var minMatk = caster.Properties.GetFloat(PropertyName.MINMATK);
				var maxMatk = caster.Properties.GetFloat(PropertyName.MAXMATK);
				var damage = RandomProvider.Get().Next((int)minMatk, (int)maxMatk + 1);

				caster.TakeSimpleHit(damage, target, SkillId.Oracle_DivineMight);
			}
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
