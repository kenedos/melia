using System;
using System.Threading.Tasks;
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
	[SkillHandler(SkillId.Mon_Dynamite_Skill_1)]
	public class Mon_Dynamite_Skill_1 : ITargetSkillHandler
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

			Send.ZC_SKILL_FORCE_TARGET(caster, target, skill);

			var originPos = caster.Position;
			var farPos = originPos.GetNearestPositionWithinDistance(target.Position, skill.Properties[PropertyName.MaxR]);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			caster.SetTargets(SkillSelectEnemiesInCircle(caster, farPos, 120f, 5));
			await skill.Wait(TimeSpan.FromMilliseconds(900));
			var position = GetRelativePosition(PosType.TargetHeight, caster, target);
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("E_Dynamite#Bip001 R Finger01", 0.7f),
				EndEffect = new EffectConfig("F_explosion072_fire", 1.5f),
				Range = 10f,
				FlyTime = 1f,
				DelayTime = 0f,
				Gravity = 600f,
				Speed = 1f,
				HitTime = 0f,
				HitCount = 1,
				GroundEffect = EffectConfig.None,
			});
		}
	}
}
