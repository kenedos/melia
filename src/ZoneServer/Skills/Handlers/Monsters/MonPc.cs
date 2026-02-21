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
using Yggdrasil.Extensions;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_pc_summon_Abomination_Skill_1)]
	public class Mon_pc_summon_Abomination_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 45, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 700;
			var damageDelay = 900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Abomination_Skill_2)]
	public class Mon_pc_summon_Abomination_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground104_smoke", 0.3f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(1700));
			position = originPos.GetRelative(farPos, distance: 55f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground104_smoke", 0.5f),
				Range = 50f,
				KnockdownPower = 0f,
				Delay = 0f,
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

	[SkillHandler(SkillId.Mon_pc_summon_Abomination_Skill_3)]
	public class Mon_pc_summon_Abomination_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(3200));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground042_light", 1f),
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
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 55f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground046_smoke", 1f),
				Range = 35f,
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
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_archon_Skill_1)]
	public class Mon_pc_summon_archon_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 85, width: 30, angle: 45f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1200;
			var damageDelay = 1400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_archon_Skill_2)]
	public class Mon_pc_summon_archon_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var position = originPos.GetRelative(farPos, distance: 70f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3.5f),
				PositionDelay = 2600,
				Effect = new EffectConfig("F_burstup013", 2f),
				Range = 40f,
				KnockdownPower = 300f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 75f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_archon_Skill_3)]
	public class Mon_pc_summon_archon_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			var position = originPos.GetRelative(farPos, distance: 59.332268f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 50.974472f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 1300,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 150f,
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

	[SkillHandler(SkillId.Mon_pc_summon_archon_Skill_4)]
	public class Mon_pc_summon_archon_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var position = originPos.GetRelative(farPos, distance: 70f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_burstup013", 2f),
				Range = 40f,
				KnockdownPower = 300f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 75f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			position = originPos.GetRelative(farPos, distance: 70f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_burstup013", 3.5f),
				Range = 60f,
				KnockdownPower = 300f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 75f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2900));
			position = originPos.GetRelative(farPos, distance: 70f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3.5f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_burstup013", 4f),
				Range = 80f,
				KnockdownPower = 300f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 75f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Avataras_Skill_1)]
	public class Mon_pc_summon_boss_Avataras_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 500;
			var damageDelay = 700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 500;
			damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Avataras_Skill_2)]
	public class Mon_pc_summon_boss_Avataras_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 800f,
				HitEffect = new EffectConfig("F_cleric_HolySmash_explosion_violet", 0.5f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 20f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos, distance: 30f);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			startingPosition = originPos.GetRelative(farPos, distance: 60f);
			endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 60f);
			endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 100f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 100f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Avataras_Skill_3)]
	public class Mon_pc_summon_boss_Avataras_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 10));
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_violet_red#Dummy_cristal07", 2f),
				EndEffect = new EffectConfig("I_explosion002_violet2_1", 1.5f),
				Range = 30f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 150f,
				Speed = 1f,
				HitTime = 0f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			for (var i = 0; i < 15; i++)
			{
				var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 80, height: 1);
				await MissileThrow(skill, caster, position, config);

				if (i < 14)
					await skill.Wait(TimeSpan.FromMilliseconds(50));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Avataras_Skill_4)]
	public class Mon_pc_summon_boss_Avataras_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 220f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 500f,
				HitEffect = new EffectConfig("F_cleric_HolySmash_explosion_violet", 2f),
				Range = 60f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 45f,
				HitTimeSpacing = 0.08f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 500f,
				HitEffect = new EffectConfig("F_smoke135_dark", 1f),
				Range = 35f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.08f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 3; i++)
			{
				startingPosition = originPos.GetRelative(farPos);
				endingPosition = originPos.GetRelative(farPos, distance: 180f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);

				if (i < 2)
					await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Basilisk_Skill_1)]
	public class Mon_pc_summon_boss_Basilisk_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 120f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(350));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force046_green2", 1f),
				EndEffect = new EffectConfig("I_explosion002_green", 2f),
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 1000f,
				Speed = 1f,
				HitTime = 500f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			for (var i = 0; i < 9; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 60, height: 2);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_BiteRegina_Skill_1)]
	public class Mon_pc_summon_boss_BiteRegina_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 30));
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force011_green", 1f),
				EndEffect = new EffectConfig("I_explosion013_green", 1.2f),
				Range = 30f,
				FlyTime = 1.2f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 2f),
			};

			for (var i = 0; i < 8; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100);
				await MissileThrow(skill, caster, position, config);

				if (i < 7)
					await skill.Wait(TimeSpan.FromMilliseconds(50));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Blud_Skill_1)]
	public class Mon_pc_summon_boss_Blud_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 50, angle: 60f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Blud_Skill_2)]
	public class Mon_pc_summon_boss_Blud_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 40, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(2300));
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_burstup003_1", 0.8f),
				Range = 30f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitEffectSpacing = 28f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 3; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 40f);
				var endingPosition = originPos.GetRelative(farPos, distance: 200f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Blud_Skill_3)]
	public class Mon_pc_summon_boss_Blud_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force039_orange#Bip001 Ponytail2", 1f),
				EndEffect = new EffectConfig("I_explosion009_orange", 1f),
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

			var position = originPos.GetRelative(farPos, distance: 160.99664f);
			await MissilePadThrow(skill, caster, position, config, 0f, "Mon_PoisonPilla_orange");
			position = GetRelativePosition(PosType.TargetRandom, caster, target, distance: 155.05614, angle: 0f, rand: 130, height: 2);
			await MissilePadThrow(skill, caster, position, config, 0f, "Mon_PoisonPilla_orange");
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 117.14211, angle: 100f, rand: 130, height: 2);
			await MissilePadThrow(skill, caster, position, config, 0f, "Mon_PoisonPilla_orange");
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 100.28521, angle: 0f, rand: 130, height: 2);
			await MissilePadThrow(skill, caster, position, config, 0f, "Mon_PoisonPilla_orange");
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Carapace_Skill_1)]
	public class Mon_pc_summon_boss_Carapace_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 100f, 50));
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var position = GetRelativePosition(PosType.TargetDistance, caster, target);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 0,
				Effect = new EffectConfig("I_force038_ice", 1.5f),
				Range = 35f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 4,
				HitDuration = 800f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_castle_princess_Skill_1)]
	public class Mon_pc_summon_boss_castle_princess_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion004_yellow", 0.3f),
				Range = 60f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 3,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_castle_princess_Skill_2)]
	public class Mon_pc_summon_boss_castle_princess_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos, distance: 80f);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 80f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke034_white#Bone011", 1f),
				EndEffect = new EffectConfig("F_explosion001_white", 1f),
				Range = 20f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetDirection, caster, target, distance: 70, rand: 30, height: 1);
			await MissileThrow(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_castle_princess_Skill_3)]
	public class Mon_pc_summon_boss_castle_princess_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var startingPosition = originPos.GetRelative(farPos, distance: 20f);
			var endingPosition = originPos.GetRelative(farPos, distance: 140f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_light029_yellow", 1f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 20f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_lineup010_ground", 2f),
				Range = 100f,
				KnockdownPower = 250f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_light029_yellow", 1f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 20f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 7; i++)
			{
				startingPosition = originPos.GetRelative(farPos, distance: 20f);
				endingPosition = originPos.GetRelative(farPos, distance: 140f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Centaurus_Skill_1)]
	public class Mon_pc_summon_boss_Centaurus_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1600;
			var damageDelay = 1800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Centaurus_Skill_2)]
	public class Mon_pc_summon_boss_Centaurus_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var targetPos = originPos.GetRelative(farPos, distance: 20f);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 120f, 50));
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_white#Point_W_position02", 0.8f),
				EndEffect = new EffectConfig("I_explosion008_violet", 1f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 20, rand: 90, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(900));
			position = originPos.GetRelative(farPos, distance: 120f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("I_explosion008_violet", 1.5f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 200f,
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

	[SkillHandler(SkillId.Mon_pc_summon_boss_Centaurus_Skill_3)]
	public class Mon_pc_summon_boss_Centaurus_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 40f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground108_smoke", 2f),
				Range = 60f,
				KnockdownPower = 100f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 5000f, 1, 15, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_chafer_Skill_1)]
	public class Mon_pc_summon_boss_chafer_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(350));
			var startingPosition = originPos.GetRelative(farPos, distance: 35f);
			var endingPosition = originPos.GetRelative(farPos, distance: 150f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_arrow_pc", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.02f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 800f,
				HitEffect = new EffectConfig("F_burstup008_smoke2", 1.3f),
				Range = 30f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.01f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Fallen_Statue_Skill_1)]
	public class Mon_pc_summon_boss_Fallen_Statue_Skill_1 : ITargetSkillHandler
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

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);
			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var startingPosition = originPos.GetRelative(farPos, distance: 30f);
			var endingPosition = originPos.GetRelative(farPos, distance: 150f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("I_fallen_statue_atk001_mash", 0.25f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 38f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Fallen_Statue_Skill_2)]
	public class Mon_pc_summon_boss_Fallen_Statue_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(900));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_smoke119##0.5", 1f),
				PositionDelay = 600,
				Effect = new EffectConfig("I_fallen_statue_atk001_mash", 0.35f),
				Range = 30f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			for (var i = 0; i < 4; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 130, height: 1);
				await EffectAndHit(skill, caster, position, config);

				if (i < 3)
					await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Fallen_Statue_Skill_3)]
	public class Mon_pc_summon_boss_Fallen_Statue_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 10f),
				PositionDelay = 2000,
				Effect = EffectConfig.None,
				Range = 120f,
				KnockdownPower = 150f,
				Delay = 0f,
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

	[SkillHandler(SkillId.Mon_pc_summon_boss_froster_lord_Skill_1)]
	public class Mon_pc_summon_boss_froster_lord_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_explosion080_ice", 0.4f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 3; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 20f);
				var endingPosition = originPos.GetRelative(farPos, distance: 120f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_froster_lord_Skill_2)]
	public class Mon_pc_summon_boss_froster_lord_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var missileConfig = new MissileConfig
			{
				Effect = EffectConfig.None,
				EndEffect = new EffectConfig("I_ice005_mash", 0.8f),
				Range = 20f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 60, rand: 50);
			await MissileThrow(skill, caster, position, missileConfig);
			position = originPos.GetRelative(farPos, distance: 60f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground114_ice", 0.6f),
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground156_ice##1.5", 1f),
				Range = 60f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 3,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			var missileConfig2 = new MissileConfig
			{
				Effect = EffectConfig.None,
				EndEffect = new EffectConfig("I_ice005_mash2", 0.8f),
				Range = 20f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 60, rand: 50);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 60, rand: 50);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 60, rand: 50);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 60, rand: 50);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 60, rand: 50);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = originPos.GetRelative(farPos, distance: 60f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_spread_out040_ice", 1.5f),
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground084_ice", 5f),
				Range = 60f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 3,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 60, rand: 50);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 60, rand: 50);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 60f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground114_ice", 0.6f),
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground084_ice", 5f),
				Range = 60f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 3,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_froster_lord_Skill_3)]
	public class Mon_pc_summon_boss_froster_lord_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1700));
			var missileConfig = new MissileConfig
			{
				Effect = EffectConfig.None,
				EndEffect = new EffectConfig("I_ice005_mash", 0.8f),
				Range = 20f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			Position position;

			for (var i = 0; i < 7; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
				await MissileThrow(skill, caster, position, missileConfig);

				if (i < 6)
					await skill.Wait(TimeSpan.FromMilliseconds(80));
			}
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			var hits = new List<SkillHitInfo>();
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(60));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			var missileConfig2 = new MissileConfig
			{
				Effect = EffectConfig.None,
				EndEffect = new EffectConfig("I_ice005_mash2", 0.8f),
				Range = 20f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(60));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(40));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(60));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(60));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("I_ground012_ice", 5f),
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 5,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			}, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(60));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(60));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(60));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(80));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 70, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 5000f, 1, 4, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Genmagnus_Skill_1)]
	public class Mon_pc_summon_boss_Genmagnus_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1300));
			var startingPosition = originPos.GetRelative(farPos, distance: 52.943882f);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("I_explosion002_green", 1f),
				Range = 40f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitEffectSpacing = 38f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Genmagnus_Skill_2)]
	public class Mon_pc_summon_boss_Genmagnus_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 80, angle: 100f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_freeze, 1, 0f, 4000f, 1, 20, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Genmagnus_Skill_3)]
	public class Mon_pc_summon_boss_Genmagnus_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var position = originPos.GetRelative(farPos, distance: 30f);
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_lineup017_red1", 6f),
				Range = 100f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 60f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_lineup017_red1", 8.5f),
				Range = 130f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 100f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_lineup017_red1", 10f),
				Range = 160f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 130f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Giltine_Skill_1)]
	public class Mon_pc_summon_boss_Giltine_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(750);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 50f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 550;
			var damageDelay = 750;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Giltine_Skill_2)]
	public class Mon_pc_summon_boss_Giltine_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.3f,
				PositionDelay = 500f,
				HitEffect = new EffectConfig("F_explosion101_dark", 1.5f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos, distance: 15f);
			var endingPosition = originPos.GetRelative(farPos, distance: 150f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 15f);
			endingPosition = originPos.GetRelative(farPos, distance: 150f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos);
			endingPosition = originPos.GetRelative(farPos, distance: 150f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			startingPosition = originPos.GetRelative(farPos, distance: 180f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 180f);
			endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 150f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Giltine_Skill_3)]
	public class Mon_pc_summon_boss_Giltine_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 10));
			await skill.Wait(TimeSpan.FromMilliseconds(850));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke001_dark_loop#weapon_Point", 1f),
				EndEffect = new EffectConfig("F_burstup006_dark", 1f),
				Range = 45f,
				FlyTime = 1.5f,
				DelayTime = 0f,
				Gravity = 350f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			Position position;

			for (var i = 0; i < 4; i++)
			{
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 80, height: 1);
				await MissileThrow(skill, caster, position, missileConfig);

				if (i < 3)
					await skill.Wait(TimeSpan.FromMilliseconds(50));
			}
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 500,
				Effect = new EffectConfig("F_rize015_1_drop5_2", 1f),
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

			for (var i = 0; i < 8; i++)
			{
				position = GetRelativePosition(PosType.TargetHeight, caster, target);
				await EffectAndHit(skill, caster, position, effectHitConfig);

				if (i < 7)
					await skill.Wait(TimeSpan.FromMilliseconds(150));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Giltine_Skill_4)]
	public class Mon_pc_summon_boss_Giltine_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 160f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 200f,
				HitEffect = new EffectConfig("F_explosion101_dark", 2f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.01f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Gorgon_Skill_1)]
	public class Mon_pc_summon_boss_Gorgon_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 70, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Gorgon_Skill_2)]
	public class Mon_pc_summon_boss_Gorgon_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 30, angle: 20f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 30, angle: 20f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2600;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 30, angle: 20f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 30, angle: 20f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Gorgon_Skill_3)]
	public class Mon_pc_summon_boss_Gorgon_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var startingPosition = originPos.GetRelative(farPos, distance: 30f);
			var endingPosition = originPos.GetRelative(farPos, distance: 210f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.1f,
				ArrowLifeTime = 0.3f,
				PositionDelay = 2500f,
				HitEffect = new EffectConfig("F_ground122_dark##1", 1f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 18f,
				HitTimeSpacing = 0.01f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_helgasercle_Skill_1)]
	public class Mon_pc_summon_boss_helgasercle_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 60, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1300;
			var damageDelay = 1500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_helgasercle_Skill_2)]
	public class Mon_pc_summon_boss_helgasercle_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red#Bip001 R Hand", 1f),
				EndEffect = new EffectConfig("F_explosion046_red", 1f),
				Range = 25f,
				FlyTime = 0.3f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 150);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 150);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red#Bip001 L Hand", 1f),
				EndEffect = new EffectConfig("F_explosion046_red", 1f),
				Range = 25f,
				FlyTime = 0.3f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			};

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150, height: 3);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150, height: 3);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 150, height: 3);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 150, height: 3);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150, height: 3);
			await MissileThrow(skill, caster, position, missileConfig2);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_helgasercle_Skill_3)]
	public class Mon_pc_summon_boss_helgasercle_Skill_3 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 120, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2100;
			var damageDelay = 2300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 120, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 120, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 120, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 120, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 3100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var position = originPos.GetRelative(farPos);
			await PadDestruction(skill, caster, position, 2, 200f, "ALL", "E_archer_DetonateTraps_explosion", 1f, 0f, 0f, 7);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_honeypin_Skill_1)]
	public class Mon_pc_summon_boss_honeypin_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_blood001_green", 1.5f),
				EndEffect = new EffectConfig("F_explosion062_blood", 0.4f),
				Range = 12f,
				FlyTime = 2f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			for (var i = 0; i < 10; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 60, height: 1);
				await MissileThrow(skill, caster, position, config);

				if (i < 9)
					await skill.Wait(TimeSpan.FromMilliseconds(150));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Ignas_Skill_1)]
	public class Mon_pc_summon_boss_Ignas_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 250, width: 25);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1200;
			var damageDelay = 1400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Ignas_Skill_2)]
	public class Mon_pc_summon_boss_Ignas_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_ignas_ArrowRain_trail", 7f),
				EndEffect = new EffectConfig("I_ignas_ArrowRain_hit2_mash", 2.2f),
				DotEffect = EffectConfig.None,
				Range = 15f,
				DelayTime = 0f,
				FlyTime = 0.4f,
				Height = 300f,
				Easing = 1f,
				HitTime = 500f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
				KnockdownPower = 130f,
				KnockType = (KnockType)3,
				VerticalAngle = 30f,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(80));
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_ignas_ArrowRain_trail", 7f),
				EndEffect = new EffectConfig("I_ignas_ArrowRain_hit_mash", 2.2f),
				DotEffect = EffectConfig.None,
				Range = 15f,
				DelayTime = 0f,
				FlyTime = 0.4f,
				Height = 300f,
				Easing = 1f,
				HitTime = 500f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
				KnockdownPower = 130f,
				KnockType = (KnockType)3,
				VerticalAngle = 30f,
			};

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(80));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(80));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(80));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(80));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(80));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(80));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(80));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(80));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(80));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, missileConfig2);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Ignas_Skill_3)]
	public class Mon_pc_summon_boss_Ignas_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			var arrowConfig = new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_burstup029_smoke_dark2", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_fire003_violet2", 0.5f),
				Range = 22f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos, distance: 65f);
			var endingPosition = originPos.GetRelative(farPos, distance: 210f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig);
			startingPosition = originPos.GetRelative(farPos, distance: 85f);
			endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig);
			startingPosition = originPos.GetRelative(farPos, distance: 85f);
			endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig);
			startingPosition = originPos.GetRelative(farPos, distance: 60f);
			endingPosition = originPos.GetRelative(farPos, distance: 130f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig);
			startingPosition = originPos.GetRelative(farPos, distance: 60f);
			endingPosition = originPos.GetRelative(farPos, distance: 130f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(290));
			var arrowConfig2 = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_fire003_violet2", 0.5f),
				Range = 22f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.08f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			startingPosition = originPos.GetRelative(farPos);
			endingPosition = originPos.GetRelative(farPos, distance: 155f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig2);
			startingPosition = originPos.GetRelative(farPos, distance: 25f);
			endingPosition = originPos.GetRelative(farPos, distance: 165f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig2);
			startingPosition = originPos.GetRelative(farPos, distance: 25f);
			endingPosition = originPos.GetRelative(farPos, distance: 165f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(410));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_fire003_violet2", 0.8f),
				Range = 40f,
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
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_spread_out026_violet", 1f),
				Range = 70f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 40f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_spread_out026_violet", 1.5f),
				Range = 100f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 70f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Kimeleech_Skill_1)]
	public class Mon_pc_summon_boss_Kimeleech_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("I_kirmeleech001_mash", 0.8f),
				Range = 15f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 15f,
				HitTimeSpacing = 0.07f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 3; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 40f);
				var endingPosition = originPos.GetRelative(farPos, distance: 120f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_lecifer_Skill_1)]
	public class Mon_pc_summon_boss_lecifer_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1125);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 925;
			var damageDelay = 1125;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 40);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 225;
			damageDelay = 1350;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_lecifer_Skill_2)]
	public class Mon_pc_summon_boss_lecifer_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 100f, 30));
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var position = originPos.GetRelative(farPos, distance: 10f);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force071_mintdark#Bip01 R Finger0", 3f),
				EndEffect = new EffectConfig("F_burstup004_dark", 3f),
				Range = 60f,
				FlyTime = 0.3f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 5.5f),
			});
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var startingPosition = originPos.GetRelative(farPos, distance: 10f);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("None", 0.8f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_rize009", 1f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 0,
				HitDuration = 1000f,
			});
			startingPosition = originPos.GetRelative(farPos, distance: 10f);
			endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("None", 0.8f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_smoke131_dark_green", 1f),
				Range = 30f,
				KnockdownPower = 200f,
				Delay = 200f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_lecifer_Skill_3)]
	public class Mon_pc_summon_boss_lecifer_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 65f,
				KnockdownPower = 230f,
				Delay = 0f,
				HitCount = 5,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_lineup017_mintdark", 5f),
				Range = 100f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 70f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_lineup017_mintdark", 8f),
				Range = 130f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 100f,
			});
			caster.StartBuff(BuffId.Rage_Rockto_atk, 1f, 0f, TimeSpan.FromMilliseconds(20000f), caster);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_lineup017_mintdark", 10f),
				Range = 155f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 130f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Marionette_Skill_1)]
	public class Mon_pc_summon_boss_Marionette_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			var position = originPos.GetRelative(farPos, distance: 60f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_violet", 1f),
				Range = 40f,
				KnockdownPower = 250f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 60f);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_dark", 1f),
				Range = 30f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			for (var i = 0; i < 3; i++)
			{
				position = originPos.GetRelative(farPos, distance: 210f);
				await EffectAndHit(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Marionette_Skill_2)]
	public class Mon_pc_summon_boss_Marionette_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup002_dark", 3f),
				Range = 70f,
				KnockdownPower = 250f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 20f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_violet", 1.5f),
				Range = 90f,
				KnockdownPower = 250f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 50f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Marionette_Skill_3)]
	public class Mon_pc_summon_boss_Marionette_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(4400));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground083_smoke", 0.8f),
				Range = 75f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_dark2_loop#Bip001 R Hand#2", 5f),
				EndEffect = new EffectConfig("F_burstup004_dark##1", 1f),
				Range = 30f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 240, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 240, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 270, height: 2);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 270, height: 2);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 240, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 240, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 270, height: 2);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 270, height: 2);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 240, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 240, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 270, height: 2);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 270, height: 2);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 240, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 240, height: 1);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 270, height: 2);
			await MissileThrow(skill, caster, position, missileConfig);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Marnoks_Skill_1)]
	public class Mon_pc_summon_boss_Marnoks_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 60f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 60f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 1400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Marnoks_Skill_2)]
	public class Mon_pc_summon_boss_Marnoks_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 280f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("none", 1f),
				Range = 50f,
				KnockdownPower = 150f,
				Delay = 1000f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.12f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Marnoks_Skill_3)]
	public class Mon_pc_summon_boss_Marnoks_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_explosion026_rize_red2", 0.4f),
				Range = 30f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 7; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 55f);
				var endingPosition = originPos.GetRelative(farPos, distance: 250f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Merge_Skill_1)]
	public class Mon_pc_summon_boss_Merge_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 120f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("I_ground003_blue##0.5", 1f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion059_ground_blue", 0.5f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			for (var i = 0; i < 8; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 80, height: 1);
				await EffectAndHit(skill, caster, position, config);

				if (i < 7)
					await skill.Wait(TimeSpan.FromMilliseconds(80));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_merregina_despair_Skill_1)]
	public class Mon_pc_summon_boss_merregina_despair_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			var position = originPos.GetRelative(farPos, distance: 176.81964f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 0.8f),
				PositionDelay = 1000,
				Effect = new EffectConfig("GroundImpact_WaterPillar_Cyan_02", 0.6f),
				Range = 80f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(400));
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_merregina_despair_Skill_2)]
	public class Mon_pc_summon_boss_merregina_despair_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos, distance: 95f);
			var endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 30f,
				ArrowSpacingTime = 30f,
				ArrowLifeTime = 1500f,
				PositionDelay = 101f,
				HitEffect = new EffectConfig("GroundImpact_ElectricCrush_Cyan_01", 0.4f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			startingPosition = originPos.GetRelative(farPos, distance: 95f);
			endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 30f,
				ArrowSpacingTime = 30f,
				ArrowLifeTime = 1200f,
				PositionDelay = 101f,
				HitEffect = new EffectConfig("GroundImpact_ElectricCrush_Cyan_01", 0.4f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			startingPosition = originPos.GetRelative(farPos, distance: 40f);
			endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 40f,
				ArrowSpacingTime = 40f,
				ArrowLifeTime = 500f,
				PositionDelay = 101f,
				HitEffect = new EffectConfig("GroundImpact_ElectricCrush_Cyan_01", 0.4f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);

			foreach (var hit in hits)
				SkillResultKnockTarget(caster, null, skill, hit, KnockType.KnockDown, KnockDirection.TowardsTarget, 200, 40, 0, 2, 2);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_merregina_despair_Skill_3)]
	public class Mon_pc_summon_boss_merregina_despair_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 32.894077f);
			await EffectAndHitRangePreview(skill, caster, position, new EffectHitConfig
			{
				PositionDelay = 1000,
				GroundEffect = new EffectConfig("GroundImpact_ElectricCrush_Purple_01", 0.6f),
				Range = 101f,
				KnockdownPower = 60f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			position = originPos.GetRelative(farPos, distance: 32.894077f);
			await EffectAndHitRangePreview(skill, caster, position, new EffectHitConfig
			{
				PositionDelay = 1000,
				GroundEffect = new EffectConfig("GroundImpact_WaterPillar_Purple_02", 0.8f),
				Range = 101f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 30f,
				ArrowSpacingTime = 30f,
				ArrowLifeTime = 500f,
				PositionDelay = 101f,
				HitEffect = new EffectConfig("GroundImpact_LavaCrack_Purple_01", 0.4f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 0f,
			};

			for (var i = 0; i < 4; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 200f);
				var endingPosition = originPos.GetRelative(farPos, distance: 200f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config, hits);
			}

			foreach (var hit in hits)
				SkillResultKnockTarget(caster, null, skill, hit, KnockType.Motion, KnockDirection.TowardsTarget, 230, 40, 0, 1, 3);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_merregina_Skill_1)]
	public class Mon_pc_summon_boss_merregina_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var position = originPos.GetRelative(farPos, distance: 50.299419f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 5f),
				PositionDelay = 1800,
				Effect = new EffectConfig("F_ground127_water", 1f),
				Range = 70f,
				KnockdownPower = 0f,
				Delay = 0f,
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

	[SkillHandler(SkillId.Mon_pc_summon_boss_merregina_Skill_2)]
	public class Mon_pc_summon_boss_merregina_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_circle006_violet", 3f),
				EndEffect = new EffectConfig("F_ground127_water", 0.60000002f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				DelayTime = 2f,
				FlyTime = 0.6f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("None", 2f),
			};

			Position position;

			for (var i = 0; i < 5; i++)
			{
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
				await MissileFall(caster, skill, position, config);
				await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_merregina_Skill_3)]
	public class Mon_pc_summon_boss_merregina_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var position = originPos.GetRelative(farPos, distance: 90f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 4f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup041_water_blue", 1f),
				Range = 50f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1050));
			var targetPos = originPos.GetRelative(farPos, distance: 90f);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.merregina_pad_pc_summon);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_mirtis_Skill_1)]
	public class Mon_pc_summon_boss_mirtis_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var position = originPos.GetRelative(farPos, distance: 60f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2.5f),
				PositionDelay = 1200,
				Effect = EffectConfig.None,
				Range = 60f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 2,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_mirtis_Skill_2)]
	public class Mon_pc_summon_boss_mirtis_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2.5f),
				PositionDelay = 0,
				Effect = new EffectConfig("F_rize007", 0.5f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 2,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			for (var i = 0; i < 4; i++)
			{
				var position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 0.45278701, angle: 0f, rand: 140, height: 1);
				await EffectAndHit(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_mirtis_Skill_3)]
	public class Mon_pc_summon_boss_mirtis_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var startingPosition = originPos.GetRelative(farPos, distance: 0.75370902f);
			var endingPosition = originPos.GetRelative(farPos, distance: 230f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.2f,
				PositionDelay = 0f,
				HitEffect = EffectConfig.None,
				Range = 30f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Moringponia_Skill_1)]
	public class Mon_pc_summon_boss_Moringponia_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 800,
				Effect = new EffectConfig("E_Moringponia", 0.8f),
				Range = 60f,
				KnockdownPower = 0f,
				Delay = 500f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 1f,
				InnerRange = 0f,
			};

			for (var i = 0; i < 4; i++)
			{
				var position = originPos.GetRelative(farPos, distance: 50f);
				await EffectAndHit(skill, caster, position, config, hits);

				if (i < 3)
					await skill.Wait(TimeSpan.FromMilliseconds(400));
			}
			SkillResultTargetBuff(caster, skill, BuffId.UC_stun, 1, 0f, 2000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Mothstem_Skill_1)]
	public class Mon_pc_summon_boss_Mothstem_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var position = originPos.GetRelative(farPos);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Mothstem, 0f, 100f, 8, 60f, 200f, 50f, 500f);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Naktis_Skill_1)]
	public class Mon_pc_summon_boss_Naktis_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 100, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2100;
			var damageDelay = 2300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Naktis_Skill_2)]
	public class Mon_pc_summon_boss_Naktis_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 230, width: 15);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2100;
			var damageDelay = 2300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 230, width: 15);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 230, width: 15);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 230, width: 15);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 230, width: 15);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 0;
			damageDelay = 2900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 230, width: 15);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 3100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 230, width: 15);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 3300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Naktis_Skill_3)]
	public class Mon_pc_summon_boss_Naktis_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground028_rize", 0.5f),
				PositionDelay = 500,
				Effect = new EffectConfig("F_rize001", 0.8f),
				Range = 35f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 130, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130, height: 1);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130, height: 1);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130, height: 1);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Neop_Skill_1)]
	public class Mon_pc_summon_boss_Neop_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			var position = GetRelativePosition(PosType.TargetRandom, caster, target);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Neop, 0f, 200f, 8, -45f, 150f, 50f);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_nuodai_Skill_1)]
	public class Mon_pc_summon_boss_nuodai_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 80, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2000;
			var damageDelay = 2200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_nuodai_Skill_2)]
	public class Mon_pc_summon_boss_nuodai_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2400));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_spread_out037_violet_ice_leaf", 1.3f),
				Range = 100f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("None", 2f),
				Range = 150f,
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
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_spread_out037_violet_ice_leaf", 3f),
				Range = 200f,
				KnockdownPower = 200f,
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

	[SkillHandler(SkillId.Mon_pc_summon_boss_nuodai_Skill_3)]
	public class Mon_pc_summon_boss_nuodai_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 300f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 190, height: 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_nuodai_hail);
			await skill.Wait(TimeSpan.FromMilliseconds(2700));
			targetPos = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 180);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_nuodai_hail);
			targetPos = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 200);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.boss_nuodai_hail);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_plokste_Skill_1)]
	public class Mon_pc_summon_boss_plokste_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var position = originPos.GetRelative(farPos, distance: 40f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup007_smoke", 0.9f),
				Range = 50f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 3,
				HitDuration = 300f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 10f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Pyroego_Skill_1)]
	public class Mon_pc_summon_boss_Pyroego_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 100, angle: 45f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 100, angle: 45f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 600;
			damageDelay = 1800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Pyroego_Skill_2)]
	public class Mon_pc_summon_boss_Pyroego_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force006_fire#Bip001 L Finger1", 2f),
				EndEffect = new EffectConfig("F_burstup027_fire1", 2.5f),
				Range = 23f,
				FlyTime = 0.7f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 160, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 160, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 160, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 160, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 160, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 100f,
				KnockdownPower = 200f,
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

	[SkillHandler(SkillId.Mon_pc_summon_boss_Pyroego_Skill_3)]
	public class Mon_pc_summon_boss_Pyroego_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var arrowConfig = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.5f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_explosion050_fire", 2f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.4f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			Position startingPosition;
			Position endingPosition;

			for (var i = 0; i < 7; i++)
			{
				startingPosition = originPos.GetRelative(farPos, distance: 20f);
				endingPosition = originPos.GetRelative(farPos, distance: 250f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig);
			}
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var arrowConfig2 = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.5f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_explosion050_fire", 2f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.4f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 6; i++)
			{
				startingPosition = originPos.GetRelative(farPos, distance: 250f);
				endingPosition = originPos.GetRelative(farPos, distance: 20f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig2);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Reaverpede_Skill_1)]
	public class Mon_pc_summon_boss_Reaverpede_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 80f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force045_green", 1f),
				EndEffect = new EffectConfig("F_ground004_yellow", 0.7f),
				Range = 35f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 500f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			};

			for (var i = 0; i < 5; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 40, height: 2);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Reaverpede_Skill_2)]
	public class Mon_pc_summon_boss_Reaverpede_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120, height: 2);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force045_green#B_chimney L1 02", 1f),
				EndEffect = new EffectConfig("F_ground004_yellow", 0.7f),
				Range = 35f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 500f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			});
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120, height: 2);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force045_green#B_chimney R2 02", 1f),
				EndEffect = new EffectConfig("F_ground004_yellow", 0.7f),
				Range = 35f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 500f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			});
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120, height: 2);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force045_green#B_chimney L3 02", 1f),
				EndEffect = new EffectConfig("F_ground004_yellow", 0.7f),
				Range = 35f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 500f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			});
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120, height: 2);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force045_green#B_chimney R1 02", 1f),
				EndEffect = new EffectConfig("F_ground004_yellow", 0.7f),
				Range = 35f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 500f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			});
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 120, height: 2);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force045_green#B_chimney R3 02", 1f),
				EndEffect = new EffectConfig("F_ground004_yellow", 0.7f),
				Range = 35f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 500f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.8f),
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Reaverpede_Skill_3)]
	public class Mon_pc_summon_boss_Reaverpede_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1700));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.5f),
				PositionDelay = 0,
				Effect = new EffectConfig("F_smoke006", 1f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 5,
				HitDuration = 1500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			position = originPos.GetRelative(farPos, distance: 80.33934f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.5f),
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 45f,
				KnockdownPower = 100f,
				Delay = 1f,
				HitCount = 2,
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

	[SkillHandler(SkillId.Mon_pc_summon_boss_Silva_griffin_Skill_1)]
	public class Mon_pc_summon_boss_Silva_griffin_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 0, angle: 55f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 900;
			var damageDelay = 1100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Silva_griffin_Skill_2)]
	public class Mon_pc_summon_boss_Silva_griffin_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1350));
			var startingPosition = originPos.GetRelative(farPos, distance: 50f);
			var endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_ground041", 1f),
				Range = 40f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			startingPosition = originPos.GetRelative(farPos, distance: 50f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_spread_out006", 1f),
				Range = 20f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Silva_griffin_Skill_3)]
	public class Mon_pc_summon_boss_Silva_griffin_Skill_3 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 90);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 600;
			var damageDelay = 800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 90);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 400;
			damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 90);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 400;
			damageDelay = 1600;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
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
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.ShadowGaoler_circlewave);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Slogutis_Skill_1)]
	public class Mon_pc_summon_boss_Slogutis_Skill_1 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var config = new EffectHitConfig
			{
				PositionDelay = 1000,
				GroundEffect = new EffectConfig("I_explosion002_dark2", 3f),
				Range = 101f,
				KnockdownPower = 80f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var position = originPos.GetRelative(farPos, distance: 96.552032f);
			await EffectAndHitRangePreview(skill, caster, position, config, hits);
			position = originPos.GetRelative(farPos, distance: 96.219543f);
			await EffectAndHitRangePreview(skill, caster, position, config, hits);

			foreach (var hit in hits)
				SkillResultKnockTarget(caster, null, skill, hit, KnockType.KnockDown, KnockDirection.TowardsTarget, 200, 40, 0, 1, 2);
			// TODO: Run Conditional Script S_R_COND_SCRIPT

		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Slogutis_Skill_2)]
	public class Mon_pc_summon_boss_Slogutis_Skill_2 : ITargetSkillHandler
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
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 4f),
				PositionDelay = 600,
				Effect = new EffectConfig("F_explosion098_dark_blue", 1f),
				Range = 60f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 53.645897f);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 4f),
				PositionDelay = 600,
				Effect = new EffectConfig("F_explosion098_dark_red", 1f),
				Range = 60f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			position = originPos.GetRelative(farPos, distance: 50.417118f);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 73.4319f);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 84.716988f);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 97.820656f);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 103.55984f);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 85.29747f);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 117.40915f);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 117.40915f);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 98.837898f);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos, distance: 112.82908f);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);

			foreach (var hit in hits)
				SkillResultKnockTarget(caster, null, skill, hit, KnockType.Motion, KnockDirection.TowardsTarget, 200, 30, 0, 1, 2);
			// TODO: Run Conditional Script S_R_COND_SCRIPT

		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Slogutis_Skill_3)]
	public class Mon_pc_summon_boss_Slogutis_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 30f,
				ArrowSpacingTime = 30f,
				ArrowLifeTime = 1000f,
				PositionDelay = 101f,
				HitEffect = new EffectConfig("F_burstup063_violet", 1f),
				Range = 40f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos, distance: 30.994553f);
			var endingPosition = originPos.GetRelative(farPos, distance: 247.14633f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			startingPosition = originPos.GetRelative(farPos, distance: 25.588245f);
			endingPosition = originPos.GetRelative(farPos, distance: 230.44615f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 25.588245f);
			endingPosition = originPos.GetRelative(farPos, distance: 234.196f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(900));
			startingPosition = originPos.GetRelative(farPos, distance: 25.588245f);
			endingPosition = originPos.GetRelative(farPos, distance: 230.44615f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 25.588245f);
			endingPosition = originPos.GetRelative(farPos, distance: 247.14633f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 25.588245f);
			endingPosition = originPos.GetRelative(farPos, distance: 234.196f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			// TODO: Run Conditional Script S_R_COND_SCRIPT

		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Slogutis_Skill_4)]
	public class Mon_pc_summon_boss_Slogutis_Skill_4 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.Zero;
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 0;
			var damageDelay = 0;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 0;
			damageDelay = 0;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 0;
			damageDelay = 0;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 0;
			damageDelay = 0;
			hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var arrowConfig = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 30f,
				ArrowSpacingTime = 30f,
				ArrowLifeTime = 1000f,
				PositionDelay = 101f,
				HitEffect = new EffectConfig("F_burstup063_violet", 1f),
				Range = 40f,
				KnockdownPower = 230f,
				Delay = 0f,
				HitEffectSpacing = 40f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos, distance: 30.994553f);
			var endingPosition = originPos.GetRelative(farPos, distance: 247.14633f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig, hits);
			startingPosition = originPos.GetRelative(farPos, distance: 25.588245f);
			endingPosition = originPos.GetRelative(farPos, distance: 230.44615f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig, hits);
			startingPosition = originPos.GetRelative(farPos, distance: 28.979105f);
			endingPosition = originPos.GetRelative(farPos, distance: 234.196f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			startingPosition = originPos.GetRelative(farPos, distance: 34.418114f);
			endingPosition = originPos.GetRelative(farPos, distance: 239.769f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig, hits);
			startingPosition = originPos.GetRelative(farPos, distance: 34.669483f);
			endingPosition = originPos.GetRelative(farPos, distance: 208.46649f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig, hits);
			startingPosition = originPos.GetRelative(farPos, distance: 35.786785f);
			endingPosition = originPos.GetRelative(farPos, distance: 237.38533f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var arrowConfig2 = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 35f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_burstup063_violet", 1f),
				Range = 40f,
				KnockdownPower = 230f,
				Delay = 0f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			startingPosition = originPos.GetRelative(farPos, distance: 25.588245f);
			endingPosition = originPos.GetRelative(farPos, distance: 284.9906f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig2, hits);
			startingPosition = originPos.GetRelative(farPos, distance: 30.994553f);
			endingPosition = originPos.GetRelative(farPos, distance: 301.02069f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig2, hits);
			startingPosition = originPos.GetRelative(farPos, distance: 28.979105f);
			endingPosition = originPos.GetRelative(farPos, distance: 278.39847f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig2, hits);

			foreach (var hit in hits)
				SkillResultKnockTarget(caster, null, skill, hit, KnockType.Motion, KnockDirection.TowardsTarget, 230, 10, 0, 1, 2);
			// TODO: Run Conditional Script S_R_COND_SCRIPT

		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Spector_F_Skill_1)]
	public class Mon_pc_summon_boss_Spector_F_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 30, angle: 40f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 400;
			var damageDelay = 600;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Spector_F_Skill_2)]
	public class Mon_pc_summon_boss_Spector_F_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion074_green", 1f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 1f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion074_green", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 1f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			for (var i = 0; i < 4; i++)
			{
				position = originPos.GetRelative(farPos, distance: 50f);
				await EffectAndHit(skill, caster, position, effectHitConfig2);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Spector_F_Skill_3)]
	public class Mon_pc_summon_boss_Spector_F_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var arrowConfig = new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_target_boss##0.7", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.1f,
				ArrowLifeTime = 0.7f,
				PositionDelay = 2000f,
				HitEffect = new EffectConfig("F_burstup004_dark", 0.4f),
				Range = 20f,
				KnockdownPower = 150f,
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
				startingPosition = originPos.GetRelative(farPos, distance: 100f);
				endingPosition = originPos.GetRelative(farPos, distance: 100f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig);
			}
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			startingPosition = originPos.GetRelative(farPos, distance: 100f);
			endingPosition = originPos.GetRelative(farPos, distance: 100f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_target_boss##0.7", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.1f,
				ArrowLifeTime = 0.7f,
				PositionDelay = 2000f,
				HitEffect = new EffectConfig("F_burstup004_dark", 0.4f),
				Range = 20f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			var arrowConfig2 = new ArrowConfig
			{
				ArrowEffect = new EffectConfig("F_sys_target_boss##1", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.1f,
				ArrowLifeTime = 0.7f,
				PositionDelay = 2000f,
				HitEffect = new EffectConfig("F_burstup004_dark", 0.4f),
				Range = 20f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			startingPosition = originPos.GetRelative(farPos, distance: 100f);
			endingPosition = originPos.GetRelative(farPos, distance: 100f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig2);
			startingPosition = originPos.GetRelative(farPos, distance: 100f);
			endingPosition = originPos.GetRelative(farPos, distance: 100f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, arrowConfig2);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_spector_gh_Skill_1)]
	public class Mon_pc_summon_boss_spector_gh_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			var position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 50f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_spector_gh_Skill_2)]
	public class Mon_pc_summon_boss_spector_gh_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 60);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 500;
			var damageDelay = 700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_spector_gh_Skill_3)]
	public class Mon_pc_summon_boss_spector_gh_Skill_3 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 100);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion005_violet", 0.8f),
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

	[SkillHandler(SkillId.Mon_pc_summon_boss_succubus_Skill_1)]
	public class Mon_pc_summon_boss_succubus_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 15, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1100;
			var damageDelay = 1300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_succubus_Skill_2)]
	public class Mon_pc_summon_boss_succubus_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 70, angle: 45f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			// TODO: No Implementation S_R_PULL_TARGET

			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 5000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_succubus_Skill_3)]
	public class Mon_pc_summon_boss_succubus_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_spin003_violet##1", 1.5f),
				Range = 60f,
				KnockdownPower = 120f,
				Delay = 0f,
				HitCount = 2,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 30f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_spin003_violet##1", 2.3f),
				Range = 100f,
				KnockdownPower = 120f,
				Delay = 0f,
				HitCount = 2,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 60f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_SwordBallista_Skill_1)]
	public class Mon_pc_summon_boss_SwordBallista_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			var position = originPos.GetRelative(farPos, distance: 32.709518f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("None", 1.5f),
				Range = 100f,
				KnockdownPower = 0f,
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

	[SkillHandler(SkillId.Mon_pc_summon_boss_SwordBallista_Skill_2)]
	public class Mon_pc_summon_boss_SwordBallista_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground093_dark##0.5", 2.3f),
				Range = 60f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 30f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("None", 3.3f),
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 60f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("None", 3.3f),
				Range = 150f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 100f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_SwordBallista_Skill_3)]
	public class Mon_pc_summon_boss_SwordBallista_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 90f,
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

			var delays = new[] { 300, 400, 500 };
			for (var i = 0; i < 4; i++)
			{
				var position = originPos.GetRelative(farPos, distance: 35f);
				await EffectAndHit(skill, caster, position, config);

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Tantaliser_Skill_1)]
	public class Mon_pc_summon_boss_Tantaliser_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion026_rize_violet", 0.8f),
				Range = 40f,
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

			Position position;

			var delays = new[] { 100, 100, 100, 50, 50, 1600 };
			for (var i = 0; i < 6; i++)
			{
				position = originPos.GetRelative(farPos, distance: 80f);
				await EffectAndHit(skill, caster, position, config);

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_explosion026_rize_violet", 1f),
				PositionDelay = 100,
				Effect = new EffectConfig("F_ground139_light_violet", 2f),
				Range = 100f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_TombLord_Skill_1)]
	public class Mon_pc_summon_boss_TombLord_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos, distance: 100f);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 120f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_dark#Bip001 R Finger2Nub", 3f),
				EndEffect = new EffectConfig("F_explosion065_green", 0.6f),
				Range = 20f,
				FlyTime = 0.3f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.2f),
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 80, rand: 110, height: 2);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 80, rand: 110, height: 1);
			await MissileThrow(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 80, rand: 110, height: 2);
			await MissileThrow(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_TombLord_Skill_2)]
	public class Mon_pc_summon_boss_TombLord_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1400,
				Effect = new EffectConfig("I_tomblord_obj_atk001_mash", 0.6f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 1f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			Position position;

			for (var i = 0; i < 6; i++)
			{
				position = originPos.GetRelative(farPos, distance: 50f);
				await EffectAndHit(skill, caster, position, effectHitConfig);
			}
			await skill.Wait(TimeSpan.FromMilliseconds(3900));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("I_tomblord_obj_atk001_mash##0.8", 0.6f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 1f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140, height: 3);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140, height: 3);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140, height: 3);
				await EffectAndHit(skill, caster, position, effectHitConfig2);
			}
			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140, height: 3);
				await EffectAndHit(skill, caster, position, effectHitConfig2);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_TombLord_Skill_3)]
	public class Mon_pc_summon_boss_TombLord_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 8f),
				PositionDelay = 1500,
				Effect = EffectConfig.None,
				Range = 70f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_dark", 3.5f),
				EndEffect = new EffectConfig("F_explosion065_green", 0.5f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				DelayTime = 1f,
				FlyTime = 1.7f,
				Height = 600f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("None", 1.2f),
			};

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170);
			await MissileFall(caster, skill, position, config);
			for (var i = 0; i < 6; i++)
			{
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
				await MissileFall(caster, skill, position, config);
			}
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 8f),
				PositionDelay = 1800,
				Effect = new EffectConfig("F_explosion052_mint", 4f),
				Range = 100f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 0f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 60f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_torelodeer_Skill_1)]
	public class Mon_pc_summon_boss_torelodeer_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var position = originPos.GetRelative(farPos, distance: 60f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup007_smoke", 0.9f),
				Range = 60f,
				KnockdownPower = 120f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_torelodeer_Skill_2)]
	public class Mon_pc_summon_boss_torelodeer_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 180f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(2450));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke011_red_white#Bip001 L Hand", 0.4f),
				EndEffect = new EffectConfig("F_explosion032_red", 1f),
				Range = 30f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 10f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(10));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(10));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(10));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(40));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_torelodeer_Skill_3)]
	public class Mon_pc_summon_boss_torelodeer_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(2700));
			var config = new MissileConfig
			{
				Effect = EffectConfig.None,
				EndEffect = new EffectConfig("F_explosion033_orange3", 0.89999998f),
				DotEffect = EffectConfig.None,
				Range = 15f,
				DelayTime = 0.25f,
				FlyTime = 0.2f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
				KnockdownPower = 180f,
				KnockType = (KnockType)4,
				VerticalAngle = 60f,
			};

			var position = originPos.GetRelative(farPos, distance: 135.89349f);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 123.49474f);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 149.89737f);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 103.66057f);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileFall(caster, skill, position, config);
			position = originPos.GetRelative(farPos, distance: 89.458336f);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 139.01341f);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 115.86102f);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 93.921181f);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 150);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 93.565727f);
			await MissileFall(caster, skill, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_unicorn_Skill_1)]
	public class Mon_pc_summon_boss_unicorn_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 60);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2000;
			var damageDelay = 2200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_unicorn_Skill_2)]
	public class Mon_pc_summon_boss_unicorn_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos, distance: 150f);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 5));
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 150, rand: 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force071_light_green#Bone_hair03", 4f),
				EndEffect = new EffectConfig("I_explosion009_rainbow", 2f),
				Range = 50f,
				FlyTime = 0.7f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_unicorn_Skill_3)]
	public class Mon_pc_summon_boss_unicorn_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2300));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 120f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 2,
				HitDuration = 1300f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_upinis_Skill_1)]
	public class Mon_pc_summon_boss_upinis_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 2.3827567E-39f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_water020_burstup", 1.2f),
				Range = 100f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			}, hits);

			foreach (var hit in hits)
				SkillResultKnockTarget(caster, null, skill, hit, KnockType.KnockDown, KnockDirection.TowardsTarget, 200, 40, 0, 1, 2);
			SkillResultTargetBuff(caster, skill, BuffId.FREEZE_EFFECT, 99, 0f, 3000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_valdovas_Skill_1)]
	public class Mon_pc_summon_boss_valdovas_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 900;
			var damageDelay = 1100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 300;
			damageDelay = 1400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			var position = originPos.GetRelative(farPos, distance: 30f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 160f,
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

	[SkillHandler(SkillId.Mon_pc_summon_boss_valdovas_Skill_2)]
	public class Mon_pc_summon_boss_valdovas_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 145f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_smoke125", 1.3f),
				Range = 50f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup003", 1f),
				Range = 60f,
				KnockdownPower = 180f,
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

	[SkillHandler(SkillId.Mon_pc_summon_boss_valdovas_Skill_3)]
	public class Mon_pc_summon_boss_valdovas_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red#Bone011", 0.5f),
				EndEffect = new EffectConfig("F_ground131_dark_red##0.3", 0.8f),
				Range = 15f,
				FlyTime = 1.2f,
				DelayTime = 0.3f,
				Gravity = 700f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, missileConfig);
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red#Bone002", 0.5f),
				EndEffect = new EffectConfig("F_ground131_dark_red##0.3", 0.8f),
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0.3f,
				Gravity = 700f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, missileConfig);
			var missileConfig3 = new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red#Bone003", 0.5f),
				EndEffect = new EffectConfig("F_ground131_dark_red##0.3", 0.8f),
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0.3f,
				Gravity = 700f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, missileConfig3);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, missileConfig3);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_smoke076", 1f),
				Range = 100f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 0f,
			};

			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red#Bone003", 0.5f),
				EndEffect = new EffectConfig("F_ground131_dark_red##0.3", 0.8f),
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0.3f,
				Gravity = 450f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			});
			var missileConfig4 = new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red#Bone001", 0.5f),
				EndEffect = new EffectConfig("F_ground131_dark_red##0.3", 0.8f),
				Range = 15f,
				FlyTime = 0.8f,
				DelayTime = 0.3f,
				Gravity = 450f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, missileConfig4);
			var missileConfig5 = new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red#Bone010", 0.5f),
				EndEffect = new EffectConfig("F_ground131_dark_red##0.3", 0.8f),
				Range = 15f,
				FlyTime = 0.6f,
				DelayTime = 0.3f,
				Gravity = 450f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			};

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, missileConfig5);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red#Bone012", 0.5f),
				EndEffect = new EffectConfig("F_ground131_dark_red##0.3", 0.8f),
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0.3f,
				Gravity = 450f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
			});
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, missileConfig5);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 170, height: 3);
			await MissileThrow(skill, caster, position, missileConfig4);
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_velnewt_Skill_1)]
	public class Mon_pc_summon_boss_velnewt_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_explosion006_orange", 0.7f),
				Range = 40f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 60f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 55.198566f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 55.075836f);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Velorchard_Skill_1)]
	public class Mon_pc_summon_boss_Velorchard_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2100);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 80, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1900;
			var damageDelay = 2100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Velorchard_Skill_2)]
	public class Mon_pc_summon_boss_Velorchard_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var targetPos = originPos.GetRelative(farPos, distance: 80f);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 80f, 50));
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force011_green#Dummy_skl1_eye_effect", 1f),
				EndEffect = new EffectConfig("F_ground004_green", 0.4f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.5f),
			};

			for (var i = 0; i < 10; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 60, rand: 40);
				await MissileThrow(skill, caster, position, config);

				if (i < 9)
					await skill.Wait(TimeSpan.FromMilliseconds(10));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Velorchard_Skill_3)]
	public class Mon_pc_summon_boss_Velorchard_Skill_3 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 15, angle: 5f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2100;
			var damageDelay = 2400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 15, angle: 5f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 15, angle: 5f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2600;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 15, angle: 5f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 15, angle: 5f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 15, angle: 5f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 2900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 15, angle: 5f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 100;
			damageDelay = 3000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_velpede_Skill_1)]
	public class Mon_pc_summon_boss_velpede_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var startingPosition = originPos.GetRelative(farPos, distance: 40f);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = new EffectConfig("none", 1f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_ground066_smoke", 3f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Zawra_Skill_1)]
	public class Mon_pc_summon_boss_Zawra_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 30, angle: 60f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Zawra_Skill_2)]
	public class Mon_pc_summon_boss_Zawra_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 100);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2100;
			var damageDelay = 2300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 120);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 300;
			damageDelay = 2600;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 130);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 300;
			damageDelay = 2900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 130);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 300;
			damageDelay = 3200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_boss_Zawra_Skill_3)]
	public class Mon_pc_summon_boss_Zawra_Skill_3 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 110, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 110, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 300;
			damageDelay = 2300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 110, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 110, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 110, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 200;
			damageDelay = 2900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_spin038_orange", 2.5f),
				Range = 80f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 0f,
				HitTimeSpacing = 0.2f,
				HitCount = 0,
				HitDuration = 1000f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			startingPosition = originPos.GetRelative(farPos);
			endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_smoke117", 1f),
				Range = 80f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 0f,
				HitTimeSpacing = 0.2f,
				HitCount = 0,
				HitDuration = 1000f,
			});
			startingPosition = originPos.GetRelative(farPos);
			endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("I_light013_spark_orange2", 3f),
				Range = 80f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 0f,
				HitTimeSpacing = 0.2f,
				HitCount = 0,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_PC_summon_Canceril_Skill_1)]
	public class Mon_PC_summon_Canceril_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup041_water_blue", 0.8f),
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
			};

			var position = originPos.GetRelative(farPos, distance: 30f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 30f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 30f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 30f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 150f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_burstup041_water_blue", 0.8f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Chapparition_Skill_1)]
	public class Mon_pc_summon_Chapparition_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 60);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Chapparition_Skill_2)]
	public class Mon_pc_summon_Chapparition_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 5f),
				PositionDelay = 600,
				Effect = new EffectConfig("F_fire003_violet", 1f),
				Range = 55f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 5,
				HitDuration = 3000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2f),
				PositionDelay = 2100,
				Effect = new EffectConfig("F_burstup002_dark", 2f),
				Range = 55f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 0,
				HitDuration = 3000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2f),
				PositionDelay = 2100,
				Effect = new EffectConfig("I_stone009_mash", 1f),
				Range = 55f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 3000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Chapparition_Skill_3)]
	public class Mon_pc_summon_Chapparition_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_spread_in009_rize##0.5", 0.5f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_rize006", 0.6f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var delays = new[] { 100, 100, 200, 200, 100, 100 };
			for (var i = 0; i < 7; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
				await EffectAndHit(skill, caster, position, config);

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Deathweaver_Skill_1)]
	public class Mon_pc_summon_Deathweaver_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 50, angle: 40f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Deathweaver_Skill_2)]
	public class Mon_pc_summon_Deathweaver_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2100;
			var damageDelay = 2300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30, angle: 10f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 600;
			damageDelay = 2900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(3700));
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1f,
				HitEffect = new EffectConfig("F_levitation001", 1f),
				Range = 20f,
				KnockdownPower = 150f,
				Delay = 1f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.01f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos, distance: 52.706604f);
			var endingPosition = originPos.GetRelative(farPos, distance: 148.34892f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 47.413452f);
			endingPosition = originPos.GetRelative(farPos, distance: 151.04364f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 48.912506f);
			endingPosition = originPos.GetRelative(farPos, distance: 149.50655f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Deathweaver_Skill_3)]
	public class Mon_pc_summon_Deathweaver_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion005_violet", 1f),
				Range = 100f,
				KnockdownPower = 150f,
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

	[SkillHandler(SkillId.Mon_pc_summon_durahan_Skill_1)]
	public class Mon_pc_summon_durahan_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_durahan_Skill_2)]
	public class Mon_pc_summon_durahan_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			var position = originPos.GetRelative(farPos, distance: 91.618141f);
			MonsterSkillPadLookDirMissile(caster, skill, position, PadName.boss_hydra_square, 200f, 1, 150f, 100f, 0f, 0);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_durahan_Skill_3)]
	public class Mon_pc_summon_durahan_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var startingPosition = originPos.GetRelative(farPos, distance: 3.8255448E-43f);
			var endingPosition = originPos.GetRelative(farPos, distance: 220f);
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
				Delay = 200f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.15f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_ellaganos_Skill_1)]
	public class Mon_pc_summon_ellaganos_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var position = originPos.GetRelative(farPos, distance: 25f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_smoke037", 1f),
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground037_smoke", 2f),
				Range = 30f,
				KnockdownPower = 250f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 65f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_ellaganos_Skill_2)]
	public class Mon_pc_summon_ellaganos_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 70, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 500;
			var damageDelay = 700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_ellaganos_Skill_3)]
	public class Mon_pc_summon_ellaganos_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion005_violet", 1f),
				Range = 100f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			position = originPos.GetRelative(farPos);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_ellaganos_Skill_4)]
	public class Mon_pc_summon_ellaganos_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(700));
			var position = originPos.GetRelative(farPos, distance: 120f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 500,
				Effect = new EffectConfig("F_ground042_light", 0.9f),
				Range = 70f,
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

	[SkillHandler(SkillId.Mon_pc_summon_Fireload_Skill_1)]
	public class Mon_pc_summon_Fireload_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 40, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Fireload_Skill_2)]
	public class Mon_pc_summon_Fireload_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

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
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Fireload_Skill_3)]
	public class Mon_pc_summon_Fireload_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

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

			for (var i = 0; i < 7; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 2);
				await MissilePadThrow(skill, caster, position, config, 0f, "boss_firewall_red");
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Flammidus_Skill_1)]
	public class Mon_pc_summon_Flammidus_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1200;
			var damageDelay = 1400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Flammidus_Skill_2)]
	public class Mon_pc_summon_Flammidus_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 150f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Flammidus_Skill_3)]
	public class Mon_pc_summon_Flammidus_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup022_smoke_s", 1f),
				Range = 40f,
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
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_burstup030_fire", 0.3f),
				Range = 20f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 4; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 40f);
				var endingPosition = originPos.GetRelative(farPos, distance: 250f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Frogola_Skill_1)]
	public class Mon_pc_summon_Frogola_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(650);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 25);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 450;
			var damageDelay = 650;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 25);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 350;
			damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Frogola_Skill_2)]
	public class Mon_pc_summon_Frogola_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 100f, 50));
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup011_violet", 1f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80, height: 1);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Frogola_Skill_3)]
	public class Mon_pc_summon_Frogola_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 50));
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_violet#B_mouth 01", 1f),
				EndEffect = new EffectConfig("F_explosion103_violet", 1f),
				Range = 10f,
				FlyTime = 2.4f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, missileConfig);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, missileConfig);
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_violet#B_mouth 01", 1f),
				EndEffect = new EffectConfig("F_explosion103_violet", 1f),
				Range = 10f,
				FlyTime = 2.5f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, missileConfig2);
			var missileConfig3 = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_violet#B_mouth 01", 1f),
				EndEffect = new EffectConfig("F_explosion103_violet", 1f),
				Range = 10f,
				FlyTime = 2.6f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, missileConfig3);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, missileConfig3);
			var missileConfig4 = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_violet#B_mouth 01", 1f),
				EndEffect = new EffectConfig("F_explosion103_violet", 1f),
				Range = 10f,
				FlyTime = 2.7f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, missileConfig4);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileThrow(skill, caster, position, missileConfig4);
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			var missileConfig5 = new MissileConfig
			{
				Effect = new EffectConfig("I_force015_violet#B_mouth 01", 1f),
				EndEffect = new EffectConfig("F_explosion103_violet", 1f),
				Range = 10f,
				FlyTime = 0.3f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			position = originPos.GetRelative(farPos, distance: 55f);
			await MissileThrow(skill, caster, position, missileConfig5);
			position = originPos.GetRelative(farPos, distance: 55f);
			await MissileThrow(skill, caster, position, missileConfig5);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 55f);
			await MissileThrow(skill, caster, position, missileConfig5);
			position = originPos.GetRelative(farPos, distance: 55f);
			await MissileThrow(skill, caster, position, missileConfig5);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 55f);
			await MissileThrow(skill, caster, position, missileConfig5);
			position = originPos.GetRelative(farPos, distance: 55f);
			await MissileThrow(skill, caster, position, missileConfig5);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Genmagnus_Skill_1)]
	public class Mon_pc_summon_Genmagnus_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 45);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Genmagnus_Skill_2)]
	public class Mon_pc_summon_Genmagnus_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(3700);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 80, angle: 100f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 3500;
			var damageDelay = 3700;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Freeze, 1, 0f, 6000f, 1, 50, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Genmagnus_Skill_3)]
	public class Mon_pc_summon_Genmagnus_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Genmagnus_circlewave);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Glackuman_Skill_1)]
	public class Mon_pc_summon_Glackuman_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 40, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Glackuman_Skill_2)]
	public class Mon_pc_summon_Glackuman_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			var position = originPos.GetRelative(farPos, distance: 50f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("E_simorph_skl", 2f),
				Range = 85f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 200, rand: 70);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_simorph_skl_3Deffect_mash", 0.5f),
				EndEffect = new EffectConfig("E_simorph_skl", 1f),
				Range = 40f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Glackuman_Skill_3)]
	public class Mon_pc_summon_Glackuman_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var targetPos = originPos.GetRelative(farPos, distance: 60f);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 50));
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_simorph_skl_3Deffect_mash#Bip001 R Hand", 0.5f),
				EndEffect = new EffectConfig("E_simorph_skl", 1f),
				Range = 50f,
				FlyTime = 0.7f,
				DelayTime = 0f,
				Gravity = 100f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			for (var i = 0; i < 3; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 50);
				await MissileThrow(skill, caster, position, config);

				if (i < 2)
					await skill.Wait(TimeSpan.FromMilliseconds(1500));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Harpeia_Skill_1)]
	public class Mon_pc_summon_Harpeia_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 50);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 900;
			var damageDelay = 1100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Harpeia_Skill_2)]
	public class Mon_pc_summon_Harpeia_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 8f),
				PositionDelay = 2200,
				Effect = new EffectConfig("F_spin012_blue", 2.5f),
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
				PositionDelay = 2200,
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
				PositionDelay = 2500,
				Effect = new EffectConfig("None", 2f),
				Range = 150f,
				KnockdownPower = 150f,
				Delay = 0f,
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

	[SkillHandler(SkillId.Mon_pc_summon_Harpeia_Skill_3)]
	public class Mon_pc_summon_Harpeia_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var startingPosition = originPos.GetRelative(farPos, distance: 30f);
			var endingPosition = GetRelativePosition(PosType.Target, caster, target, distance: 200);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 2000f,
				HitEffect = new EffectConfig("F_spin005", 2.5f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			MonsterSkillSetCollisionDamage(caster, skill, false, 1f);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Kerberos_Skill_1)]
	public class Mon_pc_summon_Kerberos_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 75, width: 50, angle: 50f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 900;
			var damageDelay = 1100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Kerberos_Skill_2)]
	public class Mon_pc_summon_Kerberos_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 35);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2000;
			var damageDelay = 2200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Kerberos_Skill_3)]
	public class Mon_pc_summon_Kerberos_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 6f),
				PositionDelay = 1700,
				Effect = new EffectConfig("F_ground082_smoke", 1f),
				Range = 100f,
				KnockdownPower = 250f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 75f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Kerberos_Skill_4)]
	public class Mon_pc_summon_Kerberos_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos, distance: 120f);
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force039_orange#Bip01 HeadNub", 1.5f),
				EndEffect = new EffectConfig("F_burstup027_fire", 1.7f),
				Range = 20f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 50f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.4f),
			};

			for (var i = 0; i < 10; i++)
			{
				var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, rand: 80, height: 1);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Lithorex_Skill_1)]
	public class Mon_pc_summon_Lithorex_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 0.7f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_rize012_green_water", 0.3f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = GetRelativePosition(PosType.Target, caster, target);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.Target, caster, target);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Lithorex_Skill_2)]
	public class Mon_pc_summon_Lithorex_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 0.7f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_rize012_green_water", 0.3f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetHeight, caster, target);
				await EffectAndHit(skill, caster, position, config);
				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
			position = GetRelativePosition(PosType.TargetDistance, caster, target);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetHeight, caster, target);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Lithorex_Skill_3)]
	public class Mon_pc_summon_Lithorex_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_ground139_light_green", 1f),
				Range = 50f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 25f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_ground139_light_green", 1.6f),
				Range = 80f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 50f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_ground139_light_green", 2.3f),
				Range = 110f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 75f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Manticen_Skill_1)]
	public class Mon_pc_summon_Manticen_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup045", 1f),
				Range = 20f,
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
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_ground121", 1.5f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.08f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 6; i++)
			{
				var startingPosition = originPos.GetRelative(farPos);
				var endingPosition = originPos.GetRelative(farPos, distance: 120f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Marionette_Skill_1)]
	public class Mon_pc_summon_Marionette_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1900);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 40, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1700;
			var damageDelay = 1900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 10));
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Marionette_Skill_2)]
	public class Mon_pc_summon_Marionette_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2200));
			var position = originPos.GetRelative(farPos, distance: 40f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_smoke", 1f),
				Range = 70f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 15f,
				InnerRange = 40f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = originPos.GetRelative(farPos, distance: 40f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_burstup001_smoke", 1.5f),
				Range = 90f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 15f,
				InnerRange = 60f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Marionette_Skill_3)]
	public class Mon_pc_summon_Marionette_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(4000));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground083_smoke", 0.8f),
				Range = 75f,
				KnockdownPower = 50f,
				Delay = 0f,
				HitCount = 2,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 10f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_ground083_smoke", 0.8f),
				Range = 75f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_merregina_Skill_1)]
	public class Mon_pc_summon_merregina_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 50.299419f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 5f),
				PositionDelay = 1800,
				Effect = new EffectConfig("F_ground127_water", 1f),
				Range = 50f,
				KnockdownPower = 150f,
				Delay = 0f,
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

	[SkillHandler(SkillId.Mon_pc_summon_merregina_Skill_2)]
	public class Mon_pc_summon_merregina_Skill_2 : ITargetSkillHandler
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
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_circle006_violet", 3f),
				EndEffect = new EffectConfig("F_ground127_water", 0.60000002f),
				DotEffect = EffectConfig.None,
				Range = 20f,
				DelayTime = 2f,
				FlyTime = 0.6f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("None", 2f),
			};

			Position position;

			for (var i = 0; i < 5; i++)
			{
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
				await MissileFall(caster, skill, position, config);
				await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			await MissileFall(caster, skill, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_merregina_Skill_3)]
	public class Mon_pc_summon_merregina_Skill_3 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 150f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 4f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup041_water_blue", 1f),
				Range = 50f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1050));
			var targetPos = originPos.GetRelative(farPos, distance: 150f);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.merregina_pad);
			targetPos = GetRelativePosition(PosType.Target, caster, target, rand: 170, height: 2);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.merregina_pad);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_mineloader_Skill_1)]
	public class Mon_pc_summon_mineloader_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 5));
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force052_pink#Dummy_ray", 1f),
				EndEffect = new EffectConfig("I_explosion003_pink", 1f),
				Range = 15f,
				FlyTime = 0.7f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			for (var i = 0; i < 4; i++)
			{
				var position = GetRelativePosition(PosType.Target, caster, target);
				await MissileThrow(skill, caster, position, config);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_mineloader_Skill_2)]
	public class Mon_pc_summon_mineloader_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

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
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.5f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			var position = originPos.GetRelative(farPos, distance: 170f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2f),
				PositionDelay = 3400,
				Effect = new EffectConfig("F_smoke037", 1f),
				Range = 65f,
				KnockdownPower = 150f,
				Delay = 150f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(1700));
			MonsterSkillSetCollisionDamage(caster, skill, false, 1f);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Minotaurs_Skill_1)]
	public class Mon_pc_summon_Minotaurs_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 60f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Minotaurs_Skill_2)]
	public class Mon_pc_summon_Minotaurs_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 7f),
				PositionDelay = 3000,
				Effect = EffectConfig.None,
				Range = 100f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 2,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2800));
			caster.StartBuff(BuffId.Rage_Rockto_atk, 1f, 0f, TimeSpan.FromMilliseconds(30000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Minotaurs_Skill_3)]
	public class Mon_pc_summon_Minotaurs_Skill_3 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 30);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2500;
			var damageDelay = 2700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 25, width: 30);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 300;
			damageDelay = 3000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 30);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 500;
			damageDelay = 3500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: -10, width: 30);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 400;
			damageDelay = 3900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Mummyghast_Skill_1)]
	public class Mon_pc_summon_Mummyghast_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2000;
			var damageDelay = 2200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Mummyghast_Skill_2)]
	public class Mon_pc_summon_Mummyghast_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1300));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground068_smoke", 1f),
				PositionDelay = 700,
				Effect = new EffectConfig("F_burstup003", 0.8f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground068_smoke", 1f),
				PositionDelay = 700,
				Effect = new EffectConfig("F_burstup003", 0.8f),
				Range = 25f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_ground068_smoke", 1f),
				PositionDelay = 700,
				Effect = new EffectConfig("F_burstup003", 0.8f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			for (var i = 0; i < 4; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 1.4137681, angle: 135f, rand: 150);
				await EffectAndHit(skill, caster, position, effectHitConfig2);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Mummyghast_Skill_3)]
	public class Mon_pc_summon_Mummyghast_Skill_3 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 150, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 9f),
				PositionDelay = 2700,
				Effect = EffectConfig.None,
				Range = 150f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 5000f, 1, 100, -1, hits);
			// TODO: No Implementation S_R_PULL_TARGET

		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_necrovanter_Skill_1)]
	public class Mon_pc_summon_necrovanter_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			var startingPosition = originPos.GetRelative(farPos, distance: 20f);
			var endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.02f,
				ArrowLifeTime = 0.02f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_burstup022_smoke", 0.5f),
				Range = 20f,
				KnockdownPower = 50f,
				Delay = 0f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_necrovanter_Skill_2)]
	public class Mon_pc_summon_necrovanter_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.2f),
				PositionDelay = 1100,
				Effect = new EffectConfig("F_burstup022_smoke", 1f),
				Range = 35f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			Position position;

			for (var i = 0; i < 3; i++)
			{
				position = originPos.GetRelative(farPos, distance: 60f);
				await EffectAndHit(skill, caster, position, effectHitConfig);
			}
			await skill.Wait(TimeSpan.FromMilliseconds(1100));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.2f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup022_smoke", 1f),
				Range = 35f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			for (var i = 0; i < 3; i++)
			{
				position = originPos.GetRelative(farPos, distance: 60f);
				await EffectAndHit(skill, caster, position, effectHitConfig2);
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_necrovanter_Skill_3)]
	public class Mon_pc_summon_necrovanter_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var startingPosition = originPos.GetRelative(farPos, distance: 0.97327f);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.1f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_burstup004_dark", 0.8f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_necrovanter_Skill_4)]
	public class Mon_pc_summon_necrovanter_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 200f, 30));
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("I_spread_out011_dark", 2f),
				PositionDelay = 1300,
				Effect = new EffectConfig("F_burstup004_dark", 1f),
				Range = 30f,
				KnockdownPower = 150f,
				Delay = 100f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160, height: 1);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_NetherBovine_Skill_1)]
	public class Mon_pc_summon_NetherBovine_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 40, angle: 45f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 500;
			var damageDelay = 700;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_NetherBovine_Skill_2)]
	public class Mon_pc_summon_NetherBovine_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 100, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1300;
			var damageDelay = 1500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			var position = originPos.GetRelative(farPos, distance: 17.369144f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 6.5f),
				PositionDelay = 500,
				Effect = EffectConfig.None,
				Range = 80f,
				KnockdownPower = 0f,
				Delay = 1000f,
				HitCount = 4,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_NetherBovine_Skill_3)]
	public class Mon_pc_summon_NetherBovine_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetRelative(farPos, distance: 20f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 7f),
				PositionDelay = 1500,
				Effect = new EffectConfig("F_spread_out008_smoke", 2f),
				Range = 80f,
				KnockdownPower = 300f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 85f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			position = originPos.GetRelative(farPos, distance: 20f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 7f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_spread_out008_smoke", 2f),
				Range = 80f,
				KnockdownPower = 200f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 70f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Nuaelle_Skill_1)]
	public class Mon_pc_summon_Nuaelle_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 700;
			var damageDelay = 900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Nuaelle_Skill_2)]
	public class Mon_pc_summon_Nuaelle_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(2100));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke004_4_grey#Dummy_effect_hand_L", 1.2f),
				EndEffect = new EffectConfig("F_explosion033_orange", 0.5f),
				Range = 20f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			for (var i = 0; i < 13; i++)
			{
				var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 130);
				await MissileThrow(skill, caster, position, config);

				if (i < 12)
					await skill.Wait(TimeSpan.FromMilliseconds(50));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Nuaelle_Skill_3)]
	public class Mon_pc_summon_Nuaelle_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke004_4_grey#Bip001 R Finger0Nub", 0.8f),
				EndEffect = new EffectConfig("F_explosion033_orange", 1f),
				Range = 40f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			Position position;

			for (var i = 0; i < 7; i++)
			{
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 110, height: 2);
				await MissileThrow(skill, caster, position, missileConfig);

				if (i < 6)
					await skill.Wait(TimeSpan.FromMilliseconds(150));
			}
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("None#Dummy_effect_hand_R", 1.5f),
				EndEffect = new EffectConfig("F_burstup029_smoke_grey", 2f),
				Range = 40f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var delays = new[] { 250, 150, 150, 150, 150, 150, 150, 150 };
			for (var i = 0; i < 9; i++)
			{
				position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 110, height: 2);
				await MissileThrow(skill, caster, position, missileConfig2);

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Prisoncutter_Skill_1)]
	public class Mon_pc_summon_Prisoncutter_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 600;
			var damageDelay = 800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 30, angle: 30f);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 500;
			damageDelay = 1300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Prisoncutter_Skill_2)]
	public class Mon_pc_summon_Prisoncutter_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

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

	[SkillHandler(SkillId.Mon_pc_summon_Prisoncutter_Skill_3)]
	public class Mon_pc_summon_Prisoncutter_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

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

	[SkillHandler(SkillId.Mon_pc_summon_Rambandgad_Skill_1)]
	public class Mon_pc_summon_Rambandgad_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos, distance: 3.8255448E-43f);
			var targets = SkillSelectEnemiesInSquare(caster, targetPos, 0f, 150f, 30f, 1);
			target = targets.Random();
			if (target == null)
				return;
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var position = GetRelativePosition(PosType.TargetHeight, caster, target);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force058_blue#Bip001 R Finger2", 0.7f),
				EndEffect = new EffectConfig("I_bomb002_blue", 2f),
				Range = 40f,
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

	[SkillHandler(SkillId.Mon_pc_summon_Rambandgad_Skill_2)]
	public class Mon_pc_summon_Rambandgad_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(2490));
			var position = GetRelativePosition(PosType.TargetDistance, caster, target);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force019_mint#Bip001 L Finger21", 2f),
				EndEffect = new EffectConfig("F_explosion098_dark_mint", 1f),
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

	[SkillHandler(SkillId.Mon_pc_summon_Rambandgad_Skill_3)]
	public class Mon_pc_summon_Rambandgad_Skill_3 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 20);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Riteris_Skill_1)]
	public class Mon_pc_summon_Riteris_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 900;
			var damageDelay = 1100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Riteris_Skill_2)]
	public class Mon_pc_summon_Riteris_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

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

			var position = originPos.GetRelative(farPos, distance: 130.85342f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 143.50571f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 147.52362f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 131.3597f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 104.72799f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 81.209633f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos, distance: 99.584045f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(350));
			position = originPos.GetRelative(farPos, distance: 86.766907f);
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

	[SkillHandler(SkillId.Mon_pc_summon_Riteris_Skill_3)]
	public class Mon_pc_summon_Riteris_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 850,
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

			var position = originPos.GetRelative(farPos, distance: 79.391739f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 94.633537f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 75.20076f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 67.284729f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 73.701302f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 75.030533f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 81.899933f);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_ShadowGaoler_Skill_1)]
	public class Mon_pc_summon_ShadowGaoler_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 60, width: 40);
			splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			hitDelay = 300;
			damageDelay = 1300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_ShadowGaoler_Skill_2)]
	public class Mon_pc_summon_ShadowGaoler_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var position = originPos.GetRelative(farPos, distance: 60f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 1.4f),
				PositionDelay = 1800,
				Effect = new EffectConfig("F_ground086_violet", 2.5f),
				Range = 45f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 3,
				HitDuration = 700f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_ShadowGaoler_Skill_3)]
	public class Mon_pc_summon_ShadowGaoler_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 180f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force013_dark#Dummy_R_hand_effect", 1f),
				EndEffect = new EffectConfig("F_smoke021_green", 0.7f),
				Range = 20f,
				FlyTime = 2.5f,
				DelayTime = 0.1f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			var delays = new[] { 300, 300, 2300, 200 };
			for (var i = 0; i < 4; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
				await MissileThrow(skill, caster, position, config);

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			var delays2 = new[] { 200, 200, 2200, 200 };
			for (var i = 0; i < 4; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
				await MissileThrow(skill, caster, position, config);

				if (i < delays2.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays2[i]));
			}
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 120, height: 1);
			await MissileThrow(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 120, height: 1);
				await MissileThrow(skill, caster, position, config);

				if (i < 2)
					await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Sparnanman_Skill_1)]
	public class Mon_pc_summon_Sparnanman_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetRelative(farPos, distance: 73.41925f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 4f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup007_smoke1", 0.8f),
				Range = 40f,
				KnockdownPower = 200f,
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

	[SkillHandler(SkillId.Mon_pc_summon_Sparnanman_Skill_2)]
	public class Mon_pc_summon_Sparnanman_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force042_violet#Bip001 L Finger0Nub", 1.5f),
				EndEffect = new EffectConfig("F_ground004_violet", 0.5f),
				Range = 20f,
				FlyTime = (0.7f + (float)RandomProvider.Get().NextDouble() * 1.8f),
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_force042_violet#Bip001 R Finger0Nub", 1.5f),
				EndEffect = new EffectConfig("F_ground004_violet", 0.5f),
				Range = 20f,
				FlyTime = (0.7f + (float)RandomProvider.Get().NextDouble() * 1.8f),
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Sparnanman_Skill_3)]
	public class Mon_pc_summon_Sparnanman_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var startingPosition = originPos.GetRelative(farPos, distance: 20f);
			var endingPosition = originPos.GetRelative(farPos, distance: 200f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.02f,
				ArrowLifeTime = 0.03f,
				PositionDelay = 1800f,
				HitEffect = new EffectConfig("F_burstup007_smoke1", 0.5f),
				Range = 35f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(2700));
			MonsterSkillSetCollisionDamage(caster, skill, false, 1f);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Spector_m_Skill_1)]
	public class Mon_pc_summon_Spector_m_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 3));
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 10);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_rize006_green", 0.6f),
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
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Spector_m_Skill_2)]
	public class Mon_pc_summon_Spector_m_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var config = new ArrowConfig
			{
				ArrowEffect = new EffectConfig("None", 0.7f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.05f,
				ArrowLifeTime = 0.05f,
				PositionDelay = 1700f,
				HitEffect = new EffectConfig("F_smoke047", 0.8f),
				Range = 12f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitEffectSpacing = 20f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			for (var i = 0; i < 3; i++)
			{
				var startingPosition = originPos.GetRelative(farPos, distance: 40f);
				var endingPosition = originPos.GetRelative(farPos, distance: 200f);
				await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			}
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			var position = originPos.GetRelative(farPos, distance: 30f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = new EffectConfig("F_rize004_dark", 1f),
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 300f,
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

	[SkillHandler(SkillId.Mon_pc_summon_Spector_m_Skill_3)]
	public class Mon_pc_summon_Spector_m_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 160f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 110);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force035_red#B_Prop1", 1.5f),
				EndEffect = new EffectConfig("F_smoke029_violet", 2.2f),
				Range = 40f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 3f),
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Strongholder_Skill_1)]
	public class Mon_pc_summon_Strongholder_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 60, angle: 55f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 800;
			var damageDelay = 1000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Strongholder_Skill_2)]
	public class Mon_pc_summon_Strongholder_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1800;
			var damageDelay = 2000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Strongholder_Skill_3)]
	public class Mon_pc_summon_Strongholder_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos, distance: 12.135043f);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_spread_in008_red", 1f),
				EndEffect = new EffectConfig("F_explosion016_red", 0.60000002f),
				DotEffect = EffectConfig.None,
				Range = 25f,
				DelayTime = 2f,
				FlyTime = 1f,
				Height = 600f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = new EffectConfig("None", 2f),
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 160);
			await MissileFall(caster, skill, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Templeshooter_Skill_1)]
	public class Mon_pc_summon_Templeshooter_Skill_1 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 150, width: 35, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1300;
			var damageDelay = 1500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Templeshooter_Skill_2)]
	public class Mon_pc_summon_Templeshooter_Skill_2 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 200, width: 60, angle: 20f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2200;
			var damageDelay = 2400;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Templeshooter_Skill_3)]
	public class Mon_pc_summon_Templeshooter_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(3400));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_arrow003_dark1", 1f),
				EndEffect = new EffectConfig("F_ground077_smoke", 0.2f),
				DotEffect = EffectConfig.None,
				Range = 18f,
				DelayTime = 0f,
				FlyTime = 0.5f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
			};

			var position = originPos.GetRelative(farPos);
			await MissileFall(caster, skill, position, config);
			position = originPos.GetRelative(farPos);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos);
			await MissileFall(caster, skill, position, config);
			position = originPos.GetRelative(farPos);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos);
			await MissileFall(caster, skill, position, config);
			position = originPos.GetRelative(farPos);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos);
			await MissileFall(caster, skill, position, config);
			position = originPos.GetRelative(farPos);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos);
			await MissileFall(caster, skill, position, config);
			position = originPos.GetRelative(farPos);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileFall(caster, skill, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileFall(caster, skill, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 80);
			await MissileFall(caster, skill, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Templeshooter_Skill_4)]
	public class Mon_pc_summon_Templeshooter_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			var config = new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.1f,
				ArrowLifeTime = 0.5f,
				PositionDelay = 1000f,
				HitEffect = new EffectConfig("F_burstup031_dark", 0.4f),
				Range = 15f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 35f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 1000f,
			};

			var startingPosition = originPos.GetRelative(farPos, distance: 30f);
			var endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
			startingPosition = originPos.GetRelative(farPos, distance: 30f);
			endingPosition = originPos.GetRelative(farPos, distance: 180f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Throneweaver_Skill_1)]
	public class Mon_pc_summon_Throneweaver_Skill_1 : ITargetSkillHandler
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

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetPos = originPos.GetRelative(farPos, distance: 130f);

			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 120, rand: 80, height: 1);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force003_green#Bone015_L", 4f),
				EndEffect = new EffectConfig("F_explosion073_ground", 4f),
				Range = 45f,
				FlyTime = 0.4f,
				DelayTime = 0f,
				Gravity = 1f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 3.5f),
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Throneweaver_Skill_2)]
	public class Mon_pc_summon_Throneweaver_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			await skill.Wait(TimeSpan.FromMilliseconds(1400));
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force011_green#Dummy_effect_tail", 1f),
				EndEffect = new EffectConfig("F_explosion052_green##0.8", 1f),
				Range = 10f,
				FlyTime = 1.2f,
				DelayTime = 0f,
				Gravity = 800f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 2.5f),
			};

			Position position;

			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetDistance, caster, target);
				await MissileThrow(skill, caster, position, missileConfig);
				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_force011_green#Dummy_effect_tail", 1f),
				EndEffect = new EffectConfig("F_explosion052_green##0.8", 1f),
				Range = 10f,
				FlyTime = 1.2f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 2.5f),
			};

			position = GetRelativePosition(PosType.TargetDistance, caster, target);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target);
			await MissileThrow(skill, caster, position, missileConfig2);
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var missileConfig3 = new MissileConfig
			{
				Effect = new EffectConfig("I_force011_green#Dummy_effect_tail", 1f),
				EndEffect = new EffectConfig("F_explosion052_green", 1f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 2.5f),
			};

			position = GetRelativePosition(PosType.TargetDistance, caster, target);
			await MissileThrow(skill, caster, position, missileConfig3);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetHeight, caster, target);
				await MissileThrow(skill, caster, position, missileConfig3);
				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
			position = GetRelativePosition(PosType.TargetDistance, caster, target);
			await MissileThrow(skill, caster, position, missileConfig3);
			position = GetRelativePosition(PosType.TargetDistance, caster, target);
			await MissileThrow(skill, caster, position, missileConfig3);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target);
			await MissileThrow(skill, caster, position, missileConfig3);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Throneweaver_Skill_3)]
	public class Mon_pc_summon_Throneweaver_Skill_3 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 30, width: 40);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Unknocker_Skill_1)]
	public class Mon_pc_summon_Unknocker_Skill_1 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos, distance: 80f);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 120f, 20));
			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 130, height: 1);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force070_coin#Dummy_skl1_effect02", 1.2f),
				EndEffect = new EffectConfig("I_Unknocker_skl2_mash", 1f),
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			});
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force070_coin#Dummy_skl1_effect02", 1.2f),
				EndEffect = new EffectConfig("I_Unknocker_skl2_mash", 1f),
				Range = 15f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			var delays = new[] { 250, 250, 250, 250, 250, 250, 150, 100 };
			for (var i = 0; i < 9; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 80, rand: 140);
				await MissileThrow(skill, caster, position, config);

				if (i < delays.Length)
					await skill.Wait(TimeSpan.FromMilliseconds(delays[i]));
			}
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Unknocker_Skill_2)]
	public class Mon_pc_summon_Unknocker_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 4f),
				PositionDelay = 2000,
				Effect = new EffectConfig("E_Unknocker_skl3", 0.8f),
				Range = 30f,
				KnockdownPower = 0f,
				Delay = 1800f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(3500));
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, config);
			position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 170);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Unknocker_Skill_3)]
	public class Mon_pc_summon_Unknocker_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			var position = originPos.GetRelative(farPos, distance: 30f);
			MonsterSkillPadMissileBuck(caster, skill, position, PadName.shootpad_Unknocker, 0f, 250f, 10, -35f, 50f, 100f);
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Unknocker_Skill_4)]
	public class Mon_pc_summon_Unknocker_Skill_4 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targetPos = originPos.GetRelative(farPos);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 100f, 5));
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			var position = GetRelativePosition(PosType.Target, caster, target, height: 2);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_force070_coin#Dummy_skl1_effect02", 2f),
				EndEffect = new EffectConfig("I_Unknocker_skl2_mash", 1f),
				Range = 30f,
				FlyTime = 1.2f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});
		}
	}

	[SkillHandler(SkillId.Mon_pc_summon_Unknocker_Skill_5)]
	public class Mon_pc_summon_Unknocker_Skill_5 : ITargetSkillHandler
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
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var targetHandle = target?.Handle ?? 0;

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 600;
			var damageDelay = 800;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}
}
