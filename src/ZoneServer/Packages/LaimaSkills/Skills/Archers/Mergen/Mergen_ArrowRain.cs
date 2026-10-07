using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.Util;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Mergen
{
	/// <summary>
	/// Handler for the Mergen skill Arrow Sprinkle, which rains arrows on
	/// the target area for up to 5 seconds, scaled by how long it was cast.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Mergen_ArrowRain)]
	public class Mergen_ArrowRainOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const string CastStartVar = "Melia.Mergen.ArrowRainCastStart";
		private static readonly TimeSpan MaxCastTime = TimeSpan.FromSeconds(1);
		private static readonly TimeSpan MaxDuration = TimeSpan.FromSeconds(5);

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			skill.Vars.Set(CastStartVar, GameClock.LocalNow);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var rainPos))
				rainPos = farPos;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, rainPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, rainPos, ForceId.GetNew(), null);

			var pad = SkillCreatePad(caster, skill, rainPos, 0f, PadName.Mergen_ArrowRain);
			if (pad != null)
				pad.Trigger.LifeTime = this.GetDuration(skill);
		}

		/// <summary>
		/// Returns how long the arrows rain, proportional to the time the
		/// skill was cast.
		/// </summary>
		/// <param name="skill"></param>
		/// <returns></returns>
		private TimeSpan GetDuration(Skill skill)
		{
			var castStart = skill.Vars.Get<DateTime>(CastStartVar, DateTime.MinValue);
			skill.Vars.Remove(CastStartVar);

			if (castStart == DateTime.MinValue)
				return MaxDuration;

			var castTime = GameClock.LocalNow - castStart;
			var castRate = Math.Clamp(castTime.TotalMilliseconds / MaxCastTime.TotalMilliseconds, 0.1, 1.0);

			return MaxDuration * castRate;
		}
	}
}
