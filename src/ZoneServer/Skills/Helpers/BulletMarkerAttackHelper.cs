using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Helpers
{
	public static class BulletMarkerAttackHelper
	{
		public static void UpdateMainAttack(Character character)
		{
			if (character.TryGetBuff(BuffId.DoubleGunStance_Buff, out _))
			{
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.DoubleGun_Attack);
				Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.None);
				return;
			}

			Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.Pistol_Attack);
			Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.None);
		}
	}
}
