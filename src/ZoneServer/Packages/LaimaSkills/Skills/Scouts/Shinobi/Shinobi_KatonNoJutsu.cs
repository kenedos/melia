using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Shinobi
{
	/// <summary>
	/// Handler for the Shinobi skill Katon no Jutsu, a chain of three
	/// gunpowder blasts walking away from the Shinobi.
	/// </summary>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Shinobi_Katon_no_jutsu)]
	public class Shinobi_KatonNoJutsuOverride : IGroundSkillHandler
	{
		private const float BlastRange = 25f;
		private static readonly (float Distance, int Delay)[] Blasts = [(30, 300), (60, 100), (90, 100)];

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

			skill.Run(Explode(skill, caster));
			ShinobiSkillHelper.ReplicateOnClones(caster, skill.Id, Explode);
		}

		/// <summary>
		/// Sets off the blasts in front of the attacker one after another.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="attacker"></param>
		/// <returns></returns>
		private static async Task Explode(Skill skill, ICombatEntity attacker)
		{
			var origin = attacker.Position;
			var direction = attacker.Direction;

			foreach (var blast in Blasts)
			{
				await skill.Wait(TimeSpan.FromMilliseconds(blast.Delay));

				if (attacker.IsDead)
					return;

				var position = origin.GetRelative(direction, blast.Distance);
				var hits = new List<SkillHitInfo>();

				foreach (var target in attacker.Map.GetAttackableEnemiesInPosition(attacker, position, BlastRange))
				{
					var skillHitResult = SCR_SkillHit(attacker, target, skill);
					target.TakeDamage(skillHitResult.Damage, attacker);

					hits.Add(new SkillHitInfo(attacker, target, skill, skillHitResult, TimeSpan.Zero, TimeSpan.Zero));
				}

				if (hits.Count > 0)
					Send.ZC_SKILL_HIT_INFO(attacker, hits);
			}
		}
	}
}
