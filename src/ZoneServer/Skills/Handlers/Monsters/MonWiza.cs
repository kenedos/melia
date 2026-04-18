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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_Wiza_moya_Skill_1)]
	public class Mon_Wiza_moya_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(300);
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
			var hitDelay = 100;
			var leadPos = GetLeadPosition(target, hitDelay);
			caster.TurnTowards(leadPos);
			var farPos = originPos.GetNearestPositionWithinDistance(leadPos, skill.Properties[PropertyName.MaxR]);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos, hitDelay));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos, int hitDelay)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 10, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var aniTime = 300;
			_ = ForceAttackEffect(caster, target, skill, hitDelay);
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_Wiza_moya_Skill_2)]
	public class Mon_Wiza_moya_Skill_2 : ITargetSkillHandler
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

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster##0.2", 0.5f),
				PositionDelay = 600,
				Effect = new EffectConfig("F_burstup015_yellow", 0.5f),
				Range = 10f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 20);
			await EffectAndHit(skill, caster, position, config);
		}
	}
}
