using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_mineloader_Skill_1)]
	public class Mon_boss_mineloader_Skill_1 : ITargetSkillHandler
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
			var startingPosition = originPos.GetRelative(farPos, distance: 25f);
			var endingPosition = caster.Map.Ground.GetLastValidPosition(originPos, originPos.GetRelative(farPos, distance: 160f));
			await EffectHitArrow(skill, caster, startingPosition, endingPosition, new ArrowConfig
			{
				ArrowEffect = EffectConfig.None,
				ArrowSpacing = 20f,
				ArrowSpacingTime = 0.01f,
				ArrowLifeTime = 1f,
				PositionDelay = 1500f,
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
				PositionDelay = 1700,
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
			//await skill.Wait(TimeSpan.FromMilliseconds(1500));
			//MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			//await skill.Wait(TimeSpan.FromMilliseconds(1700));
			//MonsterSkillSetCollisionDamage(caster, skill, false, 1f);

			caster.Position = endingPosition;
			Send.ZC_SET_POS(caster, endingPosition);
		}
	}

	[SkillHandler(SkillId.Mon_boss_mineloader_Skill_2)]
	public class Mon_boss_mineloader_Skill_2 : ITargetSkillHandler
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
			var targetPos = caster.Map.Ground.GetLastValidPosition(originPos, originPos.GetRelative(farPos, distance: 200));
			skill.Vars.Set("Melia.Pad.TargetPos", targetPos);
			SkillCreatePad(caster, skill, originPos, 0f, PadName.mineloader_laser);
			await skill.Wait(TimeSpan.FromMilliseconds(4300));
			SkillRemovePad(caster, skill);

			caster.Position = targetPos;
			Send.ZC_SET_POS(caster, targetPos);
		}
	}

	[SkillHandler(SkillId.Mon_boss_mineloader_Skill_3)]
	public class Mon_boss_mineloader_Skill_3 : ITargetSkillHandler
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
			var hits = new List<SkillHitInfo>();
			var position = originPos.GetRelative(farPos);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 13f),
				PositionDelay = 4000,
				Effect = new EffectConfig("None", 3.5f),
				Range = 180f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 5,
				HitDuration = 500f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.ElectricShock, 1, 0f, 5000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_mineloader_Skill_4)]
	public class Mon_boss_mineloader_Skill_4 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 180f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1800));
			for (var i = 0; i < 3; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 1);
				position = originPos.GetNearestPositionWithinDistance(position, 300f);
				_ = MissileThrow(skill, caster, position, new MissileConfig
				{
					Effect = new EffectConfig("I_force052_pink#Bone003", 1.5f),
					EndEffect = new EffectConfig("F_explosion024_rize", 0.8f),
					Range = 30f,
					FlyTime = 0.9f,
					DelayTime = 0f,
					Gravity = 300f,
					Speed = 1f,
					HitTime = 1000f,
					HitCount = 1,
					GroundEffect = new EffectConfig("None", 1.3f),
				});
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_mineloader_Skill_5)]
	public class Mon_boss_mineloader_Skill_5 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 180f, 20));

			for (var wave = 0; wave < 3; wave++)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(wave == 0 ? 1900 : 1800));
				for (var i = 0; i < 3; i++)
				{
					var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 1);
					position = originPos.GetNearestPositionWithinDistance(position, 300f);
					_ = MissileThrow(skill, caster, position, new MissileConfig
					{
						Effect = new EffectConfig("I_force052_pink#Bone003", 1.5f),
						EndEffect = new EffectConfig("F_explosion024_rize", 0.8f),
						Range = 30f,
						FlyTime = 0.9f,
						DelayTime = 0f,
						Gravity = 300f,
						Speed = 1f,
						HitTime = 1000f,
						HitCount = 1,
						GroundEffect = new EffectConfig("None", 1.3f),
					});
				}
			}
		}
	}
}
