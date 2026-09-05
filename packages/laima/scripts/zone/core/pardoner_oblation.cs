//--- Melia Script ----------------------------------------------------------
// Oblation Offering Box
//--- Description -----------------------------------------------------------
// Handles the offering box window a Pardoner opens from the Oblation skill
// and at the church, where the items donated to them are given away or
// taken back.
//---------------------------------------------------------------------------

using System;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Scripting.Shortcuts;

public class PardonerOblationScript : GeneralScript
{
	private const int ActionSelect = 0;
	private const int ActionRetrieve = 1;
	private const int ActionDonate = 2;
	private const int ActionOpen = 3;
	private const int ActionRefresh = 4;
	private const int ActionShopInfo = 5;

	/// <summary>
	/// Handles the offering box window's requests.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="numArg1">The action to take.</param>
	/// <param name="numArg2">The box position, or the box owner's handle.</param>
	/// <param name="numArg3">Whether the window was opened at the church.</param>
	/// <returns></returns>
	[ScriptableFunction]
	public CustomCommandResult SCR_LAIMA_OBLATION_BOX(Character character, int numArg1, int numArg2, int numArg3)
	{
		// Anyone can look into a box they're offering to; the rest is the
		// owner acting on their own.
		if (numArg1 == ActionShopInfo)
			return this.SendShopInfo(character, numArg2);

		if (!character.TryGetSkill(SkillId.Pardoner_Oblation, out _))
			return CustomCommandResult.Fail;

		switch (numArg1)
		{
			case ActionOpen:
				PardonerSkillHelper.SendOblationBox(character, numArg3 == 1);
				return CustomCommandResult.Okay;

			case ActionRefresh:
				PardonerSkillHelper.RefreshOblationBox(character);
				return CustomCommandResult.Okay;

			case ActionSelect:
			{
				var selection = PardonerSkillHelper.GetSelection(character);
				if (!selection.Contains(numArg2))
					selection.Add(numArg2);
				return CustomCommandResult.Okay;
			}

			case ActionRetrieve:
				PardonerSkillHelper.RetrieveSelection(character);
				PardonerSkillHelper.RefreshOblationBox(character);
				return CustomCommandResult.Okay;

			case ActionDonate:
			{
				if (PardonerSkillHelper.GetChurchDonationCooldown(character) > TimeSpan.Zero)
				{
					PardonerSkillHelper.GetSelection(character).Clear();
					return CustomCommandResult.Fail;
				}

				var silver = PardonerSkillHelper.DonateSelection(character);
				PardonerSkillHelper.RefreshOblationBox(character);

				if (silver > 0)
					character.ServerMessage(L("The church accepts your offerings and gives you {0} silver."), silver);

				return CustomCommandResult.Okay;
			}
		}

		return CustomCommandResult.Fail;
	}

	/// <summary>
	/// Sends the contents and capacity of the offering box belonging to
	/// the character with the given handle to the given donor.
	/// </summary>
	/// <param name="donor"></param>
	/// <param name="ownerHandle"></param>
	/// <returns></returns>
	private CustomCommandResult SendShopInfo(Character donor, int ownerHandle)
	{
		if (!donor.Map.TryGetCharacter(ownerHandle, out var pardoner))
			return CustomCommandResult.Fail;

		if (!pardoner.TryGetSkill(SkillId.Pardoner_Oblation, out _))
			return CustomCommandResult.Fail;

		PardonerSkillHelper.SendOblationShop(donor, pardoner);

		return CustomCommandResult.Okay;
	}
}
