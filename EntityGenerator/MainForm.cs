using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using System.Configuration;
using System.IO;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

namespace EntityGenerator
{
	public partial class MainForm : Form
	{
		private string _DefaultNamespace;
		private string _OutputPath;
		private ListItem[] _SelectedItems;
		private CodeType _CodeType;
		private string _DbContextName;
		private PluralizationService _PluralizationSvc;

		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Task.Factory.StartNew(LoadTables);
			Task.Factory.StartNew(LoadViews);
			tbPath.Text = Path.Combine(Environment.CurrentDirectory, "Output");
			_PluralizationSvc = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
			tbDefaultNamespace.Text = ConfigurationManager.AppSettings["defaultNamespace"];
		}

		private void LoadViews()
		{
			FillOutDatabaseObjectNames("cmdQueryViews", clbViews, DatabaseObjectType.View);
		}

		private void LoadTables()
		{
			FillOutDatabaseObjectNames("cmdQueryTables", clbTables, DatabaseObjectType.Table);
		}

		private void FillOutDatabaseObjectNames(string cmdName, CheckedListBox clb, DatabaseObjectType objectType)
		{
			string objectName;
			string strCmd = ConfigurationManager.AppSettings[cmdName];
			Database db = DatabaseFactory.CreateDatabase();
			Action beginUpdate = () => clb.BeginUpdate();
			clb.Invoke(beginUpdate);
			using (IDataReader reader = db.ExecuteReader(CommandType.Text, strCmd))
			{
				Action addItem;
				while (reader.Read())
				{
					objectName = reader.GetString(0);
					addItem = () => clb.Items.Add(new ListItem() { Value = objectName, Type = objectType });
					clb.Invoke(addItem);
				}
			}
			Action endUpdate = () => clb.EndUpdate();
			clb.Invoke(endUpdate);
		}

		private void cbAllTables_CheckedChanged(object sender, EventArgs e)
		{
			SetAllDatabaseObjectSelected(clbTables, cbAllTables);
		}

		private void cbAllViews_CheckedChanged(object sender, EventArgs e)
		{
			SetAllDatabaseObjectSelected(clbViews, cbAllViews);
		}

		private void SetAllDatabaseObjectSelected(CheckedListBox dbObjectList, CheckBox chkAll)
		{
			dbObjectList.BeginUpdate();
			for (int i = 0; i < dbObjectList.Items.Count; i++)
			{
				dbObjectList.SetItemChecked(i, chkAll.Checked);
			}
			dbObjectList.EndUpdate();
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			lbOutputs.Items.Clear();
			ListItem[] items = new ListItem[clbTables.CheckedItems.Count + clbViews.CheckedItems.Count];
			int itemIndex = 0;
			foreach (object item in clbTables.CheckedItems)
			{
				items[itemIndex] = item as ListItem;
				itemIndex++;
			}
			foreach (object item in clbViews.CheckedItems)
			{
				items[itemIndex] = item as ListItem;
				itemIndex++;
			}
			_SelectedItems = items;

			_CodeType = rbEntities.Checked ? CodeType.Entities : CodeType.DbContext;
			_DbContextName = tbDbContextName.Text;
			_DefaultNamespace = tbDefaultNamespace.Text;
			_OutputPath = tbPath.Text.EndsWith("\\") ? tbPath.Text : tbPath.Text + "\\";
			if (Directory.Exists(_OutputPath) == false)
				Directory.CreateDirectory(_OutputPath);

			Task.Factory.StartNew(StartToGenerate);
		}

		private void StartToGenerate()
		{
			ParallelLoopResult result;
			switch (_CodeType)
			{
				case CodeType.Entities:
					result = Parallel.ForEach(_SelectedItems, GenerateEntityObject);
					break;
				case CodeType.DbContext:
					//result = Parallel.ForEach(_SelectedItems, GenerateDbContext);
					Task t = Task.Factory.StartNew(GenerateDbContext);
					t.Wait();
					break;
			}
				

			Action addDone = () => lbOutputs.Items.Add("All done.");
			lbOutputs.Invoke(addDone);
		}

