using System;
using System.Collections.Generic;
using FifSysTattler.Library.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FiSysTattler.Library.Tests.Extensions
{
	[TestClass]
	public class ObjectExtensionsTests
	{
		#region Private

		#region Helper Classes

		[Serializable]
		public class SerializableTestCopyItem
		{
			public int? Id { get; set; }
			public string Name { get; set; }
			public DateTime? Date { get; set; }
			public long TimeStamp { get; set; }
			public Guid Uid { get; set; }

			public SerializableTestCopyItem()
				: this(string.Empty)
			{
			}

			public SerializableTestCopyItem(string name)
			{
				Id = 1;
				Name = name;
				Date = DateTime.Now;
				TimeStamp = DateTime.Now.Ticks;
				Uid = Guid.NewGuid();
			}
		}

		public class XmlSerializableTestCopyItem
		{
			public int? Id { get; set; }
			public string Name { get; set; }
			public DateTime? Date { get; set; }
			public long TimeStamp { get; set; }
			public Guid Uid { get; set; }

			public XmlSerializableTestCopyItem()
				: this(string.Empty)
			{
			}

			public XmlSerializableTestCopyItem(string name)
			{
				Id = 1;
				Name = name;
				Date = DateTime.Now;
				TimeStamp = DateTime.Now.Ticks;
				Uid = Guid.NewGuid();
			}
		}

		public class NotSerializableTestCopyItem
		{
			public interface IWord
			{
				string Word { get; set; }
			}

			public IWord Word { get; set; }
		}

		#endregion //Local classes for Tests

		#region Helper functions

		private static void AssertSerializableCopyItem(SerializableTestCopyItem source, SerializableTestCopyItem destination)
		{
			Assert.AreNotSame(source, destination);
			Assert.AreEqual(source.Date, destination.Date);
			Assert.AreEqual(source.Id, destination.Id);
			Assert.AreEqual(source.Name, destination.Name);
			Assert.AreEqual(source.TimeStamp, destination.TimeStamp);
			Assert.AreEqual(source.Uid, destination.Uid);
		}

		private static void AssertXmlSerializableCopyItem(XmlSerializableTestCopyItem source,
														  XmlSerializableTestCopyItem destination)
		{
			Assert.AreNotSame(source, destination);
			Assert.AreEqual(source.Date, destination.Date);
			Assert.AreEqual(source.Id, destination.Id);
			Assert.AreEqual(source.Name, destination.Name);
			Assert.AreEqual(source.TimeStamp, destination.TimeStamp);
			Assert.AreEqual(source.Uid, destination.Uid);
		}

		#endregion //Helper functions

		#endregion //Private

		[TestMethod]
		public void Binary_DeepCopy_Should_Succeed()
		{
			var source = new SerializableTestCopyItem("Some Name");

			var destination = source.DeepCopy(DeepCopyMode.Binary);

			AssertSerializableCopyItem(source, destination);
		}

		[TestMethod]
		public void Xml_DeepCopy_Should_Succeed()
		{
			var source = new SerializableTestCopyItem("Some Name");

			var destination = source.DeepCopy(DeepCopyMode.Xml);

			AssertSerializableCopyItem(source, destination);
		}

		//TODO: Determine if there is a better way to express this and make it less brittle of a test
		/// <summary>
		/// Tests TryDeepCopy. It depends on using TryDeepCopy on an object that does 
		/// use the Serializable attribute, or implement the ISerializable interface.
		/// The algorithm tries to do this first. (at the time of this writing.)
		/// </summary>
		[TestMethod]
		public void Try_DeepCopy_Binary_Should_Succeed()
		{
			var source = new SerializableTestCopyItem("Some Name");
			SerializableTestCopyItem destination;

			//var x = Mock

			if (source.TryDeepCopy(out destination))
			{
				Assert.AreNotSame(source, destination);
				AssertSerializableCopyItem(source, destination);
			}
		}

		//TODO: Determine if there is a better way to express this and make it less brittle of a test
		/// <summary>
		/// Tests TryDeepCopy. It depends on using TryDeepCopy on an object that does not 
		/// use the Serializable attribute, or implement the ISerializable interface
		/// </summary>
		[TestMethod]
		public void Try_DeepCopy_Xml_Should_Succeed()
		{
			var source = new XmlSerializableTestCopyItem("Some Name");
			XmlSerializableTestCopyItem destination;

			if (source.TryDeepCopy(out destination))
			{
				Assert.AreNotSame(source, destination);
				AssertXmlSerializableCopyItem(source, destination);
			}
		}

		[TestMethod]
		public void Try_DeepCopy_NotSerializable_Class_Should_Succeed()
		{
			var source = new NotSerializableTestCopyItem();
			NotSerializableTestCopyItem destination;

			if (source.TryDeepCopy(out destination))
			{
				Assert.AreNotSame(source, destination);
				Assert.AreEqual(default(NotSerializableTestCopyItem), destination);
			}
		}
	}
}
