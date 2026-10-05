using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.ObjectProperties;
using Melia.Shared.Scripting;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class GrimoireRefreshScript : GeneralScript
{
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
		PropertyName.MNA
	];

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

	[On("PlayerSkillLevelChanged")]
	public void OnPlayerSkillLevelChanged(object sender, PlayerSkillLevelChangedEventArgs args)
	{
		if (args.Skill.Id != SkillId.Sorcerer_Summoning)
			return;

		RefreshGrimoire(args.Character);
	}

	private static void RefreshGrimoire(Character character)
	{
		if (character?.Connection == null)
			return;

		var summons = character.Summons.GetSummons(summon =>
			summon.Vars.TryGetInt("SORCERER_SUMMONING", out var value) && value == 1);

		if (summons.Count > 0)
		{
			var summon = summons[0];

			Send.ZC_OBJECT_PROPERTY(
				character.Connection,
				summon.Handle,
				summon.Properties.GetSelect(WatchedProperties)
			);
		}

		character.AddonMessage(AddonMessage.UPDATE_GRIMOIRE_UI);
	}
}
