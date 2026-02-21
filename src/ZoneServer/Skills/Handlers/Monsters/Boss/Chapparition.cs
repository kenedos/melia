using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Shared.World;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Monsters.Boss
{
	[SkillHandler(SkillId.Mon_boss_Chapparition_Skill_1)]
	public class Mon_boss_Chapparition_Skill_1 : ITargetSkillHandler
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
			var position = originPos.GetRelative(farPos, distance: 0, angle: 0f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2.3f),
				PositionDelay = 500,
				Effect = EffectConfig.None,
				Range = 60f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Chapparition_Skill_2)]
	public class Mon_boss_Chapparition_Skill_2 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1000));
			var position = originPos.GetRelative(farPos, distance: 50);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 5f),
				PositionDelay = 1100,
				Effect = new EffectConfig("F_fire003_violet", 1f),
				Range = 55f,
				KnockdownPower = 0f,
				Delay = 0f,
				HitCount = 5,
				HitDuration = 3000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			position = originPos.GetRelative(farPos, distance: 50);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2f),
				PositionDelay = 2100,
				Effect = new EffectConfig("F_burstup002_dark", 2f),
				Range = 55f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 3000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 50);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("None", 2f),
				PositionDelay = 2100,
				Effect = new EffectConfig("I_stone009_mash", 1f),
				Range = 55f,
				KnockdownPower = 150f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 3000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 4,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
		}
	}

	[SkillHandler(SkillId.Mon_boss_Chapparition_Skill_3)]
	public class Mon_boss_Chapparition_Skill_3 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(1900));
			var hits = new List<SkillHitInfo>();

			var hitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_spread_in009_rize##0.5", 0.5f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_rize006", 0.6f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var delays = new[] { 100, 100, 200, 200, 100, 100 };

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfig, hits);

			foreach (var delay in delays)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(delay));
				position = GetRelativePosition(PosType.TargetDistance, caster, target, rand: 100, height: 1);
				position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
				await EffectAndHit(skill, caster, position, hitConfig, hits);
			}

			SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}

	[SkillHandler(SkillId.Mon_boss_Chapparition_Skill_4)]
	public class Mon_boss_Chapparition_Skill_4 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(3100));

			for (var i = 0; i < 5; i++)
			{
				var spawnPos = originPos.GetRelative(farPos, distance: 0, angle: -121f, rand: 90);
				MonsterSkillCreateMob(skill, caster, "Shredded_summon", spawnPos, 0f, "", "BasicMonster_ATK", -5, 60f, "None", "");
			}
		}
	}

	[SkillHandler(SkillId.Mon_boss_Chapparition_Skill_5)]
	public class Mon_boss_Chapparition_Skill_5 : ITargetSkillHandler
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
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 180f, 20));
			await skill.Wait(TimeSpan.FromMilliseconds(2700));
			var hits = new List<SkillHitInfo>();

			var hitConfigKnock = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_spread_in009_rize##0.5", 0.5f),
				PositionDelay = 2300,
				Effect = new EffectConfig("F_rize006", 0.5f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var hitConfigNoKnock = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_spread_in009_rize##0.5", 0.5f),
				PositionDelay = 2300,
				Effect = new EffectConfig("F_rize006", 0.5f),
				Range = 25f,
				KnockdownPower = 0f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = GetRelativePosition(PosType.TargetDistance, caster, target, distance: 2.5294259, angle: 134f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigKnock, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = GetRelativePosition(PosType.TargetDistance, caster, target);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigKnock, hits);
			position = GetRelativePosition(PosType.TargetDistance, caster, target);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigNoKnock, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigNoKnock, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.6464868, angle: 117f, rand: 190, height: 1);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigKnock, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigNoKnock, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigNoKnock, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigNoKnock, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigNoKnock, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigNoKnock, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(600));
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigKnock, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigKnock, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(500));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigKnock, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 2.0491724, angle: 157f, rand: 200);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigKnock, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigKnock, hits);
			position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigKnock, hits);
			await skill.Wait(TimeSpan.FromMilliseconds(400));
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigKnock, hits);
			position = GetRelativePosition(PosType.TargetHeight, caster, target, distance: 2.0491724, angle: 157f, rand: 180);
			position = originPos.GetNearestPositionWithinDistance(position, skill.Properties[PropertyName.MaxR]);
			await EffectAndHit(skill, caster, position, hitConfigKnock, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_curse, 1, 0f, 6000f, 1, 10, -1, hits);
		}
	}
}
