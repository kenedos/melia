using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_aklasdame_Skill_1)]
	public class Mon_aklasdame_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(900);
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

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 10);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 700;
			var damageDelay = 900;
			var hits = new List<SkillHitInfo>();
			await ForceAttackEffect(caster, target, skill, hitDelay);
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 3000f, 1, 8, -1, hits);


		}
	}

	[SkillHandler(SkillId.Mon_aklasdame_Skill_2)]
	public class Mon_aklasdame_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1100);
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

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 10);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 900;
			var damageDelay = 1100;
			await ForceAttackEffect(caster, target, skill, hitDelay);
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 10);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 100;
			damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_aklasdame_Skill_3)]
	public class Mon_aklasdame_Skill_3 : ITargetSkillHandler
	{
		private const int CastTimeMs = 1000;

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

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			if (!await MonsterCastTime(skill, caster, "Pole of Agony", CastTimeMs, target))
				return;

			var targetPos = caster.Position.GetNearestPositionWithinDistance(target.Position, 150);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_PoleofAgony);
		}
	}
}
