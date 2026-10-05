using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.PlagueDoctor
{
	/// <summary>
	/// Black Death Steam.
	/// Applies a contagious poison that periodically damages enemies and reduces critical resistance.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.PlagueDoctor_PlagueVapours)]
	public class PlagueDoctor_PlagueVapours : IGroundSkillHandler
	{
		private const int MaximumTargets = 4;
		private const float InitialAreaRadius = 100f;
		private static readonly TimeSpan DebuffDuration = TimeSpan.FromSeconds(15);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, target?.Handle ?? 0, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var area = new Circle(farPos, InitialAreaRadius);
			var targets = caster.Map.GetAttackableEnemiesIn(caster, area).Where(enemy => enemy != null && !enemy.IsDead).OrderBy(enemy => farPos.Get2DDistance(enemy.Position)).Take(MaximumTargets).ToList();
			var spreadChainId = PlagueDoctorSpreadTracker.CreateChain();

			foreach (var enemy in targets)
				this.ApplyBlackDeathSteam(skill, character, enemy, spreadChainId);

			caster.SetAttackState(false);
		}

		private void ApplyBlackDeathSteam(Skill skill, Character caster, ICombatEntity target, int spreadChainId)
		{
			target.StartBuff(BuffId.PlagueVapours_Debuff, skill.Level, spreadChainId, DebuffDuration, caster, skill.Id);
		}
	}
}
