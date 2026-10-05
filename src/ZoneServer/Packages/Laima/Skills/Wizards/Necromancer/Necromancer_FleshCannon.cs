using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.Game.Properties;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Necromancer
{
	[Package("laima")]
	[SkillHandler(SkillId.Necromancer_FleshCannon)]
	public class Necromancer_FleshCannonOverride : IGroundSkillHandler
	{
		private const int CorpsePartsCost = 15;
		private const int TotalHits = 16;
		private const int HitIntervalMilliseconds = 150;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}

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
			Send.ZC_PC_PROP_UPDATE(character, PropertyTable.GetId("PCEtc", PropertyName.Necro_DeadPartsCnt), 1);

			skill.IncreaseOverheat();

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			var effectHandle = ZoneServer.Instance.World.CreateEffectHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, targetPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, targetPos);
			Send.ZC_NORMAL.PlayCorpsePartsRing(caster, effectHandle, 0.25f, 20, 15, CorpsePartsCost, 400981);

			skill.Run(this.HandleHits(skill, caster, targetPos, effectHandle));
		}

		private async Task HandleHits(Skill skill, ICombatEntity caster, Position targetPos, int effectHandle)
		{
			var radius = (int)skill.Data.SplashRange * 5;
			var hitInterval = TimeSpan.FromMilliseconds(HitIntervalMilliseconds);

			for (var hitIndex = 0; hitIndex < TotalHits; hitIndex++)
			{
				if (caster.IsDead || caster.Map == null)
					break;

				var targets = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, radius);

				foreach (var currentTarget in targets.LimitBySDR(caster, skill))
				{
					if (currentTarget == null || currentTarget.IsDead)
						continue;

					if (hitIndex == 0)
						currentTarget.StartBuff(BuffId.Debrave_Debuff, TimeSpan.FromSeconds(4), caster);

					var result = SCR_SkillHit(caster, currentTarget, skill);
					currentTarget.TakeDamage(result.Damage, caster);

					var hit = new SkillHitInfo(caster, currentTarget, skill, result, TimeSpan.Zero, TimeSpan.Zero);

					Send.ZC_HIT_INFO(caster, currentTarget, hit.HitInfo);
				}

				if (hitIndex < TotalHits - 1)
					await skill.Wait(hitInterval);
			}

			Send.ZC_NORMAL.RemoveCorpseParts(caster, effectHandle);
		}
	}
}
