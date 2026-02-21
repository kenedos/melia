using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.Handlers.Common
{
	/// <summary>
	/// Handle for the Freeze Debuff. Frozen solid.
	/// </summary>
	[BuffHandler(BuffId.Freeze, BuffId.UC_freeze)]
	public class Freeze : BuffHandler, IBuffCombatDefenseBeforeCalcHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Frozen);
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Frozen);
		}

		/// <summary>
		/// Applies the debuff's effect during the combat calculations.
		/// </summary>
		/// <param name="buff"></param>
		/// <param name="attacker"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		/// <param name="modifier"></param>
		/// <param name="skillHitResult"></param>
		public void OnDefenseBeforeCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (ZoneServer.Instance.Conf.World.FreezeAffectsElement)
				modifier.DefenseAttribute = AttributeType.Ice;
		}
	}
}
