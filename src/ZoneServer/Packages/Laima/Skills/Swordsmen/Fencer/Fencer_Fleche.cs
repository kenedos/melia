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
	/// Handler for the Fencer skill Fleche.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fencer_Fleche)]
	public class Fencer_FlecheOverride : IGroundSkillHandler
	{
		private const int HitCount = 3;
		private const int MaximumTargets = 1;
		private const float BalestraDamageMultiplier = 1.30f;
		private static readonly TimeSpan DelayBetweenHits = TimeSpan.FromMilliseconds(100);

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
			var consumeCriticalBuff = false;

			try
			{
				var targets = SkillSelectEnemiesInSquare(caster, caster.Position, 0f, 100f, 20f, MaximumTargets)
					.Where(target => target != null && !target.IsDead)
					.ToList();

				if (targets.Count == 0)
					return;

				var damageMultiplier = Preparation_Buff_EndOverride.ConsumeDamageMultiplier(caster);
				consumeCriticalBuff = caster.IsBuffActive(BuffId.AttaqueComposee_Critical_Buff);

				if (targets[0].IsBuffActive(BuffId.BalestraFente_Debuff))
					damageMultiplier *= BalestraDamageMultiplier;

				for (var hit = 0; hit < HitCount; hit++)
				{
					SkillTargetDamage(skill, caster, targets, damageMultiplier);

					if (hit < HitCount - 1)
						await skill.Wait(DelayBetweenHits);
				}
			}
			finally
			{
				if (consumeCriticalBuff)
					caster.StopBuff(BuffId.AttaqueComposee_Critical_Buff);

				caster.SetAttackState(false);
			}
		}
	}
}