		private string GetTypeName(int dbTypeID, bool isNullable)
		{
			DataType[] types = GetDataTypes();
			DataType dt = types.FirstOrDefault(t => t.DbTypeID == dbTypeID);
			if (dt == null)
			{
				return "ERROR_DbType_" + dbTypeID.ToString();
			}
			return isNullable ? dt.NullableTypeName : dt.TypeName;
		}

		private DataType[] GetDataTypes()
		{
			DataType[] types = {
				new DataType(34, "image", "byte[]", "byte[]"), 
				new DataType(35, "text", "string", "string"), 
				new DataType(36, "uniqueidentifier", "Guid", "Guid?"), 
				//new DataType(40, "date", "", ""), 
				//new DataType(41, "time", "", ""), 
				//new DataType(42, "datetime2", "", ""), 
				//new DataType(43, "datetimeoffset", "", ""), 
				new DataType(48, "tinyint", "byte", "byte?"), 
				new DataType(52, "smallint", "short", "short?"), 
				new DataType(56, "int", "int", "int?"), 
				new DataType(58, "smalldatetime", "DateTime", "DateTime?"), 
				new DataType(59, "real", "float", "float?"), 
				new DataType(60, "money", "decimal", "decimal?"), 
				new DataType(61, "datetime", "DateTime", "DateTime?"), 
				new DataType(62, "float", "double", "double?"), 
				new DataType(98, "sql_variant", "object", "object"), 
				new DataType(99, "ntext", "string", "string"), 
				new DataType(104, "bit", "bool", "bool?"), 
				new DataType(106, "decimal", "decimal", "decimal?"), 
				new DataType(108, "numeric", "decimal", "decimal?"), 
				//new DataType(122, "smallmoney", "", ""), 
				new DataType(127, "bigint", "long", "long?"), 
				//new DataType(240, "hierarchyid", "", ""), 
				//new DataType(240, "geometry", "", ""), 
				//new DataType(240, "geography", "", ""), 
				new DataType(165, "varbinary", "byte[]", "byte[]"), 
				new DataType(167, "varchar", "string", "string"), 
				//new DataType(173, "binary", "", ""), 
				new DataType(175, "char", "string", "string"), 
				//new DataType(189, "timestamp", "", ""), 
				new DataType(231, "nvarchar", "string", "string"), 
				new DataType(239, "nchar", "string", "string"),
				new DataType(241, "xml", "string", "string")
				//new DataType(231, "sysname", "", "")
			};

			return types;
		}

		#region methods to generate source files

		private void GenerateDbContext()
		{
			string indent = string.Empty;
			using (FileStream fs = File.Create(string.Format("{0}{1}_Generated.cs", _OutputPath, _DbContextName)))
			{
				using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
				{
					// comments
					WriteWarnningComments(sw);

					// namespace declarations
					WriteLine(sw, indent, GetNamespaceDeclarations());
					sw.WriteLine();

					//namespace
					sw.Write("namespace ");
					sw.WriteLine(_DefaultNamespace);

					// the { for namespace
					sw.WriteLine("{");
					PushIndent(ref indent);

					// class name
					WriteLine(sw, indent, string.Format("public partial class {0} : DbContext", _DbContextName));

					// the { for class
					WriteLine(sw, indent, "{");

					PushIndent(ref indent);

					// properties
					GenerateDbContextProperties(sw, indent);
					sw.WriteLine();

					// override methods
					GenerateDbContextMethods(sw, indent);

					PopIndent(ref indent);

					// the } for class
					WriteLine(sw, indent, "}");

					// the } for namespace
					PopIndent(ref indent);
					WriteLine(sw, indent, "}");
				}
			}
		}

