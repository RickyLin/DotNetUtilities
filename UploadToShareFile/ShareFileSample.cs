using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

/// Copyright (c) 2013 Citrix Systems, Inc.
///
/// Permission is hereby granted, free of charge, to any person obtaining a 
/// copy of this software and associated documentation files (the "Software"),
/// to deal in the Software without restriction, including without limitation 
/// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
/// and/or sell copies of the Software, and to permit persons to whom the 
/// Software is furnished to do so, subject to the following conditions:
///
/// The above copyright notice and this permission notice shall be included in 
/// all copies or substantial portions of the Software.
///
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
/// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
/// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
/// IN THE SOFTWARE.

namespace ShareFileSampleCode
{
	/// <summary>
	/// Methods in this class make use of ShareFile API. Please see api.sharefile.com for more information.
	/// Requirements:
	///
	/// Json.NET library. see http://json.codeplex.com/
	///
	/// Optional parameters can be passed to functions as a Dictionary as follows:
	///
	/// ex:
	/// 
	/// Dictionary<string, object> optionalParameters = new Dictionary<string, object>();
	/// optionalParameters.Add("company", "ACompany");
	/// optionalParameters.Add("password", "Apassword");
	/// optionalParameters.Add("addshared", true);
	/// sample.usersCreate("firstname", "lastname", "an@email.com", false, optionalParameters);
	/// 
	/// See api.sharefile.com for optional parameter names for each operation.
	/// </summary>
	class ShareFileSample
	{
		string subdomain;
		string tld;
		string authId;

		/// <summary>
		/// Calls getAuthID to retrieve an authid that will be used for subsequent calls to API.
		/// 
		/// If you normally login to ShareFile at an address like https://mycompany.sharefile.com, 
		/// then your subdomain is mycompany and your tld is sharefile.com
		/// </summary>
		/// <param name="subdomain">your subdomain</param>
		/// <param name="tld">your top level domain</param>
		/// <param name="username">your username</param>
		/// <param name="password">your password</param>
		/// <returns>true if login was successful, false otherwise.</returns>
		public bool Authenticate(string subdomain, string tld, string username, string password)
		{
			this.subdomain = subdomain;
			this.tld = tld;

			string requestUrl = string.Format("https://{0}.{1}/rest/getAuthID.aspx?fmt=json&username={2}&password={3}",
				subdomain, tld, HttpUtility.UrlEncode(username), HttpUtility.UrlEncode(password));
			Console.WriteLine(requestUrl);

			JObject jsonObj = InvokeShareFileOperation(requestUrl);
			if (!(bool)jsonObj["error"])
			{
				string authId = (string)jsonObj["value"];
				this.authId = authId;
				return (true);
			}
			else
			{
				Console.WriteLine(jsonObj["errorCode"] + " : " + jsonObj["errorMessage"]);
				return (false);
			}
		}

		/// <summary>
		/// Prints out a folder list for the specified path or root if none is provided. Currently prints out id, filename, creationdate, type.
		/// </summary>
		/// <param name="path">folder to list</param>
		public void FolderList(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				path = "/";
			}

			Dictionary<string, object> requiredParameters = new Dictionary<string, object>();
			requiredParameters.Add("path", path);

			String url = BuildUrl("folder", "list", requiredParameters);

			JObject jsonObj = InvokeShareFileOperation(url);
			if (!(bool)jsonObj["error"])
			{
				JArray items = (JArray)jsonObj["value"];
				foreach (JObject item in items)
				{
					Console.WriteLine(item["id"] + " " + item["filename"] + " " + item["creationdate"] + " " + item["type"]);
				}
			}
			else
			{
				Console.WriteLine(jsonObj["errorCode"] + " : " + jsonObj["errorMessage"]);
			}
		}

		/// <summary>
		/// Uploads a file to ShareFile.
		/// </summary>
		/// <param name="localPath">full path to local file, i.e. c:\\path\\to\\local.file</param>
		/// <param name="optionalParameters">name/value optional parameters</param>
		public void FileUpload(string localPath, Dictionary<string, object> optionalParameters)
		{
			FileInfo file = new FileInfo(localPath);

			Dictionary<string, object> requiredParameters = new Dictionary<string, object>();
			requiredParameters.Add("filename", file.Name);

			string url = BuildUrl("file", "upload", requiredParameters, optionalParameters);

			JObject jsonObj = InvokeShareFileOperation(url);
			if (!(bool)jsonObj["error"])
			{
				string uploadUrl = (string)jsonObj["value"];
				Console.WriteLine(uploadUrl);
				string rv = UploadMultiPartFile(file, uploadUrl);
				Console.WriteLine(rv);
			}
			else
			{
				throw new ApplicationException(string.Format("Failed to request upload: {0}, {1}", jsonObj["errorCode"], jsonObj["errorMessage"]));
			}
		}

