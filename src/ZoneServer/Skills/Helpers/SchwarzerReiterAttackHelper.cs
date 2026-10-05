using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Helpers
{
	public static class SchwarzerReiterAttackHelper
	{
		public static void UpdateMainAttack(Character character)
		{
			var hasLimacon = character.TryGetBuff(BuffId.Limacon_Buff, out _);
			var hasSerialBullet = character.TryGetBuff(BuffId.DoubleBullet_Toggle_Buff, out _);

			if (hasLimacon)
			{
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.Pistol_Attack2);
			}
			else if (hasSerialBullet)
			{
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.DoubleBullet_Attack);
			}
			else
			{
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.Pistol_Attack);
			}

			Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.None);
		}
	}
}
