using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Buffs.Handlers.Swordsmen.Fencer;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Fencer skill Attaque Composee.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fencer_AttaqueComposee)]
	public class Fencer_AttaqueComposeeOverride : IGroundSkillHandler
	{
		private const int HitCount = 2;
		private const float NormalMinimumCriticalChance = 20f;
		private const float BalestraMinimumCriticalChance = 40f;
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(100);
		private static readonly TimeSpan CriticalBuffDuration = TimeSpan.FromSeconds(5);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster == null || caster.IsDead)
				return;

			if (!caster.TryGetEquipItem(EquipSlot.RightHand, out var weapon) || weapon.Data.EquipType1 != EquipType.Rapier)
			{
				caster.ServerMessage(Localization.Get("A Rapier must be equipped."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.TurnTowards(farPos);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			try
			{
				var maximumTargets = skill.GetPVPValue(10);
				var targets = SkillSelectEnemiesInSquare(caster, caster.Position, 0f, 80f, 20f, maximumTargets)
					.Where(target => target != null && !target.IsDead)
					.ToList();

				if (targets.Count == 0)
					return;

				var preparationMultiplier = Preparation_Buff_EndOverride.ConsumeDamageMultiplier(caster);

				for (var hit = 0; hit < HitCount; hit++)
				{
					SkillTargetDamage(skill, caster, targets, preparationMultiplier);

					if (hit < HitCount - 1)
						await skill.Wait(DelayBetweenHits);
				}

				var minimumCriticalChance = targets.Any(target => target.IsBuffActive(BuffId.BalestraFente_Debuff))
					? BalestraMinimumCriticalChance
					: NormalMinimumCriticalChance;

				caster.StartBuff(BuffId.AttaqueComposee_Critical_Buff, skill.Level, minimumCriticalChance, CriticalBuffDuration, caster, skill.Id);
			}
			finally
			{
				caster.SetAttackState(false);
			}
		}
	}
}
