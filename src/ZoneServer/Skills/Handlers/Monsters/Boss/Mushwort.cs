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
using Yggdrasil.Util;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_mushwort_Skill_1)]
	public class Mon_boss_mushwort_Skill_1 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var config = new ArrowConfig
			{
				ArrowEffect = new EffectConfig("None", 0.8f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 0.1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("F_explosion038_violet", 0.5f),
				Range = 30f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitEffectSpacing = 25f,
				HitTimeSpacing = 0.1f,
				HitCount = 1,
				HitDuration = 80f,
			};

			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config, hits);
			startingPosition = originPos.GetRelative(farPos);
			endingPosition = originPos.GetRelative(farPos, distance: 250f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, config, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 20000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_mushwort_Skill_2)]
	public class Mon_boss_mushwort_Skill_2 : ITargetSkillHandler
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
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 150);
			await skill.Wait(TimeSpan.FromMilliseconds(2500));

			var rnd = RandomProvider.Get();
			for (var i = 0; i < 10; i++)
			{
				var angle = rnd.NextDouble() * Math.PI * 2;
				var distance = rnd.NextDouble() * 20;
				var missilePos = new Position(
					target.Position.X + (float)(Math.Cos(angle) * distance),
					target.Position.Y,
					target.Position.Z + (float)(Math.Sin(angle) * distance)
				);
				await MissileThrow(skill, caster, missilePos, new MissileConfig
				{
					Effect = new EffectConfig("I_mushwort_atk002_mash#Bip001 R Finger1Nub", 0.5f),
					EndEffect = new EffectConfig("F_explosion034_blue#1#1.5", 0.5f),
					Range = 17f,
					FlyTime = 1f,
					DelayTime = 0f,
					Gravity = 300f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = new EffectConfig("None", 1.5f),
					// TargetEffect.Name = "F_sys_target_boss##0.5",
					// TargetEffect.Scale = 1.5f,
				});
				if (i < 9)
					await skill.Wait(TimeSpan.FromMilliseconds(150));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_mushwort_Skill_3)]
	public class Mon_boss_mushwort_Skill_3 : ITargetSkillHandler
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
				GroundEffect = new EffectConfig("None", 7f),
				PositionDelay = 1000,
				Effect = new EffectConfig("I_mushwort_atk001_mash", 2.5f),
				Range = 100f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			caster.StartBuff(BuffId.Mon_Heal_Buff, 20f, 0f, TimeSpan.FromMilliseconds(1000f), caster);
			SkillResultTargetBuff(caster, skill, BuffId.Poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 10000f, 1, 40, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_mushwort_Skill_4)]
	public class Mon_boss_mushwort_Skill_4 : ITargetSkillHandler
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

			var rnd = RandomProvider.Get();
			for (var i = 0; i < 14; i++)
			{
				for (var j = 0; j < 3; j++)
				{
					var angle = rnd.NextDouble() * Math.PI * 2;
					var distance = rnd.NextDouble() * 300;
					var missilePos = new Position(
						originPos.X + (float)(Math.Cos(angle) * distance),
						originPos.Y,
						originPos.Z + (float)(Math.Sin(angle) * distance)
					);
					skill.Run(MissileFall(caster, skill, missilePos, new MissileConfig
					{
						Effect = new EffectConfig("I_mushwort_atk002_mash", 0.5f),
						EndEffect = new EffectConfig("F_explosion034_blue", 0.5f),
						DotEffect = EffectConfig.None,
						Range = 15f,
						DelayTime = 0.5f,
						FlyTime = 1.5f,
						Height = 300f,
						Easing = 2f,
						HitTime = 1000f,
						HitCount = 1,
						HitStartFix = 0f,
						StartEasing = 0f,
						GroundEffect = new EffectConfig("F_sys_target_boss##0.5", 1.5f),
					}));
				}
				if (i < 13)
					await skill.Wait(TimeSpan.FromMilliseconds(400));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_mushwort_Skill_5)]
	public class Mon_boss_mushwort_Skill_5 : ITargetSkillHandler
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
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 200);
			await skill.Wait(TimeSpan.FromMilliseconds(2900));

			var rnd = RandomProvider.Get();
			for (var waves = 0; waves < 2; waves++)
			{
				for (var i = 0; i < 6; i++)
				{
					for (var j = 0; j < 2; j++)
					{
						var angle = rnd.NextDouble() * Math.PI * 2;
						var distance = 20 + rnd.NextDouble() * 40;
						var missilePos = new Position(
							target.Position.X + (float)(Math.Cos(angle) * distance),
							target.Position.Y,
							target.Position.Z + (float)(Math.Sin(angle) * distance)
						);
						skill.Run(MissileThrow(skill, caster, missilePos, new MissileConfig
						{
							Effect = new EffectConfig("I_mushwort_atk002_mash#Bip001 R Finger1Nub", 0.5f),
							EndEffect = new EffectConfig("F_explosion034_blue#1#1.5", 0.5f),
							Range = 17f,
							FlyTime = 1.3f,
							DelayTime = 0f,
							Gravity = 300f,
							Speed = 1f,
							HitTime = 1000f,
							HitCount = 1,
							GroundEffect = new EffectConfig("None", 1.5f),
							// TargetEffect.Name = "F_sys_target_boss##0.5",
							// TargetEffect.Scale = 1.5f,
						}));
					}
					if (i < 5)
						await skill.Wait(TimeSpan.FromMilliseconds(200));
				}
				await skill.Wait(TimeSpan.FromMilliseconds(1200));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_mushwort_Skill_6)]
	public class Mon_boss_mushwort_Skill_6 : ITargetSkillHandler
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
			var targetPos = originPos.GetNearestPositionWithinDistance(target.Position, 180);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));

			var rnd = RandomProvider.Get();
			for (var waves = 0; waves < 2; waves++)
			{
				for (var i = 0; i < 6; i++)
				{
					for (var j = 0; j < 2; j++)
					{
						var angle = rnd.NextDouble() * Math.PI * 2;
						var distance = 20 + rnd.NextDouble() * 40;
						var missilePos = new Position(
							target.Position.X + (float)(Math.Cos(angle) * distance),
							target.Position.Y,
							target.Position.Z + (float)(Math.Sin(angle) * distance)
						);
						skill.Run(MissileThrow(skill, caster, missilePos, new MissileConfig
						{
							Effect = new EffectConfig("I_mushwort_atk002_mash#Bip001 Neck", 1.2f),
							EndEffect = new EffectConfig("F_explosion034_blue#1#1.5", 0.5f),
							Range = 40f,
							FlyTime = 1.3f,
							DelayTime = 0f,
							Gravity = 600f,
							Speed = 1f,
							HitTime = 1000f,
							HitCount = 1,
							GroundEffect = new EffectConfig("None", 3.5f),
							// TargetEffect.Name = "F_sys_target_boss##0.5",
							// TargetEffect.Scale = 1.5f,
						}));
					}
					if (i < 5)
						await skill.Wait(TimeSpan.FromMilliseconds(250));
				}
				await skill.Wait(TimeSpan.FromMilliseconds(3000));
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_mushwort_Skill_7)]
	public class Mon_boss_mushwort_Skill_7 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3200));

			var rnd = RandomProvider.Get();
			for (var i = 0; i < 9; i++)
			{
				for (var j = 0; j < 3; j++)
				{
					var angle = rnd.NextDouble() * Math.PI * 2;
					var distance = 20 + rnd.NextDouble() * 40;
					var missilePos = new Position(
						target.Position.X + (float)(Math.Cos(angle) * distance),
						target.Position.Y,
						target.Position.Z + (float)(Math.Sin(angle) * distance)
					);
					skill.Run(MissileThrow(skill, caster, missilePos, new MissileConfig
					{
						Effect = new EffectConfig("I_mushwort_atk002_mash#Bip001 R Finger1Nub", 0.5f),
						EndEffect = new EffectConfig("F_explosion034_blue#1#1.5", 0.5f),
						Range = 17f,
						FlyTime = 1.3f,
						DelayTime = 0f,
						Gravity = 300f,
						Speed = 1f,
						HitTime = 1000f,
						HitCount = 1,
						GroundEffect = new EffectConfig("None", 1.5f),
						// TargetEffect.Name = "F_sys_target_boss##0.5",
						// TargetEffect.Scale = 1.5f,
					}));
				}
				if (i < 5)
					await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
		}
	}
}
