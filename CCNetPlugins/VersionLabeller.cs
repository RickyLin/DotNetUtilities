using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.CruiseControl.Core;
using ThoughtWorks.CruiseControl.Core.Util;
using ThoughtWorks.CruiseControl.Remote;
using ThoughtWorks.CruiseControl.Core.Label;
using Exortech.NetReflector;

namespace CCNetPlugins
{
	[ReflectorType("versionLabeller")]
	public class VersionLabeller : ILabeller
	{
		[ReflectorProperty("major")]
		public string Major { get; set; }

		[ReflectorProperty("minor")]
		public string Minor { get; set; }

		public string Generate(IIntegrationResult integrationResult)
		{
			// generate the version string
			string[] version = new string[4];
			version[0] = Major;
			version[1] = Minor;
			TimeSpan days, seconds;
			DateTime beginDate = new DateTime(2000, 1, 1);
			days = DateTime.Today - beginDate;
			seconds = DateTime.Now - DateTime.Today.AddHours(-12);
			version[2] = days.Days.ToString();	// Build
			version[3] = Convert.ToString(Convert.ToInt32(seconds.TotalMinutes));	// Revision

			return string.Join(".", version);
		}

		public void Run(IIntegrationResult result)
		{
			result.Label = Generate(result);
		}
	}
}
