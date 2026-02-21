using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Items;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// A function that handles the usage of an item.
	/// </summary>
	/// <param name="character">The character who used the card.</param>
	/// <param name="target">The target which the card is used on.</param>
	/// <param name="card">The card that is being used.</param>
	/// <param name="strArg">First string argument, as defined in the card data.</param>
	/// <param name="strArg2">Second argument argument, as defined in the card data.</param>
	/// <param name="strArg3">Third number argument, as defined in the card data.</param>
	/// <returns></returns>
	public delegate void CardScriptFunc(Character character, ICombatEntity target, Item card, float typeValue, string strArg, string strArg2, string strArg3);
}
