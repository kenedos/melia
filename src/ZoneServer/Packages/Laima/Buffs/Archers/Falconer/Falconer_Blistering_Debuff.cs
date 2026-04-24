using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Archers.Falconer
{
	/// <summary>
	/// Blind-style debuff applied by Falconer's Sonic Strike.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Blistering_Debuff)]
	public class Falconer_Blistering_DebuffOverride : BuffHandler
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
