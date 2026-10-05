using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Buffs.Scouts.Shinobi
{
	/// <summary>

	/// Handler for Bunshin no Jutsu buff.

	/// Applies the effects of Bunshin no Jutsu: Tai while the buff is active.

	/// </summary>

	[Package("laima")]
	[BuffHandler(BuffId.Bunshin_Buff)]
	public class Shinobi_BunshinNoJutsu_BuffOverride : BuffHandler, IBuffBeforeKnockbackHandler, IBuffBeforeKnockdownHandler, IBuffOnHitInfoCreatedHandler
	{
		private const float MoveSpeedBonus = 10f;
		private const float DamageReduction = 0.30f;

		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (activationType != ActivationType.Start || buff.Target is not Character character || character is DummyCharacter || !character.IsAbilityActive(AbilityId.Shinobi17))
				return;

			character.Properties.Modify(PropertyName.MSPD_Bonus, MoveSpeedBonus);
			buff.Vars.Set("BunshinTai.Applied", 1);

			Send.ZC_MOVE_SPEED(character);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character || character is DummyCharacter || buff.Vars.GetInt("BunshinTai.Applied") != 1)
				return;

			character.Properties.Modify(PropertyName.MSPD_Bonus, -MoveSpeedBonus);

			Send.ZC_MOVE_SPEED(character);
		}

		public KnockResult OnBeforeKnockback(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return this.IsTaiActive(buff) ? KnockResult.Prevent : default;
		}

		public KnockResult OnBeforeKnockdown(Buff buff, ICombatEntity attacker, ICombatEntity target)
		{
			return this.IsTaiActive(buff) ? KnockResult.Prevent : default;
		}

		public void OnHitInfoCreated(Buff buff, SkillHitInfo skillHitInfo)
		{
			if (!this.IsTaiActive(buff))
				return;

			skillHitInfo.HitInfo.Damage *= 1f - DamageReduction;
		}

		private bool IsTaiActive(Buff buff)
		{
			if (buff.Target is DummyCharacter clone)
				return clone.Owner is Character owner && owner.IsAbilityActive(AbilityId.Shinobi17);

			return buff.Target is Character character && character.IsAbilityActive(AbilityId.Shinobi17);
		}
	}
}
