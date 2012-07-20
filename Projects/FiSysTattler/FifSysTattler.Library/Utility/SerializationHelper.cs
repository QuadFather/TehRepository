using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FifSysTattler.Library.Utility
{
	public static class SerializationHelper
	{
		public static T DeSerializerFromXmlText<T>(MemoryStream memStream)
		{
			return (T)new XmlSerializer(typeof(T)).Deserialize(memStream);
		}

		public static MemoryStream SerializeToXmlText<T>(T item, XmlSerializerNamespaces namespaces = null)
		{
			var serializer = new XmlSerializer(typeof (T));

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
