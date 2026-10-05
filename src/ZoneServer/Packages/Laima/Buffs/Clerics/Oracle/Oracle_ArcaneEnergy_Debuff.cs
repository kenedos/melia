using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Buffs.Handlers.Clerics.Oracle
{
	/// <summary>
	/// Handle for the Arcane Energy debuff. On activation, if the
	/// caster has Oracle1 ability, deals Holy TrueDamage to the caster.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.ArcaneEnergy_Debuff)]
	public class Oracle_ArcaneEnergy_DebuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Caster is not ICombatEntity caster)
				return;

			var abilLevel = caster.GetAbilityLevel(AbilityId.Oracle1);
			if (abilLevel > 0)
				caster.TakeSimpleHit(abilLevel, buff.Target, SkillId.Oracle_ArcaneEnergy);
		}

		public override void OnEnd(Buff buff)
		{
		}
	}
}
