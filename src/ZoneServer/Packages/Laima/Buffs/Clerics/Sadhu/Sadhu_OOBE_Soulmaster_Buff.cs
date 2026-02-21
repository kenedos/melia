using System;
using System.Collections.Generic;
using Melia.Shared.Packages;
using Melia.Shared.Data.Database;
using Melia.Shared.Game.Const;
using Melia.Zone.Buffs.Base;
using Melia.Zone.Network;
using Melia.Zone.Skills.Combat;
using Melia.Zone.Skills;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Melia.Zone.World.Actors.Effects;

namespace Melia.Zone.Buffs.Handlers.Clerics.Sadhu
{
	/// <summary>
	/// Handler for the OOBE_Soulmaster_Buff which indicates spirit form.
	/// Manages the dummy body, max distance enforcement, shared HP,
	/// and damage reduction for the spirit.
	/// </summary>
	[Package("laima")]
	[BuffHandler(BuffId.OOBE_Soulmaster_Buff)]
	public class Sadhu_OOBE_Soulmaster_BuffOverride : BuffHandler, IBuffCombatDefenseAfterCalcHandler
	{
		public override void OnActivate(Buff buff, ActivationType activationType)
		{
			if (buff.Caster is not Character casterCharacter)
				return;

			buff.SetUpdateTime(500);

			var currentSp = casterCharacter.Properties.GetFloat(PropertyName.SP);
			buff.Vars.SetFloat("Melia.OOBE.InitialSP", currentSp);

			AddPropertyModifier(buff, casterCharacter, PropertyName.MSPD_BM, 20);

			Send.ZC_NORMAL.SetMainAttackSkill(casterCharacter, SkillId.Sadhu_EctoplasmAttack);

			var dummyHandle = (int)buff.NumArg2;
			if (!casterCharacter.Map.TryGetCharacter(dummyHandle, out var dummyCharacter))
				return;

			if (dummyCharacter is DummyCharacter dummy)
			{
				dummy.MirrorDamageToOwner = true;
				dummy.SyncHpFromOwner();
				dummy.Died += this.OnDummyDied;
			}

			this.CreateLinkEffect(casterCharacter, dummyHandle);
		}

		public override void WhileActive(Buff buff)
		{
			this.SyncDummyHp(buff);
			this.EnforceMaxDistance(buff);
			this.DrainSp(buff);
		}

		/// <summary>
		/// Syncs the dummy body's HP to match the spirit's current HP.
		/// </summary>
		private void SyncDummyHp(Buff buff)
		{
			if (buff.Caster is not Character casterCharacter)
				return;

			var dummyHandle = (int)buff.NumArg2;
			if (!casterCharacter.Map.TryGetCharacter(dummyHandle, out var dummyCharacter))
				return;

			if (dummyCharacter is DummyCharacter dummy)
				dummy.SyncHpFromOwner();
		}

		/// <summary>
		/// Drains SP every tick while in spirit form.
		/// Cost decreases with skill level (15 - SkillLv), minimum 5.
		/// Ends spirit form if SP is insufficient.
		/// </summary>
		private void DrainSp(Buff buff)
		{
			if (buff.Target is not Character character)
				return;

			var spCost = Math.Max(5, 15 - (int)buff.NumArg1);

			if (character.Properties.GetFloat(PropertyName.SP) < spCost)
			{
				character.StopBuff(buff.Id);
				return;
			}

			character.ModifySp(-spCost);
		}

		public override void OnEnd(Buff buff)
		{
			if (buff.Caster is not Character casterCharacter)
				return;

			RemovePropertyModifier(buff, casterCharacter, PropertyName.MSPD_BM);

			this.RecoverSp(casterCharacter, buff);

			Send.ZC_NORMAL.SetMainAttackSkill(casterCharacter, SkillId.None);
			Send.ZC_NORMAL.SetActorColor(casterCharacter, 255, 255, 255, 255, 0.01f);
			Send.ZC_PLAY_SOUND(casterCharacter, "skl_eff_yuchae_end_2");

			if (ZoneServer.Instance.Data.BuffDb.TryFind(BuffId.OOBE_Soulmaster_Buff, out var buffData))
				Send.ZC_NORMAL.RemoveBuff(casterCharacter, buffData.ClassName);

			this.RemoveLinkEffect(casterCharacter);
			this.ReturnToBody(casterCharacter, (int)buff.NumArg2);
		}

		/// <summary>
		/// Recovers SP based on the amount lost during spirit form.
		/// Recovery rate scales with Prakriti's skill level.
		/// Only applies if Prakriti is learned, and only recovers
		/// positive SP loss (never reduces SP gained from potions etc).
		/// </summary>
		private void RecoverSp(Character character, Buff oobeBuff)
		{
			if (!character.TryGetSkillLevel(SkillId.Sadhu_Prakriti, out var prakritiLevel))
				return;

			var initialSp = oobeBuff.Vars.GetFloat("Melia.OOBE.InitialSP");
			var currentSp = character.Properties.GetFloat(PropertyName.SP);
			var spLost = Math.Max(0, initialSp - currentSp);

			var recoveryRate = 0.30f + 0.03f * prakritiLevel;

			if (character.TryGetActiveAbilityLevel(AbilityId.Sadhu40, out var abilityLevel))
				recoveryRate *= 1f + abilityLevel * 0.005f;

			recoveryRate = Math.Min(1.0f, recoveryRate);
			var spRecovery = (int)(spLost * recoveryRate);

			if (spRecovery > 0)
				character.ModifySp(spRecovery);
		}

