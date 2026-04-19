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
			var targetPos = caster.Position.GetRelative(farPos, distance: 10f);
			// LimitBySR
			var targetList = SkillSelectEnemiesInSquare(caster, targetPos, 0f, 170f, 35f, 2);
			var hits = SkillTargetDamage(skill, caster, targetList, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			targetPos = originPos.GetRelative(farPos, distance: 10f);
			// LimitBySR
			targetList = SkillSelectEnemiesInSquare(caster, targetPos, 0f, 170f, 35f, 2);
			hits.AddRange(SkillTargetDamage(skill, caster, targetList, 1f));
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			targetPos = originPos.GetRelative(farPos, distance: 10f);
			// LimitBySR
			targetList = SkillSelectEnemiesInSquare(caster, targetPos, 0f, 170f, 35f, 2);
			hits.AddRange(SkillTargetDamage(skill, caster, targetList, 1f));
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 150, 10, 0, 1, 2, hits);
		}
	}
}
