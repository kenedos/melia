using System;
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
	/// Beak Mask.
	/// Blocks up to five removable debuffs during its duration.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.PlagueDoctor_BeakMask)]
	public class PlagueDoctor_BeakMask : IGroundSkillHandler
	{
		private const int MaximumBlockedDebuffs = 5;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromSeconds(60);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || character.IsDead)
				return;

			if (character.IsBuffActive(BuffId.BeakMask_Buff))
			{
				character.ServerMessage(Localization.Get("Beak Mask is already active."));
				return;
			}

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			character.SetAttackState(true);

			Send.ZC_SKILL_READY(character, skill, 1, originPos, character.Position);
			Send.ZC_NORMAL.UpdateSkillEffect(character, character.Handle, originPos, character.Direction, character.Position);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, character.Position);

			character.StartBuff(BuffId.BeakMask_Buff, skill.Level, MaximumBlockedDebuffs, BuffDuration, character, skill.Id);
			character.SetAttackState(false);
		}
	}
}
