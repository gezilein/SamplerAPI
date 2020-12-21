using System.Collections.Generic;

namespace SampleAPI.Models
{
	public class ExperimentRequest
	{
		public int AllowedPlates { get; set; }

		public IList<Experiment> Experiments { get; set; }

		public int TraySize { get; set; }
	}
}