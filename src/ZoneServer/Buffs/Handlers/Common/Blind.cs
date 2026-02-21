using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Dark, Field of vision interfered..
	/// </summary>
	[BuffHandler(BuffId.Blind, BuffId.UC_blind)]
	public class Blind : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var caster = buff.Caster;

			Send.ZC_SHOW_EMOTICON(target, "I_emo_blind", buff.Duration);
			Send.ZC_NORMAL.PlayTextEffect(target, caster, AnimationName.ShowBuffText, (float)buff.Id, null);

			if (target is IMonster monster)
			{
				if (!monster.Components.TryGet<AiComponent>(out var component))
					return;
				component.Script.SetViewRange(30);
				//component.Script.QueueEventAlert(new StatusAlert(target));
			}
		}

		public override void OnEnd(Buff buff)
		{
			var target = buff.Target;

			if (target is IMonster monster)
			{
				if (!monster.Components.TryGet<AiComponent>(out var component))
					return;
				component.Script.SetViewRange(300);
			}
		}
	}
}
