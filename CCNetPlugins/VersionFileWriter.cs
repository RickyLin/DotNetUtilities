using Exortech.NetReflector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core;
using System.IO;

namespace CCNetPlugins
{
	[ReflectorType("versionWriterTask")]
	public class VersionFileWriter : ITask
	{
		[ReflectorProperty("versionFilePath")]
		public string VersionFilePath { get; set; }

		public void Run(IIntegrationResult result)
		{
			File.WriteAllText(VersionFilePath, result.Label);
		}
	}
}
