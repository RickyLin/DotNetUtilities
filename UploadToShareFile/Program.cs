using ShareFileSampleCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace UploadToShareFile
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
			 * args:
			 * 0: src folder path, comma to separate multiple folders, do not leave white space around the comma
			 * 1: the folder where the zip file resides.
			 * 2: version file path
			 * 3: sharefile folder path
			 * 4: file name prefix
			 */

			string[] srcDirPaths = null;
			string shareFileFolderId = null, fileNamePrefix = null, versionFilePath = null, zipFilePath = null;
			string userName = null, password = null;

			if (args.Length == 0)
			{
				Console.WriteLine(@" * args:
			 * 0: src folder path, comma to separate multiple folders, do not leave white space around the comma
			 * 1: the folder where the zip file resides.
			 * 2: version file path
			 * 3: sharefile folder path
			 * 4: file name prefix");
				return;
			}

			Console.WriteLine("Get Configuration Information.");
			try
			{
				srcDirPaths = args[0].Split(',');
				zipFilePath = args[1];
				versionFilePath = args[2];
				fileNamePrefix = args[3];
				shareFileFolderId = args[4];
				userName = ConfigurationManager.AppSettings["UserName"];
				password = ConfigurationManager.AppSettings["Password"];
			}
			catch (Exception err)
			{
				Console.WriteLine("Failed to get config data. {0}", err.Message);
				Environment.ExitCode = -1;
				return;
			}

			Console.WriteLine("Check folders available.");
			foreach (string srcDirPath in srcDirPaths)
			{
				if (Directory.Exists(srcDirPath) == false)
				{
					Console.WriteLine("The folder '{0}' doesn't exist.", srcDirPath);
					Environment.ExitCode = -1;
					return;
				}
			}

			if (File.Exists(versionFilePath) == false)
			{
				Console.WriteLine("The file '{0}' doesn't exist.", versionFilePath);
				Environment.ExitCode = -1;
				return;
			}

			// create the zip file, using 7-zip to create archive
			string zipFileName = string.Format("{0}_{1}.zip", fileNamePrefix, File.ReadAllText(versionFilePath));
			string zipFilePathName = Path.Combine(zipFilePath, zipFileName);
			Console.WriteLine("Create file: {0}", zipFilePathName);

			Process zipProcess = new Process();
			string cmdParam = string.Format("a {0}", zipFilePathName);
			foreach (string srcDir in srcDirPaths)
			{
				cmdParam = cmdParam + " " + srcDir;
			}

			zipProcess.StartInfo = new ProcessStartInfo(@"C:\Program Files\7-Zip\7z.exe", cmdParam);
			zipProcess.Start();
			zipProcess.WaitForExit();

			if (zipProcess.ExitCode != 0)
			{
				Environment.ExitCode = zipProcess.ExitCode;
				return;
			}

			// upload the zip file to ShareFile
			Console.WriteLine("Upload zip file to ShareFile.");
			ShareFileSample sample = new ShareFileSample();
			//bool loginSuccess = sample.Authenticate("synvata", "sharefile.com", userName, password);
			bool loginSuccess = sample.Authenticate("synvata", "sharefile.com", "", "");
			if (!loginSuccess)
			{
				Console.WriteLine("Failed to Login ShareFile.");
				Environment.ExitCode = -1;
				return;
			}

			Dictionary<string, object> optionalParameters = new Dictionary<string, object>();
			optionalParameters.Add("folderid", shareFileFolderId);	// the ReloviewsComplete folder
			optionalParameters.Add("overwrite", true);
			try
			{
				//zipFilePathName = @"C:\Users\li\Documents\GitHub\DotNetUtilities\UploadToShareFile\bin\Debug\ReloviewsComplete_6.0.4967.1340.zip";
				sample.FileUpload(zipFilePathName, optionalParameters);
			}
			catch (Exception err)
			{
				Console.WriteLine("Failed to upload. {0}", err.Message);
				Environment.ExitCode = -1;
				return;
			}

			Console.WriteLine("Done.");
			return;
		}
	}
}
