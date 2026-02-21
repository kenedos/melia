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
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUseHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Rambandgad_Skill_1)]
	public class Mon_boss_Rambandgad_Skill_1 : ITargetSkillHandler
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
			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130);
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("F_explosion098_dark_mint#Bip001 L Finger21", 0.8f),
				EndEffect = new EffectConfig("F_explosion098_dark_mint", 1f),
				Range = 50f,
				FlyTime = 0.2f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});

		}
	}

	[SkillHandler(SkillId.Mon_boss_Rambandgad_Skill_2)]
	public class Mon_boss_Rambandgad_Skill_2 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			var position = GetRelativePosition(PosType.TargetHeight, caster, target);
			await skill.Wait(TimeSpan.FromMilliseconds(2490));
			var hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("F_explosion098_dark_mint#Bip001 L Finger21", 2f),
				EndEffect = new EffectConfig("F_explosion098_dark_mint", 1f),
				Range = 80f,
				FlyTime = 0.2f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			}, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 200, 60, 0, 1, 4, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rambandgad_Skill_3)]
	public class Mon_boss_Rambandgad_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 20);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 150, 10, 10, 1, 2, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rambandgad_Skill_4)]
	public class Mon_boss_Rambandgad_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2400));
			var targetPos = GetRelativePosition(PosType.TargetRandom, caster, caster, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, caster, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, caster, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, caster, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, caster, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, caster, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, caster, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, caster, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, caster, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			targetPos = GetRelativePosition(PosType.TargetRandom, caster, caster, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rambandgad_Skill_5)]
	public class Mon_boss_Rambandgad_Skill_5 : ITargetSkillHandler
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
			var farPos = originPos.GetRelative(caster.Direction, 120f);
			farPos = caster.Map.Ground.GetLastValidPosition(originPos, farPos);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 50, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);

			var position = caster.Map.Ground.GetLastValidPosition(originPos, farPos);
			caster.Position = position;
			Send.ZC_SET_POS(caster, position);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rambandgad_red_Skill_1)]
	public class Mon_boss_Rambandgad_red_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_bomb001_orange#Bip001 R Finger2", 0.7f),
				EndEffect = new EffectConfig("I_bomb001_orange", 2f),
				Range = 20f,
				FlyTime = 0.3f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rambandgad_red_Skill_2)]
	public class Mon_boss_Rambandgad_red_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2490));
			var position = GetRelativePosition(PosType.TargetHeight, caster, target);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force019_pink#Bip001 L Finger21", 2f),
				EndEffect = new EffectConfig("F_explosion098_dark_red", 1f),
				Range = 80f,
				FlyTime = 0.2f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rambandgad_red_Skill_3)]
	public class Mon_boss_Rambandgad_red_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 180, width: 20);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rambandgad_red_Skill_4)]
	public class Mon_boss_Rambandgad_red_Skill_4 : ITargetSkillHandler
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
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2400));
			var targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall_red);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall_red);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall_red);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall_red);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall_red);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall_red);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall_red);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall_red);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall_red);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_firewall_red);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Rambandgad_red_Skill_5)]
	public class Mon_boss_Rambandgad_red_Skill_5 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 80, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			MonsterSkillSetCollisionDamage(caster, skill, false, 1f);
		}
	}
}
