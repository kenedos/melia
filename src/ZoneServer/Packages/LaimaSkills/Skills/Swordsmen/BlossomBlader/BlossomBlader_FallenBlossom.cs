using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Handler for the Blossom Blader skill Fallen Blossom, which teleports
	/// to the closest enemy carrying the Blossom Blader's Flowering and
	/// strikes it once per stack, lifting the Flowering.
	/// </summary>
	/// <remarks>
	/// [Arts] Fallen Blossom: Blossom Flows chains it through up to 5
	/// flowering enemies.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.BlossomBlader_FallenBlossom)]
	public class BlossomBlader_FallenBlossomOverride : IGroundSkillHandler
	{
		private const int FlowsTargets = 5;
		private const int SlowStacks = 5;
		private const int SealStacks = 3;
		private const float Range = 130f;
		private static readonly TimeSpan SlowDuration = TimeSpan.FromSeconds(5);
		private static readonly TimeSpan SilenceDuration = TimeSpan.FromSeconds(3);
		private static readonly TimeSpan ChainDelay = TimeSpan.FromMilliseconds(300);
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(100);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var maxTargets = caster.IsAbilityActive(AbilityId.Blossomblader21) ? FlowsTargets : 1;
			var targets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, Range)
				.Where(a => BlossomBladerSkillHelper.GetFloweringStacks(caster, a) > 0)
				.Take(maxTargets)
				.ToList();

			if (targets.Count == 0)
			{
				caster.ServerMessage(Localization.Get("There is no enemy affected by your Flowering nearby."));
				Send.ZC_SKILL_CAST_CANCEL(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targets[0].Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targets[0].Position, ForceId.GetNew(), null);

			skill.Run(this.Strike(skill, caster, targets));
		}

		/// <summary>
		/// Teleports to each target in turn and strikes it.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targets"></param>
		/// <returns></returns>
		private async Task Strike(Skill skill, ICombatEntity caster, List<ICombatEntity> targets)
		{
			for (var i = 0; i < targets.Count; i++)
			{
				if (i > 0)
					await skill.Wait(ChainDelay);

				var target = targets[i];
				if (caster.IsDead || target.IsDead || target.Map != caster.Map)
					continue;

				var stacks = BlossomBladerSkillHelper.GetFloweringStacks(caster, target);
				if (stacks == 0)
					continue;

				caster.SetPosition(target.Position);
				caster.TurnTowards(target);

				var modifier = SkillModifier.MultiHit(stacks + BlossomBladerSkillHelper.GetBlossomShowerHits(caster));
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				Send.ZC_SKILL_HIT_INFO(caster, new SkillHitInfo(caster, target, skill, skillHitResult, HitDelay, TimeSpan.Zero));

				target.StopBuff(BuffId.Flowering_Debuff);

				if (stacks >= SlowStacks)
					target.StartBuff(BuffId.Common_Slow, skill.Level, 0, SlowDuration, caster, skill.Id);

				if (stacks >= SealStacks && caster.IsAbilityActive(AbilityId.Blossomblader4))
					target.StartBuff(BuffId.Silence_Debuff, skill.Level, 0, SilenceDuration, caster, skill.Id);
			}
		}
	}
}
