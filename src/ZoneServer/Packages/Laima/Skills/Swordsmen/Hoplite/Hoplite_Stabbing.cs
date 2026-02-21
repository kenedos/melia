using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.HandlersOverrides.Swordsmen.Hoplite
{
	/// <summary>
	/// Handler override for the Hoplite skill Stabbing.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Hoplite_Stabbing)]
	public class Hoplite_StabbingOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			// Check if caster is wielding a spear or two-handed spear
			if (caster is Character character)
			{
				var weapon = character.Inventory.GetItem(EquipSlot.RightHand);
				if (weapon == null || (weapon.Data.EquipType1 != EquipType.Spear && weapon.Data.EquipType1 != EquipType.THSpear))
				{
					caster.ServerMessage(Localization.Get("Skill requires a spear or two-handed spear."));
					Send.ZC_SKILL_DISABLE(caster);
					return;
				}
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 20, angle: 0);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, targets.FirstOrDefault()?.Handle ?? 0, originPos, caster.Direction, Position.Zero);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.Attack(skill, caster, splashArea));
		}

		/// <summary>
		/// Executes the actual attack after a delay.
		/// </summary>
		/// <remarks>
		/// This skill does 30 hits total - 1 initial hit, followed by 29 more hits in a loop.
		/// Each hit on a target applies a stacking debuff that increases damage by 2% per stack (unlimited stacks).
		/// </remarks>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="splashArea"></param>
		private async Task Attack(Skill skill, ICombatEntity caster, ISplashArea splashArea)
		{
			var delayBeforeLoop = TimeSpan.FromMilliseconds(60);
			var delayBetweenHits = TimeSpan.FromMilliseconds(70);

			var targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

			// First hit
			foreach (var target in targets.LimitBySDR(caster, skill))
			{
				var skillHitResult = SCR_SkillHit(caster, target, skill);
				target.TakeDamage(skillHitResult.Damage, caster);

				var hitInfo = new HitInfo(caster, target, skill, skillHitResult, delayBetweenHits);
				Send.ZC_HIT_INFO(caster, target, hitInfo);

				target.StartBuff(BuffId.Stabbing_Debuff, TimeSpan.FromSeconds(3));
			}

			await skill.Wait(delayBeforeLoop);

			// 29 more hits with damage increase based on debuff stacks
			for (var i = 0; i < 29; i++)
			{
				targets = caster.Map.GetAttackableEnemiesIn(caster, splashArea);

				foreach (var target in targets.LimitBySDR(caster, skill))
				{
					var modifier = SkillModifier.Default;
					if (target.TryGetBuff(BuffId.Stabbing_Debuff, out var stabbingDebuff))
					{
						// +2% damage per stack, unlimited stacking
						modifier.DamageMultiplier += 0.02f * stabbingDebuff.OverbuffCounter;
					}
					var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
					target.TakeDamage(skillHitResult.Damage, caster);

					var hitInfo = new HitInfo(caster, target, skill, skillHitResult, delayBetweenHits);
					Send.ZC_HIT_INFO(caster, target, hitInfo);

					target.StartBuff(BuffId.Stabbing_Debuff, TimeSpan.FromSeconds(3));
				}

				await skill.Wait(delayBetweenHits);
			}

			Send.ZC_SKILL_DISABLE(caster);
			Send.ZC_NORMAL.SkillCancel(caster, SkillId.Hoplite_Stabbing);
			Send.ZC_NORMAL.SkillCancelCancel(caster, SkillId.Hoplite_Stabbing);
		}
	}
}
