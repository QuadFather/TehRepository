using System.IO;

namespace FifSysTattler.Library
{
	public class FileSystemWatchItem
	{
		public string Path { get; private set; }
		public string Filter { get; private set; }
		public bool IncludeSubDirectories { get; private set; }
		public NotifyFilters NotifyFilters { get; private set; }
		public int InternalBufferSize { get; private set; }
	}
}