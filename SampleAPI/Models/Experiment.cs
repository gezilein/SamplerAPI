using System.Collections.Generic;

namespace SampleAPI.Models
{
	public class Experiment
	{
		public IEnumerable<string> Reagents { get; set; }

		public int Replicates { get; set; }

		public IEnumerable<string> Samples { get; set; }
	}
}