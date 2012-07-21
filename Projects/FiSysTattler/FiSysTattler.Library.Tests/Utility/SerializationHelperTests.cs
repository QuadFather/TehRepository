using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FifSysTattler.Library.Configuration;
using FifSysTattler.Library.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FiSysTattler.Library.Tests.Utility
{
	[TestClass]
	public class SerializationHelperTests
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

		#endregion //Private Methods

		[TestMethod]
		public void Serialize_To_Xml_Stream_Should_Succeed()
		{
			var config = GetTestConfiguration();

			var xStream = SerializationHelper.SerializeToXmlTextStream(config);

			var xDoc = XDocument.Load(xStream);

			Assert.IsTrue(xDoc.Descendants("VersionInfo").Count() == 1);
			
			var versionInfo = xDoc.Descendants("VersionInfo").Single();
			var name = versionInfo.Attribute("Name").Value;
			var lastModified = versionInfo.Attribute("LastModified").Value;
			var modifiedBy = versionInfo.Attribute("ModifiedBy").Value;

			Assert.AreEqual(config.VersionInfo.LastModified, DateTime.Parse(lastModified));
			Assert.AreEqual(config.VersionInfo.ModifiedBy, modifiedBy);
			Assert.AreEqual(config.VersionInfo.Name, name);

			var watches = xDoc.Descendants("Watches");

			Assert.IsTrue(xDoc.Descendants("Watches").Count() == 1);

			var watchItems = watches.Descendants("FileSystemWatchItem");
			int index = 0;
			foreach (var watchItem in watchItems)
			{
				var watchName = watchItem.Attribute("Name").Value;
				var watchIncludeSubDirectories = bool.Parse(watchItem.Attribute("IncludeSubDirectories").Value);
				var watchInternalBufferSize = int.Parse(watchItem.Attribute("InternalBufferSize").Value);
				var watchIsActive = bool.Parse(watchItem.Attribute("IsActive").Value);
				var watchDescription = watchItem.Element("Description").Value;
				var watchPath = watchItem.Element("Path").Value;
				var watchFilter = watchItem.Element("Filter").Value;
				var watchNotifyFilters = watchItem.Element("NotifyFilters").Value;

				Assert.AreEqual(config.Watches[index].Description, watchDescription);
				Assert.AreEqual(config.Watches[index].IncludeSubDirectories, watchIncludeSubDirectories);
				Assert.AreEqual(config.Watches[index].InternalBufferSize, watchInternalBufferSize);
				Assert.AreEqual(config.Watches[index].IsActive, watchIsActive);
				Assert.AreEqual(config.Watches[index].Name, watchName);
				Assert.AreEqual(config.Watches[index].Path, watchPath);
				Assert.AreEqual(config.Watches[index].Filter, watchFilter);
				Assert.AreEqual(config.Watches[index].NotifyFilters.ToString().Replace(",", ""), watchNotifyFilters);
				index++;
			}
		}

		[TestMethod]
		public void Serialize_To_Xml_String_Should_Succeed()
		{
			var config = GetTestConfiguration();

			var xmlText = SerializationHelper.SerializeToXmlText(config);

			Assert.IsFalse(string.IsNullOrWhiteSpace(xmlText));

			var xDoc = new XmlDocument();
			xDoc.LoadXml(xmlText);
		}
	}
}
