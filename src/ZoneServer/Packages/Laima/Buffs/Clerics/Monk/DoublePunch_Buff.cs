using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Clerics.Monk
{
	/// <summary>
	/// Handler for DoublePunch_Buff. Each stack increases final damage
	/// by 2%, up to 10 stacks. Stacks are gained per Double Punch use.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.DoublePunch_Buff)]
	public class DoublePunch_BuffOverride : BuffHandler, IBuffCombatAttackBeforeBonusesHandler
	{
		private const int MaxStacks = 10;
		private const float DamagePerStack = 0.02f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.OverbuffCounter > MaxStacks)
				buff.OverbuffCounter = MaxStacks;
		}

		public void OnAttackBeforeBonuses(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			skillHitResult.Damage *= 1f + DamagePerStack * buff.OverbuffCounter;
		}
	}
}
