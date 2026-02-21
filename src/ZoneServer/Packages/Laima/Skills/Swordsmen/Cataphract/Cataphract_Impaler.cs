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
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using System.Linq;

namespace Melia.Zone.Skills.Handlers.Cataphract
{
	/// <summary>
	/// Handler for the Cataphract skill Impaler.
	/// Mounted lance attack that impales and carries enemies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cataphract_Impaler)]
	public class Cataphract_ImpalerOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles the Impaler skill execution.
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
		/// Executes the attack and applies the impaler debuff.
		/// </summary>
		private async Task Attack(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 35, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var damageDelay = TimeSpan.FromMilliseconds(250);
			var skillHitDelay = TimeSpan.Zero;

			await skill.Wait(TimeSpan.FromMilliseconds(200));

			var hits = new List<SkillHitInfo>();
			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);
			var targetList = targets.Take(3);

			foreach (var target in targetList)
			{
				var modifier = SkillModifier.MultiHit(3);
				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);

				target.TakeDamage(skillHitResult.Damage, caster);

				var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
				skillHit.HitEffect = HitEffect.Impact;

				hits.Add(skillHit);

				if (skillHitResult.Damage > 0 && target.IsKnockdownable())
				{
					var time = 3000 + skill.Level * 300;
					target.StartBuff(BuffId.Impaler_Debuff, skill.Level, 0f, TimeSpan.FromMilliseconds(time), caster);
				}
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);
		}
	}
}
