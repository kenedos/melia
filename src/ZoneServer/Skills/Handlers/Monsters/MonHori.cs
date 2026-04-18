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

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_hori_chaser_Skill_1)]
	public class Mon_hori_chaser_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(1000);
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

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 15, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 800 + (int)(caster.Position.Get2DDistance(target.Position) * 3.4);
			var aniTime = hitDelay + 200;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 15, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 1150;
			aniTime = 150;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_hori_chaser_Skill_2)]
	public class Mon_hori_chaser_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(1200);
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

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 15, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 1000;
			var aniTime = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 15, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 1500;
			aniTime = 300;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 1700;
			aniTime = 200;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 1900;
			aniTime = 200;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 2100;
			aniTime = 200;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_hori_mage_Skill_1)]
	public class Mon_hori_mage_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(1000);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var originPos = caster.Position;
			var hitDelay = 800 + (int)(caster.Position.Get2DDistance(target.Position) * 3.4);
			var leadPos = GetLeadPosition(target, hitDelay);
			caster.TurnTowards(leadPos);
			var farPos = originPos.GetNearestPositionWithinDistance(leadPos, skill.Properties[PropertyName.MaxR]);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos, hitDelay));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos, int hitDelay)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 130, width: 10, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var aniTime = hitDelay + 200;
			_ = ForceAttackEffect(caster, target, skill, hitDelay);
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_hori_warrior_Skill_1)]
	public class Mon_hori_warrior_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(1200);
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

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 15, angle: 20f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 1000;
			var aniTime = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_hori_warrior_Skill_2)]
	public class Mon_hori_warrior_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(2200);
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

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 2000;
			var aniTime = 2200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 1000f, 1, 15, -1, hits);
		}
	}
}
