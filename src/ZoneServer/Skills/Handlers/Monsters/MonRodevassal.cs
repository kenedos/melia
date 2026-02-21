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
	[SkillHandler(SkillId.Mon_rodevassal_Skill_1)]
	public class Mon_rodevassal_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(600);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 400;
			var damageDelay = 600;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_rodevassal_Skill_2)]
	public class Mon_rodevassal_Skill_2 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 100f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force014_ice#Bone007", 0.5f),
				EndEffect = new EffectConfig("I_explosion006_blue", 1f),
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_rodevassal_Skill_3)]
	public class Mon_rodevassal_Skill_3 : ITargetSkillHandler
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
			var spawnPos = originPos.GetRelative(farPos, rand: 60, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "rodeyokel_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, rand: 60, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "rodenarcorng_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, rand: 60, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "rodenag_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "");
		}
	}

}
