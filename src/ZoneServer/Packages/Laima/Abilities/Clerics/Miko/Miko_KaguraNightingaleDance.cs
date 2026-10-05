using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Abilities;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Abilities.Handlers.Clerics.Miko
{
	/// <summary>
	/// Miko8 - Kagura: Nightingale Dance.
	///
	/// Enquanto estiver ativada:
	/// - reduz a resistência crítica dos inimigos;
	/// - faz os aliados com Kagura ignorarem parte da DEF e MDEF
	///   dos inimigos afetados.
	///
	/// Os cálculos são realizados diretamente pela skill Kagura
	/// e pelo cálculo de dano.
	/// </summary>
	[Package("laima")]
	[AbilityHandler(AbilityId.Miko8)]
	public class Miko_KaguraNightingaleDanceAbility : AbilityPropertyHandler
	{
		public override void OnActivate(Ability ability, Character character)
		{
		}

		public override void OnDeactivate(Ability ability, Character character)
		{
		}
	}
}
