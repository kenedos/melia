//--- Melia Script ----------------------------------------------------------
// Grimoire UI Refresh
//--- Description -----------------------------------------------------------
// Triggers a client-side UPDATE_GRIMOIRE_UI refresh whenever owner stats
// that feed the Sorcerer grimoire preview change. Mirrors the companion
// property update hook pattern in CharacterProperties: instead of listening
// to Inventory/Buff events directly, we subscribe to ValueChanged on the
// specific owner properties SCR_SUMMONED_MON_STATE_CALC and
// SUMMON_APPLY_OWNER_ATK_DEF consume, so equip swaps, buff changes, and
// any other source of stat invalidation propagate through the same
// property pipeline the companion UI uses.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Scripting;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class GrimoireRefreshScript : GeneralScript
{
	/// <summary>
	/// Owner properties that feed into the grimoire preview
	/// (HP, attack/defense averages, and the stat block).
	/// </summary>
	private static readonly string[] WatchedProperties =
	[
		PropertyName.MHP,
		PropertyName.DEF,
		PropertyName.MDEF,
		PropertyName.MINPATK,
		PropertyName.MAXPATK,
		PropertyName.MINMATK,
		PropertyName.MAXMATK,
		PropertyName.STR,
		PropertyName.CON,
		PropertyName.INT,
		PropertyName.DEX,
		PropertyName.MNA,
	];

	/// <summary>
	/// Subscribes to ValueChanged on every watched property of the
	/// character, so the grimoire preview refreshes when the owner's
	/// stats shift from any source.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	[On("PlayerReady")]
	public void OnPlayerReady(object sender, PlayerEventArgs args)
	{
		var character = args.Character;
		Action<string> handler = _ => RefreshGrimoire(character);

		foreach (var propertyName in WatchedProperties)
		{
			if (character.Properties.TryGet<CFloatProperty>(propertyName, out var property))
				property.ValueChanged += handler;
		}
	}

	/// <summary>
	/// Refreshes the grimoire when the level of the Sorcerer's Summoning
	/// skill changes, since the preview's transfer ratios scale with it.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	[On("PlayerSkillLevelChanged")]
	public void OnPlayerSkillLevelChanged(object sender, PlayerSkillLevelChangedEventArgs args)
	{
		if (args.Skill.Id != SkillId.Sorcerer_Summoning)
			return;

		RefreshGrimoire(args.Character);
	}

	/// <summary>
	/// Sends UPDATE_GRIMOIRE_UI to the client, causing the grimoire stat
	/// preview to recompute from the current owner state.
	/// </summary>
	/// <param name="character"></param>
	private static void RefreshGrimoire(Character character)
	{
		character.AddonMessage(AddonMessage.UPDATE_GRIMOIRE_UI);
	}
}
