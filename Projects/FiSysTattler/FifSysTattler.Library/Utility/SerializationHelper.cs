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
				var bytes = new byte[memStream.Length];
				memStream.Read(bytes, 0, bytes.Length);
				var encoded = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, bytes);
				return Encoding.Unicode.GetString(encoded);
			}
		}

		public static MemoryStream SerializeToXmlTextStream<T>(T item, XmlSerializerNamespaces namespaces = null)
		{
			var serializer = new XmlSerializer(typeof(T));
			byte[] encodedBytes = null;

			using (var memStream = new MemoryStream())
			{
				var writer = new XmlTextWriter(memStream, null /*Encoding.UTF8*/);

				if (namespaces != null)
				{
					serializer.Serialize(writer, item, namespaces);
				}
				else
				{
					serializer.Serialize(writer, item);
				}

				memStream.Seek(0, SeekOrigin.Begin);

				var bytes = new byte[memStream.Length];
				memStream.Read(bytes, 0, bytes.Length);

				encodedBytes = Encoding.Convert(Encoding.UTF8, Encoding.Unicode, bytes);
			}

			return new MemoryStream(encodedBytes);
		}
	}
}
