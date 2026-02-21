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
using Yggdrasil.Util;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_onion_the_great_Skill_1)]
	public class Mon_boss_onion_the_great_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_smoke090", 3f),
				Range = 80f,
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

	[SkillHandler(SkillId.Mon_boss_onion_the_great_Skill_2)]
	public class Mon_boss_onion_the_great_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1050));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_circle007_mash", 1f),
				EndEffect = new EffectConfig("E_onion_the_great_skl2", 1.2f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				DelayTime = 1f,
				FlyTime = 1f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("None", 0.5f),
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120);
			await MissileFall(caster, skill, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_boss_onion_the_great_Skill_3)]
	public class Mon_boss_onion_the_great_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1700);
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target?.Handle ?? 0, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 50, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1500;
			var damageDelay = 1700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_onion_the_great_Skill_4)]
	public class Mon_boss_onion_the_great_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var spawnPos = originPos.GetRelative(farPos, distance: 50);

			MonsterSkillCreateMob(skill, caster, GetRandomMob(), spawnPos, 0f, "", "BasicMonster_ATK", -2, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 50);
			MonsterSkillCreateMob(skill, caster, GetRandomMob(), spawnPos, 0f, "", "BasicMonster_ATK", -2, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 50);
			MonsterSkillCreateMob(skill, caster, GetRandomMob(), spawnPos, 0f, "", "BasicMonster_ATK", -2, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 50);
			MonsterSkillCreateMob(skill, caster, GetRandomMob(), spawnPos, 0f, "", "BasicMonster_ATK", -2, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 50);
			MonsterSkillCreateMob(skill, caster, GetRandomMob(), spawnPos, 0f, "", "BasicMonster_ATK", -2, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 50);
			MonsterSkillCreateMob(skill, caster, GetRandomMob(), spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "onion_hasbuff", "");
		}

		private string GetRandomMob()
		{
			var random = RandomProvider.Get().Next(4);
			var mob = "raider";
			switch (random)
			{
				case 1:
					mob = "Onion_Red";
					break;
				case 2:
					mob = "pappus_kepa";
					break;
				case 3:
					mob = "ellom";
					break;
			}
			return mob;
		}
	}
}
