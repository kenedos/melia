using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Pads;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Helpers
{
	public static class SkillUtilHelper
	{
		/// <summary>
		/// Applies knockback/knockdown to target and sends appropriate packet.
		/// </summary>
		public static void SkillToolKnockdown(ICombatEntity caster, ICombatEntity target, KnockType knockType, KnockDirection knockDirection, float power, float verticalAngle, float horizontalAngle, int bound, int knockdownRank)
		{
			if (target == null || target.IsDead || !target.IsKnockdownable())
				return;

			var hitType = knockType == KnockType.KnockDown ? HitType.KnockDown : HitType.KnockBack;
			var kb = new KnockBackInfo(caster, target, hitType, (int)power, (int)verticalAngle, knockDirection);

			target.Position = kb.ToPosition;
			target.AddState(StateType.KnockedBack, kb.Time);

			if (knockType == KnockType.KnockDown)
			{
				target.AddState(StateType.KnockedDown, kb.Time);
				Send.ZC_KNOCKDOWN_INFO(target, kb);
			}
			else
			{
				Send.ZC_KNOCKBACK_INFO(target, kb);
			}
		}

		public static async Task DestroyPad(ICombatEntity caster, Skill skill, Position position,
			RelationType relation, float range, int padDestroyLimit, string effectName, int destroyDelay,
			string groundEffect, float groundScale)
		{
			var padList = caster.Map.GetPadsAt(position, range);
			if (padList.Length == 0)
				return;

			var padsDestroyed = 0;
			var destroyTasks = new List<Task>();

			foreach (var pad in padList)
			{
				if (padsDestroyed >= padDestroyLimit)
					break;

				var target = pad.Creator as ICombatEntity;
				if (target == null || caster.GetRelation(target) != relation)
					continue;

				destroyTasks.Add(DestroyPadAfterTime(skill, pad, effectName, destroyDelay, groundEffect, groundScale));
				padsDestroyed++;
			}

			if (destroyTasks.Count > 0)
				await Task.WhenAll(destroyTasks);
		}

		public static async Task DestroyPadAfterTime(Skill skill, Pad pad, string effectName,
			int destroyDelay, string groundEffect, float groundScale)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(destroyDelay));
			pad.PlayEffect(effectName);
			pad.PlayGroundEffect(pad.Position, groundEffect, groundScale);
			pad.Destroy();
		}

	}
}
