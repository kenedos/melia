using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Buffs.Wizards.Sage
{
	[BuffHandler(BuffId.MissileHole_Buff)]
	public class Sage_MissileHole_BuffOverride : BuffHandler
	{
		public override void OnStart(Buff buff)
		{
		}

		public override void OnEnd(Buff buff)
		{
		}

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.MissileHole_Buff)]
		public static void OnAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target == null || skill?.Data == null)
				return;

			if (!target.IsBuffActive(BuffId.MissileHole_Buff))
				return;

			if (skill.Data.ClassType != SkillClassType.Missile)
				return;

			if (skillHitResult.Damage <= 0f)
				return;

			skillHitResult.Damage = 1f;
		}
	}
}
