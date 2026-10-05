using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Miko
{
	[Package("laima")]
	[SkillHandler(SkillId.Miko_Hamaya)]
	public class Miko_Hamaya : IGroundSkillHandler, IDynamicCasted
	{
		private const float MaximumCastRange = 100f;
		private const float PadRange = 35f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || caster.IsDead || caster.Map == null)
				return;

			if (originPos.Get2DDistance(farPos) > MaximumCastRange)
			{
				character.ServerMessage(Localization.Get("Too far away."));
				this.CancelSkill(skill, caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				this.CancelSkill(skill, caster);
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			SkillRemovePad(caster, skill);
			SkillCreatePad(caster, skill, farPos, 0f, PadName.Miko_Hamaya, range: PadRange);

			caster.SetAttackState(false);
		}

		private void CancelSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
		}
	}
}
