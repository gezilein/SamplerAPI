using System.Collections.Generic;

namespace SampleAPI.Models
{
	public class Tray
	{
		public IEnumerable<Well> Columns { get; set; }
		public IEnumerable<Well> Rows { get; set; }
	}
}