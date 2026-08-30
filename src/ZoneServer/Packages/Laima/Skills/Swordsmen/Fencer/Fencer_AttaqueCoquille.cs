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
using System.Collections.Generic;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Fencer
{
	/// <summary>
	/// Handler for the Fencer skill Attaque Coquille.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fencer_AttaqueCoquille)]
	public class Fencer_AttaqueCoquilleOverride : IGroundSkillHandler
	{
		private static readonly (int HitDelay, int AniTime)[] HitTimings = [(350, 150)];
		private const int BuffDurationMs = 4000;
		private const int BuffDurationPerLevelMs = 1000;

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
			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 55, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);
			var hits = new List<SkillHitInfo>();

			foreach (var timing in HitTimings)
				await SkillAttack(caster, skill, splashArea, timing.HitDelay, timing.AniTime, hits);

			var duration = BuffDurationMs + skill.Level * BuffDurationPerLevelMs;
			SkillResultTargetBuff(caster, skill, BuffId.AttaqueCoquille_Debuff, skill.Level, 0f, duration, 1, 100, -1, hits);
		}
	}
}
