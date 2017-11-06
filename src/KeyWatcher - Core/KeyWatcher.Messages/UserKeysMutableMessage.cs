using System.Runtime.Serialization;

namespace KeyWatcher.Messages
{
	[DataContract]
	public sealed class UserKeysMutableMessage
	{
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public char[] Keys { get; set; }
	}
}
