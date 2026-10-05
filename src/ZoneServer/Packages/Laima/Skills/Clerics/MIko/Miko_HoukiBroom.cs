using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Miko
{
	/// <summary>
	/// Sweeping.
	/// Creates a purified area that increases the level of magic circles
	/// used inside it by 2 and protects allies from removable debuffs.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Miko_HoukiBroom)]
	public class Miko_HoukiBroom : IGroundSkillHandler, IDynamicCasted
	{
		private const float MaximumCastRange = 100f;
		private const float PadRange = 40f;
		private static readonly TimeSpan CastDelay = TimeSpan.FromMilliseconds(200);

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (originPos.Get2DDistance(farPos) > MaximumCastRange)
			{
				character.ServerMessage(Localization.Get("Too far away."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.CreateSweepingPad(skill, caster, farPos));
		}

		public void StartDynamicCast(Skill skill, Character character, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, Character character, float castTime)
		{
		}

		private async Task CreateSweepingPad(Skill skill, ICombatEntity caster, Position position)
		{
			await skill.Wait(CastDelay);

			SkillRemovePad(caster, skill);
			SkillCreatePad(caster, skill, position, 0f, PadName.Miko_HoukiBroom, range: PadRange);

			caster.SetAttackState(false);
		}
	}
}
