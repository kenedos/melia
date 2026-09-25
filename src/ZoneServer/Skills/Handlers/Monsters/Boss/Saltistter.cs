using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUseHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Yggdrasil.Util;
using Melia.Zone.Skills.Helpers;
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	/// <summary>
	/// Handler for boss_Saltistter Skill 1.
	/// Single hit with slowdown debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Saltistter_Skill_1)]
	public class Mon_boss_Saltistter_Skill_1 : ITargetSkillHandler
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), farPos);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var splashArea1 = new CircleF(originPos.GetRelative(farPos, distance: 77f, angle: -1f), 50f);
			await SkillAttack(caster, skill, splashArea1, hitDelay: 700, aniTime: 900, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 8000f, 1, 30, -1, hits);
		}
	}

	/// <summary>
	/// Handler for boss_Saltistter Skill 2.
	/// Multiple ice ground hits with slowdown debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Saltistter_Skill_2)]
	public class Mon_boss_Saltistter_Skill_2 : ITargetSkillHandler
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
			var waits = new[] { 1200, 700, 1000, 1000 };
			foreach (var wait in waits)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(wait));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				foreach (var position in GetScatteredPositions(GetLeadPosition(target, 1300, caster), 4, 150, 60))
					skill.Run(IceGroundHitWithSlowdown(skill, caster, originPos.GetNearestPositionWithinDistance(position, 250f)));
			}
		}

		private async Task IceGroundHitWithSlowdown(Skill skill, ICombatEntity caster, Position position)
		{
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1300,
				Effect = new EffectConfig("F_ground113_ice", 0.22f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 8000f, 1, 10, -1, hits);
		}
	}

	/// <summary>
	/// Handler for boss_Saltistter Skill 3.
	/// Multiple ice missile throws with slowdown debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Saltistter_Skill_3)]
	public class Mon_boss_Saltistter_Skill_3 : ITargetSkillHandler
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

			if (!caster.Position.InRange2D(target.Position, 300))
				return;

			var positions = GetScatteredPositions(GetLeadPosition(target, 700, caster), 8, 100, 40);
			for (var wave = 0; wave < 2; wave++)
			{
				if (wave > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(300));

				for (var i = 0; i < 4; i++)
				{
					var position = positions[wave * 4 + i];
					_ = MissileThrow(skill, caster, position, new MissileConfig
					{
						Effect = new EffectConfig("I_force014_ice2#Dummy_effect_02", 1f),
						EndEffect = new EffectConfig("F_explosion092_ice", 1f),
						Range = 10f,
						FlyTime = 0.7f,
						DelayTime = 0f,
						Gravity = 600f,
						Speed = 1f,
						HitTime = 1000f,
						HitCount = 1,
						GroundEffect = new EffectConfig("F_sys_target_monster", 0.3f),
					});
				}
			}
		}
	}

	/// <summary>
	/// Handler for boss_Saltistter Skill 4.
	/// Single hit with slowdown debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Saltistter_Skill_4)]
	public class Mon_boss_Saltistter_Skill_4 : ITargetSkillHandler
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
			var slam = EffectAndHit(skill, caster, originPos.GetRelative(farPos, distance: 60f, angle: -3f), new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2f),
				PositionDelay = 2000,
				Effect = EffectConfig.None,
				Range = 45f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(2500));
			caster.StartBuff(BuffId.Mon_Shield, 1f, 0f, TimeSpan.FromMilliseconds(10000f), caster);
			await slam;
			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 4000f, 1, 10, -1, hits);
		}
	}

	/// <summary>
	/// Handler for ET_boss_Saltistter_minimal Skill 1.
	/// Single hit with slowdown debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_ET_boss_Saltistter_minimal_Skill_1)]
	public class Mon_ET_boss_Saltistter_minimal_Skill_1 : ITargetSkillHandler
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), farPos);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var hits = new List<SkillHitInfo>();
			var splashParam1 = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 30, angle: 120f);
			var splashArea1 = skill.GetSplashArea(SplashType.Fan, splashParam1);
			await SkillAttack(caster, skill, splashArea1, hitDelay: 700, aniTime: 900, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 8000f, 1, 30, -1, hits);
		}
	}

	/// <summary>
	/// Handler for ET_boss_Saltistter_minimal Skill 2.
	/// Multiple ice ground hits with slowdown debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_ET_boss_Saltistter_minimal_Skill_2)]
	public class Mon_ET_boss_Saltistter_minimal_Skill_2 : ITargetSkillHandler
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
			var waits = new[] { 1200, 700, 1000, 1000 };
			foreach (var wait in waits)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(wait));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				foreach (var position in GetScatteredPositions(GetLeadPosition(target, 1300, caster), 4, 150, 60))
					skill.Run(IceGroundHitWithSlowdown(skill, caster, originPos.GetNearestPositionWithinDistance(position, 250f)));
			}
		}

		private async Task IceGroundHitWithSlowdown(Skill skill, ICombatEntity caster, Position position)
		{
			var hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1300,
				Effect = new EffectConfig("F_ground113_ice", 0.22f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_slowdown, 1, 0f, 8000f, 1, 10, -1, hits);
		}
	}

	/// <summary>
	/// Handler for ET_boss_Saltistter_minimal Skill 3.
	/// Multiple ice missile throws with slowdown debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_ET_boss_Saltistter_minimal_Skill_3)]
	public class Mon_ET_boss_Saltistter_minimal_Skill_3 : ITargetSkillHandler
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			_ = MonsterSkillFollowMovePath(caster, skill, (1100, 1f, 55f), (1400, 68f, 0f));

			await skill.Wait(TimeSpan.FromMilliseconds(1900));

			if (!caster.Position.InRange2D(target.Position, 300))
				return;

			var positions = GetScatteredPositions(GetLeadPosition(target, 700, caster), 8, 100, 40);
			for (var wave = 0; wave < 2; wave++)
			{
				if (wave > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(300));

				for (var i = 0; i < 4; i++)
				{
					var position = positions[wave * 4 + i];
					_ = MissileThrow(skill, caster, position, new MissileConfig
					{
						Effect = new EffectConfig("I_force014_ice2#Dummy_effect_02", 1f),
						EndEffect = new EffectConfig("F_explosion092_ice", 1f),
						Range = 10f,
						FlyTime = 0.7f,
						DelayTime = 0f,
						Gravity = 600f,
						Speed = 1f,
						HitTime = 1000f,
						HitCount = 1,
						GroundEffect = new EffectConfig("F_sys_target_monster", 0.3f),
					});
				}
			}
		}
	}
}
