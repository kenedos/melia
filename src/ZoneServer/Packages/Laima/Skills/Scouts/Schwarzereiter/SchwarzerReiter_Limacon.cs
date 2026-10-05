using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_Limacon)]
	public class SchwarzerReiter_LimaconOverride : ISelfSkillHandler
	{
		private static readonly TimeSpan AnimationDuration = TimeSpan.FromMilliseconds(600);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (caster == null || caster.IsDead)
				return;

			if (caster.TryGetBuff(BuffId.Limacon_Buff, out _))
			{
				caster.SetAttackState(true);
				Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, dir, Position.Zero);
				Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);
				caster.StopBuff(BuffId.Limacon_Buff);
				this.FinishSkill(skill, caster);
				return;
			}

			if (!this.HasPistol(caster))
			{
				caster.ServerMessage(Localization.Get("A pistol is required."));
				this.CancelSkill(skill, caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				this.CancelSkill(skill, caster);
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, dir, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);
			skill.Run(this.ActivateLimacon(skill, caster));
		}

		private async Task ActivateLimacon(Skill skill, ICombatEntity caster)
		{
			try
			{
				await skill.Wait(AnimationDuration);

				if (caster.IsDead || !this.HasPistol(caster))
					return;

				caster.StartBuff(BuffId.Limacon_Buff, skill.Level, 0f, TimeSpan.Zero, caster, skill.Id);
			}
			finally
			{
				this.FinishSkill(skill, caster);
			}
		}

		private void CancelSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
		}

		private void FinishSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
		}

		private bool HasPistol(ICombatEntity caster)
		{
			caster.TryGetEquipItem(EquipSlot.LeftHand, out var leftHandWeapon);
			caster.TryGetEquipItem(EquipSlot.RightHand, out var rightHandWeapon);
			return (leftHandWeapon != null && leftHandWeapon.Data.EquipType1 == EquipType.Pistol) || (rightHandWeapon != null && rightHandWeapon.Data.EquipType1 == EquipType.Pistol);
		}
	}
}
