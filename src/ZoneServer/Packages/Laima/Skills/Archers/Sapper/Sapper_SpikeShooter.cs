using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Archers.Sapper
{
	/// <summary>
	/// Handler for the Sapper skill Spike Shooter.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Sapper_SpikeShooter)]
	public class Sapper_SpikeShooterOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const int CastDelayMs = 300;
		private const float SpawnDistance = 22.4f;

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			var targetPos = originPos.GetRelative(caster.Direction, distance: SpawnDistance);

			Send.ZC_SKILL_READY(caster, skill, skillHandle, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);

			Send.ZC_SYNC_START(caster, skillHandle, 1);
			SkillCreatePad(caster, skill, targetPos, 0f, PadName.Archer_SpikeShooter);
			Send.ZC_SYNC_END(caster, skillHandle, 0);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(caster, skillHandle, TimeSpan.FromMilliseconds(CastDelayMs));

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);
		}
	}
}
