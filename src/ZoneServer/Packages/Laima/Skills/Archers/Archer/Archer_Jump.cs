using System;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Skills.Handlers.Archers.Archer
{
	/// <summary>
	/// Handler for the Archer skill Leap.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Archer_Jump)]
	public class Archer_JumpOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles skill, moving the character back.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			if (caster is Character character && character.IsWearingArmorOfType(ArmorMaterialType.Iron))
			{
				caster.ServerMessage(Localization.Get("Can't use while wearing [Plate] armor."));
				Send.ZC_SKILL_DISABLE(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(false);

			Send.ZC_SKILL_READY(caster, skill, originPos, farPos);

			caster.StartBuff(BuffId.Skill_NoDamage_Buff, TimeSpan.FromSeconds(1), caster);

			var distance = this.GetJumpDistance(caster, skill);
			var targetPos = caster.Position.GetRelative(caster.Direction.Backwards, distance);
			targetPos = caster.Map.Ground.GetLastValidPosition(caster.Position, targetPos);

			caster.Position = targetPos;

			Send.ZC_NORMAL.LeapJump(caster, targetPos, 20f, 0.1f, 0.1f, 1f, 0.2f, 1f);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
		}

		/// <summary>
		/// Returns the distance to jump back.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		private float GetJumpDistance(ICombatEntity caster, Skill skill)
		{
			var evasion = caster.Properties.GetFloat(PropertyName.DR);
			var level = skill.Level;

			return Math.Min(240, (25 * level) + ((skill.SkillFactor / 100) * evasion));
		}
	}
}
