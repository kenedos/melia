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
	
		if (vars.GetBool("Laima.Custom.JobExpMigration_2026_03_17", false))
			return;
	
		character.ResetSkills();

		if (!vars.GetBool("Laima.Custom.JobExpMigration_2026_02_12", false))
		{
			var level = character.Level;
			var missingStatPoints = (level - 1) * 2;

			if (missingStatPoints > 0)
				character.Properties.Modify(PropertyName.StatByLevel, missingStatPoints);

			character.ResetStats();
			vars.SetBool("Laima.Custom.JobExpMigration_2026_02_12", true);
		}
	
		Send.ZC_OBJECT_PROPERTY(character);
	
		vars.SetBool("Laima.Custom.JobExpMigration_2026_03_17", true);
	}
}
