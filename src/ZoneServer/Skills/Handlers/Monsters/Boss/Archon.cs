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
using static Melia.Zone.Skills.SkillUseFunctions;
using Yggdrasil.Util;
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_archon_Skill_1)]
	public class Mon_boss_archon_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 85, width: 30, angle: 90f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1600;
			var damageDelay = 1800;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 150, 30, 0, 0, 0, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_archon_Skill_2)]
	public class Mon_boss_archon_Skill_2 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 70);
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3.5f),
				PositionDelay = 2200,
				Effect = new EffectConfig("F_burstup013", 2f),
				Range = 40f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_archon_Skill_3)]
	public class Mon_boss_archon_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 59.332268f);
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			hits.Clear();
			position = originPos.GetRelative(farPos, distance: 50.974472f);
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 1300,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, hits.Sum(h => h.HitInfo.Damage) * 0.5f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_archon_Skill_4)]
	public class Mon_boss_archon_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			caster.StartBuff(BuffId.Rage_Rockto_def, 1f, 0f, TimeSpan.FromMilliseconds(20000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_archon_Skill_5)]
	public class Mon_boss_archon_Skill_5 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var hits = new List<SkillHitInfo>();
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_buff_fire_spread##1", 0.3f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion050_fire", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 160);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			hits.Clear();
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 160);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			hits.Clear();
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 160);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			hits.Clear();
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 130);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			hits.Clear();
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 130);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			hits.Clear();
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 130);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			hits.Clear();
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_buff_fire_spread##1", 0.5f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion050_fire", 2f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			hits.Clear();
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			hits.Clear();
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			hits.Clear();
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			hits.Clear();
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			hits.Clear();
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, hits.Sum(h => h.HitInfo.Damage) * 0.5f, 6000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_archon_Skill_6)]
	public class Mon_boss_archon_Skill_6 : ITargetSkillHandler
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
			var rnd = RandomProvider.Get();

			// Wave 1
			var position = originPos.GetRelative(farPos, distance: 70);
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_burstup013", 2f),
				Range = 40f,
				KnockdownPower = 300f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 75f,
				InnerRange = 0,
			});
			for (var i = 0; i < 3; i++)
			{
				var firePos = GetRandomPositionAround(originPos, rnd, 150);
				_ = EffectAndHit(skill, caster, firePos, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("F_buff_fire_spread##1", 0.3f),
					PositionDelay = 1000,
					Effect = new EffectConfig("F_explosion050_fire", 1f),
					Range = 20f,
					KnockdownPower = 100f,
					Delay = 200f,
					HitCount = 1,
					HitDuration = 0f,
					CasterEffect = EffectConfig.None,
					CasterNodeName = "None",
					KnockType = 1,
					VerticalAngle = 60f,
					InnerRange = 0,
				});
			}

			await skill.Wait(TimeSpan.FromMilliseconds(2000));

			// Wave 2
			position = originPos.GetRelative(farPos, distance: 70);
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_burstup013", 3.5f),
				Range = 60f,
				KnockdownPower = 300f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 75f,
				InnerRange = 0,
			});
			for (var i = 0; i < 3; i++)
			{
				var firePos = GetRandomPositionAround(originPos, rnd, 180);
				_ = EffectAndHit(skill, caster, firePos, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("F_buff_fire_spread##1", 0.4f),
					PositionDelay = 1000,
					Effect = new EffectConfig("F_explosion050_fire", 1.5f),
					Range = 30f,
					KnockdownPower = 100f,
					Delay = 200f,
					HitCount = 1,
					HitDuration = 0f,
					CasterEffect = EffectConfig.None,
					CasterNodeName = "None",
					KnockType = 1,
					VerticalAngle = 60f,
					InnerRange = 0,
				});
			}

			await skill.Wait(TimeSpan.FromMilliseconds(2900));

			// Wave 3
			position = originPos.GetRelative(farPos, distance: 70);
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_burstup013", 4f),
				Range = 80f,
				KnockdownPower = 300f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 75f,
				InnerRange = 0,
			});
			for (var i = 0; i < 5; i++)
			{
				var firePos = GetRandomPositionAround(originPos, rnd, 200);
				_ = EffectAndHit(skill, caster, firePos, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("F_buff_fire_spread##1", 0.5f),
					PositionDelay = 1000,
					Effect = new EffectConfig("F_explosion050_fire", 2f),
					Range = 40f,
					KnockdownPower = 100f,
					Delay = 200f,
					HitCount = 1,
					HitDuration = 0f,
					CasterEffect = EffectConfig.None,
					CasterNodeName = "None",
					KnockType = 1,
					VerticalAngle = 60f,
					InnerRange = 0,
				});
			}
		}

		private static Position GetRandomPositionAround(Position center, Random rnd, float maxDistance)
		{
			var angle = rnd.NextDouble() * Math.PI * 2;
			var distance = rnd.NextDouble() * maxDistance;
			return new Position(
				center.X + (float)(Math.Cos(angle) * distance),
				center.Y,
				center.Z + (float)(Math.Sin(angle) * distance)
			);
		}
	}
}
