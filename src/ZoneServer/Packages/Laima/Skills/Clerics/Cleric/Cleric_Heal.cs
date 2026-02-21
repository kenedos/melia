using System;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.SplashAreas;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Pads;

namespace Melia.Zone.Skills.Handlers.Clerics.Cleric
{
	/// <summary>
	/// Handler for the Cleric skill Heal.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Cleric_Heal)]
	public class Cleric_HealOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		// Tile layout: Each value represents the minimum skill level required
		// for that tile to be spawned
		private static readonly int[] TileShape = new[]
		{
			6,  1,  1,  1,  7,
			8,  2,  2,  3,  9,
			10, 4,  3,  5,  11,
		};

		private const int TileRows = 3;
		private const int TileCols = 5;
		private const int TileSpacing = 20;
		private const int PadSize = 30;

		/// <summary>
		/// Start casting.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="maxCastTime"></param>
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		/// <summary>
		/// End casting.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="maxCastTime"></param>
		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		/// <summary>
		/// Handles skill, damaging targets.
		/// </summary>
		/// <param name="skill"></param>
		/// <param name="caster"></param>
		/// <param name="originPos"></param>
		/// <param name="farPos"></param>
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
			this.ExecuteHeal(caster, target, skill);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);
		}

		/// <summary>
		/// Heals the target or spawns heal pads.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		private void ExecuteHeal(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			if (!Feature.IsEnabled("DirectClericHeal"))
				this.SpawnHealPads(caster, skill);
			else
			{
				if (target == null)
					return;
				this.DirectHeal(caster, target, skill);
			}
		}

		/// <summary>
		/// Heals target directly with a buff.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="target"></param>
		/// <param name="skill"></param>
		private void DirectHeal(ICombatEntity caster, ICombatEntity target, Skill skill)
		{
			Send.ZC_NORMAL.PlayEffect(target, "F_cleric_heal_active_ground_new");

			var SCR_CalculateHeal = ScriptableFunctions.Combat.Get("SCR_CalculateHeal");
			var modifier = new SkillModifier();
			var skillHitResult = new SkillHitResult();
			var healAmount = SCR_CalculateHeal(caster, target, skill, modifier, skillHitResult);

			var healDuration = TimeSpan.FromMilliseconds(1);
			target.StartBuff(BuffId.Heal_Buff, skill.Level, healAmount, healDuration, caster);

			if (caster is Character character && character.TryGetActiveAbilityLevel(AbilityId.Cleric22, out var abilLv))
			{
				var healOverTimeAmount = abilLv * 0.05f * healAmount / 10;
				target.StartBuff(BuffId.Heal_Dot_Buff, 1, healOverTimeAmount, TimeSpan.FromMilliseconds(10000), caster);
			}
		}

		/// <summary>
		/// Spawns heal pads on the ground that heal targets on contact.
		/// </summary>
		/// <param name="caster"></param>
		/// <param name="skill"></param>
		private void SpawnHealPads(ICombatEntity caster, Skill skill)
		{
			var refPos = caster.Position.GetRelative(caster.Direction, 30);
			var level = skill.Level;

			// Effect scale is approximately ~0.02 per unit
			var scale = 25 * 0.02f;

			// Iterate over the shape array by row and column. Each value
			// represents a skill level required for that tile to be
			// spawned. If the caster's skill level is high enough, the
			// x and y offsets are used to calculate the position on the
			// tile grid and the pad is spawned.
			for (var yi = 0; yi < TileRows; ++yi)
			{
				for (var xi = 0; xi < TileCols; ++xi)
				{
					var minLevel = TileShape[xi + yi * TileCols];
					if (level < minLevel)
						continue;

					var pos = refPos.GetRelative(caster.Direction.Left, (xi - 2) * TileSpacing);
					pos = pos.GetRelative(caster.Direction, yi * TileSpacing);

					var pad = new Pad(PadName.Cleric_New_Heal, caster, skill, new Circle(pos, PadSize / 2f));
					pad.Position = pos;
					pad.NumArg1 = scale;

					caster.Map.AddPad(pad);
				}
			}
		}
	}
}
