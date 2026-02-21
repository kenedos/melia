using System.Collections.Generic;
using Melia.Zone.Network;

namespace Melia.Zone.World.Actors.Effects
{
	/// <summary>
	/// An effect that creates a visual link between multiple actors.
	/// The link is destroyed when the effect is removed.
	/// </summary>
	public class LinkerVisualEffect : Effect
	{
		private readonly int _linkerId;
		private readonly string _linkTexture;
		private readonly bool _unkBool;
		private readonly List<int> _linkedHandles;
		private readonly float _linkSecond;
		private readonly string _linkEffect;
		private readonly float _linkEffectScale;
		private readonly string _linkSound;

		public LinkerVisualEffect(int linkerId, string linkTexture, bool unkBool, List<int> linkedHandles, float linkSecond, string linkEffect, float linkEffectScale, string linkSound)
		{
			_linkerId = linkerId;
			_linkTexture = linkTexture;
			_unkBool = unkBool;
			_linkedHandles = linkedHandles;
			_linkSecond = linkSecond;
			_linkEffect = linkEffect;
			_linkEffectScale = linkEffectScale;
			_linkSound = linkSound;
		}

		/// <summary>
		/// Shows the link effect. Note: The provided helper broadcasts, this will be called for each player entering FoV.
		/// </summary>
		public override void ShowEffect(IZoneConnection conn, IActor actor)
		{
			Send.ZC_NORMAL.MakeLinker(conn, actor, _linkerId, _linkTexture, _unkBool, _linkedHandles, _linkSecond, _linkEffect, _linkEffectScale, _linkSound);
		}

		/// <summary>
		/// Called when the effect is removed, which destroys the visual link.
		/// </summary>
		public override void OnRemove(IActor actor)
		{
			Send.ZC_NORMAL.DestroyLinker(actor, _linkerId);
		}
	}
}
