using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using System.Linq;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Archers.QuarrelShooter
{
	/// <summary>
	/// Handler for the QuarrelShooter skill Scatter Caltrop.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.QuarrelShooter_ScatterCaltrop)]
	public class QuarrelShooterScatterCaltrop : IMeleeGroundSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, skill, targetPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position targetPos)
		{
			var hits = new List<SkillHitInfo>();
			var position = targetPos.GetRandomInRange2D(0, 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_shot_caltrops_mash#Bip01 R Hand", 0.2f),
				EndEffect = new EffectConfig("F_smoke008##1", 1f),
				Range = 10f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
			}, hits);
			position = targetPos.GetRandomInRange2D(0, 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_shot_caltrops_mash#Bip01 R Hand", 0.2f),
				EndEffect = new EffectConfig("F_smoke008##1", 1f),
				Range = 10f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
			}, hits);
			position = targetPos.GetRandomInRange2D(0, 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_shot_caltrops_mash#Bip01 R Hand", 0.2f),
				EndEffect = new EffectConfig("F_smoke008##1", 1f),
				Range = 10f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = targetPos.GetRandomInRange2D(0, 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_shot_caltrops_mash#Bip01 R Hand", 0.2f),
				EndEffect = new EffectConfig("F_smoke008##1", 1f),
				Range = 10f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
			}, hits);
			position = targetPos.GetRandomInRange2D(0, 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_shot_caltrops_mash#Bip01 R Hand", 0.2f),
				EndEffect = new EffectConfig("F_smoke008##1", 1f),
				Range = 10f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
			}, hits);
			position = targetPos.GetRandomInRange2D(0, 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_shot_caltrops_mash#Bip01 R Hand", 0.2f),
				EndEffect = new EffectConfig("F_smoke008##1", 1f),
				Range = 10f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(20));
			position = targetPos.GetRandomInRange2D(0, 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_shot_caltrops_mash#Bip01 R Hand", 0.2f),
				EndEffect = new EffectConfig("F_smoke008##1", 1f),
				Range = 10f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
			}, hits);
			position = targetPos.GetRandomInRange2D(0, 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_shot_caltrops_mash#Bip01 R Hand", 0.2f),
				EndEffect = new EffectConfig("F_smoke008##1", 1f),
				Range = 10f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
			}, hits);
			position = targetPos.GetRandomInRange2D(0, 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_shot_caltrops_mash#Bip01 R Hand", 0.2f),
				EndEffect = new EffectConfig("F_smoke008##1", 1f),
				Range = 10f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
			}, hits);
			position = targetPos.GetRandomInRange2D(0, 60);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_archer_shot_caltrops_mash#Bip01 R Hand", 0.2f),
				EndEffect = new EffectConfig("F_smoke008##1", 1f),
				Range = 10f,
				FlyTime = 0.1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
			}, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(70));
			await MissilePadThrow(skill, caster, targetPos, new MissileConfig
			{
				Effect = EffectConfig.None,
				EndEffect = EffectConfig.None,
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 0.5f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 0,
				GroundEffect = EffectConfig.None,
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			}, 0f, "ScatterCaltrop_Pad");
			if (caster.IsBuffActive(BuffId.DeployPavise_ReinforceSkill_Buff))
				SkillResultTargetBuff(caster, skill, BuffId.CriticalWound, 1, 0f, 10000f, 1, 20, -1, hits);
		}
	}
}
