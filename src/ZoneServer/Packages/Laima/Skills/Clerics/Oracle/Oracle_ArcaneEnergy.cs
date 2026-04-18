using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handler for the Oracle skill Arcane Energy.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Oracle_ArcaneEnergy)]
	public class Oracle_ArcaneEnergyOverride : IGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
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

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(790));
			caster.StartBuff(BuffId.ArcaneEnergy_Buff, skill.Level, 0f, TimeSpan.FromMilliseconds(SCR_GET_ArcaneEnergy_Bufftime(skill) * 1000), caster, skill.Id);
		}

		private float SCR_GET_ArcaneEnergy_Bufftime(Skill skill)
		{
			var value = skill.Level;
			var caster = skill.Owner;

			if (caster.IsAbilityActive(AbilityId.Oracle32) && value < 5)
				value = 5;
			else if (caster.IsAbilityActive(AbilityId.Oracle7))
				value += caster.GetAbilityLevel(AbilityId.Oracle7);
			return value;
		}
	}
}
