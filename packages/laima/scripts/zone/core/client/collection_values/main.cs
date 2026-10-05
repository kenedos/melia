using Melia.Zone.Scripting;
using Melia.Zone.World.Actors.Characters;

public class CollectionValuesClientScript : ClientScript
{
    protected override void Load()
    {
        this.LoadAllScripts();
    }

    protected override void Ready(Character character)
    {
        this.SendAllScripts(character);
    }
}
