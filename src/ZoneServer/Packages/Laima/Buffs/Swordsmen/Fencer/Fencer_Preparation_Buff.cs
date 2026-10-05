using System;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.Packages;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Scripting.ScriptableEvents;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Buffs.Handlers.Swordsmen.Fencer
{
	[Package("laima")]
	[BuffHandler(BuffId.Preparation_Buff)]
	public class Preparation_BuffOverride : BuffHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			// Adiciona um bonus massivo de Block (igual CrossGuard) como camada secundária de garantia
			AddPropertyModifier(buff, buff.Target, PropertyName.BLK_BM, 99999f);
		}

		public override void OnEnd(Buff buff)
		{
			var caster = buff.Target;

			RemovePropertyModifier(buff, buff.Target, PropertyName.BLK_BM);

			if (caster == null)
				return;

			caster.SetAttackState(false);
			if (caster.IsDead || caster.Map == null)
				return;

			if (caster is Character character)
				Send.ZC_SKILL_DISABLE(character);

			// Concede o buff de dano para o próximo ataque do Fencer por 5 segundos
			caster.StartBuff(BuffId.Preparation_Buff_End, (int)buff.NumArg1, 0f, TimeSpan.FromSeconds(5), caster, SkillId.Fencer_Preparation);
		}

		/// <summary>
		/// Intercepta o combate antes do cálculo final para forçar o Block e zerar o dano.
		/// </summary>
		[CombatCalcModifier(CombatCalcPhase.BeforeCalc, BuffId.Preparation_Buff)]
		public void OnDefenseBeforeCalc(ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (!target.TryGetBuff(BuffId.Preparation_Buff, out _))
				return;

			// Zera o dano e força a animação/resultado de Block no alvo
			skillHitResult.Damage = 0f;
			skillHitResult.Effect = HitEffect.SAFETY;
			skillHitResult.Result = HitResultType.Block;
		}
	}

	[Package("laima")]
	[BuffHandler(BuffId.Preparation_Buff_End)]
	public class Preparation_Buff_EndOverride : BuffHandler
	{
		private const int MinimumSkillLevel = 1;
		private const int MaximumSkillLevel = 5;
		private const float DamageBonusPerLevel = 0.20f;

		public static float ConsumeDamageMultiplier(ICombatEntity caster)
		{
			if (caster == null || !caster.TryGetBuff(BuffId.Preparation_Buff_End, out var buff))
				return 1f;

			var skillLevel = Math.Clamp((int)buff.NumArg1, MinimumSkillLevel, MaximumSkillLevel);
			var damageMultiplier = 1f + skillLevel * DamageBonusPerLevel;

			caster.StopBuff(BuffId.Preparation_Buff_End);

			return damageMultiplier;
		}
	}
}
