using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityGenerator
{
	public class DataType
	{
		public int DbTypeID { get; set; }
		public string DbTypeName { get; set; }
		public string TypeName { get; set; }
		public string NullableTypeName { get; set; }

		public DataType(int dbTypeID, string dbTypeName, string typeName, string nullableTypeName)
		{
			DbTypeID = dbTypeID;
			DbTypeName = dbTypeName;
			TypeName = typeName;
			NullableTypeName = nullableTypeName;
		}
	}
}
