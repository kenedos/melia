using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.HandlersOverrides.Wizards.Chronomancer
{
	/// <summary>
	/// Handler for QuickCast_Buff.
	/// Reduces cast time of the next casted skill, then is consumed.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.QuickCast_Buff)]
	public class QuickCast_BuffOverride : BuffHandler, IBuffOnCastStartHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var skillLevel = Math.Max(1, (int)buff.NumArg1);
			var castReduction = 30f + 3f * skillLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.CastingSpeed_BM, castReduction);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.CastingSpeed_BM);
		}

		public void OnCastStart(Buff buff, ICombatEntity caster, Skill skill)
		{
			if (skill.Data.BasicCast > 0)
			{
				if (caster.TryGetActiveAbilityLevel(AbilityId.Chronomancer15, out var abilLevel))
					caster.StartBuff(BuffId.QuickCast_After_Buff, abilLevel, 0f, TimeSpan.FromSeconds(5), caster);

				caster.StopBuff(BuffId.QuickCast_Buff);
			}
		}
	}
}
