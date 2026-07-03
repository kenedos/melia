using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Wizards.RuneCaster
{
	[Package("laima")]
	[BuffHandler(BuffId.RuneOfProtection_Giant_Buff)]
	public class RuneCaster_RuneOfProtectionGiant_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Target is not Character character)
				return;

			var defBonus = character.Properties.GetFloat(PropertyName.DEF) * 0.20f;
			var mdefBonus = character.Properties.GetFloat(PropertyName.MDEF) * 0.20f;
			var hpBonusRate = 0.20f;

			if (character.Abilities.TryGet(AbilityId.RuneCaster4, out var ability) && ability.Active)
				hpBonusRate += 0.20f;

			var hpBonus = character.Properties.GetFloat(PropertyName.MHP) * hpBonusRate;
			var moveSpeedBonus = 20f;

			buff.NumArg2 = defBonus;
			buff.NumArg3 = mdefBonus;
			buff.NumArg4 = hpBonus;
			buff.NumArg5 = moveSpeedBonus;

			character.Properties.Modify(PropertyName.DEF_BM, defBonus);
			character.Properties.Modify(PropertyName.MDEF_BM, mdefBonus);
			character.Properties.Modify(PropertyName.MHP_BM, hpBonus);
			character.Properties.Modify(PropertyName.MSPD_BM, moveSpeedBonus);

			Send.ZC_OBJECT_PROPERTY(
				character,
				PropertyName.DEF,
				PropertyName.DEF_BM,
				PropertyName.MDEF,
				PropertyName.MDEF_BM,
				PropertyName.MHP,
				PropertyName.MHP_BM);

			Send.ZC_MOVE_SPEED(character);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			character.Properties.Modify(PropertyName.DEF_BM, -buff.NumArg2);
			character.Properties.Modify(PropertyName.MDEF_BM, -buff.NumArg3);
			character.Properties.Modify(PropertyName.MHP_BM, -buff.NumArg4);
			character.Properties.Modify(PropertyName.MSPD_BM, -buff.NumArg5);

			Send.ZC_OBJECT_PROPERTY(
				character,
				PropertyName.DEF,
				PropertyName.DEF_BM,
				PropertyName.MDEF,
				PropertyName.MDEF_BM,
				PropertyName.MHP,
				PropertyName.MHP_BM);

			Send.ZC_MOVE_SPEED(character);
		}
	}
}
