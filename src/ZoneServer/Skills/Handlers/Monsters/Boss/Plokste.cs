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
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_plokste_Skill_1)]
	public class Mon_boss_plokste_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 900;
			var damageDelay = 1100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_plokste_Skill_2)]
	public class Mon_boss_plokste_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 60);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup007_yellow", 1f),
				Range = 70f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 3,
				HitDuration = 300f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 20000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_plokste_Skill_3)]
	public class Mon_boss_plokste_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground083_smoke", 1f),
				Range = 80f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 88, angle: 79f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 76, angle: 16f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 82, angle: -43f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 79, angle: -91f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 50, angle: -130f);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 148, angle: 15f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 182.29912f, angle: -18f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 205.29018f, angle: -54f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 214.38309f, angle: -85f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 127.64243f, angle: 142f);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 300.67844f, angle: -2f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 337.8923f, angle: -29f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 382.69525f, angle: -56f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 388.14017f, angle: -82f);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 282.26563f, angle: 129f);
		}
	}

	[SkillHandler(SkillId.Mon_boss_plokste_Skill_4)]
	public class Mon_boss_plokste_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup003", 0.5f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 30);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 60);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 90);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 120);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup003", 1.2f),
				Range = 45f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
		}
	}
}
