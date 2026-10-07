using System;
using System.Linq;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Skills.Helpers
{
	/// <summary>
	/// Shared functionality for the Plague Doctor's debuff handling.
	/// </summary>
	public static class PlagueDoctorSkillHelper
	{
		/// <summary>
		/// Returns true if the debuff is one the Plague Doctor's cures and
		/// masks deal with: removable, and not a stun, bind, immobility or
		/// slow.
		/// </summary>
		/// <param name="buffId"></param>
		/// <returns></returns>
		public static bool IsCurableDebuff(BuffId buffId)
		{
			if (!ZoneServer.Instance.Data.BuffDb.TryFind(buffId, out var buffData))
				return false;

			if (buffData.Type != BuffType.Debuff || !buffData.Removable)
				return false;

			return !buffData.Tags.HasAny(BuffTag.Stun, BuffTag.Hold, BuffTag.Immobilize, BuffTag.Slow);
		}

		/// <summary>
		/// Returns the number of debuffs on the entity, not counting the
		/// given one.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="exceptBuffId"></param>
		/// <returns></returns>
		public static int CountDebuffs(ICombatEntity entity, BuffId exceptBuffId = 0)
		{
			if (!entity.Components.TryGet<BuffComponent>(out var buffComponent))
				return 0;

			return buffComponent.GetList().Count(a => a.Data.Type == BuffType.Debuff && a.Id != exceptBuffId);
		}

		/// <summary>
		/// Starts a copy of the buff on the target, keeping its caster,
		/// arguments and remaining duration. Returns false if the target
		/// already has it or nothing is left of it.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="target"></param>
		/// <returns></returns>
		public static bool SpreadBuff(Buff buff, ICombatEntity target)
		{
			if (target.IsDead || target.IsBuffActive(buff.Id))
				return false;

			var remaining = buff.RemainingDuration;
			if (remaining <= TimeSpan.Zero)
				return false;

			target.StartBuff(buff.Id, buff.NumArg1, buff.NumArg2, remaining, buff.Caster, buff.SkillId);
			return true;
		}

		/// <summary>
		/// Starts the entity's Beak Mask cooldown, which runs once a mask
		/// comes off.
		/// </summary>
		/// <param name="entity"></param>
		public static void StartBeakMaskCooldown(ICombatEntity entity)
		{
			if (entity.TryGetSkill(SkillId.PlagueDoctor_BeakMask, out var skill))
				skill.StartCooldown(skill.Properties.CoolDown);
		}
	}
}
