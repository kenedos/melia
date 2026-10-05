using System;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using System.Threading.Tasks;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Packages.Laima.Skills.Archers.Mergen
{
	[Package("laima")]
	[SkillHandler(SkillId.Mergen_ArrowRain)]
	public class Mergen_ArrowSprinkle : IGroundSkillHandler, IDynamicCasted
	{
		private const string CastStartVariable = "Melia.Mergen.ArrowSprinkle.CastStart";
		private const int MaximumCastingTimeMilliseconds = 1000;
		private const int MaximumDurationMilliseconds = 5000;
		private const float PadRange = 120;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			skill.Vars.SetInt(CastStartVariable, Environment.TickCount);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster == null || caster.IsDead)
				return;

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			var castingTime = this.GetCastingTime(skill);
			var duration = TimeSpan.FromMilliseconds(
				(castingTime / (float)MaximumCastingTimeMilliseconds) *
				MaximumDurationMilliseconds
			);

			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, originPos.GetDirection(farPos), farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			SkillRemovePad(caster, skill);

			var pad = SkillCreatePad(
				caster,
				skill,
				farPos,
				0f,
				PadName.Mergen_ArrowRain,
				range: PadRange
			);

			if (pad != null)
				skill.Run(this.DestroyPadAfterDuration(skill, pad, duration));

			skill.IncreaseOverheat();
			skill.Vars.Remove(CastStartVariable);
			caster.SetAttackState(false);
		}

		private async Task DestroyPadAfterDuration(Skill skill, Pad pad, TimeSpan duration)
		{
			await skill.Wait(duration);

			if (pad.Map != null)
				pad.Destroy();
		}

		private int GetCastingTime(Skill skill)
		{
			var castStart = skill.Vars.GetInt(CastStartVariable);

			if (castStart == 0)
				return MaximumCastingTimeMilliseconds;

			var elapsed = unchecked(Environment.TickCount - castStart);
			return Math.Clamp(elapsed, 1, MaximumCastingTimeMilliseconds);
		}
	}
}
