using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Pads.Handlers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;

namespace Melia.Zone.Pads.HandlersOverride.Wizards.Psychokino
{
	[Package("laima")]
	[PadHandler(PadName.Psychokino_Raise)]
	public class Psychokino_RaiseOverride : ICreatePadHandler, IDestroyPadHandler, IEnterPadHandler, ILeavePadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(pad, true);

			// Aumentado o raio para cobrir melhor a área visual (ex: 100f)
			pad.SetRange(100f);
			pad.SetUpdateInterval(1000);

			var life = 5000f + (1000f * skill.Level); // Tempo de duração no chão (ex: 5s + 1s por nível)
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(life);

			// Aumentado para não limitar a skill a apenas 1 ou 2 alvos
			pad.Trigger.MaxUseCount = 99;
			pad.Trigger.MaxConcurrentUseCount = 15;
			pad.Trigger.MaxActorCount = 15;

			PadSelectPadKill(pad, PadName.HeavyGravity_PAD, 100f);
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(pad, false);

			// Remove o buff de suspensão dos monstros ao destruir a área no chão
			foreach (var actor in pad.Trigger.GetActors<ICombatEntity>())
			{
				if (actor.TryGetBuff(BuffId.Raise_Debuff, out var buff) && buff.Caster == creator)
				{
					actor.RemoveBuff(buff.Id);
				}
			}
		}

		public void Entered(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;
			var skill = pad.Skill;

			if (!creator.IsEnemy(initiator))
				return;

			if (pad.Trigger.AtCapacity)
				return;

			pad.Trigger.ActivateCount++;

			// Aplica o Raise_Debuff (suspensão) pelo tempo de vida do pad
			var duration = 5000 + (skill.Level * 1000);
			PadTargetBuff(pad, initiator, RelationType.Enemy, 0, 0, BuffId.Raise_Debuff, 1, 0, duration, 1, 100, false);
		}

		public void Left(object sender, PadTriggerActorArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var initiator = args.Initiator;

			if (!creator.IsEnemy(initiator))
				return;

			if (!initiator.IsBuffActive(BuffId.Raise_Debuff))
				return;

			pad.Trigger.ActivateCount--;

			// Se o inimigo sair da área do chão, perde o status de levitação
			PadTargetBuffRemove(pad, initiator, RelationType.All, 0, 0, BuffId.Raise_Debuff, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
		}
	}
}
