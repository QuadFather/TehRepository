using System;
using System.Collections.Generic;
using System.Diagnostics;
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
			var outputFilePath = "c:\\temp\\" + AppDomain.CurrentDomain.FriendlyName + ".Config.Xml";
			
			Debug.WriteLine("Output Path:" + outputFilePath);

			config.SaveConfiguration(outputFilePath);

			var proc = new Process();
			proc.StartInfo = new ProcessStartInfo
				{
					//Arguments = "\"" + outputFilePath + "\"",
					FileName = outputFilePath
				};

			proc.Start();
		}
	}
}
