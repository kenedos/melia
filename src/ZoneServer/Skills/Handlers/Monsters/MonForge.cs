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
	[SkillHandler(SkillId.Mon_Forge_Skill_1)]
	public class Mon_Forge_Skill_1 : ITargetSkillHandler
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
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos)
		{
			var targets = SkillSelectEnemiesInFan(caster, farPos, 30f, 200f, 5);

			await skill.Wait(TimeSpan.FromMilliseconds(100));
			if (targets != null)
			{
				foreach (var randomTarget in targets)
				{
					var position = GetRelativePosition(PosType.TargetHeight, caster, randomTarget);
					var speed = 0.1;
					var dist = caster.GetDistance(randomTarget);
					speed *= (dist / 100);
					speed = Math.Clamp(speed, 0.05, 0.5);
					//speed = Math.Max(0.05, speed);
					//speed = Math.Min(0.5, speed);
					await MissileThrow(skill, caster, position, new MissileConfig
					{
						Effect = new EffectConfig("I_archer_canon_force_forge#Dummy002", 0.45f),
						EndEffect = new EffectConfig("I_explosion002_orange", 1f),
						Range = 15f,
						FlyTime = (float)speed,
						DelayTime = 0f,
						Gravity = 10f,
						Speed = 0.8f,
						HitTime = 500f,
						HitCount = 1,
						GroundEffect = new EffectConfig("F_sys_target_boss", 1.5f),
					});
				}
			}
		}
	}

}
