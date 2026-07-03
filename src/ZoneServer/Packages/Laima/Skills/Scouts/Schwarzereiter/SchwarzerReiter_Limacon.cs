using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Schwarzer Reiter skill Limacon.
	/// SkillId: 51003
	/// ClassName: Schwarzereiter_Limacon
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_Limacon)]
	public class SchwarzerReiter_LimaconOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!this.HasPistol(caster))
			{
				caster.ServerMessage(Localization.Get("A pistol is required."));
				return;
			}

			if (caster.TryGetBuff(BuffId.Limacon_Buff, out _))
			{
				caster.StopBuff(BuffId.Limacon_Buff);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, Position.Zero);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(600));

			if (!this.HasPistol(caster))
			{
				caster.SetAttackState(false);
				return;
			}

			// Limacon é um buff/toggle.
			// TimeSpan.Zero deixa permanente até desligar/remover.
			caster.StartBuff(BuffId.Limacon_Buff, skill.Level, 0f, TimeSpan.Zero, caster, skill.Id);

			caster.SetAttackState(false);
		}

		private bool HasPistol(ICombatEntity caster)
		{
			caster.TryGetEquipItem(EquipSlot.LeftHand, out var leftHandWeapon);
			caster.TryGetEquipItem(EquipSlot.RightHand, out var rightHandWeapon);

			return
				(leftHandWeapon != null && leftHandWeapon.Data.EquipType1 == EquipType.Pistol) ||
				(rightHandWeapon != null && rightHandWeapon.Data.EquipType1 == EquipType.Pistol);
		}
	}
}