		/// <summary>
		/// Makes the character return to the dummy body position.
		/// </summary>
		private void ReturnToBody(Character character, int dummyHandle)
		{
			if (!character.Map.TryGetCharacter(dummyHandle, out var dummyCharacter))
				return;

			character.Position = dummyCharacter.Position;
			character.Direction = dummyCharacter.Direction;

			if (dummyCharacter is DummyCharacter dummy)
			{
				dummy.Died -= this.OnDummyDied;
				dummy.MirrorDamageToOwner = false;
			}

			Send.ZC_ROTATE(character);
			Send.ZC_SET_POS(character, dummyCharacter.Position);
			Send.ZC_OWNER(character, dummyCharacter);
			Send.ZC_LEAVE(dummyCharacter);

			character.Map.RemoveCharacter(dummyCharacter);
		}

		/// <summary>
		/// Called when the clone dummy character dies.
		/// Forces return to body and kills the player.
		/// </summary>
		private void OnDummyDied(ICombatEntity character, ICombatEntity killer)
		{
			if (character is not DummyCharacter dummy || !dummy.HasOwner)
				return;

			var owner = dummy.Owner;

			owner.StopBuff(BuffId.OOBE_Soulmaster_Buff);

			if (!owner.IsDead)
				owner.Kill(killer);
		}

		/// <summary>
		/// Enforces max distance from dummy body.
		/// If spirit goes too far, force return to body.
		/// </summary>
		private void EnforceMaxDistance(Buff buff)
		{
			if (buff.Caster is not Character casterCharacter)
				return;

			var dummyHandle = (int)buff.NumArg2;
			if (!casterCharacter.Map.TryGetCharacter(dummyHandle, out var dummyCharacter))
				return;

			var maxDistance = 180f + buff.NumArg1 * 20f;

			var distance = casterCharacter.Position.Get2DDistance(dummyCharacter.Position);
			if (distance > maxDistance)
				casterCharacter.StopBuff(buff.Id);
		}

		/// <summary>
		/// Creates visual link effects between the spirit and the dummy body.
		/// </summary>
		private void CreateLinkEffect(Character casterCharacter, int dummyHandle)
		{
			var handles = new List<int> { casterCharacter.Handle, dummyHandle };

			var linkId = ZoneServer.Instance.World.CreateLinkHandle();
			var linkEffect = new LinkerVisualEffect(linkId, "OOBE", true, handles, 0.3f, "None", 1f, "None");
			casterCharacter.AddEffect("Melia.OOBE.Link", linkEffect);

			var linkIdBg = ZoneServer.Instance.World.CreateLinkHandle();
			var linkEffectBg = new LinkerVisualEffect(linkIdBg, "OOBE_Bg", true, handles, 0.3f, "I_cleric_oobe_loop_connect", 0.2f, "None");
			casterCharacter.AddEffect("Melia.OOBE.LinkBg", linkEffectBg);
		}

		/// <summary>
		/// Removes the visual link effects between the spirit and the dummy body.
		/// </summary>
		private void RemoveLinkEffect(Character casterCharacter)
		{
			casterCharacter.RemoveEffect("Melia.OOBE.Link");
			casterCharacter.RemoveEffect("Melia.OOBE.LinkBg");
		}

		/// <summary>
		/// Damage reduction while in spirit form.
		/// Physical: 80% damage reduction.
		/// Holy: Double damage.
		/// Magical: Reduced by Prakriti skill level (40% + 3% per level, max 80%).
		/// </summary>
		public void OnDefenseAfterCalc(Buff buff, ICombatEntity attacker, ICombatEntity target, Skill skill, SkillModifier modifier, SkillHitResult skillHitResult)
		{
			if (target is not Character targetCharacter || !targetCharacter.IsOutOfBody())
				return;

			if (skill.Data.Attribute == AttributeType.Holy)
			{
				skillHitResult.Damage = (int)(skillHitResult.Damage * 2);
				return;
			}

			var isMagic = skill.Data.AttackType == SkillAttackType.Magic;

			if (isMagic)
			{
				if (targetCharacter.TryGetSkillLevel(SkillId.Sadhu_Prakriti, out var prakritiLevel))
				{
					var reduction = Math.Min(0.80f, 0.40f + 0.03f * prakritiLevel);
					skillHitResult.Damage = (int)(skillHitResult.Damage * (1f - reduction));
				}
				return;
			}

			skillHitResult.Damage = (int)(skillHitResult.Damage * 0.2f);
		}
	}
}
