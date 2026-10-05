using System;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Necromancer_FleshHoop)]
	public class Necromancer_FleshHoopOverride : IForceGroundSkillHandler
	{
		private const int CorpsePartsCost = 5;
		private const int TotalHits = 16;
		private const int SkillDurationMilliseconds = 10000;
		private const int HitIntervalMilliseconds = 625;
		private const float AttackRange = 24f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			var corpseParts = (int)character.Etc.Properties.GetFloat(PropertyName.Necro_DeadPartsCnt);

			if (corpseParts < CorpsePartsCost)
			{
				caster.ServerMessage(Localization.Get("Not enough corpse parts."));
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			character.SetEtcProperty(PropertyName.Necro_DeadPartsCnt, corpseParts - CorpsePartsCost);
			Send.ZC_OBJECT_PROPERTY(character, character.Etc, PropertyName.Necro_DeadPartsCnt);

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var forceId = ForceId.GetNew();

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_FORCE_GROUND(caster, skill, caster.Position, forceId, null);

			SkillCreatePad(caster, skill, caster.Position, 0f, PadName.Necromancer_FleshHoop_abil);
			caster.StartBuff(BuffId.FleshHoop_Buff, 1f, 0f, TimeSpan.FromMilliseconds(SkillDurationMilliseconds), caster, skill.Id);

			skill.Run(this.HandleHits(caster, skill));
		}

		private async Task HandleHits(ICombatEntity caster, Skill skill)
		{
			for (var hitIndex = 0; hitIndex < TotalHits; hitIndex++)
			{
				if (caster.IsDead || caster.Map == null)
					break;

				var targets = caster.Map.GetAttackableEnemiesInPosition(caster, caster.Position, AttackRange);

				foreach (var currentTarget in targets)
				{
					if (currentTarget == null || currentTarget.IsDead)
						continue;

					var result = SCR_SkillHit(caster, currentTarget, skill);
					currentTarget.TakeDamage(result.Damage, caster);

					var hit = new SkillHitInfo(caster, currentTarget, skill, result, TimeSpan.Zero, TimeSpan.Zero);

					Send.ZC_HIT_INFO(caster, currentTarget, hit.HitInfo);
				}

				if (hitIndex < TotalHits - 1)
					await skill.Wait(TimeSpan.FromMilliseconds(HitIntervalMilliseconds));
			}
		}
	}
}
