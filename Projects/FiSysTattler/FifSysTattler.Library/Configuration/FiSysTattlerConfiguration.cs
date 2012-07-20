using System;
using System.Collections.Generic;
using System.IO;
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
			
			using (var memStream = SerializationHelper.SerializeToXmlText(this, ns))
			{
				if (File.Exists(filePath))
				{
					var ext = Path.GetExtension(filePath);
					var fileName = Path.GetFileNameWithoutExtension(filePath);

					File.Move(filePath, Path.Combine(folderPath, fileName + "." + Path.GetRandomFileName() + ext));
				}

				using (var writer = File.OpenWrite(filePath))
				{
					var buffer = new byte[1024];
					while (memStream.Read(buffer, 0, buffer.Length) > 0)
					{
						writer.Write(buffer, 0, buffer.Length);
					}
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
