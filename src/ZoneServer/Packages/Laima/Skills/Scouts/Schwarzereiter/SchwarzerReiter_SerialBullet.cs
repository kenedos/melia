using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_DoubleBullet)]
	public class SchwarzerReiter_DoubleBulletOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (caster == null || caster.IsDead)
				return;

			if (caster.TryGetBuff(BuffId.DoubleBullet_Toggle_Buff, out _))
			{
				caster.SetAttackState(true);
				Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, dir, Position.Zero);
				Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);
				caster.StopBuff(BuffId.DoubleBullet_Toggle_Buff);
				SkillResetCooldown(skill, caster);
				this.FinishSkill(skill, caster);
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, dir, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			try
			{
				caster.StartBuff(BuffId.DoubleBullet_Toggle_Buff, skill.Level, 0f, TimeSpan.Zero, caster, skill.Id);
			}
			catch (Exception ex)
			{
				caster.ServerMessage(Localization.Get(ex.Message));
			}
			finally
			{
				SkillResetCooldown(skill, caster);
				this.FinishSkill(skill, caster);
			}
		}

		private void FinishSkill(Skill skill, ICombatEntity caster)
		{
			caster.SetAttackState(false);
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
		}
	}
}
