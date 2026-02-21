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
using static Melia.Zone.Skills.Helpers.SkillUseHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	/// <summary>
	/// Handler for boss_kurmis Skill 1.
	/// Circle AoE attack.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_kurmis_Skill_1)]
	public class Mon_boss_kurmis_Skill_1 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 60, angle: 90f);
			var splashArea = skill.GetSplashArea(SplashType.Fan, splashParam);
			var hitDelay = 900;
			var damageDelay = 1100;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	/// <summary>
	/// Handler for boss_kurmis Skill 2.
	/// Circle AoE attack with stun debuff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_kurmis_Skill_2)]
	public class Mon_boss_kurmis_Skill_2 : ITargetSkillHandler
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 90, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hitDelay = 2100;
			var damageDelay = 2300;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Stun, 1, 0f, 4000f, 1, 50, -1, hits);
		}
	}

	/// <summary>
	/// Handler for boss_kurmis Skill 3.
	/// Multiple missile throws targeting enemies.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_kurmis_Skill_3)]
	public class Mon_boss_kurmis_Skill_3 : ITargetSkillHandler
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

			for (var i = 0; i < 9; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(100));

				var position = GetRelativePosition(PosType.TargetHeight, caster, target);
				var hitTime = i == 0 ? 0f : 1000f;
				_ = MissileThrow(skill, caster, position, new MissileConfig
				{
					Effect = new EffectConfig("I_force015_white#petal_11", 2f),
					EndEffect = new EffectConfig("I_explosion014_white#petal_11", 1.5f),
					Range = 10f,
					FlyTime = 1f,
					DelayTime = 0f,
					Gravity = 600f,
					Speed = 1f,
					HitTime = hitTime,
					HitCount = 1,
					GroundEffect = EffectConfig.None,
				});
			}
		}
	}

	/// <summary>
	/// Handler for boss_kurmis Skill 4.
	/// Self-heal buff.
	/// </summary>
	[SkillHandler(SkillId.Mon_boss_kurmis_Skill_4)]
	public class Mon_boss_kurmis_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(2000));
			var matk = caster.Properties.GetFloat(PropertyName.MINMATK) + caster.Properties.GetFloat(PropertyName.MAXMATK) / 2;
			caster.StartBuff(BuffId.Mon_Heal_Buff, skill.Level, matk, TimeSpan.FromMilliseconds(5000f), caster);
		}
	}
}
