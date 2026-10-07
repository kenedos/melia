using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.Inquisitor
{
	/// <summary>
	/// Handler for the Inquisitor skill Iron Maiden, which traps a small or
	/// medium enemy that isn't a boss.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Inquisitor_IronMaiden)]
	public class Inquisitor_IronMaidenOverride : IGroundSkillHandler
	{
		private const float SearchRange = 20f;
		private static readonly TimeSpan TrapDelay = TimeSpan.FromMilliseconds(550);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!IsTrappable(caster, target))
			{
				target = caster.Map.GetAttackableEnemiesInPosition(caster, farPos, SearchRange)
					.FirstOrDefault(enemy => IsTrappable(caster, enemy));
			}

			if (target == null)
			{
				caster.ServerMessage(Localization.Get("No valid target was found."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.TurnTowards(target);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, target.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, target.Position, ForceId.GetNew(), null);

			skill.Run(this.Trap(skill, caster, target));
		}

		/// <summary>
		/// Shuts the target in the Iron Maiden.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		private async Task Trap(Skill skill, ICombatEntity caster, ICombatEntity target)
		{
			await skill.Wait(TrapDelay);

			if (caster.IsDead || target.IsDead)
				return;

			target.StartBuff(BuffId.IronMaiden_Debuff, skill.Level, 0, skill.Properties.CaptionTime, caster, skill.Id);
		}

		/// <summary>
		/// Returns true if the target is an enemy Iron Maiden can hold: small
		/// or medium, and not a boss.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		private static bool IsTrappable(ICombatEntity caster, ICombatEntity target)
		{
			if (target == null || target.IsDead || !caster.IsEnemy(target) || target.Rank == MonsterRank.Boss)
				return false;

			return target.EffectiveSize is SizeType.S or SizeType.M;
		}
	}
}
