using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityGenerator
{
	public class ListItem
	{
		public string Value { get; set; }
		public DatabaseObjectType Type { get; set; }

		public override string ToString()
		{
			return Value;
		}
	}
}
