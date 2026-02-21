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
	[SkillHandler(SkillId.Mon_ID_Banshee_pink_Skill_1)]
	public class Mon_ID_Banshee_pink_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(800);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 30, angle: 20f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 600;
			var damageDelay = 800;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 3000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_Banshee_pink_Skill_2)]
	public class Mon_ID_Banshee_pink_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(950);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 35);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 750;
			var damageDelay = 950;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			var startingPosition = originPos.GetRelative(farPos, distance: 20f);
			var endingPosition = originPos.GetRelative(farPos, distance: 80f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("None", 0.8f),
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.3f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("I_wizard_refrigerwaves_shot_mash", 0.5f),
				Range = 10f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 15f,
				HitTimeSpacing = 0.08f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Armaox_Skill_1)]
	public class Mon_ID_boss_Armaox_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_circle011_dark#J_05", 1f),
				PositionDelay = 1300,
				Effect = new EffectConfig("F_spread_out026_dark", 2f),
				Range = 100f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1300));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 100f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Armaox_Skill_2)]
	public class Mon_ID_boss_Armaox_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground016_light", 1f),
				Range = 60f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 150f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(650));
			position = originPos.GetRelative(farPos, distance: 122.72015f, angle: -1f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup003", 1.5f),
				Range = 70f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 80f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Armaox_Skill_3)]
	public class Mon_ID_boss_Armaox_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1800,
				Effect = new EffectConfig("F_spread_out023_circle_alpha##0.8", 1f),
				Range = 80f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 2000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 80f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 2000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			for (var i = 0; i < 7; i++)
			{
				position = originPos.GetRelative(farPos);
				await EffectAndHit(skill, caster, position, config);
				await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
			caster.StartBuff(BuffId.Mon_Shield, 1f, 0f, TimeSpan.FromMilliseconds(10000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Armaox_Skill_4)]
	public class Mon_ID_boss_Armaox_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground046_smoke", 1f),
				Range = 60f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 74.568489f, angle: 2f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground046_smoke", 1f),
				Range = 60f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Armaox_Skill_5)]
	public class Mon_ID_boss_Armaox_Skill_5 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2200);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 5, width: 100);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2000;
			var damageDelay = 2200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			var position = originPos.GetRelative(farPos, distance: 50);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_burstup001_violet", 1.2f),
				PositionDelay = 800,
				Effect = new EffectConfig("None", 0.6f),
				Range = 60f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 80f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var startingPosition = originPos.GetRelative(farPos, distance: 80f);
			var endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 0.25f,
				ArrowSpacingTime = 0.25f,
				ArrowLifeTime = 0f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_burstup029_smoke_violet2", 2f),
				Range = 40f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.2f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			// TODO: No Implementation S_R_PULL_TARGET

		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Armaox_Skill_6)]
	public class Mon_ID_boss_Armaox_Skill_6 : ITargetSkillHandler
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
			var targetPos = GetRelativePosition(PosType.TargetDistance, caster, target);
			var targets = SkillSelectEnemiesInCircle(caster, targetPos, 160f, 20);
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force051_dark#Bip001 Head", 1f),
				EndEffect = new EffectConfig("I_explosion002_violet", 1f),
				Range = 15f,
				FlyTime = 0.8f,
				DelayTime = 0f,
				Gravity = 800f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
			position = originPos.GetRelative(farPos, rand: 130);
			await MissileThrow(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Fireload_Skill_1)]
	public class Mon_ID_boss_Fireload_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 45);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Fireload_Skill_2)]
	public class Mon_ID_boss_Fireload_Skill_2 : ITargetSkillHandler
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
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_burstup005_fire##0.5", 1f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos, distance: 30f);
			var endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Fireload_Skill_3)]
	public class Mon_ID_boss_Fireload_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var position = originPos.GetRelative(farPos);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.Mon_Fireball_santan, 0f, 250f, 10, RandomProvider.Get().Next(360, 721), 80f, 100f, 200f);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Fireload_Skill_4)]
	public class Mon_ID_boss_Fireload_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force037_fire", 0.7f),
				EndEffect = new EffectConfig("F_fire022", 2f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			};

			for (var i = 0; i < 11; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 2);
				await MissilePadThrow(skill, caster, position, config, 0f, "boss_firewall_red");
			}
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Fireload_Skill_5)]
	public class Mon_ID_boss_Fireload_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3500));
			var spawnPos = originPos.GetRelative(farPos, distance: 85.565987f, angle: 18f, rand: 20, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "Fire_Dragon_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "DropItemList#None");
			spawnPos = originPos.GetRelative(farPos, distance: 79.838264f, angle: -21f, rand: 20, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "Fire_Dragon_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "DropItemList#None");
			spawnPos = originPos.GetRelative(farPos, distance: 70.989891f, angle: 87f, rand: 20, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "Fire_Dragon_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "DropItemList#None");
			spawnPos = originPos.GetRelative(farPos, distance: 90.717865f, angle: -84f, rand: 20, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "pyran_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "DropItemList#None");
			spawnPos = originPos.GetRelative(farPos, distance: 73.976318f, angle: 138f, rand: 20, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "pyran_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "DropItemList#None");
			spawnPos = originPos.GetRelative(farPos, distance: 63.888355f, angle: -143f, rand: 20, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "pyran_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 0f, "None", "DropItemList#None");
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Fireload_Skill_6)]
	public class Mon_ID_boss_Fireload_Skill_6 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("F_buff_Fire", 1f),
				EndEffect = new EffectConfig("F_buff_fire_spread", 0.3f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			};

			Position position;

			for (var i = 0; i < 8; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 2);
				await MissilePadThrow(skill, caster, position, config, 0f, "boss_firewall_red");
			}
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			position = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Harpeia_orange_Skill_1)]
	public class Mon_ID_boss_Harpeia_orange_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 21.738047f, angle: -4f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_spin011", 1f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Harpeia_orange_Skill_3)]
	public class Mon_ID_boss_Harpeia_orange_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			caster.StartBuff(BuffId.Mon_Shield, 1f, 0f, TimeSpan.FromMilliseconds(10000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Harpeia_orange_Skill_4)]
	public class Mon_ID_boss_Harpeia_orange_Skill_4 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 8f),
				PositionDelay = 3200,
				Effect = new EffectConfig("F_spin012_yellow", 2.5f),
				Range = 60f,
				KnockdownPower = 150f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 20f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 5f),
				PositionDelay = 3200,
				Effect = new EffectConfig("None", 2f),
				Range = 100f,
				KnockdownPower = 120f,
				Delay = 100f,
				HitCount = 2,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 50f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 5f),
				PositionDelay = 3200,
				Effect = new EffectConfig("None", 2f),
				Range = 150f,
				KnockdownPower = 150f,
				Delay = 100f,
				HitCount = 2,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 100f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Harpeia_orange_Skill_5)]
	public class Mon_ID_boss_Harpeia_orange_Skill_5 : ITargetSkillHandler
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
			var config = new ArrowConfig
			{
				ArrowEffect = new EffectConfig("None", 0.8f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.15f,
				PositionDelay = 1500f,
				HitEffect = new EffectConfig("F_warrior_whirlwind_shot_smoke", 1f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			Position startingPosition;
			Position endingPosition;

			for (var i = 0; i < 3; i++)
			{
				startingPosition = originPos.GetRelative(farPos);
				endingPosition = originPos.GetRelative(farPos, distance: 300f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			}
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			startingPosition = originPos.GetRelative(farPos);
			endingPosition = GetRelativePosition(PosType.Target, caster, target, distance: 300);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("None", 0.8f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1500f,
				HitEffect = new EffectConfig("F_warrior_whirlwind_shot_smoke", 2f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Harpeia_orange_Skill_6)]
	public class Mon_ID_boss_Harpeia_orange_Skill_6 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3500));
			var targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_Zaibas);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_Zaibas);
			targetPos = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_Zaibas);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_Zaibas);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_Zaibas);
			targetPos = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Mon_Zaibas);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Kerberos_Skill_1)]
	public class Mon_ID_boss_Kerberos_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1600);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 50, angle: 40f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1400;
			var damageDelay = 1600;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, 20f, 8000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Kerberos_Skill_2)]
	public class Mon_ID_boss_Kerberos_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2300);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2100;
			var damageDelay = 2300;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, 20f, 8000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Kerberos_Skill_3)]
	public class Mon_ID_boss_Kerberos_Skill_3 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 6f),
				PositionDelay = 1300,
				Effect = new EffectConfig("F_ground082_smoke", 1f),
				Range = 80f,
				KnockdownPower = 250f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, 20f, 8000f, 1, 5, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_armorbreak, 1, 0f, 10000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Kerberos_Skill_4)]
	public class Mon_ID_boss_Kerberos_Skill_4 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 120);
			var hits = new List<SkillHitInfo>();
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force039_orange#Bip01 HeadNub", 2f),
				EndEffect = new EffectConfig("F_burstup027_fire", 2f),
				Range = 20f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 50f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 1.6f),
			};

			for (var i = 0; i < 10; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, rand: 80, height: 1);
				await MissileThrow(skill, caster, position, config, hits);
			}
			SkillResultTargetBuff(caster, skill, BuffId.UC_flame, 1, 20f, 8000f, 1, 10, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_fear, 1, 0f, 6000f, 1, 5, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Kerberos_Skill_5)]
	public class Mon_ID_boss_Kerberos_Skill_5 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 120);
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force039_orange#Bip01 HeadNub", 2f),
				EndEffect = new EffectConfig("F_burstup027_fire", 2f),
				Range = 30f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 50f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 2f),
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 110, height: 2);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 120, rand: 110, height: 2);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 120, rand: 110, height: 2);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 120, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 120, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 120, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 120, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 120, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 120, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 120, rand: 120, height: 1);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Kerberos_Skill_6)]
	public class Mon_ID_boss_Kerberos_Skill_6 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(3500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 3300;
			var damageDelay = 3500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			var targetPos = originPos.GetRelative(farPos, distance: 150);
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_wizard_fireball_cast_fire2#Bone Tail04", 1f),
				EndEffect = new EffectConfig("F_explosion89_fire##0.8", 1f),
				Range = 30f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 400f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 2f),
			};

			for (var i = 0; i < 11; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 150, height: 1);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Kerberos_Skill_7)]
	public class Mon_ID_boss_Kerberos_Skill_7 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			targetPos = originPos.GetRelative(farPos, distance: 148.65189f, angle: -167f);
			SkillCreatePad(caster, skill, targetPos, -2.9302113f, PadName.Grinender_FirePillar);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 159.54347, angle: 0f, rand: 10);
			SkillCreatePad(caster, skill, targetPos, -1.7748886f, PadName.Grinender_FirePillar);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			targetPos = GetRelativePosition(PosType.TargetHeight, caster, target);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			await skill.Wait(TimeSpan.FromMilliseconds(900));
			targetPos = originPos.GetRelative(farPos, distance: 103.9225f, angle: 63f);
			SkillCreatePad(caster, skill, targetPos, 1.1071483f, PadName.Grinender_FirePillar);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			targetPos = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 190, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			targetPos = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 180);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Grinender_FirePillar);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			targetPos = originPos.GetRelative(farPos, distance: 103.9225f, angle: 63f);
			SkillCreatePad(caster, skill, targetPos, 1.1071483f, PadName.Grinender_FirePillar);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Kerberos_Skill_8)]
	public class Mon_ID_boss_Kerberos_Skill_8 : ITargetSkillHandler
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
			var spawnPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			MonsterSkillCreateMobPC(skill, caster, "hogma_sorcerer", spawnPos, 0f, "", "BasicMonster_ATK", -1, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			MonsterSkillCreateMobPC(skill, caster, "hogma_warrior", spawnPos, 0f, "", "BasicMonster_ATK", -1, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			MonsterSkillCreateMobPC(skill, caster, "hogma_warrior", spawnPos, 0f, "", "BasicMonster_ATK", -1, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			MonsterSkillCreateMobPC(skill, caster, "hogma_warrior", spawnPos, 0f, "", "BasicMonster_ATK", -1, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			MonsterSkillCreateMobPC(skill, caster, "hogma_warrior", spawnPos, 0f, "", "BasicMonster_ATK", -1, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			MonsterSkillCreateMobPC(skill, caster, "hogma_warrior", spawnPos, 0f, "", "BasicMonster_ATK", -1, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			MonsterSkillCreateMobPC(skill, caster, "hogma_sorcerer", spawnPos, 0f, "", "BasicMonster_ATK", -1, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			MonsterSkillCreateMobPC(skill, caster, "hogma_sorcerer", spawnPos, 0f, "", "BasicMonster_ATK", -1, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			MonsterSkillCreateMobPC(skill, caster, "hogma_sorcerer", spawnPos, 0f, "", "BasicMonster_ATK", -1, 0f, "None", "");
			spawnPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			MonsterSkillCreateMobPC(skill, caster, "hogma_sorcerer", spawnPos, 0f, "", "BasicMonster_ATK", -1, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_kucarry_balzermancer_Skill_1)]
	public class Mon_ID_boss_kucarry_balzermancer_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(700);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 65, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 500;
			var damageDelay = 700;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 100;
			damageDelay = 800;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 50;
			damageDelay = 850;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 50;
			damageDelay = 900;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 50;
			damageDelay = 950;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultKnockTarget(caster, skill, KnockType.Motion, KnockDirection.TowardsTarget, 110, 10, 0, 1, 2, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Boss_kucarry_blazermancer_Debuff, 1, 0f, 0f, 5, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_kucarry_balzermancer_Skill_10)]
	public class Mon_ID_boss_kucarry_balzermancer_Skill_10 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("I_smoke026_spread_out_orange", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("I_smoke025_spread_out", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.5f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(2700));
			var position = originPos.GetRelative(farPos);
			await PadDestruction(skill, caster, position, 20, 90f, PadName.All, "I_pattern008_explosion_mash_square", 1f, 30f, 0f, 0);
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_spread_out054", 6.5f),
				PositionDelay = 0,
				Effect = new EffectConfig("None", 1.2f),
				Range = 90f,
				KnockdownPower = 180f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			caster.StartBuff(BuffId.Mon_raid_Rage, 1f, 0f, TimeSpan.Zero, caster);
			SkillResultTargetBuff(caster, skill, BuffId.Boss_kucarry_blazermancer_Debuff, 1, 0f, 0f, 5, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_kucarry_balzermancer_Skill_11)]
	public class Mon_ID_boss_kucarry_balzermancer_Skill_11 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1400);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 130, width: 30, angle: 20f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1200;
			var damageDelay = 1400;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 130, width: 30, angle: 20f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 500;
			damageDelay = 1900;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 130, width: 30, angle: 20f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 500;
			damageDelay = 2400;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var position = originPos.GetRelative(farPos, distance: 80);
			await PadDestruction(skill, caster, position, 20, 80f, PadName.All, "I_pattern008_explosion_mash_square", 1f, 25f, 0f, 0);
			await skill.Wait(TimeSpan.FromMilliseconds(2400));
			hits = new List<SkillHitInfo>();
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_explosion125", 7f),
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 100f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 80f,
				InnerRange = 0f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Boss_kucarry_blazermancer_Debuff, 1, 0f, 0f, 5, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_kucarry_balzermancer_Skill_2)]
	public class Mon_ID_boss_kucarry_balzermancer_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(650));
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 160f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_explosion087_mint", 0.6f),
				Range = 40f,
				KnockdownPower = 130f,
				Delay = 0f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.15f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Boss_kucarry_blazermancer_Debuff, 1, 0f, 0f, 5, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_kucarry_balzermancer_Skill_3)]
	public class Mon_ID_boss_kucarry_balzermancer_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 65, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 100;
			damageDelay = 2100;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 50;
			damageDelay = 2150;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 50;
			damageDelay = 2200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 50;
			damageDelay = 2250;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Boss_kucarry_blazermancer_Debuff, 1, 0f, 0f, 5, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_kucarry_balzermancer_Skill_4)]
	public class Mon_ID_boss_kucarry_balzermancer_Skill_4 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_Kerberos_Reversi);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			var position = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos);
			SkillResultTargetBuff(caster, skill, BuffId.Boss_kucarry_blazermancer_Debuff, 1, 0f, 0f, 5, 100, -1, hits: hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_kucarry_balzermancer_Skill_5)]
	public class Mon_ID_boss_kucarry_balzermancer_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.5f),
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion125", 7f),
				Range = 100f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 80f,
				InnerRange = 0f,
			}, hits);
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.ID_kucarry_blazermancer_pad);
			SkillResultTargetBuff(caster, skill, BuffId.Boss_kucarry_blazermancer_Debuff, 1, 0f, 0f, 5, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_kucarry_balzermancer_Skill_6)]
	public class Mon_ID_boss_kucarry_balzermancer_Skill_6 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2450));
			var hits = new List<SkillHitInfo>();
			var arrowConfig = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_ground162_light", 2.5f),
				Range = 40f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 220f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			startingPosition = originPos.GetRelative(farPos);
			endingPosition = originPos.GetRelative(farPos, distance: 220f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(550));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("I_buff_002_green", 2.5f),
				Range = 60f,
				KnockdownPower = 180f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 200);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			position = originPos.GetRelative(farPos, distance: 200);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Boss_kucarry_blazermancer_Debuff, 1, 0f, 0f, 5, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_kucarry_balzermancer_Skill_7)]
	public class Mon_ID_boss_kucarry_balzermancer_Skill_7 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos);
			DestroyPad(caster, skill, position, RelationType.Enemy, 150f, 150, "I_pattern008_explosion_mash_square", 15, "F_spread_out032", 0.25f);
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			caster.StartBuff(BuffId.Mon_Shield, 1f, 0f, TimeSpan.Zero, caster);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_kucarry_balzermancer_Skill_8)]
	public class Mon_ID_boss_kucarry_balzermancer_Skill_8 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(3500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 65, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 3300;
			var damageDelay = 3500;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 100;
			damageDelay = 3600;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 50;
			damageDelay = 3650;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 50;
			damageDelay = 3700;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 50;
			damageDelay = 3750;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			var targetPos = originPos.GetRelative(farPos, distance: 80);
			var targets = SkillSelectEnemiesInCircle(caster, targetPos, 90f, 20);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_greeb_energy#Bip001 L Finger32", 1.2f),
				EndEffect = new EffectConfig("F_explosion126", 1.5f),
				Range = 15f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 90);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			hits = new List<SkillHitInfo>();
			var arrowConfig = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_explosion124", 3f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos, distance: 50f);
			var endingPosition = originPos.GetRelative(farPos, distance: 140f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig, hits);
			startingPosition = originPos.GetRelative(farPos, distance: 50f);
			endingPosition = originPos.GetRelative(farPos, distance: 140f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig, hits);
			startingPosition = originPos.GetRelative(farPos, distance: 80f);
			endingPosition = originPos.GetRelative(farPos, distance: 140f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Boss_kucarry_blazermancer_Debuff, 1, 0f, 0f, 5, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_kucarry_balzermancer_Skill_9)]
	public class Mon_ID_boss_kucarry_balzermancer_Skill_9 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			var hits = new List<SkillHitInfo>();
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_explosion124", 3f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 140f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			startingPosition = originPos.GetRelative(farPos, distance: 50f);
			endingPosition = originPos.GetRelative(farPos, distance: 140f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config, hits);
			startingPosition = originPos.GetRelative(farPos, distance: 50f);
			endingPosition = originPos.GetRelative(farPos, distance: 140f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Boss_kucarry_blazermancer_Debuff, 1, 0f, 0f, 5, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_mineloader_Skill_1)]
	public class Mon_ID_boss_mineloader_Skill_1 : ITargetSkillHandler
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
			var startingPosition = originPos.GetRelative(farPos, distance: 25f);
			var endingPosition = originPos.GetRelative(farPos, distance: 140f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 2000f,
				HitEffect = EffectConfig.None,
				Range = 0f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.5f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			var position = originPos.GetRelative(farPos, distance: 170);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2f),
				PositionDelay = 3400,
				Effect = new EffectConfig("F_smoke037", 1f),
				Range = 65f,
				KnockdownPower = 100f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(1700));
			MonsterSkillSetCollisionDamage(caster, skill, false, 1f);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_mineloader_Skill_2)]
	public class Mon_ID_boss_mineloader_Skill_2 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 130);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.mineloader_laser);
			await skill.Wait(TimeSpan.FromMilliseconds(4300));
			SkillRemovePad(caster, skill);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_mineloader_Skill_3)]
	public class Mon_ID_boss_mineloader_Skill_3 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss", 13f),
				PositionDelay = 5000,
				Effect = new EffectConfig("None", 3.5f),
				Range = 180f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 5,
				HitDuration = 1500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 5000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_mineloader_Skill_4)]
	public class Mon_ID_boss_mineloader_Skill_4 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 150);
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force052_pink#Bone003", 1.5f),
				EndEffect = new EffectConfig("F_explosion024_rize", 0.8f),
				Range = 30f,
				FlyTime = 1.5f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.3f),
			};

			for (var i = 0; i < 9; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 150, rand: 80);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_mineloader_Skill_5)]
	public class Mon_ID_boss_mineloader_Skill_5 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 50);
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force052_pink#Bone003", 1.5f),
				EndEffect = new EffectConfig("F_explosion024_rize", 0.8f),
				Range = 30f,
				FlyTime = 1.5f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.3f),
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 150, rand: 80);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 50, rand: 140, height: 1);
			await MissileThrow(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Prisoncutter_Skill_1)]
	public class Mon_ID_boss_Prisoncutter_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(800);
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
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 600;
			var damageDelay = 800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 500;
			damageDelay = 1300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Prisoncutter_Skill_2)]
	public class Mon_ID_boss_Prisoncutter_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var startingPosition = originPos.GetRelative(farPos, distance: 50f);
			var endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("I_explosion002_green", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("I_explosion002_green", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			startingPosition = originPos.GetRelative(farPos, distance: 50f);
			endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			startingPosition = originPos.GetRelative(farPos, distance: 250f);
			endingPosition = originPos.GetRelative(farPos, distance: 40f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 250f);
			endingPosition = originPos.GetRelative(farPos, distance: 45f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Prisoncutter_Skill_3)]
	public class Mon_ID_boss_Prisoncutter_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var startingPosition = originPos.GetRelative(farPos, distance: 20f);
			var endingPosition = originPos.GetRelative(farPos, distance: 230f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = EffectConfig.None,
				Range = 100f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.23f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Prisoncutter_Skill_4)]
	public class Mon_ID_boss_Prisoncutter_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("None", 1.7f),
				Range = 100f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Riteris_Skill_1)]
	public class Mon_ID_boss_Riteris_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1300);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1100;
			var damageDelay = 1300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Riteris_Skill_2)]
	public class Mon_ID_boss_Riteris_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1300));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup003##0.3", 0.5f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 127.93286f, angle: -15f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 144.40485f, angle: -4f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 145.8994f, angle: 2f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 131.81189f, angle: 14f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 100.68957f, angle: 12f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 79.612228f, angle: 1f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 98.598854f, angle: -14f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 120);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup003", 1f),
				Range = 50f,
				KnockdownPower = 100f,
				Delay = 0f,
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

	[SkillHandler(SkillId.Mon_ID_boss_Riteris_Skill_3)]
	public class Mon_ID_boss_Riteris_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup045", 0.7f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 38.241978f, angle: -96f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 77.199768f, angle: 55f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 58.235065f, angle: 23f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 80.819176f, angle: -133f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 49.115044f, angle: 115f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 54.272079f, angle: -180f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 72.766518f, angle: -123f);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_Riteris_Skill_4)]
	public class Mon_ID_boss_Riteris_Skill_4 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 60, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_werewolf_Skill_1)]
	public class Mon_ID_boss_werewolf_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 45f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1300;
			var damageDelay = 1500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 45f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 300;
			damageDelay = 1800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_werewolf_Skill_2)]
	public class Mon_ID_boss_werewolf_Skill_2 : ITargetSkillHandler
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
			var startingPosition = originPos.GetRelative(farPos, distance: 25f);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.05f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_burstup008_smoke1", 1f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			startingPosition = originPos.GetRelative(farPos, distance: 25f);
			endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_burstup008_smoke1", 1f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_werewolf_Skill_3)]
	public class Mon_ID_boss_werewolf_Skill_3 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 170, width: 30, angle: 20f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 170, width: 30, angle: 20f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 250;
			damageDelay = 2200;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 170, width: 30, angle: 20f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 250;
			damageDelay = 2500;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 170, width: 30, angle: 20f);
			splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			hitDelay = 250;
			damageDelay = 2750;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_deprotect, 1, 0f, 5000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_werewolf_Skill_4)]
	public class Mon_ID_boss_werewolf_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			MonsterSkillSetCollisionDamage(caster, skill, false, 1f);
		}
	}

	[SkillHandler(SkillId.Mon_ID_boss_werewolf_Skill_5)]
	public class Mon_ID_boss_werewolf_Skill_5 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 30, angle: 20f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(2600));
			var spawnPos = originPos.GetRelative(farPos, distance: 104.20988f, angle: -85f, rand: 40);
			MonsterSkillCreateMob(skill, caster, "Worg", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 80.699883f, angle: -24f, rand: 40);
			MonsterSkillCreateMob(skill, caster, "Worg", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 83.245842f, angle: 36f, rand: 40);
			MonsterSkillCreateMob(skill, caster, "Worg", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 83.490692f, angle: 210f, rand: 40);
			MonsterSkillCreateMob(skill, caster, "Worg", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 88.7258f, angle: 139f, rand: 40);
			MonsterSkillCreateMob(skill, caster, "Worg", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 91.750519f, angle: 89f, rand: 30);
			MonsterSkillCreateMob(skill, caster, "Worg", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 99.130264f, angle: 1f, rand: 30);
			MonsterSkillCreateMob(skill, caster, "Worg", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 116.17501f, angle: -165f, rand: 30);
			MonsterSkillCreateMob(skill, caster, "Worg", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, distance: 131.10445f, angle: 143f, rand: 30);
			MonsterSkillCreateMob(skill, caster, "Worg", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_lioni_Skill_1)]
	public class Mon_ID_kucarry_lioni_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(800);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 40, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 600;
			var damageDelay = 800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_lioni_Skill_2)]
	public class Mon_ID_kucarry_lioni_Skill_2 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 40, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 900;
			var damageDelay = 1100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_Somy_Skill_1)]
	public class Mon_ID_kucarry_Somy_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(900);
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

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = TimeSpan.Zero;
			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);
			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);
			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 10, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 700;
			var damageDelay = 900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_Somy_Skill_2)]
	public class Mon_ID_kucarry_Somy_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1000);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 10, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_symbani_Skill_1)]
	public class Mon_ID_kucarry_symbani_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(950);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 40, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 750;
			var damageDelay = 950;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_bleed, 1, hits.Sum(h => h.HitInfo.Damage) * 0.3f, 4000f, 1, 2, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_symbani_Skill_2)]
	public class Mon_ID_kucarry_symbani_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos, distance: 10f);
			var endingPosition = originPos.GetRelative(farPos, distance: 75f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 1f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = EffectConfig.None,
				Range = 15f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitEffectSpacing = 15f,
				HitTimeSpacing = 0.04f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_hemorrhage, 1, 0f, 6000f, 1, 1, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_Tot_Skill_1)]
	public class Mon_ID_kucarry_Tot_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(400);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 40, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 200;
			var damageDelay = 400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_Tot_Skill_2)]
	public class Mon_ID_kucarry_Tot_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(800);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 40, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 600;
			var damageDelay = 800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_zabbi_Skill_1)]
	public class Mon_ID_kucarry_zabbi_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(800);
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

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = TimeSpan.Zero;
			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);
			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);
			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 10);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 600;
			var damageDelay = 800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_zabbi_Skill_2)]
	public class Mon_ID_kucarry_zabbi_Skill_2 : ITargetSkillHandler
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
			var startingPosition = originPos.GetRelative(farPos, distance: 13f);
			var endingPosition = originPos.GetRelative(farPos, distance: 86f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = EffectConfig.None,
				Range = 10f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 20f,
				HitTimeSpacing = 0.02f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			startingPosition = originPos.GetRelative(farPos, distance: 86f);
			endingPosition = originPos.GetRelative(farPos, distance: 12f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = EffectConfig.None,
				Range = 10f,
				KnockdownPower = 150f,
				Delay = 10f,
				HitEffectSpacing = 20f,
				HitTimeSpacing = 0.02f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_zeuni_Skill_1)]
	public class Mon_ID_kucarry_zeuni_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(500);
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

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = TimeSpan.Zero;
			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);
			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);
			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 10);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 300;
			var damageDelay = 500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_kucarry_zeuni_Skill_2)]
	public class Mon_ID_kucarry_zeuni_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 95f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = EffectConfig.None,
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 23f,
				HitTimeSpacing = 0.13f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_Malstatue_Skill_1)]
	public class Mon_ID_Malstatue_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(500);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 10, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 300;
			var damageDelay = 500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_umblet_Skill_1)]
	public class Mon_ID_umblet_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(900);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 35, width: 10);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 700;
			var damageDelay = 900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_umblet_Skill_2)]
	public class Mon_ID_umblet_Skill_2 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 30);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_hit003_light_dark_blue", 1f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			position = originPos.GetRelative(farPos, distance: 25);
		}
	}

	[SkillHandler(SkillId.Mon_ID_umbra_mage_Skill_1)]
	public class Mon_ID_umbra_mage_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(900);
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

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var damageDelay = TimeSpan.Zero;
			var skillHitDelay = TimeSpan.Zero;
			var skillHitResult = SCR_SkillHit(caster, target, skill);
			target.TakeDamage(skillHitResult.Damage, caster);
			var skillHit = new SkillHitInfo(caster, target, skill, skillHitResult, damageDelay, skillHitDelay);
			skillHit.ForceId = ForceId.GetNew();
			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);
			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 10);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 700;
			var damageDelay = 900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_umbra_mage_Skill_2)]
	public class Mon_ID_umbra_mage_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var position = originPos.GetRelative(farPos, distance: 18);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_violet", 0.6f),
				Range = 45f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_ID_umbra_warrior_Skill_1)]
	public class Mon_ID_umbra_warrior_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 400;
			var damageDelay = 600;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_ID_umbra_warrior_Skill_2)]
	public class Mon_ID_umbra_warrior_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2300);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 2100;
			var damageDelay = 2300;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_deprotect, 1, 0f, 6000f, 1, 2, -1, hits);
		}
	}

}
