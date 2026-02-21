using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Bokor skill Effigy.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bokor_Effigy)]
	public class Bokor_EffigyOverride : IMeleeGroundSkillHandler
	{
		float Range = 250f;

		/// <summary>
		/// Handle Skill Behavior
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
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

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			SkillTargetEffects(skill, caster, "F_blood002_dark", 1.6f, false);

			var maxTargets = 3;
			var totalDamage = 0f;
			var hitTargets = new List<ICombatEntity>();

			var character = caster as Character;
			var summons = character?.Summons.GetSummons();

			// Get all enemies in range
			var allTargets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, Range);

			// Prioritize targets with CurseOfWeakness_Debuff first, then others
			var prioritizedTargets = allTargets
				.OrderByDescending(t => t.IsBuffActive(BuffId.CurseOfWeakness_Debuff))
				.Take(maxTargets);

			// Apply effects to selected targets
			foreach (var target in prioritizedTargets)
			{
				// Deal damage if target has CurseOfWeakness_Debuff
				if (target.IsBuffActive(BuffId.CurseOfWeakness_Debuff))
				{
					var modifier = SkillModifier.Default;
					var skillHit = SCR_SkillHit(caster, target, skill, modifier);
					target.TakeDamage(skillHit.Damage, caster);
					totalDamage += skillHit.Damage;
					hitTargets.Add(target);

					var skillHitInfo = new SkillHitInfo(caster, target, skill, skillHit);
					Send.ZC_SKILL_HIT_INFO(caster, skillHitInfo);

					target.StartBuff(BuffId.Pollution_Debuff, skill.Level, skillHit.Damage, TimeSpan.FromMilliseconds(6000), caster, skill.Id);
				}
			}

			// Add hate towards each zombie
			if (summons != null && summons.Count > 0)
			{
				foreach (var target in hitTargets)
				{
					if (target.Components.TryGet<AiComponent>(out var targetAi))
					{
						foreach (var summon in summons)
						{
							targetAi.Script.QueueEventAlert(new HateIncreaseAlert(summon, 300));
						}
					}
				}
			}

			// Consume up to 5 Dark Force stacks for bonus healing
			// Each stack = +30% heal (1 stack = 30%, 5 stacks = 150%)
			if (totalDamage > 0 && summons != null && summons.Count > 0)
			{
				var stacksToConsume = 0;

				if (caster.TryGetBuff(BuffId.PowerOfDarkness_Buff, out var darkForceBuff))
				{
					stacksToConsume = Math.Min(5, darkForceBuff.OverbuffCounter);
					darkForceBuff.OverbuffCounter -= stacksToConsume;
					darkForceBuff.NotifyUpdate();
				}

				if (stacksToConsume > 0)
				{
					var healMultiplier = stacksToConsume * 0.3f;
					var healPerSummon = (totalDamage * healMultiplier) / summons.Count;

					foreach (var summon in summons)
					{
						summon.Heal(healPerSummon, 0);
						Send.ZC_HEAL_INFO(summon, healPerSummon, summon.Hp, HealType.Hp);
					}
				}
			}
		}
	}
}
