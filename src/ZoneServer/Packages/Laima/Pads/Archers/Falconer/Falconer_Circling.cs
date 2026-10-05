using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using static Melia.Zone.Pads.Helpers.PadHelper;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Pads.Handlers
{
	/// <summary>
	/// Pad handler for Falconer's Circling skill.
	/// Follows the hawk companion. Applies UC_Detected_Debuff to enemies.
	/// </summary>
	[Package("laima")]
	[PadHandler(PadName.Falconer_Circling)]
	public class Falconer_CirclingOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		private const int AllyBuffDurationSeconds = 10;

		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, true);
			pad.SetRange(80f);
			pad.SetUpdateInterval(5000);

			if (creator is Character character)
			{
				var hawk = character.Companions.ActiveBirdCompanion;
				if (hawk != null)
					pad.FollowsTarget(hawk);
			}
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;

			Send.ZC_NORMAL.PadRemoveEffect(pad, "F_archer_circling_ground");
			Send.ZC_NORMAL.PadUpdate(pad, false);
			PadRemoveBuff(pad, RelationType.All, 0, 0, BuffId.UC_Detected_Debuff);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (creator.IsEnemy(initiator))
			{
				if (initiator.IsBuffActive(BuffId.UC_Detected_Debuff))
					return;

				initiator.StartBuff(BuffId.UC_Detected_Debuff, skill.Level, 0f, pad.Trigger.RemainingLifeTime, creator);
				return;
			}

			if (initiator is Character ally && IsAllyOfCreator(creator, ally))
				ally.StartBuff(BuffId.CirclingIncreaseSR_Buff, skill.Level, 0f, TimeSpan.FromSeconds(AllyBuffDurationSeconds), creator, skill.Id);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var initiator = args.Initiator;

			initiator.StopBuff(BuffId.UC_Detected_Debuff);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (creator.IsDead)
			{
				pad.Destroy();
				return;
			}

			var enemies = pad.Trigger.GetAttackableEntities(creator);
			foreach (var enemy in enemies)
			{
				if (enemy.IsDead || enemy.IsBuffActive(BuffId.UC_Detected_Debuff))
					continue;

				enemy.StartBuff(BuffId.UC_Detected_Debuff, skill.Level, 0f, pad.Trigger.RemainingLifeTime, creator);
			}

			RefreshAllyBuffs(pad, creator);
		}

		private static void RefreshAllyBuffs(Pad pad, ICombatEntity creator)
		{
			var skill = pad.Skill;
			if (skill == null)
				return;

			var duration = TimeSpan.FromSeconds(AllyBuffDurationSeconds);
			foreach (var actor in pad.Trigger.GetActors())
			{
				if (actor is not Character ally || ally.IsDead)
					continue;

				if (!IsAllyOfCreator(creator, ally))
					continue;

				ally.StartBuff(BuffId.CirclingIncreaseSR_Buff, skill.Level, 0f, duration, creator, skill.Id);
			}
		}

		private static bool IsAllyOfCreator(ICombatEntity creator, Character ally)
		{
			return creator == ally || creator.IsAlly(ally);
		}
	}
}
