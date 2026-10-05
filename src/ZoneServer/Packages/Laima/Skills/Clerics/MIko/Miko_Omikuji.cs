using System;
using System.Linq;
using System.Threading.Tasks;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;

namespace Melia.Zone.Packages.Laima.Skills.Clerics.Miko
{
	/// <summary>
	/// Omikuji.
	/// Randomly grants one of four Great Blessings to the caster
	/// and nearby allies.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Miko_Omikuji)]
	public class Miko_Omikuji : IGroundSkillHandler
	{
		private const float EffectRange = 100f;
		private const float EnhancePerLevel = 0.005f;
		private static readonly TimeSpan BuffDuration = TimeSpan.FromSeconds(20);
		private static readonly TimeSpan CastDelay = TimeSpan.FromMilliseconds(1000);

		private static readonly BuffId[] OmikujiBuffs =
		{
			BuffId.Honor_Buff,
			BuffId.Wish_Buff,
			BuffId.Safety_Buff,
			BuffId.Healthy_Buff,
		};

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character)
				return;

			if (!caster.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();

			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, farPos);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			skill.Run(this.ApplyOmikuji(skill, character));
		}

		private async Task ApplyOmikuji(Skill skill, Character caster)
		{
			await skill.Wait(CastDelay);

			var selectedBuff = OmikujiBuffs[Random.Shared.Next(OmikujiBuffs.Length)];
			var enhanceRate = this.GetEnhanceRate(caster);

			var allies = caster.Map.GetCharacters(character => character != null && !character.IsDead && character.Layer == caster.Layer && character.IsAlly(caster) && caster.Position.Get2DDistance(character.Position) <= EffectRange).ToList();

			if (!allies.Contains(caster))
				allies.Add(caster);

			foreach (var ally in allies)
			{
				this.RemoveExistingOmikujiBuffs(ally);

				ally.StartBuff(selectedBuff, skill.Level, enhanceRate, BuffDuration, caster, skill.Id);
			}

			caster.SetAttackState(false);
		}

		private float GetEnhanceRate(Character character)
		{
			if (!character.Abilities.TryGet(AbilityId.Miko7, out var ability) || !ability.Active)
				return 0f;

			var abilityLevel = Math.Min(ability.Level, 100);
			var enhanceRate = abilityLevel * EnhancePerLevel;

			if (abilityLevel >= 100)
				enhanceRate += 0.10f;

			return enhanceRate;
		}

		private void RemoveExistingOmikujiBuffs(Character character)
		{
			character.RemoveBuff(BuffId.Honor_Buff);
			character.RemoveBuff(BuffId.Wish_Buff);
			character.RemoveBuff(BuffId.Safety_Buff);
			character.RemoveBuff(BuffId.Healthy_Buff);
		}
	}
}
