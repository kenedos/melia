using System;
using System.Threading;
using Melia.Shared.Game.Const;
using Melia.Shared.Network;
using Melia.Zone.Network;
using Melia.Zone.Network.Helpers;
using Melia.Zone.Skills.Combat;

namespace Melia.Zone.World.Actors.Characters
{
	/// <summary>
	/// Represents a dummy character.
	/// </summary>
	public class DummyCharacter : Character
	{
		private static long _nextDummyDbId = 0x00F0000000000000;
		private bool _mirrorDamageToOwner;
		private bool _forwardingDamage;

		/// <summary>
		/// Returns reference to the character's owner (In case of being a dummy).
		/// </summary>
		public Character Owner { get; set; }

		/// <summary>
		/// Returns true if the DummyCharacter has Owner
		/// </summary>
		public bool HasOwner => Owner != null;

		/// <summary>
		/// When set to true, all damage taken by this dummy is forwarded
		/// to the Owner instead of being applied to the dummy's own HP.
		/// The dummy's HP is then synced to match the Owner's HP.
		/// Also subscribes to the Owner's Damaged event to keep this
		/// dummy's HP in sync when the Owner takes damage directly.
		/// </summary>
		public bool MirrorDamageToOwner
		{
			get => _mirrorDamageToOwner;
			set
			{
				if (_mirrorDamageToOwner == value)
					return;

				_mirrorDamageToOwner = value;

				if (value && this.HasOwner)
					this.Owner.Damaged += this.OnOwnerDamaged;
				else if (!value && this.HasOwner)
					this.Owner.Damaged -= this.OnOwnerDamaged;
			}
		}

		/// <summary>
		/// Returns the owner's connection, since dummy characters don't have their own.
		/// This allows packets to be sent to the player controlling the dummy.
		/// </summary>
		public override IZoneConnection Connection => Owner?.Connection;

		/// <summary>
		/// Creates a new dummy character with a unique DbId so that
		/// property update packets don't collide with the owner's ObjectId.
		/// </summary>
		public DummyCharacter()
		{
			this.DbId = Interlocked.Increment(ref _nextDummyDbId);
		}

		/// <summary>
		/// Takes damage, forwarding it to the Owner if MirrorDamageToOwner
		/// is enabled. The dummy's HP is then synced to match the Owner's.
		/// Sends a hit info packet for the Owner directly to the player's
		/// connection so the spirit's HP bar updates.
		/// </summary>
		public override bool TakeDamage(float damage, ICombatEntity attacker)
		{
			if (this.MirrorDamageToOwner && this.HasOwner && !this.Owner.IsDead)
			{
				var hpBefore = this.Owner.Properties.GetFloat(PropertyName.HP);

				_forwardingDamage = true;
				this.Owner.TakeDamage(damage, attacker);
				_forwardingDamage = false;

				this.SyncHpFromOwner();

				var hpAfter = this.Owner.Properties.GetFloat(PropertyName.HP);
				var actualDamage = hpBefore - hpAfter;

				if (actualDamage > 0)
					this.SendHitInfoDirect(attacker, this.Owner, actualDamage);

				return this.Owner.IsDead;
			}

			return base.TakeDamage(damage, attacker);
		}

		/// <summary>
		/// Called when the Owner takes damage directly (not forwarded
		/// from this dummy). Syncs this dummy's HP to match and sends
		/// a hit info packet directly to the player's connection so the
		/// body's HP bar updates.
		/// </summary>
		private void OnOwnerDamaged(Character owner, float damage, ICombatEntity attacker)
		{
			if (_forwardingDamage || !this.HasOwner || this.Map == null)
				return;

			var ownerHp = owner.Properties.GetFloat(PropertyName.HP);
			var myHp = this.Properties.GetFloat(PropertyName.HP);
			var diff = myHp - ownerHp;

			if (diff <= 0)
				return;

			this.ModifyHpSafe(-diff, out _, out _);
			this.SendHitInfoDirect(attacker, this, diff);
		}

		/// <summary>
		/// Sends a ZC_HIT_INFO packet directly to the Owner's connection,
		/// bypassing Broadcast range checks. This ensures the player
		/// always receives the HP update regardless of distance between
		/// spirit and body.
		/// </summary>
		private void SendHitInfoDirect(ICombatEntity attacker, ICombatEntity target, float damage)
		{
			var conn = this.Owner?.Connection;
			if (conn == null)
				return;

			var hitInfo = new HitInfo(attacker, target, damage, HitResultType.Hit);

			var packet = new Packet(Op.ZC_HIT_INFO);
			packet.PutInt(target.Handle);
			packet.PutInt(attacker.Handle);
			packet.PutInt((int)hitInfo.SkillId);
			packet.AddHitInfo(hitInfo);
			packet.PutByte(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutInt(0);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutFloat(0f);
			packet.PutFloat(0f);
			packet.PutInt(hitInfo.HitCount);
			packet.PutByte(1);
			packet.PutInt(0);
			packet.PutInt(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Syncs this dummy's HP to match the Owner's current HP.
		/// </summary>
		public void SyncHpFromOwner()
		{
			if (!this.HasOwner || this.Map == null)
				return;

			var ownerHp = this.Owner.Properties.GetFloat(PropertyName.HP);
			var myHp = this.Properties.GetFloat(PropertyName.HP);
			var diff = ownerHp - myHp;

			if (Math.Abs(diff) > 0.01f)
				this.ModifyHpSafe(diff, out _, out _);
		}

		/// <summary>
		/// Despawns/Removes this entity from the map.
		/// </summary>
		public void Despawn()
		{
			Send.ZC_OWNER(this.Owner, this);
			Send.ZC_LEAVE(this);

			this.Map.RemoveCharacter(this);
		}
	}
}
