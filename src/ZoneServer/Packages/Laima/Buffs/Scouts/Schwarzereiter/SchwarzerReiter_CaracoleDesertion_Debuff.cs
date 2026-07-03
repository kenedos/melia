using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Caracole: Desertion debuff.
	/// Reduces the target's defense while the debuff is active.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Caracole_Silence_Debuff)]
	public class SchwarzerReiter_Caracole_Desertion_DebuffOverride : BuffHandler
	{
		private const string DefenseReductionVar = "Melia.CaracoleDesertion.DefenseReduction";

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Calculate the defense reduction from the ability level.
			// Example formula: -5% defense per ability level.
			var reductionRate = 0.05f * buff.NumArg1;

			// Read the target's current defense at the moment the debuff is applied.
			var currentDefense = buff.Target.Properties.GetFloat(PropertyName.DEF) + buff.Target.Properties.GetFloat(PropertyName.DEF_BM);

			// Store the exact reduction value so it can be removed safely later.
			var defenseReduction = currentDefense * reductionRate;
			buff.Vars.SetFloat(DefenseReductionVar, defenseReduction);

			// Apply the defense reduction through DEF_BM.
			buff.Target.Properties.Modify(PropertyName.DEF_BM, -defenseReduction);

			// Update the client with the new defense values.
			if (buff.Target is Character targetCharacter)
			{
				Send.ZC_OBJECT_PROPERTY(
					targetCharacter,
					PropertyName.DEF,
					PropertyName.DEF_BM);
			}
		}

		public override void OnEnd(Buff buff)
		{
			// Remove exactly the same defense value that was applied on activation.
			if (!buff.Vars.TryGetFloat(DefenseReductionVar, out var defenseReduction))
				return;

			buff.Target.Properties.Modify(PropertyName.DEF_BM, defenseReduction);

			if (buff.Target is Character targetCharacter)
			{
				Send.ZC_OBJECT_PROPERTY(
					targetCharacter,
					PropertyName.DEF,
					PropertyName.DEF_BM);
			}
		}
	}
}
