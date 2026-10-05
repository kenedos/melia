using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using Yggdrasil.Data.Binary;
using Yggdrasil.Geometry;

namespace Melia.Shared.Data.Database
{
	public class NavGraphData
	{
		public string MapName { get; set; }
		public VertexListData[] Nodes { get; set; }
	}


	public class NavGraphDb : DatabaseBinaryIndexed<string, NavGraphData>
	{
		public void Save(FileStream fs)
		{
			using var bw = new BinaryWriter(fs);

			bw.Write([(byte)'N', (byte)'A', (byte)'V', (byte)'G']); // header
			bw.Write(1); // version
			bw.Write(this.Count);

			foreach (var kv in this.Entries)
			{
				var bytes = this.Serialize(kv.Value);
				bw.Write(bytes.Length);
				bw.Write(bytes);
			}
		}

		public void Load(FileStream fs)
		{
			using var br = new BinaryReader(fs);

			var header = br.ReadBytes(4);
			if (header[0] != 'N' || header[1] != 'A' || header[2] != 'V' || header[3] != 'G')
				throw new Exception("Invalid NavGraphDb file header");

			var version = br.ReadInt32();
			var count = br.ReadInt32();

			for (int i = 0; i < count; i++)
			{
				int length = br.ReadInt32();
				var bytes = br.ReadBytes(length);

				using var ms = new MemoryStream(bytes);
				using var brMap = new BinaryReader(ms);

				var data = Deserialize(brMap, version);
				this.AddOrReplace(data.MapName, data);
			}
		}

		protected override void Read(FileStream fs)
		{
			using var br = new BinaryReader(fs);

			var header = br.ReadBytes(4);
			if (header[0] != 'N' || header[1] != 'A' || header[2] != 'V' || header[3] != 'G')
				throw new Exception("Invalid NavGraphDb file header");

			var version = br.ReadInt32();
			var count = br.ReadInt32();

			for (int i = 0; i < count; i++)
			{
				int length = br.ReadInt32();
				var bytes = br.ReadBytes(length);

				using var ms = new MemoryStream(bytes);
				using var brMap = new BinaryReader(ms);

				var data = Deserialize(brMap, version);
				this.AddOrReplace(data.MapName, data);
			}
		}

		private byte[] Serialize(NavGraphData data)
		{
			using var ms = new MemoryStream();
			using var bw = new BinaryWriter(ms);

			bw.Write(data.MapName);

			bw.Write(data.Nodes.Length);
			foreach (var node in data.Nodes)
			{
				bw.Write(node.Index);
				bw.Write(node.Vertices.Length);
				foreach (var v in node.Vertices)
				{
					bw.Write(v.X);
					bw.Write(v.Y);
					bw.Write(v.Z);
				}
			}

			return ms.ToArray();
		}

		private NavGraphData Deserialize(BinaryReader br, int version)
		{
			var data = new NavGraphData();

			data.MapName = br.ReadString();

			int nodeCount = br.ReadInt32();
			data.Nodes = new VertexListData[nodeCount];

			for (int i = 0; i < nodeCount; i++)
			{
				var node = new VertexListData();
				node.Index = br.ReadInt32();
				int vertCount = br.ReadInt32();

				node.Vertices = new VertexData[vertCount];
				for (int j = 0; j < vertCount; j++)
				{
					node.Vertices[j] = new VertexData
					{
						X = br.ReadSingle(),
						Y = br.ReadSingle(),
						Z = br.ReadSingle()
					};
				}

				data.Nodes[i] = node;
			}

			return data;
		}
	}
}
