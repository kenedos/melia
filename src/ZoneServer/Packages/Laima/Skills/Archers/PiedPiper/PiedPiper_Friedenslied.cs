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
	[SkillHandler(SkillId.PiedPiper_Friedenslied)]
	public class PiedPiper_Friedenslied : IGroundSkillHandler, IDynamicCasted
	{
		private const float SkillRange = 160f;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int BaseTargetCount = 1;
		private static readonly TimeSpan EffectDuration = TimeSpan.FromSeconds(5);

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

			var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
			var maximumTargets = BaseTargetCount + (skillLevel / 2);
			var hasCancelBuff = caster is Character cancelBuffCharacter && cancelBuffCharacter.IsAbilityActive(AbilityId.PiedPiper9);
			var hasInvincibility = caster is Character invincibilityCharacter && invincibilityCharacter.IsAbilityActive(AbilityId.PiedPiper10);
			var allies = caster.Map.GetCharacters(character => character != null && !character.IsDead && character.Layer == caster.Layer && !caster.IsEnemy(character) && caster.Position.Get2DDistance(character.Position) <= SkillRange).OrderBy(character => caster.Position.Get2DDistance(character.Position)).Take(maximumTargets).ToList();
			var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, SkillRange).Where(enemy => enemy != null && !enemy.IsDead).OrderBy(enemy => caster.Position.Get2DDistance(enemy.Position)).Take(maximumTargets).ToList();

			foreach (var ally in allies)
				ally.StartBuff(BuffId.Friedenslied_Buff, skillLevel, hasInvincibility ? 1f : 0f, EffectDuration, caster, skill.Id);

			foreach (var enemy in enemies)
				enemy.StartBuff(BuffId.Friedenslied_Debuff, skillLevel, hasCancelBuff ? 1f : 0f, EffectDuration, caster, skill.Id);

			skill.IncreaseOverheat();

			if (caster is Character character)
				PiedPiperHamelnNagetierHelper.TrySummonMouse(character);

			caster.SetAttackState(false);
		}
	}
}
