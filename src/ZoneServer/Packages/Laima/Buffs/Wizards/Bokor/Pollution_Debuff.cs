using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Buffs.Handlers.Wizards.Bokor
{
	/// <summary>
	/// Handler for the Pollution debuff, applied by Bokor_Effigy.
	/// Deals periodic damage over time based on the Effigy skill.
	/// Reduces target's movement speed by 50%.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Pollution_Debuff)]
	public class Pollution_DebuffOverride : DamageOverTimeBuffHandler
	{
		private const float MoveSpeedReductionRate = 0.75f;

		/// <summary>
		/// Called when the buff is activated on the target.
		/// Snapshots damage, shows fear emoticon, and reduces movement speed.
		/// </summary>
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			base.OnActivate(buff, activationType);

			var target = buff.Target;
			Send.ZC_SHOW_EMOTICON(target, "I_emo_fear", buff.Duration);

			var currentMspd = target.Properties.GetFloat(PropertyName.MSPD);
			var reduction = currentMspd * MoveSpeedReductionRate;

			AddPropertyModifier(buff, target, PropertyName.MSPD_BM, -reduction);
		}

		/// <summary>
		/// Called when the buff ends.
		/// Removes the movement speed penalty.
		/// </summary>
		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.MSPD_BM);
		}

		/// <summary>
		/// Called periodically while the buff is active.
		/// Applies snapshotted damage from the base DamageOverTimeDebuff class.
		/// </summary>
		public override void WhileActive(Buff buff)
		{
			base.WhileActive(buff);
		}

		/// <summary>
		/// Returns the hit type for the damage tick (Poison effect).
		/// </summary>
		protected override HitType GetHitType(Buff buff)
		{
			return HitType.Poison;
		}

		/// <summary>
		/// Called after each damage tick.
		/// Adds hate towards each zombie.
		/// </summary>
		protected override void OnDamageTick(Buff buff)
		{
			var caster = buff.Caster;
			var target = buff.Target;

			if (caster is not Character character)
				return;

			var summons = character.Summons.GetSummons();
			if (summons.Count == 0)
				return;

			if (!target.Components.TryGet<AiComponent>(out var targetAi))
				return;

			// Add 300 hate to each zombie
			foreach (var summon in summons)
			{
				targetAi.Script.QueueEventAlert(new HateIncreaseAlert(summon, 300));
			}
		}
	}
}
