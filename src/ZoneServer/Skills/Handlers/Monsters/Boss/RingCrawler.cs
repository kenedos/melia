using System;
using System.Collections.Generic;
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
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_RingCrawler_Skill_1)]
	public class Mon_boss_RingCrawler_Skill_1 : ITargetSkillHandler
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
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_rize004_green1_force#Point003", 1.5f),
				EndEffect = new EffectConfig("F_explosion026_rize_green", 1f),
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			var delays = new[] { 200, 200, 200, 200, 200, 200, 200, 1000, 200, 200, 200, 200 };

			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);

			for (var i = 0; i < 13; i++)
			{
				if (i == 12)
					MonsterSkillSetCollisionDamage(caster, skill, false, 1f);

				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 140, height: 2);
				await MissileThrow(skill, caster, position, config);

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_RingCrawler_Skill_2)]
	public class Mon_boss_RingCrawler_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(4800);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 100);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 4500;
			var damageDelay = 4800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 100);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 1000;
			damageDelay = 5800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 100);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 1000;
			damageDelay = 6800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.RingCrawler_ElectricFields);
		}
	}

	[SkillHandler(SkillId.Mon_boss_RingCrawler_Skill_3)]
	public class Mon_boss_RingCrawler_Skill_3 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 11f),
				PositionDelay = 2200,
				Effect = new EffectConfig("F_ground026_rize", 3f),
				Range = 150f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Stun, 1, 0f, 5000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_RingCrawler_Skill_4)]
	public class Mon_boss_RingCrawler_Skill_4 : ITargetSkillHandler
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
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_rize004_green_force#Point005", 1f),
				EndEffect = new EffectConfig("F_explosion026_rize_green#Point006", 0.7f),
				Range = 20f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.4f),
			};

			await skill.Wait(TimeSpan.FromMilliseconds(2000));

			for (var i = 0; i < 20; i++)
			{
				var position = originPos.GetRelative(farPos);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_RingCrawler_Skill_5)]
	public class Mon_boss_RingCrawler_Skill_5 : ITargetSkillHandler
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
			var nearConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_rize006_green", 0.6f),
				Range = 35f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 2,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			};

			var farConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_rize006_green", 0.6f),
				Range = 50f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 2,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			};

			await skill.Wait(TimeSpan.FromMilliseconds(1500));

			var nearDelays = new[] { 350, 350, 350, 350, 350, 350, 1900 };
			for (var i = 0; i < 7; i++)
			{
				var position = originPos.GetRelative(farPos, distance: 50f);
				await EffectAndHit(skill, caster, position, nearConfig);
				await skill.Wait(TimeSpan.FromMilliseconds(nearDelays[i]));
			}

			var farDelays = new[] { 300, 300, 300, 300, 300, 500 };
			for (var i = 0; i < 7; i++)
			{
				var position = originPos.GetRelative(farPos, distance: 100f);
				await EffectAndHit(skill, caster, position, farConfig);
				if (i < farDelays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(farDelays[i]));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_RingCrawler_Skill_6)]
	public class Mon_boss_RingCrawler_Skill_6 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_RingCrawler_Rize);
		}
	}
}
