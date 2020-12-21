using System.Collections.Generic;

namespace SampleAPI.Models
{
	public class ExperimentRequest
	{
		public IEnumerable<Experiment> Experiments { get; set; }

		public int TraySize { get; set; }
	}
}