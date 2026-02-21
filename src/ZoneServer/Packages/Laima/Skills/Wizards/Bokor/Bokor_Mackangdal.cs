using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Shared.L10N;

namespace Melia.Zone.Skills.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Bokor skill Mackangdal.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Bokor_Mackangdal)]
	public class Bokor_MackangdalOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handle Skill Behavior
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.Components.Get<BuffComponent>().TryGet(BuffId.PowerOfDarkness_Buff, out var darkForceBuff))
			{
				caster.ServerMessage(Localization.Get("Requires Dark Force."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			var stackCount = darkForceBuff.OverbuffCounter;
			var duration = Math.Min(stackCount * 500, 10000);

			darkForceBuff.OverbuffCounter = 0;
			darkForceBuff.NotifyUpdate();

			skill.IncreaseOverheat();

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);

			Send.ZC_SYNC_START(caster, skillHandle, 1);
			caster.StartBuff(BuffId.Mackangdal_Buff, skill.Level, 0, TimeSpan.FromMilliseconds(duration), caster);
			Send.ZC_SYNC_END(caster, skillHandle, 0);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, TimeSpan.FromMilliseconds(300));

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
		}
	}
}
