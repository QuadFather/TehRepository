using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifSysTattler.Library
{
	public class WatcherManager :IDisposable
	{
		private Dictionary<object, List<FileSystemWatcher>> Watchers { get; set; }

		public WatcherManager()
		{
			Watchers = new Dictionary<object, List<FileSystemWatcher>>();

			//InitializeWatchers(Watchers);
		}

		~WatcherManager()
		{
			Dispose(false);
		}

		private void InitializeWatchers(Dictionary<object, List<FileSystemWatcher>> watchers)
		{
			foreach (var watcher in watchers.SelectMany(pair => pair.Value))
			{
				InitializeWatcher(watcher);
			}
		}

		private void TearDownWatchers(Dictionary<object, List<FileSystemWatcher>> watchers)
		{
			foreach (var watcher in watchers.SelectMany(pair => pair.Value))
			{
				TearDownWatcher(watcher);
			}
		}

		private void InitializeWatcher(FileSystemWatcher watcher)
		{
			watcher.EnableRaisingEvents = true;

			watcher.Changed += Watcher_Changed;
			watcher.Created += Watcher_Created;
			watcher.Deleted += Watcher_Deleted;
			watcher.Error += Watcher_Error;
			watcher.Renamed += Watcher_Renamed;
		}

		private void TearDownWatcher(FileSystemWatcher watcher)
		{
			watcher.EnableRaisingEvents = false;

			watcher.Changed -= Watcher_Changed;
			watcher.Created -= Watcher_Created;
			watcher.Deleted -= Watcher_Deleted;
			watcher.Error -= Watcher_Error;
			watcher.Renamed -= Watcher_Renamed;
		}

		#region FileSystemWatcher Event Handlers

		void Watcher_Renamed(object sender, RenamedEventArgs e)
		{
			throw new NotImplementedException();
		}

		void Watcher_Error(object sender, ErrorEventArgs e)
		{
			throw new NotImplementedException();
		}

		void Watcher_Deleted(object sender, FileSystemEventArgs e)
		{
			throw new NotImplementedException();
		}

		void Watcher_Created(object sender, FileSystemEventArgs e)
		{
			throw new NotImplementedException();
		}

		void Watcher_Changed(object sender, FileSystemEventArgs e)
		{
			throw new NotImplementedException();
		}

		#endregion //FileSystemWatcher Event Handlers

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void Dispose(bool disposing)
		{
			if (disposing)
			{
				//free managed resources
				TearDownWatchers(Watchers);
			}

			//free native resources
		}

		#endregion
	}
}
