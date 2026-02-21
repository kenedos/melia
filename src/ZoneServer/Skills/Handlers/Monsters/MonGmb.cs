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
	[SkillHandler(SkillId.Mon_GMB_boss_Moyabruka_Skill_1)]
	public class Mon_GMB_boss_Moyabruka_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1800);
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
			var hitDelays = new[] { 1600, 2500, 1700 };
			var damageDelays = new[] { 1800, 4300, 6000 };

			for (var i = 0; i < hitDelays.Length; i++)
			{
				var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 150);
				var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
				var hits = new List<SkillHitInfo>();
				await SkillAttack(caster, skill, splashArea, hitDelays[i], damageDelays[i], hits);
			}

			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			var position = originPos.GetRelative(farPos, distance: 3.35f, angle: 168f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_smoke014", 1f),
				Range = 60f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			position = originPos.GetRelative(farPos, distance: 3.35f, angle: 168f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_spread_in020_dark##0.5", 2f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_smoke014", 1f),
				Range = 60f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2300));
			position = originPos.GetRelative(farPos, distance: 7.04f, angle: 119f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_spread_in020_dark##0.5", 1.5f),
				PositionDelay = 0,
				Effect = new EffectConfig("F_smoke014", 1f),
				Range = 60f,
				KnockdownPower = 100f,
				Delay = 600f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			// TODO: No Implementation S_R_PULL_TARGET
		}
	}

	[SkillHandler(SkillId.Mon_GMB_boss_Moyabruka_Skill_2)]
	public class Mon_GMB_boss_Moyabruka_Skill_2 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 30));
			await skill.Wait(TimeSpan.FromMilliseconds(1200));

			var effects = new[]
			{
				"I_moyabruka_skl2_mash",
				"I_moyabruka_skl2_mash",
				"I_moyabruka_skl2_mash1",
				"I_moyabruka_skl2_mash2",
				"I_moyabruka_skl2_mash",
				"I_moyabruka_skl2_mash2",
				"I_moyabruka_skl2_mash1",
				"I_moyabruka_skl2_mash1",
				"I_moyabruka_skl2_mash1",
				"I_moyabruka_skl2_mash1",
				"I_moyabruka_skl2_mash1",
				"I_moyabruka_skl2_mash1",
			};

			var posTypes = new[]
			{
				PosType.TargetRandomDistance,
				PosType.TargetRandomDistance,
				PosType.TargetRandomDistance,
				PosType.TargetRandomDistance,
				PosType.TargetRandomDistance,
				PosType.TargetHeight,
				PosType.TargetHeight,
				PosType.TargetHeight,
				PosType.TargetHeight,
				PosType.TargetHeight,
				PosType.TargetHeight,
				PosType.TargetHeight,
			};

			for (var i = 0; i < effects.Length; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(50));

				var position = GetRelativePosition(posTypes[i], caster, target, rand: 180);
				await EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = EffectConfig.None,
					PositionDelay = 0,
					Effect = new EffectConfig(effects[i], 0.8f),
					Range = 35f,
					KnockdownPower = 0f,
					Delay = 0f,
					HitCount = 4,
					HitDuration = 500f,
					CasterEffect = EffectConfig.None,
					CasterNodeName = "None",
					KnockType = 1,
					VerticalAngle = 0f,
					InnerRange = 0f,
				});
			}
		}
	}

	[SkillHandler(SkillId.Mon_GMB_boss_Moyabruka_Skill_3)]
	public class Mon_GMB_boss_Moyabruka_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(900));
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 220f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("None", 1.2f),
				Range = 45f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.15f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_GMB_boss_Moyabruka_Skill_4)]
	public class Mon_GMB_boss_Moyabruka_Skill_4 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 50, angle: 0f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3.5f),
				PositionDelay = 1100,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_GMB_boss_woodspirit_green_Skill_1)]
	public class Mon_GMB_boss_woodspirit_green_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2300));

			var ranges = new[] { 80f, 100f, 120f, 140f, 160f, 180f };
			var innerRanges = new[] { 0f, 70f, 90f, 110f, 130f, 160f };
			var effects = new[]
			{
				new EffectConfig("E_spread_out01_leaf", 1.5f),
				EffectConfig.None,
				EffectConfig.None,
				EffectConfig.None,
				EffectConfig.None,
				EffectConfig.None,
			};

			for (var i = 0; i < ranges.Length; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(100));

				var position = originPos.GetRelative(farPos, distance: 80);
				await EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = EffectConfig.None,
					PositionDelay = 0,
					Effect = effects[i],
					Range = ranges[i],
					KnockdownPower = 130f,
					Delay = 200f,
					HitCount = 1,
					HitDuration = 0f,
					CasterEffect = EffectConfig.None,
					CasterNodeName = "None",
					KnockType = 3,
					VerticalAngle = 30f,
					InnerRange = innerRanges[i],
				});
			}
		}
	}

	[SkillHandler(SkillId.Mon_GMB_boss_woodspirit_green_Skill_2)]
	public class Mon_GMB_boss_woodspirit_green_Skill_2 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 146.21f, angle: -1f);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_apple002_mash@#Bip01 L Finger0", 0.7f),
				EndEffect = new EffectConfig("F_explosion048", 0.5f),
				Range = 25f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 2000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None##0.5", 2f),
			};

			for (var i = 0; i < 7; i++)
			{
				var position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 150, rand: 120);
				await MissileThrow(skill, caster, position, missileConfig);
			}

			for (var i = 0; i < 8; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 150, rand: 110, height: 1);
				await MissileThrow(skill, caster, position, missileConfig);
			}
		}
	}

	[SkillHandler(SkillId.Mon_GMB_boss_woodspirit_green_Skill_3)]
	public class Mon_GMB_boss_woodspirit_green_Skill_3 : ITargetSkillHandler
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
			var ranges = new[] { 150f, 120f, 100f };

			for (var i = 0; i < ranges.Length; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(300));

				var position = originPos.GetRelative(farPos);
				await EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("F_spread_in002_green#Bip01 Spine#1", 1.5f),
					PositionDelay = 1000,
					Effect = new EffectConfig("F_spread_out026_leaf", 1.5f),
					Range = ranges[i],
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
	}

	[SkillHandler(SkillId.Mon_GMB_boss_woodspirit_green_Skill_4)]
	public class Mon_GMB_boss_woodspirit_green_Skill_4 : ITargetSkillHandler
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
			var delays = new[] { 1000, 1200, 1200 };
			var knockdownPowers = new[] { 160f, 160f, 180f };
			var knockTypes = new[] { 3, 3, 4 };
			var verticalAngles = new[] { 60f, 60f, 80f };

			for (var i = 0; i < delays.Length; i++)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
				var position = originPos.GetRelative(farPos, distance: 40, angle: -35f);
				await EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = EffectConfig.None,
					PositionDelay = 36,
					Effect = new EffectConfig("E_spread_out01_leaf", 1.5f),
					Range = 120f,
					KnockdownPower = knockdownPowers[i],
					Delay = 0f,
					HitCount = 1,
					HitDuration = 1000f,
					CasterEffect = EffectConfig.None,
					CasterNodeName = "None",
					KnockType = knockTypes[i],
					VerticalAngle = verticalAngles[i],
					InnerRange = 0f,
				});
			}
		}
	}
}
