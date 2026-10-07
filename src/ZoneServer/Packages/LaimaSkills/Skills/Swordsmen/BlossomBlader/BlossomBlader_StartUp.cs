using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Swordsmen.BlossomBlader
{
	/// <summary>
	/// Handler for the Blossom Blader skill StartUp, a charge of up to 2
	/// seconds that halves the damage taken and, once released, raises
	/// critical damage by how long it was held.
	/// </summary>
	/// <remarks>
	/// A fully charged StartUp grants StartUp: Blossom Shower's extra hit.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.BlossomBlader_StartUp)]
	public class BlossomBlader_StartUpOverride : IDynamicCasted
	{
		private const string ChargeStartVar = "Melia.BlossomBlader.StartUpChargeStart";
		private static readonly TimeSpan MaxCharge = TimeSpan.FromSeconds(2);
		private static readonly TimeSpan ChargingDuration = TimeSpan.FromSeconds(3);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			skill.Vars.Set(ChargeStartVar, GameClock.LocalNow);
			caster.StartBuff(BuffId.StartUp_Charging_Buff, skill.Level, 0, ChargingDuration, caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (!caster.IsBuffActive(BuffId.StartUp_Charging_Buff) || !skill.Vars.TryGet<DateTime>(ChargeStartVar, out var chargeStart))
				return;

			caster.StopBuff(BuffId.StartUp_Charging_Buff);

			var charge = Math.Min(1f, (float)((GameClock.LocalNow - chargeStart) / MaxCharge));
			var duration = skill.Properties.CaptionTime;

			caster.StartBuff(BuffId.StartUp_Buff, skill.Level, charge, duration, caster, skill.Id);

			if (charge >= 1 && caster.IsAbilityActive(AbilityId.Blossomblader20))
				caster.StartBuff(BuffId.StartUp_Abil_Buff, skill.Level, 0, duration, caster, skill.Id);
		}
	}
}
