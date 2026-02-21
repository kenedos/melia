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
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_Gagoyle_purple_Skill_1)]
	public class Mon_Gagoyle_purple_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(850));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 70);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 80f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Raid_Velcofer_Cnt_Debuff, 1, 0f, 0f, 3, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_Gagoyle_purple_Skill_2)]
	public class Mon_Gagoyle_purple_Skill_2 : ITargetSkillHandler
	{
		private const int CastTimeMs = 2000;

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

			if (!await MonsterCastTime(skill, caster, "Pollution Zone", CastTimeMs, target))
				return;

			await skill.Wait(TimeSpan.FromMilliseconds(1550));
			var hits = new List<SkillHitInfo>();
			var startingPosition = originPos.GetRelative(farPos);
			var endingPosition = originPos.GetRelative(farPos, distance: 320f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = new EffectConfig("I_stone015_mash", 0.3f),
				Range = 50f,
				KnockdownPower = 140f,
				Delay = 0f,
				HitEffectSpacing = 50f,
				HitTimeSpacing = 0.08f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			startingPosition = originPos.GetRelative(farPos);
			endingPosition = originPos.GetRelative(farPos, distance: 320f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = EffectConfig.None,
				Range = 50f,
				KnockdownPower = 140f,
				Delay = 0f,
				HitEffectSpacing = 50f,
				HitTimeSpacing = 0.08f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(450));
			var targetPos = originPos.GetRelative(farPos, distance: 150);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.pollution_zone);
			SkillResultTargetBuff(caster, skill, BuffId.Raid_Velcofer_Cnt_Debuff, 1, 0f, 0f, 3, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_Gagoyle_purple_Skill_3)]
	public class Mon_Gagoyle_purple_Skill_3 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 60f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 30f,
				InnerRange = 0f,
			}, hits);
			position = originPos.GetRelative(farPos);
			await PadDestruction(skill, caster, position, 20, 150f, PadName.All, "I_pattern008_explosion_mash_square", 1f, 20f, 160f, 2);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 90f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 30f,
				InnerRange = 60f,
			};

			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 120f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 30f,
				InnerRange = 90f,
			};

			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			var effectHitConfig3 = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 150f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 30f,
				InnerRange = 120f,
			};

			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig3, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(850));
			var effectHitConfig4 = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 60f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 30f,
				InnerRange = 30f,
			};

			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig4, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig3, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(650));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig4, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(50));
			position = originPos.GetRelative(farPos);
			await PadDestruction(skill, caster, position, 20, 150f, PadName.All, "I_pattern008_explosion_mash_square", 1f, 20f, 160f, 2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig3, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(250));
			position = originPos.GetRelative(farPos);
			await PadDestruction(skill, caster, position, 20, 150f, PadName.All, "I_pattern008_explosion_mash_square", 1f, 20f, 160f, 2);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig4, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, effectHitConfig3, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Raid_Velcofer_Cnt_Debuff, 1, 0f, 0f, 3, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_Gagoyle_purple_Skill_4)]
	public class Mon_Gagoyle_purple_Skill_4 : ITargetSkillHandler
	{
		private const int CastTimeMs = 1000;

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

			if (!await MonsterCastTime(skill, caster, "GravityFloor", CastTimeMs, target))
				return;

			var targetPos = originPos.GetRelative(farPos);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.gravity_floor);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			var hits = new List<SkillHitInfo>();
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 120f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 3,
				HitDuration = 600f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			for (var i = 0; i < 3; i++)
			{
				var position = originPos.GetRelative(farPos);
				await EffectAndHit(skill, caster, position, config, hits);

				if (i < 2)
					await skill.Wait(TimeSpan.FromMilliseconds(1500));
			}
			// TODO: No Implementation S_R_PULL_TARGET

			SkillResultTargetBuff(caster, skill, BuffId.Raid_Velcofer_Cnt_Debuff, 1, 0f, 0f, 3, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_Gagoyle_purple_Skill_5)]
	public class Mon_Gagoyle_purple_Skill_5 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos, distance: 40);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 40f,
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
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			var startingPosition = originPos.GetRelative(farPos, distance: 80f);
			var endingPosition = originPos.GetRelative(farPos, distance: 240f);
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 25f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 0f,
				HitEffect = EffectConfig.None,
				Range = 30f,
				KnockdownPower = 130f,
				Delay = 0f,
				HitEffectSpacing = 30f,
				HitTimeSpacing = 0.08f,
				HitCount = 1,
				HitDuration = 1000f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_deprotect_Mon, 1, 0f, 30000f, 1, 100, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Catacom_MDEF_Debuff, 1, 0f, 30000f, 1, 100, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Raid_Velcofer_Cnt_Debuff, 1, 0f, 0f, 3, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_Gagoyle_purple_Skill_6)]
	public class Mon_Gagoyle_purple_Skill_6 : ITargetSkillHandler
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
			var spawnPos = originPos.GetRelative(farPos, rand: 130, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "minivern_green_mage", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, rand: 130, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "minivern_red_warrior", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			spawnPos = originPos.GetRelative(farPos, rand: 130, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "minivern_red_warrior", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, rand: 130, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "minivern_green_mage", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			spawnPos = originPos.GetRelative(farPos, rand: 130, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "minivern_red_warrior", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
			spawnPos = originPos.GetRelative(farPos, rand: 130, height: 1);
			MonsterSkillCreateMobPC(skill, caster, "minivern_green_mage", spawnPos, 0f, "", "BasicMonster_ATK", 0, 0f, "None", "");
		}
	}

	[SkillHandler(SkillId.Mon_Gagoyle_purple_Skill_7)]
	public class Mon_Gagoyle_purple_Skill_7 : ITargetSkillHandler
	{
		private const int CastTimeMs = 1000;

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

			if (!await MonsterCastTime(skill, caster, "GargoyleCirclewave", CastTimeMs, target))
				return;

			await skill.Wait(TimeSpan.FromMilliseconds(1200));
			var targetPos = originPos.GetRelative(farPos);
			var pad = SkillCreatePad(caster, skill, targetPos, 0f, PadName.gargoyle_circlewave);
			this.OnPadTriggered(caster, skill, pad);
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			targetPos = originPos.GetRelative(farPos);
			pad = SkillCreatePad(caster, skill, targetPos, 0f, PadName.gargoyle_circlewave);
			this.OnPadTriggered(caster, skill, pad);
			await skill.Wait(TimeSpan.FromMilliseconds(800));
			targetPos = originPos.GetRelative(farPos);
			pad = SkillCreatePad(caster, skill, targetPos, 0f, PadName.gargoyle_circlewave);
			this.OnPadTriggered(caster, skill, pad);
		}

		private void OnPadTriggered(ICombatEntity caster, Skill skill, Pad pad)
		{
			if (pad == null)
				return;
			pad.Trigger.Entered += (sender, e) =>
			{
				if (e.Initiator is ICombatEntity target)
				{
					var hit = SCR_SkillHit(caster, target, skill);
					var hitInfo = new SkillHitInfo(caster, target, skill, hit);
					SkillResultTargetBuff(caster, skill, hitInfo, BuffId.UC_stun, 1, 0, 2000, 1, 100);
					SkillResultTargetBuff(caster, skill, hitInfo, BuffId.Raid_Velcofer_Cnt_Debuff, 1, 0, 0, 3, 100);
				}
			};
		}
	}

	[SkillHandler(SkillId.Mon_Gagoyle_purple_Skill_8)]
	public class Mon_Gagoyle_purple_Skill_8 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 10));
			caster.StartBuff(BuffId.Mon_invincible, 1f, 0f, TimeSpan.FromMilliseconds(5000f), caster);
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var hits = new List<SkillHitInfo>();
			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_black3#Bip001 L Finger0Nub", 1.5f),
				EndEffect = new EffectConfig("F_explosion99_dark1", 2f),
				Range = 20f,
				FlyTime = (2.0f + (float)RandomProvider.Get().NextDouble() * 0.5f),
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			var missileConfig2 = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_black4#Bip001 L Finger0Nub", 1.5f),
				EndEffect = new EffectConfig("F_explosion99_dark2", 2f),
				Range = 20f,
				FlyTime = (2.0f + (float)RandomProvider.Get().NextDouble() * 0.5f),
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			var missileConfig3 = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_black3#Bip001 R Finger0Nub", 1.5f),
				EndEffect = new EffectConfig("F_explosion99_dark1", 2f),
				Range = 20f,
				FlyTime = (2.0f + (float)RandomProvider.Get().NextDouble() * 0.5f),
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig3, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			var missileConfig4 = new MissileConfig
			{
				Effect = new EffectConfig("I_force018_trail_black4#Bip001 R Finger0Nub", 1.5f),
				EndEffect = new EffectConfig("F_explosion99_dark2", 2f),
				Range = 20f,
				FlyTime = (2.0f + (float)RandomProvider.Get().NextDouble() * 0.5f),
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 1.5f),
			};

			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig4, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig3, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig4, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig3, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig4, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig3, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig4, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig2, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig3, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig4, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig3, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig4, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig3, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig4, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig3, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig4, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(150));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 140);
			await MissileThrow(skill, caster, position, missileConfig3, hits);
			OnSkillHit(caster, skill, hits);
		}

		private void OnSkillHit(ICombatEntity caster, Skill skill, List<SkillHitInfo> hits)
		{
			SkillResultTargetBuff(caster, skill, BuffId.UC_mspblood, 1, 1000f, 6000f, 1, 200, -1, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Raid_Velcofer_Cnt_Debuff, 1, 0f, 0f, 2, 100, -1, hits);
		}
	}
}
