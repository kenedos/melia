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
	[SkillHandler(SkillId.Mon_ShillEclipse_Skill_1)]
	public class Mon_ShillEclipse_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.Zero;
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 0);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 0;
			var damageDelay = 0;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			var targetPos = originPos.GetRelative(farPos);
			var targets = SkillSelectEnemiesInSquare(caster, targetPos, 0f, 100f, 15f, 20);
			if (targets.Count == 0)
				return;
			await skill.Wait(TimeSpan.FromMilliseconds(550));
			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 3.7854534E-36);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_dark3#Dummy_effect_01", 2f),
				EndEffect = new EffectConfig("F_explosion007_white", 0.3f),
				Range = 20f,
				FlyTime = 0.25f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			});
		}
	}

	[SkillHandler(SkillId.Mon_ShillEclipse_Skill_2)]
	public class Mon_ShillEclipse_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.Zero;
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
			var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 2.9022666E+25);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force098_white#Dummy_effect_02", 2f),
				EndEffect = new EffectConfig("F_explosion007_white", 0.3f),
				Range = 20f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			});
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 100f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(650));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force098_white#Dummy_effect_02", 0.5f),
				EndEffect = new EffectConfig("F_explosion007_white", 0.4f),
				Range = 20f,
				FlyTime = 0.6f,
				DelayTime = 0.28f,
				Gravity = 1500f,
				Speed = 10f,
				HitTime = 100f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			for (var i = 0; i < 5; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 3.7854534E-36, rand: 20, height: 1);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_ShillEclipse_Skill_3)]
	public class Mon_ShillEclipse_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.Zero;
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 0);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 0;
			var damageDelay = 0;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			var targetPos = originPos.GetRelative(farPos);
			var targets = SkillSelectEnemiesInSquare(caster, targetPos, 0f, 90f, 30f, 5);
			if (targets.Count == 0)
				return;
			await skill.Wait(TimeSpan.FromMilliseconds(750));
			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 3.7854534E-36);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_fire_whiteball#Dummy_effect_02", 1.3f),
				EndEffect = new EffectConfig("I_cylinder010_light_dark", 0.4f),
				Range = 30f,
				FlyTime = 0.6f,
				DelayTime = 0.3f,
				Gravity = 750f,
				Speed = 2f,
				HitTime = 0f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			});
		}
	}

}
