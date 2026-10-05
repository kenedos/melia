using System;
using System.Collections.Generic;
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

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Appraiser
{
	/// <summary>
	/// Triplet Lense.
	/// Grants five lenses that are automatically attached to enemies
	/// when the caster attacks them.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Appraiser_TripletLens)]
	public class Appraiser_TripletLens : IGroundSkillHandler, IMeleeGroundSkillHandler
	{
		private const int MaximumLenses = 5;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (caster is not Character character || caster.IsDead)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			caster.SetAttackState(true);
			caster.TurnTowards(farPos);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(
				caster,
				caster.Handle,
				originPos,
				caster.Direction,
				Position.Zero
			);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			caster.StopBuff(BuffId.TripletLens_Buff);

			for (var lens = 0; lens < MaximumLenses; lens++)
			{
				caster.StartBuff(
					BuffId.TripletLens_Buff,
					skill.Level,
					0f,
					TimeSpan.Zero,
					caster,
					skill.Id
				);
			}

			caster.SetAttackState(false);
		}
	}
}
