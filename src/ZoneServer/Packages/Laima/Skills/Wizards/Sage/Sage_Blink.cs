using System;
using System.Linq;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.Packages;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.Skills.Handlers;

namespace Melia.Zone.Packages.Laima.Skills.Wizards.Sage
{
	[Package("laima")]
	[SkillHandler(SkillId.Sage_Blink)]
	public class Sage_BlinkOverride : IGroundSkillHandler
	{
		private const float TeleportRange = 250f;
		private const float PartyRange = 60f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (caster is not Character character || character is DummyCharacter || character.IsDead || character.Map == null) return;
			var origin = character.Position;
			if (!float.IsFinite(farPos.X) || !float.IsFinite(farPos.Y) || !float.IsFinite(farPos.Z) || origin.Get2DDistance(farPos) > TeleportRange)
			{
				character.ServerMessage(Localization.Get("Too far away."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}
			var allies = character.Map.GetCharacters(c => c != character && c is not DummyCharacter && !c.IsDead && character.PartyId != 0 && c.PartyId == character.PartyId && c.Position.Get2DDistance(origin) <= PartyRange).ToArray();
			if (!character.TrySpendSp(skill))
			{
				character.ServerMessage(Localization.Get("Not enough SP."));
				Send.ZC_SKILL_DISABLE(character);
				return;
			}
			try
			{
				SageBlinkHelper.Remove(character);
				SageBlinkHelper.Create(character, skill, origin);

				skill.IncreaseOverheat();

				Send.ZC_SKILL_READY(character, skill, 1, origin, farPos);
				Send.ZC_NORMAL.UpdateSkillEffect(character, 0, origin, origin.GetDirection(farPos), Position.Zero);
				Send.ZC_SKILL_MELEE_GROUND(character, skill, farPos);

				var targetPos = farPos;
				var height = character.Map.Ground.GetHeightAt(targetPos);
				targetPos.Y = height;

				if (!character.Map.Ground.IsValidPosition(targetPos))
				{
					var newPos = character.Map.Ground.GetLastValidPosition(origin, targetPos);
					if (character.Map.Ground.TryGetHeightAt(newPos, out height))
					{
						targetPos = newPos;
						targetPos.Y = height;
					}
					else
						targetPos = origin;
				}

				character.Position = targetPos;
				Send.ZC_SET_POS(character, targetPos);

				if (character.TryGetActiveAbility(AbilityId.Sage14, out _))
				{
					foreach (var ally in allies)
					{
						if (ally.IsDead || ally.Map != character.Map) continue;
						ally.SetPosition(farPos);
						Send.ZC_MOVE_STOP(ally, farPos, 1);
					}
				}
			}
			catch
			{
				SageBlinkHelper.Remove(character);
				throw;
			}
			finally { character.SetAttackState(false); }
		}
	}
}
