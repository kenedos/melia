using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Pads;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Characters.Components;
using Melia.Zone.World.Actors.Monsters;
using Yggdrasil.Geometry.Shapes;
using Yggdrasil.Util;
using static Melia.Zone.Skills.SkillUseFunctions;
using static Melia.Zone.Skills.Helpers.SkillDamageHelper;
using static Melia.Zone.Skills.Helpers.SkillResultHelper;
using Melia.Zone.Skills.Handlers;

namespace Melia.Zone.Skills.HandlersOverrides.Wizards.Psychokino
{
	/// <summary>
	/// Handler for the Psychokino skill Swap.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Psychokino_Swap)]
	public class Psychokino_SwapOverride : IMeleeGroundSkillHandler, IDynamicCasted
	{
		protected TimeSpan DamageDelay { get; } = TimeSpan.FromMilliseconds(200);
		public void StartDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			if (caster is Character character)
			{
				if (character.IsWearingArmorOfType(ArmorMaterialType.Iron))
				{
					caster.ServerMessage(Localization.Get("Can't use while wearing [Plate] armor."));
					Send.ZC_SKILL_DISABLE(caster);
					return;
				}
			}

			caster.SetCastingState(true, skill);
			Send.ZC_NORMAL.Skill_DynamicCastStart(caster, skill.Id);
		}

		public void EndDynamicCast(Skill skill, ICombatEntity caster, float maxCastTime)
		{
			caster.SetCastingState(false, skill);
			Send.ZC_NORMAL.Skill_DynamicCastEnd(caster, skill.Id, maxCastTime);
		}

		public async void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (caster is Character character)
			{
				if (character.IsWearingArmorOfType(ArmorMaterialType.Iron))
				{
					caster.ServerMessage(Localization.Get("Can't use while wearing [Plate] armor."));
					Send.ZC_SKILL_DISABLE(caster);
					return;
				}
			}

			if (!skill.Vars.TryGet<Position>("Melia.ToolGroundPos", out var targetPos))
			{
				caster.ServerMessage(Localization.Get("No target location specified."));
				return;
			}
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}
			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			await skill.Wait(TimeSpan.FromMilliseconds(700));

			var targets2 = caster.Map.GetAttackableEnemiesInPosition(caster, targetPos, 150f);
			var swapCount = 3 + skill.Level;
			var swappedCount = 0;

			// Swaps positions with targets
			if (targets2.Count > 0)
			{
				var casterPos = caster.Position;
				foreach (var target in targets2)
				{
					if (target is not Character)
					{
						if (!Enum.TryParse<MonsterRank>(target.Properties?.GetString(PropertyName.MonRank), out var monRank))
							continue;
						if (target.MoveType != MoveType.Holding
							&& monRank != MonsterRank.MISC
							&& monRank != MonsterRank.NPC
							&& monRank != MonsterRank.Boss)
						{
							this.SwapPositionAfter(caster, target, skill, targetPos, casterPos);
							swappedCount++;
						}
					}
					else
					{
						this.SwapPositionAfter(caster, target, skill, targetPos, casterPos);
						swappedCount++;
					}

					swapCount--;
					if (swapCount <= 0)
					{
						break;
					}
				}

				if (swappedCount > 0)
				{
					await caster.PlayEffectToGround("F_wizard_swap_shot", targetPos, 1, 0.0f);
					await caster.PlayEffectToGround("F_wizard_swap_shot", casterPos, 1, 0.0f);
				}
			}

			// Teleports to position instead if no monsters were swapped
			if (swappedCount == 0)
			{
				var validTargetPos = targetPos;

				if (!caster.Map.Ground.IsValidPosition(targetPos))
				{
					var newPos = caster.Map.Ground.GetLastValidPosition(caster.Position, targetPos);
					if (caster.Map.Ground.TryGetHeightAt(newPos, out var height))
					{
						validTargetPos = newPos;
						validTargetPos.Y = height;
					}
					else
					{
						validTargetPos = caster.Position;
					}
				}
				else if (caster.Map.Ground.TryGetHeightAt(targetPos, out var height))
				{
					validTargetPos.Y = height;
				}

				caster.Position = validTargetPos;
				Send.ZC_SET_POS(caster, validTargetPos);

				await caster.PlayEffectToGround("F_wizard_swap_shot", validTargetPos, 1, 0.0f);
			}
		}

		private void SwapPositionAfter(ICombatEntity caster, ICombatEntity target, Skill skill, Position targetPos, Position casterPos)
		{
			var offsetX = (target.Position.X - targetPos.X) * 0.5f;
			var offsetY = (target.Position.Y - targetPos.Y) * 0.5f;
			var offsetZ = (target.Position.Z - targetPos.Z) * 0.5f;

			var desiredPos = new Position(casterPos.X + offsetX, casterPos.Y + offsetY, casterPos.Z + offsetZ);

			if (caster.Map.Ground.TryGetNearestValidPosition(desiredPos, out var validPos, maxDistance: 100f))
			{
				target.SetPosition(validPos);
			}
			else
			{
				target.SetPosition(casterPos);
			}

			caster.SetPosition(targetPos);

			target.InsertHate(caster, 150);

			target.StartBuff(BuffId.Swap_Debuff, 1, 1, TimeSpan.FromMilliseconds(2000), caster);
		}
	}
}
