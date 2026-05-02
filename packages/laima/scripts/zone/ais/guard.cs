using System.Collections;
using Melia.Shared.Game.Const;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;

[Ai("Guard")]
public class GuardAiScript : AiScript
{
	private const SkillId AttackSkillId = SkillId.Mon_Mandara_soldier_Skill_1;

	protected override void Setup()
	{
		this.MaxChaseDistance = 150;
		this.MaxRoamDistance = 50;
		this.SetAggroRange(150f);
		this.SetTendency(TendencyType.Aggressive);

		During("Idle", CheckEnemies);
		During("Attack", CheckTarget);
		During("Attack", CheckLeash);
	}

	private void CheckLeash()
	{
		if (this.IsBusyWithSkill())
			return;

		if (this.Entity is not Mob mob)
			return;

		if (mob.Position.Get2DDistance(mob.SpawnPosition) > this.MaxChaseDistance)
		{
			this.RemoveAllHate();
			this.StartRoutine("ReturnHome", this.ReturnHome(clearAllHateImmediately: true));
		}
	}

	protected override void Root()
	{
		StartRoutine("Idle", Idle());
	}

	public override bool IsHostileTowards(ICombatEntity otherEntity)
	{
		if (otherEntity is not Mob mob || mob.IsDead)
			return false;

		if (mob.CombatState.AttackState)
			return true;

		if (mob.Components.TryGet<AiComponent>(out var ai) && ai.Script.GetMaxHate() > 100)
			return true;

		return false;
	}

	protected override bool TryGetRandomSkill(out Skill skill)
	{
		skill = null;

		if (!this.Entity.Components.TryGet<BaseSkillComponent>(out var skills))
			return false;

		if (this.Entity.IsOnCooldown(AttackSkillId))
			return false;

		if (!skills.Has(AttackSkillId))
		{
			skill = new Skill(this.Entity, AttackSkillId, 1);
			skills.AddSilent(skill);
		}
		else
		{
			skills.TryGet(AttackSkillId, out skill);
		}

		return true;
	}
}
