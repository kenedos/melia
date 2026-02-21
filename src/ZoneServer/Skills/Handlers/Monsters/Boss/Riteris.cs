using System;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_Riteris_Skill_1)]
	public class Mon_Riteris_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(1300);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 50, width: 30, angle: 30f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 1100;
			var damageDelay = 1300;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}

	[SkillHandler(SkillId.Mon_Riteris_Skill_2)]
	public class Mon_Riteris_Skill_2 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target.Handle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1300));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("I_bomb002_blue##0.3", 3f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 73.848824f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 94.866814f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 103.28733f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 93.473053f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 68.325783f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 57.396526f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 78.402107f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(200));
			position = originPos.GetRelative(farPos, distance: 82.529999f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("F_explosion011_blue##0.3", 2f),
				Range = 50f,
				KnockdownPower = 100f,
				Delay = 0f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			});
		}
	}

	[SkillHandler(SkillId.Mon_Riteris_Skill_3)]
	public class Mon_Riteris_Skill_3 : ITargetSkillHandler
	{
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(1500));
			var config = new EffectHitConfig
			{
				GroundEffect = EffectConfig.None,
				PositionDelay = 1000,
				Effect = new EffectConfig("I_bomb002_blue", 1f),
				Range = 25f,
				KnockdownPower = 100f,
				Delay = 200f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 0f,
				InnerRange = 0f,
			};

			var position = originPos.GetRelative(farPos, distance: 38.241978f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 77.199768f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 58.235065f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 80.819176f);
			await EffectAndHit(skill, caster, position, config);
			position = originPos.GetRelative(farPos, distance: 49.115044f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 54.272079f);
			await EffectAndHit(skill, caster, position, config);
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			position = originPos.GetRelative(farPos, distance: 72.766518f);
			await EffectAndHit(skill, caster, position, config);
		}
	}

	[SkillHandler(SkillId.Mon_Riteris_Skill_4)]
	public class Mon_Riteris_Skill_4 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2500);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 30, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 2300;
			var damageDelay = 2500;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
		}
	}
}
