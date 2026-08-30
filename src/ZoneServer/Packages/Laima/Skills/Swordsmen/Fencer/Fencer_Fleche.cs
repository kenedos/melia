using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Fencer skill Fleche.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fencer_Fleche)]
	public class Fencer_FlecheOverride : IGroundSkillHandler
	{
		private static readonly (int HitDelay, int AniTime)[] HitTimings = [(200, 0), (350, 150), (500, 150), (650, 150)];
		private const float CritChanceBonus = 1f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(caster, skill, originPos, farPos));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill, Position originPos, Position farPos)
		{
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 100, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			var modifier = new SkillModifier();
			modifier.CritChanceMultiplier += CritChanceBonus;

			foreach (var timing in HitTimings)
				await SkillAttack(caster, skill, splashArea, timing.HitDelay, timing.AniTime, skillModifier: modifier);
		}
	}
}
