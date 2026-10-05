using System;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;

namespace Melia.Zone.Skills.Handlers.Swordsmen.Fencer
{
	[Package("laima")]
	[SkillHandler(SkillId.Fencer_Preparation)]
	public class Fencer_PreparationOverride : IGroundSkillHandler, IDynamicCasted, ICancelSkillHandler
	{
		private const int MaxSkillTimeMiliseconds = 5000;
		private const int TickIntervalMs = 100;

		private bool _isCasting;

		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			_isCasting = true;
			// Inicia o buff de bloqueio imediatamente no início do casting
			caster.StartBuff(BuffId.Preparation_Buff, skill.Level, 0f, TimeSpan.Zero, caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			this.StopPreparation(caster, skill);
		}

		/// <summary>
		/// Disparado pela interface ICancelSkillHandler no cancelamento da tecla/ação.
		/// </summary>
		public void Handle(Skill skill, ICombatEntity caster)
		{
			this.StopPreparation(caster, skill);
		}

		private void StopPreparation(ICombatEntity caster, Skill skill)
		{
			if (!_isCasting)
				return;

			_isCasting = false;

			if (caster == null)
				return;

			// Libera a movimentação e estado de ataque
			caster.SetAttackState(false);

			// Remove o buff defensivo (dispara o Preparation_Buff_End automaticamente via OnEnd)
			caster.RemoveBuff(BuffId.Preparation_Buff);

			// Pacotes essenciais para soltar a animação e o estado no cliente
			Send.ZC_SKILL_CAST_CANCEL(caster);
			Send.ZC_SKILL_DISABLE(caster);
			Send.ZC_NORMAL.SkillCancel(caster, skill.Id);
			Send.ZC_NORMAL.SkillCancelCancel(caster, skill.Id);
		}

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster == null || caster.IsDead)
				return;

			// Se por algum motivo o cancelamento já ocorreu antes de entrar no Handle de posição
			if (!_isCasting)
				return;

			if (!caster.TrySpendSp(skill))
			{
				this.StopPreparation(caster, skill);
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			caster.StopBuff(BuffId.Preparation_Buff_End);
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var skillHandle = ZoneServer.Instance.World.CreateSkillHandle();

			Send.ZC_SKILL_READY(caster, skill, skillHandle, caster.Position, caster.Position);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, caster.Position);

			skill.Run(this.HandleSkill(caster, skill));
		}

		private async Task HandleSkill(ICombatEntity caster, Skill skill)
		{
			var maxTicks = MaxSkillTimeMiliseconds / TickIntervalMs;
			var currentTicks = 0;

			while (_isCasting && currentTicks < maxTicks)
			{
				if (caster == null || caster.IsDead || caster.Map == null)
				{
					this.StopPreparation(caster, skill);
					break;
				}

				await skill.Wait(TimeSpan.FromMilliseconds(TickIntervalMs));
				currentTicks++;
			}

			// Se completou os 5 segundos sem soltar a tecla, encerra o canalizamento
			if (_isCasting)
			{
				this.StopPreparation(caster, skill);
			}
		}
	}
}
