using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Buffs;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Clerics.Zealot
{
	/// <summary>
	/// Handler for the Zealot skill Beady Eyed, which moves the Zealot
	/// behind the targeted enemy in an instant.
	/// </summary>
	/// <remarks>
	/// The maximum distance is the skill's factor. With Beady Eyed: Sudden
	/// Attack the Zealot gains a minimum critical chance afterwards.
	/// </remarks>
	[Package("laima-skills")]
	[SkillHandler(SkillId.Zealot_BeadyEyed)]
	public class Zealot_BeadyEyedOverride : IGroundSkillHandler
	{
		private const float DistanceBehindTarget = 15f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			var maxDistance = skill.Properties.GetFloat(PropertyName.SkillFactor);

			if (target == null || target.IsDead || !caster.Position.InRange2D(target.Position, maxDistance))
			{
				Send.ZC_SKILL_CAST_CANCEL(caster);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, target.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, target.Position, ForceId.GetNew(), null);

			var destination = target.Position.GetRelative(target.Direction.Backwards, DistanceBehindTarget);
			if (!caster.Map.Ground.TryGetNearestValidPosition(destination, out destination))
				destination = target.Position;

			caster.SetPosition(destination);
			caster.TurnTowards(target);

			if (caster.IsAbilityActive(AbilityId.Zealot8))
				caster.StartBuff(BuffId.BeadyEyed_Debuff, skill.Level, 0, Buff.DefaultDuration, caster, skill.Id);
		}
	}
}
