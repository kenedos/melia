using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Pads;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;

namespace Melia.Zone.Skills.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Handler for the Enchanter skill Enchant Aura.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Enchanter_EnchantAura)]
	public class Enchanter_EnchantAuraOverride : IGroundSkillHandler
	{
		private const int MaxTargets = 5;
		private const int AuraRadius = 100;
		private static readonly object AuraPadLock = new();
		private static readonly Dictionary<int, Pad> ActiveAuraPads = new();

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster.TryGetBuff(BuffId.EnchantAura_Buff, out _))
			{
				this.DisableAura(caster, originPos);
				return;
			}

			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);
			caster.StartBuff(BuffId.EnchantAura_Buff, skill.Level, 0f, TimeSpan.Zero, caster, skill.Id);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			skill.Run(this.HandleAura(caster, skill, originPos));
		}

		private async Task HandleAura(ICombatEntity caster, Skill skill, Position auraPosition)
		{
			Pad auraPad = null;

			try
			{
				await skill.Wait(TimeSpan.FromMilliseconds(600));
				caster.SetAttackState(false);

				if (!caster.TryGetBuff(BuffId.EnchantAura_Buff, out _))
					return;

				var auraArea = new Circle(auraPosition, AuraRadius);

				auraPad = SkillCreatePad(caster, skill, auraPosition, 0f, PadName.Enchanter_EnchantAura, true, AuraRadius);
				this.RegisterAuraPad(caster.Handle, auraPad);

				while (caster.TryGetBuff(BuffId.EnchantAura_Buff, out _))
				{
					await skill.Wait(TimeSpan.FromSeconds(1));

					if (!caster.TryGetBuff(BuffId.EnchantAura_Buff, out _))
						break;

					if (caster.IsDead || caster.Map == null)
					{
						caster.StopBuff(BuffId.EnchantAura_Buff);
						break;
					}

					if (!caster.TrySpendSp(skill))
					{
						caster.StopBuff(BuffId.EnchantAura_Buff);
						caster.ServerMessage(Localization.Get("Not enough SP."));
						break;
					}

					var targets = caster.Map
						.GetAttackableEnemiesIn(caster, auraArea)
						.Where(hitTarget => hitTarget != null && !hitTarget.IsDead)
						.Take(MaxTargets)
						.ToList();

					var aniTime = TimeSpan.FromMilliseconds(20);
					var skillHitDelay = TimeSpan.Zero;

					foreach (var hitTarget in targets)
					{
						var skillHitResult = SCR_SkillHit(caster, hitTarget, skill);

						if (caster is Character character &&
							character.Abilities.TryGet(AbilityId.Enchanter17, out var ability) &&
							ability.Active)
						{
							var bonusRate = ability.Level * 0.005f;

							if (ability.Level >= 100)
								bonusRate += 0.10f;

							skillHitResult.Damage *= 1f + bonusRate;
						}

						hitTarget.TakeDamage(skillHitResult.Damage, caster);

						var skillHit = new SkillHitInfo(caster, hitTarget, skill, skillHitResult, aniTime, skillHitDelay);
						Send.ZC_SKILL_FORCE_TARGET(caster, hitTarget, skill, skillHit);
					}
				}
			}
			finally
			{
				this.DestroyAuraPad(caster.Handle, auraPad);
				caster.SetAttackState(false);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, caster.Position, caster.Direction, Position.Zero);
				Send.ZC_SKILL_DISABLE(caster);
			}
		}

		private void DisableAura(ICombatEntity caster, Position position)
		{
			caster.StopBuff(BuffId.EnchantAura_Buff);
			this.DestroyAuraPad(caster.Handle);
			caster.SetAttackState(false);

			Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, position, caster.Direction, Position.Zero);
			Send.ZC_SKILL_DISABLE(caster);
		}

		private void RegisterAuraPad(int casterHandle, Pad auraPad)
		{
			if (auraPad == null)
				return;

			Pad previousPad = null;

			lock (AuraPadLock)
			{
				if (ActiveAuraPads.TryGetValue(casterHandle, out previousPad))
					ActiveAuraPads.Remove(casterHandle);

				ActiveAuraPads[casterHandle] = auraPad;
			}

			if (previousPad != null && previousPad != auraPad)
				previousPad.Destroy();
		}

		private void DestroyAuraPad(int casterHandle, Pad expectedPad = null)
		{
			Pad auraPad;

			lock (AuraPadLock)
			{
				if (!ActiveAuraPads.TryGetValue(casterHandle, out auraPad))
					return;

				if (expectedPad != null && auraPad != expectedPad)
					return;

				ActiveAuraPads.Remove(casterHandle);
			}

			auraPad.Destroy();
		}
	}
}
