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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Nuaelle_Skill_1)]
	public class Mon_boss_Nuaelle_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 45, width: 40, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1000;
			var damageDelay = 1200;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 8000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Nuaelle_Skill_2)]
	public class Mon_boss_Nuaelle_Skill_2 : ITargetSkillHandler
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

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke004#Bip001 L Finger3", 0.5f),
				EndEffect = new EffectConfig("F_explosion033_orange", 0.6f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 100f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			};

			var firstConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke004_3#Bip001 L Finger3", 0.5f),
				EndEffect = new EffectConfig("F_explosion033_orange", 0.6f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 100f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			};

			var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 80, height: 1);
			await MissilePadThrow(skill, caster, position, firstConfig, 0f, "Monster_Sleep");

			for (var i = 0; i < 3; i++)
			{
				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 80, height: 1);
				await MissilePadThrow(skill, caster, position, config, 0f, "Monster_Sleep");
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Nuaelle_Skill_3)]
	public class Mon_boss_Nuaelle_Skill_3 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 40);
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 100,
				Effect = new EffectConfig("F_spread_out004_dark", 1.2f),
				Range = 90f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 60f,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 6000f, 1, 100, -1, hits);
			position = originPos.GetRelative(farPos, distance: 40, height: 2);
			hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_smoke004_4_grey#Bip001 L Hand", 1.2f),
				EndEffect = new EffectConfig("F_explosion033_orange##0.5", 1f),
				Range = 50f,
				FlyTime = 0.2f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 500f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_blind, 1, 0f, 6000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Nuaelle_Skill_4)]
	public class Mon_boss_Nuaelle_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(4700));

			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke004_4_grey#Bip001 R Finger0Nub", 1f),
				EndEffect = new EffectConfig("F_explosion033_orange", 0.3f),
				Range = 20f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var angles = new float[] { 45f, 54f, 74f, 95f, 110f, 130f, 139f, 159f, 180f, -169f, -159f, -144f, -110f, -95f, -79f, -60f, -45f, -30f, -19f, 30f };

			for (var i = 0; i < angles.Length; i++)
			{
				var position = originPos.GetRelative(farPos, distance: 80, angle: angles[i]);
				await MissileThrow(skill, caster, position, config);
				await skill.Wait(TimeSpan.FromMilliseconds(50));
			}

			var lastPosition = originPos.GetRelative(farPos, distance: 80);
			await MissileThrow(skill, caster, lastPosition, config);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Nuaelle_Skill_5)]
	public class Mon_boss_Nuaelle_Skill_5 : ITargetSkillHandler
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
			var farPos = originPos;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(4700));

			var spreadConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_smoke004_4_grey#Dummy_effect_hand_R", 1f),
				EndEffect = new EffectConfig("F_burstup029_smoke_grey", 1f),
				Range = 50f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			};

			var angles = new float[] { 45f, 54f, 74f, 95f, 110f, 130f, 139f, 159f, 180f, -169f, -159f, -144f, -110f, -95f, -79f, -60f, -45f, -30f, -19f, 30f };

			for (var i = 0; i < angles.Length; i++)
			{
				var position = originPos.GetRelative(farPos, distance: 80, angle: angles[i]);
				await MissileThrow(skill, caster, position, spreadConfig);
				await skill.Wait(TimeSpan.FromMilliseconds(50));
			}

			var lastPosition = originPos.GetRelative(farPos, distance: 80);
			await MissileThrow(skill, caster, lastPosition, spreadConfig);

			await skill.Wait(TimeSpan.FromMilliseconds(1600));
			var bigPosition = originPos.GetRelative(farPos, distance: 40, height: 2);
			await MissileThrow(skill, caster, bigPosition, new MissileConfig
			{
				Effect = new EffectConfig("I_smoke004_4_grey#Dummy_effect_skl2", 1.5f),
				EndEffect = new EffectConfig("F_explosion033_orange##0.5", 1.5f),
				Range = 50f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 0f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});

			await skill.Wait(TimeSpan.FromMilliseconds(700));
			var hitPosition = originPos.GetRelative(farPos, distance: 40);
			await EffectAndHit(skill, caster, hitPosition, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2f),
				PositionDelay = 1000,
				Effect = new EffectConfig("None", 1.5f),
				Range = 90f,
				KnockdownPower = 150f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 60f,
			});
		}
	}
}
