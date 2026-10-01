//--- Melia Script ----------------------------------------------------------
// Grimoire UI Refresh
//--- Description -----------------------------------------------------------
// Triggers a client-side UPDATE_GRIMOIRE_UI refresh whenever owner stats
// that feed the Sorcerer grimoire preview change. Subscribes once to the
// character's batched Properties.Invalidated event, so a single stat/item/
// buff change that invalidates several watched properties at once still
// only results in one UPDATE_GRIMOIRE_UI send.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Game.Const;
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
	private static readonly HashSet<string> WatchedProperties = new()
	{
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
	};

	/// <summary>
	/// Subscribes once to the character's batched property invalidation
	/// event, so the grimoire preview refreshes when the owner's stats
	/// shift from any source, without sending one message per property.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="args"></param>
	[On("PlayerReady")]
	public void OnPlayerReady(object sender, PlayerEventArgs args)
	{
		var character = args.Character;
		character.Properties.Invalidated += names => OnPropertiesInvalidated(character, names);
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
	/// Called once per property invalidation batch. Refreshes the grimoire
	/// if any of the invalidated properties feed into its preview.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="invalidatedNames"></param>
	private void OnPropertiesInvalidated(Character character, IReadOnlyList<string> invalidatedNames)
	{
		if (!invalidatedNames.Any(WatchedProperties.Contains))
			return;

		RefreshGrimoire(character);
	}

	/// <summary>
	/// Sends UPDATE_GRIMOIRE_UI to the client, causing the grimoire stat
	/// preview to recompute from the current owner state. Only applies to
	/// characters that have actually learned Summoning, since the grimoire
	/// preview is Sorcerer-specific.
	/// </summary>
	/// <param name="character"></param>
	private static void RefreshGrimoire(Character character)
	{
		if (!character.Skills.Has(SkillId.Sorcerer_Summoning))
			return;

		character.AddonMessage(AddonMessage.UPDATE_GRIMOIRE_UI);
	}
}
