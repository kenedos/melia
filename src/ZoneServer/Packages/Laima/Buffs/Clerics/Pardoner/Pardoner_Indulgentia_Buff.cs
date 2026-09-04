using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Pardoner
{
	/// <summary>
	/// Handler for the Indulgentia buff.
	/// Recovers HP continuously while the buff is active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Indulgentia_Buff)]
	public class Pardoner_Indulgentia_BuffOverride : BuffHandler
	{
		public override void WhileActive(Buff buff)
		{
			var target = buff.Target;

			if (target == null || target.IsDead)
				return;

			if (buff.Caster is not ICombatEntity caster || caster.IsDead)
				return;

			if (!caster.TryGetSkill(SkillId.Pardoner_Indulgentia, out var skill))
				skill = new Skill(caster, SkillId.Pardoner_Indulgentia, (int)buff.NumArg1);

			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var healAmount = SCR_CalculateHeal(caster, target, skill, new SkillModifier(), new SkillHitResult());

			healAmount *= GetCaptionRatio(buff, 2) / 100f;
			healAmount *= buff.NumArg2;

			if (healAmount > 0)
				target.Heal(healAmount, 0);
		}
	}
}
