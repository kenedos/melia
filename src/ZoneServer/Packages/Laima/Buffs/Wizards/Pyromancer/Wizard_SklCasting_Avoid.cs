using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Packages.Laima.Buffs.Wizards.Psychokino
{
	[Package("laima")]
	[BuffHandler(BuffId.Wizard_SklCasting_Avoid)]
	public class Wizard_SklCasting_AvoidOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler
	{
		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return IsGravityPoleActive(buff, target) ? KnockResult.Prevent : KnockResult.Allow;
		}

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return IsGravityPoleActive(buff, target) ? KnockResult.Prevent : KnockResult.Allow;
		}

		private static bool IsGravityPoleActive(Buff buff, ICombatEntity target)
		{
			return target != null && !target.IsDead && target.Map != null
				&& buff.Target == target && buff.Caster == target
				&& buff.SkillId == SkillId.Psychokino_GravityPole
				&& target.TryGetSkill(SkillId.Psychokino_GravityPole, out var skill)
				&& target.IsCasting(skill);
		}
	}
}
