using System;
using System.Collections.Generic;
using System.Linq;
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
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_hidden_monster_Ancient_01_Skill_1)]
	public class Mon_hidden_monster_Ancient_01_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2400);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2200;
			var damageDelay = 2400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 40);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 40);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2600;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 40);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 40);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 40);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_hidden_monster_Ancient_01_Skill_2)]
	public class Mon_hidden_monster_Ancient_01_Skill_2 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2200;
			var damageDelay = 2500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2700;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2900;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 3100;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 3200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 3400;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 3600;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 3700;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_silence, 1, 0f, 6000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_hidden_monster_Ancient_02_Skill_1)]
	public class Mon_hidden_monster_Ancient_02_Skill_1 : ITargetSkillHandler
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
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force071_dark5", 6f),
				EndEffect = new EffectConfig("F_explosion132_white_black", 3f),
				DotEffect = EffectConfig.None,
				Range = 70f,
				DelayTime = 0f,
				FlyTime = 0.5f,
				Height = 300f,
				Easing = 1f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0.2f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("F_sys_target_pc", 2f),
				KnockdownPower = 3000f,
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 30);
			await MonsterMissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 30);
			await MonsterMissileFall(caster, skill, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_hidden_monster_Ancient_03_Skill_1)]
	public class Mon_hidden_monster_Ancient_03_Skill_1 : ITargetSkillHandler
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

			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var targetPos = GetRelativePosition(PosType.Target, caster, target, rand: 160);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Ancient_Gimmick_Pad_1);
		}
	}

	[SkillHandler(SkillId.Mon_hidden_monster_moringponia_Skill_1)]
	public class Mon_hidden_monster_moringponia_Skill_1 : ITargetSkillHandler
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

			await skill.Wait(TimeSpan.FromMilliseconds(100));

			await skill.Wait(TimeSpan.FromMilliseconds(1400));

			await skill.Wait(TimeSpan.FromMilliseconds(1400));
		}
	}

	[SkillHandler(SkillId.Mon_hidden_monster_skiaclipse_Skill_1)]
	public class Mon_hidden_monster_skiaclipse_Skill_1 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 750f, 5));
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 30);
			await MonsterMissileFall(caster, skill, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force071_dark5", 6f),
				EndEffect = new EffectConfig("F_explosion132_white_black", 3f),
				DotEffect = EffectConfig.None,
				Range = 70f,
				DelayTime = 0f,
				FlyTime = 0.5f,
				Height = 300f,
				Easing = 1f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0.2f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("F_sys_target_pc", 2f),
				KnockdownPower = 3000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_hidden_monster_skiaclipse_solo_Skill_1)]
	public class Mon_hidden_monster_skiaclipse_solo_Skill_1 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 750f, 5));
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 30);
			await MonsterMissileFall(caster, skill, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force071_dark5", 6f),
				EndEffect = new EffectConfig("F_explosion132_white_black", 3f),
				DotEffect = EffectConfig.None,
				Range = 70f,
				DelayTime = 0f,
				FlyTime = 0.5f,
				Height = 300f,
				Easing = 1f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0.2f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("F_sys_target_pc", 2f),
				KnockdownPower = 3000f,
			});
		}
	}

}
