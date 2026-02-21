using System;
using Melia.Shared.Packages;
using Melia.Shared.L10N;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Clerics.Cleric
{
	/// <summary>
	/// Handler for the Cleric skill Safety Zone.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cleric_SafetyZone)]
	public class Cleric_SafetyZoneOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		/// <summary>
		/// Start casting.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="maxCastTime"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// End casting.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="maxCastTime"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles skill
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_SYNC_START(caster, skillHandle, 1);
			Send.ZC_SYNC_END(caster, skillHandle, 0);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, TimeSpan.FromMilliseconds(300));

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var targetPos = caster.Position;

			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Cleric_New_SafetyZone_abil);
		}
	}
}
