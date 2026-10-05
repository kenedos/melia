using System.Collections.Generic;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Packages.Laima.Skills.Swordsmen.Templar
{
	[Package("laima")]
	[SkillHandler(SkillId.Templer_RevengeBanner)]
	public class Templer_RevengeBannerOverride : ITargetSkillHandler, IGroundSkillHandler, IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float AreaMultiplier = 4f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float castTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (caster == null)
				return;

			var targetPosition = target?.Position ?? caster.Position;
			this.Cast(skill, caster, caster.Position, targetPosition);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (caster == null || caster.IsDead)
			{
				StopSkill(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				StopSkill(caster);
				return;
			}

			try
			{
				var targetPosition = this.GetTargetPosition(skill, farPos);
				var areaRadius = skill.Data.SplashRange * AreaMultiplier;
				var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

				skill.IncreaseOverheat();
				caster.SetAttackState(true);
				caster.TurnTowards(targetPosition);

				Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, targetPosition);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, targetPosition);
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPosition);

				SkillRemovePad(caster, skill);
				SkillCreatePad(caster, skill, targetPosition, 0f, PadName.Templer_RevengeBanner, range: areaRadius);
			}
			finally
			{
				StopSkill(caster);
			}
		}

		private Position GetTargetPosition(Skill skill, Position farPos)
		{
			if (skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPosition))
				return targetPosition;

			return farPos;
		}

		private static void StopSkill(ICombatEntity caster)
		{
			if (caster == null)
				return;

			caster.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(caster);
		}
	}
}
