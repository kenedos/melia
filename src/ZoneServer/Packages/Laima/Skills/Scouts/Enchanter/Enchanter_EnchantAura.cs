using System;
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
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.SkillUseFunctions;

namespace Melia.Zone.Skills.Handlers.Scouts.Enchanter
{
	/// <summary>
	/// Handler for the Enchanter skill Enchant Aura.
	/// SkillId: 50808
	/// ClassName: Enchanter_EnchantAura
	///
	/// Behavior:
	/// - Toggle skill.
	/// - Creates a fixed damage area at the caster's feet.
	/// - Deals damage every second to up to 5 enemies inside the area.
	/// - Consumes the skill SP cost every second while active.
	/// - Casting again disables the aura.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Enchanter_EnchantAura)]
	public class Enchanter_EnchantAuraOverride : IGroundSkillHandler
	{
		private const int MaxTargets = 5;
		private const int AuraRadius = 100;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			// Toggle off.
			if (caster.TryGetBuff(BuffId.EnchantAura_Buff, out _))
			{
				caster.StopBuff(BuffId.EnchantAura_Buff);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, 0, originPos, caster.Direction, Position.Zero);
				return;
			}

			// Initial SP cost.
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, Position.Zero);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_TARGET(caster, skill, caster);

			skill.Run(this.HandleAura(caster, skill, originPos));
		}

		private async Task HandleAura(ICombatEntity caster, Skill skill, Position auraPosition)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(600));

			caster.StartBuff(BuffId.EnchantAura_Buff, skill.Level, 0f, TimeSpan.Zero, caster, skill.Id);
			caster.SetAttackState(false);

			var auraArea = new Circle(auraPosition, AuraRadius);

			// Creates the visible fixed aura area at the caster's position.
			var auraPad = SkillCreatePad(
				caster,
				skill,
				auraPosition,
				0f,
				PadName.Enchanter_EnchantAura,
				true,
				AuraRadius
			);

			while (caster.TryGetBuff(BuffId.EnchantAura_Buff, out _))
			{
				await skill.Wait(TimeSpan.FromSeconds(1));

				if (!caster.TryGetBuff(BuffId.EnchantAura_Buff, out _))
					break;

				// Consumes SP every second.
				// Set Enchanter_EnchantAura SP cost to 15 in skill.ies.
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

					var skillHit = new SkillHitInfo(
						caster,
						hitTarget,
						skill,
						skillHitResult,
						aniTime,
						skillHitDelay);

					Send.ZC_SKILL_FORCE_TARGET(caster, hitTarget, skill, skillHit);
				}
			}

			if (auraPad != null)
				auraPad.Destroy();
		}
	}
}
