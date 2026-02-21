using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Skills.Helpers;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_HiddenTrigger6_Velcoffer_Riad_trap_Skill_1)]
	public class Mon_HiddenTrigger6_Velcoffer_Riad_trap_Skill_1 : ITargetSkillHandler
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
			var targetPos = originPos.GetRelative(farPos, distance: 0);
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, targetPos, 400f, 50));

			var missileConfig = new MissileConfig
			{
				Effect = new EffectConfig("I_SlaveStone_mash", 1.5f),
				EndEffect = new EffectConfig("I_stone014_mash", 0.1f),
				DotEffect = EffectConfig.None,
				Range = 25f,
				DelayTime = 2f,
				FlyTime = 1f,
				Height = 300f,
				Easing = 2f,
				HitTime = 1000f,
				HitCount = 1,
				HitStartFix = 0f,
				StartEasing = 0f,
				GroundEffect = EffectConfig.None,
				KnockdownPower = 0f,
				KnockType = (KnockType)1,
				VerticalAngle = 0f,
			};

			for (var i = 0; i < 20; i++)
			{
				if (i > 0)
					await skill.Wait(TimeSpan.FromMilliseconds(175));

				var position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 420, height: 5);
				await MissileFall(caster, skill, position, missileConfig);

				position = GetRelativePosition(PosType.TargetRandomDistance, caster, target, rand: 80, height: 4);
				await MissileFall(caster, skill, position, missileConfig);
			}
		}
	}

	[SkillHandler(SkillId.Mon_HiddenTrigger6_whitetrees1_Skill_1)]
	public class Mon_HiddenTrigger6_whitetrees1_Skill_1 : ITargetSkillHandler
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
			await skill.Wait(TimeSpan.FromMilliseconds(100));
			var targetPos = originPos.GetRelative(farPos, rand: 70, height: 5);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.ID_WHITETREES1_GIMMICK2_PAD);
		}
	}
}
