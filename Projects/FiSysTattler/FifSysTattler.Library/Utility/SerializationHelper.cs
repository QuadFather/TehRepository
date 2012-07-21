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

		public static string SerializeToXmlText<T>(T item, XmlSerializerNamespaces namespaces = null)
		{
			using (var memStream = SerializeToXmlTextStream(item, namespaces))
			{
				return Encoding.UTF8.GetString(memStream.GetBuffer());
			}
		}

		public static MemoryStream SerializeToXmlTextStream<T>(T item, XmlSerializerNamespaces namespaces = null)
		{
			var serializer = new XmlSerializer(typeof(T));

			var memStream = new MemoryStream();

			var writer = new XmlTextWriter(memStream, Encoding.UTF8);

			if (namespaces != null)
			{
				serializer.Serialize(writer, item, namespaces);
			}
			else
			{
				serializer.Serialize(writer, item);
			}

			memStream.Seek(0, SeekOrigin.Begin);

			//TODO: Joo are here!
			//var encodedXml = Encoding.UTF8.GetBytes();

			return memStream;
		}
	}
}
