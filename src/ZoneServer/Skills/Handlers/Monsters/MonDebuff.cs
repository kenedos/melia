using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_Debuff_Monster1_Skill_1)]
	public class Mon_Debuff_Monster1_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			var targetPos = GetRelativePosition(PosType.TargetHeight, caster, target);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 9999f, 20));
			var skillTargets = caster.GetTargets();
			SkillTargetBuff(skill, caster, skillTargets, BuffId.Catacom_DEF_Debuff, 1f, 0f, TimeSpan.FromMilliseconds(15000f));
		}
	}

	[SkillHandler(SkillId.Mon_Debuff_Monster2_Skill_1)]
	public class Mon_Debuff_Monster2_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			var targetPos = GetRelativePosition(PosType.TargetHeight, caster, target);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 9999f, 20));
			var skillTargets = caster.GetTargets();
			SkillTargetBuff(skill, caster, skillTargets, BuffId.Catacom_MDEF_Debuff, 1f, 0f, TimeSpan.FromMilliseconds(15000f));
		}
	}

	[SkillHandler(SkillId.Mon_Debuff_Monster3_Skill_1)]
	public class Mon_Debuff_Monster3_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			var targetPos = GetRelativePosition(PosType.TargetHeight, caster, target);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 9999f, 20));
			var skillTargets = caster.GetTargets();
			SkillTargetBuff(skill, caster, skillTargets, BuffId.Catacom_ATK_Debuff, 1f, 0f, TimeSpan.FromMilliseconds(15000f));
		}
	}

	[SkillHandler(SkillId.Mon_Debuff_Monster4_Skill_1)]
	public class Mon_Debuff_Monster4_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.TurnTowards(target);
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			var targetPos = GetRelativePosition(PosType.TargetHeight, caster, target);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 9999f, 20));
			var skillTargets = caster.GetTargets();
			SkillTargetBuff(skill, caster, skillTargets, BuffId.Catacom_MATK_Debuff, 1f, 0f, TimeSpan.FromMilliseconds(15000f));
		}
	}
}
