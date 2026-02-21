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
	[SkillHandler(SkillId.Mon_InfroBurk_Skill_1)]
	public class Mon_InfroBurk_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(800);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 35, width: 10, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 800;
			var damageDelay = 800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_InfroBurk_Skill_2)]
	public class Mon_InfroBurk_Skill_2 : ITargetSkillHandler
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
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force054_fire#topbody_01", 1.2f),
				EndEffect = new EffectConfig("F_ground021_fire", 2f),
				Range = 10f,
				FlyTime = 1.3f,
				DelayTime = 0f,
				Gravity = 200f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.5f),
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 110, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 110, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 110, height: 2);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_force054_fire#topbody_01", 1.2f),
				EndEffect = new EffectConfig("F_ground021_fire", 2f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1800f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 0.5f),
			};

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 110, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 110, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
		}
	}
}
