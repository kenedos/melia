using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Effects;

namespace Melia.Zone.Skills.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handler for the Linker skill Physical Link.
	/// Creates links that share physical damage among connected party members
	/// using a star topology (caster at center).
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Linker_Physicallink)]
	public class Linker_PhysicalLinkOverride : IForceGroundSkillHandler
	{
		private const float LinkRange = 250f;
		private const string LinkTexture = "Linker3";
		private const float LinkSecond = 0.25f;
		private const string LinkEffect = "None";
		private const float LinkEffectScale = 0.5f;
		private const string LinkSound = "swd_blow_cloth2";

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity designatedTarget)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_READY(caster, skill, 1, originPos, farPos);
			Send.ZC_SKILL_FORCE_GROUND(caster, skill, farPos, forceId, null);

			// Remove existing link buffs from caster and anyone linked to them
			this.DestroyExistingLinks(caster);

			// Create the party link with star topology
			this.CreatePartyLink(caster, skill);
		}

		/// <summary>
		/// Destroys existing Link_Physical buffs from the caster
		/// and all entities linked to them.
		/// </summary>
		private void DestroyExistingLinks(ICombatEntity caster)
		{
			var buffsToDestroy = new[] { BuffId.Link_Physical };

			foreach (var buffId in buffsToDestroy)
			{
				if (caster.TryGetBuff(buffId, out var buff) &&
					buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
				{
					foreach (var handle in memberHandles)
					{
						if (caster.Map != null && caster.Map.TryGetCombatEntity(handle, out var member))
						{
							member.StopBuff(buffId);
						}
					}
				}
			}
		}

		/// <summary>
		/// Creates a link between the caster and nearby party members using star topology.
		/// </summary>
		private void CreatePartyLink(ICombatEntity caster, Skill skill)
		{
			var character = caster as Character;
			if (character == null)
				return;

			// Get party members in range (includes caster)
			var partyMembers = caster.Map.GetPartyMembersInRange(character, LinkRange);

			// Build list of linked entities starting with caster
			var linkedEntities = new List<ICombatEntity> { caster };
			var minY = caster.Position.Y;
			var maxY = caster.Position.Y;

			foreach (var member in partyMembers)
			{
				if (member.Handle == caster.Handle)
					continue;

				// Vertical distance check (keep members within reasonable height difference)
				var targetY = member.Position.Y;
				var currentMinY = minY;
				var currentMaxY = maxY;

				minY = Math.Min(minY, targetY);
				maxY = Math.Max(maxY, targetY);

				if (maxY - minY <= 35.0f)
				{
					linkedEntities.Add(member);
				}
				else
				{
					minY = currentMinY;
					maxY = currentMaxY;
				}
			}

			// Need at least 2 members (caster + 1 party member) to form a link
			if (linkedEntities.Count < 2)
				return;

			var handles = linkedEntities.Select(e => e.Handle).ToList();
			var linkId = ZoneServer.Instance.World.CreateLinkHandle();

			// Apply buff to all linked entities
			for (var i = 0; i < linkedEntities.Count; i++)
			{
				var entity = linkedEntities[i];
				var buff = entity.StartBuff(BuffId.Link_Physical, skill.Level, 0, TimeSpan.FromMinutes(5), caster, skill.Id);

				if (buff != null)
				{
					buff.Vars.Set("Melia.Link.Id", linkId);
					buff.Vars.Set("Melia.Link.Caster", caster.Handle);
					buff.Vars.Set("Melia.Link.Members", handles);

					// Mark the caster (index 0) as IsCaster for chain breaking logic
					if (i == 0)
					{
						buff.Vars.Set("Melia.Link.IsCaster", true);
						entity.AddEffect("Melia.Link.Chain", new AttachEffect("I_chain004_mash_loop2", 2, EffectLocation.Bottom));
					}
				}
			}

			// Create visual effects using star topology (caster connects to each member)
			for (var i = 1; i < linkedEntities.Count; i++)
			{
				var pairHandles = new List<int> { caster.Handle, linkedEntities[i].Handle };
				var pairLinkId = ZoneServer.Instance.World.CreateLinkHandle();

				var linkerEffect = new LinkerVisualEffect(
					pairLinkId,
					LinkTexture,
					true,
					pairHandles,
					LinkSecond,
					LinkEffect,
					LinkEffectScale,
					LinkSound
				);

				caster.AddEffect($"Link_{linkId}_{i}", linkerEffect);
			}
		}
	}
}
