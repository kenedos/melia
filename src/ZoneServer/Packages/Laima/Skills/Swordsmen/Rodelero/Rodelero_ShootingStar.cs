using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Abilities.Handlers.Swordsmen.Rodelero;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Rodelero
{
	/// <summary>
	/// Handler for the Rodelero skill Shooting Star.
	/// Leaping shield attack that hits enemies below.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Rodelero_ShootingStar)]
	public class Rodelero_ShootingStarOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles the Shooting Star skill execution.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Attack(skill, caster, originPos, farPos));
		}

		/// <summary>
		/// Executes the attack.
		/// </summary>
		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 65, width: 35, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var damageDelay = TimeSpan.FromMilliseconds(150);
			var skillHitDelay = TimeSpan.Zero;

			var hits = new List<SkillHitInfo>();
			var targetList = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			foreach (var target in targetList.LimitBySDR(caster, skill))
			{
				var modifier = SkillModifier.MultiHit(7);
				modifier.BonusPAtk = Rodelero31.GetBonusPAtk(caster);
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
