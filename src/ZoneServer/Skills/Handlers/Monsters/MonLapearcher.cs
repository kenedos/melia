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

namespace Melia.Zone.Skills.Handlers.Mon
{
	[SkillHandler(SkillId.Mon_LapeArcher_Skill_1)]
	public class Mon_LapeArcher_Skill_1 : ITargetSkillHandler
	{
		protected TimeSpan AniTime { get; } = TimeSpan.FromMilliseconds(1000);
		public void Handle(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			if (target == null)
			{
				Send.ZC_NORMAL.SkillTargetAnimation(caster, skill, caster.Direction, 1);
				Send.ZC_SKILL_FORCE_TARGET(caster, null, skill);
				return;
			}

			var originPos = caster.Position;
			var hitDelay = 800 + (int)(caster.Position.Get2DDistance(target.Position) * 1.6);
			var leadPos = GetLeadPosition(target, hitDelay);
			caster.TurnTowards(leadPos);
			var farPos = originPos.GetNearestPositionWithinDistance(leadPos, skill.Properties[PropertyName.MaxR]);

			skill.Run(this.HandleSkill(caster, target, skill, originPos, farPos, hitDelay));
		}

		private async Task HandleSkill(ICombatEntity caster, ICombatEntity target, Skill skill, Position originPos, Position farPos, int hitDelay)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 10);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var aniTime = hitDelay + 200;
			_ = ForceAttackEffect(caster, target, skill, hitDelay);
			await SkillAttack(caster, skill, splashArea, hitDelay, aniTime);
		}
	}
}
