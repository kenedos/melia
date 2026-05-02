using System;
using System.Collections.Generic;
using System.Linq;
using Melia.Zone.Network;
using Melia.Zone.Scripting;
using Melia.Zone.Scripting.AI;
using Melia.Zone.World.Actors;

[Ai("WoodCarvingAi")]
public class WoodCarvingAiScript : AiScript
{
	private readonly List<(DateTime Time, float Damage)> _damageLog = new();
	private DateTime _lastSayTime = DateTime.MinValue;

	private static readonly TimeSpan SayInterval = TimeSpan.FromSeconds(1);
	private static readonly TimeSpan DamageWindow = TimeSpan.FromSeconds(5);

	protected override void Setup()
	{
		During("Idle", this.UpdateDamageBubble);
		During("Attack", this.UpdateDamageBubble);
	}

	protected override void Root()
	{
		this.StartRoutine("Idle", this.Idle());
	}

	protected override void OnTakeDamage(ICombatEntity attacker, float damage)
	{
		lock (_damageLog)
			_damageLog.Add((DateTime.UtcNow, damage));

		// Immediately show damage on hit to verify AI is receiving hits
		Send.ZC_CHAT(this.Entity, $"Hit: {(int)damage:#,0}");
	}

	private void UpdateDamageBubble()
	{
		var now = DateTime.UtcNow;
		if (now - _lastSayTime < SayInterval)
			return;

		_lastSayTime = now;

		var cutoff = now - DamageWindow;
		float total;

		lock (_damageLog)
		{
			_damageLog.RemoveAll(e => e.Time < cutoff);
			total = _damageLog.Sum(e => e.Damage);
		}

		if (total > 0)
			Send.ZC_CHAT(this.Entity, $"DMG (5s): {(int)total:#,0}");
	}
}
