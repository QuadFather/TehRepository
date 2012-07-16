using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FifSysTattler.Library.Configuration;

namespace FiSysTattler.Console.Sandbox
{
	class Program
	{
		static void Main(string[] args)
		{
			//This will create a new WatchItemConfig xml file, just playing around for the time being
			var config = FiSysTattlerConfiguration.GetNewDefaultConfiguration();
			var item = new FileSystemWatchItem
						{
							Name = "My first watch item",
							Description = "My very first description of a watch item, yay!"
						};

			config.Watches.Add(item);
			config.SaveConfiguration("c:\\temp\\" + AppDomain.CurrentDomain.FriendlyName + ".Config.Xml" );
		}
	}
}
