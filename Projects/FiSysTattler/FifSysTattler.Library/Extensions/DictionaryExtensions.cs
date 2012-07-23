using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifSysTattler.Library.Extensions
{
	public static class DictionaryExtensions
	{
		public static void SmartAdd<TKey,TValue>(this Dictionary<TKey,TValue> dictionary, TKey key, TValue value)
		{
			if (!dictionary.ContainsKey(key))
			{
				dictionary.Add(key, value);
				return;
			}

			dictionary[key] = value;
		}
	}
}
