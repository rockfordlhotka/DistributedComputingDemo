using System;
using System.Collections.Generic;

namespace KeyWatcher.Orleans.Grains
{
	[Serializable]
	public class UserGrainState
	{
		public UserGrainState()
		{
			this.BadWords = new List<string>();
		}

		public List<string> BadWords { get; set; }
	}
}
