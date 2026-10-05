using System;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Packages.Laima.Skills.Archers.PiedPiper
{
	[Package("laima")]
	[SkillHandler(SkillId.PiedPiper_HypnotischeFlote)]
	public class PiedPiper_HypnotischeFlote : IGroundSkillHandler, IDynamicCasted
	{
		private const float SkillRange = 160f;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const int BaseTargetCount = 1;
		private static readonly TimeSpan MaximumDuration = TimeSpan.FromSeconds(10);
		private static readonly TimeSpan BaseConfusionDuration = TimeSpan.FromSeconds(3);
		private static readonly TimeSpan SelfHypnosisConfusionDuration = TimeSpan.FromSeconds(6);

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
			var allowsElite = caster is Character eliteCharacter && eliteCharacter.IsAbilityActive(AbilityId.PiedPiper6);
			var hasSelfHypnosis = caster is Character selfHypnosisCharacter && selfHypnosisCharacter.IsAbilityActive(AbilityId.PiedPiper5);
			var confusionDuration = hasSelfHypnosis ? SelfHypnosisConfusionDuration : BaseConfusionDuration;
			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, SkillRange).OfType<Mob>().Where(monster => !monster.IsDead && this.IsValidTarget(monster, allowsElite)).OrderBy(monster => caster.Position.Get2DDistance(monster.Position)).Take(maximumTargets).ToList();

			caster.StartBuff(BuffId.Fluting_Buff, skillLevel, 0, MaximumDuration, caster, skill.Id);

			foreach (var target in targets)
				target.StartBuff(BuffId.Fluting_DeBuff, skillLevel, (float)confusionDuration.TotalSeconds, MaximumDuration, caster, skill.Id);

			skill.IncreaseOverheat();

			if (caster is Character character)
				PiedPiperHamelnNagetierHelper.TrySummonMouse(character);

			caster.SetAttackState(false);
		}

		private bool IsValidTarget(Mob monster, bool allowsElite)
		{
			if (monster.Rank == MonsterRank.Normal)
				return true;

			return allowsElite && monster.Rank == MonsterRank.Elite;
		}
	}
}
