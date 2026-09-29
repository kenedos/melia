using System.Globalization;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.World.Quests.Rewards
{
	/// <summary>
	/// A reward that permanently raises a character property for as long
	/// as the quest stays completed.
	/// </summary>
	public class PropertyReward : QuestReward
	{
		/// <summary>
		/// Returns the name of the property the reward raises.
		/// </summary>
		public string Property { get; }

		/// <summary>
		/// Returns the amount added to the property.
		/// </summary>
		public float Amount { get; }

		/// <summary>
		/// Returns the icon to display for this reward.
		/// </summary>
		public override string Icon => "expup_img";

		/// <summary>
		/// Creates a quest reward for a permanent property bonus.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <param name="amount"></param>
		public PropertyReward(string propertyName, float amount)
		{
			this.Property = propertyName;
			this.Amount = amount;
		}

		/// <summary>
		/// Makes the character recalculate its properties, which now
		/// include this reward.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="quest"></param>
		public override void Give(Character character, Quest quest)
		{
			character.InvalidateProperties();
		}

		/// <summary>
		/// Returns a string representation of the reward.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			var name = this.Property switch
			{
				PropertyName.MHP => Localization.Get("Max HP"),
				PropertyName.MSP => Localization.Get("Max SP"),
				PropertyName.MSTA => Localization.Get("Max Stamina"),
				PropertyName.MaxWeight => Localization.Get("Weight Limit"),
				_ => this.Property,
			};

			return string.Format(Localization.Get("{0} +{1}"), name, this.Amount.ToString(CultureInfo.InvariantCulture));
		}
	}
}
