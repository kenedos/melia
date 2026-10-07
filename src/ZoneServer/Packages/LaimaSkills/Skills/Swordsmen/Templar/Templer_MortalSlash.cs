using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Templar
{
	/// <summary>
	/// Handler for the Templar skill Mortal Slash, which spends the
	/// Templar's Uplift for extra damage and defense penetration.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Templer_MortalSlash)]
	public class Templer_MortalSlashOverride : IGroundSkillHandler
	{
		private const int HitCount = 4;
		private const float ShieldDefenseRate = 0.30f;
		private static readonly float[] UpliftDamageRates = [0f, 0.20f, 0.60f, 1.00f];
		private static readonly float[] UpliftPenetrationRates = [0f, 0.15f, 0.20f, 0.25f];
		private static readonly TimeSpan HitDelay = TimeSpan.FromMilliseconds(50);
		private static readonly TimeSpan AniTime = TimeSpan.FromMilliseconds(250);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var uplift = TemplarSkillHelper.ConsumeUplift(caster);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 35, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, splashArea, uplift));
		}

		/// <summary>
		/// Strikes the targets in front of the Templar.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		/// <param name="uplift"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea, int uplift)
		{
			await skill.Wait(HitDelay);

			var hits = new List<SkillHitInfo>();
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var shieldAttack = this.GetShieldAttack(caster);

			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(HitCount);
				modifier.DamageMultiplier += UpliftDamageRates[uplift];
				modifier.DefensePenetrationRate += UpliftPenetrationRates[uplift];
				modifier.BonusPAtk += shieldAttack;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, AniTime, TimeSpan.Zero);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}

		/// <summary>
		/// Returns the physical attack the Templar's shield adds to the strike.
		/// </summary>
		/// <param name="caster"></param>
		/// <returns></returns>
		private float GetShieldAttack(ICombatEntity caster)
		{
			if (!caster.TryGetEquipItem(EquipSlot.LeftHand, out var offHand) || offHand.Data.EquipType1 != EquipType.Shield)
				return 0;

			return offHand.Properties.GetFloat(PropertyName.DEF) * ShieldDefenseRate;
		}
	}
}
