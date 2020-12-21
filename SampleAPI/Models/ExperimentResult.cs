using System.Collections.Generic;

namespace SampleAPI.Models
{
	public class ExperimentResult : BaseResult
	{
		public Tray[] Trays { get; set; }
	}
}