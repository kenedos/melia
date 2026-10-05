using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Melia.Zone.World.Actors.Monsters;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Wizards.Onmyoji
{
	[Package("laima")]
	[SkillHandler(SkillId.Onmyoji_FireFoxShikigami)]
	public class Onmyoji_FireFoxShikigamiOverride : IGroundSkillHandler, IMeleeGroundSkillHandler, IDynamicCasted
	{
		private const int NormalFoxId = 300002;
		private const int GrownFoxId = 300007;
		private const int DurationSeconds = 10;
		private const int GrownFoxAdditionalDurationSeconds = 10;
		private const int ControllerIntervalMilliseconds = 1000;
		private const float FollowDistance = 50f;
		private const float AuraRadius = 200f;
		private const int AuraDebuffDurationSeconds = 10;
		private const string OwnerKey = "Melia.Summoner.Owner";

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, System.Collections.Generic.IList<ICombatEntity> targets)
		{
			this.Cast(skill, caster, originPos, farPos);
		}

		private void Cast(Skill skill, ICombatEntity caster, Position originPos, Position farPos)
		{
			if (caster is not Character character || character.IsDead || character.Map == null)
				return;

			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			character.SetAttackState(true);
			character.TurnTowards(farPos);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SKILL_READY(character, skill, skillHandle, originPos, farPos);
			Send.ZC_SKILL_MELEE_GROUND(character, skill, farPos);

			foreach (var oldFox in character.Summons.GetSummons(s => !s.IsDead && ((int)s.Id == NormalFoxId || (int)s.Id == GrownFoxId)))
				oldFox.Kill(character);

			character.StopBuff(BuffId.FireFoxShikigami_Buff);
			character.StopBuff(BuffId.FireFoxShikigami_Onmyoji2_Buff);

			var monsterId = character.IsAbilityActive(AbilityId.Onmyoji3) ? GrownFoxId : NormalFoxId;
			var summon = new Summon(character, monsterId, RelationType.Friendly);
			character.Summons.AddSummon(summon);

			summon.Name = "!@#${Auto_1}_of_{Auto_2}$*$Auto_1$*$" + character.Name + "$*$Auto_2$*$@dicID_^*$ETC_20150317_000235$*^#@!";
			summon.OwnerHandle = character.Handle;
			summon.Faction = FactionType.Law;
			summon.Tendency = TendencyType.Aggressive;
			summon.FromGround = true;
			summon.Position = farPos;
			summon.Vars.Set(OwnerKey, character);

			// MHP é uma propriedade calculada e não pode ser atribuída diretamente.
			// O summon já recebe Invincible abaixo, então não é necessário forçar HP/MHP.
			summon.Properties.SetFloat(PropertyName.Level, character.Level);
			summon.Properties.SetFloat(PropertyName.Lv, character.Level);
			summon.Properties.SetFloat(PropertyName.FIXMSPD_BM, 140f);
			summon.Properties.InvalidateAll();

			var isGrownFox = character.IsAbilityActive(AbilityId.Onmyoji3);
			var durationSeconds = DurationSeconds + (isGrownFox ? GrownFoxAdditionalDurationSeconds : 0);
			var duration = TimeSpan.FromSeconds(durationSeconds);

			summon.Components.Add(new LifeTimeComponent(summon, duration));
			summon.SetState(true, canMove: true, hasAi: false);

			skillHandle = ZoneServer.Instance.World.CreateSkillHandle();
			Send.ZC_SYNC_START(character, skillHandle, 1);

			
			summon.StartBuff(BuffId.Ability_buff_PC_FireFox_Summon, TimeSpan.Zero, summon);

			// Buffs de imunidade de combate
			summon.StartBuff(BuffId.FireFoxShikigami_Invincibility, duration, summon);

			Send.ZC_SYNC_END(character, skillHandle, 0);
			Send.ZC_SYNC_EXEC_BY_SKILL_TIME(character, skillHandle, skill.Data.DefaultHitDelay);

			character.StartBuff(BuffId.FireFoxShikigami_Buff, skill.Level, 0f, duration, character, skill.Id);
			if (character.IsAbilityActive(AbilityId.Onmyoji2))
				character.StartBuff(BuffId.FireFoxShikigami_Onmyoji2_Buff, 1, 0f, duration, character, skill.Id);

			skill.Run(this.ControlFireFox(skill, character, summon));
			character.SetAttackState(false);
			Send.ZC_SKILL_DISABLE(character);
		}

		private async Task ControlFireFox(Skill skill, Character owner, Summon summon)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(100));

			// A duração da aura é controlada pelo buff do jogador, exatamente
			// como a Immolation. Se a Fox morrer, a aura continua.
			while (!owner.IsDead
				&& owner.Map != null
				&& owner.Buffs.Has(BuffId.FireFoxShikigami_Buff))
			{
				// A Fox continua seguindo o jogador enquanto existir.
				if (summon != null && !summon.IsDead && summon.Map == owner.Map
					&& summon.Position.Get2DDistance(owner.Position) > FollowDistance)
				{
					summon.MoveTo(owner.Position);
				}

				// A aura pertence ao jogador: a posição e a busca dos alvos
				// não dependem mais do summon.
				this.ApplyAura(skill, owner);

				await skill.Wait(TimeSpan.FromMilliseconds(ControllerIntervalMilliseconds));
			}

			// O summon termina somente quando o buff da aura termina.
			if (summon != null && !summon.IsDead)
				summon.Kill(owner);
		}

		private void ApplyAura(Skill skill, Character owner)
		{
			if (owner.Map == null || owner.IsDead)
				return;

			var aura = new Circle(owner.Position, AuraRadius);
			var targets = owner.Map.GetAttackableEnemiesIn(owner, aura);

			foreach (var target in targets)
			{
				if (target == null || target.IsDead)
					continue;

				// 1 hit por ciclo da aura, usando o jogador como origem do dano.
				// Assim o dano continua mesmo se a Fox desaparecer.
				this.ApplyAuraDamage(skill, owner, target);

				// Renova a redução de MDEF enquanto o alvo estiver dentro da aura.
				target.StopBuff(BuffId.FireFoxShikigami_MDef_Debuff);
				target.StartBuff(
					BuffId.FireFoxShikigami_MDef_Debuff,
					skill.Level,
					0f,
					TimeSpan.FromSeconds(AuraDebuffDurationSeconds),
					owner,
					skill.Id);
			}
		}

		private void ApplyAuraDamage(Skill skill, Character owner, ICombatEntity target)
		{
			if (target == null || target.IsDead)
				return;

			if (!owner.TryGetSkill(SkillId.Onmyoji_FireFoxShikigami, out var damageSkill))
				return;

			var isGrownFox = owner.IsAbilityActive(AbilityId.Onmyoji3);
			var modifier = SkillModifier.Default;

			// A aura sempre causa apenas 1 hit. Onmyoji17 não transforma
			// a aura em 5 hits.
			modifier.HitCount = owner.IsAbilityActive(AbilityId.Onmyoji17) ? 3 : 1;
			if (owner.IsAbilityActive(AbilityId.Onmyoji17))
				modifier.FinalDamageMultiplier *= 0.80f;
			modifier.AttackAttribute = isGrownFox ? AttributeType.Fire : AttributeType.Soul;

			if (isGrownFox)
				modifier.FinalDamageMultiplier *= 1.5f;

			var result = SCR_SkillHit(owner, target, damageSkill, modifier);

			if (owner.IsAbilityActive(AbilityId.Onmyoji20))
				result.Damage *= 1.10f;

			if (result.Result != HitResultType.Dodge && result.Damage > 0f)
				target.TakeDamage(result.Damage, owner);

			var hit = new SkillHitInfo(
				owner,
				target,
				damageSkill,
				result,
				TimeSpan.FromMilliseconds(100),
				TimeSpan.Zero);

			hit.HitInfo.Type = isGrownFox ? HitType.Fire : HitType.Soul;
			Send.ZC_SKILL_HIT_INFO(owner, new List<SkillHitInfo> { hit });
		}
	}
}
