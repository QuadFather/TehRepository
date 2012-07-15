using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FifSysTattler.Library.Utility;

namespace FifSysTattler.Library.Extensions
{
	public static class ObjectExtensions
	{
		public static T DeepCopy<T>(this T obj, DeepCopyMode mode = DeepCopyMode.Binary)
		{
			return (mode == DeepCopyMode.Binary)
						? BinaryDeepCopy(obj) 
						: XmlDeepCopy(obj);
		}

		public static bool TryDeepCopy<T>(this T obj, out T copiedObj)
		{
			try
			{
				copiedObj = BinaryDeepCopy(obj);
				return true;
			}
			catch { }

			try
			{
				copiedObj = XmlDeepCopy(obj);
				return true;
			}
			catch { }

			copiedObj = default(T);
			return false;
		}

		private static T XmlDeepCopy<T>(T item)
		{
			T copiedItem;
			var serializer = new XmlSerializer(item.GetType());
			using (var memStream = SerializationHelper.SerializeToXmlText(item, serializer))
			{
				copiedItem = SerializationHelper.DeSerializerFromXmlText<T>(serializer, memStream);
			}
			return copiedItem;
		}

		public static XmlSerializer GetXmlSerializer<T>(this T obj)
		{
			return new XmlSerializer(obj.GetType());
		}

		private static T BinaryDeepCopy<T>(T item)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, item);
				ms.Position = 0;

				return (T)formatter.Deserialize(ms);
			}
		}

		public enum DeepCopyMode
		{
			Binary,
			Xml
		}
	}
}
