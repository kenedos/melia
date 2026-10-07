using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the Druid skill Thorn, which throws thorny vines over the
	/// enemies ahead, rooting them and tearing at them 5 times.
	/// </summary>
	/// <remarks>
	/// Thorn: Bleeding gives a 10% chance per level to make them bleed for
	/// 10 seconds.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Druid_ThornVine)]
	public class Druid_ThornVineOverride : IGroundSkillHandler
	{
		private const float Length = 200f;
		private const float Width = 100f;
		private const int MaxTargets = 12;
		private const int BleedingChancePerLevel = 10;
		private static readonly TimeSpan ThrowDelay = TimeSpan.FromMilliseconds(700);
		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(5);
		private static readonly TimeSpan BleedingDuration = TimeSpan.FromSeconds(10);

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

			skill.Run(this.Throw(skill, caster));
		}

		/// <summary>
		/// Throws the vines over the enemies ahead of the caster.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Throw(Skill skill, ICombatEntity caster)
		{
			var area = new Square(caster.Position, caster.Direction, Length, Width);

			await skill.Wait(ThrowDelay);

			if (caster.IsDead)
				return;

			var bleedingChance = caster.TryGetActiveAbilityLevel(AbilityId.Druid19, out var level) ? level * BleedingChancePerLevel : 0;

			foreach (var target in caster.Map.GetAttackableEnemiesIn(caster, area).Take(MaxTargets))
			{
				var damage = SCR_SkillHit(caster, target, skill).Damage;
				if (damage <= 0)
					continue;

				target.StartBuff(BuffId.ThornVine_Debuff, skill.Level, damage, Duration, caster, skill.Id);

				if (GameRandom.Get().Next(100) < bleedingChance)
					target.StartBuff(BuffId.UC_bleed, skill.Level, damage, BleedingDuration, caster, skill.Id);
			}
		}
	}
}
