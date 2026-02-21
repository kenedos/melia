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
	[SkillHandler(SkillId.Mon_boss_Mothstem_Skill_1)]
	public class Mon_boss_Mothstem_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2700);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2600;
			var damageDelay = 2700;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 5000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Mothstem_Skill_2)]
	public class Mon_boss_Mothstem_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2600));
			var position = originPos.GetRelative(farPos);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Mothstem, 0f, 200f, 12, 55f, 200f, 50f, 150f);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Mothstem_Skill_3)]
	public class Mon_boss_Mothstem_Skill_3 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos);
			var hits = new List<SkillHitInfo>();
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_circle005_rize", 1.5f),
				EndEffect = new EffectConfig("F_explosion024_rize", 0.5f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				DelayTime = 0.2f,
				FlyTime = 0.6f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
			};

			for (var i = 0; i < 11; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
				await MissileFall(caster, skill, position, config, hits);

				if (i < 10)
					await skill.Wait(TimeSpan.FromMilliseconds(250));
			}
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Mothstem_Skill_4)]
	public class Mon_boss_Mothstem_Skill_4 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_circle005_rize#Bone_Teil_R02", 0.7f),
				EndEffect = new EffectConfig("F_explosion024_rize", 0.7f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 2f),
			};

			var delays = new[] { 50, 50, 50, 50, 200, 200 };
			for (var i = 0; i < 7; i++)
			{
				var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
				await MissileThrow(skill, caster, position, config);

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
		}
	}
}
