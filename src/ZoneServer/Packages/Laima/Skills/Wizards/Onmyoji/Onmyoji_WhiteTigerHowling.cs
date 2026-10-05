using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Onmyoji
{
	[Package("laima")]
	[SkillHandler(SkillId.Onmyoji_WhiteTigerHowling)]
	public class Onmyoji_WhiteTigerHowlingOverride : IGroundSkillHandler, IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int MinimumTargets = 5;
		private const int MaximumTargets = 14;
		private const int HitCount = 3;
		private const int HitIntervalMilliseconds = 100;
		private const float AttackRange = 150f;
		private const float FerociousDamageMultiplier = 1.25f;
		private const float VirtuousRoarRange = 200f;
		private static readonly TimeSpan FearDuration = TimeSpan.FromSeconds(7);
		private static readonly TimeSpan StunDuration = TimeSpan.FromSeconds(1);
		private static readonly TimeSpan VirtuousRoarDuration = TimeSpan.FromSeconds(10);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.TurnTowards(farPos);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, Position.Zero);

			var area = new Circle(caster.Position, AttackRange);
			var targetLimit = Math.Clamp(Math.Clamp(skill.Level, 1, 10) + 4, MinimumTargets, MaximumTargets);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area)
				.Where(target => target != null && !target.IsDead)
				.OrderBy(target => target.Position.Get2DDistance(caster.Position))
				.Take(targetLimit)
				.ToList();
			var ferociousEnabled = caster.IsAbilityActive(AbilityId.Onmyoji7);
			var hits = new List<SkillHitInfo>();

			foreach (var target in targets)
			{
				for (var hitIndex = 0; hitIndex < HitCount; hitIndex++)
				{
					var skillHitResult = SCR_SkillHit(caster, target, skill);
					if (ferociousEnabled && target.Race == RaceType.Widling)
						skillHitResult.Damage *= FerociousDamageMultiplier;

					target.TakeDamage(skillHitResult.Damage, caster);
					var hitDelay = skill.Data.DefaultHitDelay + TimeSpan.FromMilliseconds(hitIndex * HitIntervalMilliseconds);
					hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, hitDelay, TimeSpan.Zero));
				}

				target.StartBuff(BuffId.Fear, skill.Level, 0f, FearDuration, caster, skill.Id);
				target.StartBuff(BuffId.Stun, skill.Level, 0f, StunDuration, caster, skill.Id);
			}

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position, hits);

			if (caster.IsAbilityActive(AbilityId.Onmyoji8))
				this.ApplyVirtuousRoar(caster, skill);

			caster.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(caster);
		}

		private void ApplyVirtuousRoar(ICombatEntity caster, Skill skill)
		{
			if (caster is not Character character)
				return;

			var recipients = new List<Character> { character };
			var party = character.Connection?.Party;
			if (party != null)
				recipients.AddRange(party.GetPartyMembers().Where(member => member != null && member != character));

			foreach (var recipient in recipients
				.Where(recipient => !recipient.IsDead)
				.Where(recipient => recipient.Map == character.Map)
				.Where(recipient => recipient.Layer == character.Layer)
				.Where(recipient => recipient.Position.Get2DDistance(character.Position) <= VirtuousRoarRange)
				.Distinct())
			{
				recipient.StartBuff(BuffId.WhiteTigerHowling_Buff, skill.Level, 10f, VirtuousRoarDuration, character, skill.Id);
			}
		}
	}
}