		/// <summary>
		/// Downloads a file from ShareFile.
		/// </summary>
		/// <param name="fileId">id of the file to download</param>
		/// <param name="localPath">complete path to download file to, i.e. c:\\path\\to\\local.file</param>
		public void FileDownload(string fileId, string localPath)
		{
			Dictionary<string, object> requiredParameters = new Dictionary<string, object>();
			requiredParameters.Add("id", fileId);

			String url = BuildUrl("file", "download", requiredParameters);

			JObject jsonObj = InvokeShareFileOperation(url);
			if (!(bool)jsonObj["error"])
			{
				string downloadUrl = (string)jsonObj["value"];
				Console.WriteLine("downloadUrl = " + downloadUrl);

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(downloadUrl);
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();

				BufferedStream source = new BufferedStream(response.GetResponseStream());
				FileStream target = new FileStream(localPath, FileMode.Create);

				byte[] chunk = new byte[8192];
				int len = 0;
				while ((len = source.Read(chunk, 0, 8192)) > 0)
				{
					target.Write(chunk, 0, len);
				}
				Console.WriteLine("Download complete");

				response.Close();
			}
			else
			{
				Console.WriteLine(jsonObj["errorCode"] + " : " + jsonObj["errorMessage"]);
			}
		}

		/// <summary>
		/// Sends a Send a File email.
		/// </summary>
		/// <param name="path">path to file in ShareFile to send</param>
		/// <param name="to">email address to send to</param>
		/// <param name="subject">subject of the email</param>
		/// <param name="optionalParameters">name/value optional parameters</param>
		public void FileSend(string path, string to, string subject, Dictionary<string, object> optionalParameters)
		{
			Dictionary<string, object> requiredParameters = new Dictionary<string, object>();
			requiredParameters.Add("path", path);
			requiredParameters.Add("to", to);
			requiredParameters.Add("subject", subject);

			String url = BuildUrl("file", "send", requiredParameters, optionalParameters);

			JObject jsonObj = InvokeShareFileOperation(url);
			if (!(bool)jsonObj["error"])
			{
				string value = (string)jsonObj["value"];
				Console.WriteLine(value);
			}
			else
			{
				Console.WriteLine(jsonObj["errorCode"] + " : " + jsonObj["errorMessage"]);
			}
		}

		/// <summary>
		/// Creates a client or employee user in ShareFile.
		/// </summary>
		/// <param name="firstName">first name</param>
		/// <param name="lastName">last name</param>
		/// <param name="email">email address</param>
		/// <param name="isEmployee">true to create an employee, false to create a client</param>
		/// <param name="optionalParameters">name/value optional parameters</param>
		public void usersCreate(string firstName, string lastName, string email, bool isEmployee, Dictionary<string, object> optionalParameters)
		{
			Dictionary<string, object> requiredParameters = new Dictionary<string, object>();
			requiredParameters.Add("firstname", firstName);
			requiredParameters.Add("lastname", lastName);
			requiredParameters.Add("email", email);
			requiredParameters.Add("isemployee", isEmployee);

			String url = BuildUrl("users", "create", requiredParameters, optionalParameters);

			JObject jsonObj = InvokeShareFileOperation(url);
			if (!(bool)jsonObj["error"])
			{
				JObject user = (JObject)jsonObj["value"];
				Console.WriteLine(user["id"] + " " + user["primaryemail"]);
			}
			else
			{
				Console.WriteLine(jsonObj["errorCode"] + " : " + jsonObj["errorMessage"]);
			}
		}

		/// <summary>
		/// Creates a distribution group in ShareFile.
		/// </summary>
		/// <param name="name">name of group</param>
		/// <param name="optionalParameters">name/value optional parameters</param>
		public void GroupCreate(string name, Dictionary<string, object> optionalParameters)
		{
			Dictionary<string, object> requiredParameters = new Dictionary<string, object>();
			requiredParameters.Add("name", name);

			String url = BuildUrl("group", "create", requiredParameters, optionalParameters);

			JObject jsonObj = InvokeShareFileOperation(url);
			if (!(bool)jsonObj["error"])
			{
				JObject group = (JObject)jsonObj["value"];
				Console.WriteLine(group["id"] + " " + group["name"]);
			}
			else
			{
				Console.WriteLine(jsonObj["errorCode"] + " : " + jsonObj["errorMessage"]);
			}
		}

		/// <summary>
		/// Searches for items in ShareFile.
		/// </summary>
		/// <param name="query">search term</param>
		/// <param name="optionalParameters">name/value optional parameters</param>
		public void Search(string query, Dictionary<string, object> optionalParameters)
		{
			Dictionary<string, object> requiredParameters = new Dictionary<string, object>();
			requiredParameters.Add("query", query);

			String url = BuildUrl("search", "search", requiredParameters, optionalParameters);

			JObject jsonObj = InvokeShareFileOperation(url);
			if (!(bool)jsonObj["error"])
			{
				JArray items = (JArray)jsonObj["value"];
				if (items.Count == 0)
				{
					Console.WriteLine("No Results");
					return;
				}

				string path = "";
				foreach (JObject item in items)
				{
					path = "/";
					if (((string)item["parentid"]).Equals("box"))
					{
						path = "/File Box";
					}
					else
					{
						path = (string)item["parentsemanticpath"];
					}
					Console.WriteLine(path + "/" + item["filename"] + " " + item["creationdate"] + " " + item["type"]);
				}
			}
			else
			{
				Console.WriteLine(jsonObj["errorCode"] + " : " + jsonObj["errorMessage"]);
			}
		}


