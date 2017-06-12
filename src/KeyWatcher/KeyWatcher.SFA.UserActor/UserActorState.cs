using System.Collections.Generic;
using System.Runtime.Serialization;

namespace KeyWatcher.SFA.UserActor
{
	[DataContract]
	public class UserActorState
	{
		public UserActorState() =>
			this.BadWords = new List<string>();

		[DataMember]
		public List<string> BadWords { get; set; }
	}
}
