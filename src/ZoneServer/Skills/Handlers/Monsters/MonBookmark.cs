using System;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.Skills.SplashAreas;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_bookmark_mimic_Skill_1)]
	public class Mon_bookmark_mimic_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(600);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var hitDelay = 400;
			var aniTime = 600;
			var leadPos = GetLeadPosition(target, hitDelay, caster);
			caster.TurnTowards(leadPos);
			var farPos = originPos.GetNearestPositionWithinDistance(leadPos, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos, hitDelay, aniTime));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos, int hitDelay, int aniTime)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 12, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 12, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 100;
			aniTime = 100;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_bookmark_mimic_Skill_2)]
	public class Mon_bookmark_mimic_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(900);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var hitDelay = 700;
			var aniTime = 900;
			var leadPos = GetLeadPosition(target, hitDelay, caster);
			caster.TurnTowards(leadPos);
			var farPos = originPos.GetNearestPositionWithinDistance(leadPos, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos, hitDelay, aniTime));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos, int hitDelay, int aniTime)
		{
			ISplashArea splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 68f, angle: -67f), 15f);
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 75f, angle: -44f), 15f);
			hitDelay = 50;
			aniTime = 50;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 91f, angle: -26f), 15f);
			hitDelay = 50;
			aniTime = 50;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 105f, angle: -12f), 15f);
			hitDelay = 50;
			aniTime = 50;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 113f, angle: 3f), 15f);
			hitDelay = 50;
			aniTime = 50;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 12, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 50;
			aniTime = 50;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}
}
