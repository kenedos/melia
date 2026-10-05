using System;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Util;

namespace Melia.Zone.Abilities.Handlers.Archers.Cannoneer
{
	/// <summary>

	/// Cannon Barrage: Stun

	/// Concede 5% de chance de aplicar Stun por 3 segundos.

	/// A chance é calculada uma vez por alvo em cada uso da skill.

	/// </summary>

	[Package("laima")]
	[AbilityHandler(AbilityId.Cannoneer12)]
	public class Cannoneer_CannonBarrageStunAbility : AbilityPropertyHandler
	{
		private const int StunChance = 5;
		private static readonly TimeSpan StunDuration = TimeSpan.FromSeconds(3);

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool TryApplyStun(Character character, ICombatEntity target)
		{
			if (target == null || target.IsDead || !character.TryGetActiveAbilityLevel(AbilityId.Cannoneer12, out _))
				return false;

			if (RandomProvider.Get().Next(100) >= StunChance)
				return false;

			target.StartBuff(BuffId.Stun, StunDuration, character);
			return true;
		}
	}
}
