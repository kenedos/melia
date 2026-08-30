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
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Musketeer
{
	/// <summary>
	/// Handler for the Musketeer skill Penetration Shot.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Musketeer_PenetrationShot)]
	public class Musketeer_PenetrationShotOverride : IGroundSkillHandler
	{
		private const float TargetOffset = 10f;
		private const float TargetDistance = 170f;
		private const float TargetWidth = 35f;
		private const int MaxTargetsPerVolley = 2;
		private const int VolleyCount = 3;
		private static readonly TimeSpan VolleyDelay = TimeSpan.FromMilliseconds(300);
		private static readonly TimeSpan CancelDelay = TimeSpan.FromMilliseconds(150);

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
			var targetPos = originPos.GetRelative(farPos, distance: TargetOffset);
			var hits = new List<SkillHitInfo>();

			for (var i = 0; i < VolleyCount; ++i)
			{
				var targetList = SkillSelectEnemiesInSquare(caster, targetPos, 0f, TargetDistance, TargetWidth, MaxTargetsPerVolley);
				hits.AddRange(SkillTargetDamage(skill, caster, targetList));

				if (i < VolleyCount - 1)
					await skill.Wait(VolleyDelay);
			}

			await skill.Wait(CancelDelay);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);

			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 150, 10, 0, 1, 2, hits);
		}
	}
}
