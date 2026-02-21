using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.Buffs.Handlers
{
	/// <summary>
	/// Handle for the Reward Buff, which periodically generates threat on
	/// nearby enemies based on the character's held Silver.
	/// </summary>
	[BuffHandler(BuffId.Warrior_Reward_Buff)]
	public class Warrior_Reward_Buff : BuffHandler
	{
		private const int ThreatRadius = 200;
		private const int BaseHate = 50;
		private const float HatePerSilver = 0.01f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Play the initial visual effect on the buff target.
			buff.Target.PlayEffect("F_warrior_reward_shot_lineup", 0.3f);
		}

		public override void WhileActive(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			// Calculate the amount of hate to generate.
			var silverAmount = character.Inventory.CountItem(ItemId.Silver);
			var hateAmount = (int)(BaseHate + silverAmount * HatePerSilver);

			// Find all nearby enemies.
			var nearbyEnemies = character.Map.GetAttackableEnemiesIn(
				character,
				new CircleF(character.Position, ThreatRadius)
			);

			// Add hate to each enemy.
			foreach (var enemy in nearbyEnemies)
			{
				enemy.InsertHate(character, hateAmount);
			}
		}
	}
}
