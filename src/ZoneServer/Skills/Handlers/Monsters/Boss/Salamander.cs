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
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_salamander_Skill_1)]
	public class MonBossSalamanderSkill1 : ITargetSkillHandler
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

			var forceId = ForceId.GetNew();
			var position = GetRelativePosition(PosType.Self, caster, distance: 89.602318, angle: 0f);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, position, forceId, null);

			skill.Run(this.HandleSkill(caster, skill, position));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position position)
		{
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 1.5f),
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_salamander_Skill_2)]
	public class MonBossSalamanderSkill2 : ITargetSkillHandler
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

			var forceId = ForceId.GetNew();
			var position = GetRelativePosition(PosType.Self, caster);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, position, forceId, null);

			skill.Run(this.HandleSkill(caster, skill, position));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position position)
		{
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 8f),
				PositionDelay = 3000,
				Effect = new EffectConfig("F_buff_explosion_burst", 3f),
				Range = 100f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_salamander_Skill_3)]
	public class MonBossSalamanderSkill3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(3200);
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

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hitDelays = new[] { 3600, 200, 200, 200 };

			foreach (var delay in hitDelays)
			{
				var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 0, angle: 80);
				var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
				var hits = new List<SkillHitInfo>();
				await SkillAttack(caster, skill, splashArea, delay, delay, hits);
				SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, hits.Sum(h => h.HitInfo.Damage) * 0.5f, 6000f, 1, 100, -1, hits);
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_salamander_Skill_4)]
	public class MonBossSalamanderSkill4 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(200);
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

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position, forceId, null);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			var startingPosition = GetRelativePosition(PosType.Self, caster, distance: 50);
			var endingPosition = GetRelativePosition(PosType.Self, caster, distance: 250);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_monster", 1),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1.5f,
				PositionDelay = 3300f,
				HitEffect = new EffectConfig("F_fire013", 1.2f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.06f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			await skill.Wait(TimeSpan.FromMilliseconds(3100));
			if (caster.Components.TryGet<MovementComponent>(out var movementComponent))
				movementComponent.MoveTo(endingPosition);
			//caster.Position = endingPosition;
		}
	}

	[SkillHandler(SkillId.Mon_boss_salamander_Skill_5)]
	public class MonBossSalamanderSkill5 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(300);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position);

			skill.Run(this.HandleSkill(caster, target, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(600));

			var hitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.3", 8f),
				PositionDelay = 2000,
				Effect = EffectConfig.None,
				Range = 100f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = GetRelativePosition(PosType.Self, caster, target);
			await EffectAndHit(skill, caster, position, hitConfig);
			position = GetRelativePosition(PosType.Self, caster, target);
			await EffectAndHit(skill, caster, position, hitConfig);
		}
	}

	[SkillHandler(SkillId.Mon_boss_salamander_Skill_6)]
	public class MonBossSalamanderSkill6 : ITargetSkillHandler
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

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, caster.Position, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var spawnData = new (double distance, float angle, int height)[]
			{
				(76.034027, 105f, 2),
				(92.657486, 243f, 2),
				(126.91214, -11f, 2),
				(102.4713, 31f, 2),
				(121.82494, -63f, 1),
			};

			await skill.Wait(TimeSpan.FromMilliseconds(2000));

			foreach (var (distance, angle, height) in spawnData)
			{
				var spawnPos = GetRelativePosition(PosType.Self, caster, target, distance: distance, angle: angle, rand: 40, height: height);
				MonsterSkillCreateMob(skill, caster, "Salamander_summon", spawnPos, 0f, "", "BasicMonster_ATK", 0, 60f, "None", "");
				await skill.Wait(TimeSpan.FromMilliseconds(150));
			}
		}
	}
}
