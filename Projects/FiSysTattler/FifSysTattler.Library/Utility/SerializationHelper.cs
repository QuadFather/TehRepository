using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FifSysTattler.Library.Utility
{
	public class SerializationHelper
	{
		public static T DeSerializerFromXmlText<T>(XmlSerializer serializer, MemoryStream memStream)
		{
			return (T)serializer.Deserialize(memStream);
		}

		public static MemoryStream SerializeToXmlText<T>(T item, XmlSerializer serializer, XmlSerializerNamespaces namespaces = null)
		{
			var memStream = new MemoryStream();

			var xmlText = new XmlTextWriter(memStream, Encoding.UTF8);
			
			if (namespaces != null)
			{
				serializer.Serialize(xmlText, item, namespaces);
			}
			else
			{
				serializer.Serialize(xmlText, item);
			}
			
			memStream.Seek(0, SeekOrigin.Begin);

			return memStream;
		}
	}
}
