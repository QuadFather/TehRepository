using System;
using System.Xml.Serialization;

namespace FifSysTattler.Library.Configuration
{
	[Serializable]
	[XmlRoot]
	public class Version
	{
		[XmlAttribute]
		public string Name { get; set; }

		[XmlAttribute]
		public DateTime LastModified { get; set; }

		[XmlAttribute]
		public string ModifiedBy { get; set; }
	}
}