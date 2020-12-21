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

			var result = new ExperimentResult()
			{
				Trays = new Tray[request.AllowedPlates]
			};

			if (request.TraySize != (int)TraySizes.Small && request.TraySize != (int)TraySizes.Large)
			{
				result.Message = "Invalid tray size";
				return result;
			}

			try
			{
				var wells = request.Experiments.SelectMany(e => e.GetWells()).OrderBy(w => w.Reagent);
				if (wells.Count() > request.TraySize * request.AllowedPlates)
				{
					result.Message = "Unable to fulfill experiment (too few plates)";
					return result;
				}

				var sampleGroups = wells.GroupBy(w => w.Sample).ToArray();
				for (var trayIndex = 0; trayIndex < result.Trays.Length; trayIndex++)
				{
					result.Trays[trayIndex] = new Tray { TraySize = request.TraySize };

					var rowIndex = 0;

					foreach (var sampleGroup in sampleGroups)
					{
						var columnIndex = 0;
						var currentSample = sampleGroup.Key;
						foreach (var reagent in sampleGroup)
						{
							reagent.Row = rowIndex;
							reagent.Column = columnIndex++;
							result.Trays[trayIndex].Wells.Add(reagent);

							if (
								columnIndex % result.Trays[trayIndex].Columns == 0 //new row if current row is filled
								|| currentSample != sampleGroup.Key) //or sample changed
							{
								columnIndex = 0;
								rowIndex++;
								currentSample = sampleGroup.Key;
							}
						}

						rowIndex++;

						if (result.Trays[trayIndex].Wells.Count >= result.Trays[trayIndex].TraySize) //new tray (should break if allowed trays is breached)
						{
							trayIndex++;
						}
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