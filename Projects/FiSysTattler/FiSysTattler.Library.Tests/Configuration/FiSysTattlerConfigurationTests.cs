using System;
using System.Collections.Generic;
using System.IO;
using FifSysTattler.Library.Configuration;
using FifSysTattler.Library.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FiSysTattler.Library.Tests.Configuration
{
	[TestClass]
	public class FiSysTattlerConfigurationTests
	{
		#region Private Methods

		#region Test Helpers

		private static FiSysTattlerConfiguration GetTestConfiguration()
		{
			var expectedConfig = FiSysTattlerConfiguration.GetNewDefaultConfiguration();

			expectedConfig.VersionInfo = new FifSysTattler.Library.Configuration.Version
				{
					LastModified = DateTime.Now,
					ModifiedBy = "JimmyX",
					Name = "Test Name 1"
				};

			expectedConfig.Watches = new List<FileSystemWatchItem>
				{
					new FileSystemWatchItem
						{
							Description = "description 1",
							Filter = "filter1",
							IncludeSubDirectories = true,
							InternalBufferSize = 4096,
							IsActive = true,
							Name = "name 1",
							Path = "C:\\Temp",
							NotifyFilters = NotifyFilters.DirectoryName | NotifyFilters.FileName
						},
					new FileSystemWatchItem
						{
							Description = "description 2",
							Filter = "filter2",
							IncludeSubDirectories = false,
							InternalBufferSize = 3076,
							IsActive = false,
							Name = "name 2",
							Path = "C:\\Temp\\OhNoes",
							NotifyFilters = NotifyFilters.DirectoryName | NotifyFilters.FileName
						}
				};

			return expectedConfig;
		}

		#endregion //Test Helpers

		#region Reusable Assertions

		private static void AssertForWatch(FileSystemWatchItem expectedWatch, FileSystemWatchItem actualWatch)
		{
			Assert.AreEqual(expectedWatch.Description, actualWatch.Description);
			Assert.AreEqual(expectedWatch.Filter, actualWatch.Filter);
			Assert.AreEqual(expectedWatch.IncludeSubDirectories, actualWatch.IncludeSubDirectories);
			Assert.AreEqual(expectedWatch.InternalBufferSize, actualWatch.InternalBufferSize);
			Assert.AreEqual(expectedWatch.IsActive, actualWatch.IsActive);
			Assert.AreEqual(expectedWatch.Name, actualWatch.Name);
			Assert.AreEqual(expectedWatch.NotifyFilters, actualWatch.NotifyFilters);
			Assert.AreEqual(expectedWatch.Path, actualWatch.Path);
		}

		#endregion //Reusable Assertions

		#endregion //Private Methods

		[TestMethod]
		public void Save_Config_Writes_File_Should_Succeed()
		{
			var expectedConfig = GetTestConfiguration();

			var outputPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
			expectedConfig.SaveConfiguration(outputPath);

			Assert.IsTrue(File.Exists(outputPath));
		}

		[TestMethod]
		[ExpectedException(typeof(DirectoryNotFoundException))]
		public void Save_Config_Writes_To_Bad_Path_File_Should_Except()
		{
			var expectedConfig = GetTestConfiguration();

			var path = string.Format("Z:\\{0}\\", Guid.NewGuid());
			expectedConfig.SaveConfiguration(path);
		}

		[TestMethod]
		[ExpectedException(typeof(DirectoryNotFoundException))]
		public void Save_Config_Writes_To_Null_Path_File_Should_Except()
		{
			var expectedConfig = GetTestConfiguration();
			
			expectedConfig.SaveConfiguration(null);
		}

		[TestMethod]
		public void Load_Config_Should_Succees()
		{
			var expectedConfig = GetTestConfiguration();

			var outputPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
			expectedConfig.SaveConfiguration(outputPath);

			var actualConfig = FiSysTattlerConfiguration.LoadConfiguration(outputPath);

			Assert.AreEqual(expectedConfig.VersionInfo.LastModified, actualConfig.VersionInfo.LastModified);
			Assert.AreEqual(expectedConfig.VersionInfo.ModifiedBy, actualConfig.VersionInfo.ModifiedBy);
			Assert.AreEqual(expectedConfig.VersionInfo.Name, actualConfig.VersionInfo.Name);
			Assert.AreEqual(expectedConfig.Watches.Count, actualConfig.Watches.Count);

			AssertForWatch(expectedConfig.Watches[0], actualConfig.Watches[0]);
			AssertForWatch(expectedConfig.Watches[1], actualConfig.Watches[1]);
		}

		[TestMethod]
		[ExpectedException(typeof(FileNotFoundException))]
		public void Load_Config_From_Bad_Path_Should_Except()
		{
			var path = string.Format("z:\\{0}", Guid.NewGuid().ToString());

			var actualConfig = FiSysTattlerConfiguration.LoadConfiguration(path);
		}
	}
}
