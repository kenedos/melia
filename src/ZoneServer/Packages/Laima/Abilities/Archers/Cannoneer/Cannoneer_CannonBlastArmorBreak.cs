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
	/// Cannon Blast: Armor Break
	/// Concede 10% de chance de aplicar Armor Break por 3 segundos.
	/// A chance é calculada uma vez por inimigo em cada uso da skill.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Cannoneer8)]
	public class Cannoneer_CannonBlastArmorBreakAbility : AbilityPropertyHandler
	{
		private const int ArmorBreakChance = 10;
		private static readonly TimeSpan ArmorBreakDuration = TimeSpan.FromSeconds(3);

		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}

		public static bool TryApplyArmorBreak(Character character, ICombatEntity target)
		{
			if (target == null || target.IsDead || !character.TryGetActiveAbilityLevel(AbilityId.Cannoneer8, out _))
				return false;

			if (RandomProvider.Get().Next(100) >= ArmorBreakChance)
				return false;

			target.StartBuff(BuffId.ArmorBreak, ArmorBreakDuration, character);
			return true;
		}
	}
}