		private void GenerateDbContextMethods(StreamWriter sw, string indent)
		{
			// constructor
			/*
			 * the constructor looks like this:
			 * 
				public _DbContactName(string nameOrConnectionString)
					: base(nameOrConnectionString)
				{
			 * 
			 * */
			WriteLine(sw, indent, string.Format("public {0}(string nameOrConnectionString)", _DbContextName));
			PushIndent(ref indent);
			WriteLine(sw, indent, ": base(nameOrConnectionString)");
			PopIndent(ref indent);
			WriteLine(sw, indent, "{");
			WriteLine(sw, indent, "}");
			sw.WriteLine();

			// OnModelCreating method
			/*
			 * The generated OnModelCreating method looks like this:
				protected override void OnModelCreating(DbModelBuilder modelBuilder)
				{
					Configuration.ProxyCreationEnabled = false;
					Database.SetInitializer<RcDbContext>(null);
					modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
					base.OnModelCreating(modelBuilder);
				}
			*/
			WriteLine(sw, indent, "protected override void OnModelCreating(DbModelBuilder modelBuilder)");
			WriteLine(sw, indent, "{");
			PushIndent(ref indent);
			WriteLine(sw, indent, string.Format("Database.SetInitializer<{0}>(null);", _DbContextName));
			WriteLine(sw, indent, "base.OnModelCreating(modelBuilder);");
			WriteLine(sw, indent, "modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();");
			WriteLine(sw, indent, "Configuration.ProxyCreationEnabled = false;");
			PopIndent(ref indent);
			WriteLine(sw, indent, "}");
		}

		private void GenerateDbContextProperties(StreamWriter sw, string indent)
		{
			string className, propertyName, statement;
			foreach (ListItem item in _SelectedItems)
			{
				className = GetClassName(item.Value);
				if (_PluralizationSvc.IsPlural(className))
				{
					propertyName = className;
				}
				else
				{
					propertyName = _PluralizationSvc.Pluralize(className);
				}

				statement = string.Format("public DbSet<{0}> {1} {{ get; set; }}", className, propertyName);
				WriteLine(sw, indent, statement);
			}
		}

		private void GenerateEntityObject(ListItem objectItem)
		{
			string objectName = objectItem.Value;
			string className = GetClassName(objectName);
			string indent = string.Empty;
			using (FileStream fs = File.Create(string.Format("{0}{1}_Generated.cs", _OutputPath, className)))
			{
				using (StreamWriter sw = new StreamWriter(fs, Encoding.Default))
				{
					// comments
					WriteWarnningComments(sw);

					// namespace declarations
					WriteLine(sw, indent, GetNamespaceDeclarations());
					sw.WriteLine();

					//namespace
					sw.Write("namespace ");
					sw.WriteLine(_DefaultNamespace);

					// the { for namespace
					sw.WriteLine("{");
					PushIndent(ref indent);

					// class name
					if (className != objectName)
					{
						WriteLine(sw, indent, string.Format("[Table(\"{0}\")]", objectName));
					}
					WriteLine(sw, indent, string.Format("public partial class {0}", className));

					// the { for class
					WriteLine(sw, indent, "{");

					// properties
					PushIndent(ref indent);
					GenerateProperties(objectName, className, sw, indent);
					PopIndent(ref indent);

					// the } for class
					WriteLine(sw, indent, "}");

					// the } for namespace
					PopIndent(ref indent);
					WriteLine(sw, indent, "}");
				}
			}

			Action addDone = () => lbOutputs.Items.Add(objectName + ": done.");
			lbOutputs.Invoke(addDone);
		}

		private void WriteWarnningComments(StreamWriter sw)
		{
			sw.WriteLine("/*");
			sw.WriteLine(" * The code is generated by code generator.");
			sw.WriteLine(" * Do not modify this file. Any modifications will lose when the file is regenerated.");
			sw.WriteLine(" * Please extend these entity classes by taking advantage of the partical class feature.");
			sw.WriteLine(" */");
			sw.WriteLine();
		}

		private void Write(StreamWriter sw, string indent, string statement)
		{
			sw.Write(indent);
			sw.Write(statement);
		}

		private void WriteLine(StreamWriter sw, string indent, string statement)
		{
			sw.Write(indent);
			sw.WriteLine(statement);
		}

