using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Buffs.Handlers;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Components;

namespace Melia.Zone.Buffs.HandlersOverrides.Scouts.Rogue
{
	/// <summary>
	/// Handler for the Lachrymator debuff. Resets all hate on the
	/// target mob and prevents hate gain. Removed when hit.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.Lachrymator_Debuff)]
	public class Lachrymator_DebuffOverride : BuffHandler, IBuffCombatDefenseAfterCalcHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			buff.Target.AddState(StateType.Held);

			if (buff.Target.Components.TryGet<AiComponent>(out var ai))
				ai.Script.QueueEventAlert(new HateResetAlert());
		}

		public override void OnEnd(Buff buff)
		{
			buff.Target.RemoveState(StateType.Held);
		}

		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			target.StopBuff(BuffId.Lachrymator_Debuff);
		}
	}
}
