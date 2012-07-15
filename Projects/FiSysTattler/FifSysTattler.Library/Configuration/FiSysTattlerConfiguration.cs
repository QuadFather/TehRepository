using System;
using System.Collections.Generic;
using System.IO;
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
			
		}

		public static FiSysTattlerConfiguration LoadConfiguration(string filePath)
		{
			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("The filepath: {0}, could not be found.", filePath);
			}

			var config = new FiSysTattlerConfiguration();
			//TODO: fix this cheap hack by properly exposing a way to get a an serializer for a type... lazy bastard
			var serializer = config.GetXmlSerializer();

			using (var fileStream = File.OpenRead(filePath))
			using (var memStream = new MemoryStream())
			{
				memStream.SetLength(fileStream.Length);
				fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
				memStream.Seek(0, SeekOrigin.Begin);

				config = SerializationHelper.DeSerializerFromXmlText<FiSysTattlerConfiguration>(serializer, memStream);
			}
			
			return config;
		}
	}
}
