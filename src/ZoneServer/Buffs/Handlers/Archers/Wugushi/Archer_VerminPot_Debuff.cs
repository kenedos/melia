using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Wugushi
{
	/// <summary>
	/// Handle for the VerminPot Debuff (PoisonPot), which ticks damage every second.
	/// </summary>
	/// <remarks>
	/// NumArg1: None
	/// NumArg2: None
	/// </remarks>
	[BuffHandler(BuffId.Archer_VerminPot_Debuff)]
	public class Archer_VerminPot_Debuff : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			if (buff.Caster is not ICombatEntity caster)
				return;

			if (buff.Target.IsDead)
				return;

			if (!caster.TryGetSkill(buff.SkillId, out var skill))
				return;

			buff.Target.TakeSkillHit(caster, skill);
			Crescendo_Bane_Buff.TryApply(buff);
		}
	}
}
