namespace Melia.Zone.World.Actors.CombatEntities.Components
{
	/// <summary>
	/// Makes an entity trail one of another actor's model nodes on every
	/// client that sees it.
	/// </summary>
	public class FollowToActorComponent : CombatEntityComponent
	{
		/// <summary>
		/// Returns the actor being followed.
		/// </summary>
		public IActor Target { get; }

		/// <summary>
		/// Returns the name of the target's model node that is followed.
		/// </summary>
		public string NodeName { get; }

		/// <summary>
		/// Returns the packet's first unknown float.
		/// </summary>
		public float F1 { get; }

		/// <summary>
		/// Returns the packet's second unknown float.
		/// </summary>
		public float F2 { get; }

		/// <summary>
		/// Returns the packet's third unknown float.
		/// </summary>
		public float F3 { get; }

		/// <summary>
		/// Returns the packet's unknown byte.
		/// </summary>
		public byte B1 { get; }

		/// <summary>
		/// Returns the packet's fourth unknown float.
		/// </summary>
		public float F4 { get; }

		/// <summary>
		/// Creates new component.
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="target"></param>
		/// <param name="nodeName"></param>
		/// <param name="f1"></param>
		/// <param name="f2"></param>
		/// <param name="f3"></param>
		/// <param name="b1"></param>
		/// <param name="f4"></param>
		public FollowToActorComponent(ICombatEntity entity, IActor target, string nodeName, float f1, float f2, float f3, byte b1, float f4) : base(entity)
		{
			this.Target = target;
			this.NodeName = nodeName;
			this.F1 = f1;
			this.F2 = f2;
			this.F3 = f3;
			this.B1 = b1;
			this.F4 = f4;
		}
	}
}
