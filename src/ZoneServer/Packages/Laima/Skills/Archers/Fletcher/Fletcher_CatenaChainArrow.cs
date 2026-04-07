using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Game.Const;
using Melia.Shared.L10N;
using Melia.Shared.World;
using Melia.Zone.Network;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Effects;
using Melia.Zone.World.Actors.Monsters;
using Melia.Zone.World.Actors.Pads;
using Melia.Zone.Skills.Handlers.Base;
using Melia.Zone.Skills.Combat;
using Yggdrasil.Geometry.Shapes;

namespace Melia.Zone.Skills.Handlers.Archers.Fletcher
{
	/// <summary>
	/// Handler for the Fletcher skill Catena Chain Arrow.
	/// </summary>
	[Package("laima")]
	[SkillHandler(SkillId.Fletcher_CatenaChainArrow)]
	public class Fletcher_CatenaChainArrowOverride : IGroundSkillHandler, IDynamicCasted
	{
		private const string ChainLinkEffectKey = "Melia.Skill.CatenaChainLink";
		private const float PadDuration = 5f;
		private const float PadSize = 20f;
		private const float MaxLeashDistance = 150f;

		public void Handle(Skill skill, ICombatEntity caster, Position originPos, Position farPos, ICombatEntity target)
		{
			if (skill.Vars.TryGet<int>("Melia.Skill.CatenaPadHandle", out var existingPadHandle) && existingPadHandle > 0)
			{
				skill.IncreaseOverheat();
				caster.SetAttackState(true);
				Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, Position.Zero);
				Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, ForceId.GetNew(), null);

				this.DestroyCatenaPad(caster, skill);
				return;
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
			Send.ZC_NORMAL.UpdateSkillEffect(caster, caster.Handle, caster.Position, caster.Direction, targetPos);

			var effectId1 = "I_force018_trail_chain#Dummy_Force";
			var effectId2 = "F_explosion092_hit_mint";
			Send.ZC_NORMAL.SkillProjectile(caster, targetPos, effectId1, 0.4f, effectId2, 1f, 0f, TimeSpan.FromSeconds(0.5f), TimeSpan.Zero, 200f);

			var forceId = ForceId.GetNew();
			Send.ZC_SKILL_MELEE_GROUND(caster, skill, farPos, forceId, null);

			this.CreateCatenaPad(caster, skill, targetPos);
		}

		/// <summary>
		/// Creates the Catena Chain Arrow pad at the target position.
		/// </summary>
		private void CreateCatenaPad(ICombatEntity caster, Skill skill, Position position)
		{
			var pad = new Pad(PadName.Fletcher_CatenaChainArrow_PAD, caster, skill, new CircleF(position, PadSize));
			pad.Position = position;
			pad.Direction = caster.Direction;
			pad.NumArg2 = 25f;
			pad.NumArg3 = PadSize;
			pad.Trigger.LifeTime = TimeSpan.FromSeconds(PadDuration);
			pad.Trigger.UpdateInterval = TimeSpan.FromMilliseconds(500);

			var anchor = new Npc(12082, "", new Location(caster.Map.Id, position), caster.Direction);
			anchor.DisappearTime = DateTime.Now.AddSeconds(PadDuration);
			anchor.OwnerHandle = caster.Handle;
			anchor.AssociatedHandle = caster.Handle;
			caster.Map.AddMonster(anchor);

			pad.Variables.Set("Melia.Pad.CatenaAnchor", anchor);

			caster.Map.AddPad(pad);

			caster.StartBuff(BuffId.Fletcher_CatenaChainArrow_Buff, skill.Level, 0, TimeSpan.FromSeconds(PadDuration), caster);

			skill.Vars.SetInt("Melia.Skill.CatenaPadHandle", pad.Handle);

			var linkId = ZoneServer.Instance.World.CreateLinkHandle();
			var linkedHandles = new List<int> { caster.Handle, anchor.Handle };
			var linkerEffect = new LinkerVisualEffect(linkId, "Linker_cable_blue", true, linkedHandles, 0.3f, "None", 1f, "None");
			caster.AddEffect(ChainLinkEffectKey, linkerEffect);
		}

		/// <summary>
		/// Destroys the active Catena Chain Arrow pad, anchor, link, and buff.
		/// </summary>
		private void DestroyCatenaPad(ICombatEntity caster, Skill skill)
		{
			if (skill.Vars.TryGet<int>("Melia.Skill.CatenaPadHandle", out var padHandle) && padHandle > 0)
			{
				if (caster.Map.TryGetPad(padHandle, out var pad))
					caster.Map.RemovePad(pad);
			}
		}
	}
}
