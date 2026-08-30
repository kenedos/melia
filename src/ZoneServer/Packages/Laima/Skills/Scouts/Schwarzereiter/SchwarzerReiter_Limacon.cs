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
		private static readonly TimeSpan BuffDelay = TimeSpan.FromMilliseconds(600);

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
			await skill.Wait(BuffDelay);

			// Pressing the skill again ends the stance.
			if (caster.IsBuffActive(BuffId.Limacon_Buff))
			{
				caster.StopBuff(BuffId.Limacon_Buff);
				return;
			}

			if (!HasPistolEquipped(caster))
				return;

			caster.StartBuff(BuffId.Limacon_Buff, skill.Level, 0f, TimeSpan.Zero, caster, skill.Id);
		}

		/// <summary>
		/// Returns whether the caster is wielding a pistol.
		/// </summary>
		/// <param name="caster"></param>
		private static bool HasPistolEquipped(ICombatEntity caster)
		{
			if (caster.TryGetEquipItem(EquipSlot.RightHand, out var rightHand) && rightHand.Data.EquipType1 == EquipType.Pistol)
				return true;

			return caster.TryGetEquipItem(EquipSlot.LeftHand, out var leftHand) && leftHand.Data.EquipType1 == EquipType.Pistol;
		}
	}
}
