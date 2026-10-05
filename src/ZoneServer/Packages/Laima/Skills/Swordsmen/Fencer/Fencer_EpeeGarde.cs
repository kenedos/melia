using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Fencer skill Epee Garde.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fencer_EpeeGarde)]
	public class Fencer_EpeeGardeOverride : ISelfSkillHandler
	{
		private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(30);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
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

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			try
			{
				await skill.Wait(TimeSpan.FromMilliseconds(900));

				if (caster.IsDead)
					return;

				if (!caster.TryGetEquipItem(EquipSlot.RightHand, out var weapon) || weapon.Data.EquipType1 != EquipType.Rapier)
					return;

				caster.StartBuff(BuffId.EpeeGarde_Buff, skill.Level, 0f, BuffDuration, caster, skill.Id);
			}
			finally
			{
				caster.SetAttackState(false);
			}
		}
	}
}
