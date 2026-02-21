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
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUseHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	/// <summary>
	/// Handler for boss_Gaigalas Skill 1.
	/// Ground effect attack.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Gaigalas_Skill_1)]
	public class Mon_boss_Gaigalas_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 60);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 900,
				Effect = new EffectConfig("F_ground057", 1f),
				Range = 45f,
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

	/// <summary>
	/// Handler for boss_Gaigalas Skill 2.
	/// Multiple fire missile throws.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Gaigalas_Skill_2)]
	public class Mon_boss_Gaigalas_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1100));

			// Wave 1 - 2
			for (var i = 0; i < 2; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 1);
				_ = MissileWithFirewall(skill, caster, position);
			}
			for (var i = 0; i < 3; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 2);
				_ = MissileWithFirewall(skill, caster, position);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1400));

			// Wave 2 - 5
			for (var i = 0; i < 5; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 1);
				_ = MissileWithFirewall(skill, caster, position);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(500));

			// Wave 3 - 5
			for (var i = 0; i < 5; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 1);
				_ = MissileWithFirewall(skill, caster, position);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(500));

			// Wave 4 - 5
			for (var i = 0; i < 5; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 1);
				_ = MissileWithFirewall(skill, caster, position);
			}
		}

		private async Task MissileWithFirewall(Skill skill, ICombatEntity caster, Position position)
		{
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force054_fire#Dummy_head_effect", 1.3f),
				EndEffect = new EffectConfig("F_ground021_fire", 2.5f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			});
			SkillCreatePad(caster, skill, position, 0f, PadName.Mon_firewall);
		}
	}

	/// <summary>
	/// Handler for boss_Gaigalas Skill 3.
	/// Multi-hit circle AoE attack.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Gaigalas_Skill_3)]
	public class Mon_boss_Gaigalas_Skill_3 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 160, width: 60, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 2400;
			var damageDelay = 2600;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 60, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 170, width: 60, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 190, width: 60, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 60, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			hitDelay = 200;
			damageDelay = 200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);

			var rnd = new Random();
			var padCount = 12;
			var baseDirection = originPos.GetDirection(farPos);

			for (var i = 0; i < padCount; i++)
			{
				var distance = 50 + rnd.NextDouble() * 170;
				var widthOffset = (rnd.NextDouble() - 0.5) * 60;
				var padPos = originPos.GetRelative(baseDirection, (float)distance);
				padPos = padPos.GetRelative(new Direction((float)(baseDirection.DegreeAngle + 90)), (float)widthOffset);
				SkillCreatePad(caster, skill, padPos, 0f, PadName.Mon_firewall);
				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}

	/// <summary>
	/// Handler for boss_Gaigalas Skill 4.
	/// Two-hit circle AoE attack.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Gaigalas_Skill_4)]
	public class Mon_boss_Gaigalas_Skill_4 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 90, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2900;
			var damageDelay = 3100;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsCaster, 130, 10, 0, 1, 5, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 90, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 500;
			damageDelay = 3600;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsCaster, 130, 10, 0, 1, 5, hits);
		}
	}
}
