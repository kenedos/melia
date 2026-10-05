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
	[SkillHandler(SkillId.PiedPiper_Dissonanz)]
	public class PiedPiper_Dissonanz : IGroundSkillHandler
	{
		private const float SkillRange = 160f;
		private static readonly TimeSpan BaseStunDuration = TimeSpan.FromSeconds(5);
		private static readonly TimeSpan SeakerStunDuration = TimeSpan.FromSeconds(7);

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

			var hasSeaker = caster is Character seakerCharacter && seakerCharacter.IsAbilityActive(AbilityId.PiedPiper1);
			var hasSoundWaveAttack = caster is Character soundWaveCharacter && soundWaveCharacter.IsAbilityActive(AbilityId.PiedPiper2);
			var stunDuration = hasSeaker ? SeakerStunDuration : BaseStunDuration;
			var soundWaveDuration = TimeSpan.FromSeconds(Math.Max(1, skill.Level) * 2);
			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, SkillRange).Where(target => target != null && !target.IsDead).ToList();

			foreach (var target in targets)
			{
				target.StartBuff(BuffId.Dissonanz_Stun_Debuff, skill.Level, 0, stunDuration, caster, skill.Id);
				target.StartBuff(BuffId.Stun, skill.Level, 0, stunDuration, caster, skill.Id);

				if (hasSoundWaveAttack)
					target.StartBuff(BuffId.Dissonanz_Debuff, skill.Level, 0, soundWaveDuration, caster, skill.Id);
			}

			skill.IncreaseOverheat();

			if (caster is Character character)
				PiedPiperHamelnNagetierHelper.TrySummonMouse(character);

			caster.SetAttackState(false);
		}
	}
}
