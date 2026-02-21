using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Archers.Fletcher
{
	/// <summary>
	/// Handle for the Bodkin Debuff, which reduces the target's defense rate.
	/// </summary>
	/// <remarks>
	/// NumArg1: The skill level.
	/// </remarks>
	[Package("laima")]
	[BuffHandler(BuffId.Bodkin_Debuff)]
	public class Bodkin_DebuffOverride : BuffHandler
	{
		private const float DefRatePenaltyPerSkillLevel = 0.01f;
		private const float DefRatePenaltyPerAbilityLevel = 0.005f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var target = buff.Target;
			var skillLevel = buff.NumArg1;
			var abilityLevel = 0;

			if (buff.Caster is not ICombatEntity caster)
				return;

			if (caster.TryGetActiveAbilityLevel(AbilityId.Fletcher2, out var fletcherAbilityLevel))
			{
				abilityLevel = fletcherAbilityLevel;
			}

			var defensePenalty = (skillLevel * DefRatePenaltyPerSkillLevel) + (abilityLevel * DefRatePenaltyPerAbilityLevel);

			AddPropertyModifier(buff, target, PropertyName.DEF_RATE_BM, -defensePenalty);

			Send.ZC_NORMAL.PlayTextEffect(target, caster, "SHOW_BUFF_TEXT", (float)buff.Id, null, "Item");
		}

		public override void OnEnd(Buff buff)
		{
			RemovePropertyModifier(buff, buff.Target, PropertyName.DEF_RATE_BM);
		}
	}
}
