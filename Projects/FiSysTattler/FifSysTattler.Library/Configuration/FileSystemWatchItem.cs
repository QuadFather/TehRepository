using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FifSysTattler.Library.Configuration
{
	[Serializable]
	public class FileSystemWatchItem
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Path { get; set; }
		public string Filter { get; set; }
		public bool IncludeSubDirectories { get; set; }
		public NotifyFilters NotifyFilters { get; set; }
		public int InternalBufferSize { get; set; }

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

	//[Serializable]
	//internal class FileSystemWatchItemInternal : FileSystemWatchItem
	//{
	//	public string NotifyFiltersText
	//	{
	//		get
	//		{
	//			var builder = new StringBuilder();
	//			var notifyFiltersType = this.NotifyFilters.GetType();

	//			var enumNames = Enum.GetNames(notifyFiltersType);
	//			foreach (var enumName in enumNames)
	//			{
	//				if (Enum.IsDefined(notifyFiltersType, this.NotifyFilters))
	//				{
	//					builder.AppendFormat("{0}, ", enumName);
	//				}
	//			}

	//			return builder.ToString(0, builder.Length - 2);
	//		}
	//		set
	//		{
	//			var filterTexts = value.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);

	//			var filters = new List<NotifyFilters>();
	//			for (var i = 0; i < filterTexts.Length; i++ )
	//			{
	//				NotifyFilters notifyFilters;
	//				if (NotifyFilters.TryParse(filterTexts[i].Trim(), true, out notifyFilters))
	//				{
	//					if (i == 0)
	//					{
	//						this.NotifyFilters = notifyFilters;
	//					}
	//					else
	//					{
	//						this.NotifyFilters |= notifyFilters;
	//					}
	//				}
	//			}
	//		}
	//	}

	//	public FileSystemWatchItem ToFileSystemWatchItem()
	//	{
	//		return new FileSystemWatchItem
	//			{
	//				NotifyFilters = this.NotifyFilters,
	//				Filter = this.Filter,
	//				IncludeSubDirectories = this.IncludeSubDirectories,
	//				InternalBufferSize = this.InternalBufferSize,
	//				IsActive = this.IsActive,
	//				Path = this.Path
	//			};
	//	}
	//}
}