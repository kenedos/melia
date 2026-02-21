using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handler for Mon_Heal_Buff, heals target
	/// </summary>
	[BuffHandler(BuffId.Mon_Heal_Buff)]
	public class Mon_Heal_Buff : BuffHandler
	{
		/// <summary>
		/// Starts the buff, healing the target.
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = buff.Caster;
			var target = buff.Target;
			var skillId = buff.SkillId;
			var healAmount = buff.NumArg2;

			buff.UpdateTime = TimeSpan.FromMilliseconds(1000);

			target.Heal(healAmount, 0);

			var maxHp = target.Properties.GetFloat(PropertyName.MHP);
			Send.ZC_HEAL_INFO(target, healAmount, maxHp, HealType.Hp);
		}

		/// <summary>
		/// Heals over time
		/// </summary>
		/// <param name="buff"></param>
		public override void WhileActive(Buff buff)
		{
			var caster = buff.Caster;
			var target = buff.Target;
			var skillId = buff.SkillId;
			var healAmount = buff.NumArg2;

			target.Heal(healAmount, 0);

			var maxHp = target.Properties.GetFloat(PropertyName.MHP);
			Send.ZC_HEAL_INFO(target, healAmount, maxHp, HealType.Hp);
		}
	}
}
