using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Xml.Serialization;

namespace FifSysTattler.Library.Configuration
{
	[Serializable]
	public class FileSystemWatchItem
	{
		[XmlAttribute]
		public string Name { get; set; }
		[XmlElement]
		public string Description { get; set; }
		[XmlElement]
		public string Path { get; set; }
		[XmlElement]
		public string Filter { get; set; }
		[XmlAttribute]
		public bool IncludeSubDirectories { get; set; }
		//[XmlArray]
		public NotifyFilters NotifyFilters { get; set; }
		[XmlAttribute]
		public int InternalBufferSize { get; set; }
		[XmlAttribute]
		public bool IsActive { get; set; }

		public FileSystemWatchItem()
		{
			IsActive = false;
			NotifyFilters = NotifyFilters.Attributes |
			                NotifyFilters.CreationTime |
			                NotifyFilters.DirectoryName |
			                NotifyFilters.FileName |
			                NotifyFilters.LastAccess |
			                NotifyFilters.LastWrite |
			                NotifyFilters.Security |
			                NotifyFilters.Size;
			InternalBufferSize = 8192;
			IncludeSubDirectories = false;
		}
	}
}