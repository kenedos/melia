using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Helpers
{
	public static class NakMuayAttackHelper
	{
		public static void UpdateMainAttack(Character character)
		{
			if (character.TryGetBuff(BuffId.RamMuay_Buff, out _))
			{
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.NakMuay_Attack);
				Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.NakMuay_Attack2);
				return;
			}

			// Restore default sword attack when Ram Muay is disabled.
			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.Sword_Attack);
			Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.None);
		}
	}
}
