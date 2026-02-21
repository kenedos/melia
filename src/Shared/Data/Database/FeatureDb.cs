using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;
using Yggdrasil.Extensions;

namespace Melia.Shared.Data.Database
{
	/// <summary>
	/// Represents a node in the feature tree database.
	/// </summary>
	public class FeatureNode
	{
		public string Name { get; set; }
		public bool Enabled { get; set; }

		public FeatureNode Parent { get; set; }
		public List<FeatureNode> Children { get; set; } = new List<FeatureNode>();

		/// <summary>
		/// Changes this feature and all of its children.
		/// </summary>
		public void Enable(bool enabled)
			=> this.Enable(enabled, true);

		/// <summary>
		/// Changes this feature and optionally all of its children.
		/// </summary>
		public void Enable(bool enabled, bool toggleChildren)
		{
			this.Enabled = enabled;

			if (toggleChildren)
			{
				foreach (var child in this.Children)
					child.Enable(enabled);
			}
		}
	}

	/// <summary>
	/// A hierarchical feature database.
	/// </summary>
	public class FeatureDb : DatabaseJsonIndexed<string, FeatureNode>
	{
		/// <summary>
		/// Returns true if the feature with the given name is enabled.
		/// Returns false if not or if the feature doesn't exist.
		/// </summary>
		/// <param name="featureName"></param>
		/// <returns></returns>
		public bool IsEnabled(string featureName)
			=> this.Entries.TryGetValue(featureName)?.Enabled ?? false;

		/// <summary>
		/// Called to read an entry from the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			// The entries we get here are the first level parents.
			// We start to read each one recursively.
			this.ReadEntryRecursive(entry);
		}

		/// <summary>
		/// Reads entry and its children and adds the new nodes.
		/// </summary>
		/// <param name="entry"></param>
		/// <returns></returns>
		private FeatureNode ReadEntryRecursive(JObject entry)
		{
			entry.AssertNotMissing("name", "enabled");

			var node = new FeatureNode();

			// First, get the name and whether the feature is enabled
			node.Name = entry.ReadString("name");
			node.Enabled = entry.ReadBool("enabled");

			// Next, see if we already read this feature before. If so,
			// we want to override its status. We then process children
			// to allow child-level overrides, and return null to tell
			// the caller that this element should not be considered a
			// new child.
			if (this.TryFind(node.Name, out var existingNode))
			{
				// Update this node's enabled state without toggling children
				existingNode.Enabled = node.Enabled;

				// Process children if they exist in the override file
				if (entry.ContainsKey("children"))
				{
					foreach (var childEntry in entry.ForEachObject("children"))
					{
						// Recursively process child nodes to apply overrides
						var childNode = this.ReadEntryRecursive(childEntry);
						if (childNode != null)
						{
							// This is a new child that didn't exist before
							childNode.Parent = existingNode;
							existingNode.Children.Add(childNode);

							// If the child is enabled, but the parent is not,
							// the child should be disabled as well.
							if (childNode.Enabled && !existingNode.Enabled)
								childNode.Enable(false);
						}
					}
				}

				return null;
			}

			// Go through the children if there are any
			if (entry.ContainsKey("children"))
			{
				foreach (var childEntry in entry.ForEachObject("children"))
				{
					// Read the child and its children, and if it was a
					// new node, set its parent to the current node,
					// add it to its children, and check its state.
					var childNode = this.ReadEntryRecursive(childEntry);
					if (childNode != null)
					{
						childNode.Parent = node;
						node.Children.Add(childNode);

						// If the child is enabled, but the parent is not,
						// the child should be disabled as well. This check
						// is important because it doesn't work the other
						// way around, a child might be disabled while the
						// parent is enabled.
						if (childNode.Enabled && !node.Enabled)
							childNode.Enable(false);
					}
				}
			}

			// Once all is done, add the node to the entries and return it.
			this.AddOrReplace(node.Name, node);
			return node;
		}
	}
}
