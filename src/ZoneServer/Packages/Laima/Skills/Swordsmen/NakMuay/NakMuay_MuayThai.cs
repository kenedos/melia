using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for Nak Muay skill Muay Thai.
	/// Requires Ram Muay stance.
	/// Applies Muay Thai buff.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.NakMuay_MuayThai)]
	public class NakMuay_MuayThaiOverride : ISelfSkillHandler
	{
		private static readonly TimeSpan Duration = TimeSpan.FromSeconds(30);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TryGetBuff(BuffId.RamMuay_Buff, out _))
			{
				caster.ServerMessage(Localization.Get("Ram Muay is required."));
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			skill.Run(this.ApplyBuff(caster, skill));
		}

		private async Task ApplyBuff(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(350));

			caster.StartBuff(
				BuffId.MuayThai_Buff,
				skill.Level,
				0f,
				Duration,
				caster,
				skill.Id);

			caster.SetAttackState(false);
		}
	}
}
