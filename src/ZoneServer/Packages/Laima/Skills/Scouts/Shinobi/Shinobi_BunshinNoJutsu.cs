using System;
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
using Melia.Zone.World.Actors.CombatEntities.Components;

namespace Melia.Zone.Packages.Laima.Skills.Scouts.Shinobi
{
	/// <summary>

	/// Handler for Shinobi skill Bunshin no Jutsu.

	/// Creates two Shadow Clones of the Shinobi.

	/// </summary>

	[Package("laima")]
	[SkillHandler(SkillId.Shinobi_Bunshin_no_jutsu)]
	public class Shinobi_BunshinNoJutsu : IGroundSkillHandler
	{
		private const int CloneSpawnDistance = 25;
		private const int BaseCloneDurationSeconds = 60;
		private const int DurationPerAbilityLevelSeconds = 15;
		private const int MaxEnduranceAbilityLevel = 4;

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

			caster.TurnTowards(farPos);
			caster.SetAttackState(true);

			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, originPos, caster.Direction, Position.Zero);
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

			this.RemoveExistingClones(character);
			var cloneDuration = this.GetCloneDuration(character);
			character.StartBuff(BuffId.Bunshin_Buff, skill.Level, character.Handle, cloneDuration, character, skill.Id);

			var clone1Position = character.Position.GetRelative(new Direction(90), CloneSpawnDistance);
			var clone2Position = character.Position.GetRelative(new Direction(270), CloneSpawnDistance);

			var clone1 = this.CreateClone(character, skill, clone1Position, 1, cloneDuration);
			var clone2 = this.CreateClone(character, skill, clone2Position, 2, cloneDuration);

			skill.Run(this.RemoveClonesAfterDuration(skill, clone1, clone2, cloneDuration));

			caster.SetAttackState(false);
		}

		private DummyCharacter CreateClone(Character character, Skill skill, Position position, int cloneIndex, TimeSpan cloneDuration)
		{
			var clone = (DummyCharacter)character.Clone(position);

			clone.Variables.Temp.Set("Bunshin.Index", cloneIndex);

			clone.StartBuff(BuffId.Bunshin_Buff, skill.Level, character.Handle, cloneDuration, character, skill.Id);

			var aiComponent = new AiComponent(clone, "Bunshin", character);
			clone.Components.Add(aiComponent);

			Send.ZC_PLAY_ANI(clone, "BORN", false);
			Send.ZC_NORMAL.Skill_DynamicCastStart(clone, SkillId.None);

			return clone;
		}

		private void RemoveExistingClones(Character character)
		{
			var clones = character.Map.GetCharacters(target =>
				target is DummyCharacter dummy &&
				dummy.Owner == character &&
				dummy.IsBuffActive(BuffId.Bunshin_Buff));

			foreach (var clone in clones)
			{
				if (clone is DummyCharacter dummy)
					dummy.Despawn();
			}
		}

		private async Task RemoveClonesAfterDuration(Skill skill, DummyCharacter clone1, DummyCharacter clone2, TimeSpan cloneDuration)
		{
			await skill.Wait(cloneDuration);

			this.RemoveClone(clone1);
			this.RemoveClone(clone2);
		}

		private TimeSpan GetCloneDuration(Character character)
		{
			var abilityLevel = Math.Min(character.Abilities.GetLevel(AbilityId.Shinobi6), MaxEnduranceAbilityLevel);
			var durationSeconds = BaseCloneDurationSeconds + abilityLevel * DurationPerAbilityLevelSeconds;

			return TimeSpan.FromSeconds(durationSeconds);
		}

		private void RemoveClone(DummyCharacter clone)
		{
			if (clone == null || clone.Map == null)
				return;

			clone.Despawn();
		}
	}
}
