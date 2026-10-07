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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Inquisitor
{
	/// <summary>
	/// Handler for the Inquisitor skill Ripper, giant scissors that cut the
	/// enemies in front of the Inquisitor 10 times over 4 seconds, harder
	/// with every cut and against devils or under Judgment.
	/// </summary>
	/// <remarks>
	/// With Ripper: Armor Break, the cuts ignore 3% of defense per ability
	/// level.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Inquisitor_BreastRipper)]
	public class Inquisitor_BreastRipperOverride : IGroundSkillHandler
	{
		private const int CutCount = 10;
		private const float Distance = 30f;
		private const float Radius = 40f;
		private const float Angle = 150f;
		private const float DamagePerCut = 0.05f;
		private const float PunishBonus = 0.5f;
		private const float ArmorBreakPerLevel = 0.03f;
		private static readonly TimeSpan Duration = TimeSpan.FromMilliseconds(4500);
		private static readonly TimeSpan CutInterval = TimeSpan.FromMilliseconds(400);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			caster.StartBuff(BuffId.Skill_SuperArmor_Buff, Duration);
			caster.StartBuff(BuffId.BreastRipper_Buff, skill.Level, 0, Duration, caster, skill.Id);

			skill.Run(this.Cut(skill, caster));
		}

		/// <summary>
		/// Cuts the enemies in front of the caster every 0.3 seconds.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Cut(Skill skill, ICombatEntity caster)
		{
			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);
			caster.TryGetActiveAbilityLevel(AbilityId.Inquisitor19, out var armorBreakLevel);

			for (var cut = 0; cut < CutCount; cut++)
			{
				await skill.Wait(CutInterval);

				if (caster.IsDead || !caster.TryGetBuff(BuffId.BreastRipper_Buff, out var ripperBuff))
					return;

				var center = caster.Position.GetRelative(caster.Direction, Distance);
				var area = new Fan(center, caster.Direction, Radius, Angle);
				var hits = new List<SkillHitInfo>();

				foreach (var target in caster.Map.GetAttackableEnemiesIn(caster, area).Take(maxTargets))
				{
					var modifier = new SkillModifier();
					modifier.DamageMultiplier += cut * DamagePerCut;
					modifier.DefensePenetrationRate += armorBreakLevel * ArmorBreakPerLevel;

					if (InquisitorSkillHelper.IsPunishing(caster, target))
						modifier.DamageMultiplier += PunishBonus;

					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
					target.TakeDamage(skillHitResult.Damage, caster);

					hits.Add(new SkillHitInfo(caster, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
				}

				ripperBuff.IncreaseOverbuff();
				ripperBuff.NotifyUpdate();

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(caster, hits);
			}
		}
	}
}
