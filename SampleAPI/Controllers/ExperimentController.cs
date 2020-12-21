using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleAPI.Models;

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
			return new ExperimentResult();
		}
	}
}