		private void GenerateProperties(string tableName, string className, StreamWriter sw, string indent)
		{
			string strCmd = string.Format(
				@"SELECT C.[name] AS ColumnName, C.system_type_id AS DbTypeID, C.is_nullable AS IsNullable, C.max_length AS MaxLength
	, EPA.[value] AS AttributeValue, EPD.[value] AS DescriptionValue
	, Case When EPP.[value] IS NULL Then
			REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(C.[name],'-','_'), '/','_'),'#','Number'),'.','_'),' ','_')
		Else EPP.[value] End AS PropertyName, EPL.[value] AS PropertyLabel, EPDT.[value] AS PropertyDataType
FROM sys.columns C
LEFT JOIN sys.extended_properties EPA ON EPA.major_id = C.[object_id] AND EPA.minor_id = C.column_id AND EPA.name IN ('SYN_EFAttribute')
LEFT JOIN sys.extended_properties EPP ON EPP.major_id = C.[object_id] AND EPP.minor_id = C.column_id AND EPP.name IN ('SYN_EFPropertyName')
LEFT JOIN sys.extended_properties EPD ON EPD.major_id = C.[object_id] AND EPD.minor_id = C.column_id AND EPD.name IN ('MS_Description')
LEFT JOIN sys.extended_properties EPL ON EPL.major_id = C.[object_id] AND EPL.minor_id = C.column_id AND EPL.name IN ('SYN_EFPropertyLabel')
LEFT JOIN sys.extended_properties EPDT ON EPDT.major_id = C.[object_id] AND EPDT.minor_id = C.column_id AND EPDT.name IN ('SYN_EFPropertyDataType')
WHERE [object_id] = Object_ID('{0}') 
ORDER BY column_id", tableName);
			Database db = DatabaseFactory.CreateDatabase();
			using (IDataReader reader = db.ExecuteReader(CommandType.Text, strCmd))
			{
				string propertyName, columnName, attributeValue, descriptionValue, propertyType, propertyLabel, propertyDataType;
				int maxLength, dbTypeID;
				bool isNullable;
				while (reader.Read())
				{
					columnName = reader["ColumnName"].ToString();
					propertyName = reader["PropertyName"].ToString();
					dbTypeID = int.Parse(reader["DbTypeID"].ToString());
					isNullable = (bool)reader["IsNullable"];
					propertyType = GetTypeName(dbTypeID, isNullable);
					propertyLabel = reader["PropertyLabel"].ToString();
					propertyDataType = reader["PropertyDataType"].ToString();
					//if (propertyType == "ERROR")
					//{
					//	MessageBox.Show(string.Format("Cannot get type name for the type id: {0}", dbTypeID));
					//}

					maxLength = int.Parse(reader["MaxLength"].ToString());
					if (string.Compare(propertyName, className + "ID", true) == 0)
					{
						//WriteLine(sw, indent, string.Format("[Column(\"{0}\")]", columnName));
						//propertyName = "Id";
						WriteLine(sw, indent, "[Key]");
					}
					else if (propertyName == className)
						propertyName += "Content";
					else if (propertyName != columnName)
					{
						WriteLine(sw, indent, string.Format("[Column(\"{0}\")]", columnName));
					}
					else if (className.EndsWith("View", StringComparison.CurrentCultureIgnoreCase)
						&& string.Compare(propertyName, className.Substring(0, className.Length - 4) + "ID", true) == 0)
					{
						WriteLine(sw, indent, "[Key]");
					}

					// generate comment
					descriptionValue = reader["DescriptionValue"] is DBNull ? null : reader["DescriptionValue"].ToString();
					if (!string.IsNullOrEmpty(descriptionValue))
					{
						WriteLine(sw, indent, "/// <summary>");
						WriteLine(sw, indent, "/// " + descriptionValue);
						WriteLine(sw, indent, "/// </summary>");
					}

					// generate attributes
					attributeValue = reader["AttributeValue"] is DBNull ? null : reader["AttributeValue"].ToString();
					if (!string.IsNullOrEmpty(attributeValue))
					{
						string[] attributes = attributeValue.Split('|');
						foreach (string attr in attributes)
						{
							WriteLine(sw, indent, string.Format("[{0}]", attr));
						}
					}
					if (propertyType == "string" && maxLength > 0)
						WriteLine(sw, indent, string.Format("[StringLength({0})]", maxLength));

					if (propertyType.StartsWith("DateTime"))
					{
						WriteLine(sw, indent, "[DataType(DataType.Date)]");
						WriteLine(sw, indent, "[DisplayFormat(DataFormatString = \"{0:d}\", ApplyFormatInEditMode = true)]");
					}

					if (!isNullable)
					{
						WriteLine(sw, indent, "[Required]");
					}

					if (!string.IsNullOrEmpty(propertyLabel))
					{
						WriteLine(sw, indent, string.Format("[Display(Name = \"{0}\")]", propertyLabel));
					}

					if (!string.IsNullOrEmpty(propertyDataType))
					{
						WriteLine(sw, indent, string.Format("[DataType(DataType.{0})]", propertyDataType));
					}

					// generate property
					WriteLine(sw, indent, string.Format("public {0} {1} {{ get; set; }}", propertyType, propertyName));
					sw.WriteLine();
				}
			}
		}

		private void PushIndent(ref string indent)
		{
			indent = indent + "\t";
		}

		private void PopIndent(ref string indent)
		{
			indent = indent.Substring(1);
		}

		private string GetNamespaceDeclarations()
		{
			return @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;";
		}

		private string GetClassName(string tableName)
		{
			string className = tableName;
			if (char.IsLower(className[0]))
			{
				className = string.Format("{0}{1}", char.ToUpper(className[0]), className.Substring(1));
			}
			if (className.Contains('_'))
			{
				string[] nameParts = className.Split('_');
				if (_PluralizationSvc.IsPlural(nameParts[nameParts.Length - 1]))
					nameParts[1] = _PluralizationSvc.Singularize(nameParts[nameParts.Length - 1]);
				className = string.Join("_", nameParts);
			}
			else
			{
				if (_PluralizationSvc.IsPlural(className))
					className = _PluralizationSvc.Singularize(className);
			}

			if (className.StartsWith("Ref_", StringComparison.CurrentCultureIgnoreCase))
				return className.Substring(4);

			if (className.StartsWith("vw_", StringComparison.CurrentCultureIgnoreCase))
				return className.Substring(3) + "View";
			else if (className.StartsWith("vw", StringComparison.CurrentCultureIgnoreCase))
				return className.Substring(2) + "View";
			return className;
		}

		#endregion

		private void tbTableFilter_TextChanged(object sender, EventArgs e)
		{
			FilterListItem(clbTables, tbTableFilter.Text);
		}

		private void tbViewFilter_TextChanged(object sender, EventArgs e)
		{
			FilterListItem(clbViews, tbViewFilter.Text);
		}

		private void FilterListItem(CheckedListBox clb, string filter)
		{
			if (string.IsNullOrEmpty(filter))
				return;
			ListItem li;
			for (int i = 0; i < clb.Items.Count; i++)
			{
				li = clb.Items[i] as ListItem;
				if (li.Value.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase))
				{
					clb.SetSelected(i, true);
					break;
				}
			}
		}

		private void tbTableFilter_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ')
			{
				CheckSelectedListItem(clbTables);
				e.Handled = true;
				tbTableFilter.Text = string.Empty;
			}
		}

		private void tbViewFilter_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == ' ')
			{
				CheckSelectedListItem(clbViews);
				e.Handled = true;
				tbViewFilter.Text = string.Empty;
			}
		}

		private void CheckSelectedListItem(CheckedListBox clb)
		{
			if (clb.SelectedIndex < 0)
				return;
			clb.SetItemChecked(clb.SelectedIndex, !clb.GetItemChecked(clb.SelectedIndex));
		}
	}
}
