using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.HandlersOverrides.Clerics.Dievdirbys
{
	/// <summary>
	/// Handler override for CarveLaima_Buff, which forces a recalculation
	/// of skill cooldowns when it is applied and removed.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.CarveLaima_Buff)]
	public class CarveLaima_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target.Components.TryGet<SkillComponent>(out var skillComponent))
			{
				skillComponent.InvalidateAll();
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target.Components.TryGet<SkillComponent>(out var skillComponent))
			{
				skillComponent.InvalidateAll();
			}
		}
	}
}
