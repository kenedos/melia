using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Effects;

namespace Melia.Zone.Skills.Handlers.Scouts.Linker
{
	/// <summary>
	/// Handler for the Linker skill Spiritual Chain.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Linker_SpiritualChain)]
	public class Linker_SpiritualChainOverride : IMeleeGroundSkillHandler
	{
		private const float LinkRange = 250f;
		private const string LinkTexture = "Linker_blue2";
		private const float LinkSecond = 0.25f;
		private const string LinkEffect = "None";
		private const float LinkEffectScale = 0.3f;
		private const string LinkSound = "swd_blow_cloth2";

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, params ICombatEntity[] targets)
		{
			if (!caster.TrySpendSp(skill))
			{
				caster.ServerMessage(Localization.Get("Not enough SP."));
				return;
			}

			skill.IncreaseOverheat();
			caster.SetAttackState(true);

			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos);

			this.DestroyExistingLinks(caster);
			this.CreatePartyLink(caster, skill);
		}

		private void DestroyExistingLinks(ICombatEntity caster)
		{
			var buffsToDestroy = new[] { BuffId.Link_Party };

			foreach (var buffId in buffsToDestroy)
			{
				if (caster.TryGetBuff(buffId, out var buff) &&
					buff.Vars.TryGet<List<int>>("Melia.Link.Members", out var memberHandles))
				{
					foreach (var handle in memberHandles)
					{
						if (caster.Map != null && caster.Map.TryGetCombatEntity(handle, out var member))
							member.StopBuff(buffId);
					}
				}
			}
		}

		private void CreatePartyLink(ICombatEntity caster, Skill skill)
		{
			var character = caster as Character;
			if (character == null)
				return;

			var partyMembers = caster.Map.GetPartyMembersInRange(character, LinkRange);

			var linkedEntities = new List<ICombatEntity> { caster };
			var minY = caster.Position.Y;
			var maxY = caster.Position.Y;

			foreach (var member in partyMembers)
			{
				if (member.Handle == caster.Handle)
					continue;

				var targetY = member.Position.Y;
				var currentMinY = minY;
				var currentMaxY = maxY;

				minY = Math.Min(minY, targetY);
				maxY = Math.Max(maxY, targetY);

				if (maxY - minY <= 35.0f)
					linkedEntities.Add(member);
				else
				{
					minY = currentMinY;
					maxY = currentMaxY;
				}
			}

			if (linkedEntities.Count < 2)
				return;

			var handles = linkedEntities.Select(e => e.Handle).ToList();
			var linkId = ZoneServer.Instance.World.CreateLinkHandle();

			for (var i = 0; i < linkedEntities.Count; i++)
			{
				var entity = linkedEntities[i];
				var buff = entity.StartBuff(BuffId.Link_Party, skill.Level, 0, TimeSpan.FromMinutes(5), caster, skill.Id);

				if (buff != null)
				{
					buff.Vars.Set("Melia.Link.Id", linkId);
					buff.Vars.Set("Melia.Link.Caster", caster.Handle);
					buff.Vars.Set("Melia.Link.Members", handles);

					if (i == 0)
					{
						buff.Vars.Set("Melia.Link.IsCaster", true);
						entity.AddEffect("Melia.Link.Chain", new AttachEffect("I_chain004_mash_loop_multi_blue", 2, EffectLocation.Bottom));
					}
				}
			}

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
