using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SampleAPI.Models
{
	public class Experiment
	{
		public IList<string> Reagents { get; set; }

		public int Replicates { get; set; }

		public IList<string> Samples { get; set; }

		internal int GetRequiredWells()
		{
			return Replicates * Reagents.Count * Samples.Count;
		}

		internal IEnumerable<Well> GetWells()
		{
			for (var i = 0; i < Replicates; i++)
			{
				foreach (var reagent in Reagents)
				{
					foreach (var sample in Samples)
					{
						yield return new Well { Reagent = reagent, Sample = sample };
					}
				}
			}
		}
	}
}