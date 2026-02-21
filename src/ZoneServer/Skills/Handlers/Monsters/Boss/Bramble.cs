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
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	/// <summary>
	/// Handler for boss_bramble Attack 1.
	/// Single wave targeted AoE attack.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_bramble_Attack1)]
	public class Mon_boss_bramble_Attack1 : ITargetSkillHandler
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

			for (var i = 0; i < 6; i++)
			{
				var position = GetRelativePosition(PosType.Target, caster, target, rand: 120, height: 2);
				_ = EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("F_sys_target_monster", 0.5f),
					PositionDelay = 2250,
					Effect = new EffectConfig("I_bramble_obj_atk001_mash", 0.8f),
					Range = 20f,
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
				await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
		}
	}

	/// <summary>
	/// Handler for boss_bramble Skill 1.
	/// Multiple targeted AoE attacks on enemies.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_bramble_Skill_1)]
	public class Mon_boss_bramble_Skill_1 : ITargetSkillHandler
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

			// Wave 1
			for (var i = 0; i < 6; i++)
			{
				var position = GetRelativePosition(PosType.Target, caster, target, rand: 120, height: 2);
				_ = EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("F_sys_target_monster", 0.5f),
					PositionDelay = 2250,
					Effect = new EffectConfig("I_bramble_obj_atk001_mash", 0.8f),
					Range = 20f,
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
				await skill.Wait(TimeSpan.FromMilliseconds(200));
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1300));

			// Wave 2
			for (var i = 0; i < 6; i++)
			{
				var position = GetRelativePosition(PosType.Target, caster, target, rand: 120, height: 2);
				_ = EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("F_sys_target_monster", 0.5f),
					PositionDelay = 2250,
					Effect = new EffectConfig("I_bramble_obj_atk001_mash", 0.8f),
					Range = 20f,
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
				await skill.Wait(TimeSpan.FromMilliseconds(200));
			}

			await skill.Wait(TimeSpan.FromMilliseconds(1300));

			// Wave 3
			for (var i = 0; i < 5; i++)
			{
				var position = GetRelativePosition(PosType.Target, caster, target, rand: 120, height: 2);
				_ = EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("F_sys_target_monster", 0.5f),
					PositionDelay = 2250,
					Effect = new EffectConfig("I_bramble_obj_atk001_mash", 0.8f),
					Range = 20f,
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
				await skill.Wait(TimeSpan.FromMilliseconds(200));
			}
		}
	}

	/// <summary>
	/// Handler for boss_bramble Skill 2.
	/// Circle AoE attack with summoning.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_bramble_Skill_2)]
	public class Mon_boss_bramble_Skill_2 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 120, width: 50, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1700;
			var damageDelay = 1900;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			var spawnPos = originPos.GetRelative(farPos, distance: 120);
			MonsterSkillCreateMob(skill, caster, "monskill_bramble_obj", spawnPos, 0f, "", "", 0, 0f, "Bram_test", "");
		}
	}

	/// <summary>
	/// Handler for boss_bramble Skill 3.
	/// Large circle AoE attack.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_bramble_Skill_3)]
	public class Mon_boss_bramble_Skill_3 : ITargetSkillHandler
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

			// Wave 1
			for (var i = 0; i < 5; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 30, height: 2);
				_ = EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("F_sys_target_monster", 0.5f),
					PositionDelay = 2250,
					Effect = new EffectConfig("I_bramble_obj_atk001_mash", 0.8f),
					Range = 20f,
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
				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}

			await skill.Wait(TimeSpan.FromMilliseconds(2000));

			// Wave 2
			for (var i = 0; i < 5; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 30, height: 2);
				_ = EffectAndHit(skill, caster, position, new EffectHitConfig
				{
					GroundEffect = new EffectConfig("F_sys_target_monster", 0.5f),
					PositionDelay = 2250,
					Effect = new EffectConfig("I_bramble_obj_atk001_mash", 0.8f),
					Range = 20f,
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
				await skill.Wait(TimeSpan.FromMilliseconds(100));
			}
		}
	}

	/// <summary>
	/// Handler for boss_bramble Skill 4.
	/// Multiple arrow projectiles.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_bramble_Skill_4)]
	public class Mon_boss_bramble_Skill_4 : ITargetSkillHandler
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

			// Fire all 6 arrows simultaneously
			var startingPosition = originPos.GetRelative(farPos, distance: 129.33676f);
			_ = EffectHitArrow(skill, caster, startingPosition, originPos.GetRelative(farPos, distance: 250f), new ArrowConfig
			{
				ArrowEffect = new EffectConfig("I_force042_violet##0.5", 0.5f),
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.05f,
				ArrowLifeTime = 0.5f,
				PositionDelay = 1700f,
				HitEffect = new EffectConfig("E_bramble_obj_atk001_mash2##0.5", 1f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.05f,
				HitCount = 1,
				HitDuration = 1000f,
			});
			//_ = EffectHitArrow(skill, caster, startingPosition, originPos.GetRelative(farPos, distance: 150f), "I_force042_violet##0.5", 0.5f, 25f, 0.05f, 0.5f, 1700f, "E_bramble_obj_atk001_mash2##0.5", 1f, 20f, 0f, 0f, 30f, 0.05f, 1, 1000f);
			//_ = EffectHitArrow(skill, caster, startingPosition, originPos.GetRelative(farPos, distance: 150f), "I_force042_violet##0.5", 0.5f, 25f, 0.05f, 0.5f, 1700f, "E_bramble_obj_atk001_mash2##0.5", 1f, 20f, 0f, 0f, 30f, 0.05f, 1, 1000f);
			//_ = EffectHitArrow(skill, caster, startingPosition, originPos.GetRelative(farPos, distance: 150f), "I_force042_violet##0.5", 0.5f, 25f, 0.05f, 0.5f, 1700f, "E_bramble_obj_atk001_mash2##0.5", 1f, 20f, 0f, 0f, 30f, 0.05f, 1, 1000f);
			//_ = EffectHitArrow(skill, caster, startingPosition, originPos.GetRelative(farPos, distance: 250f), "I_force042_violet##0.5", 0.5f, 25f, 0.05f, 0.5f, 1700f, "E_bramble_obj_atk001_mash2##0.5", 1f, 20f, 0f, 0f, 30f, 0.05f, 1, 1000f);
			//_ = EffectHitArrow(skill, caster, startingPosition, originPos.GetRelative(farPos, distance: 250f), "I_force042_violet##0.5", 0.5f, 25f, 0.05f, 0.5f, 1700f, "E_bramble_obj_atk001_mash2##0.5", 1f, 20f, 0f, 0f, 30f, 0.05f, 1, 1000f);
		}
	}

	/// <summary>
	/// Handler for boss_bramble Skill 5.
	/// Two-hit AoE attack.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_bramble_Skill_5)]
	public class Mon_boss_bramble_Skill_5 : ITargetSkillHandler
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
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 50f,
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

			var position = originPos.GetRelative(farPos, distance: 120f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			position = originPos.GetRelative(farPos, distance: 120f);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	/// <summary>
	/// Handler for boss_bramble Skill 6.
	/// Expanding AoE attack with increasing radius.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_bramble_Skill_6)]
	public class Mon_boss_bramble_Skill_6 : ITargetSkillHandler
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

			var position = originPos.GetRelative(farPos);
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 10f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(350));
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 80f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 40f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(350));
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 100f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 60f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(350));
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 120f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 80f,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(350));
			_ = EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = EffectConfig.None,
				Range = 150f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 0f,
				InnerRange = 100f,
			});
		}
	}
}
