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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using System.Linq;
using Melia.Zone.Skills.SplashAreas;

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_Zibu_Maize_Skill_1)]
	public class Mon_Zibu_Maize_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(800);
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
			var hitDelay = 600;
			var aniTime = 800;
			var leadPos = GetLeadPosition(target, hitDelay, caster);
			caster.TurnTowards(leadPos);
			var farPos = originPos.GetNearestPositionWithinDistance(leadPos, skill.Properties[PropertyName.MaxR]);
			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos, hitDelay, aniTime));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos, int hitDelay, int aniTime)
		{
			ISplashArea splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 35f), 13f);
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 35f), 13f);
			hitDelay = 150;
			aniTime = 150;
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
			splashArea = new SplashAreas.Circle(originPos.GetRelative(farPos, distance: 35f), 13f);
			hitDelay = 150;
			aniTime = 150;
			var hits = new List<SkillHitInfo>();
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime, hits);
			SkillResultTargetBuff(caster, skill, BuffId.UC_poison, 1, hits.Sum(h => h.HitInfo.Damage) * 0.2f, 8000f, 1, 30, -1, hits);
		}
	}
}
