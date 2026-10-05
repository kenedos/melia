using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Handlers.Swordsmen.Fencer;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Fencer skill Balestra Fente.
	/// Dashes to a selected enemy, attacks it and reduces its Critical Resistance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fencer_BalestraFente)]
	public class Fencer_BalestraFenteOverride : IGroundSkillHandler
	{
		private const float TargetSearchRadius = 60f;
		private const float CriticalResistanceReductionBase = 50f;
		private const float CriticalResistanceReductionOffset = 10f;
		private const float DexterityPerReductionPoint = 20f;
		private static readonly TimeSpan DebuffDuration = TimeSpan.FromSeconds(10);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity packetTarget)
		{
			if (caster == null)
				return;

			if (caster.IsDead || caster.Map == null)
			{
				EndExecution(caster);
				return;
			}

			var target = this.FindTarget(caster, skill, farPos, packetTarget);

			if (target == null)
			{
				EndExecution(caster);
				caster.ServerMessage(Localization.Get("No valid target was found."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				EndExecution(caster);
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.TurnTowards(target.Position);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, target.Position);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, target.Position, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, target));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, ICombatEntity target)
		{
			try
			{
				await skill.Wait(TimeSpan.FromMilliseconds(150));

				if (caster.IsDead || caster.Map == null || target == null || target.IsDead || target.Map != caster.Map || caster.Position.Get2DDistance(target.Position) > skill.Data.MaxRange)
					return;

				var targetPosition = target.Position;

				caster.SetPosition(targetPosition);
				caster.TurnTowards(target.Position);
				Send.ZC_MOVE_STOP(caster, targetPosition, 1);

				await skill.Wait(TimeSpan.FromMilliseconds(100));

				if (caster.IsDead || caster.Map == null || target.Map != caster.Map)
					return;

				var startPosition = caster.Position;
				var targetCount = skill.GetPVPValue(10);
				var skillTargets = SkillSelectEnemiesInSquare(caster, startPosition, 0f, 120f, 25f, targetCount);

				if (skillTargets == null || skillTargets.Count == 0)
					return;

				var damageMultiplier = Preparation_Buff_EndOverride.ConsumeDamageMultiplier(caster);

				for (var hit = 0; hit < 4; hit++)
				{
					SkillTargetDamage(skill, caster, skillTargets, damageMultiplier);

					if (hit < 3)
						await skill.Wait(TimeSpan.FromMilliseconds(100));
				}

				if (!target.IsDead)
				{
					var dexterity = Math.Max(0f, caster.Properties.GetFloat(PropertyName.DEX));
					var dexterityBonus = (float)Math.Floor(dexterity / DexterityPerReductionPoint);
					var reduction = CriticalResistanceReductionBase + (CriticalResistanceReductionOffset + dexterityBonus * skill.Level);
					target.StartBuff(BuffId.BalestraFente_Debuff, skill.Level, reduction, DebuffDuration, caster, skill.Id);
				}
			}
			finally
			{
				EndExecution(caster);
			}
		}

		private static void EndExecution(ICombatEntity caster)
		{
			caster.SetAttackState(false);
			if (caster is Character character && character.Connection != null)
				Send.ZC_SKILL_DISABLE(character);
		}

		private ICombatEntity FindTarget(ICombatEntity caster, Skill skill, Position farPos, ICombatEntity packetTarget)
		{
			var enemies = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, skill.Data.MaxRange)
				.Where(enemy => enemy != null && !enemy.IsDead)
				.ToList();

			if (packetTarget != null)
				return enemies.FirstOrDefault(enemy => enemy.Handle == packetTarget.Handle);

			return enemies
				.Where(enemy => enemy.Position.Get2DDistance(farPos) <= TargetSearchRadius)
				.OrderBy(enemy => enemy.Position.Get2DDistance(farPos))
				.FirstOrDefault();
		}
	}
}
