using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Scouts.Schwarzereiter
{
	/// <summary>
	/// Handler for the Limacon buff, swaps main attack skill.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Limacon_Buff)]
	public class SchwarzerReiter_Limacon_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			var caster = buff.Caster;

			if (caster is Character character)
			{
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.Pistol_Attack2);
				Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.None);
			}
		}

		public override void OnEnd(Buff buff)
		{
			var caster = buff.Caster;

			if (caster is Character character)
			{
				Send.ZC_NORMAL.SetMainAttackSkill(character, SkillId.None);
				Send.ZC_NORMAL.SetSubAttackSkill(character, SkillId.None);
			}
		}
	}
}
