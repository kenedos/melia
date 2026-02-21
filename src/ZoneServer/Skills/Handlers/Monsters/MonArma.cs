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
using Yggdrasil.Geometry.Shapes;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using System.Linq;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_arma_Skill_1)]
	public class Mon_arma_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(900);
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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 35, width: 15, angle: 25f);
			var splashArea = skill.GetSplashArea(SplashType.Circle, splashParam);
			var hitDelay = 900;
			var damageDelay = 900;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay, hits);
			SkillResultTargetBuff(caster, skill, BuffId.Fire, 1, hits.Sum(h => h.HitInfo.Damage) * 0.5f, 5000f, 1, 100, -1, hits);
		}
	}

	[SkillHandler(HandlerPriority.Override, SkillId.Mon_arma_Skill_2)]
	public class Mon_arma_Skill_2Override : ParametersOnlySkill
	{
		protected override TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2000);
		protected override TimeSpan HitDelay { get; } = TimeSpan.FromMilliseconds(1800);
		protected override SplashType SplashType { get; } = SplashType.Square;
		protected override float Length { get; } = 30f;
		protected override float Width { get; } = 10f;
		protected override float Angle { get; } = 30f;
	}

	[SkillHandler(SkillId.Mon_arma_Skill_2)]
	public class Mon_arma_Skill_2 : ITargetSkillHandler
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(2000);
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
			var splashArea = new CircleF(farPos, 20);
			var hitDelay = 1800;
			var damageDelay = 2000;
			await SkillAttack(caster, skill, splashArea, hitDelay, damageDelay);
			MonsterSkillSetCollisionDamage(caster, skill, true, 1f);
			await skill.Wait(TimeSpan.FromMilliseconds(3000));
			MonsterSkillSetCollisionDamage(caster, skill, false, 1f);
		}
	}

}
