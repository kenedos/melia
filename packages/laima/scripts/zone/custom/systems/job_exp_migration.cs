using Melia.Shared.Game.Const;
using Melia.Shared.Scripting;
using Melia.Zone;
using Melia.Zone.Events.Arguments;
using Melia.Zone.Network;
using Melia.Zone.Scripting;

public class JobExpMigrationScript : GeneralScript
{
	[On("PlayerLoggedIn")]
	public void OnPlayerLoggedIn(object sender, PlayerEventArgs args)
	{
		var character = args.Character;
		var vars = character.Variables.Perm;
	
		if (vars.GetBool("Laima.Custom.JobExpMigration_2026_02_12", false))
			return;
	
		character.ResetSkills();
	
		// Stat point migration: stats_per_level changed from 1 to 3.
		// Grant the missing (level - 1) * 2 stat points and reset stats.
		var level = character.Level;
		var missingStatPoints = (level - 1) * 2;
	
		if (missingStatPoints > 0)
			character.Properties.Modify(PropertyName.StatByLevel, missingStatPoints);
	
		character.ResetStats();
	
		Send.ZC_OBJECT_PROPERTY(character);
	
		vars.SetBool("Laima.Custom.JobExpMigration_2026_02_12", true);
	}
}
