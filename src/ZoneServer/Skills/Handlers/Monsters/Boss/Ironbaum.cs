using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Shared.Util;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUseHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	/// <summary>
	/// Handler for boss_Ironbaum Skill 1.
	/// Circle AoE attack.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Ironbaum_Skill_1)]
	public class Mon_boss_Ironbaum_Skill_1 : ITargetSkillHandler
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
			ISplashArea splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 59f), 40f);
			var hitDelay = 1400;
			var aniTime = 1600;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}

	/// <summary>
	/// Handler for boss_Ironbaum Skill 2.
	/// Creates fire pad at target location.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Ironbaum_Skill_2)]
	public class Mon_boss_Ironbaum_Skill_2 : ITargetSkillHandler
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

			var pos1 = originPos.GetRelative(farPos, distance: 30);
			var pos2 = originPos.GetRelative(farPos, distance: 90);
			var pos3 = originPos.GetRelative(farPos, distance: 150);

			SkillCreatePad(caster, skill, pos1, 0f, PadName.Ironbaum_fire2);
			SkillCreatePad(caster, skill, pos2, 0f, PadName.Ironbaum_fire2);
			SkillCreatePad(caster, skill, pos3, 0f, PadName.Ironbaum_fire2);

			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			SkillRemovePad(caster, skill);
		}
	}

	/// <summary>
	/// Handler for boss_Ironbaum Skill 3.
	/// Multiple fire missile throws.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Ironbaum_Skill_3)]
	public class Mon_boss_Ironbaum_Skill_3 : ITargetSkillHandler
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
			var config = new MissileConfig
			{
				Effect = new EffectConfig("I_force054_fire#Bip001 Neck", 1f),
				EndEffect = new EffectConfig("F_ground014_fire", 0.2f),
				DotEffect = EffectConfig.None,
				Range = 10f,
				FlyTime = 0.8f,
				DelayTime = 0f,
				Gravity = 300f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.5f),
				GroundDelay = 0f,
				EffectMoveDelay = 0f,
			};

			var waveCounts = new[] { 4, 4, 4, 4, 6 };
			for (var wave = 0; wave < waveCounts.Length; wave++)
			{
				if (wave > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(1000));
				if (!caster.Position.InRange2D(target.Position, 300))
					break;

				foreach (var position in GetScatteredPositions(GetLeadPosition(target, 800, caster), waveCounts[wave], 120, 40))
					_ = MissilePadThrow(skill, caster, originPos.GetNearestPositionWithinDistance(position, 200f), config, 0f, "Mon_FirePilla_5");
			}
		}
	}

	/// <summary>
	/// Handler for boss_Ironbaum Skill 4.
	/// Creates fire pillars around the caster in waves.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_Ironbaum_Skill_4)]
	public class Mon_boss_Ironbaum_Skill_4 : ITargetSkillHandler
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

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(500));

			for (var wave = 0; wave < 6; wave++)
			{
				var positions = new Position[3];

				for (var i = 0; i < 3; i++)
				{
					positions[i] = caster.Position.GetRandomInRange2D(30, 200, GameRandom.Get());
					_ = caster.PlayEffectToGround("F_sys_target_boss##0.5", positions[i], 1.5f, 800);
				}

				await skill.Wait(TimeSpan.FromMilliseconds(800));

				for (var i = 0; i < 3; i++)
					SkillCreatePad(caster, skill, positions[i], 0f, PadName.Mon_FirePilla_5);

				if (wave < 5)
					await skill.Wait(TimeSpan.FromMilliseconds(300));
			}
		}
	}
}
