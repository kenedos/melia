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
	/// Handler for the Schwarzereiter skill Limacon.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_Limacon)]
	public class SchwarzerReiter_LimaconOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
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
			caster.TryGetEquipItem(EquipSlot.LeftHand, out var leftHandWeapon);
			caster.TryGetEquipItem(EquipSlot.RightHand, out var rightHandWeapon);
			if ((leftHandWeapon != null && leftHandWeapon.Data.EquipType1 == EquipType.Pistol) ||
				(rightHandWeapon != null && rightHandWeapon.Data.EquipType1 == EquipType.Pistol))
				caster.StartBuff(BuffId.Limacon_Buff, 1f, 0f, TimeSpan.Zero, caster, skill.Id);
		}
	}
}
