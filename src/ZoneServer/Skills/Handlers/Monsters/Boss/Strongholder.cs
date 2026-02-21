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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Strongholder_Skill_1)]
	public class Mon_boss_Strongholder_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 20f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2.5f),
				PositionDelay = 0,
				Effect = EffectConfig.None,
				Range = 35f,
				KnockdownPower = 160f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 3,
				VerticalAngle = 30f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Strongholder_Skill_2)]
	public class Mon_boss_Strongholder_Skill_2 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 130f, 50));
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var position = originPos.GetRelative(farPos, distance: 60);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 0.5f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup008_smoke", 1f),
				Range = 35f,
				KnockdownPower = 230f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			var hits = new List<SkillHitInfo>();
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_smoke129_spreadout##0.8", 0.5f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup008_smoke", 1f),
				Range = 35f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			};

			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Strongholder_Skill_3)]
	public class Mon_boss_Strongholder_Skill_3 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 10, angle: -2f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 3f),
				PositionDelay = 1300,
				Effect = EffectConfig.None,
				Range = 40f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			}, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Stun, 1, 0f, 3000f, 1, 40, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Strongholder_Skill_4)]
	public class Mon_boss_Strongholder_Skill_4 : ITargetSkillHandler
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
			caster.StartBuff(BuffId.Rage_Rockto_def, 1f, 0f, TimeSpan.FromMilliseconds(10000f), caster);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Strongholder_Skill_5)]
	public class Mon_boss_Strongholder_Skill_5 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 12.135043f, angle: -50f);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 150f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = GetRelativePosition(PosType.TargetHeight, caster, target, rand: 160);
			var hits = new List<SkillHitInfo>();
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			var config = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_smoke129_spreadout##0.8", 0.5f),
				PositionDelay = 1000,
				Effect = new EffectConfig("F_burstup008_smoke", 1f),
				Range = 35f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0,
			};

			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 10, height: 2);
			position = position.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			hits = new List<SkillHitInfo>();
			await EffectAndHit(skill, caster, position, config, hits);
			SkillResultKnockTarget(caster, skill, KnockType.KnockDown, KnockDirection.TowardsTarget, 180, 30, 10, 1, 5, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_debrave, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}
}
