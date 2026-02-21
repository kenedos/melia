using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers.Clerics.Paladin
{
	[Package("laima")]
	[PadHandler(PadName.Cleric_Barrier_PC)]
	public class Cleric_Barrier_PCOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const int MaxTrackedEnemies = 25;
		private const string TrackedEnemiesKey = "Melia.Barrier.TrackedEnemies";
		private static readonly TimeSpan ExpirationTime = TimeSpan.FromSeconds(2);

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			pad.NumArg1 = skill.Level;
			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(80f);
			pad.SetUpdateInterval(200);
			pad.Trigger.Area = new Circle(pad.Position, 80f);
			var value = 10000 + skill.Level * 3000;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(value);
			pad.Trigger.MaxUseCount = 1;

			pad.Variables.Set(TrackedEnemiesKey, new Dictionary<int, DateTime>());
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Barrier_Buff);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Barrier_MeleeAttack_Buff);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.Barrier_MagicAttack_Buff);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			PadTargetBuff(pad, initiator, RelationType.Friendly, 0, 0, BuffId.Barrier_Buff, skill.Level, 0, 0, 1, 100, false);

			if (creator.IsEnemy(initiator) && skill != null && initiator.IsKnockdownable())
			{
				var tracked = GetTrackedEnemies(pad);

				if (!TryTrackEnemy(tracked, initiator.Handle))
					return;

				var kb = new KnockBackInfo(pad.Position, initiator, HitType.KnockBack, 100, 10);
				initiator.Position = kb.ToPosition;
				initiator.AddState(StateType.KnockedBack, kb.Time);
				Send.ZC_KNOCKBACK_INFO(initiator, kb);
			}

			PadTargetBuffCheckAbility(pad, initiator, RelationType.Party, AbilityId.Paladin35, 0, 0, BuffId.Barrier_MeleeAttack_Buff, skill.Level, 0, 0, 1, 100);
			PadTargetBuffCheckAbility(pad, initiator, RelationType.Party, AbilityId.Paladin36, 0, 0, BuffId.Barrier_MagicAttack_Buff, skill.Level, 0, 0, 1, 100);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var initiator = args.Initiator;

			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.Barrier_Buff, false);
			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.Barrier_MeleeAttack_Buff, false);
			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.Barrier_MagicAttack_Buff, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (skill == null)
				return;

			var tracked = GetTrackedEnemies(pad);
			CleanupExpired(tracked);

			var enemies = pad.Trigger.GetAttackableEntities(creator);

			foreach (var enemy in enemies)
			{
				if (enemy is ICombatEntity combatEntity && creator.IsEnemy(combatEntity) && combatEntity.IsKnockdownable())
				{
					if (!TryTrackEnemy(tracked, combatEntity.Handle))
						continue;

					var kb = new KnockBackInfo(pad.Position, combatEntity, HitType.KnockBack, 100, 10);
					combatEntity.Position = kb.ToPosition;
					combatEntity.AddState(StateType.KnockedBack, kb.Time);
					Send.ZC_KNOCKBACK_INFO(combatEntity, kb);
				}
			}
		}

		private static Dictionary<int, DateTime> GetTrackedEnemies(Pad pad)
		{
			if (!pad.Variables.TryGet<Dictionary<int, DateTime>>(TrackedEnemiesKey, out var dict))
			{
				dict = new Dictionary<int, DateTime>();
				pad.Variables.Set(TrackedEnemiesKey, dict);
			}
			return dict;
		}

		private static void CleanupExpired(Dictionary<int, DateTime> tracked)
		{
			var now = DateTime.UtcNow;
			var expired = new List<int>();

			foreach (var kv in tracked)
			{
				if (now - kv.Value > ExpirationTime)
					expired.Add(kv.Key);
			}

			foreach (var key in expired)
				tracked.Remove(key);
		}

		private static bool TryTrackEnemy(Dictionary<int, DateTime> tracked, int handle)
		{
			if (tracked.ContainsKey(handle))
			{
				tracked[handle] = DateTime.UtcNow;
				return true;
			}

			CleanupExpired(tracked);

			if (tracked.Count >= MaxTrackedEnemies)
				return false;

			tracked[handle] = DateTime.UtcNow;
			return true;
		}
	}
}
