using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melia.Shared.Util
{
	public static class IPUtils
	{
		public static uint ToInt32(this string ip)
		{
			return ip.Split('.')
					 .Select(octet => Convert.ToUInt32(octet))
					 .Aggregate((result, octet) => (result << 8) + octet);
		}
	}
}
