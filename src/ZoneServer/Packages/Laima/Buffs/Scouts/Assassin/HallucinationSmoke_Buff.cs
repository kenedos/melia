using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Assassin
{
	/// <summary>
	/// Handler for Hallucination Smoke Buff, which raises crit rate
	/// </summary>
	/// <remarks>
	/// NumArg1: Skill Level
	/// NumArg2: Unused
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.HallucinationSmoke_Buff)]
	internal class HallucinationSmoke_BuffOverride : BuffHandler
	{
		/// <summary>
		/// Starts buff, increasing Crit Rate
		/// </summary>
		/// <param name="buff"></param>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var level = buff.NumArg1;
			var evasionRate = 0.20f + level * 0.03f;

			var byAbility = 1f;
			if (buff.Caster is ICombatEntity casterEntity && casterEntity.TryGetSkill(buff.SkillId, out var skill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				byAbility += SCR_Get_AbilityReinforceRate(skill);
			}
			evasionRate *= byAbility;

			// Property only exists in PC Namespace
			if (buff.Target is Character)
				AddPropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM, evasionRate);
		}

		/// <summary>
		/// Ends the buff, resetting crit rate
		/// </summary>
		/// <param name="buff"></param>
		public override void OnEnd(Buff buff)
		{
			// Property only exists in PC Namespace
			if (buff.Target is Character)
				RemovePropertyModifier(buff, buff.Target, PropertyName.DR_RATE_BM);
		}
	}
}
