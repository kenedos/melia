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

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for Nak Muay skill Ram Muay.
	/// SkillId: 11801
	/// BuffId: RamMuay_Buff = 2137
	///
	/// Toggle ON/OFF.
	/// Enables Nak Muay stance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.NakMuay_RamMuay)]
	public class NakMuay_RamMuayOverride : ISelfSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			// Toggle OFF
			if (caster.TryGetBuff(BuffId.RamMuay_Buff, out _))
			{
				caster.StopBuff(BuffId.RamMuay_Buff);

				NakMuayAttackHelper.UpdateMainAttack((Character)caster);

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
					BuffId.RamMuay_Buff,
					skill.Level,
					0f,
					TimeSpan.Zero,
					caster,
					skill.Id);

				NakMuayAttackHelper.UpdateMainAttack((Character)caster);
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
