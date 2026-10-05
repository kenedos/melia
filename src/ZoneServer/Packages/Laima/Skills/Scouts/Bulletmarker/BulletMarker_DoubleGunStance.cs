using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Bulletmarker
{
	[Package("laima")]
	[SkillHandler(SkillId.Bulletmarker_DoubleGunStance)]
	public class BulletMarker_DoubleGunStance : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (caster is not Character character || caster.IsDead)
				return;

			if (caster.TryGetBuff(BuffId.DoubleGunStance_Buff, out _))
			{
				caster.SetAttackState(true);
				Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, dir, Position.Zero);
				Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);
				caster.StopBuff(BuffId.DoubleGunStance_Buff);
				BulletMarkerAttackHelper.UpdateMainAttack(character);
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
				caster.StartBuff(BuffId.DoubleGunStance_Buff, skill.Level, 0f, TimeSpan.Zero, caster, skill.Id);
				BulletMarkerAttackHelper.UpdateMainAttack(character);
			}
			catch (Exception ex)
			{
				caster.ServerMessage(ex.Message);
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
