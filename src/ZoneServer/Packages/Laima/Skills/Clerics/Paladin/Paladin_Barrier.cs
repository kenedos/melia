using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Clerics.Paladin
{
	/// <summary>
	/// Handler for the Paladin skill Barrier.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Paladin_Barrier)]
	public class Paladin_BarrierOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (caster.TryGetActiveAbility(AbilityId.Paladin20, out var ability))
				caster.StartBuff(BuffId.Skill_Durability_Buff, 1f, 0f, TimeSpan.FromMilliseconds(1f), caster);
			caster.PlaySound("voice_cleric_barrier_shot", "voice_cleric_m_barrier_shot");
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			SkillRemovePad(caster, skill);
			caster.RemoveBuff(BuffId.Skill_Durability_Buff);
			caster.StopSound("voice_cleric_barrier_shot", "voice_cleric_m_barrier_shot");
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = targets.FirstOrDefault()?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Cleric_Barrier_PC);
		}
	}
}
