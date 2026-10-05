//--- Melia Script ----------------------------------------------------------
// Pose/Animation Item Scripts
//--- Description -----------------------------------------------------------
// Item scripts that play character poses and animations.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;
using Yggdrasil.Logging;

public class PoseItemScripts : GeneralScript
{
	/// <summary>
	/// Plays a selectable animation/pose on the character.
	/// Used by toy items like ceremonial knife, surf board, etc.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="item"></param>
	/// <param name="animationString">Animation identifier in format "POSE;animname"</param>
	/// <param name="numArg1"></param>
	/// <param name="numArg2"></param>
	/// <returns></returns>
	[ScriptableFunction]
	public ItemUseResult SCR_USE_ITEM_PLAY_SELECT_ANIM(Character character, Item item, string animationString, float numArg1, float numArg2)
	{
		// Animation string format: "POSE;animname"
		// e.g., "POSE;mercenarytoy", "POSE;surfingboard", "POSE;cocktail"
		if (string.IsNullOrEmpty(animationString))
			return ItemUseResult.Fail;

		var parts = animationString.Split(';');
		if (parts.Length < 2)
		{
			Log.Warning("SCR_USE_ITEM_PLAY_SELECT_ANIM: Invalid animation string format: {0}", animationString);
			return ItemUseResult.Fail;
		}

		var animationType = parts[0];
		var animationName = parts[1];

		if (animationType == "POSE")
		{
			// Play effect with animation name
			// The animation is handled client-side based on the effect name
			character.PlayEffect($"F_pc_artefact_{animationName}", 3.0f, 1, EffectLocation.Bottom, 1);
		}

		return ItemUseResult.Okay;
	}
}
