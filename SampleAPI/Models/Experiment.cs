using System.Collections.Generic;

namespace SampleAPI.Models
{
	public class Experiment
	{
		public IList<string> Reagents { get; set; }

		public int Replicates { get; set; }

		public IList<string> Samples { get; set; }

		/// <summary>
		/// Create required wells from current experiment (derived from Samples and Reagents)
		/// </summary>
		/// <returns>IEnumerable of <see cref="Well"/> objects containing final experiment information</returns>
		internal IEnumerable<Well> GetWells()
		{
			var groupingMode = Samples.Count >= Reagents.Count; //to later set priority and grouping (blind grouping)
			var group1 = groupingMode ? Samples : Reagents;
			var group2 = groupingMode ? Reagents : Samples;

			for (var i = 0; i < Replicates; i++)
			{
				foreach (var part1 in group1) //no need to know which part it is - just grouping by most numerous
				{
					foreach (var part2 in group2)
					{
						yield return new Well
						{
							//Experiment = new string[2] { part1, part2 }, //blind grouping (most numerous is group key)
							Sample = groupingMode ? part1 : part2, //set information for result
							Reagent = groupingMode ? part2 : part1,
							GroupingPriority = group1.Count * Replicates, //priority for grouping together bigger groups (number of replicates needed)
							GroupingKey = part1,
						};
					}
				}
			}
		}
	}
}