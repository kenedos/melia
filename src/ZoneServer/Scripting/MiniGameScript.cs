using System;
using System.Collections.Generic;
using Melia.Zone.World.Actors;
using Melia.Zone.World.Actors.Characters;
using Yggdrasil.Scripting;

namespace Melia.Zone.Scripting
{
	/// <summary>
	/// Stub: MiniGames system was removed during Laima merge.
	/// Provides minimal interface to satisfy callers.
	/// </summary>
	public abstract class MiniGameScript : IScript, IDisposable
	{
		private readonly static object ScriptsSyncLock = new();
		private readonly static Dictionary<string, MiniGameScript> Scripts = new();

		public string MiniGameId => "";

		public bool Init()
		{
			return true;
		}

		public static bool TryGet(string mGameId, out MiniGameScript mGameScript)
		{
			lock (ScriptsSyncLock)
				return Scripts.TryGetValue(mGameId, out mGameScript);
		}

		public void Dispose()
		{
			lock (ScriptsSyncLock)
				Scripts.Clear();
			GC.SuppressFinalize(this);
		}

		protected abstract void Load();

		protected void SetId(string id) { }
		protected void AddStage(string stageId) { }
		protected void SetDelay(TimeSpan startDelay) { }

		public virtual IActor[] OnStart(Character character, object minigame) => Array.Empty<IActor>();
		public virtual void OnComplete(Character character, object minigame) { }
		public virtual void OnCancel(Character character, object minigame) { }
		public virtual void OnUpdate(Character character, object minigame) { }
	}

	public class MiniGameScriptAttribute : Attribute
	{
		public string Id { get; }
		public MiniGameScriptAttribute(string miniGameId) { this.Id = miniGameId; }
	}

	public class StageScriptAttribute : Attribute
	{
		public string Id { get; }
		public StageScriptAttribute(string stageId) { this.Id = stageId; }
	}
}
