using System;
using System.Collections.Generic;
using System.IO;
using FifSysTattler.Library.Configuration;
using FifSysTattler.Library.Extensions;
//Aliasing Dictionary<object<List<FileSystemWatchItem>> to WatcherDictionary for the purposes of brevity
using WatcherDictionary = System.Collections.Generic.Dictionary<object, System.Collections.Generic.List<FifSysTattler.Library.Configuration.FileSystemWatchItem>>;

namespace FifSysTattler.Library
{
	public class WatcherManager : IDisposable
	{
		private WatcherDictionary Watchers { get; set; }

		#region Constuctors / Destructors

		public WatcherManager()
		{
			Watchers = new WatcherDictionary();
		}

		~WatcherManager()
		{
			Dispose(false);
		}

		#endregion //Constuctors / Destructors

		#region Private Methods

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

		private void InitializeWatchList(object key, List<FileSystemWatchItem> watchList)
		{
			foreach (var watchItem in watchList)
			{
				var fileSystemWatcher = new FileSystemWatcher(watchItem.Path, watchItem.Filter)
					{
						IncludeSubdirectories = watchItem.IncludeSubDirectories,
						InternalBufferSize = watchItem.InternalBufferSize,
					};

				if (watchItem.IsActive)
				{
					fileSystemWatcher.EnableRaisingEvents = false;

					fileSystemWatcher.Created += Watcher_Created;
					fileSystemWatcher.Deleted += Watcher_Deleted;
					fileSystemWatcher.Error += Watcher_Error;
					fileSystemWatcher.Renamed += Watcher_Renamed;
				}

				watchItem.FileSystemWatcher = fileSystemWatcher;
			}

			Watchers.SmartAdd(key, watchList);
		}

		private void TearDownAllWatchers(WatcherDictionary watchers)
		{
			foreach (var pair in watchers)
			{
				TearDownWatchers(pair.Value);
			}
		}

		private void TearDownWatchers(List<FileSystemWatchItem> watcherList)
		{
			watcherList.ForEach(fsw =>  {
											fsw.IsActive = false;
											TearDownWatcher(fsw.FileSystemWatcher);
										});
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

		#endregion //Private Methods

		#region Public Method

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				//free managed resources
				TearDownAllWatchers(Watchers);
				Watchers.Clear();
				Watchers = null;
			}

			//free native resources
		}

		#endregion

		#region Manager Functions

		public void Load(object key, string configFilePath)
		{
			if (string.IsNullOrWhiteSpace(configFilePath))
			{
				throw new ArgumentException("configFilePath can not be null.", "configFilePath");
			}

			Watchers.Clear();

			AddConfig(key, configFilePath);
		}

		public void AddConfig(object key, string configFilePath)
		{
			var watcherConfig = FiSysTattlerConfiguration.LoadConfiguration(configFilePath);
			AddConfig(key, watcherConfig);
		}
		
		public void AddConfig(object key, FiSysTattlerConfiguration watcherConfig)
		{
			InitializeWatchList(key, watcherConfig.Watches);
		}

		#endregion //Manager Functions

		#endregion //Public Method
	}
}
