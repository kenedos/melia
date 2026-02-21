using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Burn, Periodically inflicts 1 damage. Unable to use skills..
	/// </summary>
	[BuffHandler(BuffId.Scald)]
	public class Scald : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Caster is ICombatEntity caster)
			{
				var target = buff.Target;
				var damage = caster.Level * 2 / 5;

				var forceId = ForceId.GetNew();

				target.TakeDamage(damage, caster);

				var hitInfo = new HitInfo(caster, target, damage, HitResultType.Hit);
				hitInfo.ForceId = forceId;
				hitInfo.Type = HitType.Fire;

				Send.ZC_HIT_INFO(caster, target, hitInfo);
			}
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
