using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleAPI.Domain;
using SampleAPI.Models;
using System;
using System.Linq;

namespace SampleAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ExperimentController : ControllerBase
	{
		private readonly ILogger<ExperimentController> _logger;

		public ExperimentController(ILogger<ExperimentController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		public ExperimentResult Generate(ExperimentRequest request)
		{
			_logger.LogInformation("Incoming experimet request: {0}", request);

			var result = new ExperimentResult();

			if (request.TraySize != (int)TraySizes.Small && request.TraySize != (int)TraySizes.Large)
			{
				result.Message = "Invalid tray size";
				return result;
			}

			try
			{
				var wells = request.Experiments.SelectMany(e => e.GetWells());
				if (wells.Count() > request.TraySize * request.AllowedPlates)
				{
					result.Message = "Unable to fulfill experiment (too few plates)";
					return result;
				}

				wells = wells.OrderByDescending(w => w.GroupingPriority); //sort is important to have similar together
				var groups = wells.GroupBy(w => w.GroupingKey); //group by most numerous (set internally in preparation part)

				result.Trays = new Tray[request.AllowedPlates];
				for (var trayIndex = 0; trayIndex < result.Trays.Length; trayIndex++)
				{
					result.Trays[trayIndex] = new Tray { TraySize = request.TraySize };

					var rowIndex = 0;
					foreach (var group in groups)
					{
						var columnIndex = 0;
						var groupingKey = group.Key;
						foreach (var well in group)
						{
							well.Row = rowIndex;
							well.Column = columnIndex;
							result.Trays[trayIndex].Wells.Add(well);

							columnIndex++;

							if (columnIndex % result.Trays[trayIndex].Columns == 0 //new row if current row is filled
								|| groupingKey != well.GroupingKey) //or sample changed
							{
								columnIndex = 0;
								rowIndex++;
								groupingKey = well.GroupingKey;
							}
						}

						if (result.Trays[trayIndex].Wells.Count >= result.Trays[trayIndex].TraySize) //new tray (should break if allowed trays is breached)
						{
							if (trayIndex++ == request.AllowedPlates)
							{
								throw new ArgumentOutOfRangeException("AllowedPlates", "Number of allowed plates has been breached");
							}
						}

						rowIndex++;
					}
				}

				result.Message = "OK";
			}
			catch (Exception ex)
			{
				result.Message = string.Concat("Error generating experiments: ", ex.Message);
				_logger.LogError(ex, "Error generating experiment");
			}

			return result;
		}
	}
}