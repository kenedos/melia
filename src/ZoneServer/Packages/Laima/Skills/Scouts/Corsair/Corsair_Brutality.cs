using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Skills.Handlers.Base;

namespace Melia.Zone.Skills.Handlers.Scouts.Corsair
{
	/// <summary>
	/// Handler for the passive Corsair skill Brutality.
	/// Buff application is handled by JollyRoger_Enemy_Debuff's combat
	/// modifier, which fires for all attackers hitting flagged enemies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Corsair_Brutality)]
	public class Corsair_BrutalityOverride : ISkillHandler
	{
	}
}
