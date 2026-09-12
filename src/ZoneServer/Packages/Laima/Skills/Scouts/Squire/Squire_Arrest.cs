using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Squire
{
	/// <summary>
	/// Handler for the Squire skill Arrest.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Squire_Arrest)]
	public class Squire_ArrestOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const float TargetDistance = 120f;
		private const float TargetWidth = 60f;
		private const int BuffDelay = 550;
		private const int BuffDurationMs = 10000;
		private const string VarBoundTargets = "Melia.Arrest.BoundTargets";

		/// <summary>
		/// Releases everything the bind is holding, since it lasts only as
		/// long as the channel does.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="maxCastTime"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (!skill.Vars.TryGet<List<ICombatEntity>>(VarBoundTargets, out var boundTargets))
				return;

			skill.Vars.Remove(VarBoundTargets);

			foreach (var boundTarget in boundTargets)
				boundTarget.StopBuff(BuffId.Arrest);
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

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);

			var skillTargets = SkillSelectEnemiesInSquare(caster, originPos, 0f, TargetDistance, TargetWidth, maxTargets);
			if (skillTargets.Count == 0)
				return;

			await skill.Wait(TimeSpan.FromMilliseconds(BuffDelay));

			SkillTargetBuff(skill, caster, skillTargets, BuffId.Arrest, skill.Level, 0f, TimeSpan.FromMilliseconds(BuffDurationMs), skill.Id);

			skill.Vars.Set(VarBoundTargets, skillTargets);
		}
	}
}
