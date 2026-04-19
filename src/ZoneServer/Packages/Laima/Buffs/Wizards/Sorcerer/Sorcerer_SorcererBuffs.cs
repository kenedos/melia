using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

namespace Melia.Zone.Buffs.Handlers.Wizards.Sorcerer
{
	/// <summary>
	/// Handler for the Sorcerer_Obey_Status_Buff applied to controlled summons.
	/// </summary>
	/// <remarks>
	/// Applied when the player takes direct control of their summon.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Sorcerer_Obey_Status_Buff)]
	public class Sorcerer_Obey_Status_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// The summon is now under direct player control
			// Disable AI and allow player input
			if (buff.Target is Summon summon)
			{
				summon.Vars.SetBool("UnderPlayerControl", true);

				// Remove AI component while under control
				var aiComponent = summon.Components.Get<AiComponent>();
				if (aiComponent != null)
				{
					summon.Vars.Set("StoredAiComponent", aiComponent);
					summon.Components.Remove<AiComponent>();
				}
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Summon summon)
			{
				summon.Vars.SetBool("UnderPlayerControl", false);

				// Restore AI component
				if (summon.Vars.TryGet<AiComponent>("StoredAiComponent", out var aiComponent))
				{
					summon.Components.Add(aiComponent);
					summon.Vars.Remove("StoredAiComponent");
				}

				// Also stop control on the owner side
				if (summon.Owner is Character owner)
				{
					owner.StopBuff(BuffId.Sorcerer_Obey_PC_DEF_Buff);
					//Send.ZC_CONTROL_OBJECT(owner, summon, false, "None");
				}
			}
		}
	}

	/// <summary>
	/// Handler for the Sorcerer_Obey_PC_DEF_Buff applied to the sorcerer during Obey.
	/// </summary>
	/// <remarks>
	/// Provides defensive bonuses while controlling a summon.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Sorcerer_Obey_PC_DEF_Buff)]
	public class Sorcerer_Obey_PC_DEF_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Apply defensive bonuses while controlling summon
			var defBonus = 50f * buff.NumArg1;
			var mdefBonus = 50f * buff.NumArg1;

			AddPropertyModifier(buff, buff.Target, PropertyName.DEF_BM, defBonus);
			AddPropertyModifier(buff, buff.Target, PropertyName.MDEF_BM, mdefBonus);
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MDEF_BM);
		}
	}

	/// <summary>
	/// Handler for the Summoning_Overwork_Buff applied to summons.
	/// </summary>
	/// <remarks>
	/// Provides bonuses but may have drawbacks.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Summoning_Overwork_Buff)]
	public class Summoning_Overwork_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Overwork provides attack bonuses based on ability level
			var abilityLevel = buff.NumArg1;
			var atkBonus = 100f * abilityLevel;

			AddPropertyModifier(buff, buff.Target, PropertyName.PATK_BM, atkBonus);
			AddPropertyModifier(buff, buff.Target, PropertyName.MATK_BM, atkBonus);
		}

		public override void WhileActive(Buff buff)
		{
			// Overwork may drain HP over time as a drawback
			if (buff.Target is Summon summon)
			{
				var hpDrain = summon.Properties.GetFloat(PropertyName.MHP) * 0.01f;
				summon.TakeDamage(hpDrain, summon);
			}
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.PATK_BM);
			RemovePropertyModifier(buff, buff.Target, PropertyName.MATK_BM);
		}
	}

	/// <summary>
	/// Handler for the Ability_buff_PC_Summon applied to all player summons.
	/// </summary>
	/// <remarks>
	/// This is a baseline buff for all PC summons.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Ability_buff_PC_Summon)]
	public class Ability_buff_PC_SummonOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Mark entity as a PC summon
			if (buff.Target is Summon summon)
			{
				summon.Vars.SetBool("IsPCSummon", true);
			}
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is Summon summon)
			{
				summon.Vars.SetBool("IsPCSummon", false);
			}
		}
	}
}
