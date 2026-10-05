using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.PlagueDoctor
{
	/// <summary>
	/// Modafinil.
	/// Increases the movement speed of the caster and nearby party members.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.PlagueDoctor_Modafinil)]
	public class PlagueDoctor_Modafinil : IGroundSkillHandler
	{
		private const float BuffRange = 140f;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromMinutes(15);
		private static readonly TimeSpan CastDelay = TimeSpan.FromMilliseconds(700);

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
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.ApplyBuffs(skill, character));
		}

		private async Task ApplyBuffs(Skill skill, Character caster)
		{
			await skill.Wait(CastDelay);

			if (caster.IsDead || caster.Map == null)
			{
				caster.SetAttackState(false);
				return;
			}

			this.ApplyBuff(caster, skill);

			if (caster.Connection?.Party != null)
			{
				var partyMembers = caster.Map.GetPartyMembersInRange(caster, BuffRange, true).Where(member => member != null && !member.IsDead);

				foreach (var partyMember in partyMembers)
				{
					if (partyMember == caster)
						continue;

					this.ApplyBuff(partyMember, skill);
				}
			}

			caster.SetAttackState(false);
		}

		private void ApplyBuff(ICombatEntity target, Skill skill)
		{
			target.StartBuff(BuffId.Modafinil_Buff, skill.Level, 0f, BuffDuration, skill.Owner, skill.Id);
		}
	}
}
