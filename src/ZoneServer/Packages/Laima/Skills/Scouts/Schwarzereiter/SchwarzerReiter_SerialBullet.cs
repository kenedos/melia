using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Schwarzereiter skill Double Bullet.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Schwarzereiter_DoubleBullet)]
	public class SchwarzerReiter_DoubleBulletOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			// Toggle OFF
			if (caster.TryGetBuff(BuffId.DoubleBullet_Toggle_Buff, out _))
			{
				caster.StopBuff(BuffId.DoubleBullet_Toggle_Buff);

				Send.ZC_NORMAL.UpdateSkillEffect(
					caster,
					0,
					originPos,
					caster.Direction,
					Position.Zero);

				SkillResetCooldown(skill, caster);
				return;
			}

			// Toggle ON
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			try
			{
				caster.StartBuff(
					BuffId.DoubleBullet_Toggle_Buff,
					skill.Level,
					0f,
					TimeSpan.Zero,
					caster,
					skill.Id);
			}
			catch (Exception ex)
			{
				caster.ServerMessage(ex.Message);
			}

			SkillResetCooldown(skill, caster);

			caster.SetAttackState(false);
		}
	}
}
