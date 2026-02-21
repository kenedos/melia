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
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Glackuman_Skill_1)]
	public class Mon_boss_Glackuman_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1200);
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
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 60, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Glackuman_Skill_2)]
	public class Mon_boss_Glackuman_Skill_2 : ITargetSkillHandler
	{
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
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			var position = originPos.GetRelative(farPos, distance: 50);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 500,
				Effect = new EffectConfig("F_ground092_dark", 3f),
				Range = 55f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 0f,
				InnerRange = 30f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 50);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 4f),
				PositionDelay = 500,
				Effect = new EffectConfig("None", 3f),
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 70f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 50);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground092_dark", 5f),
				PositionDelay = 500,
				Effect = new EffectConfig("None", 5f),
				Range = 130f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 0f,
				InnerRange = 100f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Glackuman_Skill_3)]
	public class Mon_boss_Glackuman_Skill_3 : ITargetSkillHandler
	{
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
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force011_green#Bip001 R UpperArm", 2f),
				EndEffect = new EffectConfig("F_ground004_green", 0.8f),
				DotEffect = new EffectConfig("I_force003_green", 1f),
				Range = 40f,
				FlyTime = 1.5f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_force011_green#Bip001 L UpperArm", 2f),
				EndEffect = new EffectConfig("F_ground004_green", 0.8f),
				DotEffect = new EffectConfig("I_force003_green", 1f),
				Range = 40f,
				FlyTime = 1.5f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Glackuman_Skill_4)]
	public class Mon_boss_Glackuman_Skill_4 : ITargetSkillHandler
	{
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
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var position = originPos.GetRelative(farPos, distance: 50);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("E_simorph_skl", 2f),
				Range = 85f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 200, rand: 70);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_simorph_skl_3Deffect_mash", 0.5f),
				EndEffect = new EffectConfig("E_simorph_skl", 1f),
				DotEffect = EffectConfig.None,
				Range = 40f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Glackuman_Skill_5)]
	public class Mon_boss_Glackuman_Skill_5 : ITargetSkillHandler
	{
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
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_simorph_skl_3Deffect_mash#Bip001 R Hand", 0.5f),
				EndEffect = new EffectConfig("E_simorph_skl", 1f),
				DotEffect = EffectConfig.None,
				Range = 50f,
				FlyTime = 0.7f,
				DelayTime = 0f,
				Gravity = 100f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			for (var i = 0; i < 3; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 50);
				await MissileThrow(skill, caster, position, config);

				if (i < 2)
					await skill.Wait(TimeSpan.FromMilliseconds(1500));
			}
		}
	}
}
