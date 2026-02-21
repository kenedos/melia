using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.CombatEntities.Components;
using Yggdrasil.Util;

namespace Melia.Zone.Skills.Handlers.Clerics.Cleric
{
	/// <summary>
	/// Handler for the Cleric skill Cure.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cleric_Cure)]
	public class Cleric_CureOverride : IMeleeGroundSkillHandler
	{
		/// <summary>
		/// Handles skill, removing debuffs from target.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
		/// <param name="targets"></param>
		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var target = targets.FirstOrDefault();
			if (target == null)
				return;

			this.RemoveDebuffs(caster, target, skill);

			var buffDuration = 5000 + skill.Level * 2000;

			var byAbility = 1f;
			if (caster.TryGetActiveAbilityLevel(AbilityId.Cleric11, out var level))
				byAbility += level * 0.005f;
			buffDuration = (int)(buffDuration * byAbility);

			target.StartBuff(BuffId.Cure_Buff, TimeSpan.FromMilliseconds(buffDuration));

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
		}

		/// <summary>
		/// Clears all removable debuffs from the target by a certain chance.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		private void RemoveDebuffs(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			var rnd = RandomProvider.Get();
			var removeChance = this.GetRemoveChance(skill);

			var buffComponent = target.Components.Get<BuffComponent>();
			var buffs = buffComponent.GetList();

			foreach (var buff in buffs)
			{
				if (buff.Data.Type != BuffType.Debuff)
					continue;

				if (rnd.Next(100) < removeChance)
					buffComponent.Remove(buff.Id);
			}
		}

		/// <summary>
		/// Returns the chance to remove a debuff from the target.
		/// </summary>
		/// <param name="skill"></param>
		/// <returns></returns>
		private float GetRemoveChance(Skill skill)
		{
			// 100% Cure chance
			return 100;
		}
	}
}
