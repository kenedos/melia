using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Archers.PiedPiper
{
	[Package("laima")]
	[SkillHandler(SkillId.PiedPiper_Wiegenlied)]
	public class PiedPiper_Wiegenlied : IGroundSkillHandler
	{
		private const float SkillRange = 160f;
		private static readonly TimeSpan LullabyDuration = TimeSpan.FromSeconds(10);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity selectedTarget)
		{
			if (caster == null || caster.IsDead)
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.SetAttackState(false);
				return;
			}

			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position);

			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, SkillRange).Where(target => target != null && !target.IsDead).ToList();

			foreach (var target in targets)
			{
				target.StartBuff(BuffId.Sleep_Debuff, skill.Level, 0, LullabyDuration, caster, skill.Id);
				target.StartBuff(BuffId.Lullaby_Debuff, skill.Level, 1f, LullabyDuration, caster, skill.Id);
			}

			skill.IncreaseOverheat();

			if (caster is Character character)
				PiedPiperHamelnNagetierHelper.TrySummonMouse(character);

			caster.SetAttackState(false);
		}
	}
}
