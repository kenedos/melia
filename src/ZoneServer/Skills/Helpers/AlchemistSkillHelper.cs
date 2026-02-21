using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Scripting.Shared;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Items;
using Yggdrasil.Extensions;
using Yggdrasil.Util;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Helpers
{
	public static class AlchemistSkillHelper
	{
		public static HashSet<SkillId> GetHomunculusSkills()
		{
			return new HashSet<SkillId>
			{
				SkillId.Wizard_Sleep,
				SkillId.Wizard_Lethargy,
				SkillId.Wizard_MagicMissile,
				SkillId.Pyromancer_FireBall,
				SkillId.Pyromancer_EnchantFire,
				SkillId.Pyromancer_FirePillar,
				SkillId.Cryomancer_IceBolt,
				SkillId.Cryomancer_IceWall,
				SkillId.Cryomancer_IciclePike,
				SkillId.Cryomancer_SubzeroShield,
				SkillId.Cryomancer_Gust,
				SkillId.Linker_JointPenalty,
				SkillId.Linker_Physicallink,
				SkillId.Psychokino_Swap,
				SkillId.Psychokino_Teleportation,
				SkillId.Psychokino_Raise,
				SkillId.Psychokino_MagneticForce,
				SkillId.Elementalist_StoneCurse,
				SkillId.Elementalist_Rain,
				SkillId.Chronomancer_Quicken,
				SkillId.Chronomancer_Slow,
				SkillId.Chronomancer_Stop,
				SkillId.Thaumaturge_ShrinkBody,
				SkillId.Thaumaturge_SwellBody
			};
		}

		public static void HomunculusSkillUpdate(Character pc, Mob homun)
		{
			if (!pc.TryGetBuff(BuffId.Homunculus_Skill_Buff, out var buff))
				return;

			if (!homun.Components.TryGet<BaseSkillComponent>(out var skills))
			{
				skills = new BaseSkillComponent(homun);
				homun.Components.Add(skills);
			}

			skills.Clear();

			if (buff.NumArg2 != 0)
				skills.AddSilent(new Skill(homun, (SkillId)buff.NumArg2));
			if (buff.NumArg3 != 0)
				skills.AddSilent(new Skill(homun, (SkillId)buff.NumArg3));
			if (buff.NumArg4 != 0)
				skills.AddSilent(new Skill(homun, (SkillId)buff.NumArg4));
			if (buff.NumArg5 != 0)
				skills.AddSilent(new Skill(homun, (SkillId)buff.NumArg5));
		}
	}
}
