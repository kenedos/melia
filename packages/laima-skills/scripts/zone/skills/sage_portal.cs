//--- Melia Script ----------------------------------------------------------
// Sage Portal
//--- Description -----------------------------------------------------------
// Lifts the cooldowns of a Sage's portals that ran out while offline.
//---------------------------------------------------------------------------

using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Helpers;

public class SagePortalScript : GeneralScript
{
	[On("PlayerReady")]
	private void OnPlayerReady(object sender, PlayerEventArgs args)
	{
		if (args.Character.Jobs.Has(JobId.Sage))
			SageSkillHelper.RefreshPortalCooldowns(args.Character);
	}
}
