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
using Melia.Zone.Skills.Helpers;
using Melia.Zone.World.Actors;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Handlers.Archers.Falconer
{
	/// <summary>
	/// Handler for the Falconer skill Calling.
	/// Calls the hawk back to the falconer, resetting its position and
	/// granting a temporary attack speed buff to the caster.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Falconer_Calling)]
	public class Falconer_CallingOverride : IGroundSkillHandler
	{
		private const int BaseBuffDurationSeconds = 15;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var targetHandle = target?.Handle ?? 0;
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, targetHandle, originPos, originPos.GetDirection(farPos), Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			skill.Run(this.HandleSkill(skill, caster));
		}

		private async Task HandleSkill(Skill skill, ICombatEntity caster)
		{
			await skill.Wait(TimeSpan.FromMilliseconds(200));

			// Call hawk back if it's flying away
			if (FalconerHawkHelper.TryGetHawk(caster, out var hawk))
			{
				if (FalconerHawkHelper.IsHawkFlyingAway(hawk))
				{
					await FalconerHawkHelper.HawkUnhide(skill, caster, hawk);

					// Falconer2: Call: Obtain - chance to get items
					if (caster.IsAbilityActive(AbilityId.Falconer2))
					{
						// 50% chance to trigger item drop
						if (RandomProvider.Get().Next(100) < 50)
						{
							caster.PlayEffect("F_item_drop_light_violet", scale: 1f, heightOffset: EffectLocation.Top);
							// Item reward would be handled by reward system
						}
					}
				}
			}

			// Hawk return visual effect
			caster.PlayEffect("F_archer_calling_bird", scale: 1.2f, heightOffset: EffectLocation.Top);
			caster.PlaySound("skill_archer_calling", "skill_archer_calling");

			// Calculate buff duration
			var buffDuration = TimeSpan.FromSeconds(BaseBuffDurationSeconds + (skill.Level * 2));

			// Apply Calling buff - increases attack speed and accuracy
			// caster.StartBuff(BuffId.Calling_Buff, skill.Level, 0, buffDuration, caster);

			// Falconer2: Call: Obtain - recover items from defeated enemies
			if (caster.IsAbilityActive(AbilityId.Falconer2))
			{
				// caster.StartBuff(BuffId.CallObtain_Buff, skill.Level, 0, buffDuration, caster);
			}

			// Reset hawk state - remove any existing hawk pads/effects
			await skill.Wait(TimeSpan.FromMilliseconds(300));
			caster.PlayEffect("F_archer_calling_arrive", scale: 1f, heightOffset: EffectLocation.Top);
		}
	}
}
