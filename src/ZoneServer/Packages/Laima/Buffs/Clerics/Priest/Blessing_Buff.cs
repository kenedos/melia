using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for Blessing, Increases Attack.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Blessing_Buff)]
	public class Blessing_BuffOverride : BuffHandler
	{
		private const float BaseATKMultiplier = 0.20f;
		private const float ATKMultiplierPerLevel = 0.02f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = (ICombatEntity)buff.Caster;
			var target = buff.Target;
			var level = buff.NumArg1;
			var defMultiplier = BaseATKMultiplier + (ATKMultiplierPerLevel * level);

			if (caster.TryGetSkill(buff.SkillId, out var skill))
			{
				var SCR_Get_AbilityReinforceRate = ScriptableFunctions.Skill.Get("SCR_Get_AbilityReinforceRate");
				defMultiplier *= 1f + SCR_Get_AbilityReinforceRate(skill);
			}

			// Apply the defense modifier
			AddPropertyModifier(buff, target, PropertyName.PATK_RATE_BM, defMultiplier);
			AddPropertyModifier(buff, target, PropertyName.MATK_RATE_BM, defMultiplier);
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			// Remove the defense modifier
			RemovePropertyModifier(buff, target, PropertyName.PATK_RATE_BM);
			RemovePropertyModifier(buff, target, PropertyName.MATK_RATE_BM);
		}
	}
}
