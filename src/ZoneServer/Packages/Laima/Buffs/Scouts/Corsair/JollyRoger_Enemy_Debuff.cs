using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the Jolly Roger Enemy Debuff.
	/// Enemies with this debuff increase combo count when hit by party members
	/// and trigger Brutality buff application.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.JollyRoger_Enemy_Debuff)]
	public class JollyRoger_Enemy_DebuffOverride : BuffHandler
	{
		private const int ComboThreshold = 100;
		private const int FeverDurationMs = 5000;
		private const int BrutalityBuffDurationMs = 10000;

		[CombatCalcModifier(CombatCalcPhase.AfterCalc, BuffId.JollyRoger_Enemy_Debuff)]
		public void OnDefenseAfterCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.JollyRoger_Enemy_Debuff, out var buff))
				return;

			if (skillHitResult.Result == HitResultType.Dodge)
				return;

			if (attacker is not Character character)
				return;

			if (!character.IsBuffActive(BuffId.JollyRoger_Buff))
				return;

			if (buff.Caster is not Character caster)
				return;

			this.TryApplyBrutality(caster, character);
			this.IncrementCombo(caster);
		}

		/// <summary>
		/// Checks if the JollyRoger caster has Brutality and applies the
		/// buff to the attacker and their party members.
		/// </summary>
		private void TryApplyBrutality(Character caster, Character attacker)
		{
			if (!caster.TryGetSkill(SkillId.Corsair_Brutality, out var brutalitySkill))
				return;

			if (attacker.TryGetBuff(BuffId.Brutality_Buff, out var existingBuff) && existingBuff.RemainingDuration >= TimeSpan.FromSeconds(6))
				return;

			var buffDuration = TimeSpan.FromMilliseconds(BrutalityBuffDurationMs);

			if (attacker.HasParty)
			{
				foreach (var member in attacker.Map.GetPartyMembers(attacker))
					member.StartBuff(BuffId.Brutality_Buff, brutalitySkill.Level, 0, buffDuration, caster);
			}
			else
			{
				attacker.StartBuff(BuffId.Brutality_Buff, brutalitySkill.Level, 0, buffDuration, caster);
			}
		}

		/// <summary>
		/// Increments the combo counter and handles Fever Time activation.
		/// </summary>
		private void IncrementCombo(Character caster)
		{
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

					var skillLevel = caster.TryGetSkill(SkillId.Corsair_JollyRoger, out var jollySkill) ? jollySkill.Level : 1;

					if (caster.HasParty)
					{
						foreach (var member in caster.Map.GetPartyMembers(caster))
							member.StartBuff(BuffId.FeverTime, skillLevel, 0, TimeSpan.FromMilliseconds(FeverDurationMs), caster, SkillId.Corsair_JollyRoger);
					}
					else
					{
						caster.StartBuff(BuffId.FeverTime, skillLevel, 0, TimeSpan.FromMilliseconds(FeverDurationMs), caster, SkillId.Corsair_JollyRoger);
					}
				}
			}
		}
	}
}
