using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Jolly Roger Enemy Debuff.
	/// Enemies with this debuff increase combo count when hit by party members.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.JollyRoger_Enemy_Debuff)]
	public class JollyRoger_Enemy_DebuffOverride : BuffHandler, IBuffCombatDefenseAfterCalcHandler
	{
		private const int ComboThreshold = 100;
		private const int FeverDurationMs = 5000;

		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (skillHitResult.Result == HitResultType.Dodge)
				return;

			if (attacker is not Character character)
				return;

			if (!character.IsBuffActive(BuffId.JollyRoger_Buff))
				return;

			if (buff.Caster is not Character caster)
				return;

			var now = DateTime.Now;
			var feverStartTime = caster.Variables.Temp.Get<DateTime>("Melia.Buff.JollyRoger.FeverStartTime", DateTime.MinValue);
			var timeSinceFever = (now - feverStartTime).TotalMilliseconds;

			var comboCount = caster.Variables.Temp.GetInt("Melia.Buff.JollyRoger");

			// If we're in Fever mode and 5 seconds passed, reset combo
			if (comboCount >= ComboThreshold && timeSinceFever > FeverDurationMs)
			{
				comboCount = 0;
				caster.Variables.Temp.Remove("Melia.Buff.JollyRoger.FeverStartTime");
			}

			// Only increment and show combo if not at threshold
			if (comboCount < ComboThreshold)
			{
				comboCount++;
				caster.Variables.Temp.SetInt("Melia.Buff.JollyRoger", comboCount);

				if (caster.HasParty)
				{
					foreach (var member in caster.Map.GetPartyMembers(caster))
						Send.ZC_NORMAL.ShowComboEffect(member.Connection, comboCount, 5, ComboThreshold);
				}
				else
				{
					Send.ZC_NORMAL.ShowComboEffect(caster.Connection, comboCount, 5, ComboThreshold);
				}

				// Just reached threshold, start Fever and record time
				if (comboCount >= ComboThreshold)
				{
					caster.Variables.Temp.Set("Melia.Buff.JollyRoger.FeverStartTime", now);

					if (caster.HasParty)
					{
						foreach (var member in caster.Map.GetPartyMembers(caster))
							member.StartBuff(BuffId.FeverTime, TimeSpan.FromMilliseconds(FeverDurationMs), caster);
					}
					else
					{
						caster.StartBuff(BuffId.FeverTime, TimeSpan.FromMilliseconds(FeverDurationMs), caster);
					}
				}
			}
		}
	}
}
