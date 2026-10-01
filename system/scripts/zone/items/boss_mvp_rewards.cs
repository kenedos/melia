//--- Melia Script ----------------------------------------------------------
// Boss MVP Rewards
//--- Description -----------------------------------------------------------
// Shows MVP emoticon for the top DPS player when killing normal bosses.
// Uses the EffectsComponent so players entering visibility range see it.
//---------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;

/// <summary>
/// Shows MVP emoticon for the top DPS player when killing normal bosses.
/// </summary>
public class BossMvpRewardsScript : GeneralScript
{
	/// <summary>
	/// MVP emoticon name.
	/// </summary>
	private const string MvpEmoticon = "I_emo_mvp";

	/// <summary>
	/// Duration to display MVP emoticon.
	/// </summary>
	private static readonly TimeSpan MvpEmoticonDuration = TimeSpan.FromSeconds(10);

	[On("EntityKilled")]
	public void OnEntityKilled(object sender, CombatEventArgs args)
	{
		if (args.Target is not Mob mob)
			return;

		// Skip Card Album bosses - they have their own reward system
		if (mob.Vars.GetInt("CARDSUMMON_BOSS", 0) == 1)
			return;

		// Only handle boss rank monsters
		if (mob.Rank != MonsterRank.Boss)
			return;

		// Get top damage dealer
		var combatComponent = mob.Components.Get<CombatComponent>();
		var topAttacker = combatComponent?.GetTopAttackerByDamage();

		if (topAttacker == null)
			return;

		// Resolve to character (handle summons/pets)
		var mvpCharacter = GetOwningCharacter(topAttacker);
		if (mvpCharacter == null)
			return;

		// Verify character is still on the same map
		if (mvpCharacter.Map != mob.Map)
			return;

		// Show MVP emoticon
		ShowMvpEffect(mvpCharacter);
	}

	/// <summary>
	/// Gets the character that owns this combat entity.
	/// Returns the master for summons/pets, or the entity itself if it's a character.
	/// </summary>
	private Character GetOwningCharacter(ICombatEntity entity)
	{
		if (entity is Character character)
			return character;

		// Check if this is a summon with a master
		var aiComponent = entity.Components.Get<AiComponent>();
		if (aiComponent?.Script.GetMaster() is Character master)
			return master;

		return null;
	}

	/// <summary>
	/// Shows the MVP emoticon above the player's head using the EffectsComponent.
	/// </summary>
	private void ShowMvpEffect(Character character)
	{
		var effectKey = "boss_mvp";
		// Add to effects component so players entering visibility range see it
		var effectsComponent = character.Components.Get<EffectsComponent>();
		if (effectsComponent != null)
		{
			var effect = new EmoticonEffect(MvpEmoticon, MvpEmoticonDuration);

			// Remove any existing effect from a previous boss kill
			effectsComponent.RemoveEffect(effectKey);

			effectsComponent.AddEffect(effectKey, effect);

			// Schedule removal after duration
			_ = RemoveEffectAfterDelay(character, effectKey, MvpEmoticonDuration);
		}
		else
		{
			// Fallback to direct send if no effects component
			Send.ZC_SHOW_EMOTICON(character, MvpEmoticon, MvpEmoticonDuration);
		}
	}

	/// <summary>
	/// Removes an effect after a delay.
	/// </summary>
	private async Task RemoveEffectAfterDelay(Character character, string effectName, TimeSpan delay)
	{
		await Task.Delay(delay);

		if (character?.Map != null)
		{
			var effectsComponent = character.Components.Get<EffectsComponent>();
			effectsComponent?.RemoveEffect(effectName);
		}
	}
}
