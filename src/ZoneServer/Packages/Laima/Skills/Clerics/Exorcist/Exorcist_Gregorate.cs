using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages.Laima.Abilities.Clerics.Exorcist;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Exorcist
{
	[Package("laima")]
	[SkillHandler(SkillId.Exorcist_Gregorate)]
	public class Exorcist_Gregorate : ISelfSkillHandler, IDynamicCasted, ICancelSkillHandler
	{
		private const float AttackRange = 130f;
		private const int MaximumTargets = 5;
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 10;
		private const float MinimumDamageBonusRate = 0.20f;
		private const float MaximumDamageBonusRate = 0.30f;
		private const int BuffDurationSeconds = 60;
		private const int HitCount = 5;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster)
		{
			this.StopSkill(caster);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position position, Direction direction)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				this.StopSkill(character);
				return;
			}

			skill.IncreaseOverheat();
			character.SetAttackState(true);
			character.Direction = direction;

			try
			{
				Send.ZC_NORMAL.UpdateSkillEffect(character, 0, character.Position, character.Direction, Position.Zero);

				var skillLevel = Math.Clamp(skill.Level, MinimumSkillLevel, MaximumSkillLevel);
				var damageBonusRate = MinimumDamageBonusRate + (skillLevel - MinimumSkillLevel) * (MaximumDamageBonusRate - MinimumDamageBonusRate) / (MaximumSkillLevel - MinimumSkillLevel);
				var area = new Melia.Zone.Skills.SplashAreas.Circle(character.Position, AttackRange);
				var targets = character.Map.GetAttackableEnemiesIn(character, area)
						.Where(target => target != null && !target.IsDead)
						.OrderBy(target => character.Position.Get2DDistance(target.Position))
						.Take(MaximumTargets)
						.ToList();

				var hits = new List<SkillHitInfo>();

				foreach (var target in targets)
				{
					if (this.IsZombie(target))
					{
						var currentHp = Math.Max(1f, target.Properties.GetFloat(PropertyName.HP));
						var zombieModifier = SkillModifier.Default;
						zombieModifier.HitCount = HitCount;

						var zombieResult = SCR_SkillHit(character, target, skill, zombieModifier);
						zombieResult.Damage = currentHp;

						target.TakeDamage(currentHp, character);
						hits.Add(new SkillHitInfo(character, target, skill, zombieResult, TimeSpan.Zero, TimeSpan.Zero));
						continue;
					}

					target.StartBuff(BuffId.GregorateATK_Buff, skillLevel, damageBonusRate, TimeSpan.FromSeconds(BuffDurationSeconds), character, skill.Id);

					var modifier = SkillModifier.Default;
					modifier.HitCount = HitCount;
					modifier.DamageMultiplier *= Exorcist_GregorateEnhanceAbility.GetDamageMultiplier(character);

					var result = SCR_SkillHit(character, target, skill, modifier);

					if (result.Result != HitResultType.Dodge && result.Damage > 0)
						target.TakeDamage(result.Damage, character);

					hits.Add(new SkillHitInfo(character, target, skill, result, TimeSpan.Zero, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(character, hits);
			}
			finally
			{
				this.StopSkill(character);
			}
		}

		private void StopSkill(ICombatEntity caster)
		{
			if (caster is not Character character)
				return;

			character.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(character);
		}

		private bool IsZombie(ICombatEntity target)
		{
			if (target is not Mob mob || mob.Data == null)
				return false;

			return mob.Data.ClassName.Contains("zombie", StringComparison.OrdinalIgnoreCase);
		}
	}
}
