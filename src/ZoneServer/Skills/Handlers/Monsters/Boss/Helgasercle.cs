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
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_helgasercle_Skill_1)]
	public class Mon_boss_helgasercle_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(2000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 60, angle: 45f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1800;
			var aniTime = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 20000f, 1, 100, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, 0f, 7000f, 1, 20, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_helgasercle_Skill_2)]
	public class Mon_boss_helgasercle_Skill_2 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 60, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2000;
			var aniTime = 2200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 20000f, 1, 100, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, 0f, 7000f, 1, 20, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_helgasercle_Skill_3)]
	public class Mon_boss_helgasercle_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2400));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red#Bip001 R Hand", 1f),
				EndEffect = new EffectConfig("F_explosion046_red", 1f),
				Range = 20f,
				FlyTime = 0.3f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			};

			for (var i = 0; i < 5; i++)
			{
				var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 250f);
				await skill.Wait(TimeSpan.FromMilliseconds(250));
				_ = MissileThrow(skill, caster, targetPos, missileConfig);
			}
			
		}
	}

	[SkillHandler(SkillId.Mon_boss_helgasercle_Skill_4)]
	public class Mon_boss_helgasercle_Skill_4 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(2500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 100, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var aniTime = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	[SkillHandler(SkillId.Mon_boss_helgasercle_Skill_5)]
	public class Mon_boss_helgasercle_Skill_5 : ITargetSkillHandler
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
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var spawnPos = originPos.GetRelative(farPos, distance: 90);
			MonsterSkillCreateMob(skill, caster, "helgasercle_phantom", spawnPos, 180f, "헬가세르클의 분신체", "BasicMonster_ATK", -5, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 90, angle: 119f);
			MonsterSkillCreateMob(skill, caster, "helgasercle_phantom", spawnPos, 299f, "헬가세르클의 분신체", "BasicMonster_ATK", -5, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 90, angle: 240f);
			MonsterSkillCreateMob(skill, caster, "helgasercle_phantom", spawnPos, 60f, "헬가세르클의 분신체", "BasicMonster_ATK", -5, 0f, "helga_real", "");
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			
			caster.StartBuff(BuffId.Invincible, 1f, 0f, TimeSpan.FromSeconds(10), caster);
		}
	}
}
