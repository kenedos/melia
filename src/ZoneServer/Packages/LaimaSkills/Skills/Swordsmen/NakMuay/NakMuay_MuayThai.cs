using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Swordsmen.NakMuay
{
	/// <summary>
	/// Handler for the Nak Muay skill Muay Thai, which raises the final
	/// damage of Nak Muay skills for 5 minutes.
	/// </summary>
	/// <remarks>
	/// [Arts] Muay Thai: Muay Boran instead has basic attacks cast the
	/// learned Te Kha, Te Trong and Sok Chiang for 20 seconds.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.NakMuay_MuayThai)]
	public class NakMuay_MuayThaiOverride : ISelfSkillHandler
	{
		private static readonly TimeSpan Duration = TimeSpan.FromMinutes(5);
		private static readonly TimeSpan BoranDuration = TimeSpan.FromSeconds(20);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Direction dir)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, originPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, originPos, ForceId.GetNew(), null);

			if (caster.IsAbilityActive(AbilityId.NakMuay12))
				caster.StartBuff(BuffId.MuayThai_Buff, skill.Level, 0, BoranDuration, caster, skill.Id);
			else
				caster.StartBuff(BuffId.MuayThai_Abil_Buff, skill.Level, 0, Duration, caster, skill.Id);
		}
	}
}
