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

namespace Melia.Zone.Skills.Handlers.Clerics.Druid
{
	/// <summary>
	/// Handler for the Druid skill Seed Bomb, which plants seeds on the
	/// enemies in front of the Druid that burst when the enemy is hit or the
	/// seed runs out, or at once as a double burst on enemies already seeded.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Druid_Seedbomb)]
	public class Druid_SeedBombOverride : IGroundSkillHandler
	{
		private const float Distance = 70f;
		private const float Range = 100f;
		private static readonly TimeSpan SeedDelay = TimeSpan.FromMilliseconds(500);
		private static readonly TimeSpan SeedDuration = TimeSpan.FromSeconds(2);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.Seed(skill, caster));
		}

		/// <summary>
		/// Seeds the enemies in front of the caster, bursting the seeds that
		/// are already there.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <returns></returns>
		private async Task Seed(Skill skill, ICombatEntity caster)
		{
			var position = caster.Position.GetRelative(caster.Direction, Distance);

			await skill.Wait(SeedDelay);

			if (caster.IsDead)
				return;

			var maxTargets = (int)skill.Properties.GetFloat(PropertyName.CaptionRatio);

			foreach (var target in caster.Map.GetAttackableEnemiesInPosition(caster, position, Range).Take(maxTargets))
			{
				if (target.TryGetBuff(BuffId.Seedbomb_Buff, out var seed) && seed.Caster == caster)
				{
					seed.NumArg2 = 2;
					target.StopBuff(BuffId.Seedbomb_Buff);
					continue;
				}

				target.StartBuff(BuffId.Seedbomb_Buff, skill.Level, 1, SeedDuration, caster, skill.Id);
			}
		}
	}
}
