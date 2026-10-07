using System;
using System.Collections.Generic;
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
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Templar skill Retribution, which spends the
	/// Templar's Uplift for extra damage, stunning at the third stage.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Templer_Retribution)]
	public class Templer_RetributionOverride : IGroundSkillHandler
	{
		/// <summary>
		/// Number of hits Retribution deals per target.
		/// </summary>
		public const int HitCount = 5;

		private const float HitRadius = 80f;
		private static readonly float[] UpliftDamageRates = [0f, 0.20f, 0.60f, 1.00f];
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(900);
		private static readonly TimeSpan StunDuration = TimeSpan.FromSeconds(2);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
				targetPos = farPos;

			var uplift = TemplarSkillHelper.ConsumeUplift(caster);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, targetPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, originPos.GetDirection(targetPos), targetPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, targetPos, uplift));
		}

		/// <summary>
		/// Strikes the targets around the target position.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="targetPos"></param>
		/// <param name="uplift"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, Position targetPos, int uplift)
		{
			await skill.Wait(HitDelay);

			var hits = new List<SkillHitInfo>();
			var targets = caster.Map.GetAttackableEnemiesIn(caster, new CircleF(targetPos, HitRadius));

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(HitCount);
				modifier.DamageMultiplier += UpliftDamageRates[uplift];

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);

				if (uplift >= TemplarSkillHelper.MaxUplift && !target.IsDead)
					target.StartBuff(BuffId.Stun, 1, 0, StunDuration, caster, skill.Id);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);

			if (caster.IsAbilityActive(AbilityId.Templar20))
				SkillCreatePad(caster, skill, targetPos, 0f, PadName.Templer_Retribution_Residue);
		}
	}
}
