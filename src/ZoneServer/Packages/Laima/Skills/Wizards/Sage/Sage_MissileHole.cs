using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Packages;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.Sage
{
	/// <summary>
	/// Handler for Sage skill "Missile Hole".
	///
	/// Applies Missile Hole to the caster and nearby party members.
	/// The buff reduces incoming missile damage based on skill level.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sage_MissileHole)]
	public class Sage_MissileHoleOverride : IGroundSkillHandler
	{
		private const float BuffRange = 100f;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromSeconds(5);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
				return;

			caster.SetAttackState(true);

			try
			{
				Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

				ApplyBuff(caster, caster, skill);

				if (caster is Character character)
				{
					var party = character.Connection.Party;

					if (party != null)
					{
						var members = caster.Map.GetPartyMembersInRange(character, BuffRange, true);

						foreach (var member in members)
						{
							if (member == caster)
								continue;

							ApplyBuff(member, caster, skill);
						}
					}
				}

				skill.IncreaseOverheat();
			}
			finally
			{
				caster.SetAttackState(false);
			}
		}

		private static void ApplyBuff(ICombatEntity target, ICombatEntity caster, Skill skill)
		{
			target.StartBuff(BuffId.MissileHole_Buff, skill.Level, 0, BuffDuration, caster, skill.Id);

			if (caster is Character character && character.TryGetActiveAbility(AbilityId.Sage13, out _))
				character.StartBuff(BuffId.MissileHole_MSPD_Buff, skill.Level, 0, TimeSpan.FromSeconds(5), character, skill.Id);
		}
	}
}
