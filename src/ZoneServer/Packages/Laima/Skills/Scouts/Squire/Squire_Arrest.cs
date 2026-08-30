using System;
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
	public class Squire_ArrestOverride : IGroundSkillHandler
	{
		private const float TargetDistance = 120f;
		private const float TargetWidth = 30f;
		private const int BuffDelay = 550;
		private const int BuffDurationMs = 10000;
		private const int SlowDurationMs = 4000;
		private const int SlowDurationPerAbilityLevel = 400;

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
			var targetPos = originPos.GetRelative(farPos);
			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);

			var skillTargets = SkillSelectEnemiesInSquare(caster, targetPos, 0f, TargetDistance, TargetWidth, maxTargets);
			if (skillTargets.Count == 0)
				return;

			await skill.Wait(TimeSpan.FromMilliseconds(BuffDelay));

			SkillTargetBuff(skill, caster, skillTargets, BuffId.Arrest, skill.Level, 0f, TimeSpan.FromMilliseconds(BuffDurationMs), skill.Id);

			await skill.Wait(TimeSpan.FromMilliseconds(BuffDurationMs));

			foreach (var skillTarget in skillTargets)
				skillTarget.StopBuff(BuffId.Arrest);

			SkillTargetBuffAbility(caster, skill, skillTargets, AbilityId.Squire1, BuffId.UC_slowdown, 1, -1, SlowDurationMs, SlowDurationPerAbilityLevel, 1, 100, skill.Id);
		}
	}
}
