using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Clerics.Miko
{
	/// <summary>
	/// Miko15 - Kagura: Ken Enhance.
	///
	/// Aumenta o dano da Kagura: Ken em 0,5% por nível.
	/// No nível 100, concede mais 10%:
	///
	/// 50% + 10% = 60% de aumento total.
	///
	/// O cálculo é realizado diretamente pela skill Kagura.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Miko15)]
	public class Miko_KaguraKenEnhanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
