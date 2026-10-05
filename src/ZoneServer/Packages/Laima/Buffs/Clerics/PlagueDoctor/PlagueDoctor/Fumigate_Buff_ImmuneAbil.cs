using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Util;

namespace Melia.Zone.Packages.Laima.Buffs.Clerics.PlagueDoctor
{
	/// <summary>
	/// Fumigate: Prevention.
	/// Grants a 5% chance per Fumigate skill level to block conditions of level 3 or lower.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Fumigate_Buff_ImmuneAbil)]
	public class Fumigate_Buff_ImmuneAbilOverride : BuffHandler
	{
		private const int MaximumConditionLevel = 3;
		private const float ResistanceChancePerSkillLevel = 5f;
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
			if (_removingDebuff || appliedBuff == null)
				return;

			if (!entity.TryGetBuff(BuffId.Fumigate_Buff_ImmuneAbil, out var prevention))
				return;

			if (appliedBuff.Data.Type != BuffType.Debuff)
				return;

			if (appliedBuff.Data.Level > MaximumConditionLevel)
				return;

			var buffComponent = entity.Components.Get<BuffComponent>();

			if (buffComponent == null || !entity.IsBuffActive(appliedBuff.Id))
				return;

			var fumigateLevel = Math.Clamp((int)prevention.NumArg1, 1, 10);
			var resistanceChance = fumigateLevel * ResistanceChancePerSkillLevel;

			if (RandomProvider.Get().Next(100) >= resistanceChance)
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
