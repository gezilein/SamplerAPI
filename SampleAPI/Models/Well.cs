using System.Text.Json.Serialization;

namespace SampleAPI.Models
{
	public class Well
	{
		public int Column { get; set; }

		public string Reagent { get; set; }

		public int Row { get; set; }

		public string Sample { get; set; }

		[JsonIgnore]
		internal string GroupingKey { get; set; }

		//internal string[] Experiment { get; set; }

		[JsonIgnore]
		internal int GroupingPriority { get; set; }
	}
}