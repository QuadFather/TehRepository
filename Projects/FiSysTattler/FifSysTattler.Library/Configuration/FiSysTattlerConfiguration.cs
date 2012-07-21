using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FifSysTattler.Library.Extensions;
using FifSysTattler.Library.Utility;

namespace FifSysTattler.Library.Configuration
{
	[Serializable]
	[XmlRoot]
	public class FiSysTattlerConfiguration
	{
		[XmlAttribute]
		public const string FormatVersion = "1.0.0.0";

		[XmlElement]
		public Version VersionInfo { get; set; }

		[XmlArray]
		public List<FileSystemWatchItem> Watches { get; set; }

		private FiSysTattlerConfiguration()
		{
			var now = DateTime.Now;
			VersionInfo = new Version
				{
					LastModified = DateTime.Now,
					ModifiedBy = Environment.UserName,
					Name = string.Format("New Configuration {0}", now.ToString("u"))
				};

			Watches = new List<FileSystemWatchItem>();
		}

		public void SaveConfiguration(string filePath)
		{
			var folderPath = Path.GetDirectoryName(filePath);

			if (folderPath == null || !Directory.Exists(folderPath))
			{
				throw new DirectoryNotFoundException(string.Format(
														"The directory: {0}, could not be found. File: {1} could not be created.",
														folderPath ?? "{Not Provided}",
														filePath ?? "{Not Provided}"));
			}

			var ns = new XmlSerializerNamespaces();
			ns.Add(string.Empty, string.Empty);

			using (var memStream = SerializationHelper.SerializeToXmlTextStream(this, ns))
			{
				if (File.Exists(filePath))
				{
					var ext = Path.GetExtension(filePath);
					var fileName = Path.GetFileNameWithoutExtension(filePath);

					File.Move(filePath, Path.Combine(folderPath, fileName + "." + Path.GetRandomFileName() + ext));
				}

				//var xmlText = Encoding.UTF8.GetString(memStream.GetBuffer());

				//var utf8Bytes = new byte[memStream.Length];
				//memStream.Read(utf8Bytes, 0, utf8Bytes.Length);
				//var unicodeBytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, utf8Bytes);

				var xDoc = new XmlDocument();
				//xDoc.LoadXml(Encoding.Unicode.GetString(unicodeBytes));
				xDoc.Load(memStream);

				var settings = new XmlWriterSettings
									{
										Indent = true,
										IndentChars = "  ",
										NewLineChars = "\r\n",
										NewLineHandling = NewLineHandling.Replace,
										OmitXmlDeclaration = true
									};
				
				using (var writer = XmlWriter.Create(filePath, settings))
				{
					xDoc.Save(writer);
				}
			}
		}

		public static FiSysTattlerConfiguration LoadConfiguration(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("The filepath: {0}, could not be found.", filePath);
			}

			var config = new FiSysTattlerConfiguration();

			using (var fileStream = File.OpenRead(filePath))
			using (var memStream = new MemoryStream())
			{
				memStream.SetLength(fileStream.Length);
				fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
				memStream.Seek(0, SeekOrigin.Begin);

				config = SerializationHelper.DeSerializerFromXmlText<FiSysTattlerConfiguration>(memStream);
			}

			return config;
		}

		public static FiSysTattlerConfiguration GetNewDefaultConfiguration()
		{
			return new FiSysTattlerConfiguration();
		}
	}
}
