using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Enchanter
{
	/// <summary>
	/// Handler for Enchanter skill Enchant Weapon.
	/// Toggle ON/OFF.
	/// Duration: infinite.
	/// Effect: Critical Rate +2% per skill level.
	/// Lv1 = +2%, Lv10 = +20%.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Enchanter_EnchantWeaponToggle)]
	public class Enchanter_EnchantWeaponToggleOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			// Toggle OFF
			if (caster.TryGetBuff(BuffId.EnchantLightning_Buff, out _))
			{
				caster.StopBuff(BuffId.EnchantLightning_Buff);

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
					BuffId.EnchantLightning_Buff,
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
