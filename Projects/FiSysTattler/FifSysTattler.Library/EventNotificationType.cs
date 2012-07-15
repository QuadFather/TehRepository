using System;

namespace FifSysTattler.Library
{
	[Flags]
	public enum EventNotificationType
	{
		Created,
		Renamed,
		Deleted,
		Changed,
		Error
	}
}