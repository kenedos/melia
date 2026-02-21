using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Highlander
{
	/// <summary>
	/// Handler for the Highlander skill Cross Guard.
	/// </summary>
	[SkillHandler(SkillId.Highlander_CrossGuard)]
	public class Highlander_CrossGuard : IMeleeGroundSkillHandler, IDynamicCasted
	{
		/// <summary>
		/// Called when the user starts casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StartBuff(BuffId.CrossGuard_Buff, skill.Level, 0, TimeSpan.Zero, caster);
		}

		/// <summary>
		/// Called when the user stops casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.StopBuff(BuffId.CrossGuard_Buff);
		}

		/// <summary>
		/// Handles skill, ending block animation.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			var target = targets.FirstOrDefault();
			Send.ZC_SKILL_CAST_CANCEL(caster);
		}
	}
}
