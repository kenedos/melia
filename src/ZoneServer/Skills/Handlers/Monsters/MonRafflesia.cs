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

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_rafflesia_purple_Skill_1)]
	public class Mon_rafflesia_purple_Skill_1 : ITargetSkillHandler
	{
		private const int CastTimeMs = 1500;
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

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			if (!await MonsterCastTime(skill, caster, "Poison Pool", CastTimeMs, target))
				return;

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 0, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);

			await skill.Wait(TimeSpan.FromMilliseconds(1500));

			// Two waves of 3 missiles each
			for (var wave = 0; wave < 2; wave++)
			{
				if (wave > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(1000));

				for (var i = 0; i < 3; i++)
				{
					var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 1);
					position = originPos.GetNearestPositionWithinDistance(position, 300f);
					_ = FireMissileWithPad(skill, caster, position);
				}
			}
		}

		private async Task FireMissileWithPad(Skill skill, ICombatEntity caster, Position position)
		{
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force046_green#NDummy_head", 0.5f),
				EndEffect = new EffectConfig("F_ground116_green", 1.3f),
				Range = 20f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 450f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});
			var pad = SkillCreatePad(caster, skill, position, 0f, PadName.Mon_PoisonPilla);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(3000);
		}
	}

	[SkillHandler(SkillId.Mon_rafflesia_purple_Skill_2)]
	public class Mon_rafflesia_purple_Skill_2 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 50, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 8000f, 1, 20, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_rafflesia_Skill, SkillId.Mon_rafflesia_Attack1)]
	public class Mon_rafflesia_Skill : ITargetSkillHandler
	{
		private const int CastTimeMs = 1000;
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

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			if (!await MonsterCastTime(skill, caster, "Poison Pool", CastTimeMs, target))
				return;

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 0, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			await skill.Wait(TimeSpan.FromMilliseconds(1500));

			// Two waves of 3 missiles each
			for (var wave = 0; wave < 2; wave++)
			{
				if (wave > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(1000));

				for (var i = 0; i < 3; i++)
				{
					var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 1);
					position = originPos.GetNearestPositionWithinDistance(position, 300f);
					_ = FireMissileWithPad(skill, caster, position);
				}
			}
		}

		private async Task FireMissileWithPad(Skill skill, ICombatEntity caster, Position position)
		{
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force046_green#NDummy_head", 0.5f),
				EndEffect = new EffectConfig("F_ground116_green", 1.3f),
				Range = 20f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 450f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});
			var pad = SkillCreatePad(caster, skill, position, 0f, PadName.Mon_PoisonPilla);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(3000);
		}
	}

	[SkillHandler(SkillId.Mon_rafflesia_Skill_1)]
	public class Mon_rafflesia_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 50, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_sleep, 1, 0f, 8000f, 1, 20, -1, hits);
		}
	}
}
