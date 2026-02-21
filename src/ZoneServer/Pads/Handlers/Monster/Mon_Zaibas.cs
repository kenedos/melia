using System;
using System.Threading.Tasks;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Pads.Helpers.PadHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Pads.Handlers
{
	[PadHandler(PadName.Mon_Zaibas)]
	public class Mon_Zaibas : ICreatePadHandler, IDestroyPadHandler, IUpdatePadHandler
	{
		public void Created(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;

			Send.ZC_NORMAL.PadUpdate(creator, pad, true);
			pad.SetRange(30f);
			pad.SetUpdateInterval(750);
			var value = 1;
			pad.Trigger.MaxActorCount = value;
			pad.Trigger.LifeTime = TimeSpan.FromMilliseconds(15000);
			pad.Trigger.MaxUseCount = 10;
		}

		public void Destroyed(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			Send.ZC_NORMAL.PadUpdate(creator, pad, false);
		}

		public void Updated(object sender, PadTriggerArgs args)
		{
			var pad = args.Trigger;
			var creator = args.Creator;
			var skill = pad.Skill;

			if (pad.IsDead)
				return;
			if (creator == null)
				return;

			var caster = (ICombatEntity)creator;
			if (caster.IsDead)
				return;
			if (skill == null)
				return;

			var targetCount = pad.Trigger.MaxActorCount;
			var targets = pad.Map.GetAttackableEnemiesIn(caster, pad.Area);
			foreach (var actor in targets)
			{
				if (actor is not ICombatEntity target || target.IsDead)
					continue;

				if (targetCount <= 0)
					break;

				if (!caster.IsEnemy(target))
					return;

				var modifier = new SkillModifier();
				modifier.AttackType = SkillAttackType.Magic;

				var skillHitResult = SCR_SkillHit(caster, target, skill, modifier);
				var damage = skillHitResult.Damage;

				target.TakeDamage(damage, caster);

				var hitInfo = new HitInfo(caster, target, skill, skillHitResult.Damage, skillHitResult.Result);
				Send.ZC_HIT_INFO(caster, target, hitInfo);

				caster.PlayEffectToGround("F_cleric_zaibas_shot_rize", target.Position, 1, 0f, 0f, 0);

				targetCount--;

				pad.Trigger.IncreaseUseCount();
				if (pad.IsDead)
					break;
			}
		}
	}
}