		// ------------------------------------------- Utility Functions ------------------------------------------- 
		private string BuildUrl(string endpoint, string op, Dictionary<string, object> requiredParameters)
		{
			return (BuildUrl(endpoint, op, requiredParameters, new Dictionary<string, object>()));
		}

		private string BuildUrl(string endpoint, string op, Dictionary<string, object> requiredParameters, Dictionary<string, object> optionalParameters)
		{

			requiredParameters.Add("authid", this.authId);
			requiredParameters.Add("op", op);
			requiredParameters.Add("fmt", "json");

			ArrayList parameters = new ArrayList();
			foreach (KeyValuePair<string, object> kv in requiredParameters)
			{
				parameters.Add(string.Format("{0}={1}", HttpUtility.UrlEncode(kv.Key), HttpUtility.UrlEncode(kv.Value.ToString())));
			}
			foreach (KeyValuePair<string, object> kv in optionalParameters)
			{
				parameters.Add(string.Format("{0}={1}", HttpUtility.UrlEncode(kv.Key), HttpUtility.UrlEncode(kv.Value.ToString())));
			}

			String url = string.Format("https://{0}.{1}/rest/{2}.aspx?{3}", this.subdomain, this.tld, endpoint, String.Join("&", parameters.ToArray()));
			Console.WriteLine(url);

			return (url);
		}

		private JObject InvokeShareFileOperation(string requestUrl)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream());
			String json = reader.ReadToEnd();
			response.Close();
			return JObject.Parse(json);
		}

		private string UploadMultiPartFile(FileInfo fileInfo, string fileUploadUrl)
		{
			string contentType;

			byte[] uploadData = CreateMultipartFormUpload(fileInfo, fileInfo.Name, out contentType);

			string rv = Send(fileUploadUrl, contentType, uploadData);
			return (rv);
		}

		private Byte[] CreateMultipartFormUpload(FileInfo fileInfo, string remoteFilename, out string contentType)
		{
			string boundaryGuid = "upload-" + Guid.NewGuid().ToString("n");
			contentType = "multipart/form-data; boundary=" + boundaryGuid;

			MemoryStream ms = new MemoryStream();
			byte[] boundaryBytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundaryGuid + "\r\n");

			// Write MIME header
			ms.Write(boundaryBytes, 2, boundaryBytes.Length - 2);
			string header = String.Format(@"Content-Disposition: form-data; name=""{0}""; filename=""{1}""" +
				"\r\nContent-Type: application/octet-stream\r\n\r\n", "File1", remoteFilename);
			byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(header);
			ms.Write(headerBytes, 0, headerBytes.Length);

			// Load the file into the byte array
			using (FileStream file = fileInfo.OpenRead())
			{
				byte[] buffer = new byte[1024 * 1024];
				int bytesRead;

				while ((bytesRead = file.Read(buffer, 0, buffer.Length)) > 0)
				{
					ms.Write(buffer, 0, bytesRead);
				}
			}

			// Write MIME footer
			boundaryBytes = System.Text.Encoding.UTF8.GetBytes("\r\n--" + boundaryGuid + "--\r\n");
			ms.Write(boundaryBytes, 0, boundaryBytes.Length);

			byte[] retVal = ms.ToArray();
			ms.Close();

			return retVal;
		}

		private string Send(string url, string contenttype, byte[] postBytes)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Timeout = 1000 * 60 * 30; // 60 seconds * 30 = 30 minutes
			request.Method = "POST";
			request.ContentType = contenttype;
			request.ContentLength = postBytes.Length;
			request.Credentials = CredentialCache.DefaultCredentials;

			using (Stream postStream = request.GetRequestStream())
			{
				int chunkSize = 512 * 1024;
				int remaining = postBytes.Length;
				int offset = 0;

				do
				{
					if (chunkSize > remaining) { chunkSize = remaining; }
					postStream.Write(postBytes, offset, chunkSize);

					remaining -= chunkSize;
					offset += chunkSize;

				} while (remaining > 0);

				postStream.Close();
			}

			String retVal;

			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				StreamReader rdr = new StreamReader(response.GetResponseStream());
				retVal = rdr.ReadToEnd();
				response.Close();
			}

			return retVal;
		}

		public static void SampleMain(string[] args)
		{
			ShareFileSample sample = new ShareFileSample();
			Dictionary<string, object> optionalParameters = new Dictionary<string, object>();

			bool loginStatus = sample.Authenticate("mysubdomain", "sharefile.com", "my@email.address", "mypassword");
			if (loginStatus)
			{
				sample.FolderList("/MyFolder");
			}
		}
	}


}