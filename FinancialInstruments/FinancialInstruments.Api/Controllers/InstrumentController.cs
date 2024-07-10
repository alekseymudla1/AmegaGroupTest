using FinancialInstruments.Domain.Interfaces;
using FinancialInstruments.Domain.Models;
using FinancialInstruments.Integration.REST;
using FinancialInstruments.Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinancialInstruments.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class InstrumentController : ControllerBase
	{
		private readonly IInstrumentsService _instrumentService;
		public InstrumentController(IInstrumentsService instrumentService)
		{
			_instrumentService = instrumentService;
		}

		[HttpGet]
		public Instruments GetAll()
		{
			var instrumets = new Instruments();
			return instrumets;
		}

		[HttpGet("{ticker}")]
		public async Task<Instrument> GetPrice(string ticker)
		{
			return await _instrumentService.GetInstrumentAsync(ticker);
		}
	}
}
