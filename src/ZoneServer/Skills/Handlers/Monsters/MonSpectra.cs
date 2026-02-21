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
	[SkillHandler(SkillId.Mon_spectra_Skill_2)]
	public class Mon_spectra_Skill_2 : ITargetSkillHandler
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
			var effectHitConfig = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_explosion074_green", 1f),
				Range = 20f,
				KnockdownPower = 0f,
				Delay = 1f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			var position = originPos.GetRelative(farPos, distance: 60);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = originPos.GetRelative(farPos, distance: 60, angle: 54f);
			await EffectAndHit(skill, caster, position, effectHitConfig);
			position = originPos.GetRelative(farPos, distance: 60, angle: -54f);
			await EffectAndHit(skill, caster, position, new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
				PositionDelay = 2000,
				Effect = new EffectConfig("F_explosion074_green", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 1f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			});
			await skill.Wait(TimeSpan.FromMilliseconds(3400));
			var effectHitConfig2 = new EffectHitConfig
			{
				GroundEffect = new EffectConfig("F_sys_target_monster", 1f),
				PositionDelay = 1900,
				Effect = new EffectConfig("F_explosion074_green", 1f),
				Range = 20f,
				KnockdownPower = 100f,
				Delay = 1f,
				HitCount = 1,
				HitDuration = 1000f,
				CasterEffect = EffectConfig.None,
				CasterNodeName = "None",
				KnockType = 1,
				VerticalAngle = 60f,
				InnerRange = 0,
			};

			position = originPos.GetRelative(farPos, distance: 70, angle: 45f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			position = originPos.GetRelative(farPos, distance: 70, angle: -45f);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
			position = originPos.GetRelative(farPos, distance: 20);
			await EffectAndHit(skill, caster, position, effectHitConfig2);
		}
	}

}
