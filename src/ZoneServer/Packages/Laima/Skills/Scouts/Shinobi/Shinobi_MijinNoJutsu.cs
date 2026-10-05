using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Shinobi
{
	[Package("laima")]
	[SkillHandler(SkillId.Shinobi_Mijin_no_jutsu)]
	public class Shinobi_MijinNoJutsu : IGroundSkillHandler
	{
		private const int HitCount = 6;
		private const float SkillRadius = 100f;
		private static readonly TimeSpan FirstHitDelay = TimeSpan.FromMilliseconds(50);
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(70);
		private static readonly TimeSpan HitAnimationTime = TimeSpan.FromMilliseconds(50);
		private static readonly TimeSpan StealthDuration = TimeSpan.FromSeconds(8);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (caster is not DummyCharacter && !caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position);

			if (caster is not DummyCharacter)
				skill.IncreaseOverheat();

			skill.Run(this.Attack(skill, character));

			if (character is not DummyCharacter)
				ShinobiBunshinHelper.ReplicateSkill(character, skill, originPos, farPos, target);
		}

		private async Task Attack(Skill skill, Character caster)
		{
			await skill.Wait(FirstHitDelay);

			var searchArea = new Circle(caster.Position, SkillRadius);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, searchArea).Where(target => target != null && !target.IsDead).ToList();

			for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
			{
				foreach (var target in targets)
				{
					if (target == null || target.IsDead)
						continue;

					var skillHitResult = SCR_SkillHit(caster, target, skill);

					this.ApplyEnhanceAbility(caster, skillHitResult);
					this.ApplyBunshinDamageModifier(caster, skillHitResult);

					target.TakeDamage(skillHitResult.Damage, caster);
					ShinobiBunshinGenHelper.TryApply(caster, target, skill);

					var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, HitAnimationTime, TimeSpan.Zero);
					Send.ZC_SKILL_FORCE_TARGET(caster, target, skill, skillHit);
				}

				if (hitIndex < HitCount - 1)
					await skill.Wait(DelayBetweenHits);
			}

			this.ApplyStealth(caster, skill);
			caster.SetAttackState(false);
		}

		private void ApplyStealth(Character caster, Skill skill)
		{
			caster.StartBuff(BuffId.ShinobiCloaking_Buff, skill.Level, 0f, StealthDuration, caster, skill.Id);
		}

		private void ApplyBunshinDamageModifier(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			if (caster is DummyCharacter)
				skillHitResult.Damage *= 0.25f;
		}

		private void ApplyEnhanceAbility(ICombatEntity caster, SkillHitResult skillHitResult)
		{
			Character character;

			if (caster is DummyCharacter dummy)
				character = dummy.Owner;
			else
				character = caster as Character;

			if (character == null)
				return;

			if (!character.Abilities.TryGet(AbilityId.Shinobi3, out var ability) || !ability.Active)
				return;

			var enhanceRate = ability.Level * 0.005f;

			if (ability.Level >= 100)
				enhanceRate += 0.10f;

			skillHitResult.Damage *= 1f + enhanceRate;
		}
	}
}
