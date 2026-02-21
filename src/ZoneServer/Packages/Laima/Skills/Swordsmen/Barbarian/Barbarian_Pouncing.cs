using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Barbarian
{
	/// <summary>
	/// Handler for the Barbarian skill Pouncing
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Barbarian_Pouncing)]
	public class Barbarian_PouncingOverride : IDynamicCasted
	{
		/// <summary>
		/// Called when the user starts casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);

			caster.StartBuff(BuffId.Pouncing_Buff, TimeSpan.Zero);

			var padName = PadName.Barbarian_Pouncing;
			var length = 70f;

			// Barbarian41 uses a different pad, which has double range
			// but you can't move
			if (caster.IsAbilityActive(AbilityId.Barbarian41))
			{
				padName = PadName.Barbarian_Pouncing_Abil;
				length = 90f;
			}

			var pad = new Pad(padName, caster, skill, new Square(caster.Position, caster.Direction, length, 35));
			pad.Position = caster.Position;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(3500);
			pad.Movement.Speed = 100;

			caster.Map.AddPad(pad);
		}

		/// <summary>
		/// Called when the user stops casting the skill.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
			caster.StopBuff(BuffId.Pouncing_Buff);
		}
	}
}
