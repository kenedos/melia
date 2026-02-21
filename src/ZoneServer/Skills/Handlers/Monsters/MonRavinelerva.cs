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
using System.Linq;
using Melia.Zone.Skills.Helpers;


namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_RavineLerva_Skill_1)]
	public class Mon_RavineLerva_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.Zero;
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 0, width: 0, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 0;
			var damageDelay = 0;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			await skill.Wait(TimeSpan.FromMilliseconds(700));

			// Fire all 3 missiles together with random positions around target
			for (var i = 0; i < 3; i++)
			{
				var position = GetRelativePosition(PosType.TargetRandom, caster, target, rand: 40, height: 1);
				position = originPos.GetNearestPositionWithinDistance(position, 300f);
				_ = FireMissileWithPoison(skill, caster, position);
			}
		}

		private async Task FireMissileWithPoison(Skill skill, ICombatEntity caster, Position position)
		{
			var hits = new List<SkillHitInfo>();
			await MissileThrow(skill, caster, position, new MissileConfig
			{
				Effect = new EffectConfig("I_smoke048_true#Dummy002", 0.6f),
				EndEffect = new EffectConfig("I_explosion002_green", 1f),
				Range = 10f,
				FlyTime = 0.8f,
				DelayTime = 0f,
				Gravity = 1000f,
				Speed = 1f,
				HitTime = 1000f,
				HitCount = 1,
				GroundEffect = new EffectConfig("None", 0.5f),
			}, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 12000f, 1, 100, -1, hits);
		}
	}

}
