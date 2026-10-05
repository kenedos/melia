using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Clerics.Miko
{
	/// <summary>
	/// Miko18 - Kagura: Ken.
	///
	/// Quando ativada, Kagura deixa de conceder o buff aos aliados
	/// e passa a causar dano em área uma vez por segundo durante
	/// os 15 segundos de channeling, atingindo até 10 inimigos.
	///
	/// O comportamento é executado diretamente pela skill Kagura.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Miko18)]
	public class Miko_KaguraKenAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
