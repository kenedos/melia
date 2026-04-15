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
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill Hovering.
	/// Commands the hawk to hover at a target location, periodically attacking
	/// enemies that enter the area. Also provides vision of the area.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_Hovering)]
	public class Falconer_HoveringOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float HoveringRadius = 100f;
		private const int BaseDurationSeconds = 10;
		private const int BaseAttackIntervalMs = 2000;
		private const int MaxTargetsPerAttack = 5;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!FalconerHawkHelper.TryGetHawk(caster, out var hawk))
			{
				if (caster is Character character)
					character.SystemMessage("CompanionIsNotActive");
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(skill, caster, hawk, targetPos));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster, World.Actors.Monsters.Companion hawk, Position targetPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(300));

			// Create hovering pad
			var pad = SkillCreatePad(caster, skill, targetPos, 0f, PadName.Falconer_Circling, range: HoveringRadius);
			if (pad == null)
				return;

			// Calculate duration based on skill level
			// Falconer3: Hovering: Duration - increased duration
			var duration = BaseDurationSeconds * 1000;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Falconer3, out var abilLevel))
				duration += abilLevel * 3000;

			var endTime = DateTime.Now.AddMilliseconds(duration);

			// Falconer4: Hovering: Attack Speed Buff - reduced attack interval
			var attackInterval = BaseAttackIntervalMs;
			if (caster.IsAbilityActive(AbilityId.Falconer4) && skill.Level >= 3)
				attackInterval = 1200;

			// Command hawk to start hovering
			hawk.Vars.Set("Hawk.Hovering.Active", true);
			hawk.PlayAnimation("HOVERING_LOOP");

			// Periodic attack loop
			while (DateTime.Now < endTime && !caster.IsDead && !hawk.IsDead && !pad.IsDead)
			{
				// Hovering loop effect
				caster.PlayGroundEffect(targetPos, "F_archer_hovering_loop", 0.5f);

				// Find enemies in area
				var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, HoveringRadius)
					.Take(MaxTargetsPerAttack)
					.ToList();

				if (enemies.Count > 0)
				{
					await this.PerformHoveringAttack(skill, caster, hawk, enemies, targetPos);
				}

				await skill.Wait(TimeSpan.FromMilliseconds(attackInterval));
			}

			// Cleanup
			hawk.Vars.Set("Hawk.Hovering.Active", false);
			caster.PlayGroundEffect(targetPos, "F_archer_hovering_end", 1f);
			pad.Destroy();

			// Hawk flies away
			await FalconerHawkHelper.HawkFlyAway(skill, caster, hawk);
		}

		private async Task PerformHoveringAttack(Skill skill, ICombatEntity caster, World.Actors.Monsters.Companion hawk, List<ICombatEntity> enemies, Position targetPos)
		{
			var enemy = enemies.FirstOrDefault();
			if (enemy == null || enemy.IsDead)
				return;

			// Target indicator
			enemy.PlayEffect("I_sys_target001_circle", scale: 1.5f, heightOffset: EffectLocation.Bottom);

			await skill.Wait(TimeSpan.FromMilliseconds(500));

			// Attack swoop effect
			caster.PlayGroundEffect(targetPos, "F_archer_hovering_attack", 0.5f);

			// Hawk dive animation
			var syncKey = hawk.GenerateSyncKey();
			Send.ZC_NORMAL.CollisionAndBack(hawk, enemy, syncKey, "HOVERING_SHOT", 0.7f, 7f, 0.5f, 0.7f, 0f, true);

			var hits = new List<SkillHitInfo>();
			var damageDelay = TimeSpan.FromMilliseconds(50);
			var skillHitDelay = TimeSpan.Zero;

			var skillHitResult = SCR_SkillHit(caster, enemy, skill);
			enemy.TakeDamage(skillHitResult.Damage, caster);

			var skillHit = new SkillHitInfo(caster, enemy, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.HitEffect = HitEffect.Impact;
			hits.Add(skillHit);

			// Knockdown effect
			if (enemy.Properties.GetFloat(PropertyName.KDArmor) < 900)
			{
				skillHit.KnockBackInfo = new KnockBackInfo(hawk.Position, enemy, KnockBackType.KnockDown, 250, 80);
				enemy.ApplyKnockdown(caster, skill, skillHit);
			}

			Send.ZC_SKILL_HIT_INFO(caster, hits);

			await Task.CompletedTask;
		}
	}
}
