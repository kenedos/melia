using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Buffs.Handlers.Wizards.Necromancer
{
	[Package("laima")]
	[BuffHandler(BuffId.Disinter_Wizard_Buff)]
	public class Necromancer_Disinter_Wizard_BuffOverride : BuffHandler
	{
		private bool _removingDebuff;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start)
				return;

			var buffComponent = buff.Target.Components.Get<BuffComponent>();

			if (buffComponent != null)
				buffComponent.BuffStarted += this.OnBuffStarted;
		}

		public override void OnEnd(Buff buff)
		{
			var buffComponent = buff.Target.Components.Get<BuffComponent>();

			if (buffComponent != null)
				buffComponent.BuffStarted -= this.OnBuffStarted;
		}

		private void OnBuffStarted(ICombatEntity entity, Buff appliedBuff)
		{
			if (_removingDebuff || entity == null || appliedBuff == null)
				return;

			if (!entity.IsBuffActive(BuffId.Disinter_Wizard_Buff))
				return;

			if (appliedBuff.Id == BuffId.Disinter_Wizard_Buff)
				return;

			if (appliedBuff.Data.Type != BuffType.Debuff)
				return;

			var buffComponent = entity.Components.Get<BuffComponent>();

			if (buffComponent == null || !entity.IsBuffActive(appliedBuff.Id))
				return;

			_removingDebuff = true;

			try
			{
				buffComponent.Remove(appliedBuff.Id);
			}
			finally
			{
				_removingDebuff = false;
			}
		}
	}
}
