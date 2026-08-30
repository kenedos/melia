using System;
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
	/// Handler for the Fencer skill Attaque Composee.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fencer_AttaqueComposee)]
	public class Fencer_AttaqueComposeeOverride : IGroundSkillHandler
	{
		private static readonly (int HitDelay, int AniTime)[] HitTimings = [(300, 100), (450, 150)];

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
			caster.StartBuff(BuffId.Flanconnade_Buff, skill.Level, 0f, TimeSpan.FromMilliseconds(800), caster, skill.Id);

			var splashParam = skill.GetSplashParameters(caster, originPos, farPos, length: 80, width: 20, angle: 10f);
			var splashArea = skill.GetSplashArea(SplashType.Square, splashParam);

			foreach (var timing in HitTimings)
				await SkillAttack(caster, skill, splashArea, timing.HitDelay, timing.AniTime);
		}
	}
}
