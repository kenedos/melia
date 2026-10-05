using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.Druid
{
	[Package("laima")]
	[BuffHandler(BuffId.Chortasmata_Buff)]
	public class Chortasmata_BuffOverride : BuffHandler
	{
		private const float ZemynaDetectionRange = 200f;
		private const float ZemynaHealingMultiplier = 1.10f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(1000);
		}

		public override void OnEnd(Buff buff)
		{
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target == null || buff.Target.IsDead)
				return;

			if (buff.Caster is not ICombatEntity caster || caster.IsDead)
				return;

			var healingPower = caster.Properties.GetFloat(PropertyName.HEAL_PWR);
			var healingAmount = healingPower * Math.Max(0f, buff.NumArg1) / 100f;

			if (this.IsZemynaActive(buff))
				healingAmount *= ZemynaHealingMultiplier;

			if (healingAmount > 0)
				buff.Target.Heal(healingAmount, 0);
		}

		private bool IsZemynaActive(Buff buff)
		{
			if (buff.Target.Map == null)
				return false;

			return buff.Target.Map.GetActorsInRange<Mob>(buff.Target.Position, ZemynaDetectionRange, mob =>
			{
				if (mob == null || mob.IsDead)
					return false;

				var className = mob.Data.ClassName;
				return string.Equals(className, "pcskill_wood_zemina", StringComparison.OrdinalIgnoreCase)
					|| string.Equals(className, "pcskill_wood_zemina2", StringComparison.OrdinalIgnoreCase);
			}).Any();
		}
	}
}
