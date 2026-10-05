using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Krivis
{
	/// <summary>
	/// Handle for Melstis_Buff, which periodically regenerates SP
	/// based on a percentage of the caster's max SP.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Melstis_Buff)]
	public class Krivis_Melstis_BuffOverride : BuffHandler
	{
		private const int HealTickMs = 2000;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.SetUpdateTime(HealTickMs);
			this.RegenSp(buff);
		}

		public override void WhileActive(Buff buff)
		{
			this.RegenSp(buff);
		}

		/// <summary>
		/// Regenerates SP for the buff target based on a percentage of
		/// the caster's max SP, distributed over the buff duration.
		/// </summary>
		private void RegenSp(Buff buff)
		{
			var caster = (ICombatEntity)buff.Caster;

			if (caster == null || caster.IsDead)
				return;

			var target = buff.Target;
			if (target is not Character character)
				return;

			var totalSpRecharge = this.GetTotalSpRecharge(caster, buff);
			var tickCount = 20000f / HealTickMs; // 20s duration / tick interval
			var spPerTick = totalSpRecharge / tickCount;

			var spToRecharge = Math.Min(spPerTick, character.MaxSp - character.Sp);
			character.Heal(0, spToRecharge);
		}

		/// <summary>
		/// Calculates the total SP to be recharged over the buff's duration.
		/// </summary>
		private float GetTotalSpRecharge(ICombatEntity caster, Buff buff)
		{
			var skillLevel = buff.NumArg1;
			var basePercentage = 0.20f;
			var percentagePerLevel = 0.04f;
			var totalPercentage = basePercentage + (skillLevel - 1) * percentagePerLevel;

			if (caster.TryGetSkill(SkillId.Kriwi_Melstis, out var skill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				totalPercentage *= 1f + SCR_Get_AbilityReinforceRate(skill);
			}

			if (caster is Character casterChar)
				return casterChar.MaxSp * totalPercentage;
			else
				return caster.Level * 10 * totalPercentage;
		}
	}
}
