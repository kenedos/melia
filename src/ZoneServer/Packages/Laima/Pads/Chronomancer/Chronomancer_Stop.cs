using System;
using Melia.Shared.Data;
using Melia.Shared.Data.Database;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.Handlers
{
	[Package("laima")]
	[PadHandler(PadName.Chronomancer_Stop)]
	public class Chronomancer_StopOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(70f);
			pad.SetUpdateInterval(500);
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(700);
			pad.Trigger.MaxActorCount = 9;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			var buffTime = (int)((4f + 0.4f * skill.Level) * 1000);

			if (creator.TryGetActiveAbilityLevel(AbilityId.Chronomancer10, out var abilLevel))
			{
				var extensionTime = TimeSpan.FromMilliseconds(buffTime);

				foreach (var target in pad.Trigger.GetAttackableEntities(creator))
				{
					if (!creator.IsEnemy(target))
						continue;

					var debuffs = target.Components.Get<BuffComponent>()?.GetAll(b => b.Data.Type == BuffType.Debuff && b.HasDuration);
					if (debuffs == null)
						continue;

					foreach (var debuff in debuffs)
					{
						if (debuff.Vars.GetBool("Laima.Stop.BreakTimeExtended"))
							continue;

						debuff.IncreaseDuration(debuff.RemainingDuration + extensionTime);
						debuff.Vars.SetBool("Laima.Stop.BreakTimeExtended", true);
						debuff.NotifyUpdate();
					}
				}
			}
			else
			{
				PadBuffEnemyMonster(pad, RelationType.Enemy, 0, 0, BuffId.Stop_Debuff, skill.Level, 0, buffTime, 0, 100);
			}
		}
	}
}
