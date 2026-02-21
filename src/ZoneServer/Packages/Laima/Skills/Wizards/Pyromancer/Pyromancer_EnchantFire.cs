using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.MonsterSkillHelper;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using static Melia.Zone.Skills.Helpers.SkillTargetHelper;
using static Melia.Zone.Skills.Helpers.SkillUtilHelper;
using Melia.Zone.World;
using Melia.Zone.Buffs;

namespace Melia.Zone.Skills.Handlers.Pyromancer
{
	/// <summary>
	/// Handler for the Pyromancer skill Enchant Fire.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Pyromancer_EnchantFire)]
	public class Pyromancer_EnchantFireOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const float BuffRange = 300;
		private const int BuffDurationSeconds = 300;
		private const float DamageMultiplierIncreasePerLevel = 0.02f;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handle Skill Behavior
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			var damageMultiplierIncrease = skill.Level * DamageMultiplierIncreasePerLevel;

			var byAbility = 1f;
			if (caster.TryGetActiveAbility(AbilityId.Pyromancer16, out var ability))
				byAbility += ability.Level * 0.005f;
			damageMultiplierIncrease *= byAbility;

			caster.StartBuff(BuffId.EnchantFire_Buff, skill.Level, damageMultiplierIncrease, TimeSpan.FromSeconds(BuffDurationSeconds), caster);

			// Buff party members
			if (caster is Character character)
			{
				var party = character.Connection.Party;
				if (party != null)
				{
					var members = caster.Map.GetPartyMembersInRange(character, BuffRange, true);
					foreach (var member in members)
					{
						if (member == caster)
							continue;
						member.StartBuff(BuffId.EnchantFire_Buff, skill.Level, damageMultiplierIncrease, TimeSpan.FromSeconds(BuffDurationSeconds), caster);
					}
				}
			}

			// Debuff enemies with ability
			if (caster.TryGetActiveAbility(AbilityId.Pyromancer6, out ability))
			{
				var pad = SkillCreatePad(caster, skill, caster.Position, 0, PadName.Wizard_New_EnchantFire);
				if (pad == null)
					return;
				pad.Trigger.MaxConcurrentUseCount = ability.Level;
				pad.NumArg2 = ability.Level;
			}
		}
	}
}
